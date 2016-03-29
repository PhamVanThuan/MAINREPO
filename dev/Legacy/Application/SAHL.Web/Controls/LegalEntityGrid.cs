using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Utils;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Grid used to display LegalEntities.
    /// </summary>
    ///

    public class LegalEntityGrid : SAHLGridView
    {
        #region Private Attributes

        private ILegalEntity _legalEntity;
        private DateTime? _dateOfAcceptance;
        private int? _accountKey;
        private int? _applicationKey;
        private IControlRepository _controlRepository;
        private ILifeRepository _lifeRepository;

        #endregion Private Attributes

        #region Private Attributes and Enumerations

        /// <summary>
        /// Defines all columns used in the <see cref="LegalEntityGrid"/>.
        /// </summary>
        public enum GridColumns
        {
            Key = 0,
            Select,
            Name,
            IDPassport,
            Role,
            RoleStatus,
            LegalEntityStatus,
            Gender,
            IntroductionDate,
            AccountNumber,
            ApplicationKey,
            Product,
            LifeInsurableInterest,
            DateOfBirth,
            AgeNextBirthday,
            LifeBenefits,
            LifeStatus,
            FaxCode,
            FaxNumber,
            EmailAddress,
            CheckBoxFax,
            CheckBoxEmail
        }

        /// <summary>
        ///
        /// </summary>
        public enum GridColumnIDPassportHeadingType
        {
            IDNumber = 0,
            PassportNumber,
            CompanyNumber,
            IDAndPassportNumber,
            IDAndCompanyNumber
        }

        #endregion Private Attributes and Enumerations

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public LegalEntityGrid()
        {
            AutoGenerateColumns = false;
            FixedHeader = false;
            EnableViewState = false;
            HeaderCaption = "Legal Entities";
            EmptyDataSetMessage = "No data found";
            NullDataSetMessage = EmptyDataSetMessage;
            EmptyDataText = EmptyDataSetMessage;
            PostBackType = GridPostBackType.SingleClick;
            RowStyle.CssClass = "TableRowA";
            GridWidth = Unit.Percentage(100);
            Width = Unit.Percentage(100);
            GridHeight = Unit.Pixel(150);

            // add the columns to the div
            if (!DesignMode)
            {
                this.AddGridBoundColumn("Key", "Key", Unit.Empty, HorizontalAlign.Left, false);
                this.AddCheckBoxColumn("chkSelect", "", true, Unit.Percentage(1), HorizontalAlign.Center, true);
                this.AddGridBoundColumn("", "Name", Unit.Pixel(150), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("", "ID/Passport", Unit.Pixel(150), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("", "Role", Unit.Pixel(150), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("", "Status", Unit.Pixel(100), HorizontalAlign.Left, true); // role status
                this.AddGridBoundColumn("", "Status", Unit.Pixel(100), HorizontalAlign.Left, true); // legal entity status
                this.AddGridBoundColumn("", "Gender", Unit.Pixel(100), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("", "Introduction Date", Unit.Pixel(100), HorizontalAlign.Center, true);
                this.AddGridBoundColumn("", "Account Number", Unit.Pixel(100), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("", "Application Key", Unit.Pixel(100), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("", "Product", Unit.Pixel(150), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("", "Insurable Interest", Unit.Pixel(150), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("", "Date Of Birth", Unit.Pixel(100), HorizontalAlign.Center, true);
                this.AddGridBoundColumn("", "Next Age", Unit.Pixel(50), HorizontalAlign.Center, true);
                this.AddGridBoundColumn("", "Benefits", Unit.Pixel(150), HorizontalAlign.Left, true);
                this.AddGridBoundColumn("", "Status", Unit.Pixel(100), HorizontalAlign.Left, true); // life status (role + legalentity status)
                this.AddGridBoundColumn("FaxCode", "FaxCode", Unit.Empty, HorizontalAlign.Left, true);
                this.AddGridBoundColumn("FaxNumber", "FaxNumber", Unit.Empty, HorizontalAlign.Left, true);
                this.AddGridBoundColumn("EmailAddress", "EmailAddress", Unit.Empty, HorizontalAlign.Left, true);
                this.AddCheckBoxColumn("Fax", "Fax", false, new Unit("50 px"), HorizontalAlign.Left, true);
                this.AddCheckBoxColumn("Email", "Email", false, new Unit("50 px"), HorizontalAlign.Left, true);
            }

            // Set all the columns to false by default
            for (int i = 1; i < this.Columns.Count; i++)
            {
                if (i != 2)
                    this.Columns[i].Visible = false;
            }
        }

        #endregion Constructors

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        #region Methods

        /// <summary>
        /// Binds a collection of <see cref="ILegalEntity"/> entities to the grid.
        /// </summary>
        /// <param name="legalEntities">IList&lt;ILegalEntity&gt;</param>
        ///
        public void BindLegalEntities(IList<ILegalEntity> legalEntities)
        {
            if ((_accountKey == null || _accountKey == -1) && (_applicationKey == null || _applicationKey == -1))
            {
                throw new Exception("LegalEntityGrid : AccountKey or ApplicationKey must be populated");
            }

            DataSource = legalEntities;
            DataBind();
        }

        /// <summary>
        /// Binds a collection of <see cref="ILegalEntity"/> entities to the grid.
        /// </summary>
        /// <param name="legalEntities">IEventList&lt;ILegalEntity&gt;</param>
        ///
        public void BindLegalEntities(IEventList<ILegalEntity> legalEntities)
        {
            if ((_accountKey == null || _accountKey == -1) && (_applicationKey == null || _applicationKey == -1))
            {
                throw new Exception("LegalEntityGrid : AccountKey or ApplicationKey must be populated");
            }

            DataSource = legalEntities;
            DataBind();
        }

        /// <summary>
        /// Binds a collection of <see cref="ILegalEntity"/> entities to the grid.
        /// </summary>
        /// <param name="legalEntities">IReadOnlyEventList&lt;ILegalEntity&gt;</param>
        ///
        public void BindLegalEntities(IReadOnlyEventList<ILegalEntity> legalEntities)
        {
            //if ((_accountKey == null || _accountKey == -1) && (_applicationKey == null || _applicationKey == -1))
            //{
            //    throw new Exception("LegalEntityGrid : AccountKey or ApplicationKey must be populated");
            //}

            DataSource = legalEntities;
            DataBind();
        }

        /// <summary>
        /// Handles the RowDataBound event of the grid
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            base.OnRowDataBound(e);

            if (DesignMode)
                return;

            if (ColumnEmailAddressVisible)
                e.Row.Cells[(int)GridColumns.EmailAddress].CssClass = "GridHiddenColumn";
            if (ColumnFaxCodeVisible)
                e.Row.Cells[(int)GridColumns.FaxCode].CssClass = "GridHiddenColumn";
            if (ColumnFaxNumberVisible)
                e.Row.Cells[(int)GridColumns.FaxNumber].CssClass = "GridHiddenColumn";

            if (e.Row.DataItem != null)
            {
                ILegalEntity le = e.Row.DataItem as ILegalEntity;
                IRole _role = null;
                IApplicationRole _applicationRole = null;
                if (_accountKey != null && _accountKey > 0)
                    _role = le.GetRole(_accountKey.Value);
                else if (_applicationKey != null && _applicationKey > 0)
                    _applicationRole = le.GetApplicationRoleClient(_applicationKey.Value);

                if (ColumnLegalEntityNameVisible)
                    e.Row.Cells[(int)GridColumns.Name].Text = le.GetLegalName(LegalNameFormat.Full);

                if (ColumnRoleVisible)
                {
                    string roleDesc = "";
                    if (_role != null)
                        roleDesc = _role.RoleType.Description;
                    else if (_applicationRole != null)
                        roleDesc = _applicationRole.ApplicationRoleType.Description;

                    e.Row.Cells[(int)GridColumns.Role].Text = roleDesc;
                }

                if (ColumnStatusVisible)
                {
                    string roleStatusDesc = "";
                    if (_role != null)
                        roleStatusDesc = _role.GeneralStatus.Description;
                    else if (_applicationRole != null)
                        roleStatusDesc = _applicationRole.GeneralStatus.Description;

                    e.Row.Cells[(int)GridColumns.RoleStatus].Text = roleStatusDesc;
                }

               
                if (ColumnLegalEntityStatusVisible)
                    e.Row.Cells[(int)GridColumns.LegalEntityStatus].Text = le.LegalEntityStatus != null ? le.LegalEntityStatus.Description : "";

                if (ColumnIntroductionDateVisible)
                    e.Row.Cells[(int)GridColumns.IntroductionDate].Text = le.IntroductionDate.ToString(SAHL.Common.Constants.DateFormat);

                if (ColumnAccountNumberVisible)
                    e.Row.Cells[(int)GridColumns.AccountNumber].Text = _accountKey.HasValue ? _accountKey.Value.ToString() : "";

                if (ColumnApplicationKeyVisible)
                    e.Row.Cells[(int)GridColumns.ApplicationKey].Text = _applicationKey.HasValue ? _applicationKey.Value.ToString() : "";

                if (ColumnProductVisible)
                {
                    string productDesc = "";
                    if (_role != null)
                        productDesc = _role.Account.Product.Description;
                    else if (_applicationRole != null)
                        productDesc = _applicationRole.Application.ApplicationType.Description;

                    e.Row.Cells[(int)GridColumns.Product].Text = productDesc; //_role == null ? "" : _role.Account.Product.Description;
                }

                if (ColumnLifeStatusVisible)
                {
                    if (_role != null)
                    {
                        e.Row.Cells[(int)GridColumns.LifeStatus].Text = _role.GeneralStatus.Description + "/" + le.LegalEntityStatus.Description;
                        if (_role.GeneralStatus.Key == Convert.ToInt32(SAHL.Common.Globals.GeneralStatuses.Inactive))
                            e.Row.Cells[(int)GridColumns.RoleStatus].BackColor = Color.Salmon;
                    }
                    else
                        e.Row.Cells[(int)GridColumns.LifeStatus].Text = "";
                }

                if (ColumnInsurableInterestVisible)
                {
                    string _insurableInterest = "None";
                    if (_accountKey != null && _accountKey.HasValue)
                    {
                        ILifeInsurableInterest lii = le.GetInsurableInterest(_accountKey.Value);
                        if (lii != null)
                            _insurableInterest = lii.LifeInsurableInterestType.Description;
                    }

                    e.Row.Cells[(int)GridColumns.LifeInsurableInterest].Text = _insurableInterest == null ? "None" : _insurableInterest;
                }

                if (e.Row.DataItem is ILegalEntityNaturalPerson)
                {
                    ILegalEntityNaturalPerson len = e.Row.DataItem as ILegalEntityNaturalPerson;

                    if (ColumnGenderVisible)
                        e.Row.Cells[(int)GridColumns.Gender].Text = len.Gender.Description;

                    if (ColumnIDPassportVisible)
                        e.Row.Cells[(int)GridColumns.IDPassport].Text = len.IDNumber == null ? (len.PassportNumber == null ? "" : len.PassportNumber) : len.IDNumber;

                    if (ColumnBenefitsVisible)
                    {
                        int lifePolicyTypeKey = -1;
                        int lifeAccountKey = -1;

                        IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                        IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                        // get the life account key
                        if (_applicationKey.HasValue)
                        {
                            IApplicationLife applife = applicationRepo.GetApplicationLifeByKey(_applicationKey.Value);
                            lifeAccountKey = applife.Account.Key;
                        }
                        else if (_accountKey.HasValue)
                        {
                            lifeAccountKey = _accountKey.Value;
                        }
                        // get the life policy / offer
                        IAccountLifePolicy accountLifePolicy = accountRepo.GetAccountByKey(lifeAccountKey) as IAccountLifePolicy;
                        lifePolicyTypeKey = accountLifePolicy.LifePolicy != null ? accountLifePolicy.LifePolicy.LifePolicyType.Key : accountLifePolicy.CurrentLifeApplication.LifePolicyType.Key;

                        if (lifePolicyTypeKey == (int)SAHL.Common.Globals.LifePolicyTypes.AccidentOnlyCover)
                        {
                            e.Row.Cells[(int)GridColumns.LifeBenefits].Text = "Accident Benefit";
                        }
                        else
                        {
                            // work out benefits based on age as at date the assured life was added to the life policy
                            DateTime? dteAddedToPolicy = LifeRepository.GetDateAssuredLifeAddedToPolicy(lifeAccountKey, len.Key);

                            int _AgeAtAcceptance = DateUtils.CalculateAgeNextBirthday(len.DateOfBirth.HasValue ? len.DateOfBirth.Value : DateTime.Now, dteAddedToPolicy.Value);
                            if (_AgeAtAcceptance > 0)
                            {
                                int ipBenefitMaxAge = Convert.ToInt32(ControlRepository.GetControlByDescription(SAHL.Common.Constants.ControlTable.Life.IPBenefitMaxAge).ControlNumeric);
                                e.Row.Cells[(int)GridColumns.LifeBenefits].Text = _AgeAtAcceptance < ipBenefitMaxAge ? "Death and IP Benefit" : "Death Benefit";
                                e.Row.Cells[(int)GridColumns.LifeBenefits].ToolTip = "Age to determine Benefits : " + _AgeAtAcceptance.ToString();
                            }
                            else
                                e.Row.Cells[(int)GridColumns.LifeBenefits].Text = "";
                        }
                    }

                    if (ColumnDateOfBirthVisible)
                        e.Row.Cells[(int)GridColumns.DateOfBirth].Text = len.DateOfBirth.HasValue ? len.DateOfBirth.Value.ToString(SAHL.Common.Constants.DateFormat) : "";

                    if (ColumnAgeNextBirthdayVisible)
                        e.Row.Cells[(int)GridColumns.AgeNextBirthday].Text = len.AgeNextBirthday.HasValue ? len.AgeNextBirthday.ToString() : "-";
                }
                else if (e.Row.DataItem is ILegalEntityCompany)
                {
                    ILegalEntityCompany lec = e.Row.DataItem as ILegalEntityCompany;

                    if (ColumnIDPassportVisible)
                        e.Row.Cells[(int)GridColumns.IDPassport].Text = lec.RegistrationNumber == null ? "" : lec.RegistrationNumber;
                }
            }
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets a reference to a control repository.
        /// </summary>
        private IControlRepository ControlRepository
        {
            get
            {
                if (_controlRepository == null)
                    _controlRepository = RepositoryFactory.GetRepository<IControlRepository>();
                return _controlRepository;
            }
        }

        /// <summary>
        /// Gets a reference to a Life repository.
        /// </summary>
        private ILifeRepository LifeRepository
        {
            get
            {
                if (_lifeRepository == null)
                    _lifeRepository = RepositoryFactory.GetRepository<ILifeRepository>();
                return _lifeRepository;
            }
        }

        /// <summary>
        /// Gets/sets whether the CheckBox select column is visible on the grid.
        /// </summary>
        public bool CheckBoxCheckBoxVisible
        {
            get
            {
                return Columns[(int)GridColumns.Select].Visible;
            }
            set
            {
                Columns[(int)GridColumns.Select].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the LegalEntity Name column is visible on the grid.
        /// </summary>
        public bool ColumnLegalEntityNameVisible
        {
            get
            {
                return Columns[(int)GridColumns.Name].Visible;
            }
        }

        /// <summary>
        /// Gets/sets whether the ID/Passport Number column is visible on the grid.
        /// </summary>
        public bool ColumnIDPassportVisible
        {
            get
            {
                return Columns[(int)GridColumns.IDPassport].Visible;
            }
            set
            {
                Columns[(int)GridColumns.IDPassport].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets the column heading for the IDPassport column
        /// </summary>
        public GridColumnIDPassportHeadingType ColumnIDPassportHeadingType
        {
            set
            {
                switch (value)
                {
                    case GridColumnIDPassportHeadingType.IDNumber:
                        Columns[(int)GridColumns.IDPassport].HeaderText = "ID Number";
                        break;
                    case GridColumnIDPassportHeadingType.PassportNumber:
                        Columns[(int)GridColumns.IDPassport].HeaderText = "Passport Number";
                        break;
                    case GridColumnIDPassportHeadingType.CompanyNumber:
                        Columns[(int)GridColumns.IDPassport].HeaderText = "Company Number";
                        break;
                    case GridColumnIDPassportHeadingType.IDAndPassportNumber:
                        Columns[(int)GridColumns.IDPassport].HeaderText = "ID/Passport";
                        break;
                    case GridColumnIDPassportHeadingType.IDAndCompanyNumber:
                        Columns[(int)GridColumns.IDPassport].HeaderText = "ID/Company No";
                        break;
                    default:
                        Columns[(int)GridColumns.IDPassport].HeaderText = "ID/Passport";
                        break;
                }
            }
        }
        /// <summary>
        /// Gets/sets whether the Role column is visible on the grid.
        /// </summary>
        public bool ColumnCheckBoxEmailVisible
        {
            get
            {
                return Columns[(int)GridColumns.CheckBoxEmail].Visible;
            }
            set
            {
                Columns[(int)GridColumns.CheckBoxEmail].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Role column is visible on the grid.
        /// </summary>
        public bool ColumnCheckboxFaxVisible
        {
            get
            {
                return Columns[(int)GridColumns.CheckBoxFax].Visible;
            }
            set
            {
                Columns[(int)GridColumns.CheckBoxFax].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the EmailAddress column is visible on the grid.
        /// </summary>
        public bool ColumnEmailAddressVisible
        {
            get
            {
                return Columns[(int)GridColumns.EmailAddress].Visible;
            }
            set
            {
                Columns[(int)GridColumns.EmailAddress].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the FaxCode column is visible on the grid.
        /// </summary>
        public bool ColumnFaxCodeVisible
        {
            get
            {
                return Columns[(int)GridColumns.FaxCode].Visible;
            }
            set
            {
                Columns[(int)GridColumns.FaxCode].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the FaxNumber column is visible on the grid.
        /// </summary>
        public bool ColumnFaxNumberVisible
        {
            get
            {
                return Columns[(int)GridColumns.FaxNumber].Visible;
            }
            set
            {
                Columns[(int)GridColumns.FaxNumber].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Role column is visible on the grid.
        /// </summary>
        public bool ColumnRoleVisible
        {
            get
            {
                return Columns[(int)GridColumns.Role].Visible;
            }
            set
            {
                Columns[(int)GridColumns.Role].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Role Status column is visible on the grid.
        /// </summary>
        public bool ColumnStatusVisible
        {
            get
            {
                return Columns[(int)GridColumns.RoleStatus].Visible;
            }
            set
            {
                Columns[(int)GridColumns.RoleStatus].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the LegalEntity Status column is visible on the grid.
        /// </summary>
        public bool ColumnLegalEntityStatusVisible
        {
            get
            {
                return Columns[(int)GridColumns.LegalEntityStatus].Visible;
            }
            set
            {
                Columns[(int)GridColumns.LegalEntityStatus].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Gender column is visible on the grid.
        /// </summary>
        public bool ColumnGenderVisible
        {
            get
            {
                return Columns[(int)GridColumns.Gender].Visible;
            }
            set
            {
                Columns[(int)GridColumns.Gender].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the IntroductionDate column is visible on the grid.
        /// </summary>
        public bool ColumnIntroductionDateVisible
        {
            get
            {
                return Columns[(int)GridColumns.IntroductionDate].Visible;
            }
            set
            {
                Columns[(int)GridColumns.IntroductionDate].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the AccountNumber column is visible on the grid.
        /// </summary>
        public bool ColumnAccountNumberVisible
        {
            get
            {
                return Columns[(int)GridColumns.AccountNumber].Visible;
            }
            set
            {
                Columns[(int)GridColumns.AccountNumber].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the ApplicationKey column is visible on the grid.
        /// </summary>
        public bool ColumnApplicationKeyVisible
        {
            get
            {
                return Columns[(int)GridColumns.ApplicationKey].Visible;
            }
            set
            {
                Columns[(int)GridColumns.ApplicationKey].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Product column is visible on the grid.
        /// </summary>
        public bool ColumnProductVisible
        {
            get
            {
                return Columns[(int)GridColumns.Product].Visible;
            }
            set
            {
                Columns[(int)GridColumns.Product].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Insurable Interest column is visible on the grid.
        /// </summary>
        public bool ColumnInsurableInterestVisible
        {
            get
            {
                return Columns[(int)GridColumns.LifeInsurableInterest].Visible;
            }
            set
            {
                Columns[(int)GridColumns.LifeInsurableInterest].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Benefits column is visible on the grid.
        /// </summary>
        public bool ColumnBenefitsVisible
        {
            get
            {
                return Columns[(int)GridColumns.LifeBenefits].Visible;
            }
            set
            {
                Columns[(int)GridColumns.LifeBenefits].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Life Status column is visible on the grid.
        /// </summary>
        public bool ColumnLifeStatusVisible
        {
            get
            {
                return Columns[(int)GridColumns.LifeStatus].Visible;
            }
            set
            {
                Columns[(int)GridColumns.LifeStatus].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the DateOfBirth column is visible on the grid.
        /// </summary>
        public bool ColumnDateOfBirthVisible
        {
            get
            {
                return Columns[(int)GridColumns.DateOfBirth].Visible;
            }
            set
            {
                Columns[(int)GridColumns.DateOfBirth].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the AgeNextBirthday column is visible on the grid.
        /// </summary>
        public bool ColumnAgeNextBirthdayVisible
        {
            get
            {
                return Columns[(int)GridColumns.AgeNextBirthday].Visible;
            }
            set
            {
                Columns[(int)GridColumns.AgeNextBirthday].Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets DateOfAcceptance of the Life Policy.
        /// This is used in determining the Benfits to display for the LegalEntity.
        /// If this property is required, it must be set before grid is DataBound
        /// </summary>
        public DateTime? DateOfAcceptance
        {
            get
            {
                return _dateOfAcceptance;
            }
            set
            {
                _dateOfAcceptance = value;
            }
        }

        /// <summary>
        /// Gets/sets AccountKey of the Life Policy to be used for Role and/or InsurableInterest.
        /// This is used in determining the Insurable Interest.
        /// If this property is required, it must be set before grid is DataBound
        /// </summary>
        public int? AccountKey
        {
            get
            {
                return _accountKey;
            }
            set
            {
                _accountKey = value;
            }
        }

        /// <summary>
        /// Gets/sets ApplicationKey.
        /// </summary>
        public int? ApplicationKey
        {
            get
            {
                return _applicationKey;
            }
            set
            {
                _applicationKey = value;
            }
        }

        /// <summary>
        /// Gets a reference to the currently selected <see cref="ILegalEntity"/>.  If no row is selected, a null is returned.
        /// </summary>
        public ILegalEntity SelectedLegalEntity
        {
            get
            {
                if (SelectedIndex > -1)
                {
                    if (Rows.Count == 0) return null;
                    _legalEntity = (ILegalEntity)Rows[SelectedIndex].DataItem;

                    int legalEntityKey = Int32.Parse(Rows[SelectedIndex].Cells[(int)GridColumns.Key].Text);
                    if (legalEntityKey > 0)
                    {
                        ILegalEntityRepository rep = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                        _legalEntity = rep.GetLegalEntityByKey(legalEntityKey);
                    }
                }
                return _legalEntity;
            }
        }

        /// <summary>
        /// Gets a reference to the list of selected <see cref="ILegalEntity"/>.  If no row/s are selected, a null is returned.
        /// </summary>
        public IList<ILegalEntity> SelectedLegalEntities
        {
            get
            {
                IList<ILegalEntity> lstLegalEntity = new List<ILegalEntity>();
                ILegalEntityRepository rep = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                for (int i = 0; i < this.Rows.Count; i++)
                {
                    CheckBox cb = (CheckBox)Rows[i].FindControl("chkSelect");
                    object o = Page.Request.Form[cb.UniqueID];
                    bool isChecked = (o == null) ? false : true;
                    if (isChecked)
                    {
                        int legalEntityKey = Int32.Parse(Rows[i].Cells[(int)GridColumns.Key].Text);
                        if (legalEntityKey > 0)
                        {
                            _legalEntity = rep.GetLegalEntityByKey(legalEntityKey);
                            lstLegalEntity.Add(_legalEntity);
                        }
                    }
                }

                return lstLegalEntity;
            }
        }

        #endregion Properties
    }
}