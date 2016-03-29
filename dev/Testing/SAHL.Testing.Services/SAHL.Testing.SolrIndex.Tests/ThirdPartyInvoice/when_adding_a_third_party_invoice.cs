using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Testing.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Testing.Common.Helpers;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Testing.SolrIndex.Tests.ThirdPartyInvoice
{
    public class when_adding_a_third_party_invoice : SolrIndexTest
    {
        private static OpenMortgageLoanAccountsDataModel openMortgageLoanAccount;

        [TearDown]
        new public void OnTestTearDown()
        {
            if (openMortgageLoanAccount != null)
            {
                var command = new RemoveAccountFromOpenMortgageLoanAccountsCommand(openMortgageLoanAccount.AccountKey);
                metadata.Clear();
                _feTestClient.PerformCommand(command, metadata);
                openMortgageLoanAccount = null;
            }
        }

        [Test]
        [Ignore]
        public void it_should_add_a_third_party_invoice_to_the_solr_index()
        {
            var _financeClient = base.GetInstance<IFinanceDomainServiceClient>();
            metadata.Add("DomainProcessId", CombGuid.Instance.Generate().ToString());
            metadata.Add("CommandCorrelationId", CombGuid.Instance.Generate().ToString());
            openMortgageLoanAccount = TestApiClient.GetAny<OpenMortgageLoanAccountsDataModel>(new { HasThirdPartyInvoice = false }, 1000);
            var accountKey = openMortgageLoanAccount.AccountKey;
            var file = Convert.ToBase64String(ResourcesHelper.GetResourceBytes("pdf-sample.pdf"));
            var invoiceDate = DateTime.Now;
            var sahlReference = "Test-Ref-" + Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var receivedFromEmailAddress = "halouser@sahomeloans.com";
            var invoiceDocument = new AttorneyInvoiceDocumentModel(accountKey.ToString(), invoiceDate, invoiceDate,
                receivedFromEmailAddress, sahlReference, "pdf-sample", "pdf", "Invoice", file);
            var command = new AcceptThirdPartyInvoiceCommand(accountKey, invoiceDocument, (int)ThirdPartyType.Attorney);
            _financeClient.PerformCommand(command, metadata).WithoutMessages();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IEnumerable<ThirdPartyInvoiceDataModel> thirdPartyInvoices = new List<ThirdPartyInvoiceDataModel>();
            while (sw.Elapsed < TimeSpan.FromSeconds(60))
            {
                thirdPartyInvoices = TestApiClient.Get<ThirdPartyInvoiceDataModel>(new { AccountKey = accountKey, ReceivedFromEmailAddress = receivedFromEmailAddress });
                if (thirdPartyInvoices.Count() > 0)
                {
                    sw.Stop();
                    break;
                }
            }
            var thirdPartyInvoice = thirdPartyInvoices.OrderByDescending(x => x.ThirdPartyInvoiceKey).First();

            var searchForThirdPartyInvoicesQuery = new SearchForThirdPartyInvoicesQuery(thirdPartyInvoice.ThirdPartyInvoiceKey.ToString(), searchFilters, "ThirdPartyInvoice");
            AssertThirdPartyInvoiceReturnedInThirdPartyInvoiceSearch(searchForThirdPartyInvoicesQuery, thirdPartyInvoice.ThirdPartyInvoiceKey, 1);
        }
    }
}
