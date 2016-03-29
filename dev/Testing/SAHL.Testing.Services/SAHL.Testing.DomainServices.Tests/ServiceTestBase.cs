using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using SAHL.Config.Services;
using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FrontEndTest;
using SAHL.Testing.Services.Tests.Managers;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;

namespace SAHL.Testing.Services
{
    [TestFixture]
    public class ServiceTestBase<T>
    {
        protected ISystemMessageCollection messages;
        protected ServiceRequestMetadata metaData;
        protected T service;
        protected ILinkedKeyManager linkedKeyManager;
        protected Random randomizer;
        protected int linkedKey;

        protected Guid linkedGuid { get; private set; }

        private IIocContainer _container;
        private MethodInfo _methodToInvoke;
        protected IFrontEndTestServiceClient _feTestClient;
        private IIocContainer _feTestContainer;
        protected IDbFactory _dbFactory;
        protected IClientDomainManager clientDomainManager;
        public Dictionary<string, string> _metaDataDictionary;
        protected DateTime TestStartTime;

        [TestFixtureSetUp]
        public void OnTestFixtureSetup()
        {
            _metaDataDictionary = new Dictionary<string, string>();
            ServiceBootstrapper bootstrapper = new ServiceBootstrapper();
            _container = bootstrapper.Initialise();
            service = _container.GetInstance<T>();
            ServiceBootstrapper feTestBootStrapper = new ServiceBootstrapper();
            _feTestContainer = feTestBootStrapper.Initialise();
            _feTestClient = _feTestContainer.GetInstance<IFrontEndTestServiceClient>();
            _dbFactory = _container.GetInstance<IDbFactory>();
            linkedKeyManager = _container.GetInstance<ILinkedKeyManager>();
            randomizer = new Random();
        }

        [SetUp]
        public void OnTestSetup()
        {
            clientDomainManager = new ClientDomainManager(_container);

            messages = SystemMessageCollection.Empty();
            linkedGuid = CombGuid.Instance.Generate();
            linkedKey = 0;
            TestStartTime = DateTime.Now;
        }

        [TearDown]
        public void OnTestTearDown()
        {
            if (linkedGuid != Guid.Empty)
            {
                this.linkedKeyManager.DeleteLinkedKey(linkedGuid);
            }
            if(_metaDataDictionary != null)
            {
                _metaDataDictionary.Clear();
            }
        }

        public ServiceTestBase<T> Execute<T1>(T1 serviceCommand) where T1 : IServiceCommand
        {
            try
            {
                if (_metaDataDictionary.Where(x => x.Key.Equals(ServiceRequestMetadata.HEADER_USERNAME)).Count().Equals(0))
                {
                    _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERNAME, @"SAHL\HaloUser");
                }
                metaData = new ServiceRequestMetadata(_metaDataDictionary);
                object[] parameters = new object[] { serviceCommand, metaData };
                string methodName = "PerformCommand";
                if (serviceCommand is IServiceQuery)
                {
                    methodName = "PerformQuery";
                    parameters = new object[] { serviceCommand };
                }
                _methodToInvoke = this.service.GetType().GetMethods().Where(x => x.Name == methodName).FirstOrDefault();
                _methodToInvoke = _methodToInvoke.MakeGenericMethod(typeof(T1));
                if (_methodToInvoke != null)
                {
                    messages.Aggregate((ISystemMessageCollection)_methodToInvoke.Invoke(this.service, parameters));
                    if (messages.ExceptionMessages().Count() > 0)
                    {
                        var messageList = new List<string>();
                        foreach (var message in messages.ExceptionMessages())
                        {
                            messageList.Add(message.Message);
                        }
                        throw new Exception(string.Join("\n", messageList));
                    }
                    return this;
                }
                return this;
            }
            catch (Exception ex)
            {
                string message = ex.Message != null ? ex.Message.ToString() : ex.InnerException.ToString();
                Assert.Fail(string.Format("Exception encountered running test. Details: {0}", message));
            }
            return this;
        }

        protected void PerformQuery(IFrontEndTestQuery query)
        {
            var messages = _feTestClient.PerformQuery(query);
            if (messages.HasErrors)
            {
                CollateMessagesAndFailTest(messages);
            }
        }

        protected void PerformCommand(IFrontEndTestCommand command)
        {
            Dictionary<string, string> metadataDictionary = new Dictionary<string, string>();
            metadataDictionary.Add("UserName", @"SAHL\HaloUser");
            ServiceRequestMetadata metaData = new ServiceRequestMetadata(metadataDictionary);
            var messages = _feTestClient.PerformCommand(command, metaData);
            if (messages.HasErrors)
            {
                CollateMessagesAndFailTest(messages);
            }
        }

        private static void CollateMessagesAndFailTest(ISystemMessageCollection messages)
        {
            var errorMessages = string.Empty;
            foreach (var errormessage in messages.ErrorMessages())
            {
                errorMessages += errormessage.Message + "/n";
            }
            Assert.Fail(string.Format("Test Service call failed: {0}", errorMessages));
        }

        public void AndExpectThatErrorMessagesContain(params string[] expectedMessages)
        {
            foreach (var m in expectedMessages)
            {
                Assert.IsTrue(messages.ErrorMessages().Where(x => x.Message == m).FirstOrDefault() != null,
                    "Expected message: {0}", m);
            }
        }

        public void AndExpectThatMessagesContain(params SystemMessage[] expectedMessages)
        {
            foreach (var m in expectedMessages)
            {
                Assert.IsTrue(messages.AllMessages.Where(x => x.Message == m.Message && x.Severity == m.Severity).FirstOrDefault() != null,
                    "Expected {0} message: {1}", m.Severity, m.Message);
            }
        }

        public void WithoutErrors()
        {
            if (messages.ErrorMessages().Count() > 0)
            {
                var messageList = new List<string>();
                foreach (var message in messages.AllMessages)
                {
                    messageList.Add(message.Message);
                }
                Assert.Fail(string.Format("Unexpected error messages were returned when running: {0} \n {1}", _methodToInvoke.Name, string.Join("\n", messageList)));
            }
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }


        public void SetHeaderMetadataForUser(string userName)
        {
            this._metaDataDictionary = new ServiceRequestMetadata();
            var user = TestApiClient.Get<HaloUsersDataModel>(new { ADUserName = userName }).FirstOrDefault();
            var orgStructureKey = user.UserOrganisationStructureKey;
            var userCapabilities = user.Capabilities;
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERORGANISATIONSTRUCTUREKEY, orgStructureKey.ToString());
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_CURRENTUSERCAPABILITIES, userCapabilities);
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERNAME, string.Concat(@"SAHL\", user.ADUserName));
        }
    }
}