using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Utils;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI;
using SAHL.Web.AJAX;
using SAHL.Common.Web.UI.Events;
using System.Diagnostics.CodeAnalysis;
using SAHL.Web.Controls.Interfaces;

namespace SAHL.Web.Controls
{

    /// <summary>
    /// Possible edit modes for the <see cref="EmployerDetails"/> control.
    /// </summary>
    public enum EmployerDetailsEditMode
    {
        /// <summary>
        /// The employer details control cannot be edited.
        /// </summary>
        ReadOnly,

        /// <summary>
        /// Both the name and the details can be edited.
        /// </summary>
        EditAll,

        /// <summary>
        /// Only the name can be edited - all other employer details are read-only.
        /// </summary>
        EditName,

        /// <summary>
        /// Only the employer details can be edited - the name is read-only.
        /// </summary>
        EditDetails
    }

    /// <summary>
    /// This control displays employer details.  Note that this control does NOT retain employer information 
    /// via viewstate - the employer needs to be set on every postback.
    /// </summary>
    public class EmployerDetails : DetailsPanel, IEmployerDetails
    {
        /// <summary>
        /// Event that occurs when an employer is selected from the autocomplete list.
        /// </summary>
        public event KeyChangedEventHandler EmployerSelected;

        private Panel _pnlMain;
        private IEmployer _employer;
        private SAHLTextBox _txtEmployerName;
        private SAHLAutoComplete _acEmployerName;
        private IEmploymentRepository _employmentRepository;
        private ILookupRepository _lookupRepository;

        private EmployerDetailsEditMode _employerDetailsEditMode = EmployerDetailsEditMode.ReadOnly;
        private SAHLDropDownList _ddlBusinessType;
        private SAHLDropDownList _ddlEmploymentSector;
        private SAHLTextBox _txtContactPerson;
        private SAHLPhone _phnContactPersonPhone;
        private SAHLTextBox _txtContactEmail;
        private SAHLTextBox _txtAccountant;
        private SAHLTextBox _txtAccountantContact;
        private SAHLPhone _phnAccountantNumber;
        private SAHLTextBox _txtAccountantEmail;

        private HtmlInputHidden _hidEditMode;
        private HtmlInputHidden _hidEmployerKey;
        private HtmlInputHidden _hidEmployerName;

        /// <summary>
        /// Constructor.  This sets the default values of the control.
        /// </summary>
        public EmployerDetails()
        {
            GroupingText = "Employer Details";

            // add an internal panel so we can set the height within the group

            _pnlMain = new Panel();
            _pnlMain.ID = "pnlMain";
            this.Controls.Add(_pnlMain);

            _txtEmployerName = new SAHLTextBox();
            _txtEmployerName.ID = "txtEmployerName";
            _txtEmployerName.Columns = 30;
            _txtEmployerName.MaxLength = 50;
            _txtEmployerName.Mandatory = true;
            _pnlMain.Controls.Add(_txtEmployerName);

            _acEmployerName = new SAHLAutoComplete();
            _acEmployerName.ID = "acEmployerName";
            _acEmployerName.TargetControlID = _txtEmployerName.ID;
            _acEmployerName.ServiceMethod = "SAHL.Web.AJAX.Employment.GetEmployers";
            _acEmployerName.AutoPostBack = true;
            _acEmployerName.ItemSelected += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_acEmployerName_ItemSelected);
            _pnlMain.Controls.Add(_acEmployerName);

            _ddlBusinessType = new SAHLDropDownList();
            _ddlBusinessType.ID = "ddlBusinessType";
            _ddlBusinessType.Mandatory = true;
            _pnlMain.Controls.Add(_ddlBusinessType);

            _ddlEmploymentSector = new SAHLDropDownList();
            _ddlEmploymentSector.ID = "ddlEmploymentSector";
            _ddlEmploymentSector.Mandatory = true;
            _pnlMain.Controls.Add(_ddlEmploymentSector);

            _txtContactPerson = new SAHLTextBox();
            _txtContactPerson.ID = "txtContactPerson";
            _txtContactPerson.Columns = 30;
            _txtContactPerson.MaxLength = 50;
            _txtContactPerson.Mandatory = true;
            _pnlMain.Controls.Add(_txtContactPerson);

            _phnContactPersonPhone = new SAHLPhone();
            _phnContactPersonPhone.ID = "phnContactPersonPhone";
            _phnContactPersonPhone.CssClass = "mandatory";
            _pnlMain.Controls.Add(_phnContactPersonPhone);

            _txtContactEmail = new SAHLTextBox();
            _txtContactEmail.ID = "txtContactEmail";
            _txtContactEmail.Columns = 30;
            _txtContactEmail.MaxLength = 50;
            _txtContactEmail.DisplayInputType=InputType.AlphaNumNoSpace;
            
            _pnlMain.Controls.Add(_txtContactEmail);

