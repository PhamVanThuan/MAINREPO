using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicationMailingAddress
{
    public class when_adding_a_non_existing_application_mailing_address : WithCoreFakes
    {
        private static AddApplicationMailingAddressCommand command;
        private static AddApplicationMailingAddressCommandHandler handler;
        private static ApplicationMailingAddressModel model;
        private static IDomainQueryServiceClient domainQueryService;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static int addressKey, clientKey;
        private static IDomainRuleManager<ApplicationMailingAddressModel> domainRuleManager;
        private static int applicationMailingAddressKey;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            applicantDataManager = An<IApplicantDataManager>();
            domainQueryService = An<IDomainQueryServiceClient>();
            domainRuleManager = An<IDomainRuleManager<ApplicationMailingAddressModel>>();

            addressKey = 232;
            clientKey = 6398;
            handler = new AddApplicationMailingAddressCommandHandler(domainQueryService, applicationDataManager, domainRuleManager, unitOfWorkFactory, eventRaiser,applicantDataManager);
            model = new ApplicationMailingAddressModel(1234567, 12345, CorrespondenceLanguage.English, OnlineStatementFormat.PDFFormat,
                CorrespondenceMedium.Email, 123587, true);
            command = new AddApplicationMailingAddressCommand(model);
            applicationDataManager.WhenToldTo(x => x.DoesApplicationMailingAddressExist(command.model.ApplicationNumber)).Return(false);

            applicationDataManager.WhenToldTo(x => x.DoesOpenApplicationExist(command.model.ApplicationNumber)).Return(true);

            var clientAddressQueryResult = new ServiceQueryResult<GetClientAddressQueryResult>(
                new GetClientAddressQueryResult[]
                {
                    new GetClientAddressQueryResult() { AddressKey = addressKey, ClientKey = clientKey }
                });

            domainQueryService.WhenToldTo(d => d.PerformQuery(Param.IsAny<GetClientAddressQuery>()))
                .Return<GetClientAddressQuery>(y => { y.Result = clientAddressQueryResult; return messages; });

            applicationMailingAddressKey = 1234;
            applicationDataManager.WhenToldTo(x => x.SaveApplicationMailingAddress(Param.IsAny<ApplicationMailingAddressModel>(), Param.IsAny<int>(), Param.IsAny<int>()))
                .Return(applicationMailingAddressKey);
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

        private It should_execute_rules = () =>
        {
            domainRuleManager.WasToldTo(dr => dr.ExecuteRules(messages, Param.IsAny<ApplicationMailingAddressModel>()));
        };

        private It should_save_the_application_mailing_address = () =>
        {
            applicationDataManager.WasToldTo(x => x.SaveApplicationMailingAddress(model, clientKey, addressKey));
        };

        private It should_raise_a_mailing_address_added_to_application_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Param<MailingAddressAddedToApplicationEvent>.Matches(m => m.ApplicationNumber == command.model.ApplicationNumber 
                    && m.ClientAddressKey == command.model.ClientAddressKey
                    && m.ClientToUseForEmailCorrespondence == command.model.ClientToUseForEmailCorrespondence
                    && m.CorrespondenceLanguage == command.model.CorrespondenceLanguage
                    && m.CorrespondenceMedium == command.model.CorrespondenceMedium)
                , applicationMailingAddressKey
                , (int)GenericKeyType.OfferMailingAddress
                , Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_return_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}
