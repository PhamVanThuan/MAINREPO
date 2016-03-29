using NUnit.Framework;
using SAHL.Config.Services;
using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest;
using SAHL.Testing.ComcorpServices.Tests.Connector;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.IO;
using System.ServiceModel;
using System.Xml;

namespace SAHL.Testing.ComcorpServices.Tests
{
    [TestFixture]
    internal class DocumentServiceTestBase<T>
    {
        private SAHLDocumentServiceClient client;
        private IIocContainer _container;
        protected DateTime testStartDate;
        protected T documentRequest;
        protected IFrontEndTestServiceClient feTestClient;

        [TestFixtureSetUp]
        public void OnTestFixtureSetup()
        {
            ServiceBootstrapper bootstrapper = new ServiceBootstrapper();
            _container = bootstrapper.Initialise();
            this.client = new SAHLDocumentServiceClient();
            feTestClient = _container.GetInstance<IFrontEndTestServiceClient>();
            testStartDate = DateTime.Now.Subtract(new TimeSpan(2, 4, 0)); //Add 02h04m00.000s offset for SYSAWS02 server
        }

        public SupportingDocumentsReply SubmitCoApplicantDocs(ImagingCoApplicantRequest coApplicantDocs, string expectedMessage, bool failOnException = false)
        {
            try
            {
                return this.client.ProcessCoApplicantMessage(coApplicantDocs);
            }
            catch (FaultException<SAHLFault> validationFault)
            {
                if (!validationFault.Detail.FaultDescription.Contains(expectedMessage))
                {
                    Assert.Fail(expectedMessage);
                }
                else
                {
                    if (failOnException)
                    {
                        Assert.Fail(validationFault.Detail.FaultDescription);
                    }
                }
            }
            catch (Exception ex)
            {
                if (failOnException)
                {
                    Assert.Fail(ex.Message);
                }
            }
            return null;
        }

        public SupportingDocumentsReply SubmitApplicationDocs(ImagingApplicationRequest applicationDocs, string expectedMessage, bool failOnException = false)
        {
            try
            {
                return this.client.ProcessApplicationMessage(applicationDocs);
            }
            catch (FaultException<SAHLFault> validationFault)
            {
                if (!validationFault.Detail.FaultDescription.Contains(expectedMessage))
                {
                    Assert.Fail(expectedMessage);
                }
                else
                {
                    if (failOnException)
                    {
                        Assert.Fail(validationFault.Detail.FaultDescription);
                    }
                }
            }
            catch (Exception ex)
            {
                if (failOnException)
                {
                    Assert.Fail(ex.Message);
                }
            }

            return null;
        }

        public SupportingDocumentsReply SubmitMainApplicantDocs(ImagingMainApplicantRequest mainApplicantDocs, string expectedMessage, bool failOnException = false)
        {
            try
            {
                return this.client.ProcessMainApplicantMessage(mainApplicantDocs);
            }
            catch (FaultException<SAHLFault> validationFault)
            {
                if (!validationFault.Detail.FaultDescription.Contains(expectedMessage))
                {
                    Assert.Fail(expectedMessage);
                }
                else
                {
                    if (failOnException)
                    {
                        Assert.Fail(validationFault.Detail.FaultDescription);
                    }
                }
            }
            catch (Exception ex)
            {
                if (failOnException)
                {
                    Assert.Fail(ex.Message);
                }
            }

            return null;
        }

        public void GenerateRequestFromTemplateXML(string templatePath)
        {
            var assemblyDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(assemblyDirectory, templatePath);
            var reader = XmlReader.Create(filePath);
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T), "http://tempuri.org/");
            this.documentRequest = (T)serializer.Deserialize(reader);
            reader.Close();
        }

        public U GenerateRequestFromTemplateXML<U>(string templatePath)
        {
            var assemblyDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(assemblyDirectory, templatePath);
            var reader = XmlReader.Create(filePath);
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(U), "http://tempuri.org/");
            var documentRequest = (U)serializer.Deserialize(reader);
            reader.Close();
            return documentRequest;
        }

        public void AddMacToRequest()
        {
            var macHelper = new MacHelper();
            var mac = macHelper.CreateMessageMac(this.documentRequest);
            SetProperty(this.documentRequest, "RequestHeader.RequestMac", mac);
        }

        public void AddMacToRequest<T>(T documentRequest)
        {
            var macHelper = new MacHelper();
            var mac = macHelper.CreateMessageMac(documentRequest);
            SetProperty(documentRequest, "RequestHeader.RequestMac", mac);
        }

        public void SetProperty(object source, string property, object target)
        {
            string[] bits = property.Split('.');
            for (int i = 0; i < bits.Length - 1; i++)
            {
                var prop = source.GetType().GetProperty(bits[i]);
                source = prop.GetValue(source, null);
            }
            var propertyToSet = source.GetType().GetProperty(bits[bits.Length - 1]);
            propertyToSet.SetValue(source, target, null);
        }

        protected void PerformQuery(IFrontEndTestQuery query)
        {
            var messages = feTestClient.PerformQuery(query);
            if (messages.HasErrors)
                Assert.Fail("Test Service Query Failed.");
        }

        protected void PerformCommand(IFrontEndTestCommand command)
        {
            ServiceRequestMetadata metaData = new ServiceRequestMetadata();
            var messages = feTestClient.PerformCommand(command, metaData);
            if (messages.HasErrors)
                Assert.Fail("Test Service Command Failed.");
        }
    }
}