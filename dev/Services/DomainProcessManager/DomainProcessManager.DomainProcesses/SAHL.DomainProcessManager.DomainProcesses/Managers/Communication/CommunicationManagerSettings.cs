using System;
using System.Collections.Specialized;

namespace SAHL.DomainProcessManager.DomainProcesses.Managers.Communication
{
    public class CommunicationManagerSettings : ICommunicationManagerSettings
    {
        private NameValueCollection nameValueCollection;

        public CommunicationManagerSettings(NameValueCollection nameValueCollection)
        {
            this.nameValueCollection = nameValueCollection;
        }

        public string CamTeamEmailAddress
        {
            get
            {
                string value = nameValueCollection["CAMTeamEmailAddress"];
                return value ?? string.Empty;
            }
        }

        public string ITFrontEndTeamEmailAddress
        {
            get
            {
                string value = nameValueCollection["ITFrontEndTeamEmailAddress"];
                return value ?? string.Empty;
            }
        }

        public string AttorneyInvoiceFailuresEmailAddress
        {
          get
          {
            string value = nameValueCollection["AttorneyInvoiceFailures"];
            return value ?? string.Empty;
          }
        }


        public string ThirdPartyInvoiceProcessorEmailAddress
        {
            get 
            {
                string value = nameValueCollection["ThirdPartyInvoiceProcessorEmailAddress"];
                return value ?? string.Empty;
            }
        }
    }
}
