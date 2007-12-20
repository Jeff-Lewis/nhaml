using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Mindscape.NHaml.Exceptions;
using Mindscape.NHaml.Properties;

namespace Mindscape.NHaml.Rules
{
  public class TagMarkupRule : MarkupRule
  {
    private const string Id = "id";
    private const string Class = "class";

    private static readonly Regex _tagRegex = new Regex(
      @"^([-:\w]+)([-\w\.\#]*)\s*(\{(.*)\})?([\/=]?)(.*)$",
      RegexOptions.Compiled | RegexOptions.Singleline);

    private static readonly Regex _idClassesRegex = new Regex(
      @"(?:(?:\#([-\w]+))|(?:\.([-\w]+)))+",
      RegexOptions.Compiled | RegexOptions.Singleline);

    protected virtual string PreprocessLine(InputLine inputLine)
    {
      return inputLine.NormalizedText;
    }

    public override char Signifier
    {
      get { return '%'; }
    }

    public override BlockClosingAction Render(CompilationContext compilationContext)
    {
      Match match = _tagRegex.Match(PreprocessLine(compilationContext.CurrentInputLine));

      if (!match.Success)
      {
        SyntaxException.Throw(compilationContext.CurrentInputLine, Resources.ErrorParsingTag, compilationContext.CurrentInputLine);
      }

      string openingTag = compilationContext.CurrentInputLine.Indent + '<' + match.Groups[1].Value;
      string closingTag = "</" + match.Groups[1].Value + '>';

      compilationContext.ViewBuilder.AppendOutput(openingTag);

      ParseAndRenderAttributes(compilationContext, match);

      string action = match.Groups[5].Value;

      if (string.Equals("/", action)
        || compilationContext.TemplateCompiler.IsAutoClosing(match.Groups[1].Value))
      {
        compilationContext.ViewBuilder.AppendOutputLine(" />");

        return null;
      }
      else
      {
        string content = match.Groups[6].Value.Trim();

        if (string.IsNullOrEmpty(content))
        {
          compilationContext.ViewBuilder.AppendOutputLine(">");
          closingTag = compilationContext.CurrentInputLine.Indent + closingTag;
        }
        else
        {
          if ((content.Length > 50) || string.Equals("=", action))
          {
            compilationContext.ViewBuilder.AppendOutputLine(">");
            compilationContext.ViewBuilder.AppendOutput(compilationContext.CurrentInputLine.Indent + "  ");

            if (string.Equals("=", action))
            {
              compilationContext.ViewBuilder.AppendCodeLine(content);
            }
            else
            {
              compilationContext.ViewBuilder.AppendOutputLine(content);
            }

            closingTag = compilationContext.CurrentInputLine.Indent + closingTag;
          }
          else
          {
            compilationContext.ViewBuilder.AppendOutput(">" + content);
          }
        }

        return delegate
          {
            compilationContext.ViewBuilder.AppendOutputLine(closingTag);
          };
      }
    }

    private static void ParseAndRenderAttributes(CompilationContext compilationContext, Match tagMatch)
    {
      string idAndClasses = tagMatch.Groups[2].Value;
      string attributesHash = tagMatch.Groups[4].Value.Trim();

      Match match = _idClassesRegex.Match(idAndClasses);

      List<string> classes = new List<string>();

      foreach (Capture capture in match.Groups[2].Captures)
      {
        classes.Add(capture.Value);
      }

      if (classes.Count > 0)
      {
        attributesHash = PrependAttribute(attributesHash,
          '@' + Class, string.Join(" ", classes.ToArray()));
      }

      string id = null;

      foreach (Capture capture in match.Groups[1].Captures)
      {
        id = capture.Value;
      }

      if (!string.IsNullOrEmpty(id))
      {
        attributesHash = PrependAttribute(attributesHash, Id, id);
      }

      if (!string.IsNullOrEmpty(attributesHash))
      {
        compilationContext.ViewBuilder.AppendOutput(" ");

        string attributes = TryPrecompileAttributes(compilationContext.TemplateCompiler, attributesHash);

        if (!string.IsNullOrEmpty(attributes))
        {
          compilationContext.ViewBuilder.AppendOutput(attributes);
        }
        else
        {
          compilationContext.ViewBuilder.AppendCode("new {" + attributesHash + "}.RenderAttributes()");
        }
      }
    }

    private static string TryPrecompileAttributes(TemplateCompiler templateCompiler, string attributesHash)
    {
      string source = "public class AttributeEvaluator:IAttributeEvaluator{ public string Eval(){return new {"
        + attributesHash + "}.RenderAttributes();}}";

      Type type = new TypeBuilder(templateCompiler).Build(source, "AttributeEvaluator");

      string attributes = null;

      if (type != null)
      {
        attributes = ((IAttributeEvaluator)Activator.CreateInstance(type)).Eval();
      }

      return attributes;
    }

    private static string PrependAttribute(string attributesHash, string name, string value)
    {
      string attribute = name + "=\"" + value + "\"";

      return string.IsNullOrEmpty(attributesHash) ? attribute : attribute + "," + attributesHash;
    }
  }
}