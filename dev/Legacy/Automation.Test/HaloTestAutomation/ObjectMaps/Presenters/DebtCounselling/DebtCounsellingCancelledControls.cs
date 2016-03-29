using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class DebtCounsellingCancelledControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlReason")]
        protected SelectList CancellationReason { get; set; }

        [FindBy(Id = "ctl00_Main_gvLinkedAccounts")]
        protected Table LinkedAccounts { get; set; }

        protected bool GroupedAccountsExist(string accountKey)
        {
            return this.LinkedAccounts.TableCells.Exists(Find.ByText(accountKey));
        }

        [FindBy(Id = "ctl00_Main_btnConfirm")]
        protected Button Submit { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button Cancel { get; set; }
    }
}