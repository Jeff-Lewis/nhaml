﻿using System.Web.Mvc;

namespace NHaml.Samples.Mvc.Boo.Models
{
    public class ProductsEditViewData
    {
        public Product Product { get; set; }
        public SelectList Suppliers { get; set; }
        public SelectList Categories { get; set; }
    }

    public class ProductsNewViewData
    {
        public SelectList Suppliers { get; set; }
        public SelectList Categories { get; set; }
    }
}