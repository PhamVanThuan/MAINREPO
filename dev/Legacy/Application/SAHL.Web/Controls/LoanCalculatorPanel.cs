using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.AJAX;
using SAHL.Web.Controls.Interfaces;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Generate and display the Loan Calculator.  
    /// <para>
    /// <list type="bullet">
    ///     <listheader>
    ///         <description>Notes on usage</description>
    ///     </listheader>
    ///     <item>
    ///         <description>
    ///         The purpose of this component is to provide a single source for the Loan Calculator
    ///         </description>
    ///     </item>
    /// </list>
    /// </para>
    /// </summary>
    public class LoanCalculatorPanel : Panel, ILoanCalculatorPanel
    {

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

        private string _confirmedBy;

        private Panel _pnlMain;
        private SAHLDropDownList _ddlEmploymentType;
        private SAHLDropDownList _ddlEmploymentStatus;
        private CascadingDropDown _ccdRemunerationType;
        private SAHLDropDownList _ddlRemunerationType;
        private SAHLDateBox _dtStartDate;
        private SAHLDateBox _dtEndDate;
        private SAHLCurrencyBox _currBasicIncome;
        private SAHLCurrencyBox _currConfirmedIncome;
        private SAHLTextBox _txtContactPerson;
        private SAHLPhone _phnContactNumber;
        private SAHLTextBox _txtDepartment;
        private HtmlInputHidden _hidEmploymentKey;
        private HtmlInputHidden _hidRemunerationTypeKey;

        /// <summary>
        /// Constructor.  Sets default values for the control.
        /// </summary>
        public LoanCalculatorPanel()
        {
         //   GroupingText = "Employment Details";
            if (DesignMode) return;

            // create internal panel so we can set the height
            _pnlMain = new Panel();
            _pnlMain.ID = "pnlMain";
           // Controls.Add(_pnlMain);

            // create repositories and set default values
            _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

            _ddlEmploymentType = new SAHLDropDownList();
            _ddlEmploymentType.ID = "ddlEmploymentType";
            _ddlEmploymentType.PleaseSelectItem = true;
            _pnlMain.Controls.Add(_ddlEmploymentType);

            _ddlEmploymentStatus = new SAHLDropDownList();
            _ddlEmploymentStatus.ID = "ddlEmploymentStatus";
            _ddlEmploymentStatus.PleaseSelectItem = false;
            _pnlMain.Controls.Add(_ddlEmploymentStatus);

            _ddlRemunerationType = new SAHLDropDownList();
            _ddlRemunerationType.ID = "ddlRemunerationType";
            _ddlRemunerationType.PleaseSelectItem = true;
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
            _pnlMain.Controls.Add(_dtStartDate);

            _dtEndDate = new SAHLDateBox();
            _dtEndDate.ID = "dtEndDate";
            _pnlMain.Controls.Add(_dtEndDate);

            _currBasicIncome = new SAHLCurrencyBox();
            _currBasicIncome.ID = "currMonthlyIncome";
            _pnlMain.Controls.Add(_currBasicIncome);

            _currConfirmedIncome = new SAHLCurrencyBox();
            _currConfirmedIncome.ID = "currConfirmedIncome";
            _pnlMain.Controls.Add(_currConfirmedIncome);

            _txtContactPerson = new SAHLTextBox();
            _txtContactPerson.ID = "txtContactPerson";
            _txtContactPerson.Columns = 15;
            _txtContactPerson.MaxLength = 50;
            _pnlMain.Controls.Add(_txtContactPerson);

            _phnContactNumber = new SAHLPhone();
            _phnContactNumber.ID = "phnContactNumber";
            _pnlMain.Controls.Add(_phnContactNumber);

            _txtDepartment = new SAHLTextBox();
            _txtDepartment.ID = "txtDepartment";
            _txtDepartment.Columns = 15;
            _txtDepartment.MaxLength = 50;
            _pnlMain.Controls.Add(_txtDepartment);

            _hidEmploymentKey = new HtmlInputHidden();
            _hidEmploymentKey.ID = "hidEmploymentKey";
            _pnlMain.Controls.Add(_hidEmploymentKey);

            _hidRemunerationTypeKey = new HtmlInputHidden();
            _hidRemunerationTypeKey.ID = "hidRemunerationTypeKey";
            _pnlMain.Controls.Add(_hidRemunerationTypeKey);


        }

        #region Properties

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
                return _currConfirmedIncome.Amount;
            }
            set
            {
                _currConfirmedIncome.Amount = value;
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
        /// Gets/sets the contact person displayed.
        /// </summary>
        public string ContactPerson
        {
            get
            {
                return _txtContactPerson.Text;
            }
            set
            {
                _txtContactPerson.Text = value;
            }
        }
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
        public string ContactPhoneCode
        {
            get
            {
                return _phnContactNumber.Code;
            }
            set
            {
                _phnContactNumber.Code = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Contact Number value.
        /// </summary>
        public string ContactPhoneNumber
        {
            get
            {
                return _phnContactNumber.Number;
            }
            set
            {
                _phnContactNumber.Number = value;
            }
        }

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
        public string Department
        {
            get
            {
                return _txtDepartment.Text;
            }
            set
            {
                _txtDepartment.Text = value;
            }
        }

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
                return null;
            }
            set
            {
                if (value == null)
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
                if (_lookupRepository.RemunerationTypes.ObjectDictionary.ContainsKey(_hidRemunerationTypeKey.Value))
                    return _lookupRepository.RemunerationTypes.ObjectDictionary[_hidRemunerationTypeKey.Value];
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





        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (DesignMode) return;

            // employment types - but do NOT include unknown
            _ddlEmploymentType.DataSource = _lookupRepository.EmploymentTypes.BindableDictionary;
            _ddlEmploymentType.DataBind();
            _ddlEmploymentType.Items.Remove(_ddlEmploymentType.Items.FindByValue(((int)EmploymentTypes.Unknown).ToString()));

            // employment status
            _ddlEmploymentStatus.DataSource = _lookupRepository.EmploymentStatuses.BindableDictionary;
            _ddlEmploymentStatus.DataBind();

            // remuneration types
            _ddlRemunerationType.DataSource = _lookupRepository.RemunerationTypes.BindableDictionary;
            _ddlRemunerationType.DataBind();

        }

        /// <summary>
        /// Overridden.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (DesignMode) return;

            // add JavaScript events - when dropdown values are raised these should fire events that can be handled by views using the control
            string javascript =
                @"function EmploymentDetails_valuesChanged()
                {
                    var inputValues = {
                        employmentTypeKey: document.getElementById('" + _ddlEmploymentType.ClientID + @"').value,
                        remunerationTypeKey: document.getElementById('" + _ddlRemunerationType.ClientID + @"').value
                    };

                    if (window.EmploymentDetails_onValuesChanged)
                        EmploymentDetails_onValuesChanged(inputValues);
                }";

            const string jskey = "EmploymentDetailsJavaScript";
            if (!Page.ClientScript.IsClientScriptBlockRegistered(jskey))
                Page.ClientScript.RegisterClientScriptBlock(typeof(EmploymentDetails), jskey, javascript, true);

            // add values changed events to drop-down boxes
            _ddlEmploymentType.Attributes["onchange"] = "EmploymentDetails_valuesChanged();" + _ddlEmploymentType.Attributes["onchange"];
            _ddlRemunerationType.Attributes["onchange"] = "EmploymentDetails_valuesChanged();" + _ddlRemunerationType.Attributes["onchange"];

            // make the height of the inner panel the same as this - this is necessary otherwise the frameset doesn't take up the full height
            // of the control if it doesn't have to
            _pnlMain.Height = Height;

        }
        #endregion

       //void RegisterClientScripts()
       // {
       //     System.Text.StringBuilder mBuilder = new System.Text.StringBuilder();
			
       //         mBuilder.AppendLine("function checkInputs(chkSendTo, chkFax, chkEmail, chkAlternate)");
       //         mBuilder.AppendLine("{");
       //         mBuilder.AppendLine("    ret = false;");
       //         mBuilder.AppendLine("");    
       //         mBuilder.AppendLine("    if(chkSendTo.checked)");
       //         mBuilder.AppendLine("    {");
       //         mBuilder.AppendLine("        if (chkSendTo == chkFax)");
       //         mBuilder.AppendLine("        {");
       //         mBuilder.AppendLine("            var tbFax = document.getElementById('<%=tbFax.ClientID%>');");
       //         mBuilder.AppendLine("        if (tbFax.firstChild.value.length > 0 && tbFax.lastChild.value.length > 0)");
       //         mBuilder.AppendLine("            ret = true; ");
       //         mBuilder.AppendLine("    }");
       //         mBuilder.AppendLine("    ");
       //         mBuilder.AppendLine("    if (chkSendTo == chkEmail)");
       //         mBuilder.AppendLine("    {");
       //         mBuilder.AppendLine("        var tbEmail = document.getElementById('<%=tbEmail.ClientID%>');");
       //         mBuilder.AppendLine("        if (tbEmail.value.length > 0)");
       //         mBuilder.AppendLine("            ret = true;");
       //         mBuilder.AppendLine("    }");
       //         mBuilder.AppendLine("    ");
       //         mBuilder.AppendLine("    if (chkSendTo == chkAlternate)");
       //         mBuilder.AppendLine("    {");
       //         mBuilder.AppendLine("        var tbFax = document.getElementById('<%=tbAlternateFax.ClientID%>');");
       //         mBuilder.AppendLine("        if (tbFax.firstChild.value.length > 0 && tbFax.lastChild.value.length > 0)");
       //         mBuilder.AppendLine("            ret = true; ");
       //         mBuilder.AppendLine("        ");
       //         mBuilder.AppendLine("        var tbEmail = document.getElementById('<%=tbAlternateEmail.ClientID%>');");
       //         mBuilder.AppendLine("        if (tbEmail.value.length > 0)");
       //         mBuilder.AppendLine("            ret = true;");
       //         mBuilder.AppendLine("    }");
       //         mBuilder.AppendLine("}");
       //         mBuilder.AppendLine("");
       //         mBuilder.AppendLine("return ret;");
       //         mBuilder.AppendLine("}");
       //         mBuilder.AppendLine("");
       //         mBuilder.AppendLine("");
       //         mBuilder.AppendLine("function tabsMenu_ClientActiveTabChanged(sender, e) ");
       //         mBuilder.AppendLine("{ ");
       //         mBuilder.AppendLine("    __doPostBack('<%=tabsMenu.ClientID%>', sender.get_activeTabIndex()); ");
       //         mBuilder.AppendLine("}");
       //         mBuilder.AppendLine("");
       //         mBuilder.AppendLine("function checkContactInfo()");
       //         mBuilder.AppendLine("{");
       //         mBuilder.AppendLine("    // onKeyUp for the send to contact inputs check to see if the data is valid and we can continue (enable btnNext)");
       //         mBuilder.AppendLine("    var chkSendToFax = document.getElementById('<%=chkSendToFax.ClientID%>');");
       //         mBuilder.AppendLine("    var chkSendToEmail = document.getElementById('<%=chkSendToEmail.ClientID%>');");
       //         mBuilder.AppendLine("    var chkAlternate = document.getElementById('<%=chkAlternateContact.ClientID%>');"); 
       //         mBuilder.AppendLine("    ");
       //         mBuilder.AppendLine("    if (chkSendToFax.checked)");
       //         mBuilder.AppendLine("        sendToOnClick('<%=chkSendToFax.ClientID%>');");
       //         mBuilder.AppendLine("        ");
       //         mBuilder.AppendLine("    if (chkSendToEmail.checked)");
       //         mBuilder.AppendLine("        sendToOnClick('<%=chkSendToEmail.ClientID%>');");
       //         mBuilder.AppendLine("        ");
       //         mBuilder.AppendLine("     if (chkAlternate.checked)");
       //         mBuilder.AppendLine("        sendToOnClick('<%=chkAlternateContact.ClientID%>');");
       //         mBuilder.AppendLine("}");
       //         mBuilder.AppendLine("");
       //         mBuilder.AppendLine("function disableNext()");
       //         mBuilder.AppendLine("{");
       //         mBuilder.AppendLine("    var btnNext = document.getElementById('<%=btnNext.ClientID%>');");
       //         mBuilder.AppendLine("    var tabsMenu = $get('<%=tabsMenu.ClientID%>').control;");
       //         mBuilder.AppendLine("");
       //         mBuilder.AppendLine("    SAHLButton_setEnabled('<%=btnNext.ClientID%>', false);");
       //         mBuilder.AppendLine("    tabsMenu.get_tabs()[1].set_enabled(false);");
       //         mBuilder.AppendLine("    tabsMenu.get_tabs()[2].set_enabled(false);");
       //         mBuilder.AppendLine("    tabsMenu.get_tabs()[3].set_enabled(false);");
       //         mBuilder.AppendLine("}");
       //         mBuilder.AppendLine("");
       //         mBuilder.AppendLine("function copy()");
       //         mBuilder.AppendLine("{");
       //         mBuilder.AppendLine("    var tbTotal = document.getElementById('<%=tbTotalCashRequired.ClientID%>');");
       //         mBuilder.AppendLine("    var lTotal = document.getElementById('<%=lTotalCashRequired.ClientID%>');");
       //         mBuilder.AppendLine("    ");
       //         mBuilder.AppendLine("    lTotal.innerText = 'R ' + String(tbTotal.value);");
       //         mBuilder.AppendLine("}");
       //         mBuilder.AppendLine("");
       //         mBuilder.AppendLine("function editCheckChange(ctrl)");
       //         mBuilder.AppendLine("{");
       //         mBuilder.AppendLine("    var cbTotal = document.getElementById('<%=cbTotal.ClientID%>');");
       //         mBuilder.AppendLine("    var cbType = document.getElementById('<%=cbType.ClientID%>');");
       //         mBuilder.AppendLine("    ");
       //         mBuilder.AppendLine("    var tbTotal = document.getElementById('<%=tbTotalCashRequired.ClientID%>');");
       //         mBuilder.AppendLine("    var lTotal = document.getElementById('<%=lTotalCashRequired.ClientID%>');");
       //         mBuilder.AppendLine("    var tbReadvance = document.getElementById('<%=tbReadvanceRequired.ClientID%>');");
       //         mBuilder.AppendLine("    var lReadvance = document.getElementById('<%=lReadvanceRequired.ClientID%>');");
       //         mBuilder.AppendLine("    var tbFadvance = document.getElementById('<%=tbFurtherAdvReq.ClientID%>');");
       //         mBuilder.AppendLine("    var lFadvance = document.getElementById('<%=lFurtherAdvReq.ClientID%>');");
       //         mBuilder.AppendLine("    var tbFloan = document.getElementById('<%=tbFurtherLoanReq.ClientID%>');");
       //         mBuilder.AppendLine("    var lFloan = document.getElementById('<%=lFurtherLoanReq.ClientID%>');");
       //         mBuilder.AppendLine("    ");
       //         mBuilder.AppendLine("    cbTotal.checked = false;");
       //         mBuilder.AppendLine("    cbType.checked = false;");
       //         mBuilder.AppendLine("    ");
       //         mBuilder.AppendLine("    ctrl.checked = true;");
       //         mBuilder.AppendLine("    ");
       //         mBuilder.AppendLine("    if (ctrl == cbType)");
       //         mBuilder.AppendLine("    {");
       //         mBuilder.AppendLine("        tbTotal.style.display = 'none';");
       //         mBuilder.AppendLine("        lTotal.style.display = 'inline';");
       //         mBuilder.AppendLine("        ");
       //         mBuilder.AppendLine("        tbReadvance.style.display = 'inline';");
       //         mBuilder.AppendLine("        lReadvance.style.display = 'none';");
       //         mBuilder.AppendLine("        tbFadvance.style.display = 'inline';");
       //         mBuilder.AppendLine("        lFadvance.style.display = 'none';");
       //         mBuilder.AppendLine("        tbFloan.style.display = 'inline';");
       //         mBuilder.AppendLine("        lFloan.style.display = 'none';");
       //         mBuilder.AppendLine("    }");
       //         mBuilder.AppendLine("    else");
       //         mBuilder.AppendLine("    {");
       //         mBuilder.AppendLine("        tbTotal.style.display = 'inline';");
       //         mBuilder.AppendLine("        lTotal.style.display = 'none';");
       //         mBuilder.AppendLine("        ");
       //         mBuilder.AppendLine("        tbReadvance.style.display = 'none';");
       //         mBuilder.AppendLine("        lReadvance.style.display = 'inline';");
       //         mBuilder.AppendLine("        tbFadvance.style.display = 'none';");
       //         mBuilder.AppendLine("        lFadvance.style.display = 'inline';");
       //         mBuilder.AppendLine("        tbFloan.style.display = 'none';");
       //         mBuilder.AppendLine("        lFloan.style.display = 'inline';");
       //         mBuilder.AppendLine("    }");
       //         mBuilder.AppendLine("    ");
       //         mBuilder.AppendLine("    sumTotal();");
       //         mBuilder.AppendLine("}");
       //         mBuilder.AppendLine("");
       //         mBuilder.AppendLine("function sumTotal()");
       //         mBuilder.AppendLine("{");
       //         mBuilder.AppendLine("    var tbTotal = document.getElementById('<%=tbTotalCashRequired.ClientID%>');");
       //         mBuilder.AppendLine("    var lTotal = document.getElementById('<%=lTotalCashRequired.ClientID%>');");
       //         mBuilder.AppendLine("    var tbReadvance = document.getElementById('<%=tbReadvanceRequired.ClientID%>');");
       //         mBuilder.AppendLine("    var lReadvance = document.getElementById('<%=lReadvanceRequired.ClientID%>');");
       //         mBuilder.AppendLine("    var tbFadvance = document.getElementById('<%=tbFurtherAdvReq.ClientID%>');");
       //         mBuilder.AppendLine("    var lFadvance = document.getElementById('<%=lFurtherAdvReq.ClientID%>');");
       //         mBuilder.AppendLine("    var tbFloan = document.getElementById('<%=tbFurtherLoanReq.ClientID%>');");
       //         mBuilder.AppendLine("    var lFloan = document.getElementById('<%=lFurtherLoanReq.ClientID%>');");
       //         mBuilder.AppendLine("");
       //         mBuilder.AppendLine("    var retval = 0;");
       //         mBuilder.AppendLine("    ");
       //         mBuilder.AppendLine("    if (!isNaN(eval(tbReadvance.value)))");
       //         mBuilder.AppendLine("    {");
       //         mBuilder.AppendLine("        retval += eval(tbReadvance.value);");
       //         mBuilder.AppendLine("        lReadvance.innerText = 'R ' + String(tbReadvance.value);");
       //         mBuilder.AppendLine("    }");
       //         mBuilder.AppendLine("        ");
       //         mBuilder.AppendLine("    if (!isNaN(eval(tbFadvance.value)))");
       //         mBuilder.AppendLine("    {");
       //         mBuilder.AppendLine("        retval += eval(tbFadvance.value);");
       //         mBuilder.AppendLine("        lFadvance.innerText = 'R ' + String(tbFadvance.value);");
       //         mBuilder.AppendLine("    }");
       //         mBuilder.AppendLine("        ");
       //         mBuilder.AppendLine("    if (!isNaN(eval(tbFloan.value)))");
       //         mBuilder.AppendLine("    {");
       //         mBuilder.AppendLine("        retval += eval(tbFloan.value);");
       //         mBuilder.AppendLine("        lFloan.innerText = 'R ' + String(tbFloan.value);");
       //         mBuilder.AppendLine("    }");
       //         mBuilder.AppendLine("    ");
       //         mBuilder.AppendLine("    tbTotal.value = retval;");
       //         mBuilder.AppendLine("    lTotal.innerText = 'R ' + String(retval);");
       //         mBuilder.AppendLine("}");

       //     if (!Page.ClientScript.IsClientScriptBlockRegistered(GetType(), "lstScripts"))
       //         Page.ClientScript.RegisterClientScriptBlock(GetType(), "lstScripts", mBuilder.ToString(), true);
       // }
    }
}
