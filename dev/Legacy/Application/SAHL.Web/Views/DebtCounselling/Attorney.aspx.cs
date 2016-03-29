using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using presenter = SAHL.Web.Views.DebtCounselling.Interfaces;
using models = SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.DebtCounselling
{
	public partial class Attorney : SAHLCommonBaseView, presenter.IAttorney
	{
		public bool Readonly { get; set; }
		public int SelectedAttorneyKey { get; set; }
		public string SelectedAttorneyName { get; set; }

		public event EventHandler<EventArgs> AttorneyUpdateClick;

		/// <summary>
		/// On Page Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				divAttorneyUpdate.Visible = !Readonly;
				divAttorneyView.Visible = Readonly;
				lblSelectedAttorney.Text = SelectedAttorneyName;
			}
		}

		/// <summary>
		/// Bind Legal Entities
		/// </summary>
		/// <param name="attornies"></param>
		/// <param name="selectedAttorneyKey"></param>
		public void BindAttornies(IDictionary<int, string> attornies, int selectedAttorneyKey)
		{
			cmbAttornies.Items.Clear();
			foreach (KeyValuePair<int, string> attorneyDetail in attornies)
			{
				ListItem attorneyItem = new ListItem(attorneyDetail.Value, attorneyDetail.Key.ToString());
				cmbAttornies.Items.Add(attorneyItem);
			}

			if (selectedAttorneyKey > 0)
			{
				cmbAttornies.SelectedValue = selectedAttorneyKey.ToString();
			}
		}

		/// <summary>
		/// On Attorney Select Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnAttorneyUpdateClicked(object sender, EventArgs e)
		{
			if (AttorneyUpdateClick != null)
			{
				SelectedAttorneyKey = int.Parse(cmbAttornies.SelectedValue);
				AttorneyUpdateClick(sender, e);
			}
		}
	}
}