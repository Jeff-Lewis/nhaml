using System;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI;

namespace NHaml.Web.Mvc
{
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class NHamlMvcViewEngine : VirtualPathProviderViewEngine
    {
        private readonly TemplateEngine _templateEngine = new TemplateEngine();

        public virtual string DefaultMaster { get; set; }

        public NHamlMvcViewEngine()
        {
            InitializeTemplateEngine();
            InitializeViewLocations();
        }

        private void InitializeTemplateEngine()
        {
            DefaultMaster = "application";

            _templateEngine.Options.AddUsing( "System.Web" );
            _templateEngine.Options.AddUsing( "System.Web.Mvc" );
            _templateEngine.Options.AddUsing( "System.Web.Mvc.Html" );
            _templateEngine.Options.AddUsing( "System.Web.Routing" );

            _templateEngine.Options.AddUsing( "NHaml.Web.Mvc" );

            _templateEngine.Options.AddReference( typeof( UserControl ).Assembly.Location );
            _templateEngine.Options.AddReference( typeof( RouteValueDictionary ).Assembly.Location );
            _templateEngine.Options.AddReference( typeof( DataContext ).Assembly.Location );
            _templateEngine.Options.AddReference( typeof( LinkExtensions ).Assembly.Location );
            _templateEngine.Options.AddReference( typeof( Action ).Assembly.Location );
            _templateEngine.Options.AddReference( typeof( Expression ).Assembly.Location );
            _templateEngine.Options.AddReference( typeof( IView ).Assembly.Location );
            _templateEngine.Options.AddReference( typeof( NHamlMvcView<> ).Assembly.Location );
        }

        protected TemplateEngine TemplateEngine
        {
            get { return _templateEngine; }
        }

        private void InitializeViewLocations()
        {
            ViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.haml",
                "~/Views/Shared/{0}.haml"
            };

            MasterLocationFormats = new[]
            {
                "~/Views/Shared/{0}.haml"
            };

            PartialViewLocationFormats = ViewLocationFormats;
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (string.IsNullOrEmpty(masterName))
            {
                var controllerName = controllerContext.RouteData.GetRequiredString("controller");
                var result = base.FindView(controllerContext, viewName, controllerName, useCache);

                if (result.View == null)
                {
                    result = base.FindView(controllerContext, viewName, DefaultMaster, useCache);
                }

                return result.View == null ? base.FindPartialView(controllerContext, viewName, useCache) : result;
            }

            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return (IView)_templateEngine.Compile(
                VirtualPathToPhysicalPath(controllerContext.RequestContext, partialPath),
                GetViewBaseType(controllerContext)).CreateInstance();
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return (IView)_templateEngine.Compile(
                VirtualPathToPhysicalPath(controllerContext.RequestContext, viewPath),
                VirtualPathToPhysicalPath(controllerContext.RequestContext, masterPath),
                GetViewBaseType(controllerContext)).CreateInstance();
        }

        protected virtual Type ViewGenericBaseType
        {
            get { return typeof(NHamlMvcView<>); }
        }

        protected virtual Type GetViewBaseType(ControllerContext controllerContext)
        {
            var modelType = typeof(object);

            var viewData = controllerContext.Controller.ViewData;

            var viewContext = controllerContext as ViewContext;

            if ((viewContext != null) && (viewContext.ViewData != null))
            {
                viewData = viewContext.ViewData;
            }

            if ((viewData != null) && (viewData.Model != null))
            {
                modelType = viewData.Model.GetType();
            }

            return ViewGenericBaseType.MakeGenericType(modelType);
        }

        protected virtual string VirtualPathToPhysicalPath(RequestContext context, string path)
        {
            return context.HttpContext.Request.MapPath(path);
        }
    }
}