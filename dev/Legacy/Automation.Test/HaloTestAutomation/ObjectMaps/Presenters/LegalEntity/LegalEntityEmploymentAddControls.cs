//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ObjectMaps.Pages
{
    using WatiN.Core;

    /// <summary>
    ///
    /// </summary>
    public abstract class LegalEntityEmploymentAddControls : BasePageControls
    {
        /// <summary>
        ///
        /// </summary>
        [FindBy(Id = "ctl00_Main_pnlEmployerDetails_txtEmployerName")]
        protected TextField txtEmployerName { get; set; }

        protected Element SAHLAutoCompleteDiv_iframe
        {
            get
            {
                return base.Document.Element("SAHLAutoCompleteDiv_iframe");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="employerName"></param>
        /// <returns></returns>
        protected Div SAHLAutoComplete_DefaultItem(string employerName)
        {
            return base.Document.Div("SAHLAutoCompleteDiv").Div(Find.ByText(employerName));
        }

        /// <summary>
        ///
        /// </summary>
        protected DivCollection SAHLAutoComplete_DefaultItem_Collection
        {
            get
            {
                return base.Document.Div("SAHLAutoCompleteDiv").Divs;
            }
        }

        [FindBy(Id = "ctl00_Main_pnlEmploymentDetails_ddlEmploymentType")]
        protected SelectList ddlEmploymentType { get; set; }

        [FindBy(Id = "ctl00_Main_pnlEmploymentDetails_ddlEmploymentStatus")]
        protected SelectList ddlEmploymentStatus { get; set; }

        [FindBy(Id = "ctl00_Main_pnlEmploymentDetails_ddlRemunerationType")]
        protected SelectList ddlRemunerationType { get; set; }

        [FindBy(Id = "ctl00_Main_pnlEmploymentDetails_dtStartDate")]
        protected TextField dtStartDate { get; set; }

        protected Image dtStartDateImage
        {
            get
            {
                return base.Document.Image("ctl00_Main_pnlEmploymentDetails_ctl00_Main_pnlEmploymentDetails_dtStartDateImage");
            }
        }

        [FindBy(Id = "ctl00_Main_pnlEmploymentDetails_dtEndDate")]
        protected TextField dtEndDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        protected Image dtEndDateImage
        {
            get
            {
                return base.Document.Image("ctl00_Main_pnlEmploymentDetails_ctl00_Main_pnlEmploymentDetails_dtEndDateImage");
            }
        }

        [FindBy(Id = "ctl00_Main_pnlEmploymentDetails_currMonthlyIncome")]
        protected TextField currMonthlyIncome { get; set; }

        [FindBy(Id = "ctl00_Main_pnlEmploymentDetails_currMonthlyIncome_txtRands")]
        protected TextField txtRands { get; set; }

        [FindBy(Id = "ctl00_Main_pnlEmploymentDetails_currMonthlyIncome_txtCents")]
        protected TextField txtCents { get; set; }

        [FindBy(Id = "ctl00_Main_pnlEmploymentDetails_txtDepartment")]
        protected TextField txtDepartment { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_btnSave")]
        protected Button btnSave { get; set; }
    }
}