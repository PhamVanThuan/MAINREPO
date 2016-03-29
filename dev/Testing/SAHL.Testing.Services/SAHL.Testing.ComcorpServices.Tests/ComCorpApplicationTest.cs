using System;
using System.Text;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.Testing.ComcorpServices.Tests.Connector;
using SAHL.Core.Testing.Ioc;
using NUnit.Framework;
using System.ServiceModel;
using SAHL.Core.Testing;

namespace SAHL.Testing.ComcorpServices.Tests
{
    [TestFixture]
    public abstract class ComCorpApplicationTest
    {
        private const string XmlNamespace = @"xmlns=""http://schemas.datacontract.org/2004/07/SAHL.WCFServices.ComcorpConnector.Objects""";
        private SAHLIntegrationServiceClient serviceClient;

        [TestFixtureSetUp]
        public void Setup()
        {
            var testingIoc = TestingIoc.Initialise();
            this.serviceClient = new SAHLIntegrationServiceClient();
        }

        [Test]
        public void SubmitXml()
        {
            var typeUnderTest = this.GetType();
            var fileName = Path.Combine("Xml", String.Format("{0}.xml", typeUnderTest.Name));
            var xmlDocument = this.LoadXmlDocument(fileName);

            var comcorpApplication = this.CreateApplicationFromXmlDocument(xmlDocument);

            try
            {
                var appKey = 0;
                Int32.TryParse(this.serviceClient.Submit(comcorpApplication), out appKey);
                var offer = TestApiClient.Get<Offer>(new { offerkey = appKey}).FirstOrDefault();
                Assert.NotNull(offer, "Expected an offer to bre create for comcorp application");
                Assert.AreEqual(offer.OfferKey, appKey);
            }
            catch (FaultException<SAHLFault> validationFault)
            {
                var expectedError = (ExpectError)Attribute.GetCustomAttribute(typeUnderTest, typeof(ExpectError));
                if (expectedError == null)
                {
                    Assert.Fail(validationFault.Detail.FaultDescription);
                }
                else
                {
                    Assert.True(validationFault.Detail.FaultDescription.Contains(expectedError.Message),"expected error: {0}, but was {1}", expectedError.Message, validationFault.Detail.FaultDescription);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        private string LoadXmlDocument(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new Exception("XML File not found");
            }

            var xmlDocument = XDocument.Load(fileName);
            var envelopeDesc = xmlDocument.Descendants().FirstOrDefault(x => x.Name.LocalName == typeof(Application).Name);

            var xmlData = envelopeDesc.ToString();
            xmlData = xmlData.Replace(@"xmlns=""http://schemas.microsoft.com/2004/06/ServiceModel/Management/MessageTrace""", "");
            xmlData = xmlData.Replace(@"xmlns=""http://tempuri.org/""", XmlNamespace);

            return xmlData;
        }

        private Application CreateApplicationFromXmlDocument(string xmlDocument)
        {
            var myMapping = (new SoapReflectionImporter().ImportTypeMapping(typeof(Application)));
            var mySerializer = new XmlSerializer(myMapping);
            var formatter = new DataContractSerializer(typeof(Application));

            using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(xmlDocument)))
            {
                var instance = formatter.ReadObject(memoryStream) as Application;
                return instance;
            }
        }

    }
    public class ExpectError : Attribute
    {
        public ExpectError(string message)
        {
            this.Message = message;
        }
        public string Message { get; internal set; }
        public int OfferKey { get; set; }
    }

    public class Offer
    {
        public int OfferKey { get; set; }
    }
}
