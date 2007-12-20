using System.Web.Mvc;

namespace Mindscape.NHaml.Tests.TestApp.Controllers
{
  public class HomeController : Controller
  {
    [ControllerAction]
    public void Index()
    {
      RenderView("Index");
    }

    [ControllerAction]
    public void About()
    {
      RenderView("About");
    }
  }
}