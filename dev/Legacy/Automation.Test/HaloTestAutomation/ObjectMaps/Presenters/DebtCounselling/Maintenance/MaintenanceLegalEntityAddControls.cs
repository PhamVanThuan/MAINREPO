using WatiN.Core;

namespace ObjectMaps
{
    public class MaintenanceLegalEntityAddControls : Page
    {
        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtWorkPhoneCode")]     
        public TextField ctl00MainapLegalEntityDetailscontenttxtWorkPhoneCode;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtWorkPhoneNumber")]  
        public TextField ctl00MainapLegalEntityDetailscontenttxtWorkPhoneNumber;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtFaxCode")]  
        public TextField ctl00MainapLegalEntityDetailscontenttxtFaxCode;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtFaxNumber")]  
        public TextField ctl00MainapLegalEntityDetailscontenttxtFaxNumber;

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_addressDetails_txtUnitNumber")]
        public TextField ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtUnitNumber;

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_addressDetails_txtBuildingNumber")]  
        public TextField ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtBuildingNumber;

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_addressDetails_txtBuildingName")]  
        public TextField ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtBuildingName;

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_addressDetails_txtStreetNumber")]  
        public TextField ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtStreetNumber;

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_addressDetails_txtStreetName")]  
        public TextField ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtStreetName;

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_addressDetails_txtSuburb")]  
        public TextField ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtSuburb;

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_addressDetails_txtCity")]  
        public TextField ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtCity;

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_addressDetails_txtPostalCode")]
        public TextField ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtPostalCode;

        [FindBy(Id = "ctl00_Main_SelectAddress")]  
        public Button ctl00MainSelectAddress;

        [FindBy(Id = "ctl00_Main_btnSubmitButton")]  
        public Button ctl00MainbtnSubmitButton;

        [FindBy(Id = "ctl00_Main_btnCancelButton")]  
        public Button ctl00MainbtnCancelButton;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_ddlLegalEntityTypes")]  
        public SelectList ctl00MainapLegalEntityDetailscontentddlLegalEntityTypes;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_ddlOrganisationType")]  
        public SelectList ctl00MainapLegalEntityDetailscontentddlOrganisationType;

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_addressDetails_ddlCountry")]  
        public SelectList ctl00MainapLegalEntityAddDetailscontentaddressDetailsddlCountry;

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_addressDetails_ddlProvince")]  
        public SelectList ctl00MainapLegalEntityAddDetailscontentaddressDetailsddlProvince;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtNatAddInitials")]  
        public TextField ctl00MainapLegalEntityDetailscontenttxtNatAddInitials;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtNatAddPreferredName")]  
        public TextField ctl00MainapLegalEntityDetailscontenttxtNatAddPreferredName;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtHomePhoneCode")]  
        public TextField ctl00MainapLegalEntityDetailscontenttxtHomePhoneCode;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtHomePhoneNumber")]
        public TextField ctl00MainapLegalEntityDetailscontenttxtHomePhoneNumber;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_ddlOSDescriptionTypeAdd")]  
        public SelectList ctl00MainapLegalEntityDetailscontentddlOSDescriptionTypeAdd;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_ddlNatAddSalutation")]  
        public SelectList ctl00MainapLegalEntityDetailscontentddlNatAddSalutation;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_ddlNatAddGender")]
        public SelectList ctl00MainapLegalEntityDetailscontentddlNatAddGender;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_ddlNatAddStatus")]  
        public SelectList ctl00MainapLegalEntityDetailscontentddlNatAddStatus;

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_addressDetails_txtPostOffice")]
        public TextField ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtPostOffice;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtEmailAddress")]
        public TextField ctl00MainapLegalEntityDetailscontenttxtEmailAddress;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_CORegisteredNameUpdate")]  
        public TextField ctl00MainapLegalEntityDetailscontentCORegisteredNameUpdate;

        [FindBy(Text = "Legal Entity Address Details")]  
        public Link linkLegalEntityAddressDetails;

        [FindBy(Text = "Legal Entity Details")]
        public Link linkLegalEntityDetails;

        [FindBy(Id = "ctl00_Main_lblParentOrgStructure")]  
        public Span spanctl00MainlblParentOrgStructure;

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_ddlAddressType")]  
        public SelectList ctl00MainapLegalEntityAddDetailscontentddlAddressType;

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_ddlAddressFormat")]
        public SelectList ctl00MainapLegalEntityAddDetailscontentddlAddressFormat;

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_dtEffectiveDate")]  
        public TextField ctl00MainapLegalEntityAddDetailscontentdtEffectiveDate;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtNatAddIDNumber")]
        public TextField ctl00MainapLegalEntityDetailscontenttxtNatAddIDNumber;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtNatAddFirstNames")]  
        public TextField ctl00MainapLegalEntityDetailscontenttxtNatAddFirstNames;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtNatAddSurname")]
        public TextField ctl00MainapLegalEntityDetailscontenttxtNatAddSurname;
 
        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtCellPhoneNumber")]
        public TextField ctl00MainapLegalEntityDetailscontenttxtCellPhoneNumber;

        [FindBy(Id = "SAHLAutoCompleteDiv_iframe")]  
        public Element SAHLAutoCompleteDiv_iframe;

        public DivCollection SAHLAutoComplete_DefaultItem_Collection()
        {
            return SAHLAutoComplete.Divs;
        }

        [FindBy(Id="SAHLAutoCompleteDiv")]
        public Div SAHLAutoComplete;

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_CORegistrationNumberUpdate")]
        public TextField ctl00MainapLegalEntityDetailscontentCORegistrationNumberUpdate;				
    }
}