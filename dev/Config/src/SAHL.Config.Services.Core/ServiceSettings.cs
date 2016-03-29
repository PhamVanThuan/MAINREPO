using System;
using System.Collections.Specialized;

namespace SAHL.Config.Services.Core
{
    public class ServiceSettings : IServiceSettings
    {
        protected readonly NameValueCollection nameValueCollection;

        public ServiceSettings(NameValueCollection nameValueCollection)
        {
            if (nameValueCollection == null) { throw new ArgumentNullException("nameValueCollection"); }
            this.nameValueCollection = nameValueCollection;
        }

        public string DisplayName
        {
            get
            {
                var value = this.nameValueCollection["DisplayName"];
                return value ?? string.Empty;
            }
        }

        public string Description
        {
            get
            {
                var value = this.nameValueCollection["Description"];
                return value ?? string.Empty;
            }
        }

        public string ServiceName
        {
            get
            {
                var value = this.nameValueCollection["ServiceName"];
                return value ?? string.Empty;
            }
        }
    }
}