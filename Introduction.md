**Note: All examples are based on the latest version of NHaml. Check the [GettingNHaml](GettingNHaml.md) for information about getting the latest version.**

From the Haml website:

"Haml is a markup language that‘s used to cleanly and simply describe the XHTML of any web document, without the use of inline code. Haml functions as a replacement for inline page templating systems such as PHP, ERB, and ASP. However, Haml avoids the need for explicitly coding XHTML into the template, because it is actually an abstract description of the XHTML, with some code to generate dynamic content."

In other words, NHaml is an external DSL for XML (and hence XHTML). It’s primary qualities are it’s simplicity, terseness, performance and that it outputs nicely formatted XML. Additionally, the NHaml view engine provides support for Rails style layouts and partials – more on that below.

An example is the best way to grok how NHaml works. Below we have a typical view targeting the default Web Forms view engine. Observe the proliferation of angle-brackets, closing tags and general cruft.

~/Views/Products/List.aspx

```
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
    CodeBehind="List.aspx" Inherits="MvcApplication5.Views.Products.List" Title="Products" %>
<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
  <h2><%= ViewData.CategoryName %></h2>
  <ul>
    <% foreach (var product in ViewData.Products) { %>
      <li>
        <%= product.ProductName %> 
        <div class="editlink">
          (<%= Html.ActionLink("Edit", new { Action="Edit", ID=product.ProductID })%>)
        </div>
      </li>
    <% } %>
  </ul>
  <%= Html.ActionLink("Add New Product", new { Action="New" }) %>
</asp:Content>
```

Now, let us bask in the glory of the NHaml version:

~/Views/Products/List.haml

```
%h2= ViewData.CategoryName
%ul
  - foreach (var product in ViewData.Products)
    %li
      = product.ProductName 
      .editlink
        = Html.ActionLink("Edit", new { Action="Edit" ID=product.ProductID })
= Html.ActionLink("Add New Product", new { Action="New" })
```

There are a couple of things going on here. First, NHaml uses indentation (2 spaces by default) as an alternative to closing tags. Second, the first non-whitespace character in a line can describe how the line should be processed. For example, the first line in the example above starts with a ’%’, which directs NHaml to process that line as an XHTML tag. These directives are called Markup Rules and are discussed in detail on the [NHamlLanguageReference](NHamlLanguageReference.md) page.

Source from http://andrewpeters.net/2007/12/19/introducing-nhaml-an-aspnet-mvc-view-engine/