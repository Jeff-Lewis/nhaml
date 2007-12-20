using System.IO;
using System.Text.RegularExpressions;

namespace Mindscape.NHaml.Rules
{
  public class PartialMarkupRule : MarkupRule
  {
    private static readonly Regex _partialRegex
      = new Regex(@"^\s*([\w-]+)$", RegexOptions.Compiled | RegexOptions.Singleline);

    public override char Signifier
    {
      get { return '_'; }
    }

    public override void Process(CompilationContext compilationContext)
    {
      Render(compilationContext);
    }

    public override BlockClosingAction Render(CompilationContext compilationContext)
    {
      Match match = _partialRegex.Match(compilationContext.CurrentInputLine.NormalizedText);

      if (match.Success)
      {
        string templateDirectory
          = Path.GetDirectoryName(compilationContext.TemplatePath);

        string partialTemplatePath
          = Path.Combine(templateDirectory, '_' + match.Groups[1].Value + ".haml");

        compilationContext.MergeTemplate(partialTemplatePath);
      }
      else if (!string.IsNullOrEmpty(compilationContext.LayoutPath))
      {
        compilationContext.MergeTemplate(compilationContext.TemplatePath);
      }

      return null;
    }
  }
}