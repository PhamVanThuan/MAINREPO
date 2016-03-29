using NUnit.Framework;
using SAHL.Config.Services;
using SAHL.Core;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Core.X2.Messages;
using SAHL.Services.Interfaces.DomainProcessManagerProxy;
using SAHL.Services.Interfaces.FrontEndTest;
using SAHL.Services.Interfaces.X2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.DomainProcessManager.Tests
{
    [TestFixture]
    public class DomainProcessManagerTest
    {
        private IIocContainer _container;
        protected IFrontEndTestServiceClient _feTestClient;
        protected IDomainProcessManagerProxyServiceClient _dpmClient;
        protected IX2Service _x2ServiceClient;
        protected ILinkedKeyManager linkedKeyManager;
        protected int linkedKey;
        protected DateTime testStartTime;
        protected DateTime testFixtureStartTime;

        protected Guid linkedGuid { get; private set; }

        [TestFixtureSetUp]
        public void OnTestFixtureSetup()
        {
            ServiceBootstrapper bootstrapper = new ServiceBootstrapper();
            _container = bootstrapper.Initialise();
            ServiceBootstrapper feTestBootStrapper = new ServiceBootstrapper();
            _feTestClient = _container.GetInstance<IFrontEndTestServiceClient>();
            _dpmClient = _container.GetInstance<IDomainProcessManagerProxyServiceClient>();
            _x2ServiceClient = _container.GetInstance<IX2Service>();
            linkedKeyManager = _container.GetInstance<ILinkedKeyManager>();
            testFixtureStartTime = DateTime.Now;
        }

        [SetUp]
        public void OnTestSetup()
        {
            linkedGuid = CombGuid.Instance.Generate();
            linkedKey = 0;
            testStartTime = DateTime.Now;
        }

        [TearDown]
        public void OnTestTearDown()
        {
            if (linkedGuid != Guid.Empty)
            {
                this.linkedKeyManager.DeleteLinkedKey(linkedGuid);
            }
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }

        public void PerformActivity(Guid correlationId, long instanceId, IServiceRequestMetadata metadata, string activityName, bool ignoreWarnings, Dictionary<string, string> mapVariables = null, object data = null)
        {
            //perform start activity request
            var userStartActivityRequest = new X2RequestForExistingInstance(correlationId, instanceId, X2RequestType.UserStart, metadata, activityName, ignoreWarnings, mapVariables, data);
            _x2ServiceClient.PerformCommand(userStartActivityRequest);
            var userStartActivityResult = userStartActivityRequest.Result;
            Assert.IsFalse(userStartActivityResult.IsErrorResponse,
                string.Format("Instance: {0} X2 Error Response: {1} with System Messages: {2}", instanceId, userStartActivityResult.Message, string.Join(" | ",
                userStartActivityResult.SystemMessages != null ? userStartActivityResult.SystemMessages.AllMessages.Select(x => x.Message) : new List<string>())));

            //perform complete activity request
            var userCompleteActivityRequest = new X2RequestForExistingInstance(correlationId, instanceId, X2RequestType.UserComplete, metadata, activityName, ignoreWarnings, mapVariables, data);
            _x2ServiceClient.PerformCommand(userCompleteActivityRequest);
            var userCompleteActivityResult = userCompleteActivityRequest.Result;
            var messageList = new List<string>();
            Assert.IsFalse(userCompleteActivityResult.IsErrorResponse,
                string.Format("Instance: {0} X2 Error Response: {1} with System Messages: {2}", instanceId, userCompleteActivityResult.Message, string.Join(" | ",
                userCompleteActivityResult.SystemMessages != null ? userCompleteActivityResult.SystemMessages.AllMessages.Select(x => x.Message) : new List<string>())));
        }

        public IServiceRequestMetadata GetHeaderMetadataForUser(string userName)
        {
            var metadataDictionary = new ServiceRequestMetadata();
            var user = TestApiClient.Get<HaloUsersDataModel>(new { ADUserName = userName }).FirstOrDefault();
            var orgStructureKey = user.UserOrganisationStructureKey;
            var userCapabilities = user.Capabilities;
            metadataDictionary.Add(ServiceRequestMetadata.HEADER_USERORGANISATIONSTRUCTUREKEY, orgStructureKey.ToString());
            metadataDictionary.Add(ServiceRequestMetadata.HEADER_CURRENTUSERCAPABILITIES, userCapabilities);
            metadataDictionary.Add(ServiceRequestMetadata.HEADER_USERNAME, string.Concat(@"SAHL\", user.ADUserName));
            return metadataDictionary;
        }
    }
}