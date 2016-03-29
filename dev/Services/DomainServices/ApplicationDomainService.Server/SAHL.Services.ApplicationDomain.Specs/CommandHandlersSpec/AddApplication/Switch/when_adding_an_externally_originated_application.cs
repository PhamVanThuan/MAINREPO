using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplication.Switch
{
    public class when_adding_an_externally_originated_application : WithCoreFakes
    {
        private static AddSwitchApplicationCommand command;
        private static AddSwitchApplicationCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static SwitchApplicationModel switchApplicationModel;
        private static int applicationNumber;
        private static IApplicationManager applicationManager;
        private static IDomainRuleManager<SwitchApplicationModel> ruleContext;

        private Establish context = () =>
        {
            switchApplicationModel = new SwitchApplicationModel(OfferStatus.Open, 1, OriginationSource.Capitec, 420000, 500000, 240, 100000, Product.NewVariableLoan, "reference1", 1);

            applicationManager = An<IApplicationManager>();
            applicationDataManager = An<IApplicationDataManager>();
            ruleContext = An<IDomainRuleManager<SwitchApplicationModel>>();

            command = new AddSwitchApplicationCommand(switchApplicationModel, Guid.NewGuid());
            handler = new AddSwitchApplicationCommandHandler(serviceCommandRouter, applicationDataManager, linkedKeyManager, unitOfWorkFactory, eventRaiser, applicationManager, ruleContext);
            //save application
            applicationNumber = 1234;
            applicationManager.WhenToldTo(x => x.SaveApplication(switchApplicationModel.ApplicationType, switchApplicationModel.ApplicationStatus, Param.IsAny<DateTime>(), 
                switchApplicationModel.ApplicationSourceKey, Param.IsAny<int>(), OriginationSource.SAHomeLoans, switchApplicationModel.Reference, switchApplicationModel.ApplicantCount))
                .Return(applicationNumber);

            //set external originator attribute
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<SetExternalOriginatorAttributeCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(messages);
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
            applicationManager.WasToldTo(x => x.SaveApplication(switchApplicationModel.ApplicationType, switchApplicationModel.ApplicationStatus, Param.IsAny<DateTime>(), 
                switchApplicationModel.ApplicationSourceKey, Param.IsAny<int>(), OriginationSource.SAHomeLoans, switchApplicationModel.Reference, switchApplicationModel.ApplicantCount));
        };

        private It should_raise_a_switch_application_added_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<SwitchApplicationAddedEvent>
                (y => y.ApplicationSourceKey == switchApplicationModel.ApplicationSourceKey),
                    applicationNumber, (int)GenericKeyType.Offer, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_save_the_external_originator_attribute = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param.IsAny<SetExternalOriginatorAttributeCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_link_the_application_to_the_guid_provided = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(Param.IsAny<int>(), Param.IsAny<Guid>()));
        };

        private It should_not_have_any_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}