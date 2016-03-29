using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.Application;
using SAHL.Services.Capitec.Managers.Application.Models;
using SAHL.Services.Capitec.Managers.DecisionTreeResult;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.Managers.Security;
using SAHL.Services.Capitec.Specs.Stubs;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.DecisionTree;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.ApplyForSwitchLoanCommandHandlerSpecs
{
    public class when_applying_for_a_loan_and_the_application_exists : WithFakes
    {
        private static ILookupManager lookupService;
        private static IServiceQueryRouter serviceQueryRouter;
        private static IServiceCommandRouter serviceCommandRouter;
        private static IDecisionTreeServiceClient decisionTreeService;
        private static IApplicationManager applicationService;
        private static IApplicantManager applicantService;
        private static IDecisionTreeResultManager decisionTreeResultService;
        private static IApplicationDataManager applicationDataService;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static ICommunicationsServiceClient communicationsService;

        private static ApplyForSwitchLoanCommandHandler handler;
        private static ApplyForSwitchLoanCommand command;
        private static SwitchLoanApplication application;
        private static Guid applicationID;
        private static ISystemMessageCollection messages;
        private static SwitchLoanDetails switchLoanDetails;
        private static ServiceRequestMetadata metadata;
        private static ISecurityDataManager securityDataManager;

        private Establish context = () =>
        {
            serviceCommandRouter = An<IServiceCommandRouter>();
            serviceQueryRouter = An<IServiceQueryRouter>();
            lookupService = An<ILookupManager>();
            decisionTreeService = An<IDecisionTreeServiceClient>();
            applicationService = An<IApplicationManager>();
            applicantService = An<IApplicantManager>();
            decisionTreeResultService = An<IDecisionTreeResultManager>();
            applicationDataService = An<IApplicationDataManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            securityDataManager = An<ISecurityDataManager>();
            var stubs = new ApplicantStubs();
            switchLoanDetails = new SwitchLoanDetails(Guid.Parse(OccupancyTypeEnumDataModel.OWNER_OCCUPIED), Guid.Parse(EmploymentTypeEnumDataModel.SALARIED), 40000, 1500000, 1000000, 1200000, 5000, 8, 240, true);
            //
            applicationID = Guid.NewGuid();
            var applicants = new List<Applicant>() { stubs.GetApplicant };
            application = new SwitchLoanApplication(switchLoanDetails, applicants, new Guid(), "1184050800000-0700");
            //
            handler = new ApplyForSwitchLoanCommandHandler(lookupService, serviceQueryRouter, serviceCommandRouter, applicationService, applicantService
                , decisionTreeResultService, applicationDataService, unitOfWorkFactory, securityDataManager, communicationsService);
            command = new ApplyForSwitchLoanCommand(applicationID, application);
            //
            applicationService.WhenToldTo(x => x.DoesApplicationExist(applicationID)).Return(true);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_not_save_the_applicant = () =>
        {
            serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param.IsAny<AddApplicantsCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_perform_an_ITC_check_for_the_applicant = () =>
        {
            applicationService.WasNotToldTo(x => x.PerformITCs(Param.IsAny<Dictionary<Guid, Applicant>>()));
        };

        private It should_not_save_the_application = () =>
        {
            applicationService.WasNotToldTo(x => x.AddLoanApplication(0, applicationID, Param.IsAny<LoanApplication>()));
        };

        private It should_not_save_the_applicants_against_the_application = () =>
        {
            applicantService.WasNotToldTo(x => x.AddApplicantToApplication(Param.IsAny<Guid>(), applicationID));
        };

        private It should_not_return_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}