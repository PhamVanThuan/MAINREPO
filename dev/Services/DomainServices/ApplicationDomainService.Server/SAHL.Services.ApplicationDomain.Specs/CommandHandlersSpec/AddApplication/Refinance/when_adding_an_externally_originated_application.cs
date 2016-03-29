using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.DomainQuery;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplication.Refinance
{
    public class when_adding_an_externally_originated_application : WithCoreFakes
    {
        private static AddRefinanceApplicationCommand command;
        private static AddRefinanceApplicationCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static RefinanceApplicationModel refinanceApplicationModel;
        private static IApplicationManager applicationManager;
        private static int applicationNumber;
        private static IDomainRuleManager<RefinanceApplicationModel> ruleContext;

        private Establish context = () =>
        {
            refinanceApplicationModel = new RefinanceApplicationModel(OfferStatus.Open, 1, OriginationSource.Comcorp, 950000, 240, 100000, Product.NewVariableLoan, "reference1", 1);
            applicationManager = An<IApplicationManager>();
            applicationDataManager = An<IApplicationDataManager>();
            ruleContext = An<IDomainRuleManager<RefinanceApplicationModel>>();
            //save application
            applicationNumber = 1234;
            applicationManager.WhenToldTo(x => x.SaveApplication(refinanceApplicationModel.ApplicationType, refinanceApplicationModel.ApplicationStatus, Param.IsAny<DateTime>(), 
                refinanceApplicationModel.ApplicationSourceKey, Param.IsAny<int>(), OriginationSource.SAHomeLoans, refinanceApplicationModel.Reference, refinanceApplicationModel.ApplicantCount))
                .Return(applicationNumber);

            command = new AddRefinanceApplicationCommand(refinanceApplicationModel, Guid.NewGuid());
            handler = new AddRefinanceApplicationCommandHandler(serviceCommandRouter, applicationDataManager, linkedKeyManager, unitOfWorkFactory, eventRaiser, applicationManager, ruleContext);
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
            applicationManager.WasToldTo(x => x.SaveApplication(refinanceApplicationModel.ApplicationType, refinanceApplicationModel.ApplicationStatus, Param.IsAny<DateTime>(), 
                refinanceApplicationModel.ApplicationSourceKey, 0, OriginationSource.SAHomeLoans, refinanceApplicationModel.Reference, refinanceApplicationModel.ApplicantCount));
        };

        private It should_raise_an_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<RefinanceApplicationAddedEvent>
                (y => y.ApplicationSourceKey == (int)OriginationSource.SAHomeLoans),
                    applicationNumber, (int)GenericKeyType.Offer, Param.IsAny<IServiceRequestMetadata>()));
        };
        
        private It should_link_the_application_to_the_guid_provided = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(Param.IsAny<int>(), Param.IsAny<Guid>()));
        };

        private It should_add_the_attribute_for_the_external_originator = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Arg.Is<SetExternalOriginatorAttributeCommand>(
                y=>y.OriginationSource == command.RefinanceApplicationModel.OriginationSource), serviceRequestMetaData));
        };

        private It should_not_have_any_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}