using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Domicilium;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.LinkDomiciliumAddressToApplicant
{
    class when_application_role_is_not_found : WithCoreFakes
    {
        static IDomiciliumDataManager domiciliumDataManager;
        static IApplicantDataManager applicantDataManager;
        static IApplicationDataManager applicationDataManager;
        static IDomainQueryServiceClient domainQueryService;
        static LinkDomiciliumAddressToApplicantCommandHandler handler;
        static LinkDomiciliumAddressToApplicantCommand command;
        static ApplicantDomiciliumAddressModel applicantDomiciliumAddressModel;
        static int clientDomiciliumKey, applicationNumber, clientKey;
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
            domainQueryService = An<IDomainQueryServiceClient>();
            domainRuleContext = An<IDomainRuleManager<ApplicantDomiciliumAddressModel>>();
            adUserManager = An<IADUserManager>();
            OfferRoleDataModel applicationRoleDataModel = null;
            applicantDomiciliumAddressModel = new ApplicantDomiciliumAddressModel(clientDomiciliumKey, applicationNumber, clientKey);
            command = new LinkDomiciliumAddressToApplicantCommand(applicantDomiciliumAddressModel);
            handler = new LinkDomiciliumAddressToApplicantCommandHandler(applicantDataManager, applicationDataManager, domiciliumDataManager, unitOfWorkFactory, eventRaiser,
                domainRuleContext, adUserManager);

            applicantDataManager.WhenToldTo(x => x.DoesApplicantHavePendingDomiciliumOnApplication(clientKey, clientDomiciliumKey)).Return(false);
            domiciliumDataManager.WhenToldTo(x => x.IsDomiciliumAddressPendingDomiciliumAddress(clientDomiciliumKey)).Return(true);
            applicantDataManager.WhenToldTo(x => x.GetActiveApplicationRole(applicantDomiciliumAddressModel.ApplicationNumber, applicantDomiciliumAddressModel.ClientKey))
                .Return(applicationRoleDataModel);

        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        It should_attempty_to_retrieve_the_application_role = () =>
        {
            applicantDataManager.WasToldTo(x => x.GetActiveApplicationRole(applicantDomiciliumAddressModel.ApplicationNumber, applicantDomiciliumAddressModel.ClientKey));
        };

        It should_not_link_the_domicilium_address_to_the_applicant = () =>
        {
            applicantDataManager.WasNotToldTo(x => x.LinkDomiciliumAddressToApplicant(Param.IsAny<OfferRoleDomiciliumDataModel>()));
        };

        It should_contain_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldNotEqual(0);
        };
    }
}
