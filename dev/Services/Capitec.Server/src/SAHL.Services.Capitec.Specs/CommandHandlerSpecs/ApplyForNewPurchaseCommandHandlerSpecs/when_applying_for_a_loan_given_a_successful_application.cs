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
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.DecisionTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.ApplyForNewPurchaseCommandHandlerSpecs
{
    public class when_applying_for_a_loan_given_a_successful_application : WithFakes
    {
        private static ILookupManager lookupService;
        private static IServiceQueryRouter serviceQueryRouter;
        private static IServiceCommandRouter serviceCommandRouter;
        private static IDecisionTreeServiceClient decisionTreeService;
        private static IApplicationManager applicationService;
        private static IApplicantManager applicantService;
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
        private static CalculatedLoanDetailsModel calculatedDetails;
        private static ServiceRequestMetadata metadata;
        private static IDecisionTreeResultManager decisionTreeResultService;
        private static IApplicationDataManager applicationDataService;
        private static ISecurityDataManager securityDataManager;
        private static Guid branchId;

        private Establish context = () =>
        {
            decisionTreeResultService = An<IDecisionTreeResultManager>();
            //itcProfile = new ITCProfile(null, new List<ITCJudgement>(), new List<ITCDefault>(), new List<ITCPaymentProfile>(), new List<ITCNotice>(), null, true);
            serviceCommandRouter = An<IServiceCommandRouter>();
            serviceQueryRouter = An<IServiceQueryRouter>();
            lookupService = An<ILookupManager>();
            decisionTreeService = An<IDecisionTreeServiceClient>();
            applicationService = An<IApplicationManager>();
            applicantService = An<IApplicantManager>();
            applicationDataService = An<IApplicationDataManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            communicationsService = An<ICommunicationsServiceClient>();

            stubs = new ApplicantStubs();
            newPurchaseLoanDetails = new NewPurchaseLoanDetails(new Guid(), new Guid(), 30000, 300000, 100000, 500, 240, true);
            //
            calculatedDetails = new CalculatedLoanDetailsModel();
            calculatedDetails.EligibleApplication = true;
            generatedID = Guid.NewGuid();
            applicationID = Guid.NewGuid();
            branchId = Guid.NewGuid();
            applicants = new List<Applicant>() { stubs.GetApplicant };
            application = new NewPurchaseApplication(newPurchaseLoanDetails, applicants, new Guid(), "1184050800000-0700");
            securityDataManager = An<ISecurityDataManager>();
            //
            handler = new ApplyForNewPurchaseCommandHandler(lookupService, serviceQueryRouter, serviceCommandRouter, decisionTreeService, applicationService
                , applicantService, decisionTreeResultService, applicationDataService, unitOfWorkFactory, securityDataManager, communicationsService);
            command = new ApplyForNewPurchaseCommand(applicationID, application);
            //
            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(generatedID);

            applicationService.WhenToldTo(x => x.PerformITCs(Param.IsAny<Dictionary<Guid, Applicant>>())).Return(true);
            applicationService.WhenToldTo(x => x.RecalculateNewPurchaseApplication(
                    Param<LoanApplication>.Matches(m => m.Applicants == application.Applicants),
                    Param<Dictionary<Guid, Applicant>>.Matches(m => m.Values.Contains(applicants[0])), true,
                    applicationID)).Return(true);
            securityDataManager.WhenToldTo(x => x.GetBranchForUser(application.UserId)).Return(new BranchDataModel(branchId, "Musgrave", Guid.NewGuid(), true, "2017"));
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_save_the_applicants = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<AddApplicantsCommand>.Matches(m=>
                m.Applicants.First().Value == stubs.GetApplicant), 
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_perform_an_ITC_check_for_the_applicant = () =>
        {
            applicationService.WasToldTo(x => x.PerformITCs(Param.IsAny<Dictionary<Guid, Applicant>>()));
        };

        private It should_get_the_next_application_key = () =>
        {
            applicationDataService.WasToldTo(x => x.GetNextApplicationNumber());
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
            applicantService.WasToldTo(x => x.AddApplicantToApplication(generatedID, applicationID));
        };

        private It should_recalculate_the_application = () =>
        {
            applicationService.WasToldTo(x => x.RecalculateNewPurchaseApplication(Param.IsAny<LoanApplication>(), Param.IsAny<Dictionary<Guid, Applicant>>(), true, applicationID));
        };

        private It should_notify_the_applicants_that_the_application_was_successful = () =>
        {
            communicationsService.WasToldTo(commSrv => commSrv.PerformCommand(Param<NotifyThatApplicationReceivedCommand>
                .Matches(m => m.Recipients.Select(r => r.CellPhoneNumber).Contains(stubs.GetApplicant.Information.CellPhoneNumber))
                , metadata));
        };

        private It should_create_an_sa_homeloans_application = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<CreateSAHomeLoansNewPurchaseApplicationCommand>.Matches(a => a.ApplicationType == Interfaces.Capitec.Common.Enumerations.ApplicationStatusEnums.InProgress && a.SystemMessageCollection.WarningMessages().Count() == 0), metadata));
        };
    }
}