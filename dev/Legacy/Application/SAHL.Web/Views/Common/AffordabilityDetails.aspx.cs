using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.Common.Models.Affordability;

namespace SAHL.Web.Views.Common
{
    public partial class AffordabilityDetails : SAHLCommonBaseView, IAffordabilityDetails
    {
        #region Private Members

        private string _submitButtonText;
        private bool _showButtons;
        private bool _readOnly;
        private ILegalEntity _legalEntity;
        private string _selectedNumContributingDependants;
        private string _selectedNumDependantsInHousehold;
        private string _numberOfDependantsInHousehold;
        private string _contributingDependants;
        private IApplication _application;
        private IEnumerable<AffordabilityModel> _affordabilityModel;

        #endregion Private Members

        /// <summary>
        /// Get or Set The Application Object
        /// </summary>
        public IApplication application
        {
            set { _application = value; }
            get { return _application; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            DisableControls(IncomeListView, !_readOnly);
            DisableControls(DebtRepaymentListView, !_readOnly);
            DisableControls(MonthlyExpenseListView, !_readOnly);

            if (_readOnly)
            {
                txtDependantsInHousehold.ReadOnly = true;
                txtDependantsInHousehold.BorderStyle = BorderStyle.None;

                txtContributingDependants.ReadOnly = true;
                txtContributingDependants.BorderStyle = BorderStyle.None;
            }

            btnCancel.Visible = _showButtons;
            btnSubmit.Visible = _showButtons;
            btnSubmit.Text = _submitButtonText;
        }

        private void DisableControls(Control parent, bool state)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is SAHLCurrencyBox)
                {
                    ((SAHLCurrencyBox)(c)).Enabled = state;
                }
                else if (c is SAHLTextBox)
                {
                    ((SAHLTextBox)(c)).ReadOnly = !state;
                }


                DisableControls(c, state);
            }
        }

        protected void btnSaveUpdate_Click(object sender, EventArgs e)
        {
            _selectedNumContributingDependants = txtContributingDependants.Text;
            _selectedNumDependantsInHousehold = txtDependantsInHousehold.Text;

            UpdateAffordabilityModel(IncomeListView.Items);
            UpdateAffordabilityModel(MonthlyExpenseListView.Items);
            UpdateAffordabilityModel(DebtRepaymentListView.Items);

            if (OnSubmitButtonClicked != null)
            {
                OnSubmitButtonClicked(sender, e);
            }

        }

        private void UpdateAffordabilityModel(IList<ListViewDataItem> listViewItems)
        {
            foreach (ListViewDataItem lvItem in listViewItems)
            {
                var affordabilityTypeKey = Convert.ToInt32(((System.Web.UI.WebControls.HiddenField)(lvItem.FindControl("key"))).Value);
                var updatedAmountValue = ((SAHLCurrencyBox)lvItem.FindControl("txtAmount")).Amount.Value;
                var userDescription = ((SAHLTextBox)lvItem.FindControl("txtDescription")).Text;
                _affordabilityModel.Update(x => x.Amount = (x.Key == affordabilityTypeKey) ? updatedAmountValue : x.Amount);
                _affordabilityModel.Update(x => x.Description = ((x.Key == affordabilityTypeKey) && (x.DescriptionRequired == true)) ? userDescription : x.Description);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
            {
                OnCancelButtonClicked(sender, e);
            }
        }

        #region IAffordabilityDetails Members

        public ILegalEntity LegalEntity
        {
            set { _legalEntity = value; }
        }

        public IEnumerable<AffordabilityModel> Affordability
        {
            get { return _affordabilityModel; }
            set { _affordabilityModel = value; }
        }

        public bool ShowButtons
        {
            set { _showButtons = value; }
        }

        public bool ReadOnly
        {
            set { _readOnly = value; }
        }

        public string SubmitButtonText
        {
            set { _submitButtonText = value; }
        }

        public event EventHandler OnCancelButtonClicked;

        //public event KeyChangedEventHandler OnSubmitButtonClicked;
        public event EventHandler OnSubmitButtonClicked;

        public void BindControls()
        {
            IncomeListView.DataSource = _affordabilityModel.Where(x => x.AffordabilityTypeGroups == SAHL.Common.Globals.AffordabilityTypeGroups.INCOME).OrderBy(x=> x.Sequence).ToArray();
            IncomeListView.DataBind();

            MonthlyExpenseListView.DataSource = _affordabilityModel.Where(x => x.AffordabilityTypeGroups == SAHL.Common.Globals.AffordabilityTypeGroups.MONTHLYEXPENSE).OrderBy(x => x.Sequence).ToArray();
            MonthlyExpenseListView.DataBind();

            DebtRepaymentListView.DataSource = _affordabilityModel.Where(x => x.AffordabilityTypeGroups == SAHL.Common.Globals.AffordabilityTypeGroups.DEBTREPAYMENT).OrderBy(x => x.Sequence).ToArray();
            DebtRepaymentListView.DataBind();

            txtContributingDependants.Text = _contributingDependants;
            txtDependantsInHousehold.Text = _numberOfDependantsInHousehold;
        }

        public string NumberOfDependantsInHouseHold
        {
            set { _numberOfDependantsInHousehold = value; }
        }

        public string ContributingDependants
        {
            set { _contributingDependants = value; }
        }

        public string SelectedNumDependantsInHousehold
        {
            get { return _selectedNumDependantsInHousehold; }
        }

        public string SelectedNumContributingDependants
        {
            get { return _selectedNumContributingDependants; }
        }

        #endregion IAffordabilityDetails Members
    }
}