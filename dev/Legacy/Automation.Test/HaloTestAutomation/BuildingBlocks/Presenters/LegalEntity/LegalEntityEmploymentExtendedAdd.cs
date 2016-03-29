using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LegalEntity
{
    public class LegalEntityEmploymentExtendedAdd : LegalEntityEmploymentExtendedAddControls
    {
        internal void AddBasicIncomeAndSave(string monthlyIncomeRands)
        {
            base.txtMonthBasicIncome_txtRands.TypeText(monthlyIncomeRands);
            base.btnSave.Click();
        }
    }
}