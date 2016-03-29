using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
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

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.LinkExternalVendorToApplication
{
    public class when_linking_a_vendor_to_an_application : WithCoreFakes
    {
        private static LinkExternalVendorToApplicationCommand command;
        private static LinkExternalVendorToApplicationCommandHandler handler;      
        private static IDomainRuleManager<VendorModel> domainRuleManager;
        private static IApplicationDataManager applicationDataManager;     
        private static VendorModel vendorModel;
        private static int applicationNumber;
        private static string vendorCode;
        private static int offerRoleKey;
        
        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            domainRuleManager = An<IDomainRuleManager<VendorModel>>();
            eventRaiser = An<IEventRaiser>();
            applicationNumber = 1498174;
            vendorCode = "testvc1";

            vendorModel = new VendorModel(2, vendorCode, 7874, 956482, 1);

            command = new LinkExternalVendorToApplicationCommand(applicationNumber, OriginationSource.Comcorp, vendorCode);
            handler = new LinkExternalVendorToApplicationCommandHandler(applicationDataManager,
               domainRuleManager,
               unitOfWorkFactory,
               eventRaiser);
            applicationDataManager.WhenToldTo(x => x.GetExternalVendorForVendorCode(vendorCode)).Return(vendorModel);
            offerRoleKey = 1111;
            applicationDataManager.WhenToldTo(x => x.SaveExternalVendorOfferRole(Param.IsAny<OfferRoleDataModel>())).Return(offerRoleKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_get_external_vendor_for_vendorcode = () =>
        {
            applicationDataManager.WasToldTo(x => x.GetExternalVendorForVendorCode(vendorCode));
        };

        private It should_execute_the_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), vendorModel));
        };

        private It should_save_external_vendor_offerRole = () =>
        {
            applicationDataManager.WasToldTo(x => x.SaveExternalVendorOfferRole(Param<OfferRoleDataModel>
                .Matches(y => y.LegalEntityKey == vendorModel.LegalEntityKey
                && y.OfferKey == command.ApplicationNumber
                && y.OfferRoleTypeKey == (int)OfferRoleType.ExternalVendor
                && y.GeneralStatusKey == vendorModel.GeneralStatusKey)));
        };

        private It should_raise_an_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(
                  Param.IsAny<DateTime>()
                , Arg.Is<ExternalVendorLinkedToApplicationEvent>(y => y.ApplicationNumber == command.ApplicationNumber && y.ClientKey == vendorModel.LegalEntityKey)
                , offerRoleKey
                , (int)GenericKeyType.OfferRole
                , Param.IsAny<IServiceRequestMetadata>()
            ));
        };    
    }
}