using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.BankAccountDomain.CommandHandlers;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.BankAccountDomain.Rules;
using SAHL.Services.BankAccountDomain.Utils;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Specs.CommandHandlerSpecs.Internal.AddBankAccount
{
    public class when_registering_rules : WithCoreFakes
    {
        private static AddBankAccountCommandHandler handler;
        private static IBankAccountDataManager bankAccountDataManager;
        private static IDomainRuleManager<BankAccountModel> domainRuleManager;
        private static ICdvValidationManager validationManager;

        private Establish context = () =>
        {
            validationManager = An<ICdvValidationManager>();
            bankAccountDataManager = An<IBankAccountDataManager>();
            domainRuleManager = An<IDomainRuleManager<BankAccountModel>>();
        };

        private Because of = () =>
        {
            handler = new AddBankAccountCommandHandler(bankAccountDataManager, linkedKeyManager, domainRuleManager, validationManager, unitOfWorkFactory);
        };

        private It should_register_the_does_branch_exist_for_bank_rule_without_any_context = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<BankAccountMustAdhereToCDVValidationRule>()));
        };

        private It should_register_the_cdv_validation_rule_without_any_context = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<BankAccountMustAdhereToCDVValidationRule>()));
        };
    }
}