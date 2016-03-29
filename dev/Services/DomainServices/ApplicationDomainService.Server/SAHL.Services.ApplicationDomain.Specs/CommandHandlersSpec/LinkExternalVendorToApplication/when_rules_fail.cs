using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.LinkDomiciliumAddressToApplicant
{
    public class when_rules_fail : WithCoreFakes
    {
        private static LinkExternalVendorToApplicationCommand command;
        private static LinkExternalVendorToApplicationCommandHandler handler;
        private static VendorModel vendorModel;
        private static OfferRoleDataModel offerRoleModel;
        private static IApplicationDataManager applicationDataManager;
        private static IDomainRuleManager<VendorModel> domainRuleManager;
        private static string errorMessage;
        private static DateTime dateStamp;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            domainRuleManager = An<IDomainRuleManager<VendorModel>>();
            eventRaiser = An<IEventRaiser>();
            int applicationNumber = 1498174;
            string vendorCode = "testvc1";
            errorMessage = "Vendor is not active.";
            offerRoleModel = new OfferRoleDataModel(956482, applicationNumber, (int)OfferRoleType.ExternalVendor, 1, dateStamp);
            vendorModel = new VendorModel(2, "testvc1", 7874, 956482, 1);
            dateStamp = DateTime.Now;
            command = new LinkExternalVendorToApplicationCommand(applicationNumber, OriginationSource.Comcorp, vendorCode);
            handler = new LinkExternalVendorToApplicationCommandHandler(applicationDataManager,
               domainRuleManager,
               unitOfWorkFactory,
               eventRaiser);
            applicationDataManager.WhenToldTo(x => x.GetExternalVendorForVendorCode(vendorCode)).Return(vendorModel);
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), vendorModel))
                .Callback<ISystemMessageCollection>(a => { a.AddMessage(new SystemMessage(errorMessage, SystemMessageSeverityEnum.Error)); });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_execute_the_rules_on_command = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), vendorModel));
        };

        private It should_return_the_error_messages_from_the_rules = () =>
        {
            messages.ErrorMessages().First().Message.ShouldContain(errorMessage);
        };

        private It should_not_save_a_legal_entity_key = () =>
        {
            applicationDataManager.WasNotToldTo(x => x.SaveExternalVendorOfferRole(offerRoleModel));
        };

        It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(raiser => raiser.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<ExternalVendorLinkedToApplicationEvent>(), Param.IsAny<int>(), Param.IsAny<int>(), 
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}