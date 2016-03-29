using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SAHL.Testing.Services.Tests.FinanceDomain
{
    [TestFixture]
    public class when_approving_third_party_invoice : ServiceTestBase<IFinanceDomainServiceClient>
    {
        private static ThirdPartyInvoiceDataModel _thirdPartyInvoice;

        [SetUp]
        public void TestSetup()
        {
            var invoiceProcessor = TestApiClient.Get<HaloUsersDataModel>(new { ADUserName = "InvoiceProcessor" }).FirstOrDefault();
            var orgStructureKey = invoiceProcessor.UserOrganisationStructureKey;
            var userCapabilities = invoiceProcessor.Capabilities;
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERORGANISATIONSTRUCTUREKEY, orgStructureKey.ToString());
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_CURRENTUSERCAPABILITIES, userCapabilities);
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERNAME, string.Concat(@"SAHL\", invoiceProcessor.ADUserName));
        }

        [TearDown]
        public void TestTearDown()
        {
            if (_thirdPartyInvoice != null)
            {
                var command = new RemoveEmptyThirdPartyInvoiceCommand(_thirdPartyInvoice.ThirdPartyInvoiceKey);
                base.PerformCommand(command);
                _thirdPartyInvoice = null;
            }
        }

        [Test]
        public void given_invoice_total_is_less_than_users_mandate()
        {
            SetupAwaitingApprovalInvoice(10000M);
            var command = new ApproveThirdPartyInvoiceCommand(_thirdPartyInvoice.ThirdPartyInvoiceKey);
            base.Execute(command).WithoutErrors();
            var result = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(_thirdPartyInvoice.ThirdPartyInvoiceKey);
            Assert.AreEqual((int)InvoiceStatus.Approved, result.InvoiceStatusKey, "ThirdPartyInvoiceStatus of ThirdPartyInvoice: {0} was not updated to {1}",
                _thirdPartyInvoice.ThirdPartyInvoiceKey, InvoiceStatus.Approved.ToString());
        }

        [Test]
        public void given_domain_rule_fails_should_return_messages()
        {
            var emptyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
            _thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(emptyInvoice.ThirdPartyInvoiceKey);
            ThirdPartyInvoiceDataModel invoice = new ThirdPartyInvoiceDataModel(_thirdPartyInvoice.ThirdPartyInvoiceKey, "SAHL_Test", (int)InvoiceStatus.Received, _thirdPartyInvoice.AccountKey,
                null, Convert.ToBase64String(Guid.NewGuid().ToByteArray()), DateTime.Now.AddDays(-1), "clintons@sahomeloans.com", 0, 0, 0, true, DateTime.Now, string.Empty);
            var updateInvoiceCommand = new UpdateThirdPartyInvoiceCommand(invoice);
            base.PerformCommand(updateInvoiceCommand);
            _thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(_thirdPartyInvoice.ThirdPartyInvoiceKey);
            var command = new ApproveThirdPartyInvoiceCommand(_thirdPartyInvoice.ThirdPartyInvoiceKey);
            base.Execute(command)
                .AndExpectThatErrorMessagesContain("The Third Party who sent the invoice has not been captured.");
        }

        [Test]
        public void when_capabilities_are_not_all_linked_to_org_structure_key()
        {
            SetupAwaitingApprovalInvoice(5000M);
            _metaDataDictionary.Remove(ServiceRequestMetadata.HEADER_CURRENTUSERCAPABILITIES); //remove the default ones
            var userCapabilities = "Invoice Processor,Invoice Approver,Loss Control Fee Consultant,Loss Control Fee Invoice Approver Under R30000";
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_CURRENTUSERCAPABILITIES, userCapabilities);
            var command = new ApproveThirdPartyInvoiceCommand(_thirdPartyInvoice.ThirdPartyInvoiceKey);
            base.Execute(command).AndExpectThatErrorMessagesContain("One or more capabilities are not linked to user");
        }

        [Test]
        public void given_invoice_total_exceeds_users_mandate()
        {
            SetupAwaitingApprovalInvoice(25000M);
            var command = new ApproveThirdPartyInvoiceCommand(_thirdPartyInvoice.ThirdPartyInvoiceKey);
            base.Execute(command)
                .AndExpectThatErrorMessagesContain("Loss Control Fee Invoice Approver Under R15000 cannot approve invoice amount greater than R14999.99.");
            var result = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(_thirdPartyInvoice.ThirdPartyInvoiceKey);
            Assert.AreEqual((int)InvoiceStatus.Received, result.InvoiceStatusKey, "ThirdPartyInvoiceStatus of ThirdPartyInvoice: {0} was incorrectly updated to {1}",
                _thirdPartyInvoice.ThirdPartyInvoiceKey, InvoiceStatus.Approved.ToString());
        }

        [Test]
        public void when_successfully_approving_an_invoice_it_should_update_the_projections()
        {
            SetupAwaitingApprovalInvoice(2500M);
            var thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney, Id = _thirdPartyInvoice.ThirdPartyId }, 1);
            var oldprojection = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyId = thirdParty.Id });
            var oldProjectionresult = oldprojection.FirstOrDefault();

            var command = new ApproveThirdPartyInvoiceCommand(_thirdPartyInvoice.ThirdPartyInvoiceKey);
            base.Execute(command).WithoutErrors();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            AttorneyInvoiceMonthlyBreakdownDataModel projectionAfter = null;
            int processedBefore = oldProjectionresult == null ? 0 : oldProjectionresult.Processed;
            int unprocessedBefore = oldProjectionresult == null ? 0 : oldProjectionresult.Unprocessed;
            int processedAfter = 0;
            int unprocessedAfter = 0;
            while (sw.Elapsed < TimeSpan.FromSeconds(50))
            {
                projectionAfter = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyId = thirdParty.Id }).FirstOrDefault();
                processedAfter = projectionAfter == null ? 0 : projectionAfter.Processed;
                unprocessedAfter = projectionAfter == null ? 0 : projectionAfter.Unprocessed;
                if (processedBefore != processedAfter && unprocessedBefore != unprocessedAfter)
                {
                    break;
                }
            }
            sw.Stop();
            Assert.That(processedAfter == processedBefore + 1, "Processed was not incremeneted when approving ThirdPartyInvoice {0}", _thirdPartyInvoice.ThirdPartyInvoiceKey);
            Assert.That(unprocessedAfter == unprocessedBefore - 1, "Unprocessed was not deceremented when approving ThirdPartyInvoice {0}", _thirdPartyInvoice.ThirdPartyInvoiceKey);
        }

        private void SetupAwaitingApprovalInvoice(decimal totalAmount)
        {
            decimal vatAmount = Math.Round((totalAmount * 14 / 114), 2);
            decimal amountExVat = totalAmount - vatAmount;
            var emptyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
            _thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(emptyInvoice.ThirdPartyInvoiceKey);
            var thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney }, 1);
            Guid thirdPartyId = thirdParty.Id;
            ThirdPartyInvoiceDataModel invoice = new ThirdPartyInvoiceDataModel(_thirdPartyInvoice.ThirdPartyInvoiceKey, "SAHL_Test", (int)InvoiceStatus.Received,
                _thirdPartyInvoice.AccountKey, thirdPartyId, Convert.ToBase64String(Guid.NewGuid().ToByteArray()), DateTime.Now.AddDays(-1), "clintons@sahomeloans.com", amountExVat,
                vatAmount, totalAmount, true, DateTime.Now, string.Empty);
            var updateInvoiceCommand = new UpdateThirdPartyInvoiceCommand(invoice);
            PerformCommand(updateInvoiceCommand);
            var invoiceLineItems = new List<InvoiceLineItemDataModel>() {
                new InvoiceLineItemDataModel(emptyInvoice.ThirdPartyInvoiceKey, 1, amountExVat, true, vatAmount, totalAmount)
            };
            var command = new InsertInvoiceLineItemsCommand(invoiceLineItems);
            PerformCommand(command);
            _thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(_thirdPartyInvoice.ThirdPartyInvoiceKey);
        }
    }
}