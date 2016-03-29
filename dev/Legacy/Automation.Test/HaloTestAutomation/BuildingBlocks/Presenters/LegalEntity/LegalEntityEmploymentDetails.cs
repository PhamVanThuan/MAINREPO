using BuildingBlocks.Services.Contracts;
using BuildingBlocks.Timers;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Threading;
using WatiN.Core.UtilityClasses;
using TimeoutException = WatiN.Core.Exceptions.TimeoutException;

namespace BuildingBlocks.Presenters.LegalEntity
{
    /// <summary>
    ///
    /// </summary>
    public class LegalEntityEmploymentDetails : LegalEntityEmploymentAddControls
    {
        private static IWatiNService watinService;

        public LegalEntityEmploymentDetails()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="employerName"></param>
        /// <param name="employmentType"></param>
        /// <param name="remunerationType"></param>
        /// <param name="startDate"></param>
        /// <param name="monthlyIncomeRands"></param>
        /// <param name="clickSaveButton"></param>
        public void AddEmploymentDetails(string employerName, string employmentType, string remunerationType, string startDate, string monthlyIncomeRands, bool clickSaveButton)
        {
            SimpleTimer timer;
            //employerName lookup is case sensitive
            if (!string.IsNullOrEmpty(employerName))
            {
                base.txtEmployerName.TypeText(employerName);
                base.SAHLAutoCompleteDiv_iframe.WaitUntilExists();
                base.SAHLAutoComplete_DefaultItem(employerName).MouseDown();
            }
            if (!string.IsNullOrEmpty(employmentType))
            {
                base.ddlEmploymentType.Option(employmentType).Select();
                if (!string.IsNullOrEmpty(remunerationType))
                {
                    //we need to wait for a bit here
                    timer = new SimpleTimer(TimeSpan.FromSeconds(30));
                    while (true)
                    {
                        if (timer.Elapsed)
                        {
                            throw new TimeoutException(
                                "Timed out after 30 seconds while attempting to select '" + remunerationType + "' option from the 'Remuneration Type' select list"
                                );
                        }
                        Thread.Sleep(3000);
                        if (base.ddlRemunerationType.SelectedOption.Text != remunerationType)
                        {
                            base.ddlRemunerationType.Option(remunerationType).Select();
                            break;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                // Had problems entering StartDate immediately after setting remunerationType.  Added timed looping structure to deal with this
                timer = new SimpleTimer(TimeSpan.FromSeconds(30));
                while (true)
                {
                    if (timer.Elapsed)
                    {
                        throw new TimeoutException(
                            "Timed out after 30 seconds while attempting to type text '" + startDate + "' in the 'Start Date' text input field"
                            );
                    }
                    Thread.Sleep(3000);
                    if (base.dtStartDate.Value != startDate && !timer.Elapsed)
                    {
                        //startDate needs to be passed with exact format "00/00/0000"
                        base.dtStartDate.Value = startDate;
                        break;
                    }
                }
            }

            if (clickSaveButton)
            {
                switch (remunerationType)
                {
                    case "Basic + Commission":
                    case "Salaried":
                    case "": // this is here to cater for when we want this to be empty for validation testing
                        base.btnSave.Click();
                        if (base.Document.Page<BasePageAssertions>().ValidationSummaryExists() == false)
                        {
                            base.Document.Page
                                <LegalEntityEmploymentExtendedAdd>().AddBasicIncomeAndSave(monthlyIncomeRands);
                        }
                        break;

                    default:
                        base.txtRands.TypeText(monthlyIncomeRands);
                        base.btnSave.Click();
                        break;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="iConfirmedIncome"></param>
        public void BasicConfirmSalariedEmployment(TestBrowser browser, int iConfirmedIncome, int? salaryPaymentDay = null)
        {
            browser.Page<LegalEntityEmploymentUpdate>().UpdateEmploymentDetails("Current", null, null, null, null, null, null, "Yes", "Yes", ButtonTypeEnum.Save);
            browser.Page<LegalEntityEmploymentExtendedUpdate>().UpdateExtendedEmploymentDetails_ConfirmedIncome(iConfirmedIncome, -1, -1, -1, -1, -1, -1, -1, -1, -1);
            browser.Page<LegalEntityEmploymentExtendedUpdate>().UpdateExtendedEmploymentDetails_ConfirmedEmployment("Test", "031", "1234567", "Test", 1, salaryPaymentDay, unionMembershipIndex: 1);
            browser.Page<LegalEntityEmploymentExtendedUpdate>().UpdateExtendedEmploymentDetials_VerificationProcess(true, false, false, false);
            browser.Page<LegalEntityEmploymentExtendedUpdate>().UpdateExtendedEmploymentDetials_Buttons(ButtonTypeEnum.Save);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="iConfirmedIncome"></param>
        public void BasicConfirmSelfEmployedEmployment(TestBrowser browser, int iConfirmedIncome, int salaryPaymentDay)
        {
            browser.Page<LegalEntityEmploymentUpdate>().UpdateEmploymentDetails("Current", null, null, null, null, null, iConfirmedIncome.ToString(), "Yes", "Yes", ButtonTypeEnum.Save);
            browser.Page<LegalEntityEmploymentExtendedUpdate>().UpdateExtendedEmploymentDetails_ConfirmedEmployment("Test", "031", "1234567", "Test", 1, salaryPaymentDay, unionMembershipIndex: 1);
            browser.Page<LegalEntityEmploymentExtendedUpdate>().UpdateExtendedEmploymentDetials_VerificationProcess(true, false, false, false);
            browser.Page<LegalEntityEmploymentExtendedUpdate>().UpdateExtendedEmploymentDetials_Buttons(ButtonTypeEnum.Save);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="buttonPress"></param>
        public static void HandleValidationWarningMessage(TestBrowser browser, ButtonTypeEnum buttonPress)
        {
            switch (buttonPress)
            {
                case ButtonTypeEnum.Save:
                    browser.Page<LegalEntityEmploymentExtendedUpdate>().HandleValidationWarningMessageSave();
                    break;

                case ButtonTypeEnum.Cancel:
                    browser.Page<LegalEntityEmploymentExtendedUpdate>().HandleValidationWarningMessageCancel();
                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="employerName"></param>
        /// <param name="employmentType"></param>
        /// <param name="employmentStatus"></param>
        /// <param name="remunerationType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="strBasicIncome"></param>
        /// <param name="iCommission"></param>
        /// <param name="iOvertime"></param>
        /// <param name="iShift"></param>
        /// <param name="iPerformance"></param>
        /// <param name="iAllowances"></param>
        /// <param name="iPAYE"></param>
        /// <param name="iUIF"></param>
        /// <param name="iPensionProvidentRA"></param>
        /// <param name="iMedicalAid"></param>
        /// <param name="bClickSave"></param>
        public void AddEmploymentExtendedDetails(string employerName, string employmentType, string employmentStatus, string remunerationType,
            string startDate, string endDate, string strBasicIncome, int iCommission, int iOvertime, int iShift, int iPerformance, int iAllowances, int iPAYE,
            int iUIF, int iPensionProvidentRA, int iMedicalAid, bool bClickSave)
        {
            SimpleTimer timer;
            //employerName lookup is case sensitive
            if (!string.IsNullOrEmpty(employerName))
            {
                base.txtEmployerName.TypeText(employerName);
                base.SAHLAutoCompleteDiv_iframe.WaitUntilExists();
                base.SAHLAutoComplete_DefaultItem(employerName).MouseDown();
            }
            if (!string.IsNullOrEmpty(employmentType))
            {
                base.ddlEmploymentType.Option(employmentType).Select();
                if (!string.IsNullOrEmpty(remunerationType))
                {
                    //we need to wait for a bit here
                    timer = new SimpleTimer(TimeSpan.FromSeconds(30));
                    while (true)
                    {
                        if (timer.Elapsed)
                        {
                            throw new TimeoutException(
                                "Timed out after 30 seconds while attempting to select '" + remunerationType + "' option from the 'Remuneration Type' select list"
                                );
                        }
                        Thread.Sleep(3000);
                        if (base.ddlRemunerationType.SelectedOption.Text != remunerationType)
                        {
                            base.ddlRemunerationType.Option(remunerationType).Select();
                            break;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                // Had problems entering StartDate immediately after setting remunerationType.  Added timed looping structure to deal with this
                timer = new SimpleTimer(TimeSpan.FromSeconds(30));
                while (true)
                {
                    if (timer.Elapsed)
                    {
                        throw new TimeoutException(
                            "Timed out after 30 seconds while attempting to type text '" + startDate + "' in the 'Start Date' text input field"
                            );
                    }
                    Thread.Sleep(3000);
                    if (base.dtStartDate.Value != startDate && !timer.Elapsed)
                    {
                        //startDate needs to be passed with exact format "00/00/0000"
                        base.dtStartDate.Value = startDate;
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                base.dtEndDate.Value = endDate;
            }
            base.ddlEmploymentStatus.Option(employmentStatus).Select();
            base.btnSave.Click();
            base.Document.Page<LegalEntityEmploymentExtendedUpdate>().UpdateExtendedEmploymentDetails_MonthlyIncome(Convert.ToInt32(strBasicIncome), iCommission, iOvertime,
                iShift, iPerformance, iAllowances, iPAYE, iUIF, iPensionProvidentRA, iMedicalAid);
            if (bClickSave)
                base.btnSave.Click();
        }

        /// <summary>
        /// This will just save the employment record
        /// </summary>
        internal void SaveEmployment()
        {
            base.btnSave.Click();
            base.btnSave.Click();
        }

        /// <summary>
        /// Adds an employment record
        /// </summary>
        /// <param name="employment"></param>
        public void AddEmploymentDetails(Automation.DataModels.Employment employment, bool clickNext)
        {
            base.Document.Page<LegalEntityEmploymentDetails>().AddEmploymentDetails(employment.Employer, employment.EmploymentType, employment.RemunerationType,
                employment.StartDate, employment.MonthlyIncomeRands.ToString(), clickNext);
        }

        /// <summary>
        /// Changes the remuneration type
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="employment"></param>
        public void SelectRemunerationType(Automation.DataModels.Employment employment)
        {
            base.ddlRemunerationType.Option(employment.RemunerationType).Select();
            Thread.Sleep(2000);
            base.btnSave.Click();
        }

        /// <summary>
        /// Checks the text of the Save Button
        /// </summary>
        /// <param name="expectedText"></param>
        public void AssertSaveButtonText(string expectedText)
        {
            Assert.IsTrue(base.btnSave.Text == expectedText);
        }

        /// <summary>
        /// Clicks the Save Button
        /// </summary>
        public void ClickSave()
        {
            base.btnSave.Click();
        }

        /// <summary>
        /// Clicks the Cancel button
        /// </summary>
        public void ClickCancel()
        {
            base.btnCancel.Click();
        }

        /// <summary>
        /// Asserts the controls on the employment add screen
        /// </summary>
        /// <param name="employerName"></param>
        /// <param name="selectedEmploymentType"></param>
        /// <param name="selectedEmploymentStatus"></param>
        /// <param name="selectedRemunerationType"></param>
        /// <param name="startDate"></param>
        public void AssertEmploymentAddDetails(string employerName, string selectedEmploymentType, string selectedEmploymentStatus,
            string selectedRemunerationType, string startDate)
        {
            string actualEmployer = base.txtEmployerName.Value == null ? string.Empty : base.txtEmployerName.Value;
            string actualStartDate = base.dtStartDate.Value == null ? string.Empty : base.dtStartDate.Value;
            Assert.AreEqual(employerName, actualEmployer);
            Assert.AreEqual(selectedEmploymentType, base.ddlEmploymentType.SelectedItem);
            Assert.AreEqual(selectedEmploymentStatus, base.ddlEmploymentStatus.SelectedItem);
            GeneralTimer.Wait(2000);
            Assert.AreEqual(selectedRemunerationType, base.ddlRemunerationType.SelectedItem);
            Assert.AreEqual(startDate, actualStartDate);
        }

        public void AssertEmployerDetails(string EmployerName, string EmploymentType, string EmploymentStatus, string RemunerationType, string StartDate)
        {
            base.Document.Page<BasePageAssertions>().AssertViewLoaded("LegalEntityEmploymentAdd");
            Assert.AreEqual(base.txtEmployerName.Value, EmployerName);
            Assert.AreEqual(base.ddlEmploymentType.SelectedItem, EmploymentType);
            Assert.AreEqual(base.ddlEmploymentStatus.SelectedItem, EmploymentStatus);
            GeneralTimer.Wait(2000);
            Assert.AreEqual(base.ddlRemunerationType.SelectedItem, RemunerationType);
            Assert.AreEqual(base.dtStartDate.Value, StartDate);
        }

        public void AssertEmployerControlsReset()
        {
            base.Document.Page<BasePageAssertions>().AssertViewLoaded("LegalEntityEmploymentAdd");
            Assert.AreEqual(base.txtEmployerName.Value, null);
            Assert.AreEqual(base.ddlEmploymentType.SelectedItem, "- Please select -");
            Assert.AreEqual(base.ddlEmploymentStatus.SelectedItem, "Current");
            GeneralTimer.Wait(2000);
            Assert.AreEqual(base.ddlRemunerationType.SelectedItem, "- Please select -");
            Assert.AreEqual(base.dtStartDate.Value, null);
        }
    }
}