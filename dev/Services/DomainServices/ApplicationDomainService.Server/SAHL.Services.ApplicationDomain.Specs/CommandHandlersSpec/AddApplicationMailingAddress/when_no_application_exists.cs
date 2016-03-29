using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicationMailingAddress
{
    public class when_no_application_exists : WithCoreFakes
    {
        private static AddApplicationMailingAddressCommand command;
        private static AddApplicationMailingAddressCommandHandler handler;
        private static IDomainQueryServiceClient domainQueryService;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static IDomainRuleManager<ApplicationMailingAddressModel> domainRuleManager;
        private static ApplicationMailingAddressModel model;

        private Establish context = () =>
        {
            domainQueryService = An<IDomainQueryServiceClient>();
            applicationDataManager = An<IApplicationDataManager>();
            applicantDataManager = An<IApplicantDataManager>();
            domainRuleManager = An<IDomainRuleManager<ApplicationMailingAddressModel>>();
            model = new ApplicationMailingAddressModel(1, 2, CorrespondenceLanguage.English, OnlineStatementFormat.PDFFormat, CorrespondenceMedium.Email, 1, true);
            handler = new AddApplicationMailingAddressCommandHandler(domainQueryService, applicationDataManager, domainRuleManager, unitOfWorkFactory, eventRaiser, applicantDataManager);
            command = new AddApplicationMailingAddressCommand(model);
            //data manager must return false
            applicationDataManager.WhenToldTo(x => x.DoesOpenApplicationExist(command.model.ApplicationNumber)).Return(false);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_an_application_mailing_address_exists = () =>
        {
            applicationDataManager.WasToldTo(x => x.DoesApplicationMailingAddressExist(command.model.ApplicationNumber));
        };

        private It should_check_that_the_application_exists = () =>
        {
            applicationDataManager.WasToldTo(x => x.DoesOpenApplicationExist(command.model.ApplicationNumber));
        };

        private It should_check_if_the_client_address_exists = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery(Param.IsAny<GetClientAddressQuery>()));
        };

        private It should_not_run_rules = () =>
        {
            domainRuleManager.WasNotToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<ApplicationMailingAddressModel>()));
        };

        private It should_not_save_application_mailing_address = () =>
        {
            applicationDataManager.WasNotToldTo(x => x.SaveApplicationMailingAddress(command.model, Param.IsAny<int>(), Param.IsAny<int>()));
        };

        private It should_not_raise_a_mailing_address_added_to_application_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<MailingAddressAddedToApplicationEvent>(), Param.IsAny<int>(), Param.IsAny<int>(), 
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_return_a_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldContain("The application number provided does not exist. No Mailing Address could be added.");
        };
    }
}