using NUnit.Framework;
using SAHL.WCFServices.ComcorpConnector.Objects.Document;
using System;
using System.IO;

namespace SAHL.Testing.ComcorpServices.Tests.MacAuthentication
{
    [TestFixture]
    public class MacAuthenticationTest
    {
        [Test]
        public void when_creating_message_mac()
        {
            var assemblyDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var sampleMessagePath = Path.Combine(assemblyDirectory, "SampleMessage.xml");

            var macHelper = new MacHelper();

            var documentRequest = new ImagingApplicationRequest
            {
                RequestHeader = new applicationHeaderType
                {
                    RequestMac = "aeuioeuieui"
                },
                SupportingDocuments = new ApplicationSupportingDocuments
                {
                    ApplicationDocuments = new documentType[]
                {
                    new documentType {
                        DocumentComments = "",
                        DocumentDescription = "",
                        DocumentImage = "ontehuntoehunoethuoeuoeuentu",
                        DocumentReference = 123123,
                        DocumentType = "ID Documents"
                    }
                }
                }
            };

            var mac = macHelper.CreateMessageMac(documentRequest);
            var result = macHelper.AuthenticateMessage(documentRequest, mac);
        }
    }
}