using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Testing.Common;
using SAHL.Testing.Common.Helpers;
using SAHL.Testing.Common.Models;
using System;
using System.Linq;
using SAHL.Core.Testing.Constants.Workflows;
using System.Collections.Generic;
using System.Diagnostics;

namespace SAHL.Testing.SolrIndex.Tests.Task
{
    public class when_creating_a_new_instance : SolrIndexTest
    {
        private OpenMortgageLoanAccountsDataModel openMortgageLoanAccount;

        [TearDown]
        public void TearDown()
        {
            if (openMortgageLoanAccount != null)
            {
                metadata.Clear();
                var command = new RemoveAccountFromOpenMortgageLoanAccountsCommand(openMortgageLoanAccount.AccountKey);
                _feTestClient.PerformCommand(command, metadata);
            }
        }

        [Test]
        public void it_should_add_a_record_to_the_task_index()
        {
            //create new thirdpartyinvoice instance
            var _financeDomainClient = GetInstance<IFinanceDomainServiceClient>();
            openMortgageLoanAccount = TestApiClient.GetAny<OpenMortgageLoanAccountsDataModel>(new { HasThirdPartyInvoice = false }, 1000);
            var file = Convert.ToBase64String(ResourcesHelper.GetResourceBytes("pdf-sample.pdf"));
            var invoiceDate = DateTime.Now;
            var sahlReference = "Test-Ref-" + Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var recievedFromEmailAddress = "halouser@sahomeloans.com";
            var invoiceDocument = new AttorneyInvoiceDocumentModel(openMortgageLoanAccount.AccountKey.ToString(), invoiceDate, invoiceDate,
                recievedFromEmailAddress, sahlReference, "pdf-sample", "pdf", "Invoice", file);

            metadata.Add("DomainProcessId", CombGuid.Instance.Generate().ToString());
            metadata.Add("CommandCorrelationId", CombGuid.Instance.Generate().ToString());
            var command = new AcceptThirdPartyInvoiceCommand(openMortgageLoanAccount.AccountKey, invoiceDocument, (int)ThirdPartyType.Attorney);
            _financeDomainClient.PerformCommand(command, metadata).WithoutMessages();

            //find latest thirdpartyinvoice details and instance details
            var thirdPartyInvoices = TestApiClient.Get<ThirdPartyInvoiceDataModel>(new { AccountKey = openMortgageLoanAccount.AccountKey });
            var thirdPartyInvoice = thirdPartyInvoices.OrderByDescending<ThirdPartyInvoiceDataModel, int>(x => x.ThirdPartyInvoiceKey).FirstOrDefault();

            IEnumerable<ThirdPartyInvoicesInstanceDataModel> instances = new List<ThirdPartyInvoicesInstanceDataModel>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromSeconds(30))
            {
                instances = TestApiClient.Get<ThirdPartyInvoicesInstanceDataModel>(new { GenericKey = thirdPartyInvoice.ThirdPartyInvoiceKey });
                if (instances.Count() > 0)
                {
                    sw.Stop();
                    break;
                }
            }
            var instance = instances.OrderByDescending<ThirdPartyInvoicesInstanceDataModel, long>(x => x.InstanceId).FirstOrDefault();

            //check that the instance is added to the task index
            var taskQueryResults = SearchTaskIndexByInstanceId(instance.InstanceId, ThirdPartyInvoices.States.LossControlInvoiceReceived);
            Assert.AreEqual(1, taskQueryResults.Where(x => x.GenericKeyValue == thirdPartyInvoice.ThirdPartyInvoiceKey.ToString()
                && x.GenericKeyTypeKey == Convert.ToString((int)GenericKeyType.ThirdPartyInvoice)
                && x.UserName == "Loss Control Fee Consultant"
                && x.Attribute1Value == thirdPartyInvoice.SahlReference
                && x.Attribute3Value == openMortgageLoanAccount.AccountKey.ToString()).Count(),
                string.Format("Expected a record to be added to the task index for ThirdPartyInvoiceKey: {0}, AccountKey: {1}", thirdPartyInvoice.ThirdPartyInvoiceKey, openMortgageLoanAccount.AccountKey));
        }
    }
}