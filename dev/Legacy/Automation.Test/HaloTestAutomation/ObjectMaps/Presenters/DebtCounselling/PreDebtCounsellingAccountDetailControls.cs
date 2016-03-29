using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class PreDebtCounsellingAccountDetailControls : ObjectMaps.Pages.BasePageControls
    {
        //Common
        [FindBy(Id = "ctl00_Main_lblProduct")]
        protected Span Product { get; set; }

        [FindBy(Id = "ctl00_Main_lblTerm")]
        protected Span Term { get; set; }

        [FindBy(Id = "ctl00_Main_lblHOCPremium")]
        protected Span HOCPremium { get; set; }

        [FindBy(Id = "ctl00_Main_lblLifePremium")]
        protected Span LifePremium { get; set; }

        //Variable Leg
        [FindBy(Id = "ctl00_Main_lblVariableMarketRate")]
        protected Span VariableMarketRate { get; set; }

        [FindBy(Id = "ctl00_Main_lblVariableLinkRate")]
        protected Span VariableLinkRate { get; set; }

        [FindBy(Id = "ctl00_Main_lblVariableInstallment")]
        protected Span VariableInstallment { get; set; }

        [FindBy(Id = "ctl00_Main_lblVariableInterestRate")]
        protected Span VariableInterestRate { get; set; }

        //Fixed Leg
        [FindBy(Id = "ctl00_Main_lblFixedInstallment")]
        protected Span FixedInstallment { get; set; }

        [FindBy(Id = "ctl00_Main_lblFixedInterestRate")]
        protected Span FixedInterestRate { get; set; }

        [FindBy(Id = "ctl00_Main_lblFixedMarketRate")]
        protected Span FixedMarketRate { get; set; }

        [FindBy(Id = "ctl00_Main_lblFixedLinkRate")]
        protected Span FixedLinkRate { get; set; }

        [FindBy(Id = "ctl00_Main_lblTotalInstallment")]
        protected Span TotalInstallment { get; set; }

        [FindBy(Id = "ctl00_Main_gridRateOverrides_DXMainTable")]
        protected Table RateOverrides { get; set; }
    }
}