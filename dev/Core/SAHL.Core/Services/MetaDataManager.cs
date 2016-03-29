using SAHL.Core.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace SAHL.Core.Services
{
    public class MetadataManager : IMetadataManager
    {
        public const string HEADER_USERNAME = "username";
        public const string HEADER_CURRENTUSERROLE = "currentuserrole";
        public const string HEADER_USERIPADDRESS = "userIPAddress";
        public const string METADATA_KEY_PREFIX = "#md#_";
        private IHostContext hostContext;

        public MetadataManager(IHostContext hostContext)
        {
            this.hostContext = hostContext;
        }

        public IServiceRequestMetadata GetMetaData()
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            string[] keys = this.hostContext.GetKeysWithPrefix(METADATA_KEY_PREFIX);

            foreach (var key in keys)
            {
                var value = this.hostContext.GetContextValue(key, METADATA_KEY_PREFIX);

                if (key == HEADER_USERNAME && string.IsNullOrEmpty(value))
                {
                    string contextUserName = this.GetContextUserName();
                    value = contextUserName;
                }

                metadata.Add(key, value);
            }

            if (!keys.Contains(HEADER_USERNAME))
            {
                string contextUserName = this.GetContextUserName();
                metadata.Add(HEADER_USERNAME, contextUserName);
            }

            return new ServiceRequestMetadata(metadata);
        }

        private string GetContextUserName()
        {
            IPrincipal principalUser = hostContext.GetUser();
            if (principalUser != null)
            {
                return principalUser.Identity.Name;
            }
            return "";
        }
    }
}