using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ExternalVendorMustExistAndBeActive
{
    public class when_inactive_external_vendor_exists : WithFakes
    {
        private static VendorModel ruleModel;
        private static ISystemMessageCollection messages;
        private static ExternalVendorMustBeActiveRule rule;

        private Establish context = () =>
        {
            int vendorKey = 1111;
            string vendorCode = "Vendor1";
            int organisationStructureKey = 2222;
            int legalEntityKey = 3333;
            int generalStatusKey = (int)GeneralStatus.Inactive;

            ruleModel = new VendorModel(vendorKey, vendorCode, organisationStructureKey, legalEntityKey, generalStatusKey);
            messages = SystemMessageCollection.Empty();

            rule = new ExternalVendorMustBeActiveRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_throw_an_error = () =>
        {
            messages.ErrorMessages().ShouldContain(x => x.Message == "Vendor is not active.");
        };
    }
}
