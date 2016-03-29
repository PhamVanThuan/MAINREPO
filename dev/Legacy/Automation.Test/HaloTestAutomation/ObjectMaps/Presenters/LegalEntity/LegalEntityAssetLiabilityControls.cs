using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class LegalEntityAssetLiabilityControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlType")]
        protected SelectList ddlType { get; set; }

        [FindBy(Id = "ctl00_Main_dtDateAcquired")]
        protected TextField dtDateAcquired { get; set; }

        [FindBy(Id = "ctl00_Main_ddlAddress")]
        protected SelectList ddlAddress { get; set; }

        [FindBy(Id = "ctl00_Main_btnAssociateAddress")]
        protected Button btnAssociateAddress { get; set; }

        [FindBy(Id = "ctl00_Main_txtAssetValue_txtRands")]
        protected TextField txtAssetValue_txtRands { get; set; }

        [FindBy(Id = "ctl00_Main_txtLiabilityValue_txtRands")]
        protected TextField txtLiabilityValue_txtRands { get; set; }

        [FindBy(Id = "ctl00_Main_txtLiabilityValue_txtRands")]
        protected TextField txtAssetValue_txtCents { get; set; }

        [FindBy(Id = "ctl00_Main_txtLiabilityValue_txtCents")]
        protected TextField txtLiabilityValue_txtCents { get; set; }

        [FindBy(Id = "ctl00_Main_btnAddUpdate")]
        protected Button btnAddUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_txtCompanyName")]
        protected TextField txtCompanyName { get; set; }

        [FindBy(Id = "ctl00_Main_txtDescription")]
        protected TextField ctl00_Main_txtDescription { get; set; }

        [FindBy(Id = "ctl00_Main_txtSurrenderValue_txtRands")]
        protected TextField ctl00_Main_txtSurrenderValue_txtRands { get; set; }

        [FindBy(Id = "ctl00_Main_ddlSubType")]
        protected SelectList ctl00_Main_ddlSubType { get; set; }

        [FindBy(Id = "ctl00_Main_txtFinancialInstitution")]
        public TextField ctl00_Main_txtFinancialInstitution { get; set; }

        [FindBy(Id = "ctl00_Main_dtDateRepayable")]
        protected TextField ctl00_Main_dtDateRepayable { get; set; }

        [FindBy(Id = "ctl00_Main_txtInstalmentValue_txtRands")]
        protected TextField ctl00_Main_txtInstalmentValue_txtRands { get; set; }

        [FindBy(Id = "ctl00_Main_btnAssociateAddress")]
        public Button CaptureAddress { get; set; }

        [FindBy(Id = "ctl00_Main_grdAssetLiability")]
        protected Table ctl00_Main_grdAssetLiability { get; set; }

        [FindBy(Id = "ctl00_Main_btnDelete")]
        protected Button ctl00_Main_btnDelete { get; set; }

        [FindBy(Id = "ctl00_Main_ddlAssociate")]
        protected SelectList ctl00_Main_ddlAssociate { get; set; }

        [FindBy(Id = "ctl00_Main_lblAddress")]
        protected Span AddressLabel { get; set; }

        [FindBy(Id = "ctl00_Main_lblAssetType")]
        protected Span AssetTypeLabel { get; set; }
    }
}