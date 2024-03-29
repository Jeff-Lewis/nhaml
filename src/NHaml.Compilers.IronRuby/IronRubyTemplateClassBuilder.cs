using System;
using System.Collections.Generic;
using System.Text;
using NHaml.Utils;

namespace NHaml.Compilers.IronRuby
{
    internal  class IronRubyTemplateClassBuilder : TemplateClassBuilder
    {
        public IronRubyTemplateClassBuilder(string className)
            : base(className)
        {
        }

        public override void AppendAttributeTokens(string schema, string name, IList<ExpressionStringToken> values)
        {
            var code = new StringBuilder();
            foreach (var item in values)
            {
                if (item.IsExpression)
                {
                    code.AppendFormat("({0}) + ", item.Value);
                }
                else
                {
                    code.AppendFormat("\"{0}\" + ", item.Value.Replace("\"", "\\\""));
                }
            }

            if (code.Length > 3)
            {
                code.Remove(code.Length - 3, 3);
            }

            var format = string.Format("render_attribute_if_value_not_null(text_writer, \"{0}\", \"{1}\", {2})", schema,
                                       name, code);

            AppendSilentCode(format, true);
        }


        public override void AppendPreambleCode(string code)
        {
            Preamble.AppendLine(code);
        }

        public override void AppendOutput(string value)
        {
            if (value == null)
            {
                return;
            }


            if (Depth > 0)
            {
                if (value.StartsWith(string.Empty.PadLeft(Depth*2), StringComparison.Ordinal))
                {
                    value = value.Remove(0, Depth*2);
                }
            }

            Output.AppendLine(string.Format("text_writer.Write('{0}')", value.Replace("'", "\\'")));
        }

        public override void AppendOutputLine()
        {
            Output.AppendLine("text_writer.WriteLine()");
        }

        public override void AppendCode(string code, bool escapeHtml)
        {
            if (code != null)
            {
                if (escapeHtml)
                {
                    code = string.Format("System::Web::HttpUtility.HtmlEncode({0})", code);
                }
                else
                {
                    code += ".ToString()";
                }

                Output.AppendLine(string.Format("text_writer.Write({0})", code));
            }
        }

        public override void AppendChangeOutputDepth(int depth)
        {
        }

        public override void AppendSilentCode(string code, bool closeStatement)
        {
            if (code != null)
            {
                code = code.Trim();

                if (closeStatement && !code.EndsWith(";", StringComparison.Ordinal))
                {
                    code += ';';
                }

                Output.AppendLine(code);
            }
        }

        public override void EndCodeBlock()
        {
            Output.AppendLine("end");

            base.EndCodeBlock();
        }

        public override string Build(IList<string> imports)
        {
            Output.AppendLine("end;end");

            var result = new StringBuilder();

            var baseClassName = Utility.MakeBaseClassName(BaseType, "[", "]", "::");
            result.AppendLine(string.Format("class {0}<{1}", ClassName, baseClassName));
            result.AppendLine("def core_render(text_writer)");

            result.Append(Preamble);

            result.Append(Output);

            return result.ToString();
        }
    }
}