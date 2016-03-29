using System.Linq;
using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Core.Data.Models.FETest;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Collections.Generic;
using SAHL.Core.Services;
using System.Threading;
using System.Diagnostics;

namespace SAHL.Testing.Services.Tests.FinanceDomain
{
    [TestFixture]
    public class when_querying_an_invoice : ServiceTestBase<IFinanceDomainServiceClient>
    {
        private int _thirdPartyInvoiceKey = 0;
        private EmptyThirdPartyInvoicesDataModel emptyInvoice;

        [SetUp]
        public void OnTestSetup()
        {
            emptyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
            _thirdPartyInvoiceKey = emptyInvoice.ThirdPartyInvoiceKey;
            SetHeaderMetadataForUser("InvoiceProcessor");
        }

        [Test]
        public void when_successfully_querying_post_approval()
        {
            var thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney }, 1);
            Guid thirdPartyId = thirdParty.Id;
            decimal lineItemAmount = 100.00M;
            decimal lineItem2Amount = 200.00M;
            List<InvoiceLineItemModel> lineItems = new List<InvoiceLineItemModel>()
            {
                new InvoiceLineItemModel(null, _thirdPartyInvoiceKey, 1, lineItemAmount, true),
                new InvoiceLineItemModel(null, _thirdPartyInvoiceKey, 1, lineItem2Amount, false)
            };
            
            DateTime invoiceDate = DateTime.Now;
            string paymentReference = "Query integration test";
            ThirdPartyInvoiceModel invoice = new ThirdPartyInvoiceModel(_thirdPartyInvoiceKey, thirdPartyId, string.Concat("SAHL-InvNo-", Convert.ToBase64String(Guid.NewGuid().ToByteArray())),
                invoiceDate, lineItems, false, paymentReference);
            var command = new CaptureThirdPartyInvoiceCommand(invoice);
            Execute(command).WithoutErrors();

