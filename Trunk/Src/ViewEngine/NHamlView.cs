using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

namespace Mindscape.NHaml.ViewEngine
{
  [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
  [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
  public abstract class NHamlView<TViewData> : INHamlView
  {
    private readonly ICompiledView _compiledView;

    private ViewContext _viewContext;
    private AjaxHelper _ajax;
    private HtmlHelper _html;
    private UrlHelper _url;

    private TViewData _viewData;

    protected NHamlView()
    {
      _compiledView = (ICompiledView)this;
    }

    public void RenderView(ViewContext viewContext)
    {
      _viewContext = viewContext;

      _ajax = new AjaxHelper(_viewContext);
      _html = new HtmlHelper(_viewContext);
      _url = new UrlHelper(_viewContext);

      viewContext.HttpContext.Response.Write(_compiledView.Render());
    }

    public AjaxHelper Ajax
    {
      get { return _ajax; }
    }

    public HtmlHelper Html
    {
      get { return _html; }
    }

    public UrlHelper Url
    {
      get { return _url; }
    }

    public TViewData ViewData
    {
      get { return _viewData; }
    }

    public void SetViewData(object viewData)
    {
      _viewData = (TViewData)viewData;
    }
  }
}