            _txtAccountant = new SAHLTextBox();
            _txtAccountant.ID = "txtAccountant";
            _txtAccountant.Columns = 30;
            _txtAccountant.MaxLength = 50;
            _pnlMain.Controls.Add(_txtAccountant);

            _txtAccountantContact = new SAHLTextBox();
            _txtAccountantContact.ID = "txtAccountantContact";
            _txtAccountantContact.Columns = 30;
            _txtAccountantContact.MaxLength = 50;
            _pnlMain.Controls.Add(_txtAccountantContact);

            _phnAccountantNumber = new SAHLPhone();
            _phnAccountantNumber.ID = "phnAccountantNumber";
            _pnlMain.Controls.Add(_phnAccountantNumber);

            _txtAccountantEmail = new SAHLTextBox();

            _txtAccountantEmail.ID = "txtAccountantEmail";
            _txtAccountantEmail.Columns = 30;
            _txtAccountantEmail.MaxLength = 50;
            _txtAccountantEmail.DisplayInputType = InputType.AlphaNumNoSpace;
            _pnlMain.Controls.Add(_txtAccountantEmail);

            _hidEditMode = new HtmlInputHidden();
            _hidEditMode.ID = "hidEditMode";
            _pnlMain.Controls.Add(_hidEditMode);

            _hidEmployerKey = new HtmlInputHidden();
            _hidEmployerKey.ID = "hidEmployerKey";
            _pnlMain.Controls.Add(_hidEmployerKey);

            _hidEmployerName = new HtmlInputHidden();
            _hidEmployerName.ID = "hidEmployerName";
            _pnlMain.Controls.Add(_hidEmployerName);

        }

        #region Properties

        /// <summary>
        /// Gets/sets the current edit mode of the control.  This defaults to <see cref="EmployerDetailsEditMode.ReadOnly"/>.
        /// </summary>
        public EmployerDetailsEditMode EditMode
        { 
            get 
            {
                return _employerDetailsEditMode;
            }
            set
            {
                _employerDetailsEditMode = value;
            }
        }

        /// <summary>
        /// Gets/sets the <see cref="IEmployer"/> displayed on the control.
        /// </summary>
        public IEmployer Employer
        {
            get
            {
                if (_employer == null)
                {

                    if (String.IsNullOrEmpty(_hidEmployerKey.Value))
                    {
                        _employer = _employmentRepository.GetEmptyEmployer();
                    }
                    else
                    {
                        _employer = _employmentRepository.GetEmployerByKey(Int32.Parse(_hidEmployerKey.Value));
                    }
                }


                if (String.IsNullOrEmpty(_txtEmployerName.Text))
                    _employer.Name = _hidEmployerName.Value;
                else
                    _employer.Name = _txtEmployerName.Text;
                _employer.AccountantContactPerson = _txtAccountantContact.Text;
                _employer.AccountantEmail = _txtAccountantEmail.Text;
                _employer.AccountantName = _txtAccountant.Text;
                _employer.AccountantTelephoneCode = _phnAccountantNumber.Code;
                _employer.AccountantTelephoneNumber = _phnAccountantNumber.Number;
                _employer.ContactEmail = _txtContactEmail.Text;
                _employer.ContactPerson = _txtContactPerson.Text;
                if (_ddlBusinessType.SelectedValue != SAHLDropDownList.PleaseSelectValue)
                    _employer.EmployerBusinessType = _lookupRepository.EmployerBusinessTypes.ObjectDictionary[_ddlBusinessType.SelectedValue];
                else
                    _employer.EmployerBusinessType = null;
                if (_ddlEmploymentSector.SelectedValue != SAHLDropDownList.PleaseSelectValue)
                    _employer.EmploymentSector = _lookupRepository.EmploymentSectors.ObjectDictionary[_ddlEmploymentSector.SelectedValue];
                else
                    _employer.EmploymentSector = null;
                _employer.TelephoneCode = _phnContactPersonPhone.Code;
                _employer.TelephoneNumber = _phnContactPersonPhone.Number;

                return _employer;
            }
            set
            {
                _employer = value;
                if (_employer == null)
                    _employer = _employmentRepository.GetEmptyEmployer();

                // update the value of the input fields to reflect the employer record
                _hidEmployerKey.Value = _employer.Key.ToString();
                _txtAccountantContact.Text = _employer.AccountantContactPerson;
                _txtAccountantEmail.Text = _employer.AccountantEmail;
                _txtAccountant.Text = _employer.AccountantName;
                _phnAccountantNumber.Code = _employer.AccountantTelephoneCode;
                _phnAccountantNumber.Number = _employer.AccountantTelephoneNumber;
                _txtContactEmail.Text = _employer.ContactEmail;
                _txtContactPerson.Text = _employer.ContactPerson;
                if (_employer.EmployerBusinessType != null)
                    _ddlBusinessType.SelectedValue = _employer.EmployerBusinessType.Key.ToString();
                else
                    _ddlBusinessType.ClearSelection();
                if (_employer.EmploymentSector != null)
                    _ddlEmploymentSector.SelectedValue = _employer.EmploymentSector.Key.ToString();
                else
                    _ddlEmploymentSector.ClearSelection();
                _txtEmployerName.Text = _employer.Name;
                _hidEmployerName.Value = _employer.Name;
                _phnContactPersonPhone.Code = _employer.TelephoneCode;
                _phnContactPersonPhone.Number = _employer.TelephoneNumber;
            }
        }

