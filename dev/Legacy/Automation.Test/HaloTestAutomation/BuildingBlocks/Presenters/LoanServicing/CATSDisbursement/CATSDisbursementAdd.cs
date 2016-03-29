using Automation.DataAccess;
using BuildingBlocks.Assertions;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using ObjectMaps.Pages;
using System;
using System.Collections.Generic;
using System.Threading;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LoanServicing.CATSDisbursement
{
    public class CATSDisbursementAdd : CATSDisbursementAddControls
    {
        private readonly IApplicationService applicationService;
        private readonly ILegalEntityService legalEntityService;
        private readonly IWatiNService watinService;
        private readonly ICommonService commonService;
        private readonly IBankingDetailsService bankingDetailService;
        private readonly IDisbursementService disbursementService;

        public CATSDisbursementAdd()
        {
            applicationService = ServiceLocator.Instance.GetService<IApplicationService>();
            legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
            bankingDetailService = ServiceLocator.Instance.GetService<IBankingDetailsService>();
            disbursementService = ServiceLocator.Instance.GetService<IDisbursementService>();
        }

        /// <summary>
        /// Posts a CAP Readvance against an Account that has a CAP offer awaiting a readvance in the CAP2 Workflow
        /// </summary>
        public void PostCAP2Readvance()
        {
            base.DisbursementTypeSelectList.Select(DisbursementTransactionTypes.CAP2ReAdvance);
            Thread.Sleep(1500);
            //click the add button
            base.btnAdd.Click();
            watinService.HandleConfirmationPopup(base.btnSave);
            Thread.Sleep(1500);
            //click the post button
            watinService.HandleConfirmationPopup(base.btnPost);
        }

        /// <summary>
        /// Selects a specific disbursement type from the drop down list
        /// </summary>
        /// <param name="disbursementType">Disbursement Type to Select</param>
        public void SelectDisbursementType(string disbursementType)
        {
            base.DisbursementTypeSelectList.Select(disbursementType);
            base.Document.DomContainer.WaitForComplete();
        }

        /// <summary>
        /// This is used to post a Readvance Disbursement for a Further Lending Application
        /// </summary>
        /// <param name="b">IE TestBrowser Object</param>
        /// <param name="offerKey">OfferKey for the Application</param>
        /// <param name="splitToValuations">TRUE = split disbursement between client and the SAHL Val acct, FALSE = client only</param>
        /// <param name="postDisbursement">TRUE = Post the Disbursement, FALSE = Only add the disbursement</param>
        /// <param name="readvAmt">Readvance Value</param>
        /// <param name="valAmt">Amount to split to the Val Account</param>
        public void PostReadvance(int offerKey, bool splitToValuations, out double valAmt, out double readvAmt, bool postDisbursement)
        {
            string randAmount;
            string centsAmount;
            //we need to get the amount to post
            QueryResults r = applicationService.GetLatestOfferInformationByOfferKey(offerKey);
            string amount = r.Rows(0).Column("LoanAgreementAmount").Value;
            //get the bank accounts to disburse to
            string clientBankAccount = bankingDetailService.GetBankAccountString("Client", offerKey).RemoveDoubleSpace();
            string valBankAccount = bankingDetailService.GetBankAccountString("Valuations", offerKey).RemoveDoubleSpace();
            //if the client bank account doesnt exist we need to add it prior to disbursing
            if (string.IsNullOrEmpty(clientBankAccount))
            {
                int legalEntityKey = applicationService.GetLegalEntityKeyOfFirstActiveOfferRole(offerKey, OfferRoleTypeEnum.MainApplicant);
                legalEntityService.InsertLegalEntityBankAccount(legalEntityKey);
                //fetch the bank details again
                clientBankAccount = bankingDetailService.GetBankAccountString("Client", offerKey).RemoveDoubleSpace();
            }
            base.DisbursementTypeSelectList.Select(DisbursementTransactionTypes.ReAdvance);
            Thread.Sleep(500);
            //populate the amount
            commonService.SplitRandsCents(out randAmount, out centsAmount, amount);
            base.txtTotalAmountRands.Value = randAmount;
            base.txtTotalAmountCents.Value = centsAmount;
            //populate the reference
            base.txtDisbursementReference.Value = String.Format(@"{0} : Readvance Disbursement Test", offerKey);
            Thread.Sleep(500);
            if (!splitToValuations)
            {
                Thread.Sleep(500);
                base.BankDetailsSelectList.Option(clientBankAccount).Select();
                Thread.Sleep(1500);
                base.txtTotalAmountToDisburseRands.Value = randAmount;
                base.txtTotalAmountToDisburseCents.Value = centsAmount;
                readvAmt = Convert.ToDouble(amount);
                valAmt = 0.00;
                //click the add button
                base.btnAdd.Click();
                base.Document.DomContainer.WaitForComplete();
                Thread.Sleep(2000);
                SaveAndPost(postDisbursement);
            }
            else
            {
                double readvanceAmt = Convert.ToDouble(amount);
                const double valFee = 1000.00;
                readvanceAmt = readvanceAmt - valFee;
                valAmt = valFee;
                readvAmt = readvanceAmt;
                //split the readvance amount
                commonService.SplitRandsCents(out randAmount, out centsAmount, readvanceAmt.ToString());
                Thread.Sleep(500);
                base.BankDetailsSelectList.Option(clientBankAccount).Select();
                Thread.Sleep(1500);
                base.txtTotalAmountToDisburseRands.Value = randAmount;
                base.txtTotalAmountToDisburseCents.Value = centsAmount;

                //add the disbursement
                base.btnAdd.Click();
                base.Document.DomContainer.WaitForComplete();
                Thread.Sleep(2000);
                //split the val fee
                commonService.SplitRandsCents(out randAmount, out centsAmount, valFee.ToString());
                base.BankDetailsSelectList.Option(valBankAccount).Select();
                base.Document.DomContainer.WaitForComplete();
                base.txtTotalAmountToDisburseRands.Value = randAmount;
                base.txtTotalAmountToDisburseCents.Value = centsAmount;
                base.btnAdd.Click();
                SaveAndPost(postDisbursement);
            }
        }

        /// <summary>
        /// This methd does the work of clicking on the SAVE and POST buttons, whilst handling the dialogue popup boxes
        /// for confirmation
        /// </summary>
        /// <param name="post">TRUE = post the disbursement, FALSE = save the disbursement only</param>
        private void SaveAndPost(bool post)
        {
            Thread.Sleep(1500);
            //need to find the dialogue box after the save button
            watinService.HandleConfirmationPopup(base.btnSave);
            base.Document.DomContainer.WaitForComplete();
            //click the post button
            if (post)
                watinService.HandleConfirmationPopup(base.btnPost);
        }

        /// <summary>
        /// This is used to post a Readvance Disbursement for a Further Lending Application
        /// </summary>
        /// <param name="offerKey">OfferKey for the Application</param>
        /// <param name="splitToValuations">TRUE = split disbursement between client and the SAHL Val acct, FALSE = client only</param>
        /// <param name="postDisbursement">TRUE = Post the Disbursement, FALSE = Only add the disbursement</param>
        /// <param name="readvAmt">Readvance Value</param>
        /// <param name="valAmt">Amount to split to the Val Account</param>
        /// <param name="reference">Disbursement Reference</param>
        public void PostReadvance(int offerKey, bool splitToValuations, out double valAmt, out double readvAmt, bool postDisbursement,
            out string reference)
        {
            string randAmount, centsAmount;
            //we need to get the amount to post
            QueryResults results = applicationService.GetLatestOfferInformationByOfferKey(offerKey);
            string amount = results.Rows(0).Column("LoanAgreementAmount").Value;
            //get the bank accounts to disburse to
            string clientBankAccount = bankingDetailService.GetBankAccountString("Client", offerKey).RemoveDoubleSpace();
            string valBankAccount = bankingDetailService.GetBankAccountString("Valuations", offerKey).RemoveDoubleSpace();
            //if the client bank account doesnt exist we need to add it prior to disbursing
            if (string.IsNullOrEmpty(clientBankAccount))
            {
                int legalEntityKey = applicationService.GetLegalEntityKeyOfFirstActiveOfferRole(offerKey, OfferRoleTypeEnum.MainApplicant);
                legalEntityService.InsertLegalEntityBankAccount(legalEntityKey);
                clientBankAccount = bankingDetailService.GetBankAccountString("Client", offerKey).RemoveDoubleSpace();
            }

            base.DisbursementTypeSelectList.Select(DisbursementTransactionTypes.ReAdvance);
            Thread.Sleep(500);
            //populate the amount
            commonService.SplitRandsCents(out randAmount, out centsAmount, amount);
            base.txtTotalAmountRands.Value = randAmount;
            base.txtTotalAmountCents.Value = centsAmount;
            //populate the reference
            reference = String.Format(@"{0} : Readvance Disbursement Test", offerKey);
            base.txtDisbursementReference.Value = reference;
            Thread.Sleep(500);
            if (!splitToValuations)
            {
                Thread.Sleep(500);
                base.BankDetailsSelectList.Option(clientBankAccount).Select();
                Thread.Sleep(1500);
                base.txtTotalAmountToDisburseRands.Value = randAmount;
                base.txtTotalAmountToDisburseCents.Value = centsAmount;
                readvAmt = Convert.ToDouble(amount);
                valAmt = 0.00;
                //click the add button
                base.btnAdd.Click();
                base.Document.DomContainer.WaitForComplete();
                Thread.Sleep(2000);
                SaveAndPost(postDisbursement);
            }
            else
            {
                double readvanceAmt = Convert.ToDouble(amount);
                const double valFee = 1000.00;
                readvanceAmt = readvanceAmt - valFee;
                valAmt = valFee;
                readvAmt = readvanceAmt;
                //split the readvance amount
                commonService.SplitRandsCents(out randAmount, out centsAmount, readvanceAmt.ToString());
                Thread.Sleep(500);
                base.BankDetailsSelectList.Option(clientBankAccount).Select();
                Thread.Sleep(1500);
                base.txtTotalAmountToDisburseRands.Value = randAmount;
                base.txtTotalAmountToDisburseCents.Value = centsAmount;
                //add the disbursement
                base.btnAdd.Click();
                base.Document.DomContainer.WaitForComplete();
                Thread.Sleep(2000);
                base.BankDetailsSelectList.Refresh();
                base.txtTotalAmountToDisburseRands.Refresh();
                base.txtTotalAmountToDisburseCents.Refresh();
                //split the val fee
                commonService.SplitRandsCents(out randAmount, out centsAmount, valFee.ToString());
                base.BankDetailsSelectList.Option(valBankAccount).Select();
                Thread.Sleep(1500);
                base.txtTotalAmountToDisburseRands.Value = randAmount;
                base.txtTotalAmountToDisburseCents.Value = centsAmount;
                base.btnAdd.Click();
                SaveAndPost(postDisbursement);
            }
        }

        /// <summary>
        /// This method will capture the disbursement reference and click the Post button.
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="reference">Disbursement Reference</param>
        public void Post(int offerKey, out string reference)
        {
            reference = String.Format(@"{0} : Readvance Disbursement Test", offerKey.ToString());
            base.txtDisbursementReference.Value = reference;
            watinService.HandleConfirmationPopup(base.btnPost);
        }

        /// <summary>
        /// Add a disbursement record on the CATSDisbursementAdd view
        /// </summary>
        /// <param name="disbursementType"></param>
        /// <param name="totalAmountToDisburse"></param>
        /// <param name="disbursementReference"></param>
        /// <param name="bankDetails"></param>
        /// <param name="amount"></param>
        /// <param name="button"></param>
        public void AddDisbursement(string disbursementType, decimal totalAmountToDisburse, string disbursementReference, string bankDetails, decimal amount, ButtonTypeEnum button)
        {
            if (!string.IsNullOrEmpty(disbursementType))
                base.DisbursementTypeSelectList.Option(disbursementType).Select();
            if (totalAmountToDisburse >= 0)
                watinService.PopulateRandCentsFields(base.txtTotalAmountToDisburseRands, base.txtTotalAmountToDisburseCents, totalAmountToDisburse);
            if (!string.IsNullOrEmpty(disbursementReference))
                base.txtDisbursementReference.Value = disbursementReference;
            if (!string.IsNullOrEmpty(bankDetails))
                base.BankDetailsSelectList.Option(bankDetails).Select();
            if (amount >= 0)
                watinService.PopulateRandCentsFields(base.txtTotalAmountRands, base.txtTotalAmountCents, amount);

            ClickButton(button);
        }

        /// <summary>
        /// Add a disbursement record on the CATSDisbursementAdd view
        /// </summary>
        /// <param name="disbursementType"></param>
        /// <param name="totalAmountToDisburse"></param>
        /// <param name="disbursementReference"></param>
        /// <param name="bankDetails"></param>
        /// <param name="amount"></param>
        /// <param name="button"></param>
        public void AddDisbursement(string disbursementType, decimal totalAmountToDisburse, string disbursementReference, int bankDetails, decimal amount, ButtonTypeEnum button)
        {
            if (!string.IsNullOrEmpty(disbursementType))
                base.DisbursementTypeSelectList.Option(disbursementType).Select();
            if (totalAmountToDisburse >= 0)
                watinService.PopulateRandCentsFields(base.txtTotalAmountRands, base.txtTotalAmountCents, totalAmountToDisburse);
            if (!string.IsNullOrEmpty(disbursementReference))
                base.txtDisbursementReference.Value = disbursementReference;
            if (bankDetails >= 0)
                base.BankDetailsSelectList.Options[bankDetails].Select();
            if (amount >= 0)
                watinService.PopulateRandCentsFields(base.txtTotalAmountToDisburseRands, base.txtTotalAmountToDisburseCents, amount);

            ClickButton(button);
        }

        /// <summary>
        /// Add a disbursement record on the CATSDisbursementAdd view
        /// </summary>
        /// <param name="disbursementType"></param>
        /// <param name="totalAmountToDisburse"></param>
        /// <param name="disbursementReference"></param>
        /// <param name="bankDetails"></param>
        /// <param name="amount"></param>
        /// <param name="button"></param>
        public void AddDisbursement(int disbursementType, decimal totalAmountToDisburse, string disbursementReference, int bankDetails, decimal amount, ButtonTypeEnum button)
        {
            if (disbursementType >= 0)
                base.DisbursementTypeSelectList.Options[disbursementType].Select();
            if (totalAmountToDisburse >= 0)
                watinService.PopulateRandCentsFields(base.txtTotalAmountRands, base.txtTotalAmountCents, totalAmountToDisburse);
            if (!string.IsNullOrEmpty(disbursementReference))
                base.txtDisbursementReference.TypeText(disbursementReference);
            if (bankDetails >= 0)
                base.BankDetailsSelectList.Options[bankDetails].Select();
            if (amount >= 0)
                watinService.PopulateRandCentsFields(base.txtTotalAmountToDisburseRands, base.txtTotalAmountToDisburseCents, amount);

            ClickButton(button);
        }

        /// <summary>
        /// Click the specified button on the CATSDisbursementAdd view
        /// </summary>
        /// <param name="button">ButtonType</param>
        public void ClickButton(ButtonTypeEnum button)
        {
            switch (button)
            {
                case ButtonTypeEnum.Add:
                    base.btnAdd.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.btnCancel.Click();
                    break;

                case ButtonTypeEnum.Save:
                    watinService.HandleConfirmationPopup(base.btnSave);
                    break;

                case ButtonTypeEnum.Delete:
                    watinService.HandleConfirmationPopup(base.btnDelete);
                    break;

                case ButtonTypeEnum.Post:
                    watinService.HandleConfirmationPopup(base.btnPost);
                    break;

                default:
                    break;
            }
        }

        #region Validation

        /// <summary>
        /// Assert the expected controls exist on the CATSDisbursementAdd view
        /// </summary>
        public void AssertCATSDisbursementAddControlsExist_NoDisbursementRecords()
        {
            var enabledAddControls = new List<Element>() {
                    base.DisbursementTypeSelectList,
                    base.txtTotalAmountToDisburseRands,
                    base.txtTotalAmountToDisburseCents,
                    base.txtDisbursementReference,
                    base.BankDetailsSelectList,
                    base.txtTotalAmountRands,
                    base.txtTotalAmountCents,
                    base.btnCancel
                };

            WatiNAssertions.AssertFieldsExist(enabledAddControls);
            WatiNAssertions.AssertFieldsAreEnabled(enabledAddControls);

            var disabledAddControls = new List<Element>() {
                    base.btnAdd,
                    base.btnDelete
                };

            WatiNAssertions.AssertFieldsExist(disabledAddControls);
            WatiNAssertions.AssertFieldsAreDisabled(disabledAddControls);
        }

        /// <summary>
        /// Assert the expected controls exist on the CATSDisbursementAdd view
        /// </summary>
        public void AssertCATSDisbursementAddControlsExist_PendingDisbursementRecords()
        {
            var enabledAddControls = new List<Element>() {
                    base.txtTotalAmountToDisburseRands,
                    base.txtTotalAmountToDisburseCents,
                    base.txtDisbursementReference,
                    base.BankDetailsSelectList,
                    base.txtTotalAmountRands,
                    base.txtTotalAmountCents,
                    base.btnAdd,
                    base.btnDelete,
                    base.btnCancel,
                    base.btnPost
                };

            WatiNAssertions.AssertFieldsExist(enabledAddControls);
            WatiNAssertions.AssertFieldsAreEnabled(enabledAddControls);

            var disabledAddControls = new List<Element>() {
                    base.DisbursementTypeSelectList,
                    base.btnSave
                };

            WatiNAssertions.AssertFieldsExist(disabledAddControls);
            WatiNAssertions.AssertFieldsAreDisabled(disabledAddControls);
        }

        /// <summary>
        /// Assert the expected options exist in the Disbursement Type dropdown list
        /// </summary>
        public void AssertDisbursementTypeOptions(List<string> disbursementTypes)
        {
            WatiNAssertions.AssertSelectListContents(base.DisbursementTypeSelectList, disbursementTypes, true);
        }

        public void AssertBankAccountList(List<string> bankAccounts)
        {
            WatiNAssertions.AssertSelectListContents(base.BankDetailsSelectList, bankAccounts, true);
        }

        #endregion Validation
    }
}