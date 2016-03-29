using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicant
{
    public class when_adding_a_valid_applicant : WithCoreFakes
    {
        private static AddLeadApplicantToApplicationCommand command;
        private static AddLeadApplicantToApplicationCommandHandler handler;
        private static IApplicantDataManager applicantDataManager;
        private static IDomainRuleManager<AddLeadApplicantToApplicationCommand> domainRuleManager;
        private static LegalEntityDataModel newLegalEntity;        
        private static int expectedApplicationkey = 1001;
        private static IDomainQueryServiceClient domainQueryClient;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            domainQueryClient = An<IDomainQueryServiceClient>();
            validationUtils = An<IValidationUtils>();
            // this guy has no legalentitykey therefore seen as a new legal entity
            newLegalEntity = new LegalEntityDataModel(2, 6, 2, 2, DateTime.Now, 2, "bob", "bs", "smith", null, "8001045000007", null, null, null, null, null, DateTime.Now.AddYears(-35),
                          null, null, null, null, null, null, null, null, null, 1, 1, null, null, null, null, null, 2, 2, null);

            applicantDataManager = An<IApplicantDataManager>();
            domainRuleManager = An<IDomainRuleManager<AddLeadApplicantToApplicationCommand>>();

            command = new AddLeadApplicantToApplicationCommand(Guid.NewGuid(), Param.IsAny<int>(), expectedApplicationkey, LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
            handler = new AddLeadApplicantToApplicationCommandHandler(applicantDataManager, unitOfWorkFactory, domainRuleManager, eventRaiser, linkedKeyManager, validationUtils, domainQueryClient);

            //execute rule
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<AddLeadApplicantToApplicationCommand>()))
                .Callback<ISystemMessageCollection, AddLeadApplicantToApplicationCommand>((y, z) => { y.AddMessages(messages.AllMessages); });

            //check if applicant exists
            applicantDataManager.WhenToldTo(x => x.GetApplicantByIDNumber(Param.IsAny<string>())).Return(newLegalEntity);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_validate_the_applicant = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<AddLeadApplicantToApplicationCommand>()));
        };

        private It should_save_the_applicant_roles = () =>
        {
            applicantDataManager.WasToldTo(x => x.AddApplicantRole(Param.IsAny<OfferRoleDataModel>()));
        };

        private It should_link_the_application_role_key_to_the_guid = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(Param.IsAny<int>(), Param.IsAny<Guid>()));
        };

        private It should_should_raise_an_applicant_added_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<LeadApplicantAddedToApplicationEvent>
                (y => y.ApplicationNumber == expectedApplicationkey),
                    Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_have_any_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
