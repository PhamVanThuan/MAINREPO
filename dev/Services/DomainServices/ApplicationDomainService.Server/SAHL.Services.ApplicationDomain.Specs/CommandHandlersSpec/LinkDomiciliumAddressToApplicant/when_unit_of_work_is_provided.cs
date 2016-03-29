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
    public class when_unit_of_work_is_provided : WithCoreFakes
    {
        private static IDomiciliumDataManager domiciliumDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static IApplicationDataManager applicationDataManager;
        private static IDomainQueryServiceClient domainQueryService;
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
            domainQueryService = An<IDomainQueryServiceClient>();
            adUserManager = An<IADUserManager>();

            OfferRoleDataModel applicationRoleDataModel = new OfferRoleDataModel(clientKey, 34, 5, (int)GeneralStatus.Active, DateTime.Now);
            applicantDomiciliumAddressModel = new ApplicantDomiciliumAddressModel(clientDomiciliumKey, applicationNumber, clientKey);
            applicantDataManager.WhenToldTo(x => x.GetActiveApplicationRole(Param.IsAny<int>(), Param.IsAny<int>())).Return(applicationRoleDataModel);
            adUserManager.WhenToldTo(x => x.GetAdUserKeyByUserName(Param.IsAny<string>())).Return(1234);
            command = new LinkDomiciliumAddressToApplicantCommand(applicantDomiciliumAddressModel);
            handler = new LinkDomiciliumAddressToApplicantCommandHandler(applicantDataManager, applicationDataManager, domiciliumDataManager
                , unitOfWorkFactory, eventRaiser, domainRuleContext,
                adUserManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_create_unit_of_work = () =>
        {
            unitOfWorkFactory.WasToldTo(x => x.Build());
        };

        private It should_complete_unit_of_work = () =>
        {
            unitOfWork.WasToldTo(x => x.Complete());
        };
    }
}