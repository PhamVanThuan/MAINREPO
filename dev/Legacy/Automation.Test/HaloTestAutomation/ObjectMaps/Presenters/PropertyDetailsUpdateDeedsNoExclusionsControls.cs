using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class PropertyDetailsUpdateDeedsNoExclusionsControls : BasePageControls
    {
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

        [FindBy(Id = "ctl00_Main_txtBondAccountNumber")]
        protected TextField ctl00MaintxtBondAccountNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtTitleDeedNumber")]
        protected TextField ctl00MaintxtTitleDeedNumber { get; set; }

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

        [FindBy(Id = "ctl00_Main_ddlDeedsOffice")]
        protected SelectList ctl00MainddlDeedsOffice { get; set; }
    }
}