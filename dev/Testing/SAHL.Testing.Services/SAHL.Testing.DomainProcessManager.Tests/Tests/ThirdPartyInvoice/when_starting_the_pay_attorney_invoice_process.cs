using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.EventStore;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.DomainProcessManagerProxy.Commands;
using SAHL.Services.Interfaces.DomainProcessManagerProxy.Models;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Enum;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Testing.Common;
using SAHL.Testing.Common.Helpers;
using SAHL.Testing.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SAHL.Testing.DomainProcessManager.Tests.Tests.ThirdPartyInvoice
{
    [TestFixture]
    public class when_starting_the_pay_attorney_invoice_process : DomainProcessManagerTest
    {
        private IFinanceDomainServiceClient _finClient;
        private ICATSServiceClient _catsClient;

        [TestFixtureSetUp]
        new public void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            _finClient = base.GetInstance<IFinanceDomainServiceClient>();
            _catsClient = base.GetInstance<ICATSServiceClient>();
            IServiceRequestMetadata metadata = new ServiceRequestMetadata();

            var todaysCATSPaymentBatches = TestApiClient.Get<CATSPaymentBatchDataModel>(new { CATSPaymentBatchTypeKey = (int)CATSPaymentBatchType.ThirdPartyInvoice })
                .Where(x => x.CreatedDate > testFixtureStartTime.Subtract(testFixtureStartTime.TimeOfDay));

            foreach (var catsPaymentBatch in todaysCATSPaymentBatches)
            {
                catsPaymentBatch.CreatedDate = testFixtureStartTime.AddDays(-1);
                var updateCATSPaymentBatchCommand = new UpdateCATSPaymentBatchCommand(catsPaymentBatch);
                _feTestClient.PerformCommand(updateCATSPaymentBatchCommand, metadata).WithoutMessages();
            }

            var files = new List<GetFileNamesOfFilesInDirectoryQueryResult>();

            var getNamesOfFilesInDirectoryQuery = new GetFileNamesOfFilesInDirectoryQuery(CatsAppConfigSettings.CATSOutputFileLocation, "*", System.IO.SearchOption.TopDirectoryOnly);
            _feTestClient.PerformQuery(getNamesOfFilesInDirectoryQuery).WithoutMessages();
            files.AddRange(getNamesOfFilesInDirectoryQuery.Result.Results);

            getNamesOfFilesInDirectoryQuery = new GetFileNamesOfFilesInDirectoryQuery(CatsAppConfigSettings.CATSFailureFileLocation, "*", System.IO.SearchOption.TopDirectoryOnly);
            _feTestClient.PerformQuery(getNamesOfFilesInDirectoryQuery).WithoutMessages();
            files.AddRange(getNamesOfFilesInDirectoryQuery.Result.Results);

            foreach (var file in files)
            {
                var removeFileByPathCommand = new RemoveFileByPathCommand(file.FullName);
                _feTestClient.PerformCommand(removeFileByPathCommand, metadata).WithoutMessages();
            }
        }

        [Test]
        public void given_a_valid_payment_batch_it_should_complete_succesfully()
        {
            IServiceRequestMetadata metadata = new ServiceRequestMetadata();
            Dictionary<string, string> mapVariables = new Dictionary<string, string>();

            int nextFileSequenceNumber = (int)TestApiClient.GetByKey<ControlDataModel>(114).ControlNumeric + 1;

            var thirdParties = TestApiClient.Get<ThirdPartiesDataModel>(new { HasBankAccount = true, GenericKeyTypeKey = (int)GenericKeyType.Attorney }, 2);
            Dictionary<int, ThirdPartyInvoiceDataModel> thirdPartyInvoices = new Dictionary<int, ThirdPartyInvoiceDataModel>();

            foreach (var thirdParty in thirdParties)
            {
                //Create case
                var openMortgageLoanAccount = TestApiClient.GetAny<OpenMortgageLoanAccountsDataModel>(new { HasThirdPartyInvoice = false }, 1000);
                var removeAccountFromOpenMortgageLoanAccountsCommand = new RemoveAccountFromOpenMortgageLoanAccountsCommand(openMortgageLoanAccount.AccountKey);
                _feTestClient.PerformCommand(removeAccountFromOpenMortgageLoanAccountsCommand,metadata).WithoutMessages();

                metadata.Clear();
                metadata.Add("DomainProcessId", CombGuid.Instance.Generate().ToString());
                metadata.Add("CommandCorrelationId", CombGuid.Instance.Generate().ToString());
                var file = Convert.ToBase64String(ResourcesHelper.GetResourceBytes("pdf-sample.pdf"));
                var sahlReference = "Test-Ref-" + Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                var recievedFromEmailAddress = "halouser@sahomeloans.com";
                var invoiceDocument = new AttorneyInvoiceDocumentModel(openMortgageLoanAccount.AccountKey.ToString(), testStartTime, testStartTime,
                    recievedFromEmailAddress, sahlReference, "pdf-sample", "pdf", "Invoice", file);
                var acceptThirdPartyInvoiceCommand = new AcceptThirdPartyInvoiceCommand(openMortgageLoanAccount.AccountKey, invoiceDocument, (int)ThirdPartyType.Attorney);
                _finClient.PerformCommand(acceptThirdPartyInvoiceCommand, metadata).WithoutMessages();

                var thirdPartyInvoice = TestApiClient.Get<ThirdPartyInvoiceDataModel>(new { AccountKey = openMortgageLoanAccount.AccountKey })
                    .OrderByDescending(x => x.ThirdPartyId)
                    .FirstOrDefault();

                long instanceId = WaitForThirdPartyInvoiceInstance(thirdPartyInvoice.ThirdPartyInvoiceKey, "Loss Control Invoice Received");
                
                //accept invoice
                metadata = base.GetHeaderMetadataForUser("InvoiceProcessor");

                mapVariables.Clear();
                mapVariables.Add("GenericKey", thirdPartyInvoice.ThirdPartyInvoiceKey.ToString());
                base.PerformActivity(CombGuid.Instance.Generate(), instanceId, metadata, "Accept Invoice", true, mapVariables);

                //capture invoice
                List<InvoiceLineItemModel> lineItems = new List<InvoiceLineItemModel>()
                {
                    new InvoiceLineItemModel(null, thirdPartyInvoice.ThirdPartyInvoiceKey, 1, 100.00M, true),
                    new InvoiceLineItemModel(null, thirdPartyInvoice.ThirdPartyInvoiceKey, 2, 200.00M, false)
                };

                var invoice = new ThirdPartyInvoiceModel(thirdPartyInvoice.ThirdPartyInvoiceKey, thirdParty.Id, string.Concat("SAHL-InvNo-", Convert.ToBase64String(Guid.NewGuid().ToByteArray())),
                    DateTime.Now, lineItems, true, string.Format("AutomatedTest-PmtRef-{0}", DataGenerator.RandomInt(1, 999)));

                var captureThirdPartyInvoiceCommand = new CaptureThirdPartyInvoiceCommand(invoice);
                _finClient.PerformCommand<CaptureThirdPartyInvoiceCommand>(captureThirdPartyInvoiceCommand, new ServiceRequestMetadata(metadata)).WithoutMessages();

                base.PerformActivity(CombGuid.Instance.Generate(), instanceId, metadata, "Capture Invoice", true);

                //approve for payment
                metadata = base.GetHeaderMetadataForUser("InvoiceProcessor");

                mapVariables.Clear();
                mapVariables.Add("ThirdPartyInvoiceKey", thirdPartyInvoice.ThirdPartyInvoiceKey.ToString());
                base.PerformActivity(CombGuid.Instance.Generate(), instanceId, metadata, "Approve for Payment", true);

                thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(thirdPartyInvoice.ThirdPartyInvoiceKey);
                thirdPartyInvoices.Add(thirdParty.LegalEntityKey, thirdPartyInvoice);
            }

            //because of
            var thirdPartyPayments = new List<ThirdPartyPaymentModel>();
            foreach (var tpi in thirdPartyInvoices)
            {
                long instanceId = Convert.ToInt64(TestApiClient.Get<ThirdPartyInvoicesInstanceDataModel>(new { GenericKey = tpi.Value.ThirdPartyInvoiceKey }).FirstOrDefault().InstanceId);
                thirdPartyPayments.Add(new ThirdPartyPaymentModel(tpi.Value.ThirdPartyInvoiceKey, instanceId, tpi.Value.AccountKey, tpi.Value.SahlReference));
            }

            metadata = GetHeaderMetadataForUser("InvoicePmtProcessor");

            var startPayAttorneyProcessCommand = new StartPayAttorneyProcessCommand(thirdPartyPayments);
            _dpmClient.PerformCommand(startPayAttorneyProcessCommand, metadata).WithoutMessages();

            var catsPaymentBatch = WaitForCATSPaymentBatch(nextFileSequenceNumber);

            //It should raise CATSPaymentBatchRecipientsNotifiedEvent
            WaitForEvent(catsPaymentBatch.CATSPaymentBatchKey, GenericKeyType.CATSPaymentBatch, "CATSPaymentBatchRecipientsNotified");

            //It should raise SummarisedPaymentsToRecipientEvent per legal entity
            foreach (var thirdParty in thirdParties)
            {
                var summarisedPaymentsToRecipientEvents = TestApiClient.Get<EventDataModel>(new { GenericKey = thirdParty.LegalEntityKey, GenericKeyTypeKey = (int)GenericKeyType.LegalEntity });
                Assert.AreEqual(1, summarisedPaymentsToRecipientEvents.Where(x => x.EventInsertDate > testStartTime && x.Data.Contains("SummarisedPaymentsToRecipient")).Count(), "Expected an event to be raised for Legal Entity: {0}", thirdParty.LegalEntityKey);
            }
            
            //It should post a financial transaction per invoice
            foreach (var tpi in thirdPartyInvoices)
            {
                var financialService = TestApiClient.Get<FinancialServiceDataModel>(new { AccountKey = tpi.Value.AccountKey, FinancialServiceTypeKey = 1 }).FirstOrDefault();
                var query = new GetFinancialTransactionsQuery(financialService.FinancialServiceKey);
                _feTestClient.PerformQuery(query);
                var financialTransactions = query.Result.Results;
                int loanTransactionType = (bool)tpi.Value.CapitaliseInvoice ? (int)LoanTransactionTypeEnum.CapitalisedLegalFeeTransaction : (int)LoanTransactionTypeEnum.NonCapitalisedLegalFeeTransaction;
                CollectionAssert.IsNotEmpty(financialTransactions.Where(x => x.TransactionTypeKey == loanTransactionType
                    && x.Amount == Convert.ToDecimal(tpi.Value.TotalAmountIncludingVAT) && x.Reference == tpi.Value.SahlReference));
            }

            var getNamesOfFilesInDirectoryQuery = new GetFileNamesOfFilesInDirectoryQuery(CatsAppConfigSettings.CATSOutputFileLocation, "*", System.IO.SearchOption.TopDirectoryOnly);
            _feTestClient.PerformQuery(getNamesOfFilesInDirectoryQuery).WithoutMessages();
            Assert.AreEqual(1, getNamesOfFilesInDirectoryQuery.Result.Results.Where(x => x.FileName == catsPaymentBatch.CATSFileName).Count(),
                "Expected a file named {0} to be in the {1} directory for CATSPaymentBatch {2}",
                catsPaymentBatch.CATSFileName,
                CatsAppConfigSettings.CATSOutputFileLocation,
                catsPaymentBatch.CATSPaymentBatchKey);
            
        }

        private long WaitForThirdPartyInvoiceInstance(int thirdPartyInvoiceKey, string state)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var query = new GetThirdPartyInvoicesInstanceDetailsQuery(thirdPartyInvoiceKey);
            while (sw.Elapsed < TimeSpan.FromSeconds(120))
            {
                _feTestClient.PerformQuery(query).WithoutMessages();
                if (query.Result.Results.Where(x=>x.StateName == state).Count() > 0)
                {
                    break;
                }
            }
            sw.Stop();
            Assert.AreEqual(1, query.Result.Results.Where(x => x.StateName == state).Count(),
                 string.Format(@"Check for Third Party Invoice Instance failed when checking for ThirdPartyInvoice {0} at the {1} state", thirdPartyInvoiceKey, state));
            return query.Result.Results.Where(x => x.StateName == state).FirstOrDefault().InstanceID;
        }
        
        private EventDataModel WaitForEvent(int genericKey, GenericKeyType genericKeyType, string eventName)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IEnumerable<EventDataModel> events = new List<EventDataModel>();
            while (sw.Elapsed < TimeSpan.FromSeconds(120))
            {

                events = TestApiClient.Get<EventDataModel>(new { GenericKey = genericKey, GenericKeyTypeKey = (int)genericKeyType });

                if (events.Where(x => x.EventInsertDate > testStartTime && x.Data.Contains(eventName)).Count() > 0)
                {
                    break;
                }
            }
            sw.Stop();
            Assert.AreEqual(1, events.Where(x => x.EventInsertDate > testStartTime && x.Data.Contains(eventName)).Count(), "Expected an {0} event to be raised for CATS Payment Batch: {1} with Event Start Date > {2}", eventName, genericKey, testStartTime);
            return events.Where(x => x.EventInsertDate > testStartTime && x.Data.Contains(eventName)).FirstOrDefault();
        }

        private CATSPaymentBatchDataModel WaitForCATSPaymentBatch(int nextFileSequenceNumber)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IEnumerable<CATSPaymentBatchDataModel> catsPaymentBatch = new List<CATSPaymentBatchDataModel>();
            while (sw.Elapsed < TimeSpan.FromSeconds(120))
            {

                catsPaymentBatch = TestApiClient.Get<CATSPaymentBatchDataModel>(new { CATSFileSequenceNo = nextFileSequenceNumber });

                if (catsPaymentBatch.Count() > 0)
                {
                    break;
                }
            }
            sw.Stop();
            Assert.AreEqual(1, catsPaymentBatch.Count(), "Expected a CATS Payment Batch to be created and linked to FileSequenceNumber: {0}", nextFileSequenceNumber);
            return catsPaymentBatch.FirstOrDefault();
        }
    }
}