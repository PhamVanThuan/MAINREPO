using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplication.Refinance
{
    public class when_adding_an_sahl_originated_application : WithCoreFakes
    {
        private static AddRefinanceApplicationCommand command;
        private static AddRefinanceApplicationCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static RefinanceApplicationModel model;
        private static int expectedApplicationKey;
        private static IApplicationManager applicationManager;
        private static IDomainQueryServiceClient domainQueryService;
        private static IDomainRuleManager<RefinanceApplicationModel> ruleContext;

        private Establish context = () =>
        {
            model = new RefinanceApplicationModel(OfferStatus.Open, 1, OriginationSource.SAHomeLoans, 1500000, 240, 750000, Product.NewVariableLoan, "reference1", 1);

            applicationManager = An<IApplicationManager>();
            domainQueryService = An<IDomainQueryServiceClient>();
            applicationDataManager = An<IApplicationDataManager>();
            ruleContext = An<IDomainRuleManager<RefinanceApplicationModel>>();

            command = new AddRefinanceApplicationCommand(model, Guid.NewGuid());
            handler = new AddRefinanceApplicationCommandHandler(serviceCommandRouter, applicationDataManager, linkedKeyManager, unitOfWorkFactory, eventRaiser, applicationManager, ruleContext);

            //save application
            expectedApplicationKey = 1234;
            applicationManager.WhenToldTo(x => x.SaveApplication(model.ApplicationType, model.ApplicationStatus, Param.IsAny<DateTime>(), model.ApplicationSourceKey,
                Param.IsAny<int>(), OriginationSource.SAHomeLoans, model.Reference, command.RefinanceApplicationModel.ApplicantCount)).Return(expectedApplicationKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_get_a_reserved_account_number_for_the_application = () =>
        {
            applicationDataManager.WasToldTo(x => x.GetReservedAccountNumber());
        };

        private It should_save_the_application = () =>
        {
            applicationManager.WasToldTo(x => x.SaveApplication(model.ApplicationType, model.ApplicationStatus, Param.IsAny<DateTime>(), model.ApplicationSourceKey, Param.IsAny<int>(),
                OriginationSource.SAHomeLoans, Param.IsAny<string>(), command.RefinanceApplicationModel.ApplicantCount));
        };

        private It should_not_save_an_external_originator_attribute = () =>
        {
            serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param.IsAny<SetExternalOriginatorAttributeCommand>(), serviceRequestMetaData));
        };

        private It should_link_the_application_to_the_guid_provided = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(Param.IsAny<int>(), Param.IsAny<Guid>()));
        };

        private It should_not_have_any_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_raise_a_refinance_application_added_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<RefinanceApplicationAddedEvent>
                (y => y.ApplicationSourceKey == model.ApplicationSourceKey),
                    expectedApplicationKey, (int)GenericKeyType.Offer, Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}