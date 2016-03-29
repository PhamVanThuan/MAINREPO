using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.LinkDomiciliumAddressToApplicant
{
    public class when_external_vendor_does_not_exist : WithCoreFakes
    {
        private static LinkExternalVendorToApplicationCommand command;
        private static LinkExternalVendorToApplicationCommandHandler handler;
        private static IDomainRuleManager<VendorModel> domainRuleManager;
        private static IApplicationDataManager applicationDataManager;
        private static OriginationSource originationSource;
        private static VendorModel vendorModel;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            domainRuleManager = new DomainRuleManager<VendorModel>();

            eventRaiser = An<IEventRaiser>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            unitOfWorkFactory.WhenToldTo(x => x.Build()).Return(An<IUnitOfWork>());

            int applicationNumber = 1498174;
            string vendorCode = "testvc1";
            originationSource = OriginationSource.Comcorp;
            vendorModel = new VendorModel(2, "testvc1", 7874, 956482, 1);
            command = new LinkExternalVendorToApplicationCommand(applicationNumber, originationSource, vendorCode);
            handler = new LinkExternalVendorToApplicationCommandHandler(applicationDataManager,
                domainRuleManager,
                unitOfWorkFactory,
                eventRaiser);
            applicationDataManager.WhenToldTo(x => x.GetExternalVendorForVendorCode(vendorCode)).Return(() => null);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_add_an_error_message_to_the_system_messages_collection = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Vendor could not be found.");
        };

        private It should_not_link_the_vendor_to_the_application = () =>
        {
            applicationDataManager.WasNotToldTo(x => x.SaveExternalVendorOfferRole(Param.IsAny<OfferRoleDataModel>()));
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<IEvent>(),
                Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}