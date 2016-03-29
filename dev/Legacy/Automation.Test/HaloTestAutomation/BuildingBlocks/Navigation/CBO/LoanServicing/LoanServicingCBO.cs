using Common.Enums;
using ObjectMaps.CBO.LoanServicing;

namespace BuildingBlocks.Navigation.CBO.LoanServicing
{
    public class LoanServicingCBO : LoanServicingControls
    {
        /// <summary>
        /// Selects the second loan account link. This is used for the Mortgage Loan Account, which appears twice in the CBO.
        /// </summary>
        /// <param name="accountKey">Account Number</param>
        public void ParentAccountNode(int accountKey)
        {
            base.ParentLoanAccount(accountKey).Click();
        }

        /// <summary>
        /// Selects the first loan account link. This is used for the Mortgage Loan Account, which appears twice in the CBO.
        /// </summary>
        /// <param name="accountKey">Account Number</param>
        public void LoanAccountNode(int accountKey)
        {
            base.LoanAccount(accountKey).Click();
        }

        /// <summary>
        /// Selects the Life Account
        /// </summary>
        /// <param name="accountKey">Account Number of the Life account</param>
        public void LifeAccountNode(int accountKey)
        {
            base.LifeAccount(accountKey).Click();
        }

        /// <summary>
        /// Navigates to the Variable Loan Node.
        /// </summary>
        /// <param name="accountKey">The HOC Acccount Number. Required in case the Legal Entity has multiple loan accounts.</param>
        public void VariableLoanNode(int accountKey)
        {
            base.VariableLoan(accountKey).Click();
        }

        /// <summary>
        /// Navigates to the Legal Entity Parent Node
        /// </summary>
        /// <param name="LegalEntityName">Legal Entity Full Name</param>
        public void LegalEntityParentNode(string LegalEntityName)
        {
            base.LegalEntityParentNode(LegalEntityName).Click();
        }

