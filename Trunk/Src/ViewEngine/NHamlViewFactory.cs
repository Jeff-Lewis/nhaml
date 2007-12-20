using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

using Mindscape.NHaml.ViewEngine.Configuration;

namespace Mindscape.NHaml.ViewEngine
{
  [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
  [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
  public sealed class NHamlViewFactory : IViewFactory
  {
    private static readonly Dictionary<string, CompiledView> _viewCache
      = new Dictionary<string, CompiledView>();

    private static readonly TemplateCompiler _templateCompiler
      = new TemplateCompiler();

    private static bool _production;

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static NHamlViewFactory()
    {
      _templateCompiler.AddUsing("System.Web");
      _templateCompiler.AddUsing("System.Web.Mvc");
      _templateCompiler.AddUsing("Mindscape.NHaml.ViewEngine");

      _templateCompiler.ViewBaseType = typeof(NHamlView<>);

      _templateCompiler.AddReference(typeof(IView).Assembly.Location);
      _templateCompiler.AddReference(typeof(DataContext).Assembly.Location);
      _templateCompiler.AddReference(typeof(TextInputExtensions).Assembly.Location);

      LoadConfiguration();
    }

    private static void LoadConfiguration()
    {
      NHamlViewEngineSection section = NHamlViewEngineSection.Read();

      if (section != null)
      {
        _production = section.Production;
      }
    }

    public IView CreateView(ControllerContext controllerContext, string viewName,
      string masterName, object viewData)
    {
      string controller = (string)controllerContext.RouteData.Values["controller"];
      string viewKey = controller + "/" + viewName;

      CompiledView compiledView;

      if (!_viewCache.TryGetValue(viewKey, out compiledView))
      {
        lock (_viewCache)
        {
          if (!_viewCache.TryGetValue(viewKey, out compiledView))
          {
            string templatePath = controllerContext.HttpContext.Request
              .MapPath("~/Views/" + viewKey + ".haml");

            string layoutPath = FindLayout(controllerContext.HttpContext.Request
              .MapPath("~/Views/Shared"), masterName, controller);

            compiledView = new CompiledView(_templateCompiler, templatePath, layoutPath, viewData);

            _viewCache.Add(viewKey, compiledView);
          }
        }
      }

      if (!_production)
      {
        compiledView.RecompileIfNecessary(viewData);
      }

      INHamlView view = compiledView.CreateView();

      if (ViewDataIsDictionary(viewData))
      {
        viewData = new ViewData(viewData);
      }

      view.SetViewData(viewData);

      return view;
    }

    public static bool ViewDataIsDictionary(object viewData)
    {
      return (viewData != null)
        && (typeof(IDictionary).IsAssignableFrom(viewData.GetType()));
    }

    private static string FindLayout(string layoutsFolder, string masterName, string controller)
    {
      string layoutPath = layoutsFolder + "\\" + masterName + ".haml";

      if (File.Exists(layoutPath))
      {
        return layoutPath;
      }

      layoutPath = layoutsFolder + "\\" + controller + ".haml";

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
  }
}