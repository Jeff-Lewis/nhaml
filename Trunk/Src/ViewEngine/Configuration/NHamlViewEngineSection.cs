using System;
using System.Configuration;
using System.Globalization;
using System.Security.Permissions;
using System.Web;

namespace Mindscape.NHaml.ViewEngine.Configuration
{
  [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
  [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
  public sealed class NHamlViewEngineSection : ConfigurationSection
  {
    private const string ProductionAttribute = "production";

    public static NHamlViewEngineSection Read()
    {
      return (NHamlViewEngineSection)ConfigurationManager.GetSection("nhamlViewEngine");
    }

    [ConfigurationProperty(ProductionAttribute)]
    public bool Production
    {
      get { return Convert.ToBoolean(this[ProductionAttribute], CultureInfo.CurrentCulture); }
      set { this[ProductionAttribute] = value; }
    }
  }
}