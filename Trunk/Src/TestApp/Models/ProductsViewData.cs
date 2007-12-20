using System;
using System.Collections.Generic;
using Mindscape.NHaml.Tests.TestApp.Models;

namespace Mindscape.NHaml.Tests.TestApp.Models
{
  public class ProductsEditViewData
  {
    public Product Product { get; set; }
    public List<Supplier> Suppliers { get; set; }
    public List<Category> Categories { get; set; }
  }

  public class ProductsNewViewData
  {
    public List<Supplier> Suppliers { get; set; }
    public List<Category> Categories { get; set; }
  }
}
