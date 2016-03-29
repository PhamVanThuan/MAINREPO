using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class FixedDebitOrderUpdateControls : FixedDebitOrderBaseControls
    {
        [FindBy(Id = "ctl00_Main_FixedDebitOrderAmountUpdate")]
        protected TextField FixedDebitOrderAmount { get; set; }

        [FindBy(Id = "ctl00_Main_EffectiveDateUpdate")]
        protected TextField EffectiveDate { get; set; }
    }
}