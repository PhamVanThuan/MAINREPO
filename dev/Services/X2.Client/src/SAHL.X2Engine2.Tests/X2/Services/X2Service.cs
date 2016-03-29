using System;
using System.Configuration;
using NUnit.Framework;
using SAHL.Config.Services.X2.Client;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Tests.X2.Models;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Tests.X2.Services
{
    public sealed class X2Service : IX2Service
    {
        private X2ServiceClient client;
        ServiceRequestMetadata metadata = null;

        public X2Service()
        {
            var url = ConfigurationManager.AppSettings["X2WebHost_Url"].ToString();
            IJsonActivator jsonActivator = new JsonActivator();
            IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration(url);
            this.client = new X2ServiceClient(serviceConfiguration);
        }

        public X2Case CreateWorkflowInstance(X2ProcessWorkflow process, string requestByUser, string activityName)
        {

            var serviceRequestMetadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, requestByUser }
                            }); ;
            var request = new X2CreateInstanceRequest(Guid.NewGuid(), activityName, process.Process, process.Workflow, serviceRequestMetadata, false);
            var response = this.MakeX2WebRequest(request, metadata);
            var createCompleteRequest = new X2RequestForExistingInstance(Guid.NewGuid(), response.InstanceId, X2RequestType.CreateComplete, serviceRequestMetadata, activityName, false);
            response = this.MakeX2WebRequest(createCompleteRequest, metadata);
            return new X2Case(response.InstanceId, null, null, null);
        }

        public void RaiseExternalFlag(X2ProcessWorkflow process, long instanceId, X2ExternalActivity x2ExternalActivity)
        {
            var serviceRequestMetadata = new ServiceRequestMetadata();
            var raiseExternalFlag = new X2ExternalActivityRequest(Guid.NewGuid(), instanceId, 0, x2ExternalActivity.ID, instanceId, process.WorkflowId, serviceRequestMetadata);
            this.MakeX2WebRequest(raiseExternalFlag, metadata);
        }

        private X2Response MakeX2WebRequest(X2Request request, ServiceRequestMetadata metadata)
        {
            var messageCollection = client.PerformCommand(request);
            var errorResponse = request.Result as X2ErrorResponse;
            if (errorResponse != null)
                Assert.Fail(errorResponse.Message);
            else if (request.Result != null && request.Result.Message.Contains("stack trace"))
                Assert.Fail(request.Result.Message);
            return request.Result;
        }

        public void Dispose()
        {
        }

        
    }
}