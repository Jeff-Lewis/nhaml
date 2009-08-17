using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using IronRuby;
using Microsoft.Scripting.Hosting;
using NHaml.Rules;

namespace NHaml.Compilers.IronRuby
{
    public class IronRubyTemplateCompiler : ITemplateCompiler
    {
        private static readonly List<string> MidBlockKeywords
          = new List<string> { "ELSE", "ELSIF", "RESCUE", "ENSURE", "WHEN" };

        private readonly ScriptEngine _scriptEngine = Ruby.CreateEngine();

        public TemplateClassBuilder CreateTemplateClassBuilder( string className, Type templateBaseType )
        {
            return new IronRubyTemplateClassBuilder( className, templateBaseType );
        }

        public TemplateFactory Compile(ViewSourceReader viewSourceReader, TemplateOptions options, TemplateClassBuilder builder)
        {
            var ruby = new StringBuilder();

            foreach (var reference in options.References)
            {
                ruby.AppendLine( string.Format("require '{0}'", reference) );
            }

            ruby.Append(builder.Build(options.Usings));

            var templateSource = ruby.ToString();

            Trace.WriteLine( templateSource );

            _scriptEngine.Execute( templateSource );

            return CreateTemplateFactory( _scriptEngine, builder.ClassName );
        }

        protected virtual IronRubyTemplateFactory CreateTemplateFactory( ScriptEngine scriptEngine, string className )
        {
            return new IronRubyTemplateFactory( _scriptEngine, className );
        }

        public BlockClosingAction RenderSilentEval(ViewSourceReader viewSourceReader, TemplateClassBuilder builder)
        {
            var code = viewSourceReader.CurrentInputLine.NormalizedText;

            builder.AppendSilentCode(code, false);

            if( viewSourceReader.IsBlock )
            {
                builder.BeginCodeBlock();

                if( !viewSourceReader.CurrentInputLine.NormalizedText.Trim().Split( ' ' )[0].ToUpperInvariant().Equals( "CASE" ) )
                {
                    return () =>
                      {
                          if( (viewSourceReader.CurrentInputLine.Text.TrimStart().StartsWith(SilentEvalMarkupRule.SignifierChar)) &&
                            MidBlockKeywords.Contains( viewSourceReader.CurrentInputLine.NormalizedText.Trim().Split( ' ' )[0].ToUpperInvariant() ) )
                          {
                              builder.Unindent();
                          }
                          else
                          {
                              builder.EndCodeBlock();
                          }
                      };
                }
            }

            return MarkupRule.EmptyClosingAction;
        }

    }
}