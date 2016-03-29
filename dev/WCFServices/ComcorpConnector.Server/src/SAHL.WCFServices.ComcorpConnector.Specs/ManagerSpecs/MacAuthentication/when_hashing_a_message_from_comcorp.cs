using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.WCFServices.ComcorpConnector.Interfaces;
using SAHL.WCFServices.ComcorpConnector.Managers.MacAuthentication;
using System.Globalization;
using System.IO;
using System.Xml;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.MacAuthentication
{
    public class when_hashing_a_message_from_comcorp : WithCoreFakes
    {
        private static IComcorpConnectorSettings comcorpConnectorSettings;
        private static MacAuthenticationManager manager;
        private static string result;
        private static string expectedResult;
        private static string message;

        private Establish context = () =>
        {
            comcorpConnectorSettings = An<IComcorpConnectorSettings>();
            manager = new MacAuthenticationManager(comcorpConnectorSettings);
            XmlDocument doc = new XmlDocument();
            doc.Load("TestMessage.xml");
            message = doc.InnerXml;
            expectedResult = "vaPpjpcsKt/Nvmu6gp7TO1AOk78=";
        };

        private Because of = () =>
        {
            result = manager.HashMessage(message);
        };

        private It should_return_a_base64_encoded_sha1_hash_of_the_message = () =>
        {
            result.ShouldEqual(expectedResult);
        };
    }
}