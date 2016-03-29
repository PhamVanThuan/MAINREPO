using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ValuationDetailsAddControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtValuationAmount_txtRands")]
        protected TextField ValuationRandsAmount { get; set; }

        [FindBy(Id = "ctl00_Main_txtValuationAmount_txtCents")]
        protected TextField ValuationCentsAmount { get; set; }

        [FindBy(Id = "ctl00_Main_dateValuationDate")]
        protected TextField ValuationDate { get; set; }

        [FindBy(Id = "ctl00_Main_txtMunicipalValuation")]
        protected TextField MunicipalValuation { get; set; }

        [FindBy(Id = "ctl00_Main_txtHOCConventionalAmount_txtRands")]
        protected TextField HOCConventionalAmount { get; set; }

        [FindBy(Id = "ctl00_Main_txtHOCConventionalAmount_txtCents")]
        protected TextField HOCConventionalAmountCents { get; set; }

        [FindBy(Id = "ctl00_Main_txtHOCThatchAmount_txtRands")]
        protected TextField HOCThatchAmount { get; set; }

        [FindBy(Id = "ctl00_Main_txtHOCThatchAmount_txtCents")]
        protected TextField HOCThatchAmountCents { get; set; }

        [FindBy(Id = "ctl00_Main_BackButton")]
        protected Button MainBackButton { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button Add { get; set; }

        [FindBy(Id = "ctl00_Main_ddlValuer")]
        protected SelectList Valuers { get; set; }

        [FindBy(Id = "ctl00_Main_ddlHOCRoofDescription")]
        protected SelectList HOCRoofDescription { get; set; }
    }
}