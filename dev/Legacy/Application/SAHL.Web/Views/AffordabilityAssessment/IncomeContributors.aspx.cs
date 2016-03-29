using SAHL.Common.Web.UI;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Web.Views.AffordabilityAssessment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.AffordabilityAssessment
{
    /// <summary>
    ///
    /// </summary>
    public partial class IncomeContributors : SAHLCommonBaseView, IIncomeContributors
    {
        private int _selectedLegalEntityKey;

        #region Protected Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;

            ddlLegalEntity.VerifyPleaseSelect();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.ShouldRunPage)
                return;

            txtNumberOfContributingApplicants.Focus();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.ShouldRunPage)
                return;

            ddlLegalEntity.PleaseSelectItem = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddContributor_Click(object sender, EventArgs e)
        {
            // get the selected legalentity
            if (String.Compare(ddlLegalEntity.SelectedValue, "-select-", true) == 0)
                _selectedLegalEntityKey = 0;
            else
                _selectedLegalEntityKey = Convert.ToInt32(ddlLegalEntity.SelectedItem.Value);

            OnAddContributorButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            OnSubmitButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        #endregion Protected Members

        #region Events

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnAddContributorButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        #endregion Events

        #region IAdd Members

        public void BindApplicantContributors(IEnumerable<LegalEntityModel> applicantContributorsList, IEnumerable<int> selectedLegalEntities)
        {
            foreach (LegalEntityModel contributor in applicantContributorsList)
            {
                ListItem item = new ListItem(contributor.LegalEntityDescription, contributor.LegalEntityKey.ToString());
                item.Selected = selectedLegalEntities.Any(x => x == contributor.LegalEntityKey);

                chklApplicantContributors.Items.Add(item);
            }
        }

        public void BindNonApplicantContributors(IEnumerable<LegalEntityModel> nonApplicantContributorsList, IEnumerable<int> selectedLegalEntities)
        {
            foreach (LegalEntityModel contributor in nonApplicantContributorsList)
            {
                ListItem item = new ListItem(contributor.LegalEntityDescription, contributor.LegalEntityKey.ToString());
                item.Selected = selectedLegalEntities.Any(x => x == contributor.LegalEntityKey);

                chklNonApplicantContributors.Items.Add(item);
            }
        }

        public void BindLegalEntityDropdownList(IEnumerable<LegalEntityModel> applicantContributorsList)
        {
            foreach (LegalEntityModel contributor in applicantContributorsList)
            {
                ddlLegalEntity.Items.Add(new ListItem(contributor.LegalEntityDescription, contributor.LegalEntityKey.ToString()));
            }
        }

        #endregion IAdd Members

        /// <summary>
        ///
        /// </summary>
        public int SelectedLegalEntityKey
        {
            set
            {
                _selectedLegalEntityKey = value;
            }
            get
            {
                return _selectedLegalEntityKey;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int NumberOfContributingApplicants
        {
            get
            {
                int numberOfContributingApplicants;
                int.TryParse(txtNumberOfContributingApplicants.Text, out numberOfContributingApplicants);
                return numberOfContributingApplicants;
            }
            set
            {
                txtNumberOfContributingApplicants.Text = value.ToString();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int NumberOfHouseholdDependants
        {
            get
            {
                int numberOfHouseholdDependants;
                int.TryParse(txtNumberOfHouseholdDependants.Text, out numberOfHouseholdDependants);
                return numberOfHouseholdDependants;
            }
            set
            {
                txtNumberOfHouseholdDependants.Text = value.ToString();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<int> GetContributorsList
        {
            get
            {
                IList<int> contributorsList = new List<int>();
                foreach (ListItem item in chklApplicantContributors.Items)
                {
                    if (item.Selected)
                        contributorsList.Add(Convert.ToInt32(item.Value));
                }
                foreach (ListItem item in chklNonApplicantContributors.Items)
                {
                    if (item.Selected)
                        contributorsList.Add(Convert.ToInt32(item.Value));
                }
                return contributorsList;
            }
        }
    }
}