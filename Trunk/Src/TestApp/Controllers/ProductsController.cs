using System;
using System.Web.Mvc;
using System.Web.Mvc.BindingHelpers;
using Mindscape.NHaml.Tests.TestApp.Models;

namespace Mindscape.NHaml.Tests.TestApp.Controllers
{
  public class ProductsController : Controller
  {
    NorthwindDataContext northwind = new NorthwindDataContext();

    //
    // Products/Category/1

    [ControllerAction]
    public void Category(int id)
    {
      Category category = northwind.GetCategoryById(id);

      RenderView("List", category);
    }

    //
    // Products/New

    [ControllerAction]
    public void New()
    {
      ProductsNewViewData viewData = new ProductsNewViewData();

      viewData.Suppliers = northwind.GetSuppliers();
      viewData.Categories = northwind.GetCategories();

      RenderView("New", viewData);
    }

    //
    // Products/Create

    [ControllerAction]
    public void Create()
    {
      Product product = new Product();
      product.UpdateFrom(Request.Form);

      northwind.AddProduct(product);
      northwind.SubmitChanges();

      RedirectToAction(new { Action = "Category", ID = product.CategoryID });
    }

    //
    // Products/Edit/5

    [ControllerAction]
    public void Edit(int id)
    {
      ProductsEditViewData viewData = new ProductsEditViewData();

      viewData.Product = northwind.GetProductById(id);
      viewData.Categories = northwind.GetCategories();
      viewData.Suppliers = northwind.GetSuppliers();

      RenderView("Edit", viewData);
    }

    //
    // Products/Update/5

    [ControllerAction]
    public void Update(int id)
    {
      Product product = northwind.GetProductById(id);
      product.UpdateFrom(Request.Form);

      northwind.SubmitChanges();

      RedirectToAction(new { Action = "Category", ID = product.CategoryID });
    }
  }
}