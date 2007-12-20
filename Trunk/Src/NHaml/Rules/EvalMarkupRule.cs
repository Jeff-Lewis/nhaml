namespace Mindscape.NHaml.Rules
{
  public class EvalMarkupRule : MarkupRule
  {
    public override char Signifier
    {
      get { return '='; }
    }

    public override BlockClosingAction Render(CompilationContext compilationContext)
    {
      compilationContext.ViewBuilder.AppendOutput(compilationContext.CurrentInputLine.Indent);
      compilationContext.ViewBuilder.AppendCodeLine(compilationContext.CurrentInputLine.NormalizedText.Trim());

      return null;
    }
  }
}