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

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplication.NewPurchase
{
    public class when_adding_an_externally_originated_application : WithCoreFakes
    {
        private static AddNewPurchaseApplicationCommand command;
        private static AddNewPurchaseApplicationCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static NewPurchaseApplicationModel newPurchaseApplicationModel;
        private static IApplicationManager applicationManager;
        private static int applicationNumber;
        private static IDomainQueryServiceClient domainQueryService;
        private static IDomainRuleManager<NewPurchaseApplicationModel> ruleContext;

        private Establish context = () =>
        {
            newPurchaseApplicationModel = new NewPurchaseApplicationModel(OfferStatus.Open, 1, OriginationSource.SAHomeLoans, 100000, 550000, 550000, 240, Product.NewVariableLoan, 
                "reference1", 1, "transfer attorney");

            applicationManager = An<IApplicationManager>();
            domainQueryService = An<IDomainQueryServiceClient>();
            ruleContext = An<IDomainRuleManager<NewPurchaseApplicationModel>>();

            applicationDataManager = An<IApplicationDataManager>();
            //save application
            applicationNumber = 1234;
            applicationManager.WhenToldTo(x => x.SaveApplication(newPurchaseApplicationModel.ApplicationType, newPurchaseApplicationModel.ApplicationStatus, Param.IsAny<DateTime>(), 
                newPurchaseApplicationModel.ApplicationSourceKey, Param.IsAny<int>(), newPurchaseApplicationModel.OriginationSource, newPurchaseApplicationModel.Reference, 
                newPurchaseApplicationModel.ApplicantCount)).Return(applicationNumber);

            command = new AddNewPurchaseApplicationCommand(newPurchaseApplicationModel, Guid.NewGuid());
            handler = new AddNewPurchaseApplicationCommandHandler(serviceCommandRouter, applicationDataManager, linkedKeyManager, eventRaiser, unitOfWorkFactory, applicationManager, ruleContext);
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
            applicationManager.WasToldTo(x => x.SaveApplication(newPurchaseApplicationModel.ApplicationType, newPurchaseApplicationModel.ApplicationStatus, Param.IsAny<DateTime>(), 
                newPurchaseApplicationModel.ApplicationSourceKey, 0, OriginationSource.SAHomeLoans, newPurchaseApplicationModel.Reference, newPurchaseApplicationModel.ApplicantCount));
        };

        private It should_raise_a_new_purchase_application_added_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<NewPurchaseApplicationAddedEvent>
                (y => y.ApplicationSourceKey == newPurchaseApplicationModel.ApplicationSourceKey),
                    applicationNumber, (int)GenericKeyType.Offer, Param.IsAny<IServiceRequestMetadata>()));
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