using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHomeloans.SAHL_VS_WebJS
{
    public class DependancyInstaller
    {
        PackageRegistrySettings regSettings;

        public DependancyInstaller(WebJSPackage package)
        {
            regSettings = new PackageRegistrySettings(package);
        }

        public void Dependancy<T>(string regKey, T expectedValue, Action action) where T : IComparable
        {
            T setting = regSettings.GetValue<T>(regKey);
            if (setting == null || setting.CompareTo(expectedValue) != 0)
            {
                action();
                regSettings.SetValue(regKey, expectedValue);
            }
        }
    }
}
