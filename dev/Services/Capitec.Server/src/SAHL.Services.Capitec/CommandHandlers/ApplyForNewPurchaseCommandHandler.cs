﻿using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.Application;
using SAHL.Services.Capitec.Managers.Application.Models;
using SAHL.Services.Capitec.Managers.DecisionTreeResult;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.Managers.Security;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.Capitec.Common;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.ExternalServices.Notification;
using SAHL.Services.Interfaces.DecisionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Services.Capitec;


namespace SAHL.Services.Capitec.CommandHandlers
{
    public class ApplyForNewPurchaseCommandHandler : IServiceCommandHandler<ApplyForNewPurchaseCommand>
    {
        private ILookupManager lookupService;
        private IServiceQueryRouter serviceQueryRouter;
        private IServiceCommandRouter serviceCommandRouter;
        private IApplicationManager applicationService;
        private IApplicantManager applicantService;
        private IDecisionTreeServiceClient decisionTreeService;
        private IDecisionTreeResultManager decisionTreeResultService;
        private IApplicationDataManager applicationDataService;
        private IUnitOfWorkFactory unitOfWorkFactory;
        private ISecurityDataManager securityDataManager;
        private ICommunicationsServiceClient communicationsService;

        public ApplyForNewPurchaseCommandHandler(ILookupManager lookupService, IServiceQueryRouter serviceQueryRouter
            , IServiceCommandRouter serviceCommandRouter, IDecisionTreeServiceClient decisionTreeService, IApplicationManager applicationService, IApplicantManager applicantService, IDecisionTreeResultManager decisionTreeResultService, IApplicationDataManager applicationDataService, IUnitOfWorkFactory unitOfWorkFactory, ISecurityDataManager securityDataManager, ICommunicationsServiceClient communicationsService)
        {
            this.lookupService = lookupService;
            this.serviceQueryRouter = serviceQueryRouter;
            this.serviceCommandRouter = serviceCommandRouter;
            this.decisionTreeService = decisionTreeService;
            this.applicationService = applicationService;
            this.applicationDataService = applicationDataService;
            this.applicantService = applicantService;
            this.decisionTreeResultService = decisionTreeResultService;
            this.applicationDataService = applicationDataService;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.securityDataManager = securityDataManager;
            this.communicationsService = communicationsService;
        }

        public ISystemMessageCollection HandleCommand(ApplyForNewPurchaseCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            Dictionary<Guid, Applicant> applicantsToAdd = new Dictionary<Guid, Applicant>();
            var applicationID = command.ApplicationID;
            bool applicationPassed = false;
            bool itcsPassed = false;
            int applicationNumber;
            if (applicationService.DoesApplicationExist(command.ApplicationID))
            {
                return messages;
            }

            Enumerations.ApplicationStatusEnums applicationStatus;
            
            using (var uow = this.unitOfWorkFactory.Build())
            {
                foreach (var applicant in command.NewPurchaseApplication.Applicants)
                {
                    var applicantID = lookupService.GenerateCombGuid();
                    applicantsToAdd.Add(applicantID, applicant);
                }
              
                var addApplicantsCommand = new AddApplicantsCommand(applicantsToAdd);
                var serviceMessages = serviceCommandRouter.HandleCommand(addApplicantsCommand, new ServiceRequestMetadata());

                itcsPassed = applicationService.PerformITCs(applicantsToAdd);

                var loanDetails = new ApplicationLoanDetails(command.NewPurchaseApplication.NewPurchaseLoanDetails);
                BranchDataModel branch = securityDataManager.GetBranchForUser(command.NewPurchaseApplication.UserId);
                var application = new LoanApplication(command.NewPurchaseApplication.ApplicationDate, loanDetails, command.NewPurchaseApplication.Applicants, command.NewPurchaseApplication.UserId, command.NewPurchaseApplication.CaptureStartDate, branch.Id);
                applicationNumber = SaveApplication(applicationID, application, applicantsToAdd);
                applicationStatus = Enumerations.ApplicationStatusEnums.InProgress;
                applicationPassed = applicationService.RecalculateNewPurchaseApplication(application, applicantsToAdd, itcsPassed, applicationID);

                if (applicationPassed && itcsPassed)
                {
                    var recepients = applicantsToAdd.Values.Select(x => new Recipient(x.Information.CellPhoneNumber)).ToList();
                    NotifyThatApplicationReceivedCommand notifyCommand = new NotifyThatApplicationReceivedCommand(recepients, applicationNumber);
                    messages.Aggregate(this.communicationsService.PerformCommand(notifyCommand, metadata));
                }
                else
                {
                    foreach (var applicant in applicantsToAdd)
                    {
                        messages.Aggregate(decisionTreeResultService.GetITCResultForApplicant(applicant.Key).ITCMessages);
                    }

                    applicationStatus = Enumerations.ApplicationStatusEnums.PortalDecline;
                    applicationService.SetApplicationToDeclined(applicationID);

                    //Case fb2074 : Do not notify user that the application has failed.
                    messages.Aggregate(decisionTreeResultService.GetCalculationResultForApplication(applicationID).Messages);
                }

                var createSAHomeLoansNewPurchaseApplicationCommand = new CreateSAHomeLoansNewPurchaseApplicationCommand(applicationNumber, messages, applicationStatus, command.NewPurchaseApplication, command.ApplicationID);
                serviceCommandRouter.HandleCommand(createSAHomeLoansNewPurchaseApplicationCommand, metadata);

                uow.Complete();
            }
            //We think this was done on purpose.
            return SystemMessageCollection.Empty();
        }

        private int SaveApplication(Guid applicationID, LoanApplication application, Dictionary<Guid, Applicant> applicants)
        {
            var applicationNumber = applicationDataService.GetNextApplicationNumber();

            applicationService.AddLoanApplication(applicationNumber, applicationID, application);

            foreach (var applicant in applicants)
            {
                applicantService.AddApplicantToApplication(applicant.Key, applicationID);
            }

            return applicationNumber;
        }
    }
}