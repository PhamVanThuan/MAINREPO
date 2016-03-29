using System;
using System.Data;
using System.Linq;
using DomainService2.SharedServices;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using X2DomainService.Interface.Common;
using System.Collections.Generic;
using SAHL.Common;

namespace DomainService2.Workflow.DebtCounselling
{
    public class CancelDebtCounselling_EXTCommandHandler : IHandlesDomainServiceCommand<CancelDebtCounselling_EXTCommand>
    {
        private IX2WorkflowService x2WorkflowService;
        private IDebtCounsellingRepository debtCounsellingRepository;
        private ICommonRepository commonRepository;
        private IMessageService messageService;
        private ICommon commonWorkflowService;
        private ILookupRepository lookupRepository;
        private IWorkflowSecurityRepository workflowSecurityRepository;
        private IOrganisationStructureRepository organisationStructureRepository;

        public CancelDebtCounselling_EXTCommandHandler(IX2WorkflowService x2WorkflowService,
                                                       IDebtCounsellingRepository debtCounsellingRepository,
                                                       ICommonRepository commonRepository,
                                                       IMessageService messageService,
                                                       ICommon commonWorkflowService,
                                                       ILookupRepository lookupRepository,
                                                       IWorkflowSecurityRepository workflowSecurityRepository,
                                                       IOrganisationStructureRepository organisationStructureRepository)
        {
            this.x2WorkflowService = x2WorkflowService;
            this.debtCounsellingRepository = debtCounsellingRepository;
            this.commonRepository = commonRepository;
            this.messageService = messageService;
            this.commonWorkflowService = commonWorkflowService;
            this.lookupRepository = lookupRepository;
            this.workflowSecurityRepository = workflowSecurityRepository;
            this.organisationStructureRepository = organisationStructureRepository;
        }

        public void Handle(IDomainMessageCollection messages, CancelDebtCounselling_EXTCommand command)
        {
            bool success = false;
            bool stageTransitionExists = x2WorkflowService.HasInstancePerformedActivity(command.InstanceID, command.ActivityName);

            IDebtCounselling debtCounsellingCase = debtCounsellingRepository.GetDebtCounsellingByKey(command.DebtCounsellingKey);
            if (debtCounsellingCase.DebtCounsellor == null)
            {
                messages.Add(new Error("Debt Counsellor for Case does not exist", "Debt Counsellor for Case does not exist"));
            }
            else
            {
                string debtCounsellorEmailAddress = debtCounsellingCase.DebtCounsellor.EmailAddress;
                string debtCounsellingConsultantEmailAddress = String.Empty;
                int aDUserKey = 0;

                List<WorkflowRoleTypes> assignment = new List<WorkflowRoleTypes>(){
                    WorkflowRoleTypes.DebtCounsellingConsultantD,
                    WorkflowRoleTypes.DebtCounsellingCourtConsultantD
                };

                SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole.WorkflowAssignment.WFRAssignmentRow assignmentRow = workflowSecurityRepository.GetWorkflowRoleAssignment(assignment, command.InstanceID).WFRAssignment.FirstOrDefault();
                if(assignmentRow != null)
                {
                    aDUserKey = assignmentRow.ADUserKey;
                }

                if (aDUserKey > 0)
                {
                    IADUser aDuser = organisationStructureRepository.GetADUserByKey(aDUserKey);
                    if (aDuser != null && aDuser.LegalEntity != null && !string.IsNullOrEmpty(aDuser.LegalEntity.EmailAddress))
                        debtCounsellingConsultantEmailAddress = aDuser.LegalEntity.EmailAddress;
                }

                string accountLegalName = String.Empty;
                string legalNumber = String.Empty;

                accountLegalName = debtCounsellingCase.Account.GetLegalName(LegalNameFormat.Full);

                foreach (ILegalEntity legalEntity in debtCounsellingCase.Clients)
                {
                    if (legalEntity.UnderDebtCounselling)
                    {
                        if (!string.IsNullOrEmpty(legalEntity.LegalNumber))
                        {
                            legalNumber += legalEntity.LegalNumber + " & ";
                        }
                    }
                }

                if (legalNumber.EndsWith(" & "))
                    legalNumber = legalNumber.Substring(0, legalNumber.Length - 3);

                /*
                 * If there is a Recoveries Proposal Received state transition against the case,
                 * then email the Debt Counselor and notify them that the mortgage bond has been cancelled and they need to continue payments to the Recoveries Proposal.
                 * An eWorks case must be created for Recoveries process and the Recoveries Proposal Data to be inserted as a note on eWorks for their information.
                 */
                ICorrespondenceTemplate correspondenceTemplate;
                if (stageTransitionExists)
                {
                    correspondenceTemplate = commonRepository.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.MortgageLoanCancelledContinuePaying);
                }
                /*
                 * If there is no Recoveries Proposal Received state transition against the case
                 * then email the Debt Counselor and notify them that the mortgage bond has been cancelled and
                 * they do not need to make any further payments. No eworks case is created for
                 */
                else
                {
                    correspondenceTemplate = commonRepository.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.MortgageLoanCancelledDontContinuePaying);
                }

                bool notificationSendSucceeded = messageService.SendEmailExternal(command.DebtCounsellingKey, "no-reply@sahomeloans.com", debtCounsellorEmailAddress, debtCounsellingConsultantEmailAddress, String.Empty, String.Format(correspondenceTemplate.Subject, debtCounsellingCase.Account.Key.ToString()), String.Format(correspondenceTemplate.Template, debtCounsellingCase.Account.Key.ToString(), accountLegalName, legalNumber), "", "", "");
                if (!notificationSendSucceeded)
                {
                    messages.Add(new Error("SendNotificationsCancellatonRegistered_EXT: Notification to Debt Counsellor could not be sent", "SendNotificationsCancellatonRegistered_EXT: Notification to Debt Counsellor could not be sent"));
                }
                else
                {
                    debtCounsellingRepository.SendNotification(debtCounsellingCase);
                }
            }

            //Perform E-work action
            if (stageTransitionExists)
            {
                // check if there is a e-work case
                string eFolderID = string.Empty;
                string eStageName = string.Empty;
                IADUser eADUser = null;
                debtCounsellingRepository.GetEworkDataForLossControlCase(debtCounsellingCase.Account.Key, out eStageName, out eFolderID, out eADUser);

                if (!String.IsNullOrEmpty(eStageName))
                {
                    string eADUserName = "";
                    if (eADUser != null)
                        eADUserName = eADUser.ADUserName;

                    if (!commonWorkflowService.PerformEWorkAction(messages, eFolderID, Constants.EworkActionNames.X2ReturnDebtCounselling, debtCounsellingCase.Account.Key, eADUserName, eStageName))
                        throw new Exception(string.Format("Unable to perform EWork Action: {0}", Constants.EworkActionNames.X2ReturnDebtCounselling));
                }
            }

            //Update Debt Counselling Status
            if (debtCounsellingCase != null)
            {
                debtCounsellingCase.DebtCounsellingStatus = lookupRepository.DebtCounsellingStatuses[DebtCounsellingStatuses.Cancelled];
                debtCounsellingRepository.SaveDebtCounselling(debtCounsellingCase);
                success = true;
            }

            command.Result = success;
        }
    }
}