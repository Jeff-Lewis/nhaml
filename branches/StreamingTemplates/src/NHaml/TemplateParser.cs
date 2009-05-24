using System.Collections.Generic;
using NHaml.Compilers;
using NHaml.Rules;

namespace NHaml
{
    public delegate void BlockClosingAction();

    public sealed class TemplateParser
    {
        private readonly string _singleIndent;

        public TemplateParser( 
            TemplateEngine templateEngine, TemplateClassBuilder templateClassBuilder,
            IList<ITemplateContentProvider> layoutProviders, ITemplateContentProvider templateContentProvider)
        {
            BlockClosingActions = new Stack<BlockClosingAction>();
            InputFiles = new Dictionary<string, ITemplateContentProvider>();
            TemplateEngine = templateEngine;
            TemplateClassBuilder = templateClassBuilder;
            TemplateContentProvider = templateContentProvider;
            MergedTemplatePaths = new List<ITemplateContentProvider>();
            MergedTemplatePaths.AddRange(layoutProviders);
            MergedTemplatePaths.Add(templateContentProvider);

            Meta = new Dictionary<string, string>();

            foreach (var layoutPath in layoutProviders)
            {
                InputFiles.Add(layoutPath.Key, layoutPath);
            }
            InputFiles.Add(templateContentProvider.Key,templateContentProvider);

            if (TemplateEngine.UseTabs)
            {
                _singleIndent = "\t";
            }
            else
            {
                _singleIndent = string.Empty.PadLeft(TemplateEngine.IndentSize);
            }
        }

        public Dictionary<string, string> Meta { get; private set; }

        public TemplateEngine TemplateEngine { get; private set; }

        public TemplateClassBuilder TemplateClassBuilder { get; private set; }
        public ITemplateContentProvider TemplateContentProvider { get; private set; }

        public List<ITemplateContentProvider> MergedTemplatePaths { get; private set; }

        public Dictionary<string, ITemplateContentProvider> InputFiles { get; private set; }

        private LinkedList<InputLine> InputLines { get; set; }

        private LinkedListNode<InputLine> CurrentNode { get; set; }

        private LinkedListNode<InputLine> NextNode
        {
            get { return CurrentNode.Next; }
        }

        public InputLine CurrentInputLine
        {
            get { return CurrentNode.Value; }
        }

        private InputLine NextInputLine
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

        public int CurrentTemplateIndex { get; set; }

        public void Parse()
        {
            InputLines = new LinkedList<InputLine>();
            InputLines.AddLast(new InputLine(string.Empty, null, 0, TemplateEngine.IndentSize));

            InputLines.AddLast(new InputLine(EofMarkupRule.SignifierChar.ToString(), null, 1, TemplateEngine.IndentSize));

            CurrentNode = InputLines.First.Next;
            for (CurrentTemplateIndex = 0; CurrentTemplateIndex < MergedTemplatePaths.Count; CurrentTemplateIndex++)
            {
                var templatePath = MergedTemplatePaths[CurrentTemplateIndex];
                MergeTemplate(templatePath, false);

                while (CurrentNode.Next != null)
                {
                    while (CurrentInputLine.IsMultiline && NextInputLine.IsMultiline)
                    {
                        CurrentInputLine.Merge(NextInputLine);
                        InputLines.Remove(NextNode);
                    }

                    if (CurrentInputLine.IsMultiline)
                    {
                        CurrentInputLine.TrimEnd();
                    }

                    CurrentInputLine.ValidateIndentation();

                    TemplateEngine.GetRule(CurrentInputLine).Process(this);
                }

                CloseBlocks();
            }
        }

        public void MoveNext()
        {
            CurrentNode = CurrentNode.Next;
        }

        public void MergeTemplate( ITemplateContentProvider templatePath, bool replaceCurrentNode )
        {

            var previous = CurrentNode.Previous;

            var lineNumber = 0;

            using( var reader = templatePath.GetContentReader()  )
            {
                string line;

                while( (line = reader.ReadLine()) != null )
                {
                    InputLines.AddBefore( CurrentNode,
                      new InputLine( CurrentNode.Value.Indent + line, templatePath.Key, lineNumber++, TemplateEngine.IndentSize ) );
                }
            }
            if (replaceCurrentNode)
            {
                InputLines.Remove(CurrentNode);

            }
                CurrentNode = previous.Next;

                if (!InputFiles.ContainsKey(templatePath.Key))
                {
                    InputFiles.Add( templatePath.Key,templatePath );
                }
        }

        public void CloseBlocks()
        {
            for( var j = 0;
                 ((j <= CurrentNode.Previous.Value.IndentCount
                   - CurrentInputLine.IndentCount)
                     && (BlockClosingActions.Count > 0));
                 j++ )
            {
                BlockClosingActions.Pop()();
            }
        }

    }
}