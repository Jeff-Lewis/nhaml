**Note: All examples are based on the latest version of NHaml. Check the [GettingNHaml](GettingNHaml.md) for information about getting the latest version.**

Currently, unlike Rails, the ASP.NET MVC framework delegates layout and partial handling to the view engine. Therefore, NHaml provides it’s own layout & partial system.

# Partials #

Partials are small reusable sub-views available within a single controller context. NHaml implements a Rails-like partial system. Any view template beginning with an '`_`' is considered a partial. To use a partial simply use the NHaml '`_`' markup rule like so:

~/Views/Products/List.haml

```
- foreach (var product in ViewData.Products)
  %li
    _ Product
```

In this example NHaml will replace the line "`_` Product" with the contents of the file _Product.haml in the current controller's view folder `(`~/Views/Products`)`_

~/Views/Products/_Product.haml_

```
= product.ProductName 
%span.editlink
  = Html.ActionLink("Edit", new { Action="Edit" ID=product.ProductID })
```
At compile-time, both layouts and partials are merged into a single view and so any ViewData context is available.

# Layouts #
Layouts are the NHaml equivalent of master pages. NHaml provides a Rails-like layout system. Layouts are applied automatically based on the following conventions:

If a layout is specified through the masterName argument of RenderView then that layout is applied. Or,
If a layout with the same name as the controller exists in Views/Shared then it is applied. Or,
If a layout called Application.haml exists in the Views/Shared folder then it is applied.
Here is an example layout targeting the Products controller (Views/Shared/Products.haml)

~/Views/Products/Products.haml

```
!!!
%html{xmlns="http://www.w3.org/1999/xhtml"}
  %head
    %title My Sample MVC Application
    %link{href="../../Content/Sites" rel="stylesheet" type="text/css"}
  %body
    #inner
      #header
        %h1 My Store Manager - Products Section
      #menu
        %ul
          %li
            %a{href="/"} Home
          %li
            = Html.ActionLink("About Us", "About", "Home")
          %li
            = Html.ActionLink("Products", new { Controller = "Products" Action = "Category" ID = 1 })
      #maincontent
        _
      #footer
```
A layout file uses the  '`_`' partial operator with no arguments to signify where view content should be inserted into the layout.

Source from http://andrewpeters.net/2007/12/19/introducing-nhaml-an-aspnet-mvc-view-engine/