            var legalEntity = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = thirdParty.LegalEntityKey });
            AttorneyInvoiceMonthlyBreakdownDataModel projectionPreApproval = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = legalEntity.FirstOrDefault().RegisteredName }).FirstOrDefault();
            int processedPreApproval = projectionPreApproval == null ? 0 : projectionPreApproval.Processed;
            int unprocessedPreApproval = projectionPreApproval == null ? 0 : projectionPreApproval.Unprocessed;

            var approveCommand = new ApproveThirdPartyInvoiceCommand(_thirdPartyInvoiceKey);
            Execute(approveCommand).WithoutErrors();

            AttorneyInvoiceMonthlyBreakdownDataModel projectionPostApproval = null;
            int processedPostApproval = 0;
            int unprocessedPostApproval = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromSeconds(10))
            {
                projectionPostApproval = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = legalEntity.FirstOrDefault().RegisteredName }).FirstOrDefault();
                processedPostApproval = projectionPostApproval == null ? 0 : projectionPostApproval.Processed;
                unprocessedPostApproval = projectionPostApproval == null ? 0 : projectionPostApproval.Unprocessed;
                if (processedPostApproval == processedPreApproval + 1 && unprocessedPostApproval == unprocessedPreApproval -1)
                {
                    break;
                }
            }
            sw.Stop();

            var queryCommand = new QueryThirdPartyInvoiceCommand(_thirdPartyInvoiceKey, "Query integration test");
            Execute(queryCommand).WithoutErrors();
            
            AttorneyInvoiceMonthlyBreakdownDataModel projectionPostQuery = null;
            int processedPostQuery = 0;
            int unprocessedPostQuery = 0;
            sw.Reset();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromSeconds(10))
            {
                projectionPostQuery = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = legalEntity.FirstOrDefault().RegisteredName }).FirstOrDefault();
                processedPostQuery = projectionPostQuery == null ? 0 : projectionPostQuery.Processed;
                unprocessedPostQuery = projectionPostQuery == null ? 0 : projectionPostQuery.Unprocessed;
                if (processedPostQuery == processedPostApproval - 1 && unprocessedPostQuery == unprocessedPostApproval + 1)
                {
                    break;
                }
            }
            sw.Stop();

            //Since the invoice has been approved, it has been flagged as processed
            Assert.That(processedPostQuery == processedPostApproval - 1,
                "Processed count should have been decremented when querying ThirdPartyInvoice {0} post approval", _thirdPartyInvoiceKey);
            Assert.That(unprocessedPostQuery == unprocessedPostApproval + 1,
                "Unprocessed count should have been incremented when querying ThirdPartyInvoice {0} post approval", _thirdPartyInvoiceKey);
        }

        [Test]
        public void when_successfully_querying_pre_approval()
        {
            var thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney }, 1);
            Guid thirdPartyId = thirdParty.Id;
            decimal lineItemAmount = 100.00M;
            decimal lineItem2Amount = 200.00M;
            List<InvoiceLineItemModel> lineItems = new List<InvoiceLineItemModel>()
            {
                new InvoiceLineItemModel(null, _thirdPartyInvoiceKey, 1, lineItemAmount, true),
                new InvoiceLineItemModel(null, _thirdPartyInvoiceKey, 1, lineItem2Amount, false)
            };

            DateTime invoiceDate = DateTime.Now;
            string paymentReference = "Query integration test";
            ThirdPartyInvoiceModel invoice = new ThirdPartyInvoiceModel(_thirdPartyInvoiceKey, thirdPartyId, string.Concat("SAHL-InvNo-", Convert.ToBase64String(Guid.NewGuid().ToByteArray())),
                invoiceDate, lineItems, false, paymentReference);
            
            var legalEntity = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = thirdParty.LegalEntityKey });
            AttorneyInvoiceMonthlyBreakdownDataModel projectionPreCapture = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = legalEntity.FirstOrDefault().RegisteredName }).FirstOrDefault();
            int unprocessedPreCapture = projectionPreCapture == null ? 0 : projectionPreCapture.Unprocessed;

            var command = new CaptureThirdPartyInvoiceCommand(invoice);
            Execute(command).WithoutErrors();
                        
            AttorneyInvoiceMonthlyBreakdownDataModel projectionPostCapture = null;
            int processedPostCapture = 0;
            int unprocessedPostCapture = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromSeconds(10))
            {
                projectionPostCapture = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = legalEntity.FirstOrDefault().RegisteredName }).FirstOrDefault();
                processedPostCapture = projectionPostCapture == null ? 0 : projectionPostCapture.Processed;
                unprocessedPostCapture = projectionPostCapture == null ? 0 : projectionPostCapture.Unprocessed;
                if (unprocessedPostCapture == unprocessedPreCapture + 1)
                {
                    break;
                }
            }
            sw.Stop();

            var queryCommand = new QueryThirdPartyInvoiceCommand(_thirdPartyInvoiceKey, "Query integration test");
            Execute(queryCommand).WithoutErrors();

            AttorneyInvoiceMonthlyBreakdownDataModel projectionPostQuery = null;
            int processedPostQuery = 0;
            int unprocessedPostQuery = 0;
            sw.Reset();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromSeconds(10))
            {
                projectionPostQuery = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = legalEntity.FirstOrDefault().RegisteredName }).FirstOrDefault();
                processedPostQuery = projectionPostQuery == null ? 0 : projectionPostQuery.Processed;
                unprocessedPostQuery = projectionPostQuery == null ? 0 : projectionPostQuery.Unprocessed;
                if (unprocessedPostCapture == unprocessedPreCapture - 1)
                {
                    break;
                }
            }
            sw.Stop();

            //Since the invoice has not been approved, it is not marked as processed
            Assert.That(processedPostQuery == processedPostCapture, "Processed count should not be decremented when querying ThirdPartyInvoice {0} pre approval", _thirdPartyInvoiceKey);
            Assert.That(unprocessedPostQuery == unprocessedPostCapture, "Unprocessed count should not be decremented when querying ThirdPartyInvoice {0} pre approval", _thirdPartyInvoiceKey);
        }

        [TearDown]
        public void OnTestTearDown()
        {
            if (emptyInvoice != null)
            {
                var command = new RemoveEmptyThirdPartyInvoiceCommand(_thirdPartyInvoiceKey);
                PerformCommand(command);
                _thirdPartyInvoiceKey = 0;
                emptyInvoice = null;
            }
        }

    }
}