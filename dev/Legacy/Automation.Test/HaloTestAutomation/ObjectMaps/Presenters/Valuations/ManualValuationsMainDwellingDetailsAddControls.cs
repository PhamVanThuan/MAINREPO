using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ManualValuationsMainDwellingDetailsAddControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtMainBuildingExtent")]
        protected TextField BuildingExtent { get; set; }

        [FindBy(Id = "ctl00_Main_txtMainBuildingRate")]
        protected TextField MainBuildingRate { get; set; }

        [FindBy(Id = "ctl00_Main_txtMainBuildingReplaceValue")]
        protected TextField MainBuildingReplaceValue { get; set; }

        [FindBy(Id = "ctl00_Main_txtCottageExtent")]
        protected TextField CottageExtent { get; set; }

        [FindBy(Id = "ctl00_Main_txtCottageRate")]
        protected TextField CottageRate { get; set; }

        [FindBy(Id = "ctl00_Main_txtCottageReplaceValue")]
        protected TextField CottageReplaceValue { get; set; }

        [FindBy(Id = "ctl00_Main_txtEscalation")]
        protected TextField Escalation { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button CancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button SubmitButton { get; set; }

        [FindBy(Id = "ctl00_Main_ddlMainBuildingClassification")]
        protected SelectList MainBuildingClassification { get; set; }

        [FindBy(Id = "ctl00_Main_ddlMainBuildingRoofType")]
        protected SelectList MainBuildingRoofType { get; set; }

        [FindBy(Id = "ctl00_Main_ddlCottageRoofType")]
        protected SelectList CottageRoofType { get; set; }
    }
}