using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Data.Models.EventStore;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Identity;
using SAHL.Core.Services;
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

namespace SAHL.Testing.Services.Tests.Tests.FinanceDomain
{
    public class when_marking_invoice_as_paid : ServiceTestBase<IFinanceDomainServiceClient>
    {
        private ThirdPartyInvoiceDataModel thirdPartyInvoice;

        [SetUp]
        public void Setup()
        {
            var invoicePmtProcessor = TestApiClient.Get<HaloUsersDataModel>(new { ADUserName = "InvoicePmtProcessor" }).FirstOrDefault();
            var orgStructureKey = invoicePmtProcessor.UserOrganisationStructureKey;
            var userCapabilities = invoicePmtProcessor.Capabilities;
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERORGANISATIONSTRUCTUREKEY, orgStructureKey.ToString());
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_CURRENTUSERCAPABILITIES, userCapabilities);
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERNAME, string.Concat(@"SAHL\", invoicePmtProcessor.ADUserName));
        }

        [Test]
        public void when_marking_invoices_as_paid()
        {
            var processingPaymentinvoice = TestApiClient.Get<ThirdPartyInvoiceDataModel>(new { invoicestatuskey = (int)InvoiceStatus.Approved});
            var _command = new ProcessThirdPartyInvoicePaymentCommand(processingPaymentinvoice.FirstOrDefault().ThirdPartyInvoiceKey);
            Execute(_command).WithoutErrors();
            var thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney }, 1000);
            var legalEntity = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = thirdParty.LegalEntityKey });
            var projectionBefore = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = legalEntity.FirstOrDefault().RegisteredName }).FirstOrDefault();
            var command = new MarkThirdPartyInvoiceAsPaidCommand(processingPaymentinvoice.FirstOrDefault().ThirdPartyInvoiceKey);
            Execute(command).WithoutErrors();

            var summarisedEvent = TestApiClient.Get<EventDataModel>();
        }
    }
}
