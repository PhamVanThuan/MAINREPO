using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Controls;
using SAHL.Common.Globals;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Security;
using SAHL.Web.Controls.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Security;

namespace SAHL.Web.Views.Common
{
    public partial class Employment : SAHLCommonBaseView, IEmploymentView
    {

        private IEmploymentRepository _employmentRepository;

        private ILookupRepository _lookupRepository;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;

            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            grdEmployment.SelectedIndexChanged += new EventHandler(grdEmployment_SelectedIndexChanged);
            btnSubsidyDetails.Click += new EventHandler(btnSubsidyDetails_Click);
            btnExtended.Click += new EventHandler(btnExtended_Click);
            pnlGrid.Authenticate +=new SAHLSecurityControlEventHandler(pnlGrid_Authenticate);
        }

        protected void pnlGrid_Authenticate(object source, SAHLSecurityControlEventArgs e)
        {
            if (!SecurityHelper.CheckSecurity(pnlGrid.SecurityTag, this))
            {
                e.Cancel = false;
                EmploymentDetails.ConfirmedEmploymentReadOnly = true;
                EmploymentDetails.ConfirmedIncomeReadOnly = true;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            string javascript = @"function SetText(textVal)
                                {
                                    var btn = document.getElementById('" + btnSave.ClientID + @"') 
                                    btn.value = textVal;
                                }";

            Page.ClientScript.RegisterClientScriptBlock(typeof(Employment), "EmploymentView", javascript, true);

            #region Ticket 9974 - Removed Code

            //StringBuilder sb = new StringBuilder();
            //sb.Append("\nfunction EmploymentDetails_onValuesChanged(inputValues)");
            //sb.Append("\n{");
            //sb.Append("\n   var origVal = '" + btnSave.Text + "';");
            //sb.Append("\n   var btnSave = document.getElementById('" + btnSave.ClientID + "');");
            //sb.Append("\n   var showNext = false;");
            //sb.Append("\n   if (inputValues.employmentTypeKey == " + ((int)EmploymentTypes.Subsidised).ToString() + ")");
            //sb.Append("\n       showNext = true;");
            //sb.Append("\n   else if ((inputValues.employmentTypeKey == " + ((int)EmploymentTypes.Salaried).ToString() + ") && ");
            //sb.Append("\n           (inputValues.remunerationTypeKey == " + ((int)RemunerationTypes.Salaried).ToString() + " || inputValues.remunerationTypeKey == " + ((int)RemunerationTypes.BasicAndCommission).ToString() + "))");
            //sb.Append("\n       showNext = true;");
            //sb.Append("\n   btnSave.value = (showNext ? 'Next' : origVal);");
            //sb.Append("\n   EmploymentDetails_setIncomeFieldsEnabled(!showNext)");
            //sb.Append("\n}");
            //// add javascript for the employment type changed event
            //Page.ClientScript.RegisterClientScriptBlock(typeof(Employment), "EmploymentView", sb.ToString(), true);

            #endregion
        }

        #region IEmploymentView Members

        /// <summary>
        /// Implements <see cref="IEmploymentView.BindEmploymeRemunerationTypentDetails"/>
        /// </summary>
        /// <param name="employmentDetails"></param>
        /// <param name="showPrevious">Whether to show previous employment details.</param>
        public void BindEmploymentDetails(SAHL.Common.Collections.Interfaces.IEventList<SAHL.Common.BusinessModel.Interfaces.IEmployment> employmentDetails, bool showPrevious)
        {
            grdEmployment.BindEmploymentList(employmentDetails, showPrevious);
        }

        /// <summary>
        /// Implements <see cref="IEmploymentView.GetCapturedEmployment()"/>
        /// </summary>
        public IEmployment GetCapturedEmployment()
        {
            IEmploymentType empType = EmploymentDetails.EmploymentType;

            // if the employment type has not been selected, return null
            if (empType == null || empType.Key == (int)EmploymentTypes.Unknown)
                return null;

            // create a blank employment object, and use that
            IEmployment employment = EmploymentRepository.GetEmptyEmploymentByType(empType);
            return GetCapturedEmployment(employment);
        }

        /// <summary>
        /// Gets a populated employment record from the view.  This is not necessarily an object from the 
        /// database - it is purely a populated <see cref="IEmployment"/> entity whose values reflect the 
        /// values of the input boxes on the view.  If an employer has been selected, it will be added to 
        /// the new entity.
        /// </summary>
        public IEmployment GetCapturedEmployment(IEmployment employment)
        {
            // Employment Details cannot be changed if it is being set to Previous
            if (EmploymentDetails.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
            {
                if (EmploymentDetails.BasicIncome.HasValue)
                    employment.BasicIncome = EmploymentDetails.BasicIncome.Value;

                if (EmploymentDetails.ConfirmedBasicIncome.HasValue)
                    employment.ConfirmedBasicIncome = EmploymentDetails.ConfirmedBasicIncome.Value;

                if (EmploymentDetails.ConfirmedEmployment.HasValue)
                    employment.ConfirmedEmploymentFlag = EmploymentDetails.ConfirmedEmployment.Value;
                else if (EmploymentDetails.ConfirmedEmploymentReadOnly == false)
                    employment.ConfirmedEmploymentFlag = null;

                if (EmploymentDetails.ConfirmedIncome.HasValue)
                    employment.ConfirmedIncomeFlag = EmploymentDetails.ConfirmedIncome.Value;
                else if (EmploymentDetails.ConfirmedIncomeReadOnly == false)
                    employment.ConfirmedIncomeFlag = null;

                if (EmployerDetails.EmployerKey.HasValue && EmployerDetails.EmployerKey.Value > 0)
                {
                    IEmployer employer = EmploymentRepository.GetEmployerByKey(EmployerDetails.EmployerKey.Value);
                    employment.Employer = employer;
                }

                employment.RemunerationType = EmploymentDetails.RemunerationType;
            }

            if (EmploymentDetails.StartDate.HasValue)
                employment.EmploymentStartDate = EmploymentDetails.StartDate.Value;
            else
                employment.EmploymentStartDate = null;

            if (EmploymentDetails.EmploymentStatus != null)
                employment.EmploymentStatus = EmploymentDetails.EmploymentStatus;

            if (EmploymentDetails.EndDate.HasValue)
                employment.EmploymentEndDate = EmploymentDetails.EndDate;
            else
                employment.EmploymentEndDate = null;

            return employment;
        }

        private IEmploymentRepository EmploymentRepository
        {
            get
            {
                if (_employmentRepository == null)
                    _employmentRepository = RepositoryFactory.GetRepository<IEmploymentRepository>();
                return _employmentRepository;
            }
        }

        private ILookupRepository LookupRepository
        {
            get
            {
                if (_lookupRepository == null)
                    _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                return _lookupRepository;
            }
        }

        /// <summary>
        /// Implements <see cref="IEmploymentView.SelectedEmployment"/>
        /// </summary>
        public IEmployment SelectedEmployment
        {
            get
            {
                if (grdEmployment.PostBackType == GridPostBackType.None) return null;
                return grdEmployment.SelectedEmployment;
            }
            set
            {
                grdEmployment.SelectedEmployment = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IEmploymentView.EmploymentDetails"/>
        /// </summary>
        public IEmploymentDetails EmploymentDetails
        {
            get
            {
                return pnlEmploymentDetails;
            }
        }

        /// <summary>.EmployerDetails"/>
        /// </summary>
        public IEmployerDetails EmployerDetails
        {
            get
            {
                return pnlEmployerDetails;
            }
        }

        /// <summary>
        /// Implements <see cref="IEmploymentView.CancelButtonVisible"/>
        /// </summary>
        public bool CancelButtonVisible
        {
            set
            {
                btnCancel.Visible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IEmploymentView.ExtendedDetailsButtonVisible"/>
        /// </summary>
        public bool ExtendedDetailsButtonVisible
        {
            set
            {
                btnExtended.Visible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IEmploymentView.SubsidyDetailsButtonVisible"/>
        /// </summary>
        public bool SubsidyDetailsButtonVisible
        {
            set
            {
                btnSubsidyDetails.Visible = value;
            }
        }

        public bool GridPostBack
        {
            set
            {
                if (value)
                    grdEmployment.PostBackType = GridPostBackType.SingleClick;
                else
                    grdEmployment.PostBackType = GridPostBackType.None;
            }
        }

        /// <summary>
        /// Gets/sets whether the legal entity column on the grid is visible.  This defaults to false.
        /// </summary>
        public bool GridColumnLegalEntityVisible
        {
            get
            {
                return grdEmployment.ColumnLegalEntityVisible;
            }
            set
            {
                grdEmployment.ColumnLegalEntityVisible = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the start date column on the grid is visible.  This defaults to true.
        /// </summary>
        public bool GridColumnStartDateVisible
        {
            get
            {
                return grdEmployment.ColumnStartDateVisible;
            }
            set
            {
                grdEmployment.ColumnStartDateVisible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IEmploymentView.SaveButtonVisible"/>
        /// </summary>
        public bool SaveButtonVisible
        {
            set
            {
                btnSave.Visible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IEmploymentView.SaveButtonText"/>
        /// </summary>
        public string SaveButtonText
        {
            get
            {
                return btnSave.Text;
            }
            set
            {
                btnSave.Text = value;
            }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Common.Interfaces.IEmploymentView.SaveButtonClicked">IEmploymentView.SaveButtonClicked</see>.
        /// </summary>
        public event EventHandler SaveButtonClicked;

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Common.Interfaces.IEmploymentView.CancelButtonClicked">IEmploymentView.CancelButtonClicked</see>.
        /// </summary>
        public event EventHandler CancelButtonClicked;

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Common.Interfaces.IEmploymentView.EmploymentSelected">IEmploymentView.EmploymentSelected</see>.
        /// </summary>
        public event EventHandler EmploymentSelected;
        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Common.Interfaces.IEmploymentView.SubsidyDetailsClicked">IEmploymentView.SubsidyDetailsClicked</see>.
        /// </summary>
        public event EventHandler SubsidyDetailsClicked;
        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Common.Interfaces.IEmploymentView.ExtendedDetailsClicked">IEmploymentView.ExtendedDetailsClicked</see>.
        /// </summary>
        public event EventHandler ExtendedDetailsClicked;

        #endregion

        #region Event handlers

        void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveButtonClicked != null)
                SaveButtonClicked(sender, e);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelButtonClicked != null)
                CancelButtonClicked(sender, e);
        }

        protected void btnSubsidyDetails_Click(object sender, EventArgs e)
        {
            if (SubsidyDetailsClicked != null)
                SubsidyDetailsClicked(sender, new KeyChangedEventArgs(grdEmployment.SelectedEmployment.Key));
        }

        void btnExtended_Click(object sender, EventArgs e)
        {
            if (ExtendedDetailsClicked != null)
                ExtendedDetailsClicked(sender, new KeyChangedEventArgs(grdEmployment.SelectedEmployment.Key));

        }

        void grdEmployment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EmploymentSelected != null)
                EmploymentSelected(sender, e);
        }

        #endregion
     
    }
}