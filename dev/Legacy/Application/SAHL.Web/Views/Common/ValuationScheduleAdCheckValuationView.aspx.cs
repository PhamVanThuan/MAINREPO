using System;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class ValuationScheduleAdCheckValuationView : SAHLCommonBaseView, IValuationScheduleAdCheckValuation
    {
        private int propertyItemIndex;

        #region IValuationScheduleAdCheckValuation Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnValidatePropertyClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnInstructClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler grdPropertiesSelectedIndexChanged;

        //***********************************************************************************
        // SET UP THE PROPERTY DISPLAY VISIBILITY METHODS

        /// <summary>
        /// Property to show/hide the 'ddlValuer' object
        /// </summary>
        public bool ShowddlValuer
        {
            set { ddlValuer.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblValuerValue' object
        /// </summary>
        public bool ShowlblValuerValue
        {
            set { lblValuerValue.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'dtAssessmentByDateValue' object
        /// </summary>
        public bool ShowdtAssessmentByDateValue
        {
            set { dtAssessmentByDateValue.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblAssessmentByDateValue' object
        /// </summary>
        public bool ShowlblAssessmentByDateValue
        {
            set { lblAssessmentByDateValue.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'ddlAssessmentReasonValue' object
        /// </summary>
        public bool ShowddlAssessmentReasonValue
        {
            set { ddlAssessmentReasonValue.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblAssessmentReasonValue' object
        /// </summary>
        public bool ShowlblAssessmentReasonValue
        {
            set { lblAssessmentReasonValue.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'ddlAssessmentPriorityValue' object
        /// </summary>
        public bool ShowddlAssessmentPriorityValue
        {
            set { ddlAssessmentPriorityValue.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblAssessmentPriorityValue' object
        /// </summary>
        public bool ShowlblAssessmentPriorityValue
        {
            set { lblAssessmentPriorityValue.Visible = value; }
        }

        // SHOW / HIDE THE CONTACT DETAILS (LABELS)

        /// <summary>
        /// Property to show/hide the 'lblContact1Value' object
        /// </summary>
        public bool ShowlblContact1Value
        {
            set { lblContact1Value.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblPhone1Value' object
        /// </summary>
        public bool ShowlblPhone1Value
        {
            set { lblPhone1Value.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblWorkPhone1Value' object
        /// </summary>
        public bool ShowlblWorkPhone1Value
        {
            set { lblWorkPhone1Value.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblCellPhone1Value' object
        /// </summary>
        public bool ShowlblCellPhone1Value
        {
            set { lblCellPhone1Value.Visible = value; }
        }


        /// <summary>
        /// Property to show/hide the 'lblContact2Value' object
        /// </summary>
        public bool ShowlblContact2Value
        {
            set { lblContact2Value.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblPhone2Value' object
        /// </summary>
        public bool ShowlblPhone2Value
        {
            set { lblPhone2Value.Visible = value; }
        }

        // SHOW / HIDE THE CONTACT DETAILS (TextBoxes)

        /// <summary>
        /// Property to show/hide the 'txtContact1Name' object
        /// </summary>
        public bool ShowtxtContact1Name
        {
            set { txtContact1Name.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'txtContact1Phone' object
        /// </summary>
        public bool ShowtxtContact1Phone
        {
            set { txtContact1Phone.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'txtContact1WorkPhone' object
        /// </summary>
        public bool ShowtxtContact1WorkPhone
        {
            set { txtContact1WorkPhone.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'txtContact1MobilePhone' object
        /// </summary>
        public bool ShowtxtContact1MobilePhone
        {
            set { txtContact1MobilePhone.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'txtContact2Name' object
        /// </summary>
        public bool ShowtxtContact2Name
        {
            set { txtContact2Name.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'txtContact2Phone' object
        /// </summary>
        public bool ShowtxtContact2Phone
        {
            set { txtContact2Phone.Visible = value; }
        }

        //  SHOW/HIDE THE FORM NAVIGATION BUTTONS

        /// <summary>
        /// Property to show/hide the 'btnInstruct' button
        /// </summary>
        public bool ShowbtnInstruct
        {
            set { btnInstruct.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'btnValidate' button
        /// </summary>
        public bool ShowbtnValidateProperty
        {
            set { btnValidate.Visible = value; }
        }


        /// <summary>
        /// Property to show/hide the 'txtSpecialInstructions' text box
        /// </summary>
        public bool ShowtxtSpecialInstructions
        {
            set { txtSpecialInstructions.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'pnlValuationAssignmentDetails' panel
        /// </summary>
        public bool ShowpnlValuationAssignmentDetails
        {
            set { pnlValuationAssignmentDetails.Visible = value; }
        }

        //***********************************************************************************

        /// <summary>
        /// Property to get or set the 'lblAssessmentNumberValue' text
        /// </summary>
        public string SetlblAssessmentNumberValue
        {
            set { lblAssessmentNumberValue.Text = value; }
            get { return lblAssessmentNumberValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblRequestNumberValue' text
        /// </summary>
        public string SetlblRequestNumberValue
        {
            set { lblRequestNumberValue.Text = value; }
            get { return lblRequestNumberValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblApplicationNameValue' text
        /// </summary>
        public string SetlblApplicationNameValue
        {
            set { lblApplicationNameValue.Text = value; }
            get { return lblApplicationNameValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblRequestedByValue' text
        /// </summary>
        public string SetlblRequestedByValue
        {
            set { lblRequestedByValue.Text = value; }
            get { return lblRequestedByValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblBuildingValue' text
        /// </summary>
        public string SetlblBuildingValue
        {
            set { lblBuildingValue.Text = value; }
            get { return lblBuildingValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblTitleTypeValue' text
        /// </summary>
        public string SetlblTitleTypeValue
        {
            set { lblTitleTypeValue.Text = value; }
            get { return lblTitleTypeValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblStreetValue' text
        /// </summary>
        public string SetlblStreetValue
        {
            set { lblStreetValue.Text = value; }
            get { return lblStreetValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblOccupancyTypeValue' text
        /// </summary>
        public string SetlblOccupancyTypeValue
        {
            set { lblOccupancyTypeValue.Text = value; }
            get { return lblOccupancyTypeValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblSuburbValue' text
        /// </summary>
        public string SetlblSuburbValue
        {
            set { lblSuburbValue.Text = value; }
            get { return lblSuburbValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblAreaClassificationValue' text
        /// </summary>
        public string SetlblAreaClassificationValue
        {
            set { lblAreaClassificationValue.Text = value; }
            get { return lblAreaClassificationValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblCityValue' text
        /// </summary>
        public string SetlblCityValue
        {
            set { lblCityValue.Text = value; }
            get { return lblCityValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'txtPropertyDescription' text
        /// </summary>
        public string SettxtPropertyDescription
        {
            set { txtPropertyDescription.Text = value; }
            get { return txtPropertyDescription.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblProvinceValue' text
        /// </summary>
        public string SetlblProvinceValue
        {
            set { lblProvinceValue.Text = value; }
            get { return lblProvinceValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblERFNumberValue' text
        /// </summary>
        public string SetlblERFNumberValue
        {
            set { lblERFNumberValue.Text = value; }
            get { return lblERFNumberValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblCountryValue' text
        /// </summary>
        public string SetlblCountryValue
        {
            set { lblCountryValue.Text = value; }
            get { return lblCountryValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblPortionNumberValue' text
        /// </summary>
        public string SetlblPortionNumberValue
        {
            set { lblPortionNumberValue.Text = value; }
            get { return lblPortionNumberValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblPostalCodeValue' text
        /// </summary>
        public string SetlblPostalCodeValue
        {
            set { lblPostalCodeValue.Text = value; }
            get { return lblPostalCodeValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblERFSuburbValue' text
        /// </summary>
        public string SetlblERFSuburbValue
        {
            set { lblERFSuburbValue.Text = value; }
            get { return lblERFSuburbValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'txtTitleDeedNumber' text
        /// </summary>
        public string SettxtTitleDeedNumber
        {
            set { txtTitleDeedNumber.Text = value; }
            get { return txtTitleDeedNumber.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblERFMetroDescriptionValue' text
        /// </summary>
        public string SetlblERFMetroDescriptionValue
        {
            set { lblERFMetroDescriptionValue.Text = value; }
            get { return lblERFMetroDescriptionValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblDeedsPropertyTypeValue' text
        /// </summary>
        public string SetlblDeedsPropertyTypeValue
        {
            set { lblDeedsPropertyTypeValue.Text = value; }
            get { return lblDeedsPropertyTypeValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblSectionalSchemeNameValue' text
        /// </summary>
        public string SetlblSectionalSchemeNameValue
        {
            set { lblSectionalSchemeNameValue.Text = value; }
            get { return lblSectionalSchemeNameValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblSAPTGPropertyNumberValue' text
        /// </summary>
        public string SetlblSAPTGPropertyNumberValue
        {
            set { lblSAPTGPropertyNumberValue.Text = value; }
            get { return lblSAPTGPropertyNumberValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblSectionalUnitNumberValue' text
        /// </summary>
        public string SetlblSectionalUnitNumberValue
        {
            set { lblSectionalUnitNumberValue.Text = value; }
            get { return lblSectionalUnitNumberValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblValuerValue' text
        /// </summary>
        public string SetlblValuerValue
        {
            set { lblValuerValue.Text = value; }
            get { return lblValuerValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblAssessmentByDateValue' text
        /// </summary>
        public string SetlblAssessmentByDateValue
        {
            set { lblAssessmentByDateValue.Text = value; }
            get
            {
                if (Request.Form[lblAssessmentByDateValue.UniqueID] != null)
                    return Convert.ToString(Request.Form[lblAssessmentByDateValue.UniqueID]);

                return "";
            }
        }


        /// <summary>
        /// Property to get or set the 'dtAssessmentByDateValue' text
        /// </summary>
        public DateTime SetdtAssessmentByDateValue
        {
            set { dtAssessmentByDateValue.Date = value; }
            get { return Convert.ToDateTime(dtAssessmentByDateValue.Date); }
        }

        /// <summary>
        /// Property to get or set the 'lblAssessmentReasonValue' text
        /// </summary>
        public string SetlblAssessmentReasonValue
        {
            set { lblAssessmentReasonValue.Text = value; }
            get { return lblAssessmentReasonValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblAssessmentPriorityValue' text
        /// </summary>
        public string SetlblAssessmentPriorityValue
        {
            set { lblAssessmentPriorityValue.Text = value; }
            get { return lblAssessmentPriorityValue.Text; }
        }

        /// <summary>
        /// Property to get or set the 'txtSpecialInstructions' text
        /// </summary>
        public string SettxtSpecialInstructions
        {
            set { txtSpecialInstructions.Text = value; }
            get { return txtSpecialInstructions.Text; }
        }


        /// <summary>
        ///  Set the ReadOnly status of txtSpecialInstructions
        /// </summary>
        public bool SettxtSpecialInstructionsReadOnly
        {
            set { txtSpecialInstructions.ReadOnly = value; }
            get { return txtSpecialInstructions.ReadOnly; }
        }

        /// <summary>
        ///  Set the ReadOnly status of txtPropertyDescription
        /// </summary>
        public bool SettxtPropertyDescriptionReadOnly
        {
            set { txtPropertyDescription.ReadOnly = value; }
            get { return txtPropertyDescription.ReadOnly; }
        }

        //***********************************************************
        // Set the contact Details (Labels)

        /// <summary>
        /// Property to get or set the 'lblContact1Value' text
        /// </summary>
        public string SetlblContact1Value
        {
            set { lblContact1Value.Text = value; }
            get { return lblContact1Value.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblPhone1Value' text
        /// </summary>
        public string SetlblPhone1Value
        {
            set { lblPhone1Value.Text = value; }
            get { return lblPhone1Value.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblWorkPhone1Value' text
        /// </summary>
        public string SetlblWorkPhone1Value
        {
            set { lblWorkPhone1Value.Text = value; }
            get { return lblWorkPhone1Value.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblCellPhone1Value' text
        /// </summary>
        public string SetlblCellPhone1Value
        {
            set { lblCellPhone1Value.Text = value; }
            get { return lblCellPhone1Value.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblContact2Value' text
        /// </summary>
        public string SetlblContact2Value
        {
            set { lblContact2Value.Text = value; }
            get { return lblContact2Value.Text; }
        }

        /// <summary>
        /// Property to get or set the 'lblPhone2Value' text
        /// </summary>
        public string SetlblPhone2Value
        {
            set { lblPhone2Value.Text = value; }
            get { return lblPhone2Value.Text; }
        }

        // Set the contact Details (TextBoxes)


        /// <summary>
        /// Property to get or set the 'txtContact1Name' text
        /// </summary>
        public string SettxtContact1Name
        {
            set { txtContact1Name.Text = value; }
            get { return txtContact1Name.Text; }
        }


        /// <summary>
        /// Property to get or set the 'txtContact1Phone' text
        /// </summary>
        public string SettxtContact1Phone
        {
            set { txtContact1Phone.Text = value; }
            get { return txtContact1Phone.Text; }
        }

        /// <summary>
        /// Property to get or set the 'txtContact1WorkPhone' text
        /// </summary>
        public string SettxtContact1WorkPhone
        {
            set { txtContact1WorkPhone.Text = value; }
            get { return txtContact1WorkPhone.Text; }
        }

        /// <summary>
        /// Property to get or set the 'txtContact1MobilePhone' text
        /// </summary>
        public string SettxtContact1MobilePhone
        {
            set { txtContact1MobilePhone.Text = value; }
            get { return txtContact1MobilePhone.Text; }
        }

        /// <summary>
        /// Property to get or set the 'txtContact2Name' text
        /// </summary>
        public string SettxtContact2Name
        {
            set { txtContact2Name.Text = value; }
            get { return txtContact2Name.Text; }
        }

        /// <summary>
        /// Property to get or set the 'txtContact2Phone' text
        /// </summary>
        public string SettxtContact2Phone
        {
            set { txtContact2Phone.Text = value; }
            get { return txtContact2Phone.Text; }
        }

        /// <summary>
        /// Property to get or set the 'ddlValuerSelectedIndex' SelectedValue
        /// </summary>
        public int SetddlValuerSelectedIndex
        {
            set { ddlValuer.SelectedValue = value.ToString(); }

            get
            {
                if (Request.Form[ddlValuer.UniqueID] != null && Request.Form[ddlValuer.UniqueID] != "-select-")
                    return Convert.ToInt32(Request.Form[ddlValuer.UniqueID]);

                return -1;
            }
        }


        /// <summary>
        /// Property to get or set the 'ddlValuerSelectedIndex' SelectedValue
        /// </summary>
        public int SetddlPropertiesSelectedSelectedIndex
        {
            set { grdProperties.SelectedIndex = value; }

            get { return Convert.ToInt32(Request.Form[grdProperties.SelectedIndex]); }
        }

        /// <summary>
        /// Property to get or set the 'SetddlAssessmentPrioritySelectedIndex' SelectedValue
        /// </summary>
        public int SetddlAssessmentPrioritySelectedIndex
        {
            set { ddlAssessmentPriorityValue.SelectedIndex = value; }

            get
            {
                if (Request.Form[ddlAssessmentPriorityValue.UniqueID] != null && Request.Form[ddlAssessmentPriorityValue.UniqueID] != "-select-")
                    return Convert.ToInt32(Request.Form[ddlAssessmentPriorityValue.UniqueID]);

                return -1;
            }
        }

        /// <summary>
        /// Property to get or set the 'ddlAssessmentReasonValue' SelectedValue
        /// </summary>
        public int ddlAssessmentReasonValueSelectedIndex
        {
            set { ddlAssessmentReasonValue.SelectedValue = Convert.ToString(value); }

            get
            {
                return Convert.ToInt32(ddlAssessmentReasonValue.SelectedValue);
            }
        }

        /// <summary>
        /// Property to get or set the 'ddlAssessmentPriorityValue' SelectedValue
        /// </summary>
        public int SetddlAssessmentPriorityValue
        {
            set { ddlAssessmentPriorityValue.SelectedValue = value.ToString(); }

            get
            {
                if (Request.Form[ddlAssessmentPriorityValue.UniqueID] != null && Request.Form[ddlAssessmentPriorityValue.UniqueID] != "-select-")
                    return Convert.ToInt32(Request.Form[ddlAssessmentPriorityValue.UniqueID]);

                return -1;
            }
        }

        /// <summary>
        /// Property to get or set the 'ddlAssessmentReasonValue' SelectedValue
        /// </summary>
        public int SetddlAssessmentReasonValue
        {

            set { ddlAssessmentReasonValue.SelectedValue = value.ToString(); }

            get
            {
                if (Request.Form[ddlAssessmentReasonValue.UniqueID] != null && Request.Form[ddlAssessmentReasonValue.UniqueID] != "-select-")
                    return Convert.ToInt32(Request.Form[ddlAssessmentReasonValue.UniqueID]);

                return -1;
            }

        }


        /// <summary>
        /// Property to get or set the 'ddlAssessmentPriorityValue' Selected Text
        /// </summary>
        public string SetddlAssessmentPriorityText
        {
            get { return ddlAssessmentPriorityValue.Text; }
        }

        /// <summary>
        /// Bind the Properties Grid
        /// </summary>
        /// <param name="DT"></param>
        public void BindgrdProperties(DataTable DT)
        {
            grdProperties.AutoGenerateColumns = false;
            grdProperties.AddGridBoundColumn("PropertyID", "Property ID", Unit.Percentage(0), HorizontalAlign.Left, true);
            grdProperties.AddGridBoundColumn("LegalDescription", "Address", Unit.Percentage(0), HorizontalAlign.Left, true);
            //grdProperties.AddGridBoundColumn("PROP_ID", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            //grdProperties.AddGridBoundColumn("UNIT", "Unit", Unit.Percentage(10), HorizontalAlign.Left, true);
            //grdProperties.AddGridBoundColumn("STREET_NUMBER", "Number", Unit.Percentage(10), HorizontalAlign.Left, true);
            //grdProperties.AddGridBoundColumn("STREET_NAME", "Street", Unit.Percentage(20), HorizontalAlign.Left, true);
            //grdProperties.AddGridBoundColumn("STREET_TYPE", "Type", Unit.Percentage(10), HorizontalAlign.Left, true);
            //grdProperties.AddGridBoundColumn("SUBURB", "suburb", Unit.Percentage(20), HorizontalAlign.Left, true);
            //grdProperties.AddGridBoundColumn("DEEDTOWN", "Town", Unit.Percentage(20), HorizontalAlign.Left, true);
            //grdProperties.AddGridBoundColumn("ERF", "ERF", Unit.Percentage(10), HorizontalAlign.Left, true);
            grdProperties.DataSource = DT;
            grdProperties.DataBind();
        }


        //***********************************************************
        // SET UP THE BEHAVIOUR FOR THE DROPDOWN LISTS

        /// <summary>
        /// bind the object list to the 'ddlValuer' drop down list
        /// </summary>
        public void bindddlValuer(DataTable DT)
        {
            ddlValuer.Items.Clear();
            foreach (DataRow drow in DT.Rows)
            {
                ListItem item = new ListItem(drow[1].ToString(), drow[0].ToString(), true);
                if (ddlValuer.Items != null) ddlValuer.Items.Add(item);
            }
        }

        /// <summary>
        /// bind the object list to the 'ddlAssessmentReasonValue' drop down list
        /// </summary>
        public void bindddlAssessmentReasonValue(DataTable DT)
        {
            ddlAssessmentReasonValue.Items.Clear();
            foreach (DataRow drow in DT.Rows)
            {
                ListItem item = new ListItem(drow[0].ToString(), drow[1].ToString(), true);
                if (ddlAssessmentReasonValue.Items != null) ddlAssessmentReasonValue.Items.Add(item);
            }
        }

        /// <summary>
        ///  bind the object list to the 'ddlAssessmentPriorityValue' drop down list
        /// </summary>
        public void bindddlAssessmentPriorityValue(DataTable DT)
        {
            ddlAssessmentPriorityValue.Items.Clear();
            foreach (DataRow drow in DT.Rows)
            {
                ListItem item = new ListItem(drow[0].ToString(), drow[1].ToString(), true);
                if (ddlAssessmentPriorityValue.Items != null) ddlAssessmentPriorityValue.Items.Add(item);
            }
        }


        /// <summary>
        ///  Set or Get the Properties Grid Postbacktype
        /// </summary>
        public GridPostBackType SetPostBackTypegrdProperties
        {
            set { grdProperties.PostBackType = value; }
            get { return grdProperties.PostBackType; }
        }

        /// <summary>
        /// Property to show/hide the 'grdProperties' grid
        /// </summary>
        public bool ShowgrdProperties
        {
            set { grdProperties.Visible = value; }
        }


        /// <summary>
        /// Property to show/hide the 'pnlSelectProperty' panel
        /// </summary>
        public bool ShowpnlSelectProperty
        {
            set { pnlSelectProperty.Visible = value; }
            get { return pnlSelectProperty.Visible; }
        }

        /// <summary>
        /// Gets or sets the Proeprty Item index
        /// </summary>
        public int PropertyItemIndex
        {
            set { propertyItemIndex = value; }
            get { return propertyItemIndex; }
        }

        #endregion

        protected void btnInstruct_Click(object sender, EventArgs e)
        {
            // Return to Default Screen
            if (btnInstructClicked != null)
                btnInstructClicked(sender, e);
        }

        protected void btnValidateProperty_Click(object sender, EventArgs e)
        {
            // Return to Default Screen
            if (btnValidatePropertyClicked != null)
                btnValidatePropertyClicked(sender, e);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;
        }

        protected void grdProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            PropertyItemIndex = grdProperties.SelectedIndex;
            if (grdPropertiesSelectedIndexChanged != null)
                grdPropertiesSelectedIndexChanged(sender, new KeyChangedEventArgs(grdProperties.SelectedIndex));
        }
    }
}