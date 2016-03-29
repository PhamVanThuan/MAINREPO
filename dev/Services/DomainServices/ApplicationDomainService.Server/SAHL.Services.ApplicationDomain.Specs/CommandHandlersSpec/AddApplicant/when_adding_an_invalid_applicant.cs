using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicant
{
    public class when_adding_an_invalid_applicant : WithCoreFakes
    {
        private static AddLeadApplicantToApplicationCommand command;
        private static AddLeadApplicantToApplicationCommandHandler handler;        
        private static IApplicantDataManager applicantDataManager;
        private static IDomainRuleManager<AddLeadApplicantToApplicationCommand> domainRuleManager;
        private static ISystemMessageCollection expectedMessages;
        private static IDomainQueryServiceClient domainQueryClient;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {

            applicantDataManager = An<IApplicantDataManager>();
            domainQueryClient = An<IDomainQueryServiceClient>();
            validationUtils = An<IValidationUtils>();
            domainRuleManager = An<IDomainRuleManager<AddLeadApplicantToApplicationCommand>>();

            command = new AddLeadApplicantToApplicationCommand(Guid.NewGuid(), Param.IsAny<int>(), Param.IsAny<int>(), LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
            handler = new AddLeadApplicantToApplicationCommandHandler(applicantDataManager, unitOfWorkFactory, domainRuleManager, eventRaiser, linkedKeyManager, validationUtils, domainQueryClient);

            expectedMessages = SystemMessageCollection.Empty();
            expectedMessages.AddMessage(new SystemMessage("Test message", SystemMessageSeverityEnum.Error));
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<AddLeadApplicantToApplicationCommand>()))
                .Callback<ISystemMessageCollection, AddLeadApplicantToApplicationCommand>((y, z) => { y.AddMessages(expectedMessages.AllMessages); });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_have_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };

        private It should_have_the_expected_messages = () =>
        {
            messages.ShouldBeLike(expectedMessages);
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<Event>(),
                    Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}