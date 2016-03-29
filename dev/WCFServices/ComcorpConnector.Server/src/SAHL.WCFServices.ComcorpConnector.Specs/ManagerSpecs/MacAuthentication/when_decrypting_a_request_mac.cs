using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.WCFServices.ComcorpConnector.Interfaces;
using SAHL.WCFServices.ComcorpConnector.Managers.MacAuthentication;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.MacAuthentication
{
    public class when_decrypting_a_request_mac : WithCoreFakes
    {
        private static IComcorpConnectorSettings comcorpConnectorSettings;
        private static MacAuthenticationManager manager;
        private static string requestMac;
        private static string expectedHash;
        private static string result;

        private Establish context = () =>
        {
            comcorpConnectorSettings = An<IComcorpConnectorSettings>();
            comcorpConnectorSettings.WhenToldTo(x => x.DocumentsPrivateKeyPath).Return(".\\PrivateKey.xml");
            requestMac = @"m8a/wXAZ2qo4vcm72PrGAUNQ73+jVBiQwVsfEmoKrDIDXJlRe5Wbew6XcQBiivywKUfg1wKnH8d/b0Mk+9hJM3dHtN0XO4hjGoglNVJ0mDDy3BohEGUhixA
+kgCXJSbbNZv2DOH4H2tVzcD5CFlnxzFUeDGxRRq44bLEUx71y62k7VaraWi5bV3jXjFqduMwd9k/PiZzcGaYyDU5zvuZePuYGpYI6kGqt40O3nJwSAdbfEBG/eCjLnshE3A0LvDtGnT7S1GXd
NClbryVPzJK9U5nET7FklA+lQyJCukh3+8bSdh6mAACtT7yulvqpbVgJDJcYTO9hXduc94ijTalKQ==";
            expectedHash = "eYmWAygdK1f7vHaQBfTr76Z7rTA=";
            manager = new MacAuthenticationManager(comcorpConnectorSettings);
        };

        private Because of = () =>
        {
            result = manager.GetDecryptedRequestMac(requestMac);
        };

        private It should_return_the_ascii_encoded_hash = () =>
        {
            result.ShouldEqual(expectedHash);
        };
    }
}