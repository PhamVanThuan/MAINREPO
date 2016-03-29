using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicant
{
    public class when_registering_rules : WithCoreFakes
    {
        private static AddLeadApplicantToApplicationCommandHandler handler;
        private static IApplicantDataManager applicantDataManager;
        private static IDomainRuleManager<AddLeadApplicantToApplicationCommand> domainRuleManager;
        private static IDomainQueryServiceClient domainQueryClient;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
            {
                applicantDataManager = An<IApplicantDataManager>();
                domainQueryClient = An<IDomainQueryServiceClient>();
                validationUtils = An<IValidationUtils>();
                domainRuleManager = An<IDomainRuleManager<AddLeadApplicantToApplicationCommand>>();
            };

        private Because of = () =>
            {
                handler = new AddLeadApplicantToApplicationCommandHandler(applicantDataManager, unitOfWorkFactory, domainRuleManager, eventRaiser, linkedKeyManager, validationUtils, 
                    domainQueryClient);
            };

        private It should_register_ApplicantsMustBeBetween18And65YearsOld_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<ApplicantsMustBeBetween18And65YearsOldRule>()));
        };

        private It should_register_ClientCannotBeAnExistingApplicantRule_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<ClientCannotBeAnExistingApplicantRule>()));
        };

        private It should_register_the_applicant_firstName_and_Surname_required_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<ApplicantMustHaveFirstNamesAndSurnameRule>()));
        };

        private It should_register_the_contact_detail_required_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<ApplicantMustHaveAtLeastOneContactDetailRule>()));
        };
    }
}