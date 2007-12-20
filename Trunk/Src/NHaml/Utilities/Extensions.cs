using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Mindscape.NHaml.Utilities
{
  public static class Extensions
  {
    [SuppressMessage("Microsoft.Naming", "CA1720")]
    public static string RenderAttributes(this object obj)
    {
      if (obj != null)
      {
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);

        if (properties.Count > 0)
        {
          StringBuilder attributes = new StringBuilder();

          attributes.Append(properties[0].Name.Replace('_', '-') + "=\"" + properties[0].GetValue(obj) + "\"");

          for (int i = 1; i < properties.Count; i++)
          {
            attributes.Append(" " + properties[i].Name + "=\"" + properties[i].GetValue(obj) + "\"");
          }

          return attributes.ToString();
        }
      }

      return null;
    }
  }
}