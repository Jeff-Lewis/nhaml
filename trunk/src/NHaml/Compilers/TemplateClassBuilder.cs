using System;
using System.Collections.Generic;
using System.Text;

namespace NHaml.Compilers
{
    public abstract class TemplateClassBuilder
    {

		public const string DefaultTextWriterVariableName = "textWriter";
        protected TemplateClassBuilder(string className, Type baseType)
        {

			CurrentTextWriterVariableName = DefaultTextWriterVariableName; 
            Output = new StringBuilder();
            Preamble = new StringBuilder();
            ClassName = className;
            BaseType = baseType;
        }


		public string CurrentTextWriterVariableName { get; set; }
        public Type BaseType { get; set; }

        protected StringBuilder Output { get; private set; }

        protected StringBuilder Preamble { get; private set; }

        public int Depth { get; set; }
        public int BlockDepth { get; set; }

        public string ClassName { get; private set; }

        public abstract void AppendOutput(string value, bool newLine);

        public virtual void AppendOutput(string value)
        {
            AppendOutput(value, false);
        }

        public virtual void AppendCodeLine(string code, bool escapeHtml)
        {
            AppendCode(code, true, escapeHtml);
        }

        public virtual void AppendOutputLine(string value)
        {
            AppendOutput(value, true);
        }

        public virtual void AppendCode(string code)
        {
            AppendCode(code, false, false);
        }

        public virtual void BeginCodeBlock()
        {
            Indent();
        }

        public virtual void EndCodeBlock()
        {
            Unindent();
        }

        public void Unindent()
        {
            Depth--;
        }
        public void Indent()
        {
            Depth++;
        }

        public abstract void AppendCode(string code, bool newLine, bool escapeHtml);
        public abstract void AppendSilentCode(string code, bool closeStatement);
        public abstract void AppendPreambleCode(string code);
        public abstract void AppendChangeOutputDepth(int depth);

        public abstract string Build(IList<string> imports);
        public abstract void AppendAttributeTokens( string schema, string name, IList<ExpressionStringToken> values );
        public abstract void AppendHamlComment(string text);
    }
}