        /// <summary>
        /// Gets the key of the selected employer, if there is one.
        /// </summary>
        public int? EmployerKey
        {
            get
            {
                int i = 0;
                if (Int32.TryParse(_hidEmployerKey.Value, out i))
                    return new int?(i);

                return new int?();
            }
        }

        #endregion

        #region Methods

        private void AddRow(string title, string displayValue, Control c, bool readOnly)
        {

            if (readOnly)
                base.AddRow(_pnlMain, title, displayValue);
            else
            {
                base.AddRow(_pnlMain, title, c);
            }

            c.Visible = !readOnly;

        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearEmployer()
        {
            Employer = null;
        }

        /// <summary>
        /// Raises the <see cref="EmployerSelected"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected void OnEmployerSelected(KeyChangedEventArgs e)
        {
            if (EmployerSelected != null)
                EmployerSelected(this, e);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!DesignMode)
            {
                _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                _employmentRepository = RepositoryFactory.GetRepository<IEmploymentRepository>();

                // bind the collections to the drop down lists
                _ddlBusinessType.DataSource = _lookupRepository.EmployerBusinessTypes.BindableDictionary;
                _ddlBusinessType.DataBind();
                // Created new lookup repository for active employment sectors only
                _ddlEmploymentSector.DataSource = _lookupRepository.EmploymentSectorsActive;
                _ddlEmploymentSector.DataBind();
                string editMode = Page.Request.Form[_hidEditMode.UniqueID];
                if (!String.IsNullOrEmpty(editMode))
                    EditMode = (EmployerDetailsEditMode)Int32.Parse(editMode);
            }

        }

        /// <summary>
        /// Overridden.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!DesignMode)
            {
                bool readOnly;

                // if employer is null then create a blank one so we don't get null exceptions
                if (_employer == null)
                    _employer = _employmentRepository.GetEmptyEmployer();

                readOnly = (EditMode == EmployerDetailsEditMode.ReadOnly || EditMode == EmployerDetailsEditMode.EditDetails);
                if (!readOnly)
                {
                    if (_employer.Key > 0)
                        _acEmployerName.SelectedValue = _employer.Key.ToString();
                    ((SAHLCommonBaseView)Page).RegisterWebService(ServiceConstants.Employment);
                }

                AddRow("Employer Name", _hidEmployerName.Value, _txtEmployerName, readOnly);
                _acEmployerName.Visible = (EditMode == EmployerDetailsEditMode.EditName);

                // reset the readonly mode for the rest of the controls
                readOnly = (EditMode == EmployerDetailsEditMode.ReadOnly || EditMode == EmployerDetailsEditMode.EditName);
                AddRow("Business Type", (_employer.EmployerBusinessType == null ? "" : _employer.EmployerBusinessType.Description), _ddlBusinessType, readOnly);
                AddRow("Employment Sector", (_employer.EmploymentSector == null ? "" : _employer.EmploymentSector.Description), _ddlEmploymentSector, readOnly);
                AddRow("Contact Person", _employer.ContactPerson, _txtContactPerson, readOnly);
                AddRow("Contact Phone No.", StringUtils.JoinNullableStrings(_employer.TelephoneCode, "-", _employer.TelephoneNumber), _phnContactPersonPhone, readOnly);
                AddRow("Email Address", _employer.ContactEmail, _txtContactEmail, readOnly);
                AddRow("Accountant", _employer.AccountantName, _txtAccountant, readOnly);
                AddRow("Accountant Contact", _employer.AccountantContactPerson, _txtAccountantContact, readOnly);
                AddRow("Accountant Phone No.", StringUtils.JoinNullableStrings(_employer.AccountantTelephoneCode, "-", _employer.AccountantTelephoneNumber), _phnAccountantNumber, readOnly);
                AddRow("Accountant Email", _employer.AccountantEmail, _txtAccountantEmail, readOnly);

                _hidEditMode.Value = ((int)EditMode).ToString();

                _pnlMain.Height = this.Height;
            }
        }

        #endregion

        #region Event Handlers

        void _acEmployerName_ItemSelected(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            _hidEmployerName.Value = "";
            OnEmployerSelected(e);
        }

        #endregion

    }
}
