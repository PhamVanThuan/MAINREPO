using System.Xml;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.WCFServices.ComcorpConnector.Interfaces;
using SAHL.WCFServices.ComcorpConnector.Managers.MacAuthentication;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.MacAuthentication
{
    public class when_authenticating_a_message_with_an_invalid_request_mac : WithCoreFakes
    {
        private static IComcorpConnectorSettings comcorpConnectorSettings;
        private static MacAuthenticationManager manager;
        private static bool result;
        private static string message;
        private static string requestMac;

        private Establish context = () =>
        {
            requestMac = @"zukQu+USn92SWbz4omQc+zA9bDzEllFp/0IgfOUslZwfFunCEwje3TMVcEtdcfDmVeurAeDf2OShk7hUmSRW3CQK4QKkRsppDudvjo5/aq84qO2udWKSCp+9
GpXW2Xg5yz3i1OW+f5mrJNf4SVBJ0fZhfyXKwJivfsdkBDKV5w2xhGKNxM9Ftbgw8YaCkUE65VFC7etbCY3Y4Jqp1+uy9lT4oBatgX0DcOB1xwr4+YZPY1jnBEnMPfjxnw3NUBWNFq1N6PvB5pv
hRd2nQLpJvmp3X4YrN6i6cgJSwFTk+OiwwVUAdi9QXpMKvp5sYujiTiJ1UHcX4xnahhWBc6hpog==";
            comcorpConnectorSettings = An<IComcorpConnectorSettings>();
            comcorpConnectorSettings.WhenToldTo(x => x.DocumentsPrivateKeyPath).Return(".\\PrivateKey.xml");
            manager = new MacAuthenticationManager(comcorpConnectorSettings);
            XmlDocument doc = new XmlDocument();
            doc.Load("TestMessage.xml");
            message = doc.InnerXml;
        };

        private Because of = () =>
        {
            result = manager.AuthenticateMessage(message, requestMac);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}