using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.AJAX;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using System.Data;
using SAHL.Web.Views.DebtCounselling.Presenters;
using System.Text.RegularExpressions;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.DebtCounselling
{
	public partial class CreateCase : SAHLCommonBaseView, ICreateCase
	{
		public event EventHandler<EventArgs> CreateCaseClick;
		public event EventHandler<EventArgs> CancelClick;
		public event KeyChangedEventHandler PersonOfInterestClick;
		public event KeyChangedEventHandler LegalEntityIDNumberSelected;

		public SAHLTreeView TreeViewAccount
		{
			get { return trvAccounts; }
		}

		/// <summary>
		/// On Page Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.Params.Get("__EVENTTARGET") == "LegalEntitySearchPattern")
			{
				OnLegalEntityIDNumberSelected(this, new KeyChangedEventArgs(Request.Params.Get("__EVENTARGUMENT")));
			}
		}

		/// <summary>
		/// On Legal Entity ID Number Selected
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnLegalEntityIDNumberSelected(object sender, KeyChangedEventArgs e)
		{
			if (LegalEntityIDNumberSelected != null)
			{
				LegalEntityIDNumberSelected(sender, e);
			}
			txtPassportNumber.Text = String.Empty;
		}

		/// <summary>
		/// On Search Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnCreateCaseClick(object sender, EventArgs e)
		{
			//Ensure that the Accounts that were selected is updated

			if (CreateCaseClick != null)
			{
				CreateCaseClick(this, e);
			}
		}

		/// <summary>
		/// Add Legal Entity To List
		/// </summary>
		/// <param name="accounts"></param>
		public void UpdateDisplay(IList<AccountForView> accounts)
		{
			UpdatePeopleOfImportance(accounts);
			UpdateAccountsOfImportance(accounts);

			UpdateRelatedLegalEntities(accounts);
		}

		/// <summary>
		/// Show Debt Counsellor
		/// </summary>
		/// <param name="debtCounsellor"></param>
		public void ShowDebtCounsellor(ILegalEntity debtCounsellor)
		{
			lblDebtCounsellor.Text = debtCounsellor.DisplayName;
		}

		/// <summary>
		/// 
		/// </summary>
		public DateTime? Date17pt1 
        {
            get
            {
                if (dte17pt1Date.DateIsValid)
                    return dte17pt1Date.Date;

                return null;
            }
            set { dte17pt1Date.Date = value; }
        }

        public string ReferenceNumber
        {
            get
            {
                return txtReferenceNo.Text.Trim();
            }
        }

		/// <summary>
		/// Update People of Importance
		/// </summary>
		/// <param name="accounts"></param>
		public void UpdatePeopleOfImportance(IList<AccountForView> accounts)
		{
			if (trvPeople.Nodes.Count > 0)
			{
				trvPeople.Nodes.Clear();
			}
			foreach (AccountForView accountForView in accounts)
			{
				foreach (LegalEntityForView entityForView in accountForView.LegalEntities)
				{
                    SAHLTreeNode legalEntityNode = new SAHLTreeNode("<img src='../../Images/Client.gif' onmouseover=\"this.src='../../Images/delete.png'\" onmouseout=\"this.src='../../Images/Client.gif'\" /></td><td><input type='checkbox' tooltip=\"Show My Accounts\" onclick=\"HighlightRelated(this,\'a" + accountForView.Key + "\', \'l" + entityForView.Key + "\');\"/>" + entityForView.DisplayName, entityForView.Key.ToString());
					legalEntityNode.CssClass += "a" + accountForView.Key + " ";
					legalEntityNode.CssClass += "l" + entityForView.Key;
					legalEntityNode.OnClientClick = "DeSelectFromAccount('" + entityForView.Key + "'); return true;";
					if (trvPeople.FindNode(entityForView.Key.ToString()) == null && entityForView.IsInteresting)
					{
						trvPeople.Nodes.Add(legalEntityNode);
					}
				}
			}
		}

		/// <summary>
		/// Update Accounts of Importance
		/// </summary>
		/// <param name="accounts"></param>
		public void UpdateAccountsOfImportance(IList<AccountForView> accounts)
		{
			if (trvAccounts.Nodes.Count > 0)
			{
				trvAccounts.Nodes.Clear();
			}

			foreach (AccountForView accountForView in accounts)
			{
				if (accountForView.IsInteresting)
				{
                    SAHLTreeNode accountNode = new SAHLTreeNode("<img src='../../Images/originationsources/" + accountForView.OriginationSourceKey + ".gif'\"/>" + accountForView.ProductDescription + " (" + accountForView.Key.ToString() + ") ", accountForView.Key.ToString());

					accountNode.CssClass += "a" + accountForView.Key + " ";

					foreach (LegalEntityForView legalEntityForView in accountForView.LegalEntities)
					{
						accountNode.CssClass += "l" + legalEntityForView.Key + " ";
					}
					accountNode.Expanded = true;
					accountNode.CheckBoxVisible = false;
					accountNode.CheckBoxDisabled = true;

					//If the account is under debt counselling
					if (accountForView.IsUnderDebtCounselling)
					{
						accountNode.Text = "<img src='../../Images/debtcounselling.gif'/>" + accountNode.Text;
					}

					foreach (LegalEntityForView entityForView in accountForView.LegalEntities)
					{
                        SAHLTreeNode legalEntityNode = new SAHLTreeNode(entityForView.DisplayName + " (" + entityForView.RoleTypeDescription + ")", accountForView.Key + "-" + entityForView.Key.ToString());
						if (entityForView.IsUnderDebtCounselling)
						{
							legalEntityNode.Text = "<img src='../../Images/debtcounselling.gif'/>" + legalEntityNode.Text;
						}
						//Disable the Checkbox for non-natural persons
						if (entityForView.LegalEntityType != SAHL.Common.Globals.LegalEntityTypes.NaturalPerson ||
							entityForView.IsUnderDebtCounselling)
						{
							legalEntityNode.CheckBoxVisible = false;
							legalEntityNode.CheckBoxDisabled = true;
						}
						accountNode.Nodes.Add(legalEntityNode);
						if (entityForView.IsInteresting && 
							entityForView.FlaggedForDebtCounselling &&
							entityForView.IsUnderDebtCounselling == false)
						{
							trvAccounts.CheckedValuePaths.Add(accountForView.Key + "/" + accountForView.Key + "-" + entityForView.Key.ToString());
						}
					}
					trvAccounts.Nodes.Add(accountNode);
				}
			}
		}

		/// <summary>
		/// Update Legal Entities
		/// </summary>
		/// <param name="accounts"></param>
		public void UpdateRelatedLegalEntities(IList<AccountForView> accounts)
		{
			footRelatedResults.Controls.Clear();
			
			List<LegalEntityForView> distinctLegalEntities = new List<LegalEntityForView>();

			foreach (AccountForView accountForView in accounts)
			{
				if (accountForView.IsInteresting)
				{
					foreach (LegalEntityForView legalEntity in accountForView.LegalEntities)
					{
						if (!legalEntity.IsInteresting)
						{
							if (distinctLegalEntities.Find(le => le.Key == legalEntity.Key) == null)
							{
								distinctLegalEntities.Add(legalEntity);
							}
						}
					}
				}
			}

			foreach (LegalEntityForView entityForView in distinctLegalEntities)
			{
				TableRow row = new TableRow();
				row.Attributes.Add("onmouseover", "this.className='HighlightLightGrey'");
				row.Attributes.Add("onmouseout", "this.className=''");
				row.Attributes.Add("onclick", "__doPostBack('LegalEntitySearchPattern'," + entityForView.Key + ");");
                row.Height = Unit.Pixel(16);
				//Add the Person
				if (!entityForView.IsInteresting)
				{
					TableCell nameCell = new TableCell() { Text = entityForView.DisplayName };
					//nameCell.Style.Add("padding-bottom", "4px");
					TableCell idNumberCell = new TableCell() { Text = entityForView.IDNumber };;
					//idNumberCell.Style.Add("padding-bottom", "4px");
					row.Cells.AddRange(new TableCell[] { nameCell, idNumberCell});
				}
				footRelatedResults.Controls.Add(row);
			}
		}

		/// <summary>
		/// On Remove Person Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnRemovePersonClick(object sender, SAHLTreeNodeEventArgs e)
		{
			if (PersonOfInterestClick != null)
			{
				PersonOfInterestClick(sender, new KeyChangedEventArgs(int.Parse(e.TreeNode.Value)));
			}
		}

		/// <summary>
		/// On Cancel Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnCancelClick(object sender, EventArgs e)
		{
			if (CancelClick != null)
			{
				CancelClick(sender, null);
			}
		}
	}
}