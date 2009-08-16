using System.Collections.Generic;
using NHaml.Compilers;
using NHaml.Rules;
using NHaml.TemplateResolution;

namespace NHaml
{
    public delegate void BlockClosingAction();

    public sealed class TemplateParser : IViewSourceReader
    {
        private readonly string _singleIndent;

        public TemplateParser(TemplateOptions options, TemplateClassBuilder templateClassBuilder, IList<IViewSource> viewSources)
        {
            BlockClosingActions = new Stack<BlockClosingAction>();
            Options = options;
            Builder = templateClassBuilder;
            ViewSources = viewSources;
            ViewSourceQueue = new Queue<IViewSource>();
            ViewSourceModifiedChecks = new List<Func<bool>>();
            for (var i = 0; i < viewSources.Count; i++)
            {
                var viewSource = viewSources[i];
                ViewSourceModifiedChecks.Add(() => viewSource.IsModified);
                ViewSourceQueue.Enqueue(viewSource);
            }


            if (Options.UseTabs)
            {
                _singleIndent = "\t";
            }
            else
            {
                _singleIndent = string.Empty.PadLeft(Options.IndentSize);
            }
        }

        public IList<Func<bool>> ViewSourceModifiedChecks { get; set; }

        public IList<IViewSource> ViewSources { get; set; }

        public Queue<IViewSource> ViewSourceQueue { get; set; }


        public TemplateOptions Options { get; private set; }

        public TemplateClassBuilder Builder { get; private set; }

        private LinkedList<InputLine> InputLines { get; set; }

        private LinkedListNode<InputLine> CurrentNode { get; set; }

   
        public InputLine CurrentInputLine
        {
            get { return CurrentNode.Value; }
        }


        public InputLine NextInputLine
        {
            get { return CurrentNode.Next.Value; }
        }

        public Stack<BlockClosingAction> BlockClosingActions { get; private set; }

        public bool IsBlock
        {
            get { return NextInputLine.IndentCount > CurrentInputLine.IndentCount; }
        }

        public string NextIndent
        {
            get { return CurrentInputLine.Indent + _singleIndent; }
        }


        public void Parse()
        {
            InputLines = new LinkedList<InputLine>();
            InputLines.AddLast(new InputLine(string.Empty, 0, Options.IndentSize));
            InputLines.AddLast(new InputLine(EofMarkupRule.SignifierChar, 1, Options.IndentSize));
            CurrentNode = InputLines.First.Next;
            ProcessViewSource(ViewSourceQueue.Dequeue());
        }

        private void ProcessViewSource(IViewSource viewSource)
        {
            MergeTemplate(viewSource, false);

            while (CurrentNode.Next != null)
            {
                while (CurrentInputLine.IsMultiline && NextInputLine.IsMultiline)
                {
                    CurrentInputLine.Merge(NextInputLine);
                    InputLines.Remove(CurrentNode.Next);
                }

                if (CurrentInputLine.IsMultiline)
                {
                    CurrentInputLine.TrimEnd();
                }

                CurrentInputLine.ValidateIndentation();

                var rule = Options.GetRule(CurrentInputLine);
                CurrentInputLine.NormalizedText = GetNormalizedText(rule, CurrentInputLine);
                if (rule.PerformCloseActions)
                {
                    CloseBlocks();
                    BlockClosingActions.Push(rule.Render(this, Options, Builder));
                    MoveNext();
                }
                else
                {
                   rule.Render(this, Options, Builder);
                }

            }

            CloseBlocks();
        }


        private static string GetNormalizedText(MarkupRule markupRule, InputLine inputLine)
        {
            var length = markupRule.Signifier.Length;
            var text = inputLine.Text;
            text = text.TrimStart();
            return text.Substring(length, text.Length - length);
        }

        public void MoveNext()
        {
            CurrentNode = CurrentNode.Next;
        }

        public void MergeTemplate(IViewSource viewSource, bool replaceCurrentNode)
        {

            var previous = CurrentNode.Previous;

            var lineNumber = 0;

            using (var reader = viewSource.GetStreamReader())
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length == 0)
                    {
                        continue;
                    }
                    var inputLine = new InputLine(CurrentNode.Value.Indent + line, lineNumber++, Options.IndentSize);
                    InputLines.AddBefore(CurrentNode, inputLine);
                }
            }
            if (replaceCurrentNode)
            {
                InputLines.Remove(CurrentNode);

            }
            CurrentNode = previous.Next;

        }

        public void CloseBlocks()
        {
            var currentIndentCount = CurrentInputLine.IndentCount;
            var previousIndentCount = CurrentNode.Previous.Value.IndentCount;
            for (var index = 0; (index <= previousIndentCount - currentIndentCount) && (BlockClosingActions.Count > 0); index++)
            {
                var blockClosingAction = BlockClosingActions.Pop();
                blockClosingAction();
            }
        }
    }

}