using System;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

namespace Mindscape.NHaml.ViewEngine
{
  [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
  [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
  public sealed class NHamlControllerFactory : IControllerFactory
  {
    public IController CreateController(RequestContext context, Type controllerType)
    {
      Controller controller = (Controller)Activator.CreateInstance(controllerType);

      controller.ViewFactory = new NHamlViewFactory();

      return controller;
    }
  }
}