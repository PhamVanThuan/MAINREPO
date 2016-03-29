using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.LinkDomiciliumAddressToApplicant
{
    public class when_origination_source_is_not_comcorp : WithCoreFakes
    {
        private static IDomainRuleManager<VendorModel> domainRuleManager;
        private static IApplicationDataManager applicationDataManager;
        private static OriginationSource originationSource;

        private static LinkExternalVendorToApplicationCommand command;
        private static LinkExternalVendorToApplicationCommandHandler handler;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            domainRuleManager = new DomainRuleManager<VendorModel>();

            int applicationNumber = 1498174;
            string vendorCode = "testvc1";
            originationSource = OriginationSource.Capitec;

            command = new LinkExternalVendorToApplicationCommand(applicationNumber, originationSource, vendorCode);
            handler = new LinkExternalVendorToApplicationCommandHandler(applicationDataManager,
                domainRuleManager,
                unitOfWorkFactory,
                eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_return_messages = () =>
        {
            messages.ErrorMessages().ShouldContain(x => x.Message == "Origination source must be Comcorp.");
        };
    }
}
