using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.EventStore;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Testing.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SAHL.Testing.Common.Helpers;

namespace SAHL.Testing.Services.Tests.FinanceDomain
{
    [TestFixture]
    public class when_accepting_a_third_party_invoice : ServiceTestBase<IFinanceDomainServiceClient>
    {
        private static OpenMortgageLoanAccountsDataModel openMortgageLoanAccount;

        [TearDown]
        new public void OnTestTearDown()
        {
            if (openMortgageLoanAccount != null)
            {
                var command = new RemoveAccountFromOpenMortgageLoanAccountsCommand(openMortgageLoanAccount.AccountKey);
                PerformCommand(command);
                openMortgageLoanAccount = null;
            }
            base.OnTestTearDown();
        }

        [Test, Repeat(5)]
        public void given_a_valid_invoice()
        {
            _metaDataDictionary.Add("DomainProcessId", CombGuid.Instance.Generate().ToString());
            _metaDataDictionary.Add("CommandCorrelationId", CombGuid.Instance.Generate().ToString());
            openMortgageLoanAccount = TestApiClient.GetAny<OpenMortgageLoanAccountsDataModel>(new { HasThirdPartyInvoice = false }, 1000);
            var accountKey = openMortgageLoanAccount.AccountKey;
            var file = Convert.ToBase64String(ResourcesHelper.GetResourceBytes("pdf-sample.pdf"));
            var invoiceDate = DateTime.Now;
            var sahlReference = "Test-Ref-" + Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var recievedFromEmailAddress = "halouser@sahomeloans.com";
            var invoiceDocument = new AttorneyInvoiceDocumentModel(accountKey.ToString(), invoiceDate, invoiceDate,
                recievedFromEmailAddress, sahlReference, "pdf-sample", "pdf", "Invoice", file);
            var command = new AcceptThirdPartyInvoiceCommand(accountKey, invoiceDocument, (int)ThirdPartyType.Attorney);
            base.Execute(command).WithoutErrors();
            WaitForEmptyThirdPartyInvoice(accountKey, recievedFromEmailAddress, invoiceDate);
        }

        private void WaitForEmptyThirdPartyInvoice(int accountKey, string receivedFromEmailAddress, DateTime invoiceDate)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IEnumerable<ThirdPartyInvoiceDataModel> thirdPartyInvoices = new List<ThirdPartyInvoiceDataModel>();
            while (sw.Elapsed < TimeSpan.FromSeconds(60))
            {
                thirdPartyInvoices = TestApiClient.Get<ThirdPartyInvoiceDataModel>(new { AccountKey = accountKey, ReceivedFromEmailAddress = receivedFromEmailAddress });
                if (thirdPartyInvoices.Count() > 0)
                {
                    break;
                }
            }
            sw.Stop();
            var thirdPartyInvoice = thirdPartyInvoices.OrderByDescending(x => x.ThirdPartyInvoiceKey).First();
            Assert.That(thirdPartyInvoice.ReceivedDate>= invoiceDate, 
                 string.Format(@"Check for empty invoice assertion failed when checking for ThirdPartyInvoice record where AccountKey = {0}, ReceivedFromEmailAddress = {1} and ReceivedDate > {2}", 
                 accountKey, receivedFromEmailAddress, invoiceDate));
            var invoiceLineItems = TestApiClient.Get<InvoiceLineItemDataModel>(new { ThirdPartyInvoiceKey = thirdPartyInvoice.ThirdPartyInvoiceKey });
            Assert.AreEqual(0, invoiceLineItems.Count(),
                string.Format(@"Check for empty invoice assertion failed when checking for InvoiceLineItem records where ThirdPartyInvoiceKey = {0}", thirdPartyInvoice.ThirdPartyInvoiceKey));
        }
    }
}