using System;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class ValuationScheduleLightStoneValuationView : SAHLCommonBaseView, IValuationScheduleLightstoneValuation
    {

        #region IValuationScheduleLightStoneValuation Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnValidatePropertyClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnInstructClicked;


        //***********************************************************************************
        // SET UP THE PROPERTY DISPLAY VISIBILITY METHODS



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
        // Get/Set the contact Details (Labels)

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

        // get/Set the contact Details (TextBoxes)


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



        //***********************************************************
        // SET UP THE BEHAVIOUR FOR THE DROPDOWN LISTS

        /// <summary>
        /// bind the object list to the 'ddlValuer' drop down list
        /// </summary>
        //public void bindddlValuer(DataTable DT)
        //{
        //    ddlValuer.Items.Clear();
        //    foreach (DataRow drow in DT.Rows)
        //    {
        //        ListItem item = new ListItem(drow[1].ToString(), drow[0].ToString(), true);
        //        if (ddlValuer.Items != null) ddlValuer.Items.Add(item);
        //    }
        //}

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



        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;
        }

        public void BindPropertyDetails(IProperty prop)
        {
            IAddressStreet addStreet = prop.Address as IAddressStreet;

            if (addStreet != null)
            {
                lblStreetValue.Text = addStreet.StreetNumber + " " + addStreet.StreetName;
                lblBuildingValue.Text = addStreet.BuildingNumber + " " + addStreet.BuildingName;
            }

            lblSuburbValue.Text = prop.Address.RRR_SuburbDescription;
            lblCityValue.Text = prop.Address.RRR_CityDescription;
            lblProvinceValue.Text = prop.Address.RRR_ProvinceDescription;
            lblCountryValue.Text = prop.Address.RRR_CountryDescription;
            lblPostalCodeValue.Text = prop.Address.RRR_PostalCode;
            txtPropertyDescription.Text = prop.PropertyDescription1 + " " + prop.PropertyDescription2 + " " + prop.PropertyDescription3;

            if (prop.DeedsPropertyType != null)
                lblDeedsPropertyTypeValue.Text = prop.DeedsPropertyType.Description;

            lblTitleTypeValue.Text = prop.TitleType.Description;

            if (prop.PropertyTitleDeeds.Count > 0)
                txtTitleDeedNumber.Text = prop.PropertyTitleDeeds[prop.PropertyTitleDeeds.Count - 1].TitleDeedNumber;

            lblOccupancyTypeValue.Text = prop.OccupancyType.Description;
            lblAreaClassificationValue.Text = prop.AreaClassification.Description;
            lblERFNumberValue.Text = prop.ErfNumber;
            lblPortionNumberValue.Text = prop.ErfPortionNumber;
            lblERFMetroDescriptionValue.Text = prop.ErfMetroDescription;
            lblERFSuburbValue.Text = prop.Address.RRR_SuburbDescription;
            lblSectionalSchemeNameValue.Text = prop.SectionalSchemeName;
            lblSectionalUnitNumberValue.Text = prop.SectionalUnitNumber;
            lblAssessmentByDateValue.Text = DateTime.Now.ToShortDateString();
        }

        public void BindPropertyAccessDetails(IPropertyAccessDetails pad)
        {
            // TextBoxes
            txtContact1Name.Text = pad.Contact1;
            txtContact1MobilePhone.Text = pad.Contact1MobilePhone;
            txtContact1Phone.Text = pad.Contact1Phone;
            txtContact1WorkPhone.Text = pad.Contact1WorkPhone;
            txtContact2Name.Text = pad.Contact2;
            txtContact2Phone.Text = pad.Contact2Phone;
            //Labels
            lblContact1Value.Text = pad.Contact1;
            lblCellPhone1Value.Text = pad.Contact1MobilePhone;
            lblPhone1Value.Text = pad.Contact1Phone;
            lblWorkPhone1Value.Text = pad.Contact1WorkPhone;
            lblContact2Value.Text = pad.Contact2;
            lblPhone2Value.Text = pad.Contact2Phone;
        }

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

    }
}