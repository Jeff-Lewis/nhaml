namespace Mindscape.NHaml.Rules
{
  public class SilentEvalMarkupRule : MarkupRule
  {
    public override char Signifier
    {
      get { return '-'; }
    }

    public override BlockClosingAction Render(CompilationContext compilationContext)
    {
      bool isBlock = compilationContext.NextInputLine.IndentSize > compilationContext.CurrentInputLine.IndentSize;

      compilationContext.ViewBuilder.AppendSilentCode(compilationContext.CurrentInputLine.NormalizedText, !isBlock);

      if (isBlock)
      {
        compilationContext.ViewBuilder.BeginCodeBlock();

        return delegate
          {
            compilationContext.ViewBuilder.EndCodeBlock();
          };
      }

      return null;
    }
  }
}