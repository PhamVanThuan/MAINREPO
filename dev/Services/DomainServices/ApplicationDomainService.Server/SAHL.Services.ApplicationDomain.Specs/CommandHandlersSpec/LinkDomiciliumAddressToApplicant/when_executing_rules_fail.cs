using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Domicilium;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.LinkDomiciliumAddressToApplicant
{
    public class when_executing_rules_fail : WithCoreFakes
    {
        private static IDomiciliumDataManager domiciliumDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static IApplicationDataManager applicationDataManager;
        private static LinkDomiciliumAddressToApplicantCommandHandler handler;
        private static LinkDomiciliumAddressToApplicantCommand command;
        private static ApplicantDomiciliumAddressModel applicantDomiciliumAddressModel;
        private static int clientDomiciliumKey, applicationNumber, clientKey;
        private static IDomainRuleManager<ApplicantDomiciliumAddressModel> domainRuleContext;
        private static IADUserManager adUserManager;

        private Establish context = () =>
        {
            clientDomiciliumKey = 100;
            applicationNumber = 123;
            clientKey = 9090;
            applicantDataManager = An<IApplicantDataManager>();
            applicationDataManager = An<IApplicationDataManager>();
            domiciliumDataManager = An<IDomiciliumDataManager>();
            domainRuleContext = An<IDomainRuleManager<ApplicantDomiciliumAddressModel>>();
            adUserManager = An<IADUserManager>();

            unitOfWorkFactory.WhenToldTo(x => x.Build()).Return(unitOfWork);

            applicantDomiciliumAddressModel = new ApplicantDomiciliumAddressModel(clientDomiciliumKey, applicationNumber, clientKey);
            command = new LinkDomiciliumAddressToApplicantCommand(applicantDomiciliumAddressModel);
            handler = new LinkDomiciliumAddressToApplicantCommandHandler(
                applicantDataManager,
                applicationDataManager,
                domiciliumDataManager,
                unitOfWorkFactory,
                eventRaiser,
                domainRuleContext,
                adUserManager);

            domainRuleContext.WhenToldTo(
                x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<ApplicantDomiciliumAddressModel>()))
                .Callback<ISystemMessageCollection>(
                    y => y.AddMessage(new SystemMessage("The applicant currently has a pending domicilium address.", SystemMessageSeverityEnum.Error)));
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_execute_the_rules = () =>
        {
            domainRuleContext.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), applicantDomiciliumAddressModel));
        };

        private It should_not_link_the_domicilium_address_to_the_applicant = () =>
        {
            applicantDataManager.WasNotToldTo(x => x.LinkDomiciliumAddressToApplicant(Param.IsAny<OfferRoleDomiciliumDataModel>()));
        };

        private It should_contain_error_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("The applicant currently has a pending domicilium address.");
        };
    }
}