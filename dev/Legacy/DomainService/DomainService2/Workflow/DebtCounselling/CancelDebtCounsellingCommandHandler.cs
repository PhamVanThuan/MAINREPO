namespace DomainService2.Workflow.DebtCounselling
{
    using System;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Globals;
    using X2DomainService.Interface.Common;
    using SAHL.Common;

    public class CancelDebtCounsellingCommandHandler : IHandlesDomainServiceCommand<CancelDebtCounsellingCommand>
    {
        private IDebtCounsellingRepository DebtCounsellingRepository;
        private IReasonRepository ReasonRepository;
        private ICommon CommonWorkflowService;

        public CancelDebtCounsellingCommandHandler(IDebtCounsellingRepository debtcounsellingRepository, IReasonRepository reasonRepository, ICommon commonWorkflowService)
        {
            this.DebtCounsellingRepository = debtcounsellingRepository;
            this.ReasonRepository = reasonRepository;
            this.CommonWorkflowService = commonWorkflowService;
        }

        public void Handle(IDomainMessageCollection messages, CancelDebtCounsellingCommand command)
        {
            // get the debt counselling record
            IDebtCounselling debtCounselling = DebtCounsellingRepository.GetDebtCounsellingByKey(command.DebtCounsellingKey);
            if (debtCounselling == null)
                throw new Exception("No DebtCounselling record exists.");

            DebtCounsellingRepository.SendNotification(debtCounselling);

            

            // get the latest reason definition that was selected for the cancellation
            IReasonDefinition latestReasonDefinition = ReasonRepository.GetReasonDefinitionByKey(command.LatestReasonDefintionKey);

            // only perform the SAHL.Common.Constants.EworkActionNames.X2ReturnDebtCounselling - NO case creation required as per our agreement with BA
            // IF "DCCancelledClientSequestrated" THEN ignore
            // IF "CaseCreatedInError" and no case exists in e-work THEN ignore
            // IF "CaseCreatedInError" and case exists in e-work THEN move it in e-work back to the stage it was in prior to Debt Counselling
            if (latestReasonDefinition != null && latestReasonDefinition.ReasonType.Key == (int)ReasonTypes.DebtCounsellingCancelled && latestReasonDefinition.ReasonDescription.Key != (int)ReasonDescriptions.DCCancelledClientSequestrated)
            {
                // check if there is a e-work case
                string eFolderID = string.Empty;
                string eStageName = string.Empty;
                IADUser eADUser = null;
                DebtCounsellingRepository.GetEworkDataForLossControlCase(debtCounselling.Account.Key, out eStageName, out eFolderID, out eADUser);

                if (!String.IsNullOrEmpty(eStageName))
                {
                    string eADUserName = "";
                    if (eADUser != null)
                        eADUserName = eADUser.ADUserName;

                    if (!CommonWorkflowService.PerformEWorkAction(messages, eFolderID, Constants.EworkActionNames.X2ReturnDebtCounselling, debtCounselling.Account.Key, eADUserName, eStageName))
                        throw new Exception(string.Format("Unable to perform EWork Action: {0}", Constants.EworkActionNames.X2ReturnDebtCounselling));
                }
            }
        }
    }
}