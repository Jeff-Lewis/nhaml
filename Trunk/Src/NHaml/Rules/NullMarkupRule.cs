using System.Diagnostics.CodeAnalysis;

namespace Mindscape.NHaml.Rules
{
  public class NullMarkupRule : MarkupRule
  {
    [SuppressMessage("Microsoft.Security", "CA2104")]
    public static readonly NullMarkupRule Instance = new NullMarkupRule();

    private NullMarkupRule()
    {
    }

    public override char Signifier
    {
      get { return new char(); }
    }

    public override BlockClosingAction Render(CompilationContext compilationContext)
    {
      compilationContext.ViewBuilder.AppendOutputLine(compilationContext.CurrentInputLine.Text);

      return null;
    }
  }
}