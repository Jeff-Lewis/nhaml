**Note: All examples are based on the latest version of NHaml. Check the [GettingNHaml](GettingNHaml.md) for information about getting the latest version.**

Here are a few examples of some NHaml ActionLink helpers that work using REST conventions.
```
= Html.ActionLink<CategoriesController>() // /categories
= Html.ActionLink<CategoriesController>(RestfulAction.New, "Add Category") // /categories/new
= Html.ActionLink(category)  // /categories/show/42
= Html.ActionLink(category, RestfulAction.Edit, "Edit Category") // /categories/edit/42
= Html.ActionLink(category, RestfulAction.Destroy, "Delete Category") // /categories/destroy/42
```


Source from http://andrewpeters.net/2008/08/21/nhaml-block-methods-and-restful-helpers/