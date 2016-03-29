using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Services.LifeDomain.CommandHandlers;
using SAHL.Services.LifeDomain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.LifeDomain.Specs.CommandHandlers.SendDisabilityClaimManualApprovalLetter
{
    public class when_told_to_send_disability_claim_approval_letter_manually : WithCoreFakes
    {
        private static SendDisabilityClaimManualApprovalLetterCommandHandler handler;
        private static SendDisabilityClaimManualApprovalLetterCommand command;
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static ICommunicationsServiceClient communicationsServiceClient;
        private static IServiceQueryRouter serviceQueryRouter;

        private Establish context = () =>
            {
                serviceQueryRouter = An<IServiceQueryRouter>();
                lifeDomainDataManager = An<ILifeDomainDataManager>();
                communicationsServiceClient = An<ICommunicationsServiceClient>();

                var serviceQueryResult = new ServiceQueryResult<DisabilityClaimDetailModel>(new DisabilityClaimDetailModel[]
                {
                    new DisabilityClaimDetailModel(1,1,1,1,"name",DateTime.Now,DateTime.Now,
                              DateTime.Now,"claimant",1,"mental","not well"
                            , DateTime.Now,1,"claim status",DateTime.Now,1,DateTime.Now)
                });

                serviceQueryRouter.WhenToldTo<IServiceQueryRouter>(x => x.HandleQuery(Param.IsAny<GetDisabilityClaimByKeyQuery>()))
                    .Callback<GetDisabilityClaimByKeyQuery>(y => { y.Result = serviceQueryResult; });

                var serviceQueryResult_2 = new ServiceQueryResult<DisabilityClaimFurtherLendingExclusionModel>(new DisabilityClaimFurtherLendingExclusionModel[]
                {
                    new DisabilityClaimFurtherLendingExclusionModel(1,"desc",DateTime.Now,10.00)
                });

                serviceQueryRouter.WhenToldTo<IServiceQueryRouter>(x => x.HandleQuery(Param.IsAny<GetFurtherLendingExclusionsByDisabilityClaimKeyQuery>()))
                    .Callback<GetFurtherLendingExclusionsByDisabilityClaimKeyQuery>(y => { y.Result = serviceQueryResult_2; });

                lifeDomainDataManager.WhenToldTo(x => x.GetEmailAddressForUserWhoApprovedDisabilityClaim(Param.IsAny<int>())).Return("emailaddress");

                command = new SendDisabilityClaimManualApprovalLetterCommand(1);
                handler = new SendDisabilityClaimManualApprovalLetterCommandHandler(
                                  lifeDomainDataManager
                                , communicationsServiceClient
                                , combGuid
                                , serviceQueryRouter
                                , eventRaiser
                          );
            };

        private Because of = () =>
            {
                messages = handler.HandleCommand(command, serviceRequestMetaData);
            };

        private It should_get_email_address_for_aduser = () =>
        {
            lifeDomainDataManager.WasToldTo(x => x.GetEmailAddressForUserWhoApprovedDisabilityClaim(Param.IsAny<int>()));
        };

        private It should_get_the_life_account_policy_key = () =>
        {
            serviceQueryRouter.WasToldTo(x => x.HandleQuery(Param.IsAny<GetDisabilityClaimByKeyQuery>()));
        };

        private It should_get_the_exclusions = () =>
        {
            serviceQueryRouter.WasToldTo(x => x.HandleQuery(Param.IsAny<GetFurtherLendingExclusionsByDisabilityClaimKeyQuery>()));
        };

        private It should_send_an_internal_email_to_the_aduser = () =>
        {
            communicationsServiceClient.WasToldTo(x => x.PerformCommand(Param.IsAny<SendInternalEmailCommand>(), Param.IsAny<ServiceRequestMetadata>()));
        };

        private It should_raise_an_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(
                  Param.IsAny<DateTime>()
                , Param.IsAny<DisabilityClaimManualApprovalLetterSentEvent>()
                , Param.IsAny<int>()
                , Param.IsAny<int>()
                , Param.IsAny<IServiceRequestMetadata>()
             ));
        };
    }
}
