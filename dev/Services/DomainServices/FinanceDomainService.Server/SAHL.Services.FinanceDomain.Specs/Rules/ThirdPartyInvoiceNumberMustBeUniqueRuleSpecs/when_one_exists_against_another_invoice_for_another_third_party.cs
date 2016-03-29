using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.Rules.ThirdPartyInvoiceNumberMustBeUniqueRuleSpecs
{
    public class when_one_exists_against_another_invoice_for_another_third_party : WithFakes
    {
        private static InvoiceNumberCannotExistAgainstAnotherInvoiceForTheSameThirdPartyRule rule;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static IEnumerable<ThirdPartyInvoiceDataModel> invoices;
        private static string invoiceNumber;
        private static ThirdPartyInvoiceModel ruleModel;
        private static ISystemMessageCollection messages;
        private static Guid thirdPartyId;

        private Establish context = () =>
        {
            thirdPartyId = Guid.NewGuid();
            messages = SystemMessageCollection.Empty();
            invoiceNumber = @"SAHL\2233\RC";
            ruleModel = new ThirdPartyInvoiceModel(5555, thirdPartyId, invoiceNumber, DateTime.Now.AddDays(-5), new InvoiceLineItemModel[] { 
                new InvoiceLineItemModel(null, 5555, 1, 500, true) 
            }, true, string.Empty);
            invoices = new ThirdPartyInvoiceDataModel[] { new ThirdPartyInvoiceDataModel(9995, "sahlReference", 1, 1408282, Guid.NewGuid(), invoiceNumber, DateTime.Now.AddDays(-5),
                "halouser@sahomeloans.com", 1500, 100, 1600, true, DateTime.Now.AddDays(-7), string.Empty)};
            dataManager = An<IThirdPartyInvoiceDataManager>();
            dataManager.WhenToldTo(x => x.GetThirdPartyInvoicesByInvoiceNumber(ruleModel.InvoiceNumber)).Return(invoices);
            rule = new InvoiceNumberCannotExistAgainstAnotherInvoiceForTheSameThirdPartyRule(dataManager);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_return_no_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}