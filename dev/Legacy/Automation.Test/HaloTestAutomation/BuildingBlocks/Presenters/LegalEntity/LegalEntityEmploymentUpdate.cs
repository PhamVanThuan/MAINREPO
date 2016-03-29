using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Threading;
using WatiN.Core;
using WatiN.Core.UtilityClasses;

namespace BuildingBlocks.Presenters.LegalEntity
{
    public class LegalEntityEmploymentUpdate : LegalEntityEmploymentUpdateControls
    {
        public string GetEmploymentType()
        {
            return base.EmploymentType.Text;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="TestBrowser"></param>
        /// <param name="identifier"></param>
        /// <param name="employerName"></param>
        /// <param name="status"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="monthlyIncome"></param>
        /// <param name="confirmedIncome"></param>
        /// <param name="confirmedEmploymentFlag"></param>
        /// <param name="confirmedIncomeFlag"></param>
        /// <param name="buttonPress"></param>
        public void UpdateEmploymentDetails(string identifier, string employerName, string status, string startDate, string endDate,
            string monthlyIncome, string confirmedIncome, string confirmedEmploymentFlag, string confirmedIncomeFlag, ButtonTypeEnum buttonPress)
        {
            if (identifier != null) base.grdEmploymentCell.Filter(Find.ByText(identifier)).First().Click();
            if (employerName != null)
            {
                base.ctl00MainpnlEmployerDetailstxtEmployerName.TypeText(employerName);
                base.SAHLAutoCompleteDiv_iframe.WaitUntilExists(30);
                base.SAHLAutoComplete_DefaultItem(employerName).MouseDown();
            }
            if (status != null)
            {
                base.ctl00MainpnlEmploymentDetailsddlEmploymentStatus.Option(status).Select();
                base.Document.DomContainer.WaitForComplete();
            }
            if (startDate != null) base.ctl00MainpnlEmploymentDetailsdtStartDate.Value = startDate;
            if (endDate != null) base.ctl00MainpnlEmploymentDetailsdtEndDate.Value = endDate;
            if (confirmedEmploymentFlag != null)
            {
                WatiN.Core.Logging.Logger.LogAction("Selecting Option '{0}' from SelectList 'Confirmed Employment'", confirmedEmploymentFlag);
                base.ctl00MainpnlEmploymentDetailsddlConfirmedEmployment.Option(confirmedEmploymentFlag).Select();
                base.Document.DomContainer.WaitForComplete();
            }
            if (confirmedIncomeFlag != null)
            {
                Thread.Sleep(2000);
                WatiN.Core.Logging.Logger.LogAction("Selecting Option '{0}' from SelectList 'Confirmed Income'", confirmedIncomeFlag);
                // Had problems selecting from the ConfirmedIncomeFlag dropdown immediately after setting ConfirmedEmploymentFlag.
                // Added timed looping structure to deal with this
                SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(30));
                while (base.ctl00MainpnlEmploymentDetailsddlConfirmedIncome.SelectedOption.Text != confirmedIncomeFlag)
                {
                    if (timer.Elapsed)
                    {
                        throw new TimeoutException(
                            "while attempting to select Option:'" + confirmedIncomeFlag + "' from SelectList: 'Confirmed Income'"
                            );
                    }
                    base.ctl00MainpnlEmploymentDetailsddlConfirmedIncome.Option(confirmedIncomeFlag).Select();
                    Thread.Sleep(1000);
                }
            }
            if (monthlyIncome != null) base.currMonthlyIncome.TypeText(monthlyIncome);
            if (confirmedIncome != null) base.currConfirmedIncome.TypeText(confirmedIncome);

            switch (buttonPress)
            {
                case ButtonTypeEnum.Cancel:
                    base.ctl00MainbtnCancel.Click();
                    break;

                case ButtonTypeEnum.Save:
                    if (base.RemunerationType.Text == "Salaried") base.ctl00MainbtnSave.WaitUntil("value", "Next");
                    base.ctl00MainbtnSave.Click();
                    break;
            }
        }

        public void AssertMonthlyIncomeTextboxExists(string MonthlyIncome)
        {
            WatiN.Core.Logging.Logger.LogAction("Asserting that 'Confirmed Income' textbox exist");
            base.Document.DomContainer.WaitForComplete();
            Assert.True(base.currMonthlyIncome.Exists, "The 'Monthly Income' textbox does not exists");
            Assert.DoesNotThrow(delegate { base.currMonthlyIncome.TypeText(MonthlyIncome); },
                string.Format(@"Exception thrown when attempting to input value: {0} in the 'Monthly Income' textbox", MonthlyIncome));
        }

        public void AssertMonthlyIncomeTextboxDoesNotExist()
        {
            Assert.False(base.currMonthlyIncome.Exists, "The 'Monthly Income' textbox exists");
            WatiN.Core.Logging.Logger.LogAction("Asserting that 'Monthly Income' textbox does not exist");
        }

        public void AssertConfirmedIncomeTextboxExists(string ConfirmedIncome)
        {
            WatiN.Core.Logging.Logger.LogAction("Asserting that 'Confirmed Income' textbox exist");
            base.Document.DomContainer.WaitForComplete();
            Assert.True(base.currConfirmedIncome.Exists, "The 'Confirmed Income' textbox does not exists");
            Assert.DoesNotThrow(delegate { base.currConfirmedIncome.TypeText(ConfirmedIncome); },
                string.Format(@"Exception thrown when attempting to input value: {0} in the 'Confirmed Income' textbox", ConfirmedIncome));
        }

        public void AssertConfirmedIncomeTextBoxDoesNotExist()
        {
            base.Document.DomContainer.WaitForComplete();
            Assert.False(base.currConfirmedIncome.Exists, "The 'Confirmed Income' field exists");
            WatiN.Core.Logging.Logger.LogAction("Asserting that 'Confirmed Income' textbox does not exist");
        }
    }
}