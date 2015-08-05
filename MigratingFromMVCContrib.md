To upgrade from the MvcContrib version of NHaml:

  1. Remove references to MvcContrib and the NHamlViewEngine
  1. Add a reference to NHaml.dll and NHaml.Web.MVC.dll
  1. In your global.asax add a using statement for NHaml.Web.MVC
  1. The same factory setting will work:
```
  ControllerBuilder.Current.SetControllerFactory(typeof(NHamlControllerFactory));
```
  1. If you have setup a configuration section, change the section tag to the following:
```
  <section name="nhaml" type="NHaml.Configuration.NHamlSection, NHaml"/>
```
  1. In your actual section, rename it to "nhaml" and remove the "views" grouping tag.

Done!