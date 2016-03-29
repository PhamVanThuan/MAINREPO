using SAHL.WCFServices.ComcorpConnector.Interfaces;
using System.Collections.Specialized;

namespace SAHL.WCFServices.ComcorpConnector
{
    public class ComcorpConnectorSettings : IComcorpConnectorSettings
    {
        private NameValueCollection nameValueCollection;

        public ComcorpConnectorSettings(NameValueCollection nameValueCollection)
        {
            this.nameValueCollection = nameValueCollection;
        }

        public string DocumentsPrivateKeyPath { get { return nameValueCollection["DocumentsPrivateKeyPath"]; } }
    }
}