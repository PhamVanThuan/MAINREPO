using Microsoft.Win32;
using System;
namespace SAHomeloans.SAHL_VS_WebJS
{
    public class PackageRegistrySettings
    {
        RegistryKey packageKey;

        public PackageRegistrySettings(WebJSPackage package)
        {
            packageKey = package.ApplicationRegistryRoot.OpenSubKey("Packages").OpenSubKey("{" + typeof(WebJSPackage).GUID.ToString() + "}",true);
        }

        public T GetValue<T>(string name) where T : IComparable
        {
            object key = this.packageKey.GetValue(name);
            if (key == null)
            {
                return default(T);
            }
            return (T)this.packageKey.GetValue(name);
        }

        public void SetValue<T>(string name,T value)
        {
            packageKey.SetValue(name, value);
        }
    }
}