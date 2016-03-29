using System;
using System.Collections.Generic;
using System.Linq;
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
using SAHL.Services.Capitec.Managers.DecisionTreeResult.Models;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.Managers.Security;
using SAHL.Services.Capitec.Specs.Stubs;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.Communications;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.ApplyForSwitchLoanCommandHandlerSpecs
{
    public class when_applying_for_a_loan_given_the_application_is_unsuccessful : WithFakes
    {
        private static ILookupManager lookupService;
        private static IServiceQueryRouter serviceQueryRouter;
        private static IServiceCommandRouter serviceCommandRouter;
        private static IDecisionTreeServiceClient decisionTreeService;
        private static IApplicationManager applicationService;
        private static IApplicantManager applicantService;
        private static IApplicationDataManager applicationDataService;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static ICommunicationsServiceClient communicationsService;

        private static ApplyForSwitchLoanCommandHandler handler;
        private static ApplyForSwitchLoanCommand command;
        private static SwitchLoanApplication application;
        private static List<Applicant> applicants;
        private static Guid applicationID;
        private static Guid generatedID;
        private static ISystemMessageCollection messages;
        private static ApplicantStubs stubs;
        private static SwitchLoanDetails switchLoanDetails;
        private static IDecisionTreeResultManager decisionTreeResultService;
        private static CreditPricingResult creditPricingResult;
        private static CreditBureauAssessmentResult itcResult;
        private static ServiceRequestMetadata metadata;
        private static Guid branchId;
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
            stubs = new ApplicantStubs();
            branchId = Guid.NewGuid();
            switchLoanDetails = new SwitchLoanDetails(Guid.Parse(OccupancyTypeEnumDataModel.OWNER_OCCUPIED), Guid.Parse(EmploymentTypeEnumDataModel.SALARIED), 40000, 1500000, 1000000, 1200000, 5000, 8, 240, true);
            //
            generatedID = Guid.NewGuid();
            applicationID = Guid.NewGuid();
            applicants = new List<Applicant>() { stubs.GetApplicant };
            application = new SwitchLoanApplication(switchLoanDetails, applicants, new Guid(), "1184050800000-0700");
            //
            handler = new ApplyForSwitchLoanCommandHandler(lookupService, serviceQueryRouter, serviceCommandRouter, applicationService
                , applicantService, decisionTreeResultService, applicationDataService, unitOfWorkFactory, securityDataManager, communicationsService);
            command = new ApplyForSwitchLoanCommand(applicationID, application);
            //
            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(generatedID);
            applicationService.WhenToldTo(x => x.PerformITCs(Param.IsAny<Dictionary<Guid, Applicant>>())).Return(true);
            applicationService.WhenToldTo(x => x.RecalculateSwitchApplication(
                Param<LoanApplication>.Matches(m => m.ApplicationDate == application.ApplicationDate && m.Applicants == application.Applicants),
                Param<Dictionary<Guid, Applicant>>.Matches(m => m.Values.Contains(applicants[0])), true, applicationID)).Return(false);
            //
            creditPricingResult = new CreditPricingResult();
            creditPricingResult.Messages = SystemMessageCollection.Empty();
            creditPricingResult.Messages.AddMessage(new SystemMessage("Application calculate failed.", SystemMessageSeverityEnum.Warning));
            decisionTreeResultService.WhenToldTo(x => x.GetCalculationResultForApplication(applicationID)).Return(creditPricingResult);
            //
            //
            itcResult = new CreditBureauAssessmentResult();
            itcResult.ITCMessages = SystemMessageCollection.Empty();
            decisionTreeResultService.WhenToldTo(x => x.GetITCResultForApplicant(generatedID)).Return(itcResult);
            securityDataManager.WhenToldTo(x => x.GetBranchForUser(application.UserId)).Return(new BranchDataModel(branchId, "Musgrave", Guid.NewGuid(), true, "2017"));
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_get_the_next_application_key = () =>
        {
            applicationDataService.WasToldTo(x => x.GetNextApplicationNumber());
        };

        private It should_save_the_application = () =>
        {
            applicationService.WasToldTo(x => x.AddLoanApplication(0, applicationID, Param<LoanApplication>.Matches(m => m.BranchId == branchId && m.ApplicationDate == application.ApplicationDate && m.Applicants == application.Applicants)));
        };

        private It should_recalculate_the_application = () =>
        {
            applicationService.WasToldTo(x => x.RecalculateSwitchApplication(
                Param<LoanApplication>.Matches(m => m.ApplicationDate == application.ApplicationDate && m.Applicants == application.Applicants),
                Param.IsAny<Dictionary<Guid, Applicant>>(), true, applicationID));
        };

        private It should_set_the_application_status_to_declined = () =>
        {
            applicationService.WasToldTo(x => x.SetApplicationToDeclined(applicationID));
        };

        private It should_create_an_sa_homeloans_application = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<CreateSAHomeLoansSwitchApplicationCommand>.Matches(a => a.ApplicationType == Interfaces.Capitec.Common.Enumerations.ApplicationStatusEnums.PortalDecline
                && a.SystemMessageCollection.WarningMessages().FirstOrDefault(w => creditPricingResult.Messages.WarningMessages().Contains(w)) != null), metadata));
        };
    }
}