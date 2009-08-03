using Castle.MonoRail.Framework;

namespace NHaml.Web.MonoRail.Tests.Components
{
    public class RenderSectionComponent : ViewComponent
    {
        public override void Render()
        {
            Context.RenderSection("Section1");
            Context.RenderSection("Section2");
        }
    }
}