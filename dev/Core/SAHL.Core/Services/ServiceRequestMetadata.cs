using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAHL.Core.Services
{
    [Serializable]
    public class ServiceRequestMetadata : Dictionary<string, string>, IServiceRequestMetadata
    {
        public const string HEADER_USERNAME = "username";

        public const string HEADER_USERORGANISATIONSTRUCTUREKEY = "userorganisationstructurekey";

        public const string HEADER_CURRENTUSERROLE = "currentuserrole";

        public const string HEADER_CURRENTUSERCAPABILITIES = "currentusercapabilities";

        public ServiceRequestMetadata()
        {
        }

        public ServiceRequestMetadata(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ServiceRequestMetadata(IDictionary<string, string> metadata)
        {
            foreach (var kvp in metadata)
            {
                this.Add(kvp.Key, kvp.Value);
            }
        }

        public string UserName
        {
            get
            {
                return this.ContainsKey(HEADER_USERNAME) ? this[HEADER_USERNAME] : "";
            }
        }

        public string CurrentUserRole
        {
            get
            {
                return this.ContainsKey(HEADER_CURRENTUSERROLE) ? this[HEADER_CURRENTUSERROLE] : string.Empty;
            }
        }

        public int? UserOrganisationStructureKey
        {
            get
            {
                string userOrganisationStructureKeyFromHeader = this.ContainsKey(HEADER_USERORGANISATIONSTRUCTUREKEY)
                                                              ? this[HEADER_USERORGANISATIONSTRUCTUREKEY] : string.Empty;

                int? userOrganisationStructureKey = null;
                int keyParsedFromHeader;
                if (int.TryParse(userOrganisationStructureKeyFromHeader, out keyParsedFromHeader))
                {
                    userOrganisationStructureKey = keyParsedFromHeader;
                }
                return userOrganisationStructureKey;
            }
        }


        public string[] CurrentUserCapabilities
        {
            get
            {
                return this.ContainsKey(HEADER_CURRENTUSERCAPABILITIES) ? this[HEADER_CURRENTUSERCAPABILITIES].Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries) : null;
            }
        }
    }
}