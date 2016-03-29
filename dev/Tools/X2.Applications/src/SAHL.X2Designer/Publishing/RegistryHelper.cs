using Microsoft.Win32;

namespace SAHL.X2Designer.Publishing
{
    public class RegistryHelper
    {
        public static object GetObject(RegistryHive hive, string RegPath, string Name)
        {
            using (RegistryKey CurrentUserKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.CurrentUser, string.Empty))
            {
                if (null == CurrentUserKey.OpenSubKey(RegPath))
                    return null;
                else
                {
                    using (RegistryKey key = CurrentUserKey.OpenSubKey(RegPath))
                    {
                        return key.GetValue(Name, null);
                    }
                }
            }
        }

        public static void SetObject(RegistryHive hive, string RegPath, string Name, object value, RegistryValueKind keyType)
        {
            using (RegistryKey CurrentUserKey = RegistryKey.OpenRemoteBaseKey(hive, string.Empty))
            {
                RegistryKey Key = null;
                if (null == CurrentUserKey.OpenSubKey(RegPath))
                {
                    Key = CurrentUserKey.CreateSubKey(RegPath);
                }
                else
                {
                    Key = CurrentUserKey.OpenSubKey(RegPath, true);
                }
                Key.SetValue(Name, value, keyType);
                Key.Flush();
                Key.Close();
            }
        }
    }
}