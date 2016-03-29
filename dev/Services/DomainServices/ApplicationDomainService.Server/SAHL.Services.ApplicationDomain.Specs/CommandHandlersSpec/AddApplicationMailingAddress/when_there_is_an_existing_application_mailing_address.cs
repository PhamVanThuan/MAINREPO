﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicationMailingAddress
{
    public class when_there_is_an_existing_application_mailing_address : WithCoreFakes
    {
        private static AddApplicationMailingAddressCommand command;
        private static AddApplicationMailingAddressCommandHandler handler;
        private static ApplicationMailingAddressModel model;
        private static IDomainQueryServiceClient domainQueryService;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static IDomainRuleManager<ApplicationMailingAddressModel> domainRuleManager;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            applicantDataManager = An<IApplicantDataManager>();
            domainQueryService = An<IDomainQueryServiceClient>();
            domainRuleManager = An<IDomainRuleManager<ApplicationMailingAddressModel>>();
            model = new ApplicationMailingAddressModel(1234567, 12345, CorrespondenceLanguage.English, OnlineStatementFormat.PDFFormat,
                CorrespondenceMedium.Email, 123587, true);
            command = new AddApplicationMailingAddressCommand(model);

            applicationDataManager.WhenToldTo(adm => adm.DoesApplicationMailingAddressExist(command.model.ApplicationNumber)).Return(true);

            handler = new AddApplicationMailingAddressCommandHandler(domainQueryService, applicationDataManager, domainRuleManager, unitOfWorkFactory, eventRaiser,applicantDataManager);
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

        private It should_check_that_the_client_address_record_exists = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery(Param.IsAny<GetClientAddressQuery>()));
        };

        private It should_not_execute_rules = () =>
        {
            domainRuleManager.WasNotToldTo(dr => dr.ExecuteRules(messages, Param.IsAny<ApplicationMailingAddressModel>()));
        };

        private It should_not_save_the_application_mailing_address = () =>
        {
            applicationDataManager.WasNotToldTo(x => x.SaveApplicationMailingAddress(Param.IsAny<ApplicationMailingAddressModel>(), Param.IsAny<int>(), Param.IsAny<int>()));
        };

        private It should_not_raise_a_mailing_address_added_to_application_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<MailingAddressAddedToApplicationEvent>(), Param.IsAny<int>(), Param.IsAny<int>(), 
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Application has already been assigned a mailing address");
        };
    }
}