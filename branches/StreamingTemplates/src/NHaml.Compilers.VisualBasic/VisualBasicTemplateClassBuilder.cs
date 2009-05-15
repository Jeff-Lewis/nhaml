using System;
using System.Collections.Generic;
using System.Text;
using NHaml.Utils;

namespace NHaml.Compilers.VisualBasic
{
    internal sealed class VisualBasicTemplateClassBuilder : TemplateClassBuilder
    {
        public VisualBasicTemplateClassBuilder(string className, Type baseType)
            : base(className, baseType)
        {
        }

        public override void AppendOutput(string value, bool newLine)
        {
            if (value == null)
            {
                return;
            }

            var method = newLine ? "WriteLine" : "Write";

            value = value.Replace("\"", "\"\"");

            if (Depth > 0)
            {
                if (value.StartsWith(string.Empty.PadLeft(Depth*2), StringComparison.Ordinal))
                {
                    value = value.Remove(0, Depth*2);
                }
            }

            Output.AppendFormat("textWriter.{0}(\"{1}\")\r\n", method, value);
        }

        public override void AppendCode(string code, bool newLine, bool escapeHtml)
        {
            if (code != null)
            {
                if (code.StartsWith("'") && code.EndsWith("'") && code.Length == 3)
                {
                    code = string.Format("(Convert.ToString(\"{0}\"c))", code);
                }
                else
                {
                    code = string.Format("(Convert.ToString({0}))", code);
                }


                code = string.Format("(Convert.ToString({0}))", code);

                if (escapeHtml)
                {
                    code = string.Format("(HttpUtility.HtmlEncode{0})", code);
                }

                Output.AppendLine(string.Format("textWriter.{0}{1}", (newLine ? "WriteLine" : "Write"), code));
            }
        }

        public override void AppendChangeOutputDepth(int depth)
        {
            if (BlockDepth != depth)
            {
                Output.AppendLine("Output.Depth = " + depth);

                BlockDepth = depth;
            }
        }

        public override void AppendSilentCode(string code, bool closeStatement)
        {
            if (code != null)
            {
                code = code.Trim();

                //if( closeStatement && !code.EndsWith( ";", StringComparison.Ordinal ) )
                //{
                //    code += ';';
                //}

                Output.AppendLine(code);
            }
        }

        public override void AppendAttributeTokens(string schema, string name, IEnumerable<ExpressionStringToken> values)
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
                    code.AppendFormat("\"{0}\" + ", item.Value.Replace("\\\"", "\"\""));
                }
            }

            if (code.Length > 3)
            {
                code.Remove(code.Length - 3, 3);
            }

            var format = string.Format("RenderAttributeIfValueNotNull(textWriter, \"{0}\", \"{1}\",{2})", schema, name,
                                       code);
            AppendSilentCode(format, true);
        }

        public override void BeginCodeBlock()
        {
            Depth++;
            //  Output.AppendLine( "{" );
        }

        public override void EndCodeBlock()
        {
            //  Output.AppendLine( string.Empty);
            Depth--;
        }

        public override void AppendPreambleCode(string code)
        {
            Preamble.AppendLine(code);
        }

        public override string Build()
        {
            Output.AppendLine("End Sub");
            Output.Append("End Class");

            var result = new StringBuilder();

            result.AppendLine(Utility.FormatInvariant("public Class {0} \r\nInherits {1} ",
                                                      ClassName, Utility.MakeBaseClassName(BaseType, "(Of ", ")", ".")));
            result.AppendLine("Protected Overrides Sub CoreRender(ByVal textWriter As TextWriter)");

            result.Append(Preamble);
            result.Append(Output);

            return result.ToString();
        }
    }
}