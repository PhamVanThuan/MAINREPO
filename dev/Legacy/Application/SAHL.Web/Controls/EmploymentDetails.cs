using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Utils;
using SAHL.Common.Globals;
using AjaxControlToolkit;
using SAHL.Web.AJAX;
using SAHL.Web.Controls.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Display employment details on a panel.  
    /// <para>
    /// <list type="bullet">
    ///     <listheader>
    ///         <description>Notes on usage</description>
    ///     </listheader>
    ///     <item>
    ///         <description>
    ///         This control needs to be populated with an employment record on every page load.  
    ///         It will, however, retain values in input fields on postback.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         When the employment type changes, some client-side activity occurs.  If you need to hook 
    ///         into this, add a javascript method called EmploymentDetails_onEmpTypeChanged(element) to 
    ///         your page - the element value will be populated with the dropdown list containing the 
    ///         employment types.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         The control also exposes the JavaScript method EmploymentDetails_setIncomeFieldsEnabled(enabled).  This 
    ///         can be used to toggle whether the monthly and confirmed income fields are enabled from the client.
    ///         </description>
    ///     </item>
    /// </list>
    /// </para>This control needs to be supplied with an employment 
    /// record on every page load
    /// </summary>
    public class EmploymentDetails : DetailsPanel, IEmploymentDetails
    {
        private const string _confirmedEmploymentHint = "Telephonic confirmation has been done";
        private const string _confirmedIncomeHint = "Verification of Income Documents has been done";

        private ILookupRepository _lookupRepository;
        private ILegalEntity _legalEntity;

        private bool _employmentTypeReadOnly = true;
        private bool _employmentStatusReadOnly = true;
        private bool _remunerationTypeReadOnly = true;
        private bool _startDateReadOnly = true;
        private bool _endDateReadOnly = true;
        private bool _basicIncomeReadOnly = true;
        private bool _confirmedBasicIncomeReadOnly = true;
        private bool _contactPersonReadOnly = true;
        private bool _contactPhoneNumberReadOnly = true;
        private bool _departmentReadOnly = true;
        private bool _confirmedEmploymentReadOnly = true;
        private bool _confirmedIncomeReadOnly = true;

        private string _confirmedBy;
        private DateTime? _confirmedDate = new DateTime?();

        private Panel _pnlMain;
        private SAHLDropDownList _ddlEmploymentType;
        private SAHLDropDownList _ddlEmploymentStatus;
        private CascadingDropDown _ccdRemunerationType;
        private SAHLDropDownList _ddlRemunerationType;
        private SAHLDateBox _dtStartDate;
        private SAHLDateBox _dtEndDate;
        private SAHLCurrencyBox _currBasicIncome;
        private SAHLCurrencyBox _currConfirmedIncome;
        // private SAHLTextBox _txtContactPerson;
        // private SAHLPhone _phnContactNumber;
        // private SAHLTextBox _txtDepartment;
        private HtmlInputHidden _hidEmploymentKey;
        private HtmlInputHidden _hidRemunerationTypeKey;
        private SAHLDropDownList _ddlConfirmedEmployment;
        private SAHLDropDownList _ddlConfirmedIncome;

        // public event SAHLSecurityControlEventHandler ConfirmIncomeAuthenticate;

        /// <summary>
        /// Constructor.  Sets default values for the control.
        /// </summary>
        public EmploymentDetails()
        {
            GroupingText = "Employment Details";
            if (DesignMode) return;

            // create internal panel so we can set the height
            _pnlMain = new Panel();
            _pnlMain.ID = "pnlMain";
            this.Controls.Add(_pnlMain);

            // create repositories and set default values
            _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

            _ddlEmploymentType = new SAHLDropDownList();
            _ddlEmploymentType.ID = "ddlEmploymentType";
            _ddlEmploymentType.PleaseSelectItem = true;
            _ddlEmploymentType.Mandatory = true;
            _ddlEmploymentType.AutoPostBack = true;
            _pnlMain.Controls.Add(_ddlEmploymentType);

            _ddlEmploymentStatus = new SAHLDropDownList();
            _ddlEmploymentStatus.ID = "ddlEmploymentStatus";
            _ddlEmploymentStatus.PleaseSelectItem = false;
            _ddlEmploymentStatus.AutoPostBack = true;
            _pnlMain.Controls.Add(_ddlEmploymentStatus);

            _ddlRemunerationType = new SAHLDropDownList();
            _ddlRemunerationType.ID = "ddlRemunerationType";
            _ddlRemunerationType.PleaseSelectItem = true;
            _ddlRemunerationType.Mandatory = true;
            _ddlRemunerationType.AutoPostBack = true;
            _pnlMain.Controls.Add(_ddlRemunerationType);

            _ccdRemunerationType = new CascadingDropDown();
            _ccdRemunerationType.ID = "ccdRemunerationType";
            _ccdRemunerationType.TargetControlID = _ddlRemunerationType.UniqueID;
            _ccdRemunerationType.Category = "RemunerationType";
            _ccdRemunerationType.PromptText = SAHLDropDownList.PleaseSelectText;
            _ccdRemunerationType.PromptValue = SAHLDropDownList.PleaseSelectValue;
            _ccdRemunerationType.ServicePath = ServiceConstants.Employment;
            _ccdRemunerationType.ServiceMethod = "GetRemunerationTypesByEmploymentType";
            _ccdRemunerationType.ParentControlID = _ddlEmploymentType.UniqueID;
            _ccdRemunerationType.EmptyValue = SAHLDropDownList.PleaseSelectValue;
            _ccdRemunerationType.EmptyText = SAHLDropDownList.PleaseSelectText;
            _ccdRemunerationType.LoadingText = SAHLDropDownList.LoadingText;
            _pnlMain.Controls.Add(_ccdRemunerationType);

            _dtStartDate = new SAHLDateBox();
            _dtStartDate.ID = "dtStartDate";
            _dtStartDate.Mandatory = true;
            _pnlMain.Controls.Add(_dtStartDate);

            _dtEndDate = new SAHLDateBox();
            _dtEndDate.ID = "dtEndDate";
            _pnlMain.Controls.Add(_dtEndDate);

            _currBasicIncome = new SAHLCurrencyBox();
            _currBasicIncome.ID = "currMonthlyIncome";
            _pnlMain.Controls.Add(_currBasicIncome);

            _currConfirmedIncome = new SAHLCurrencyBox();
            _currConfirmedIncome.ID = "currConfirmedIncome";
            _currConfirmedIncome.SecurityDisplayType = SAHLSecurityDisplayType.Disable;
            _currConfirmedIncome.SecurityTag = "EmploymentConfirmIncomeOnly";
            _pnlMain.Controls.Add(_currConfirmedIncome);

            //_txtContactPerson = new SAHLTextBox();
            //_txtContactPerson.ID = "txtContactPerson";
            //_txtContactPerson.Columns = 15;
            //_txtContactPerson.MaxLength = 50;
            //_pnlMain.Controls.Add(_txtContactPerson);

            //_phnContactNumber = new SAHLPhone();
            //_phnContactNumber.ID = "phnContactNumber";
            //_pnlMain.Controls.Add(_phnContactNumber);

            //_txtDepartment = new SAHLTextBox();
            //_txtDepartment.ID = "txtDepartment";
            //_txtDepartment.Columns = 15;
            //_txtDepartment.MaxLength = 50;
            //_pnlMain.Controls.Add(_txtDepartment);

            _hidEmploymentKey = new HtmlInputHidden();
            _hidEmploymentKey.ID = "hidEmploymentKey";
            _pnlMain.Controls.Add(_hidEmploymentKey);

            _hidRemunerationTypeKey = new HtmlInputHidden();
            _hidRemunerationTypeKey.ID = "hidRemunerationTypeKey";
            _pnlMain.Controls.Add(_hidRemunerationTypeKey);

            _ddlConfirmedEmployment = new SAHLDropDownList();
            _ddlConfirmedEmployment.ID = "ddlConfirmedEmployment";
            _ddlConfirmedEmployment.PleaseSelectItem = true;
            _ddlConfirmedEmployment.Mandatory = true;
            _ddlConfirmedEmployment.AutoPostBack = true;
            _pnlMain.Controls.Add(_ddlConfirmedEmployment);

            _ddlConfirmedIncome = new SAHLDropDownList();
            _ddlConfirmedIncome.ID = "ddlConfirmedIncome";
            _ddlConfirmedIncome.PleaseSelectItem = true;
            _ddlConfirmedIncome.Mandatory = true;
            _ddlConfirmedIncome.AutoPostBack = true;
            _pnlMain.Controls.Add(_ddlConfirmedIncome);

        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool? ConfirmedEmployment
        {
            set 
            {
                if (value.HasValue)
                    _ddlConfirmedEmployment.SelectedValue = Convert.ToInt32(value.Value).ToString();
                else
                    _ddlConfirmedEmployment.SelectedIndex = -1;
            }
            get
            {
                if (_ddlConfirmedEmployment.SelectedValue != null && _ddlConfirmedEmployment.SelectedValue != "-select-")
                    return Convert.ToBoolean(Convert.ToInt32(_ddlConfirmedEmployment.SelectedValue));
                else
                    return new bool?();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool? ConfirmedIncome
        {
            set 
            { 
                if (value.HasValue)
                    _ddlConfirmedIncome.SelectedValue = Convert.ToInt32(value.Value).ToString();
                else
                    _ddlConfirmedIncome.SelectedIndex = -1;
            }
            get
            {
                if (_ddlConfirmedIncome.SelectedValue != null && _ddlConfirmedIncome.SelectedValue != "-select-")
                    return Convert.ToBoolean(Convert.ToInt32(_ddlConfirmedIncome.SelectedValue));
                else
                    return new bool?();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ConfirmedEmploymentText
        {
            get
            {
                if (ConfirmedEmployment.HasValue)
                    return Enum.GetName(typeof(SAHL.Common.Globals.ConfirmedEmployment), Convert.ToInt32(ConfirmedEmployment.Value)).ToString();
                else
                    return "-";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ConfirmedIncomeText
        {
            get
            {
                if (ConfirmedIncome.HasValue)
                    return Enum.GetName(typeof(SAHL.Common.Globals.ConfirmedIncome), Convert.ToInt32(ConfirmedIncome.Value)).ToString();
                else
                    return "-";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ConfirmedEmploymentReadOnly
        {
            set { _confirmedEmploymentReadOnly = value; }
            get { return _confirmedEmploymentReadOnly; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ConfirmedIncomeReadOnly
        {
            set { _confirmedIncomeReadOnly = value; }
            get { return _confirmedIncomeReadOnly; }
        }

        /// <summary>
        /// Gets/sets the name of the person in PreCredit who confirmed the employment details.
        /// </summary>
        public string ConfirmedBy
        {
            get
            {
                return _confirmedBy;
            }
            set
            {
                _confirmedBy = value;
            }
        }

        /// <summary>
        /// Gets/sets the confirmed income amount displayed.
        /// </summary>
        public double? ConfirmedBasicIncome
        {
            get
            {
                return _currConfirmedIncome.Amount; ;
            }
            set
            {
                _currConfirmedIncome.Amount = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Confirmed Income field is enabled.
        /// </summary>
        public bool ConfirmedBasicIncomeEnabled
        {
            get
            {
                return _currConfirmedIncome.Enabled;
            }
            set
            {
                _currConfirmedIncome.Enabled = value;
            }
        }

        /// <summary>
        /// Gets/sets the security tag on the confirmed income field.
        /// </summary>
        public string ConfirmedBasicIncomeSecurityTag
        {
            get
            {
                return _currConfirmedIncome.SecurityTag;
            }
            set
            {
                _currConfirmedIncome.SecurityTag = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Confirmed Income field can be edited.
        /// </summary>
        public bool ConfirmedBasicIncomeReadOnly
        {
            get
            {
                return _confirmedBasicIncomeReadOnly;
            }
            set
            {
                _confirmedBasicIncomeReadOnly = value;
            }
        }

        /// <summary>
        /// Gets/sets the date the employment record was confirmed.
        /// </summary>
        public DateTime? ConfirmedDate
        {
            get
            {
                return _confirmedDate;
            }
            set
            {
                _confirmedDate = value;
                if (_confirmedDate == null)
                    _confirmedDate = new DateTime?();
            }
        }

        /// <summary>
        /// Gets/sets the contact person displayed.
        /// </summary>
        //public string ContactPerson
        //{
        //    get
        //    {
        //        return _txtContactPerson.Text;
        //    }
        //    set
        //    {
        //        _txtContactPerson.Text = value;
        //    }
        //}
        /// <summary>
        /// Gets/sets whether the Contact Person field can be edited.
        /// </summary>
        public bool ContactPersonReadOnly
        {
            get
            {
                return _contactPersonReadOnly;
            }
            set
            {
                _contactPersonReadOnly = value;
            }
        }


        /// <summary>
        /// Gets/sets whether the Contact Code field can be edited.
        /// </summary>
        //public string ContactPhoneCode
        //{
        //    get
        //    {
        //        return _phnContactNumber.Code;
        //    }
        //    set
        //    {
        //        _phnContactNumber.Code = value;
        //    }
        //}

        /// <summary>
        /// Gets/sets whether the Contact Number value.
        /// </summary>
        //public string ContactPhoneNumber
        //{
        //    get
        //    {
        //        return _phnContactNumber.Number;
        //    }
        //    set
        //    {
        //        _phnContactNumber.Number = value;
        //    }
        //}

        /// <summary>
        /// Gets/sets whether the Contact Number field can be edited.
        /// </summary>
        public bool ContactPhoneNumberReadOnly
        {
            get
            {
                return _contactPhoneNumberReadOnly;
            }
            set
            {
                _contactPhoneNumberReadOnly = value;
            }
        }

        /// <summary>
        /// Gets/sets the value in the Department field.
        /// </summary>
        //public string Department
        //{
        //    get
        //    {
        //        return _txtDepartment.Text;
        //    }
        //    set
        //    {
        //        _txtDepartment.Text = value;
        //    }
        //}

        /// <summary>
        /// Gets/sets whether the Department field can be edited.
        /// </summary>
        public bool DepartmentReadOnly
        {
            get
            {
                return _departmentReadOnly;
            }
            set
            {
                _departmentReadOnly = value;
            }
        }

        /// <summary>
        /// Gets/sets the employment status key selected on the control.
        /// </summary>
        public int? EmploymentStatusKey
        {
            get
            {
                string key = _ddlEmploymentStatus.SelectedValue;
                int result;
                if (Int32.TryParse(key, out result))
                    return result;
                else
                    return new int?();
            }
        }

        /// <summary>
        /// Gets/sets the employment status.
        /// </summary>
        public IEmploymentStatus EmploymentStatus
        {
            get
            {
                return _lookupRepository.EmploymentStatuses.ObjectDictionary[_ddlEmploymentStatus.SelectedValue];
            }
            set
            {
                _ddlEmploymentStatus.SelectedValue = value.Key.ToString();
            }
        }

        /// <summary>
        /// Gets/sets whether the Status field can be edited.
        /// </summary>
        public bool EmploymentStatusReadOnly
        {
            get
            {
                return _employmentStatusReadOnly;
            }
            set
            {
                _employmentStatusReadOnly = value;
            }
        }

        /// <summary>
        /// Gets/sets the employment type key selected on the control.
        /// </summary>
        public int? EmploymentTypeKey
        {
            get
            {
                string key = _ddlEmploymentType.SelectedValue;
                int result;
                if (Int32.TryParse(key, out result))
                    return result;
                else
                    return new int?();
            }
        }

        /// <summary>
        /// Gets/sets the employment type.
        /// </summary>
        public IEmploymentType EmploymentType
        {
            get
            {
                if (_lookupRepository.EmploymentTypes.ObjectDictionary.ContainsKey(_ddlEmploymentType.SelectedValue))
                    return _lookupRepository.EmploymentTypes.ObjectDictionary[_ddlEmploymentType.SelectedValue];
                else
                    return null;
            }
            set
            {
                if (value == null || value.Key == (int)EmploymentTypes.Unknown)
                    _ddlEmploymentType.ClearSelection();
                else
                    _ddlEmploymentType.SelectedValue = value.Key.ToString();
            }
        }

        /// <summary>
        /// Gets/sets whether the Employment Type field can be edited.
        /// </summary>
        public bool EmploymentTypeReadOnly
        {
            get
            {
                return _employmentTypeReadOnly;
            }
            set
            {
                _employmentTypeReadOnly = value;
            }
        }

        /// <summary>
        /// Gets/sets the value in the End Date field.
        /// </summary>
        public DateTime? EndDate
        {
            get
            {
                return _dtEndDate.Date;
            }
            set
            {
                _dtEndDate.Date = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the End Date field can be edited.
        /// </summary>
        public bool EndDateReadOnly
        {
            get
            {
                return _endDateReadOnly;
            }
            set
            {
                _endDateReadOnly = value;
            }
        }

        /// <summary>
        /// Gets/sets the legal entity applicable to the control.  This affects the remuneration types displayed 
        /// in the Remuneration Type drop down box.  Leaving this as null will result in all possible 
        /// remuneration types being shown.
        /// </summary>
        public ILegalEntity LegalEntity
        {
            get
            {
                return _legalEntity;
            }
            set
            {
                _legalEntity = value;
            }
        }

        /// <summary>
        /// Gets/sets the value in the Monthly Income field.
        /// </summary>
        public double? BasicIncome
        {
            get
            {
                return _currBasicIncome.Amount;
            }
            set
            {
                _currBasicIncome.Amount = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Monthly Income field is enabled.
        /// </summary>
        public bool BasicIncomeEnabled
        {
            get
            {
                return _currBasicIncome.Enabled;
            }
            set
            {
                _currBasicIncome.Enabled = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Monthly Income field can be edited.
        /// </summary>
        public bool BasicIncomeReadOnly
        {
            get
            {
                return _basicIncomeReadOnly;
            }
            set
            {
                _basicIncomeReadOnly = value;
            }
        }

        /// <summary>
        /// Gets/sets the remuneration type.
        /// </summary>
        public IRemunerationType RemunerationType
        {
            get
            {
                if (_lookupRepository.RemunerationTypes.ObjectDictionary.ContainsKey(_ddlRemunerationType.SelectedValue))
                    return _lookupRepository.RemunerationTypes.ObjectDictionary[_ddlRemunerationType.SelectedValue];
                else if (_lookupRepository.RemunerationTypes.ObjectDictionary.ContainsKey(_hidRemunerationTypeKey.Value))
                    return _lookupRepository.RemunerationTypes.ObjectDictionary[_hidRemunerationTypeKey.Value];
                else
                    return null;
            }
            set
            {
                if (value == null)
                {
                    _ccdRemunerationType.SelectedValue = "";
                    _hidRemunerationTypeKey.Value = "";
                }
                else
                {
                    _ccdRemunerationType.SelectedValue = value.Key.ToString();
                    _hidRemunerationTypeKey.Value = value.Key.ToString();
                }
            }

        }

        /// <summary>
        /// Gets/sets whether the Remuneration Type field can be edited.
        /// </summary>
        public bool RemunerationTypeReadOnly
        {
            get
            {
                return _remunerationTypeReadOnly;
            }
            set
            {
                _remunerationTypeReadOnly = value;
            }
        }

        /// <summary>
        /// Gets/sets the value in the Start Date field.
        /// </summary>
        public DateTime? StartDate
        {
            get
            {
                return _dtStartDate.Date;
            }
            set
            {
                _dtStartDate.Date = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Start Date field can be edited.
        /// </summary>
        public bool StartDateReadOnly
        {
            get
            {
                return _startDateReadOnly;
            }
            set
            {
                _startDateReadOnly = value;
            }
        }

        #endregion

        #region Methods

        private void AddRow(string title, string displayValue, WebControl c, bool readOnly, string hint)
        {
            if (readOnly)
                base.AddRow(_pnlMain, title, displayValue);
            else
            {
                base.AddRow(_pnlMain, title, c, hint);
            }

            c.Visible = !readOnly;

        }

        private void AddRow(string title, string displayValue, WebControl c, bool readOnly)
        {
            if (readOnly)
                base.AddRow(_pnlMain, title, displayValue);
            else
            {
                base.AddRow(_pnlMain, title, c);
            }

            c.Visible = !readOnly;

        }

        private void HideRemunerationOption(RemunerationTypes remunType)
        {
            string key = ((int)remunType).ToString();
            ListItem li = _ddlRemunerationType.Items.FindByValue(key);
            if (li != null)
                _ddlRemunerationType.Items.Remove(li);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Set DataSource to Controls
            BindDropDownListControls();
        }

        /// <summary>
        /// Overridden.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (DesignMode) return;

            // add the rows to the display
            AddRow("Employment Type", (EmploymentType == null ? "Unknown" : EmploymentType.Description), _ddlEmploymentType, EmploymentTypeReadOnly);
            if (EmploymentTypeReadOnly)
            {
                _ddlEmploymentType.Visible = true;
                _ddlEmploymentType.Style.Add(HtmlTextWriterStyle.Display, "none");
            }

            AddRow("Status", EmploymentStatus.Description, _ddlEmploymentStatus, EmploymentStatusReadOnly);

            AddRow("Remuneration Type", (RemunerationType == null ? "" : RemunerationType.Description), _ddlRemunerationType, RemunerationTypeReadOnly);
            // if a legal entity is set, we need to get rid of options that are not applicable
            if (_legalEntity != null)
            {
                if (_legalEntity is ILegalEntityNaturalPerson)
                {
                    HideRemunerationOption(RemunerationTypes.BusinessProfits);
                }
            }

            AddRow("Start Date", (StartDate.HasValue ? StartDate.Value.ToString(SAHL.Common.Constants.DateFormat) : ""), _dtStartDate, StartDateReadOnly);
            AddRow("End Date", (EndDate.HasValue ? EndDate.Value.ToString(SAHL.Common.Constants.DateFormat) : ""), _dtEndDate, EndDateReadOnly);
            AddRow("Monthly Income", (BasicIncome.HasValue ? BasicIncome.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : ""), _currBasicIncome, BasicIncomeReadOnly);
            AddRow("Confirmed Income", (ConfirmedBasicIncome.HasValue ? ConfirmedBasicIncome.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : ""), _currConfirmedIncome, ConfirmedBasicIncomeReadOnly);
            AddRow(_pnlMain, "Confirmed By", ConfirmedBy);
            AddRow(_pnlMain, "Confirmed Date", (ConfirmedDate.HasValue ? ConfirmedDate.Value.ToString(SAHL.Common.Constants.DateFormat) : ""));
            AddRow("Confirmed Employment", ConfirmedEmploymentText, _ddlConfirmedEmployment, ConfirmedEmploymentReadOnly, _confirmedEmploymentHint);
            AddRow("Confirmed Income", ConfirmedIncomeText, _ddlConfirmedIncome, ConfirmedIncomeReadOnly, _confirmedIncomeHint);

//            string javascript = @"function ConfirmedIncome_Validate()
//                                    {
//                                        var txtConfIncome = SAHLCurrencyBox_getValue('" + _currConfirmedIncome.ClientID + @"');
//                                        if (txtConfIncome > 0)
//                                            SetText(""Next"");
//                                        else
//                                            SetText(""Update"");
//                                    }";

//            _currConfirmedIncome.Attributes.Add("onblur", "ConfirmedIncome_Validate();");
//            _currConfirmedIncome.Attributes.Add("OnValueChanged", "ConfirmedIncome_Validate();");

            #region Ticket 9974 - Removed Code
            /*
            AddRow("Contact Person", ContactPerson, _txtContactPerson, ContactPersonReadOnly);
            AddRow("Phone Number", StringUtils.JoinNullableStrings(ContactPhoneCode, "-", ContactPhoneNumber), _phnContactNumber, ContactPhoneNumberReadOnly);
            AddRow("Department", Department, _txtDepartment, DepartmentReadOnly);
            */

            /* 
             * These JavaScript Methods will be replaced by Events, 
             * as there is alot of business logic that needs to take place 
             */
            // add JavaScript events - when dropdown values are raised these should fire events that can be handled by views using the control
            //            string javascript = 
            //                @"function EmploymentDetails_valuesChanged()
            //                {
            //                    var inputValues = {
            //                        employmentTypeKey: document.getElementById('" + _ddlEmploymentType.ClientID + @"').value,
            //                        remunerationTypeKey: document.getElementById('" + _ddlRemunerationType.ClientID + @"').value
            //                    };
            //
            //                    if (window.EmploymentDetails_onValuesChanged)
            //                        EmploymentDetails_onValuesChanged(inputValues);
            //                }
            //
            //
            //                function EmploymentDetails_setIncomeFieldsEnabled(enabled)
            //                {
            //                    var txtBasicIncome = document.getElementById('" + _currBasicIncome.ClientID + @"');
            //                    var txtConfIncome = document.getElementById('" + _currConfirmedIncome.ClientID + @"');
            //                    
            //                    EmploymentDetails_toggleCurrencyField(txtBasicIncome, enabled);
            //                    EmploymentDetails_toggleCurrencyField(txtConfIncome, enabled);
            //                }
            //
            //                function EmploymentDetails_toggleCurrencyField(currencyControl, enabled)
            //                {
            //                    if (currencyControl == null) return;
            //                    SAHLCurrencyBox_setEnabled(currencyControl.id, enabled);
            //                    if (!enabled) SAHLCurrencyBox_setValue(currencyControl.id, '');
            //                }
            //                ";

            //string jskey = "EmploymentDetailsJavaScript";
            //if (!Page.ClientScript.IsClientScriptBlockRegistered(jskey))
            //    Page.ClientScript.RegisterClientScriptBlock(typeof(EmploymentDetails), jskey, javascript, true);

            // add values changed events to drop-down boxes
            //_ddlEmploymentType.Attributes["onchange"] = "EmploymentDetails_valuesChanged();" + _ddlEmploymentType.Attributes["onchange"];
            //_ddlRemunerationType.Attributes["onchange"] = "EmploymentDetails_valuesChanged();" + _ddlRemunerationType.Attributes["onchange"];
            #endregion

            // make the height of the inner panel the same as this - this is necessary otherwise the frameset doesn't take up the full height
            // of the control if it doesn't have to
            _pnlMain.Height = Height;
        }

        public void BindDropDownListControls()
        {
            // employment types - but do NOT include unknown
            _ddlEmploymentType.DataSource = _lookupRepository.EmploymentTypes.BindableDictionary;
            _ddlEmploymentType.DataBind();
            //_ddlEmploymentType.Items.Remove(_ddlEmploymentType.Items.FindByValue(((int)EmploymentTypes.Unknown).ToString()));

            // employment status
            _ddlEmploymentStatus.DataSource = _lookupRepository.EmploymentStatuses.BindableDictionary;
            _ddlEmploymentStatus.DataBind();

            // remuneration types
            _ddlRemunerationType.DataSource = _lookupRepository.RemunerationTypes.BindableDictionary;
            _ddlRemunerationType.DataBind();

            // ConfirmedEmployment Answers
            Dictionary<string, string> dictCE = new Dictionary<string, string>();
            dictCE.Add(((int)SAHL.Common.Globals.ConfirmedEmployment.No).ToString(), SAHL.Common.Globals.ConfirmedEmployment.No.ToString());
            dictCE.Add(((int)SAHL.Common.Globals.ConfirmedEmployment.Yes).ToString(), SAHL.Common.Globals.ConfirmedEmployment.Yes.ToString());
            _ddlConfirmedEmployment.DataSource = dictCE;
            _ddlConfirmedEmployment.DataBind();

            // ConfirmedIncome Answers
            Dictionary<string, string> dictCI = new Dictionary<string, string>();
            dictCI.Add(((int)SAHL.Common.Globals.ConfirmedIncome.No).ToString(), SAHL.Common.Globals.ConfirmedIncome.No.ToString());
            dictCI.Add(((int)SAHL.Common.Globals.ConfirmedIncome.Yes).ToString(), SAHL.Common.Globals.ConfirmedIncome.Yes.ToString());
            _ddlConfirmedIncome.DataSource = dictCI;
            _ddlConfirmedIncome.DataBind();
        }

        #endregion

        #region Event Handlers

        /*
        public event KeyChangedEventHandler ConfirmedIncomeSelectedIndexChanged;
        public event KeyChangedEventHandler ConfirmedEmploymentSelectedIndexChanged;
        public event KeyChangedEventHandler EmploymentTypeSelectedIndexChanged;
        public event KeyChangedEventHandler RemunerationTypeSelectedIndexChanged;

        protected void _ddlConfirmedIncome_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_confirmedIncomeReadOnly && ConfirmedIncomeSelectedIndexChanged != null && _ddlConfirmedIncome.SelectedValue != "-select-")
            {
                ConfirmedIncomeSelectedIndexChanged(sender, new KeyChangedEventArgs(_ddlConfirmedIncome.SelectedValue));
            }
        }

        protected void _ddlConfirmedEmployment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_confirmedEmploymentReadOnly && ConfirmedEmploymentSelectedIndexChanged != null && _ddlConfirmedEmployment.SelectedValue != "-select-")
            {
                ConfirmedEmploymentSelectedIndexChanged(sender, new KeyChangedEventArgs(_ddlConfirmedEmployment.SelectedValue));
            }
        }

        protected void _ddlEmploymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_employmentTypeReadOnly && EmploymentTypeSelectedIndexChanged != null && _ddlEmploymentType.SelectedValue != "-select-")
            {
                EmploymentTypeSelectedIndexChanged(sender, new KeyChangedEventArgs(_ddlEmploymentType.SelectedValue));
            }
        }

        protected void _ddlRemunerationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_remunerationTypeReadOnly && RemunerationTypeSelectedIndexChanged != null && _ddlRemunerationType.SelectedValue != "-select-")
            {
                RemunerationTypeSelectedIndexChanged(sender, new KeyChangedEventArgs(_ddlRemunerationType.SelectedValue));
            }
        }
        */


        //void _currConfirmedIncome_Authenticate(object source, SAHLSecurityControlEventArgs e)
        //{
        //    if (ConfirmIncomeAuthenticate != null)
        //        ConfirmIncomeAuthenticate(_currConfirmedIncome, e);
        //}

       #endregion
    }
}
