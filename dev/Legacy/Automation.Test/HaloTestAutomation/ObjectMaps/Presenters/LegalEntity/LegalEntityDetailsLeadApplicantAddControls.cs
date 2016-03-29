using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class LegalEntityDetailsLeadApplicantAddControls : BasePageControls
    {
        public SelectList selectLegalEntityType
        {
            get
            {
                return base.Document.SelectList(new Regex("^[A-Za-z0-1_]*ddlLegalEntityTypes$"));
            }
        }

        public SelectList selectRole
        {
            get
            {
                return base.Document.SelectList(new Regex("^[A-Za-z0-1_]*ddlRoleTypeAdd$"));
            }
        }

        public CheckBox checkboxIncomeContributor
        {
            get
            {
                return base.Document.CheckBox(new Regex("^[A-Za-z0-1_]*chkAddIncomeContributor$"));
            }
        }

        public TextField textfieldIDNumber
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtNatAddIDNumber$"));
            }
        }

        public SelectList selectSalutation
        {
            get
            {
                return base.Document.SelectList(new Regex("^[A-Za-z0-1_]*ddlNatAddSalutation$"));
            }
        }

        public TextField textfieldInitials
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtNatAddInitials$"));
            }
        }

        public TextField textfieldFirstNames
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtNatAddFirstNames$"));
            }
        }

        public TextField textfieldSurname
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtNatAddSurname$"));
            }
        }

        public TextField textfieldPreferredName
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtNatAddPreferredName$"));
            }
        }

        public SelectList selectGender
        {
            get
            {
                return base.Document.SelectList(new Regex("^[A-Za-z0-1_]*ddlNatAddGender$"));
            }
        }

        public SelectList selectMaritalStatus
        {
            get
            {
                return base.Document.SelectList(new Regex("^[A-Za-z0-1_]*ddlNatAddMaritalStatus$"));
            }
        }

        public SelectList selectPopulationGroup
        {
            get
            {
                return base.Document.SelectList(new Regex("^[A-Za-z0-1_]*ddlNatAddpopulationGroup$"));
            }
        }

        public SelectList selectEducation
        {
            get
            {
                return base.Document.SelectList(new Regex("^[A-Za-z0-1_]*ddlNatAddEducation$"));
            }
        }

        public SelectList selectCitizenshipType
        {
            get
            {
                return base.Document.SelectList(new Regex("^[A-Za-z0-1_]*ddlNatAddCitizenshipType$"));
            }
        }

        public TextField textfieldDateOfBirth
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtNatAddDateOfBirth$"));
            }
        }

        public TextField textfieldPassportNumber
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtNatAddPassportNumber$"));
            }
        }

        public TextField textfieldTaxNumber
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtNatAddTaxNumber$"));
            }
        }

        public SelectList selectHomeLanguage
        {
            get
            {
                return base.Document.SelectList(new Regex("^[A-Za-z0-1_]*ddlNatAddHomeLanguage$"));
            }
        }

        public SelectList selectDocumentLanguage
        {
            get
            {
                return base.Document.SelectList(new Regex("^[A-Za-z0-1_]*ddlNatAddDocumentLanguage$"));
            }
        }

        public SelectList selectStatus
        {
            get
            {
                return base.Document.SelectList(new Regex("^[A-Za-z0-1_]*ddlNatAddStatus$"));
            }
        }

        public TextField textfieldHomePhoneCode
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtHomePhoneCode$"));
            }
        }

        public TextField textfieldHomePhoneNumber
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtHomePhoneNumber$"));
            }
        }

        public TextField textfieldWorkPhoneCode
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtWorkPhoneCode$"));
            }
        }

        public TextField textfieldWorkPhoneNumber
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtWorkPhoneNumber$"));
            }
        }

        public TextField textfieldFaxCode
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtFaxCode$"));
            }
        }

        public TextField textfieldFaxNumber
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtFaxNumber$"));
            }
        }

        public TextField textfieldCellphoneNo
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtCellPhoneNumber$"));
            }
        }

        public TextField textfieldEmailAddress
        {
            get
            {
                return base.Document.TextField(new Regex("^[A-Za-z0-1_]*txtEmailAddress$"));
            }
        }

        public CheckBox checkboxTelemarketing
        {
            get
            {
                return base.Document.CheckBox(new Regex("^[A-Za-z0-1_]*MarketingOption[A-Za-z]*ListBox_0$"));
            }
        }

        public CheckBox checkboxMarketing
        {
            get
            {
                return base.Document.CheckBox(new Regex("^[A-Za-z0-1_]*MarketingOption[A-Za-z]*ListBox_1$"));
            }
        }

        public CheckBox checkboxCustomerLists
        {
            get
            {
                return base.Document.CheckBox(new Regex("^[A-Za-z0-1_]*MarketingOption[A-Za-z]*ListBox_2$"));
            }
        }

        public CheckBox checkboxEmail
        {
            get
            {
                return base.Document.CheckBox(new Regex("^[A-Za-z0-1_]*MarketingOption[A-Za-z]*ListBox_3$"));
            }
        }

        public CheckBox checkboxSMS
        {
            get
            {
                return base.Document.CheckBox(new Regex("^[A-Za-z0-1_]*MarketingOption[A-Za-z]*ListBox_4$"));
            }
        }

        public Button btnCreateLead
        {
            get
            {
                return base.Document.Button(new Regex("^[A-Za-z0-1_]*btnSubmitButton$"));
            }
        }

        public Button btnCancel
        {
            get
            {
                return base.Document.Button(new Regex("^[A-Za-z0-1_]*btnCancelButton$"));
            }
        }

        public Button btnRefresh
        {
            get
            {
                return base.Document.Button(new Regex("^[A-Za-z0-1_]*btnRefresh$"));
            }
        }

        public Element SAHLAutoCompleteDiv_iframe
        {
            get
            {
                return base.Document.Element("SAHLAutoCompleteDiv_iframe");
            }
        }

        public Div SAHLAutoComplete_DefaultItem(string IDNumber)
        {
            return base.Document.Div("SAHLAutoCompleteDiv").Div(Find.ByText(new Regex("^" + IDNumber + "[\x20-\x7E]*$")));
        }

        public TextField textfieldCompanyName
        {
            get
            {
                return base.Document.TextField("ctl00_Main_txtCOAddCompanyName");
            }
        }

        public TextField textfieldTradingName
        {
            get
            {
                return base.Document.TextField("ctl00_Main_txtCOAddTradingName");
            }
        }

        public TextField textfieldRegistrationNumber
        {
            get
            {
                return base.Document.TextField("ctl00_Main_txtCOAddRegistrationNumber");
            }
        }

        [FindBy(Id = "ctl00_Main_ddlUpdDocumentLanguage")]
        protected SelectList ctl00MainddlUpdDocumentLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_ddlCOAddDocumentlanguage")]
        protected SelectList CompanyDocumentLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_txtCOAddTaxNumber")]
        protected TextField CompanyTaxNumber { get; set; }
    }
}