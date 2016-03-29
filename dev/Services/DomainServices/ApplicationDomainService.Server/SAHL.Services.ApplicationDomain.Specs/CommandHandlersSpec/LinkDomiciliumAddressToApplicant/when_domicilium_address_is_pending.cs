using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Domicilium;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.LinkDomiciliumAddressToApplicant
{
    public class when_domicilium_address_is_pending : WithCoreFakes
    {
        private static IDomiciliumDataManager domiciliumDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static IApplicationDataManager applicationDataManager;
        private static LinkDomiciliumAddressToApplicantCommandHandler handler;
        private static LinkDomiciliumAddressToApplicantCommand command;
        private static ApplicantDomiciliumAddressModel applicantDomiciliumAddressModel;
        private static int clientDomiciliumKey, applicationNumber, clientKey;
        private static int adUserKey, applicationRoleDomiciliumKey;
        private static IDomainRuleManager<ApplicantDomiciliumAddressModel> domainRuleContext;
        private static IADUserManager adUserManager;

        private Establish context = () =>
        {
            clientDomiciliumKey = 100;
            applicationNumber = 123;
            applicationRoleDomiciliumKey = 5689;
            clientKey = 9090;
            applicantDataManager = An<IApplicantDataManager>();
            applicationDataManager = An<IApplicationDataManager>();
            domiciliumDataManager = An<IDomiciliumDataManager>();
            eventRaiser = An<IEventRaiser>();
            domainRuleContext = An<IDomainRuleManager<ApplicantDomiciliumAddressModel>>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            unitOfWorkFactory.WhenToldTo(x => x.Build()).Return(An<IUnitOfWork>());
            adUserManager = An<IADUserManager>();

            OfferRoleDataModel applicationRoleDataModel = new OfferRoleDataModel(clientKey, 34, 5, (int)GeneralStatus.Active, DateTime.Now);
            applicantDomiciliumAddressModel = new ApplicantDomiciliumAddressModel(clientDomiciliumKey, applicationNumber, clientKey);
            command = new LinkDomiciliumAddressToApplicantCommand(applicantDomiciliumAddressModel);
            handler = new LinkDomiciliumAddressToApplicantCommandHandler(applicantDataManager,
                applicationDataManager,
                domiciliumDataManager,
                unitOfWorkFactory,
                eventRaiser,
                domainRuleContext,
                adUserManager);

            applicantDataManager.WhenToldTo(x => x.DoesApplicantHavePendingDomiciliumOnApplication(clientKey, clientDomiciliumKey)).Return(false);
            domiciliumDataManager.WhenToldTo(x => x.IsDomiciliumAddressPendingDomiciliumAddress(clientDomiciliumKey)).Return(true);
            applicantDataManager.WhenToldTo(
                x => x.GetActiveApplicationRole(applicantDomiciliumAddressModel.ApplicationNumber, applicantDomiciliumAddressModel.ClientKey))
                .Return(applicationRoleDataModel);
            applicantDataManager.WhenToldTo(x => x.LinkDomiciliumAddressToApplicant(Param.IsAny<OfferRoleDomiciliumDataModel>()))
                .Return(applicationRoleDomiciliumKey);

            adUserKey = 173;
            adUserManager.WhenToldTo(x => x.GetAdUserKeyByUserName(Param.IsAny<string>())).Return(adUserKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_run_rules = () =>
        {
            domainRuleContext.WasToldTo(x => x.ExecuteRules(messages, applicantDomiciliumAddressModel));
        };

        private It should_not_contain_any_messages = () =>
        {
            messages.AllMessages.Count().ShouldEqual(0);
        };

        private It should_retrieve_the_client_role_key = () =>
        {
            applicantDataManager.WasToldTo(
                x => x.GetActiveApplicationRole(applicantDomiciliumAddressModel.ApplicationNumber, applicantDomiciliumAddressModel.ClientKey));
        };

        private It should_link_the_domicilium_address_to_the_applicant = () =>
        {
            applicantDataManager.WasToldTo(x => x.LinkDomiciliumAddressToApplicant(Param.IsAny<OfferRoleDomiciliumDataModel>()));
        };

        private It should_not_contain_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldEqual(0);
        };
    }
}
