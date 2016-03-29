using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Domicilium;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.LinkDomiciliumAddressToApplicant
{
    public class when_aduser_exists : WithCoreFakes
    {
        static IDomiciliumDataManager domiciliumDataManager;
        static IApplicantDataManager applicantDataManager;
        static IApplicationDataManager applicationDataManager;
        static IDomainQueryServiceClient domainQueryService;
        static LinkDomiciliumAddressToApplicantCommandHandler handler;
        static LinkDomiciliumAddressToApplicantCommand command;
        static ApplicantDomiciliumAddressModel applicantDomiciliumAddressModel;
        static int clientDomiciliumKey, applicationNumber, clientKey;
        static int ADUserKey;
        static IDomainRuleManager<ApplicantDomiciliumAddressModel> domainRuleContext;
        static IADUserManager adUserManager;

        Establish context = () =>
        {
            clientDomiciliumKey = 100;
            applicationNumber = 123;
            clientKey = 9090;
            applicantDataManager = An<IApplicantDataManager>();
            applicationDataManager = An<IApplicationDataManager>();
            domiciliumDataManager = An<IDomiciliumDataManager>();
            domainRuleContext = An<IDomainRuleManager<ApplicantDomiciliumAddressModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();
            adUserManager = An<IADUserManager>();

            OfferRoleDataModel applicationRoleDataModel = new OfferRoleDataModel(clientKey, 34, 5, (int)GeneralStatus.Active, DateTime.Now);
            applicantDomiciliumAddressModel = new ApplicantDomiciliumAddressModel(clientDomiciliumKey, applicationNumber, clientKey);
            command = new LinkDomiciliumAddressToApplicantCommand(applicantDomiciliumAddressModel);
            handler = new LinkDomiciliumAddressToApplicantCommandHandler(applicantDataManager, applicationDataManager
                , domiciliumDataManager, unitOfWorkFactory, eventRaiser,
                domainRuleContext, adUserManager);

            applicantDataManager.WhenToldTo(x => x.DoesApplicantHavePendingDomiciliumOnApplication(clientKey, clientDomiciliumKey)).Return(false);
            domiciliumDataManager.WhenToldTo(x => x.IsDomiciliumAddressPendingDomiciliumAddress(clientDomiciliumKey)).Return(true);
            applicantDataManager.WhenToldTo(
                x => x.GetActiveApplicationRole(
                    applicantDomiciliumAddressModel.ApplicationNumber, applicantDomiciliumAddressModel.ClientKey)).Return(applicationRoleDataModel);


            //get ad user key
            ADUserKey = 173;
            adUserManager.WhenToldTo(x => x.GetAdUserKeyByUserName(Param.IsAny<string>())).Return(ADUserKey);

            adUserManager.WhenToldTo(x => x.GetAdUserKeyByUserName(Param.IsAny<string>())).Return(1);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        It should_retrieve_the_aduser_key = () =>
        {
            adUserManager.WasToldTo(x => x.GetAdUserKeyByUserName(serviceRequestMetaData.UserName));
        };

        It should_not_contain_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldEqual(0);
        };
    }
}
