using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Data;

namespace SAHL.Web.Views.Common
{
    public partial class RequestPhysicalValuation : SAHLCommonBaseView , IRequestPhysicalValuation
    {
        public event EventHandler SubmitClicked;
        public event EventHandler CancelClicked;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindReasons(Dictionary<int, string> reasonDefinitions)
        {
            ddlValuationReasons.DataSource = reasonDefinitions;
            ddlValuationReasons.DataTextField = "Value";
            ddlValuationReasons.DataValueField = "Key";
            ddlValuationReasons.DataBind();
        }

        public void BindValuations(DataTable valuationsData)
        {
            requestedValuationsGrid.DataSource = valuationsData;
            requestedValuationsGrid.DataBind();
        }

        public void BindPropertyAccessDetails(IPropertyAccessDetails propertyAccessDetails)
        {
            if (propertyAccessDetails == null) return;

            txtContact1Name.Text = propertyAccessDetails.Contact1;
            txtContact1Phone.Text = propertyAccessDetails.Contact1Phone;
            txtContact1WorkPhone.Text = propertyAccessDetails.Contact1WorkPhone;
            txtContact1MobilePhone.Text = propertyAccessDetails.Contact1MobilePhone;
            txtContact2Name.Text = propertyAccessDetails.Contact2;
            txtContact2Phone.Text = propertyAccessDetails.Contact2Phone;
        }


        

        public string Contact1Name
        {
            get { return txtContact1Name.Text; }
        }

        public string Contact1Phone
        {
            get { return txtContact1Phone.Text; }
        }

        public string Contact1WorkPhone
        {
            get { return txtContact1WorkPhone.Text; }
        }
        
        public string Contact1MobilePhone
        {
            get { return txtContact1MobilePhone.Text; }
        }

        public string Contact2Name
        {
            get { return txtContact2Name.Text; }
        }

        public string Contact2Phone
        {
            get { return txtContact2Phone.Text; }
        }

        public DateTime? AssessmentDate
        {
            get { return dtAssessmentByDateValue.Date; }
        }

        public string SpecialInstructions
        {
            get { return this.txtSpecialInstructions.Text; }
        }

        public int SelectedValuationReasonDefinitionKey
        {
            get 
            {
                int reasonID = -1;
                if (Int32.TryParse(ddlValuationReasons.SelectedValue, out reasonID))
                {
                    return reasonID;
                }
                return -1;
            }
        }

        public string SelectedValuationReasonDescription
        {
            get { return ddlValuationReasons.SelectedItem.Text; }
        }


        public void DisableSubmitButton()
        {
            btnSubmit.Enabled = false;
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (SubmitClicked != null)
            {
                SubmitClicked(sender, e);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelClicked != null)
            {
                CancelClicked(sender, e);
            }
        }
    }
}