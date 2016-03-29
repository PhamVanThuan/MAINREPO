using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class DebitOrderDetailsAppUpdateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button ctl00MainbtnUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button ctl00MainCancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_DOPaymentTypeUpdate")]
        protected SelectList ctl00MainDOPaymentTypeUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_BankAccountUpdate")]
        protected SelectList ctl00MainBankAccountUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_DebitOrderDayUpdate")]
        protected SelectList ctl00MainDebitOrderDayUpdate { get; set; }
    }
}