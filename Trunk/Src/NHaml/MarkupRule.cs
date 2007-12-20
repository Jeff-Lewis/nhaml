namespace Mindscape.NHaml
{
  public abstract class MarkupRule
  {
    public abstract char Signifier { get; }

    public abstract BlockClosingAction Render(CompilationContext compilationContext);

    public virtual void Process(CompilationContext compilationContext)
    {
      compilationContext.CloseBlocks();
      compilationContext.BlockClosingActions.Push(Render(compilationContext) ?? delegate
        {
        });
      compilationContext.MoveNext();
    }
  }
}