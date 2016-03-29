using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.WCFServices.ComcorpConnector.Interfaces;
using SAHL.WCFServices.ComcorpConnector.Managers.MacAuthentication;
using Machine.Fakes;
using System.IO;
using System.Xml.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.MacAuthentication
{
    public class when_authenticating_a_message_with_a_valid_request_mac : WithCoreFakes
    {
        private static IComcorpConnectorSettings comcorpConnectorSettings;
        private static MacAuthenticationManager manager;
        private static bool result;
        private static string message;
        private static string requestMac;
        private static MacHelper macHelper;
        private Establish context = () =>
        {
            macHelper = new MacHelper();
            XmlDocument doc = new XmlDocument();
            doc.Load("SampleMessage.xml");
            message = doc.InnerXml;
            requestMac = macHelper.CreateMessageMac(message);
            comcorpConnectorSettings = An<IComcorpConnectorSettings>();
            comcorpConnectorSettings.WhenToldTo(x => x.DocumentsPrivateKeyPath).Return(".\\PrivateKey.xml");
            manager = new MacAuthenticationManager(comcorpConnectorSettings);
        };

        private Because of = () =>
        {
            result = manager.AuthenticateMessage(message, requestMac);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
