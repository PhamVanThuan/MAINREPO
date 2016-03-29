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
using SAHL.Web.Controls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration
{
    /// <summary>
    /// View used to display, add, edit and delete employer details.
    /// </summary>
    public partial class Employer : SAHLCommonBaseView, SAHL.Web.Views.Administration.Interfaces.IEmployer
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnEmployerSelected(object sender, KeyChangedEventArgs e)
        {
            if (EmployerSelected != null)
                EmployerSelected(sender, e);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            employerDetails.EmployerSelected += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(employerDetails_EmployerSelected);

            // add event handlers
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnClearForm.Click += new EventHandler(btnClearForm_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
        }

        void btnClearForm_Click(object sender, EventArgs e)
        {
            if (ClearFormButtonClicked != null)
                ClearFormButtonClicked(sender, e);
        }

        void employerDetails_EmployerSelected(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            OnEmployerSelected(sender, e);
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            if (UpdateButtonClicked != null)
                UpdateButtonClicked(sender, e);
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            if (AddButtonClicked != null)
                AddButtonClicked(sender, e);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelButtonClicked != null)
                CancelButtonClicked(sender, e);
        }

        #region IEmployer Members

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEmployer.EmployerSelected">IEmployer.EmployerSelected</see>.
        /// </summary>
        public event KeyChangedEventHandler EmployerSelected;

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEmployer.EditMode">IEmployer.EditMode</see>.
        /// </summary>
        public EmployerDetailsEditMode EditMode
        {
            get
            {
                return employerDetails.EditMode;
            }
            set
            {
                employerDetails.EditMode = value;
            }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEmployer.AddButtonVisible">IEmployer.AddButtonVisible</see>.
        /// </summary>
        public bool AddButtonVisible
        {
            set { btnAdd.Visible = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEmployer.CancelButtonVisible">IEmployer.CancelButtonVisible</see>.
        /// </summary>
        public bool CancelButtonVisible
        {
            set { btnCancel.Visible = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEmployer.ClearFormButtonEnabled">IEmployer.ClearFormButtonEnabled</see>.
        /// </summary>
        public bool ClearFormButtonEnabled
        {
            set { btnClearForm.Enabled = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEmployer.ClearFormButtonVisible">IEmployer.ClearFormButtonVisible</see>.
        /// </summary>
        public bool ClearFormButtonVisible
        {
            set { btnClearForm.Visible = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEmployer.UpdateButtonEnabled">IEmployer.UpdateButtonEnabled</see>.
        /// </summary>
        public bool UpdateButtonEnabled
        {
            set { btnUpdate.Enabled = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEmployer.UpdateButtonVisible">IEmployer.UpdateButtonVisible</see>.
        /// </summary>
        public bool UpdateButtonVisible
        {
            set { btnUpdate.Visible = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEmployer.AddButtonClicked">IEmployer.AddButtonClicked</see>.
        /// </summary>
        public event EventHandler AddButtonClicked;

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEmployer.CancelButtonClicked">IEmployer.CancelButtonClicked</see>.
        /// </summary>
        public event EventHandler CancelButtonClicked;

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEmployer.ClearFormButtonClicked">IEmployer.ClearFormButtonClicked</see>.
        /// </summary>
        public event EventHandler ClearFormButtonClicked;

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEmployer.UpdateButtonClicked">IEmployer.UpdateButtonClicked</see>.
        /// </summary>
        public event EventHandler UpdateButtonClicked;


        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEmployer.SelectedEmployer">IEmployer.SelectedEmployer</see>.
        /// </summary>
        public IEmployer SelectedEmployer
        {
            get
            {
                return employerDetails.Employer;
            }
            set
            {
                employerDetails.Employer = value;
            }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IEmployer.ClearEmployer">IEmployer.ClearEmployer</see>.
        /// </summary>
        public void ClearEmployer()
        {
            employerDetails.ClearEmployer();
        }

        #endregion
    }
}
