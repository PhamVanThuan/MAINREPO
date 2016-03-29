using System.Reflection;
using WatiN.Core;

namespace ObjectMaps.NavigationControls
{
    public abstract class BaseNavigation : Page
    {
        protected override void InitializeContents()
        {
            base.InitializeContents();
            var watinProperties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var watinProperty in watinProperties)
            {
                if (watinProperty.PropertyType.BaseType != null
                        && watinProperty.PropertyType.BaseType.Name.Contains(typeof(Element).Name)
                        && !watinProperty.PropertyType.BaseType.Name.Contains("Collection"))
                {
                    var element = (Element)watinProperty.GetValue(this, null);
                    element.Description = watinProperty.Name;
                }
            }
        }
    }
}