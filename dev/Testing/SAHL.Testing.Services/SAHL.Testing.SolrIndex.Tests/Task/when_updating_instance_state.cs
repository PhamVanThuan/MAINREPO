using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Core.X2.Messages;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Services.Interfaces.X2;
using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Testing.Constants.Workflows;

namespace SAHL.Testing.SolrIndex.Tests.Task
{
    public class when_updating_instance_state : SolrIndexTest
    {
        [Test]
        public void it_should_update_the_instance_state_in_the_task_index()
        {
            //find invoice in task index
            searchFilters.Add(new SearchFilter("Workflow", "Third Party Invoices"));
            searchFilters.Add(new SearchFilter("State", ThirdPartyInvoices.States.LossControlInvoiceReceived));
            var setupTaskQuery = new SearchForTaskQuery("*", searchFilters, "Task");
            _searchService.PerformQuery(setupTaskQuery);
            var setupTaskQueryResult = setupTaskQuery.Result.Results.FirstOrDefault();

            //setup x2 request data
            var serviceConfiguration = GetInstance <IServiceUrlConfiguration>();
            var x2ServiceClient = GetInstance<IX2Service>();

            var instanceId = setupTaskQueryResult.InstanceId;
            Guid correlationId = Guid.NewGuid();
            var activityName = "Reject Invoice";
            metadata.Add(SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, "X2");
            var mapVariables = new Dictionary<string, string>();
            mapVariables = new Dictionary<string, string>();
            mapVariables.Add("ThirdPartyInvoiceKey", setupTaskQueryResult.GenericKeyValue);
            object data = "SAHL.Testing.SolrIndex.Tests.Task.when_updating_instance_state Test";

            //perform start activity request
            var userStartActivityRequest = new X2RequestForExistingInstance(correlationId, instanceId, X2RequestType.UserStart, metadata, activityName, false, mapVariables, data);
            x2ServiceClient.PerformCommand(userStartActivityRequest);
            var userStartActivityResult = userStartActivityRequest.Result;
            Assert.IsFalse(userStartActivityResult.IsErrorResponse, 
                string.Format("X2 Error Response: {0} with System Messages: {1}", userStartActivityResult.Message, string.Join(" | ",
                userStartActivityResult.SystemMessages != null ? userStartActivityResult.SystemMessages.AllMessages.Select(x => x.Message) : new List<string>())));

            //perform complete activity request
            var userCompleteActivityRequest = new X2RequestForExistingInstance(correlationId, instanceId, X2RequestType.UserComplete, metadata, activityName, false, mapVariables, data);
            x2ServiceClient.PerformCommand(userCompleteActivityRequest);
            var userCompleteActivityResult = userCompleteActivityRequest.Result;
            var messageList = new List<string>();
            Assert.IsFalse(userCompleteActivityResult.IsErrorResponse,
                string.Format("X2 Error Response: {0} with System Messages: {1}", userCompleteActivityResult.Message, string.Join(" | ",
                userCompleteActivityResult.SystemMessages != null ? userCompleteActivityResult.SystemMessages.AllMessages.Select(x => x.Message) : new List<string>())));

            //assert that the instance state was updated in the solr index
            searchFilters.Clear();
            searchFilters.Add(new SearchFilter("State", ThirdPartyInvoices.States.InvoiceRejected));
            var assertionTaskQueryResults = SearchTaskIndexByInstanceId(instanceId, ThirdPartyInvoices.States.InvoiceRejected, searchFilters);
            Assert.AreEqual(1, assertionTaskQueryResults.Where(x => x.GenericKeyValue == setupTaskQueryResult.GenericKeyValue
                && x.GenericKeyTypeKey == Convert.ToString((int)GenericKeyType.ThirdPartyInvoice)
                && x.UserName == ""
                && x.Attribute1Value == setupTaskQueryResult.Attribute1Value
                && x.Attribute3Value == setupTaskQueryResult.Attribute3Value).Count(),
                string.Format("Expected State of Instance: {0} to be updated to Invoice Rejected", instanceId));
        }
    }
}