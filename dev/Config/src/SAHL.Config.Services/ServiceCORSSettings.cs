using System;
using System.Collections.Specialized;

namespace SAHL.Config.Services
{
    public class ServiceCORSSettings : IServiceCORSSettings
    {
        protected readonly NameValueCollection nameValueCollection;

        public ServiceCORSSettings(NameValueCollection nameValueCollection)
        {
            if (nameValueCollection == null) { throw new ArgumentNullException("nameValueCollection"); }
            this.nameValueCollection = nameValueCollection;
        }

        public string AllowedOrigins
        {
            get
            {
                var value = this.nameValueCollection["AllowedOrigins"];
                return value ?? string.Empty;
            }
        }


        public string AllowedMethods
        {
            get
            {
                var value = this.nameValueCollection["AllowedMethods"];
                return value ?? string.Empty;
            }
        }

        public string AllowedHeaders
        {
            get
            {
                var value = this.nameValueCollection["AllowedHeaders"];
                return value ?? string.Empty;
            }
        }

        public string ExposedHeaders
        {
            get
            {
                var value = this.nameValueCollection["ExposedHeaders"];
                return value ?? string.Empty;
            }
        }
    }
}