using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class PropertyDetailsControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtPropertyDescription1")]
        protected TextField ctl00MaintxtPropertyDescription1 { get; set; }

        [FindBy(Id = "ctl00_Main_txtPropertyDescription2")]
        protected TextField ctl00MaintxtPropertyDescription2 { get; set; }

        [FindBy(Id = "ctl00_Main_txtPropertyDescription3")]
        protected TextField ctl00MaintxtPropertyDescription3 { get; set; }

        [FindBy(Id = "ctl00_Main_ErfNumber")]
        protected Span ctl00MainErfNumber { get; set; }

        [FindBy(Id = "ctl00_Main_PortionNumber")]
        protected Span ctl00MainPortionNumber { get; set; }

        [FindBy(Id = "ctl00_Main_ErfSuburb")]
        protected Span ctl00MainErfSuburb { get; set; }

        [FindBy(Id = "ctl00_Main_ErfMetroDescription")]
        protected Span ctl00MainErfMetroDescription { get; set; }

        [FindBy(Id = "ctl00_Main_TitleDeedNumber")]
        protected Span ctl00MainTitleDeedNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtTitleDeedNumber")]
        protected TextField ctl00txtMainTitleDeedNumber { get; set; }

        [FindBy(Id = "ctl00_Main_InspectionContact")]
        protected Span ctl00MainInspectionContact { get; set; }

        [FindBy(Id = "ctl00_Main_InspectionNumber")]
        protected Span ctl00MainInspectionNumber { get; set; }

        [FindBy(Id = "ctl00_Main_InspectionContact2")]
        protected Span ctl00MainInspectionContact2 { get; set; }

        [FindBy(Id = "ctl00_Main_InspectionNumber2")]
        protected Span ctl00MainInspectionNumber2 { get; set; }

        [FindBy(Id = "ctl00_Main_lbCurrentDataProvider")]
        protected Span ctl00MainlbCurrentDataProvider { get; set; }

        [FindBy(Id = "ctl00_Main_BondAccountNumber")]
        protected Span ctl00MainBondAccountNumber { get; set; }

        [FindBy(Id = "ctl00_Main_LightStonePropertyID")]
        protected Span ctl00MainLightStonePropertyID { get; set; }

        [FindBy(Id = "ctl00_Main_DeedsOffice")]
        protected Span ctl00MainDeedsOffice { get; set; }

        [FindBy(Id = "ctl00_Main_AdCheckPropertyID")]
        protected Span ctl00MainAdCheckPropertyID { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button ctl00MainbtnUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button ctl00MainbtnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_ddlPropertyType")]
        protected SelectList ctl00MainddlPropertyType { get; set; }

        [FindBy(Id = "ctl00_Main_ddlTitleType")]
        protected SelectList ctl00MainddlTitleType { get; set; }

        [FindBy(Id = "ctl00_Main_ddlOccupancyType")]
        protected SelectList ctl00MainddlOccupancyType { get; set; }

        [FindBy(Id = "ctl00_Main_ddlAreaClassification")]
        protected SelectList ctl00MainddlAreaClassification { get; set; }

        [FindBy(Id = "ctl00_Main_ddlDeedsPropertyType")]
        protected SelectList ctl00MainddlDeedsPropertyType { get; set; }

        [FindBy(Id = "ctl00_Main_txtErfNumber")]
        protected TextField ctl00MaintxtErfNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtPortionNumber")]
        protected TextField ctl00MaintxtPortionNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtErfSuburb")]
        protected TextField ctl00MaintxtErfSuburb { get; set; }

        [FindBy(Id = "ctl00_Main_txtErfMetroDescription")]
        protected TextField ctl00MaintxtErfMetroDescription { get; set; }

        [FindBy(Id = "ctl00_Main_txtSectionalSchemeName")]
        protected TextField ctl00MaintxtSectionalSchemeName { get; set; }

        [FindBy(Id = "ctl00_Main_txtSectionalUnitNumber")]
        protected TextField ctl00MaintxtSectionalUnitNumber { get; set; }

        [FindBy(Id = "ctl00_Main_SectionalSchemeName")]
        protected Span ctl00_Main_SectionalSchemeName { get; set; }

        [FindBy(Id = "ctl00_Main_SectionalUnitNumber")]
        protected Span ctl00_Main_SectionalUnitNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtBondAccountNumber")]
        protected TextField ctl00MaintxtBondAccountNumber { get; set; }

        [FindBy(Id = "ctl00_Main_ddlDeedsOffice")]
        protected SelectList ctl00MainddlDeedsOffice { get; set; }

        [FindBy(Id = "ctl00_Main_InspectionContact")]
        protected Span ctl00_Main_InspectionContact { get; set; }

        [FindBy(Id = "ctl00_Main_InspectionNumber")]
        protected Span ctl00_Main_InspectionNumber { get; set; }

        [FindBy(Id = "ctl00_Main_InspectionContact2")]
        protected Span ctl00_Main_InspectionContact2 { get; set; }

        [FindBy(Id = "ctl00_Main_InspectionNumber2")]
        protected Span ctl00_Main_InspectionNumber2 { get; set; }

        [FindBy(Id = "ctl00_Main_lbCurrentDataProvider")]
        protected Span ctl00_Main_lbCurrentDataProvider { get; set; }

        [FindBy(Id = "ctl00_Main_LightStonePropertyID")]
        protected Span ctl00_Main_LightStonePropertyID { get; set; }

        [FindBy(Id = "ctl00_Main_AdCheckPropertyID")]
        protected Span ctl00_Main_AdCheckPropertyID { get; set; }
    }
}