using System;
using System.Collections.Specialized;
using SAHL.Config.Services.Core;

namespace SAHL.Config.Services.Windows
{
    public class WindowsServiceSettings : ServiceSettings, IWindowsServiceSettings
    {
        public WindowsServiceSettings(NameValueCollection nameValueCollection) 
            : base(nameValueCollection)
        {
        }


        public bool EnableFirstChanceException
        {
            get
            {
                var value = this.nameValueCollection["EnableFirstChanceException"];
                return value != null && Convert.ToBoolean(value);
            }
        }

        public bool EnableUnhandledException
        {
            get
            {
                var value = this.nameValueCollection["EnableUnhandledException"];
                return value != null && Convert.ToBoolean(value);
            }
        }
    }
}
