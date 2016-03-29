using ObjectMaps.Pages;
using WatiN.Core;

namespace BuildingBlocks.Presenters.Origination
{
    public class ApplicationDeclarations : ApplicationDeclarationsControls
    {
        /// <summary>
        /// Provides the default answers to the declarations in order for the workflow case to proceed
        /// </summary>
        public void ApplicationDeclarationsUpdate()
        {
            int selectListCount = 0;
            foreach (SelectList selectL in base.GetAllSelectListsOnScreen)
            {
                //This will set all the values of the select lists to "No" except for the last one.
                selectL.Option("No").Select();
                selectListCount++;
                if (selectListCount == base.GetAllSelectListsOnScreen.Count)
                {
                    selectL.Option("Yes").Select();
                }
            }
            base.UpdateButton.Click();
        }

        /// <summary>
        /// Updates the application declarations update
        /// </summary>
        /// <param name="app">Application Declaration Answer</param>
        public void ApplicationDeclarationUpdate(Automation.DataModels.ApplicationDeclaration app)
        {
            base.InsolvencySelect.Option(app.InsolvencyAnswer).Select();
            base.RehabDate.Value = string.IsNullOrEmpty(app.DateRehabilitatedAnswer) ? "" : app.DateRehabilitatedAnswer;
            base.AdminSelect.Option(app.AdministrationOrderAnswer).Select();
            base.RescindedDate.Value = string.IsNullOrEmpty(app.DateRescindedAnswer) ? "" : app.DateRescindedAnswer;
            base.DebtCounselingSelect.Option(app.CurrentUnderDebtCounsellingAnswer).Select();
            base.DebtRearrangementSelect.Option(app.CurrentDebtRearrangementAnswer).Select();
            base.CreditCheckSelect.Option(app.ConductCreditCheckAnswer).Select();
            base.UpdateButton.Click();
        }
    }
}