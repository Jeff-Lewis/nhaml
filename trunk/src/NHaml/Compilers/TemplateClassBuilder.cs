using System.Text;

namespace NHaml.Compilers
{
    public abstract class TemplateClassBuilder
    {
        private readonly string _className;

        private readonly StringBuilder _output = new StringBuilder();
        private readonly StringBuilder _preamble = new StringBuilder();

        protected TemplateClassBuilder( string className )
        {
            _className = className;
        }

        protected StringBuilder Output
        {
            get { return _output; }
        }

        protected StringBuilder Preamble
        {
            get { return _preamble; }
        }

        public int Depth { get; set; }
        public int BlockDepth { get; set; }

        public string ClassName
        {
            get { return _className; }
        }

        public abstract void AppendOutput( string value, bool newLine );

        public virtual void AppendOutput( string value )
        {
            AppendOutput( value, false );
        }

        public virtual void AppendCodeLine( string code, bool escapeHtml )
        {
            AppendCode( code, true, escapeHtml );
        }

        public virtual void AppendOutputLine( string value )
        {
            AppendOutput( value, true );
        }

        public virtual void AppendCode( string code )
        {
            AppendCode( code, false, false );
        }

        public virtual void BeginCodeBlock()
        {
            Depth++;
        }

        public virtual void EndCodeBlock()
        {
            Unindent();
        }

        public virtual void Unindent()
        {
            Depth--;
        }

        public abstract void AppendCode( string code, bool newLine, bool escapeHtml );
        public abstract void AppendSilentCode( string code, bool closeStatement );
        public abstract void AppendPreambleCode( string code );
        public abstract void AppendAttributeCode( string name, string code );
        public abstract void AppendChangeOutputDepth( int depth );

        public abstract string Build();
    }
}