        /// <summary>
        /// Navigate to the Home Owners Cover (HOC) node
        /// </summary>
        /// <param name="HOCAccountKey">The HOC Acccount Number. Required in case the Legal Entity has multiple HOC accounts.</param>
        public void HomeOwnersCoverNode(int HOCAccountKey)
        {
            base.HomeOwnersCover(HOCAccountKey).Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        public void Correspondence(int index = 0)
        {
            base.CorrespondenceNodes[index].Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void LossControl()
        {
            base.LossControl.Click();
            base.DebtCounselling.Click();
        }

        public void LifePolicyClaim(NodeTypeEnum nodeType)
        {
            if (base.LifePolicyClaim.Exists)
            {
                base.LifePolicyClaim.Click();
                switch (nodeType)
                {
                    case NodeTypeEnum.Add:
                        base.LifePolicyClaimAdd.Click();
                        break;

                    case NodeTypeEnum.Update:
                        base.LifePolicyClaimUpdate.Click();
                        break;
                }
            }
        }

        public void CATSDisbursement(NodeTypeEnum nodeType)
        {
            if (base.CATSDisbursement.Exists)
            {
                base.CATSDisbursement.Click();
                switch (nodeType)
                {
                    case NodeTypeEnum.Add:
                        base.ManageDisbursements.Click();
                        break;

                    case NodeTypeEnum.Delete:
                        base.DeleteDisbursements.Click();
                        break;

                    case NodeTypeEnum.View:
                        break;

                    case NodeTypeEnum.Rollback:
                        base.RollbackDisbursements.Click();
                        break;
                }
            }
        }

        public void LoanTransactions()
        {
            base.LoanTransactions.Click();
        }

        public void PostTransactions()
        {
            base.PostTransactions.Click();
        }

        public void RollbackTransactions()
        {
            base.RollbackTransactions.Click();
        }

        public void LoanAdjustments()
        {
            base.LoanAdjustments.Click();
        }

        public void FurtherLendingCalculator()
        {
            base.FurtherLendingCalculator.Click();
        }

        public void RemoveLegalEntities()
        {
            while (base.imageCollectionTreeImage.Count > 0)
            {
                base.imageCollectionTreeImage[0].Click();
            }
        }

        public void CancelLifePolicy()
        {
            base.CancelPolicy.Click();
        }

        public void ChangeTerm()
        {
            base.ChangeTerm.Click();
        }

        public void UpdateFinancialAdjustments()
        {
            base.UpdateFinancialAdjustments.Click();
        }

        public void AddSuretor()
        {
            base.Suretor.Click();
            base.AddSuretor.Click();
        }

        public void CaptureClientSurvey()
        {
            base.CaptureClientSurvey.Click();
        }

        public void ArrearTransactions()
        {
            base.ArrearTransactions.Click();
        }

        public void PostArrearTransactions()
        {
            base.PostArrearTransactions.Click();
        }

        public void CreateDisabilityClaim()
        {
            base.CreateDisabilityClaim.Click();
        }

        /// <summary>
        /// Navigates to the manual debit order screens.
        /// </summary>
        /// <param name="nodeType">Node Type</param>
        public void ManualDebitOrders(NodeTypeEnum nodeType)
        {
            if (base.ManualDebitOrders.Exists)
            {
                base.ManualDebitOrders.Click();
                switch (nodeType)
                {
                    case NodeTypeEnum.Add:
                        base.AddManualDebitOrder.Click();
                        break;

                    case NodeTypeEnum.Update:
                        base.UpdateManualDebitOrder.Click();
                        break;

                    case NodeTypeEnum.Delete:
                        base.DeleteManualDebitOrder.Click();
                        break;

                    case NodeTypeEnum.View:
                        break;
                }
            }
        }

        public void DeleteManualDebitOrder()
        {
            base.DeleteManualDebitOrder.Click();
        }

        public void ClickLegalEntityDetails()
        {
            base.LegalEntityDetails.Click();
        }

        public void ClickUpdateLegalEntityDetails()
        {
            base.UpdateLegalEntityDetails.Click();
        }

        public void ClickEnableUpdateLegalEntity()
        {
            base.EnableUpdateLegalEntity.Click();
        }

        public void UpdateBond()
        {
            base.UpdateBond.Click();
        }

        public void AddLoanAgreement()
        {
            base.AddLoanAgreement.Click();
        }

        /// <summary>
        /// Navigates to the address details screens
        /// </summary>
        /// <param name="nodeType"></param>
        public void AddressDetails(NodeTypeEnum nodeType)
        {
            if (base.AddressDetails.Exists)
            {
                base.AddressDetails.Click();
                switch (nodeType)
                {
                    case NodeTypeEnum.Update:
                        base.UpdateAddressDetails.Click();
                        break;

                    case NodeTypeEnum.Add:
                        base.AddAddressDetails.Click();
                        break;

                    case NodeTypeEnum.Delete:
                        base.DeleteAddressDetails.Click();
                        break;

                    case NodeTypeEnum.View:
                        break;
                }
            }
        }

        /// <summary>
        /// Navigates to the Legal Entity Memo screens on the Loan Servicing CBO
        /// </summary>
        /// <param name="nodeType"></param>
        public void LegalEntityMemo(NodeTypeEnum nodeType)
        {
            base.LegalEntityMemo.Click();
            switch (nodeType)
            {
                case NodeTypeEnum.Update:
                    base.UpdateLegalEntityMemo.Click();
                    break;

                case NodeTypeEnum.Add:
                    base.AddLegalEntityMemo.Click();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Navigates to the loan detail screens.
        /// </summary>
        /// <param name="nodeType"></param>
        public void LoanDetail(NodeTypeEnum nodeType)
        {
            if (base.LoanDetailSummary.Exists)
            {
                base.LoanDetailSummary.Click();
                switch (nodeType)
                {
                    case NodeTypeEnum.Update:
                        base.UpdateLoanDetail.Click();
                        break;

                    case NodeTypeEnum.Add:
                        base.AddLoanDetail.Click();
                        break;

                    case NodeTypeEnum.Delete:
                        base.DeleteLoanDetail.Click();
                        break;

                    case NodeTypeEnum.View:
                        break;
                }
            }
        }

        /// <summary>
        /// Navigates to the Assets Liabilities screens on the Loan Servicing CBO
        /// </summary>
        /// <param name="nodeType"></param>
        public void AssetsLiabilities(NodeTypeEnum nodeType)
        {
            base.AssetsAndLiabilities.Click();
            switch (nodeType)
            {
                case NodeTypeEnum.Update:
                    base.UpdateAssetsAndLiabilities.Click();
                    break;

                case NodeTypeEnum.Add:
                    base.AddAssetsAndLiabilities.Click();
                    break;

                case NodeTypeEnum.Delete:
                    base.DeleteAssetsAndLiabilities.Click();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Click the Property Update link on the Loan Servicing CBO
        /// </summary>
        /// <param name="nodeType"></param>
        public void ClickUpdateProperty()
        {
            base.UpdateProperty.Click();
        }

        /// <summary>
        /// Click the Update Deeds Office on the Loan Servicing CBO
        /// </summary>
        public void ClickUpdateDeedsOfficeDetails()
        {
            base.UpdateDeedsOfficeDetails.Click();
        }

        /// <summary>
        /// Clieck the Update Deeds Office link on the Loan Servicing CBO
        /// </summary>
        public void ClickUpdatePropertyAddress()
        {
            base.UpdatePropertyAddress.Click();
        }

        /// <summary>
        /// Clickks on the Remove Suretor action
        /// </summary>
        public void RemoveSuretor()
        {
            base.RemoveSuretor.Click();
        }

        /// <summary>
        /// Clicks on the Update HOC Details action.
        /// </summary>
        public void UpdateHOCDetails()
        {
            base.UpdateHOCDetails.Click();
        }

        /// <summary>
        /// Handles navigation for valuation details
        /// </summary>
        /// <param name="nodeType"></param>
        public void ClickValuationDetails(NodeTypeEnum nodeType)
        {
            base.ValuationDetails.Click();
            switch (nodeType)
            {
                case NodeTypeEnum.Update:
                    base.UpdateValuation.Click();
                    break;

                case NodeTypeEnum.Add:
                    base.AddValuation.Click();
                    break;

                case NodeTypeEnum.Request:
                    base.RequestPhysicalValuation.Click();
                    break;
            }
        }

        /// <summary>
        /// Handles navigation for fixed debit orders.
        /// </summary>
        /// <param name="nodeType"></param>
        public void FixedDebitOrders(NodeTypeEnum nodeType)
        {
            switch (nodeType)
            {
                case NodeTypeEnum.Update:
                    base.UpdateFixedDebitOrder.Click();
                    break;

                case NodeTypeEnum.Delete:
                    base.DeleteFixedDebitOrder.Click();
                    break;

                case NodeTypeEnum.View:
                    base.FixedDebitOrderSummary.Click();
                    break;
            }
        }

        public void LoanStatement()
        {
            base.LoanStatement.Click();
        }

        public void LegalInstructionForeclosure()
        {
            base.LegalInstructionForeclosure.Click();
        }

        public void LegalInstructionLetterofDemand()
        {
            base.LegalInstructionLetterofDemand.Click();
        }

        public void NCALeviedChargesForm27()
        {
            base.NCALeviedChargesForm27.Click();
        }

        public void MS65Form()
        {
            base.MS65Form.Click();
        }

        public void CapLetter()
        {
            base.CapLetter.Click();
        }

        public void SuperLoIntroductionLetter()
        {
            base.SuperLoIntroductionLetter.Click();
        }

        public void BondCancelledLetter()
        {
            base.BondCancelledLetter.Click();
        }

        public void SendEndorsementLetter()
        {
            base.SendEndorsementLetter.Click();
        }

        public void HOCCancellationLetter()
        {
            base.HOCCancellationLetter.Click();
        }

        public void HOCEndorsementAndSchedule()
        {
            base.HOCEndorsementAndSchedule.Click();
        }

        public void HOCCoverLetterAndPolicySchedule()
        {
            base.HOCCoverLetterAndPolicySchedule.Click();
        }

        public void InstalmentLetter()
        {
            base.InstalmentLetter.Click();
        }

        public void AcknowledgeCancellationInstruction()
        {
            base.AcknowledgeCancellationInstruction.Click();
        }

        public void IncomeTaxStatement()
        {
            base.IncomeTaxStatement.Click();
        }

        public void Z299()
        {
            base.Z299.Click();
        }

        public void InsolvenciesPowerofAttorney()
        {
            base.InsolvenciesPowerofAttorney.Click();
        }

        public void InsolvenciesStatementofAccount()
        {
            base.InsolvenciesStatementofAccount.Click();
        }

        public void InsolvenciesClaimAffidavit()
        {
            base.InsolvenciesClaimAffidavit.Click();
        }

        public void PreDebtCounsellingAccountDetails()
        {
            base.PreDebtCounsellingAccountDetails.Click();
        }

        public void ConvertStaffLoan()
        {
            base.ConvertStaffLoan.Click();
        }

        public void ChangeRate()
        {
            base.ChangeRate.Click();
        }

        public void AccountMailingAddress(NodeTypeEnum nodeType)
        {
            if (base.AccountMailingAddress.Exists)
            {
                base.AccountMailingAddress.Click();
                switch (nodeType)
                {
                    case NodeTypeEnum.Update:
                        base.UpdateAccountMailingAddress.Click();
                        break;
                }
            }
        }

        public void ChangeInstalment()
        {
            base.ChangeInstalment.Click();
        }

        public void MarkNonPerforming()
        {
            base.MarkNonPerforming.Click();
        }

        /// <summary>
        /// Navigates to the debit order details screens.
        /// </summary>
        /// <param name="nodeType">Node Type</param>
        public void DebitOrderDetails(NodeTypeEnum nodeType)
        {
            if (base.DebitOrderDetails.Exists)
            {
                base.DebitOrderDetails.Click();
                switch (nodeType)
                {
                    case NodeTypeEnum.Add:
                        base.AddDebitOrder.Click();
                        break;

                    case NodeTypeEnum.Update:
                        base.UpdateDebitOrder.Click();
                        break;

                    case NodeTypeEnum.Delete:
                        base.DeleteDebitOrder.Click();
                        break;

                    case NodeTypeEnum.View:
                        break;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        public void ClickNodes(string node)
        {
            base.GetLink(node).Click();
        }

        /// <summary>
        /// Navigate to the Cancel Cap node
        /// </summary>
        public void CancelCap()
        {
            base.AcceptedCapHistory.Click();
            base.CancelCap.Click();
        }

        /// <summary>
        /// Navigate to the rollback arrear transactions node
        /// </summary>
        public void RollbackArrearTransaction()
        {
            base.ArrearTransactions.Click();
            base.RollbackArrearTransactions.Click();
        }

        public void ClickCreatePersonalLoanLead()
        {
            base.CreatePersonalLoanLead.Click();
        }

        /// <summary>
        /// Navigate to thePersonal Loans node
        /// </summary>
        /// <param name="accountKey">The HOC Acccount Number. Required in case the Legal Entity has multiple HOC accounts.</param>
        public void PersonalLoansNode(int accountkey)
        {
            base.PersonalLoan.Click();
        }

        public void CapitalisationLetter()
        {
            base.CapitalisationLetter.Click();
        }

        public void SendForm23Letter()
        {
            base.HOCForm23Letter.Click();
        }

        public void ExternalLife()
        {
            base.ExternalLife.Click();
        }
    }
}