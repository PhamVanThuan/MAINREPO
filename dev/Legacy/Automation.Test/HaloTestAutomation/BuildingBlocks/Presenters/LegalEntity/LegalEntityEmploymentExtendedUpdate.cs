using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.LegalEntity
{
    public class LegalEntityEmploymentExtendedUpdate : LegalEntityEmploymentExtendedUpdateControls
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="TestBrowser"></param>
        /// <param name="iBasicIncome"></param>
        /// <param name="iCommission"></param>
        /// <param name="iOvertime"></param>
        /// <param name="iShift"></param>
        /// <param name="iPerformance"></param>
        /// <param name="iAllowances"></param>
        /// <param name="iPAYE"></param>
        /// <param name="iUIF"></param>
        /// <param name="iPensionProvidentRA"></param>
        /// <param name="iMedicalAid"></param>
        public void UpdateExtendedEmploymentDetails_MonthlyIncome(int iBasicIncome, int iCommission, int iOvertime, int iShift,
            int iPerformance, int iAllowances, int iPAYE, int iUIF, int iPensionProvidentRA, int iMedicalAid)
        {
            if (iBasicIncome >= 0) base.txtMonthBasicIncome_txtRands.TypeText(iBasicIncome.ToString());
            if (iCommission >= 0) base.txtMonthCommission_txtRands.TypeText(iCommission.ToString());
            if (iOvertime >= 0) base.txtMonthOvertime_txtRands.TypeText(iOvertime.ToString());
            if (iShift >= 0) base.txtMonthShift_txtRands.TypeText(iShift.ToString());
            if (iPerformance >= 0) base.txtMonthPerformance_txtRands.TypeText(iPerformance.ToString());
            if (iAllowances >= 0) base.txtMonthAllowances_txtRands.TypeText(iAllowances.ToString());
            if (iPAYE >= 0) base.txtMonthPAYE_txtRands.TypeText(iPAYE.ToString());
            if (iUIF >= 0) base.txtMonthUIF_txtRands.TypeText(iUIF.ToString());
            if (iPensionProvidentRA >= 0) base.txtMonthPension_txtRands.TypeText(iPensionProvidentRA.ToString());
            if (iMedicalAid >= 0) base.txtMonthMedicalAid_txtRands.TypeText(iMedicalAid.ToString());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="TestBrowser"></param>
        /// <param name="iBasicIncome"></param>
        /// <param name="iCommission"></param>
        /// <param name="iOvertime"></param>
        /// <param name="iShift"></param>
        /// <param name="iPerformance"></param>
        /// <param name="iAllowances"></param>
        /// <param name="iPAYE"></param>
        /// <param name="iUIF"></param>
        /// <param name="iPensionProvidentRA"></param>
        /// <param name="iMedicalAid"></param>
        public void UpdateExtendedEmploymentDetails_ConfirmedIncome(int iBasicIncome, int iCommission, int iOvertime, int iShift,
            int iPerformance, int iAllowances, int iPAYE, int iUIF, int iPensionProvidentRA, int iMedicalAid)
        {
            if (iBasicIncome >= 0) base.txtConfBasicIncome_txtRands.TypeText(iBasicIncome.ToString());
            if (iCommission >= 0) base.txtConfCommission_txtRands.TypeText(iCommission.ToString());
            if (iOvertime >= 0) base.txtConfOvertime_txtRands.TypeText(iOvertime.ToString());
            if (iShift >= 0) base.txtConfShift_txtRands.TypeText(iShift.ToString());
            if (iPerformance >= 0) base.txtConfPerformance_txtRands.TypeText(iPerformance.ToString());
            if (iAllowances >= 0) base.txtConfAllowances_txtRands.TypeText(iAllowances.ToString());
            if (iPAYE >= 0) base.txtConfPAYE_txtRands.TypeText(iPAYE.ToString());
            if (iUIF >= 0) base.txtConfUIF_txtRands.TypeText(iUIF.ToString());
            if (iPensionProvidentRA >= 0) base.txtConfPension_txtRands.TypeText(iPensionProvidentRA.ToString());
            if (iMedicalAid >= 0) base.txtConfMedicalAid_txtRands.TypeText(iMedicalAid.ToString());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="TestBrowser"></param>
        /// <param name="sContactPerson"></param>
        /// <param name="sPhoneCode"></param>
        /// <param name="sPhoneNumber"></param>
        /// <param name="sDepartment"></param>
        /// <param name="iConfirmationSourceIndex"></param>
        public void UpdateExtendedEmploymentDetails_ConfirmedEmployment(string sContactPerson, string sPhoneCode, string sPhoneNumber,
            string sDepartment, int iConfirmationSourceIndex, int? salaryPaymentDay = null, int? unionMembershipIndex = null)
        {
            if (salaryPaymentDay != null)
                base.SalaryPayDay.TypeText(salaryPaymentDay.Value.ToString());
            if (!string.IsNullOrEmpty(sContactPerson)) base.txtContactPerson.TypeText(sContactPerson);
            if (!string.IsNullOrEmpty(sPhoneCode)) base.spPhoneNumber__CODE.TypeText(sPhoneCode);
            if (!string.IsNullOrEmpty(sPhoneNumber)) base.spPhoneNumber__NUMB.TypeText(sPhoneNumber);
            if (!string.IsNullOrEmpty(sDepartment)) base.txtDepartment.TypeText(sDepartment);
            if (iConfirmationSourceIndex >= 0 && iConfirmationSourceIndex <= base.ddlConfirmationSource.Options.Count) base.ddlConfirmationSource.Options[iConfirmationSourceIndex].Select();
            if (unionMembershipIndex != null && unionMembershipIndex >= 0 && unionMembershipIndex <= base.ddlUnionMembership.Options.Count) base.ddlUnionMembership.Options[unionMembershipIndex.Value].Select();
            else if (iConfirmationSourceIndex >= 0) base.ddlConfirmationSource.Options[1].Select();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="TestBrowser"></param>
        /// <param name="salarySlip"></param>
        /// <param name="accountantsLetter"></param>
        /// <param name="bankStatement"></param>
        /// <param name="reconOfBankStatement"></param>
        public void UpdateExtendedEmploymentDetials_VerificationProcess(bool salarySlip, bool accountantsLetter, bool bankStatement,
            bool reconOfBankStatement)
        {
            base.checkboxSalarySlip.Checked = salarySlip;
            base.checkboxAccountantsLetter.Checked = accountantsLetter;
            base.checkboxBankStatement.Checked = bankStatement;
            base.checkboxReconOfBankStatement.Checked = reconOfBankStatement;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="TestBrowser"></param>
        /// <param name="buttonPress"></param>
        public void UpdateExtendedEmploymentDetials_Buttons(ButtonTypeEnum buttonPress)
        {
            switch (buttonPress)
            {
                case ButtonTypeEnum.Save:
                    base.btnSave.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.btnCancel.Click();
                    break;

                case ButtonTypeEnum.Back:
                    base.btnBack.Click();
                    break;
            }
        }

        public void HandleValidationWarningMessageSave()
        {
            base.btnValidationWarningSave.Click();
        }

        public void HandleValidationWarningMessageCancel()
        {
            base.btnValidationWarningCancel.Click();
        }

        public void AssertDetails(int VariableMontlyIncome, int GrossMonthlyIncome, int Deductions, int netIncome)
        {
            Assert.AreEqual(Convert.ToInt32(base.txtMonthVariableTotal_txtRands.Value.Replace(",", "")), VariableMontlyIncome);
            Assert.AreEqual(Convert.ToInt32(base.txtMonthGrossIncome_txtRands.Value.Replace(",", "")), GrossMonthlyIncome);
            Assert.AreEqual(Convert.ToInt32(base.txtMonthDeductions_txtRands.Value.Replace(",", "")), Deductions);
            Assert.AreEqual(Convert.ToInt32(base.txtMonthNetIncome_txtRands.Value.Replace(",", "")), netIncome);
        }

        public void ClickBack()
        {
            base.btnBack.Click();
        }

        public void ClickCancel()
        {
            base.btnCancel.Click();
        }

        public void AssertThatMonthlyIncomeFieldsOnExtendedUpdateScreenAreEnabled()
        {
            Assert.True(base.txtMonthBasicIncome_txtRands.Enabled
                && base.txtMonthVariableTotal_txtRands.Enabled
                && base.txtMonthCommission_txtRands.Enabled
                && base.txtMonthOvertime_txtRands.Enabled
                && base.txtMonthShift_txtRands.Enabled
                && base.txtMonthPerformance_txtRands.Enabled
                && base.txtMonthAllowances_txtRands.Enabled
                && base.txtMonthGrossIncome_txtRands.Enabled
                && base.txtMonthDeductions_txtRands.Enabled
                && base.txtMonthPAYE_txtRands.Enabled
                && base.txtMonthUIF_txtRands.Enabled
                && base.txtMonthPension_txtRands.Enabled
                && base.txtMonthMedicalAid_txtRands.Enabled
                && base.txtMonthNetIncome_txtRands.Enabled, "One or more of the 'Monthly Income' input fields are disabled");
            WatiN.Core.Logging.Logger.LogAction("Asserting that the 'Monthly Income' input fields on the LegalEntityEmploymentExtendedUpdate screen are enabled");
        }

        public void AssertThatMonthlyIncomeFieldsOnExtendedUpdateScreenAreDisabled()
        {
            Assert.False(base.txtMonthBasicIncome_txtRands.Enabled
                && base.txtMonthVariableTotal_txtRands.Enabled
                && base.txtMonthCommission_txtRands.Enabled
                && base.txtMonthOvertime_txtRands.Enabled
                && base.txtMonthShift_txtRands.Enabled
                && base.txtMonthPerformance_txtRands.Enabled
                && base.txtMonthAllowances_txtRands.Enabled
                && base.txtMonthGrossIncome_txtRands.Enabled
                && base.txtMonthDeductions_txtRands.Enabled
                && base.txtMonthPAYE_txtRands.Enabled
                && base.txtMonthUIF_txtRands.Enabled
                && base.txtMonthPension_txtRands.Enabled
                && base.txtMonthMedicalAid_txtRands.Enabled
                && base.txtMonthNetIncome_txtRands.Enabled, "One or more of the 'Monthly Income' input fields are enabled");
            WatiN.Core.Logging.Logger.LogAction("Asserting that the 'Monthly Income' input fields on the LegalEntityEmploymentExtendedUpdate screen are disabled");
        }

        public void AssertThatConfirmedIncomeFieldsOnExtendedUpdateScreenAreEnabled()
        {
            Assert.True(base.txtConfBasicIncome_txtRands.Enabled
                && base.txtConfVariableTotal_txtRands.Enabled
                && base.txtConfCommission_txtRands.Enabled
                && base.txtConfOvertime_txtRands.Enabled
                && base.txtConfShift_txtRands.Enabled
                && base.txtConfPerformance_txtRands.Enabled
                && base.txtConfAllowances_txtRands.Enabled
                && base.txtConfGrossIncome_txtRands.Enabled
                && base.txtConfDeductions_txtRands.Enabled
                && base.txtConfPAYE_txtRands.Enabled
                && base.txtConfUIF_txtRands.Enabled
                && base.txtConfPension_txtRands.Enabled
                && base.txtConfMedicalAid_txtRands.Enabled
                && base.txtConfNetIncome_txtRands.Enabled, "One or more of the 'Confirmed Income' unput fields are not disabled");
            WatiN.Core.Logging.Logger.LogAction("Asserting that the 'Confirmed Income' input fields on the LegalEntityEmploymentExtendedUpdate screen are enabled");
        }

        public void AssertThatConfirmedIncomeFieldsOnExtendedUpdateScreenAreDisabled(TestBrowser TestBrowser)
        {
            Assert.False(base.txtConfBasicIncome_txtRands.Enabled
                && base.txtConfVariableTotal_txtRands.Enabled
                && base.txtConfCommission_txtRands.Enabled
                && base.txtConfOvertime_txtRands.Enabled
                && base.txtConfShift_txtRands.Enabled
                && base.txtConfPerformance_txtRands.Enabled
                && base.txtConfAllowances_txtRands.Enabled
                && base.txtConfGrossIncome_txtRands.Enabled
                && base.txtConfDeductions_txtRands.Enabled
                && base.txtConfPAYE_txtRands.Enabled
                && base.txtConfUIF_txtRands.Enabled
                && base.txtConfPension_txtRands.Enabled
                && base.txtConfMedicalAid_txtRands.Enabled
                && base.txtConfNetIncome_txtRands.Enabled, "One of the 'Confirmed Income' input fields are not disabled");
            WatiN.Core.Logging.Logger.LogAction("Asserting that the 'Confirmed Income' input fields on the LegalEntityEmploymentExtendedUpdate screen are disabled");
        }

        public void AssertThatConfirmedCaptureFieldsOnExtendedUpdateScreenAreEnabled()
        {
            Assert.True(base.txtContactPerson.Enabled
                && base.spPhoneNumber__CODE.Enabled
                && base.spPhoneNumber__NUMB.Enabled
                && base.txtDepartment.Enabled
                && base.ddlConfirmationSource.Enabled
                && base.ddlUnionMembership.Enabled, "One of the Confirmed Capture input fields are not enabled");
            WatiN.Core.Logging.Logger.LogAction("Asserting that the Confirmed Capture input fields on the LegalEntityEmploymentExtendedUpdate screen are enabled");
        }

        public void AssertThatConfirmedCaptureFieldsOnExtendedUpdateScreenAreDisabled()
        {
            Assert.False(base.txtContactPerson.Enabled
                && base.spPhoneNumber__CODE.Enabled
                && base.spPhoneNumber__NUMB.Enabled
                && base.txtDepartment.Enabled
                && base.ddlConfirmationSource.Enabled
                && base.ddlUnionMembership.Enabled, "One of the Confirmed Capture input fields are not enabled");
            WatiN.Core.Logging.Logger.LogAction("Asserting that the Confirmed Capture input fields on the LegalEntityEmploymentExtendedUpdate screen are disabled");
        }
    }
}