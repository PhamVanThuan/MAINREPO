using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicantDeclarations
{
    public class when_unit_of_work_is_provided : WithCoreFakes
    {
        private static AddApplicantDeclarationsCommand command;
        private static AddApplicantDeclarationsCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static IDomainRuleManager<ApplicantDeclarationsModel> domainRuleContext;
        private static ApplicantDeclarationsModel applicantDeclarations;
        private static IDomainQueryServiceClient domainQueryService;

        private Establish context = () =>
        {
            //create mock objects
            applicationDataManager = An<IApplicationDataManager>();
            domainRuleContext = An<IDomainRuleManager<ApplicantDeclarationsModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();
            applicantDataManager = An<IApplicantDataManager>();

            //new up handler
            handler = new AddApplicantDeclarationsCommandHandler(
                serviceCommandRouter, applicationDataManager, domainQueryService, domainRuleContext
                , eventRaiser, applicantDataManager, unitOfWorkFactory);

            //new up command
            applicantDeclarations = new ApplicantDeclarationsModel(1234, 67, DateTime.MinValue,
                new DeclaredInsolventDeclarationModel(OfferDeclarationAnswer.No, null),
                new UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer.No, DateTime.MinValue),
                new CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer.No, null),
                new PermissionToConductCreditCheckDeclarationModel(OfferDeclarationAnswer.Yes));
            command = new AddApplicantDeclarationsCommand(applicantDeclarations);

            applicantDataManager.WhenToldTo(x => x.GetActiveClientRoleOnApplication(applicantDeclarations.ApplicationNumber, applicantDeclarations.ClientKey))
                .Return(new List<OfferRoleDataModel>());
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
