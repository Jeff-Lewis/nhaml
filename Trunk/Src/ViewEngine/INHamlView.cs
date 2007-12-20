using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

namespace Mindscape.NHaml.ViewEngine
{
  [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
  [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
  public interface INHamlView : IView
  {
    void SetViewData(object viewData);
  }
}