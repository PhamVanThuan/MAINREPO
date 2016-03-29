using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ManualValuationsMainDwellingExtendedDetailsAddControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtCombinedOutbuildingsExtent")]
        protected TextField OutbuildingsExtent { get; set; }

        [FindBy(Id = "ctl00_Main_txtCombinedOutbuildingsRate")]
        protected TextField OutbuildingsRate { get; set; }

        [FindBy(Id = "ctl00_Main_txtCombinedOutbuildingsReplaceValue")]
        protected TextField OutbuildingsReplaceValue { get; set; }

        [FindBy(Id = "ctl00_Main_txtImprovementReplacementValue")]
        protected TextField ImprovementReplaceValue { get; set; }

        [FindBy(Id = "ctl00_Main_BackButton")]
        protected Button BackButton { get; set; }

        [FindBy(Id = "ctl00_Main_AddButton")]
        protected Button AddButton { get; set; }

        [FindBy(Id = "ctl00_Main_RemoveButton")]
        protected Button RemoveButton { get; set; }

        [FindBy(Id = "ctl00_Main_ddlBuildingType")]
        protected SelectList MainBuildingTypes { get; set; }

        [FindBy(Id = "ctl00_Main_ddlCombinedOutbuildingsRoofType")]
        protected SelectList OutbuildingsRoofTypes { get; set; }

        [FindBy(Id = "ctl00_Main_ddlImprovementType")]
        protected SelectList ImprovementType { get; set; }

        [FindBy(Id = "ctl00_Main_dteImprovementDate")]
        protected TextField ImprovementDate { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button Next { get; set; }
    }
}