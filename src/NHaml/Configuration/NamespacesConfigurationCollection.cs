using System.Security.Permissions;
using System.Web;

namespace NHaml.Configuration
{
    [AspNetHostingPermission( SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal )]
    [AspNetHostingPermission( SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal )]
    public class NamespacesConfigurationCollection : ConfigurationCollection<NamespaceConfigurationElement>
    {
    }
}