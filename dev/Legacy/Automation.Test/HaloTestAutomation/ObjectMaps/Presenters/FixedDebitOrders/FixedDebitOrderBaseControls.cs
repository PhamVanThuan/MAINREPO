using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class FixedDebitOrderBaseControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_AccountsGrid")]
        protected Table AccountsGrid { get; set; }

        [FindBy(Id = "ctl00_Main_FutureOrderGrid")]
        protected Table FutureFixedDebitOrdersGrid { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button btnSubmit { get; set; }

        protected TableRow FutureDatedDebitOrderGridSelectRow(string futureDatedChangeKey)
        {
            var fdc = FutureFixedDebitOrdersGrid.TableCells;
            return fdc.Filter(Find.ByText(futureDatedChangeKey))[0].ContainingTableRow;
        }
    }
}