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

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.ApplyForNewPurchaseCommandHandlerSpecs
{
    public class when_applying_for_a_loan_and_the_itc_for_an_applicant_fails : WithFakes
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

        private static ApplyForNewPurchaseCommandHandler handler;
        private static ApplyForNewPurchaseCommand command;
        private static NewPurchaseApplication application;
        private static List<Applicant> applicants;
        private static Guid applicationID;
        private static Guid generatedID;
        private static ISystemMessageCollection messages;
        private static ApplicantStubs stubs;
        //private static ITCProfile itcProfile;
        private static NewPurchaseLoanDetails newPurchaseLoanDetails;
        private static CreditBureauAssessmentResult itcResult;
        private static CreditPricingResult creditPricingResult;
        private static ServiceRequestMetadata metadata;
        private static ISecurityDataManager securityDataManager;

        private Establish context = () =>
        {
            //itcProfile = new ITCProfile(null, new List<ITCJudgement>(), new List<ITCDefault>(), new List<ITCPaymentProfile>(), new List<ITCNotice>(), null, true);
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
            newPurchaseLoanDetails = new NewPurchaseLoanDetails(new Guid(), new Guid(), 30000, 300000, 100000, 500, 240, true);
            //
            applicationID = Guid.NewGuid();
            applicants = new List<Applicant>() { stubs.GetApplicant, stubs.GetSecondApplicant };
            application = new NewPurchaseApplication(newPurchaseLoanDetails, applicants, new Guid(), "1184050800000-0700");
            branchId = Guid.NewGuid();
            //
            handler = new ApplyForNewPurchaseCommandHandler(lookupService, serviceQueryRouter, serviceCommandRouter, decisionTreeService, applicationService
                , applicantService, decisionTreeResultService, applicationDataService, unitOfWorkFactory, securityDataManager, communicationsService);
            command = new ApplyForNewPurchaseCommand(applicationID, application);
            //
            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(() => { return Guid.NewGuid(); });

            applicationService.WhenToldTo(x => x.PerformITCs(Param.IsAny<Dictionary<Guid, Applicant>>())).Return(false);
            applicationService.WhenToldTo(x => x.RecalculateNewPurchaseApplication(
                    Param<LoanApplication>.Matches(m => m.ApplicationDate == application.ApplicationDate && m.Applicants == application.Applicants),
                    Param<Dictionary<Guid, Applicant>>.Matches(m => m.Values.Contains(applicants[0])), false,
                    applicationID)).Return(true);
            securityDataManager.WhenToldTo(x => x.GetBranchForUser(application.UserId)).Return(new BranchDataModel(branchId, "Musgrave", Guid.NewGuid(), true, "2017"));

            itcResult = new CreditBureauAssessmentResult();
            itcResult.ITCMessages = new SystemMessageCollection();
            itcResult.ITCMessages.AddMessage(new SystemMessage("ITC has failed", SystemMessageSeverityEnum.Warning));
            decisionTreeResultService.WhenToldTo(x => x.GetITCResultForApplicant(Param.IsAny<Guid>())).Return(itcResult);

            creditPricingResult = new CreditPricingResult();
            creditPricingResult.Messages = SystemMessageCollection.Empty();
            decisionTreeResultService.WhenToldTo(x => x.GetCalculationResultForApplication(applicationID)).Return(creditPricingResult);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_save_the_applicant = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<AddApplicantsCommand>.Matches(m=>
                m.Applicants.Count == 2), 
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_perform_an_ITC_check_for_the_applicant = () =>
        {
            applicationService.WasToldTo(x => x.PerformITCs(Param.IsAny<Dictionary<Guid, Applicant>>()));
        };

        private It should_save_the_application = () =>
        {
            applicationService.WasToldTo(x => x.AddLoanApplication(0, applicationID, Param<LoanApplication>.Matches(m =>
                m.BranchId == branchId &&
                m.Applicants.Count() == application.Applicants.Count() &&
                m.LoanDetails.Deposit == application.NewPurchaseLoanDetails.Deposit)));
        };

        private It should_save_the_applicants_against_the_application = () =>
        {
            applicantService.WasToldTo(x => x.AddApplicantToApplication(Param.IsAny<Guid>(), applicationID)).Times(2);
        };

        private It should_still_recalculate_the_application = () =>
        {
            applicationService.WasToldTo(x => x.RecalculateNewPurchaseApplication(Param.IsAny<LoanApplication>(), Param.IsAny<Dictionary<Guid, Applicant>>(), false, applicationID));
        };

        private It should_set_the_application_status_to_declined = () =>
        {
            applicationService.WasToldTo(x => x.SetApplicationToDeclined(applicationID));
        };

        private It should_not_return_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_collect_the_messages_for_each_applicant = () =>
        {
            decisionTreeResultService.WasToldTo(x => x.GetITCResultForApplicant(Param.IsAny<Guid>())).Times(2);
        };

        private static Guid branchId;
    }
}