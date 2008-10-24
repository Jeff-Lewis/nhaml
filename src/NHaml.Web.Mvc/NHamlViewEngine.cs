using System;
using System.Data.Linq;
using System.IO;
using System.Linq.Expressions;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

using Microsoft.Web.Mvc;

using NHaml.Engine;

namespace NHaml.Web.Mvc
{
  [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
  [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
  public class NHamlViewEngine : ViewEngine<NHamlCompiledView, ControllerContext, INHamlView, ViewDataDictionary>,
    IViewEngine
  {
    public NHamlViewEngine()
    {
      TemplateCompiler.AddUsing("System.Web");
      TemplateCompiler.AddUsing("System.Web.Mvc");
      TemplateCompiler.AddUsing("System.Web.Routing");

      TemplateCompiler.AddUsing("NHaml.Web.Mvc");

      TemplateCompiler.ViewBaseType = typeof(NHamlView<>);

      TemplateCompiler.AddReference(typeof(UserControl).Assembly.Location);
      TemplateCompiler.AddReference(typeof(RouteValueDictionary).Assembly.Location);
      TemplateCompiler.AddReference(typeof(DataContext).Assembly.Location);
      TemplateCompiler.AddReference(typeof(LinkExtensions).Assembly.Location);
      TemplateCompiler.AddReference(typeof(Action).Assembly.Location);
      TemplateCompiler.AddReference(typeof(Expression).Assembly.Location);
      TemplateCompiler.AddReference(typeof(IView).Assembly.Location);

      foreach (var name in System.Reflection.Assembly.GetCallingAssembly().GetReferencedAssemblies())
      {
        TemplateCompiler.AddReference(System.Reflection.Assembly.Load(name).Location);
      }

      TemplateCompiler.LoadFromConfiguration();
    }

    #region IViewEngine Members

    public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName)
    {
      return new ViewEngineResult(FindAndCreatePartialView(partialViewName, controllerContext), this);
    }

    public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName)
    {
      return new ViewEngineResult(FindAndCreateView(viewName, masterName, controllerContext), this);
    }

    public void ReleaseView(ControllerContext controllerContext, IView view)
    {
    }

    #endregion

    protected override NHamlCompiledView CreateView(string viewName, string layoutName, ControllerContext context)
    {
      var templatePath = context.HttpContext.Request
        .MapPath("~/Views/" + GetViewKey(viewName, context) + ".haml");

      var masterPath = SelectLayout(layoutName, context);

      return new NHamlCompiledView(
        TemplateCompiler,
        templatePath,
        masterPath,
        context.Controller.ViewData);
    }

    protected override NHamlCompiledView CreatePartialView(string viewName, ControllerContext context)
    {
      var templatePath = context.HttpContext.Request
        .MapPath("~/Views/" + GetViewKey(viewName, context) + ".haml");

      return new NHamlCompiledView(
        TemplateCompiler,
        templatePath,
        null,
        context.Controller.ViewData);
    }

    protected override string GetViewKey(string viewName, ControllerContext context)
    {
      return GetControllerName(context) + "/" + viewName;
    }

    protected override ViewDataDictionary GetViewData(ControllerContext context)
    {
      return context.Controller.ViewData;
    }

    protected static string SelectLayout(string layoutName, RequestContext requestContext)
    {
      var layoutsFolder = requestContext.HttpContext.Request.MapPath("~/Views/Shared");

      var layoutPath = layoutsFolder + "\\" + layoutName + ".haml";

      if (File.Exists(layoutPath))
      {
        return layoutPath;
      }

      layoutPath = layoutsFolder + "\\" + GetControllerName(requestContext) + ".haml";

      if (File.Exists(layoutPath))
      {
        return layoutPath;
      }

      layoutPath = layoutsFolder + "\\application.haml";

      if (File.Exists(layoutPath))
      {
        return layoutPath;
      }

      return null;
    }

    private static string GetControllerName(RequestContext requestContext)
    {
      return (string)requestContext.RouteData.Values["controller"];
    }
  }
}