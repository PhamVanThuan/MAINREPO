using Automation.DataAccess.DataHelper;
using Automation.Framework.DataAccess;
using Common.Constants;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Automation.Framework
{
    public class DCWorkflow : WorkflowBase
    {
        /// <summary>
        /// Work to be done prior to completing respond to debt counsellor
        /// </summary>
        /// <param name="keyValue"></param>
        public void PriorToRespondToDebtCounsellor(int keyValue)
        {
            base.InsertCorrespondence(keyValue, 4060, 27);
        }

        public void PriorToNotificationOfDecision(int keyValue)
        {
            base.InsertCorrespondence(keyValue, 4074, 27);
        }

        /// <summary>
        /// Prior to performing the send proposal for approval activity we need to insert an active proposal and update the x2data table to handle
        /// the assignment of the DCSUser
        /// </summary>
        /// <param name="keyValue">debtCounsellingKey</param>
        public void PriorToSendProposalForApproval(int keyValue)
        {
            this.InsertProposal(keyValue, 1, 5, TestUsers.DebtCounsellingSupervisor, 1, 1);
            this.UpdateDataTableForAssignment(TestUsers.DebtCounsellingSupervisor, (int)WorkflowRoleTypeEnum.DebtCounsellingSupervisorD, keyValue);
        }

        /// <summary>
        /// Prior to performing the send proposal for approval activity we need to insert an active proposal and update the x2data table to handle
        /// the assignment of the DCSUser
        /// </summary>
        /// <param name="keyValue">debtCounsellingKey</param>
        public void PriorToSendProposalForApprovalNoProposalInsertRequired(int keyValue)
        {
            this.UpdateDataTableForAssignment(TestUsers.DebtCounsellingSupervisor, (int)WorkflowRoleTypeEnum.DebtCounsellingSupervisorD, keyValue);
        }

        /// <summary>
        /// Prior to performing the send proposal for approval activity we need to insert an active proposal and update the x2data table to handle
        /// the assignment of the DCSUser
        /// </summary>
        /// <param name="keyValue">debtCounsellingKey</param>
        public void PriorToSendProposalAssignToManager(int keyValue)
        {
            this.InsertProposal(keyValue, 1, 5, TestUsers.DebtCounsellingSupervisor, 1, 1);
            this.UpdateDataTableForAssignment(TestUsers.DebtCounsellingManager, (int)WorkflowRoleTypeEnum.RecoveriesManagerD, keyValue);
        }

        /// <summary>
        /// Prior to performing the payment received action we need to set the proposal review date and the payment received details
        /// </summary>
        /// <param name="keyValue"></param>
        public void PriorToPaymentReceived(int keyValue)
        {
            this.UpdatePaymentReceivedAmountAndDate(keyValue, 7500);
            this.UpdateProposalReviewDate(keyValue);
        }

        /// <summary>
        /// Code to run prior to proposal acceptance
        /// </summary>
        /// <param name="keyValue"></param>
        public void PriorToProposalAcceptance(int keyValue)
        {
            int proposalKey = base.GetProposals(keyValue).Where(x => x.ProposalTypeKey == ProposalTypeEnum.Proposal && x.ProposalStatusKey == ProposalStatusEnum.Active).Select(y => y.ProposalKey).FirstOrDefault();
            base.InsertReason(proposalKey, 524);
            this.MarkProposalAsAccepted(keyValue);
            this.CreateAccountSnapshot(keyValue);
        }

        public void PriorToPendCancellation(int keyValue)
        {
            var accountKey = this.GetAccountKeyByDebtCounsellingKey(keyValue);
            base.InsertDetailTypeForAccount(accountKey, DetailTypeEnum.UnderCancellation);
        }

        public void PriorToCaptureRecoveriesProposal(int keyValue)
        {
            var accountKey = this.GetAccountKeyByDebtCounsellingKey(keyValue);
            base.InsertRecoveriesProposal(accountKey, 50000.00, 2500.00, 1617, GeneralStatusEnum.Active);
        }

        public void PriorToEscalateRecoveriesProposal(int keyValue)
        {
            this.UpdateDataTableForAssignment(TestUsers.DebtCounsellingSupervisor, 5, keyValue);
        }

        public void PriorSendtoLitigation(int keyValue)
        {
            var accountKey = this.GetAccountKeyByDebtCounsellingKey(keyValue);
            var eworkDataHelper = new EWorkDataHelper();
            var results = eworkDataHelper.GetLossControlByeFolder(accountKey.ToString());
            string eStageName = results.Rows(0).Column("UserToDo").GetValueAs<string>();
            base.UpdateEworkAssignedUser(accountKey, TestUsers.ForeclosureConsultant, eStageName);
        }

        public void PriorToBondExclusionsArrears(int keyValue)
        {
            this.PutAccountIntoArrears(keyValue);
        }

        public void PriorToNotificationOfSequestration(int keyValue)
        {
            var externalRoles = base.GetExternalRoles(keyValue, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, false);
            int legalEntityKey = (from e in externalRoles select e.LegalEntityKey).FirstOrDefault();
            base.InsertReason(legalEntityKey, 575);
        }

        public void PriorToTerminateApplication(int keyValue)
        {
            //insert reason
            base.InsertReason(keyValue, 529);
        }

        public void PriorToSendTerminationLetter(int keyValue)
        {
            var clientRoles = base.GetExternalRoles(keyValue, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, false).Where(x => x.ExternalRoleTypeKey == ExternalRoleTypeEnum.Client);
            foreach (var client in clientRoles)
            {
                base.InsertDomiciliumAddressForLegalEntity(client.LegalEntityKey);
            }
        }

        public void PriorToEXTIntoArrears(int keyValue)
        {
            this.PutAccountIntoArrears(keyValue);
        }

        public void PriorToCourtDetails(int keyValue)
        {
            base.InsertCourtDetails(keyValue, HearingTypeEnum.Court, HearingAppearanceTypeEnum.CourtOrderGranted, DateTime.Now.AddDays(5), "Test");
        }

        public void PriorToAttorneyToOppose(int keyValue)
        {
            base.InsertExternalRole(keyValue, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, 474654, ExternalRoleTypeEnum.LitigationAttorney, GeneralStatusEnum.Active);
        }

        public void PriorToDecline(int keyValue)
        {
            int proposalKey = base.GetProposals(keyValue).Where(x => x.ProposalTypeKey == ProposalTypeEnum.Proposal && x.ProposalStatusKey == ProposalStatusEnum.Active).Select(y => y.ProposalKey).FirstOrDefault();
            base.InsertReason(proposalKey, 535);
        }

        public void PriorToDeclineProposalCourtOrderWithAppeal(int keyValue)
        {
            int proposalKey = base.GetProposals(keyValue).Where(x => x.ProposalTypeKey == ProposalTypeEnum.Proposal && x.ProposalStatusKey == ProposalStatusEnum.Active).Select(y => y.ProposalKey).FirstOrDefault();
            base.InsertReason(proposalKey, 576);
        }

        public void PostDecline(int keyValue)
        {
            SetStageTransitionForApproveDeclineReasons(keyValue, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_DeclineProposal);
        }

        public void PostApproval(int keyValue)
        {
            SetStageTransitionForApproveDeclineReasons(keyValue, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_AcceptProposal);
        }

        #region WorkerMethods

        private void PutAccountIntoArrears(int keyValue)
        {
            var accountKey = this.GetAccountKeyByDebtCounsellingKey(keyValue);
            base.PutAccountIntoArrears(accountKey);
        }

        private int GetAccountKeyByDebtCounsellingKey(int keyValue)
        {
            var account = DataHelper.GetAccountByDebtCounsellingKey(keyValue);
            return account.AccountKey;
        }

        public void InsertProposal(int debtCounsellingKey, int proposalStatusKey, int noOfProposalItems, string adUser, int hocInclusive, int lifeInclusive)
        {
            var parameters = new Dictionary<string, string>
                                 {
                                     {"@debtcounsellingkey", debtCounsellingKey.ToString()},
                                     {"@proposalstatuskey", proposalStatusKey.ToString()},
                                     {"@adusername", adUser},
                                     {"@proposalitems", noOfProposalItems.ToString()},
                                     {"@hocinclusive", hocInclusive.ToString()},
                                     {"@lifeinclusive", lifeInclusive.ToString()}
                                 };
            DataHelper.ExecuteProcedure("test.InsertProposal", parameters);
        }

        public void UpdateDataTableForAssignment(string adUser, int workflowRoleTypeKey, int key)
        {
            string sql = string.Format(@"update x2.x2data.debt_counselling
                            set AssignADUserName = '{0}', AssignWorkflowRoleTypeKey = {1}
                            where debtCounsellingKey = {2}", adUser, workflowRoleTypeKey, key);
            DataHelper.ExecuteAdHocSQL(sql);
        }

        public void CreateAccountSnapshot(int debtCounsellingKey)
        {
            var parameters = new Dictionary<string, string> { { "@debtCounsellingKey", debtCounsellingKey.ToString() } };
            DataHelper.ExecuteProcedure("[2am].dbo.pDebtCounsellingSnapshot", parameters);
        }

        public void MarkProposalAsAccepted(int debtCounsellingKey)
        {
            string sql = string.Format(@"update p
                                    set p.accepted = 1
                                    from [2am].debtcounselling.debtcounselling dc
                                    join [2am].debtcounselling.proposal p on dc.debtcounsellingkey = p.debtcounsellingkey
                                    and p.proposalStatusKey = 1 and p.proposalTypeKey = 1
                                    where dc.debtcounsellingKey = {0}", debtCounsellingKey);
            DataHelper.ExecuteAdHocSQL(sql);
        }

        public void UpdatePaymentReceivedAmountAndDate(int debtCounsellingKey, float paymentReceivedAmount)
        {
            string sql = string.Format(@"update [2am].debtcounselling.debtcounselling
                                        set paymentReceivedAmount = {0}, paymentReceivedDate = getdate()
                                        where debtCounsellingKey = {1}", paymentReceivedAmount, debtCounsellingKey);
            DataHelper.ExecuteAdHocSQL(sql);
        }

        public void UpdateProposalReviewDate(int debtCounsellingKey)
        {
            var sql = string.Format(@"update [2am].debtcounselling.proposal
                                        set reviewDate = dateadd(mm, +12, getdate())
                                        where debtCounsellingKey = {0} and proposalTypeKey = 1
                                        and proposalStatusKey = 1 and Accepted = 1", debtCounsellingKey);
            DataHelper.ExecuteAdHocSQL(sql);
        }

        private void SetStageTransitionForApproveDeclineReasons(int keyValue, StageDefinitionStageDefinitionGroupEnum sdsdgKey)
        {
            int proposalKey = base.GetProposals(keyValue).Where(x => x.ProposalTypeKey == ProposalTypeEnum.Proposal && x.ProposalStatusKey == ProposalStatusEnum.Active).Select(y => y.ProposalKey).FirstOrDefault();
            base.SetStageTransitionOnLatestReason(keyValue, proposalKey, sdsdgKey);
        }

        #endregion WorkerMethods
    }
}