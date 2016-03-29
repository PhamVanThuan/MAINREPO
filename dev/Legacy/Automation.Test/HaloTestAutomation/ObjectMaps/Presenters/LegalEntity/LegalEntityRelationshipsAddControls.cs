using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LegalEntityRelationshipsAddControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnNewLegalEntity")]
        protected Button btnNewLegalEntity { get; set; }
    }
}