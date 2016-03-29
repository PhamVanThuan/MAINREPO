﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FinanceDomain.Queries;
using SAHL.Workflow.ThirdPartyInvoices;
using System;
using System.Collections.Generic;

namespace SAHL.Workflow.ThirdPartyInvoice.Specs.ThirdPartyInvoiceWorkflowProcessSpecs
{
    public class when_retrieving_third_party_invoice : WithFakes
    {
        private static ThirdPartyInvoiceWorkflowProcess thirdPartyInvoiceWorkflowProcess;
        private static IFinanceDomainServiceClient financeDomainServiceClient;
        private static int thirdPartyInvoiceKey;
        private static ISystemMessageCollection messages;
        private static GetThirdPartyInvoiceQueryResult results;
        private static GetThirdPartyInvoiceQueryResult expectedThirdPartyInvoiceDataModel;

        private Establish context = () =>
        {
            financeDomainServiceClient = An<IFinanceDomainServiceClient>();
            messages = An<ISystemMessageCollection>();
            thirdPartyInvoiceKey = 12409;
            thirdPartyInvoiceWorkflowProcess = new ThirdPartyInvoiceWorkflowProcess(financeDomainServiceClient);
            expectedThirdPartyInvoiceDataModel = new GetThirdPartyInvoiceQueryResult
            {
                ThirdPartyInvoiceKey = 1234,
                SahlReference = @"SAHL-2015\05\123",
                ThirdPartyId = Guid.NewGuid(),
                ThirdPartyRegisteredName = "Radles Inc",
                AccountKey = 12111,
                InvoiceStatusKey = 2,
                AmountExcludingVAT = 100.00m,
                VATAmount = 14.0M,
                TotalAmountIncludingVAT = 114.0M,
                ReceivedFromEmailAddress = "randles@randlesinc.com",
                InvoiceDate = DateTime.Now,
                InvoiceNumber = "SAHL780",
                ReceivedDate = DateTime.Now
            };
            var enumerableResults = new List<GetThirdPartyInvoiceQueryResult> { expectedThirdPartyInvoiceDataModel };
            financeDomainServiceClient.WhenToldTo(x => x.PerformQuery(Param.IsAny<GetThirdPartyInvoiceQuery>()))
                .Return<GetThirdPartyInvoiceQuery>(y =>
                {
                    y.Result = new ServiceQueryResult<GetThirdPartyInvoiceQueryResult>(enumerableResults);
                    return SystemMessageCollection.Empty();
                });
        };

        private Because of = () =>
        {
            results = thirdPartyInvoiceWorkflowProcess.GetThirdPartyInvoiceByThirdPartyInvoiceKey(messages, thirdPartyInvoiceKey);
        };

        private It should_retrieve_the_invoice_from_the_finance_domain = () =>
        {
            financeDomainServiceClient.WasToldTo(x => x.PerformQuery(Param.IsAny<GetThirdPartyInvoiceQuery>()));
        };

        private It should_return_the_invoice_requested = () =>
        {
            (
                results.AccountKey == expectedThirdPartyInvoiceDataModel.AccountKey &&
                results.InvoiceDate == expectedThirdPartyInvoiceDataModel.InvoiceDate &&
                results.InvoiceNumber == expectedThirdPartyInvoiceDataModel.InvoiceNumber &&
                results.InvoiceStatusKey == expectedThirdPartyInvoiceDataModel.InvoiceStatusKey &&
                results.ReceivedFromEmailAddress == expectedThirdPartyInvoiceDataModel.ReceivedFromEmailAddress &&
                results.SahlReference == expectedThirdPartyInvoiceDataModel.SahlReference
            ).ShouldBeTrue();
        };
    }
}