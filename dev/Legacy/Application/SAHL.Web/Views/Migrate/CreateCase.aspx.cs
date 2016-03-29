using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Web.Views.Migrate.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.AJAX;
using SAHL.Common.Web.UI.Events;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Migrate
{
	public partial class CreateCase : SAHLCommonBaseView, ICreateCase
	{
		private enum grdLEColumns
		{
			AccountKey = 0,
			LegalEntityKey,
			DBSelected,
			DisplayName,
			IDNumber,
			RoleDescription,
			Update
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!ShouldRunPage) return;

			switch (WizardPage)
			{
				case 0: //select Account by key
					RegisterWebService(ServiceConstants.Account);
					acAccount.ServiceMethod = WebServiceUrls.SearchForAccountsByKey;
					break;
				case 1: //list legal entities, with select if applicable
					BindLegalEntityGrid();

					trLegalEntities.Visible = true;
					txtAccountKey.Visible = false;
					lblAccountKey.Text = AccountKey.ToString();
					lblAccountKey.Visible = true;
					btnNext.Visible = true;

					break;
				case 2: //select DC presenter, should not be in here, so do nothing
					break;
				case 3: //Case Detail
                    RegisterWebService(ServiceConstants.Account);

					trLegalEntities.Visible = false;
					txtAccountKey.Visible = false;
					lblAccountKey.Text = AccountKey.ToString();
					lblAccountKey.Visible = true;
					trCaseDetails.Visible = true;
					btnNext.Visible = true;
					btnBack.Visible = true;
					break;
				case 4:
					btnNext.Visible = true;
					btnNext.Enabled = true;

					btnBack.Visible = true;

					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Populate Case Details
		/// </summary>
		public void PopulateCaseDetails()
		{
			if (SelectedCase == null)
			{
				string error = String.Format("The Debt Counselling Case is not Valid for Account Key {0}", AccountKey);
				Messages.Add(new SAHL.Common.DomainMessages.Error(error, error));
				return;
			}

            if (SelectedCase.DebtCounsellingConsultantKey != null)
                ddlConsultant.SelectedValue = SelectedCase.DebtCounsellingConsultantKey.ToString();

            

			dteCourtOrder.Date = SelectedCase.CourtOrderDate;
			dteTermination.Date = SelectedCase.TerminateDate;

            if (SelectedCase.DateOf171.HasValue)
            {
                dte171.Date = SelectedCase.DateOf171; 
                dte60Days.Text = SelectedCase.SixtyDaysDate.HasValue ? SelectedCase.SixtyDaysDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "";
            }
            else
            {
                dte171.Date = Get171DateFromEworks;
                if (Get60DaysDate.HasValue)
                    dte60Days.Text = Get60DaysDate.Value.ToString(SAHL.Common.Constants.DateFormat);
            }

			dtePaymentRecieved.Date = SelectedCase.PaymentReceivedDate;
		}

		/// <summary>
		/// On Search Result Item Selected
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnAccountItemSelected(object sender, KeyChangedEventArgs e)
		{
			AccountKey = Convert.ToInt32(e.Key.ToString());

			//get LE's, Bind to grid
			btnNext_Click(sender, e);
		}

		/// <summary>
		/// Handles the RowDataBound event of the grid
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grdLegalEntities_OnRowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				GridViewRow gvr = e.Row;

				if (String.Compare(gvr.Cells[(int)grdLEColumns.DBSelected].Text, "1", true) == 0)
				{
					if (gvr.Cells[(int)grdLEColumns.Update].Controls.Count > 0)
					{
						CheckBox cb = (CheckBox)gvr.Cells[(int)grdLEColumns.Update].Controls[0];
						if (cb != null)
						{
							cb.Checked = true;
						}
					}
				}
			}
		}

		public void BindLegalEntityGrid()
		{
			if (LegalEntities == null)
				LegalEntities = MDCRepo.GetAccountLegalEntities(AccountKey);

			grdLegalEntities.AddGridBoundColumn("AccountKey", "AccountKey", Unit.Empty, HorizontalAlign.Left, false);
			grdLegalEntities.AddGridBoundColumn("LegalEntityKey", "LegalEntityKey", Unit.Empty, HorizontalAlign.Left, false);
			grdLegalEntities.AddGridBoundColumn("selected", "selected", Unit.Empty, HorizontalAlign.Left, false);
			grdLegalEntities.AddGridBoundColumn("DisplayName", "Legal Entity", Unit.Pixel(200), HorizontalAlign.Left, true);
			grdLegalEntities.AddGridBoundColumn("IDNumber", "IDNumber", Unit.Pixel(200), HorizontalAlign.Left, true);
			grdLegalEntities.AddGridBoundColumn("RoleDescription", "Role", Unit.Pixel(200), HorizontalAlign.Left, true);
			grdLegalEntities.AddCheckBoxColumn("Update", "Update", true, Unit.Pixel(50), HorizontalAlign.Left, true);

			grdLegalEntities.DataSource = LegalEntities;
			grdLegalEntities.DataBind();
		}

        /// <summary>
        /// Bind Consultant Users
        /// </summary>
        /// <param name="users"></param>
        public void BindConsultantUsers(DataTable users)
        {
            ddlConsultant.DataSource = users;
            ddlConsultant.DataTextField = "Description";
            ddlConsultant.DataValueField = "Key";
            ddlConsultant.DataBind();
        }

		public int AccountKey { get; set; }
		public int WizardPage { get; set; }
		public DataTable LegalEntities { get; set; }

		private IMigrationDebtCounsellingRepository _mdcRepo;
		private IMigrationDebtCounsellingRepository MDCRepo
		{
			get
			{
				if (_mdcRepo == null)
					_mdcRepo = RepositoryFactory.GetRepository<IMigrationDebtCounsellingRepository>();

				return _mdcRepo;
			}
		}

		private IMigrationDebtCounselling _selectedCase;
		public IMigrationDebtCounselling SelectedCase
		{
			get
			{
				if (_selectedCase == null)
				{
					_selectedCase = MDCRepo.GetMigrationDebtCounsellingByAccountKey(AccountKey);
				}
				return _selectedCase;
			}
			set { _selectedCase = value; }
		}
        private DataTable _consultantUsers;
        public DataTable ConsultantUsers
        {
            get
            {
                return _consultantUsers;
            }
            set
            {
                _consultantUsers = value;
                BindConsultantUsers(_consultantUsers);
            }
        }

		protected void btnBack_Click(object sender, EventArgs e)
		{
            //If the Wizard is on the Case Details View
            if (WizardPage == 3)
            {
                //Save the Debt Counselling Case From View
                GetDebtCounsellingCaseFromView();
            }

            WizardPage -= 1;

			if (OnSubmitButtonClicked != null)
				OnSubmitButtonClicked(sender, e);
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			//If the Wizard is on the Case Details View
			if (WizardPage == 3)
			{
				//Save the Debt Counselling Case From View
				GetDebtCounsellingCaseFromView();
			}

			WizardPage += 1;

			if (OnSubmitButtonClicked != null)
				OnSubmitButtonClicked(sender, e);
		}

		/// <summary>
		/// Get Debt Counselling Case From View
		/// </summary>
		/// <returns></returns>
		private void GetDebtCounsellingCaseFromView()
		{
			if (SelectedCase == null)
			{
				string error = String.Format("The Debt Counselling Case is not Valid for Account Key {0}", AccountKey);
				Messages.Add(new SAHL.Common.DomainMessages.Error(error, error));
				return;
			}

			if (!ValidateDebtCounsellingCase())
			{
				return;
			}

            if (ddlConsultant.SelectedValue != "-select-")
            {
                SelectedCase.DebtCounsellingConsultantKey = Convert.ToInt32(ddlConsultant.SelectedValue);
            }
			SelectedCase.DateOf171 = dte171.Date;
			SelectedCase.CourtOrderDate = dteCourtOrder.Date;
			SelectedCase.TerminateDate = dteTermination.Date;
            //SelectedCase.SixtyDaysDate = dte60Days.Date;
			SelectedCase.PaymentReceivedDate = dtePaymentRecieved.Date;
		}

		/// <summary>
		/// Validate Debt Counselling Case
		/// </summary>
		/// <returns></returns>
		private bool ValidateDebtCounsellingCase()
		{
			bool valid = true;
            if (!dte171.Date.HasValue)
            {
                string error = "The Date of the 17.1 is Mandatory";
                Messages.Add(new SAHL.Common.DomainMessages.Error(error, error));
                valid = false;
            }

            if (ddlConsultant.SelectedValue == "-select-")
            {
                string error = "The Consultant must be selected.";
                Messages.Add(new SAHL.Common.DomainMessages.Error(error, error));
                valid = false;
            }

			return valid;
		}

		/// <summary>
		/// 
		/// </summary>
		public IDictionary<Int32, bool> ListDCLegalEntities
		{
			get
			{
				Dictionary<Int32, bool> listDCLegalEntities = new Dictionary<Int32, bool>();

				foreach (GridViewRow gvr in grdLegalEntities.Rows)
					listDCLegalEntities.Add(Convert.ToInt32(gvr.Cells[(int)grdLEColumns.LegalEntityKey].Text), ((CheckBox)gvr.Cells[(int)grdLEColumns.Update].Controls[0]).Checked);

				return listDCLegalEntities;
			}
		}

		public event EventHandler OnSubmitButtonClicked;


        public DateTime? Get171DateFromEworks { get; set; }
        public DateTime? Get60DaysDate { get; set; }
        
    }
}