using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using System;
using System.Collections.Generic;

namespace SAHL.Testing.Workflow.Tests.ThirdPartyInvoices
{
    internal class Example : WorkflowActivityTest
    {
        [Test]
        public void it_should_update_the_instance_state_in_the_task_index()
        {
            var setupTaskQueryResult = TestApiClient.GetAny<OpenThirdPartyInvoicesDataModel>(new { WorkflowState = "Accepted for Processing", InvoiceStatusKey = (int)InvoiceStatus.Received });

            //setup x2 request data

            var instanceId = setupTaskQueryResult.InstanceID;
            Guid correlationId = Guid.NewGuid();
            var activityName = "Reject Invoice";
            var metadata = GetHeaderMetadataForUser("InvoiceProcessor");
            var mapVariables = new Dictionary<string, string>();
            mapVariables = new Dictionary<string, string>();
            mapVariables.Add("ThirdPartyInvoiceKey", setupTaskQueryResult.ThirdPartyInvoiceKey.ToString());
            object data = "SAHL.Testing.SolrIndex.Tests.Task.when_updating_instance_state Test";

            PerformActivity(correlationId, instanceId, metadata, activityName, false, mapVariables, data);
        }
    }
}