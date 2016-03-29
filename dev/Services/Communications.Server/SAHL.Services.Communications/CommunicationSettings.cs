using System.Collections.Specialized;

namespace SAHL.Services.Communications
{
    public class CommunicationSettings : ICommunicationSettings
    {
        private NameValueCollection nameValueCollection;

        public CommunicationSettings(NameValueCollection nameValueCollection)
        {
            this.nameValueCollection = nameValueCollection;
        }

        public string ComcorpLiveRepliesServiceVersion
        {
            get
            {
                string value = nameValueCollection["ComcorpLiveRepliesServiceVersion"];
                return value ?? string.Empty;
            }
        }

        public string ComcorpLiveRepliesWebserviceURL
        {
            get
            {
                string value = nameValueCollection["ComcorpLiveRepliesWebserviceURL"];
                return value ?? string.Empty;
            }
        }

        public string ComcorpLiveRepliesPublicKeyModulus
        {
            get
            {
                string value = nameValueCollection["ComcorpLiveRepliesPublicKeyModulus"];
                return value ?? string.Empty;
            }
        }

        public string ComcorpLiveRepliesPublicKeyExponent
        {
            get
            {
                string value = nameValueCollection["ComcorpLiveRepliesPublicKeyExponent"];
                return value ?? string.Empty;
            }
        }

        public string ComcorpLiveRepliesBankId
        {
            get
            {
                string value = nameValueCollection["ComcorpLiveRepliesBankId"];
                return value ?? string.Empty;
            }
        }

        public string InternalEmailFromAddress
        {
            get
            {
                string value = nameValueCollection["InternalEmailFromAddress"];
                return value ?? string.Empty;
            }
        }
    }
}