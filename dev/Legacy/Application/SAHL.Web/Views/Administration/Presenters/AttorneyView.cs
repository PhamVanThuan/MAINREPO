using System;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Administration.Presenters
{
	/// <summary>
	/// Attorney View
	/// </summary>
	public class AttorneyView : AttorneyBase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public AttorneyView(SAHL.Web.Views.Administration.Interfaces.IAttorney view, SAHLCommonBaseController controller)
			: base(view, controller)
		{

		}

		#region Events

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);

            _view.OnContactsButton_Clicked += new EventHandler(_view_OnContactsButtonClicked);

			base._Adding = false;
			base._Editing = false;

			if (_view.IsPostBack)
			{
				_view.Set_pnlAttorneyDetailsVisibility = false;
				_view.Set_pnlAddressVisibility = false;
				if (this.GlobalCacheData.ContainsKey("DeedsOfficeKey"))
					BindAttorneyDropDown((int)this.GlobalCacheData["DeedsOfficeKey"]);
			}
			else
			{
				if (this.GlobalCacheData.ContainsKey("AttorneyKey") && this.GlobalCacheData.ContainsKey("DeedsOfficeKey"))
				{
                    //this.GlobalCacheData.Add("AttorneyKey", this.GlobalCacheData["AttorneyKey"]);
                    //this.GlobalCacheData.Add("DeedsOfficeKey", this.GlobalCacheData["DeedsOfficeKey"]);
                    //this.GlobalCacheData.Remove("AttorneyKey");
                    //this.GlobalCacheData.Remove("DeedsOfficeKey");
					BindAttorneyDropDown((int)this.GlobalCacheData["DeedsOfficeKey"]);
				}
			}
		}

        protected void _view_OnContactsButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("AttorneyContactView");
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewPreRender(object sender, EventArgs e)
		{
			base.OnViewPreRender(sender, e);

			if (_view.IsPostBack)
			{
				if (this.GlobalCacheData.ContainsKey("AttorneyKey"))
				{
					PopulateAttorneyControls((int)this.GlobalCacheData["AttorneyKey"]);
					ShowAttorneyDetails();
				}
				else
				{
					_view.Set_pnlAttorneyDetailsVisibility = false;
					_view.Set_pnlAddressVisibility = false;
					if (this.GlobalCacheData.ContainsKey("DeedsOfficeKey"))
					{
						SetAttorneyVisibility(true);
						BindAttorneyDropDown((int)this.GlobalCacheData["DeedsOfficeKey"]);
					}
					else
						SetAttorneyVisibility(false);
				}
			}
			else
			{
				if (this.GlobalCacheData.ContainsKey("AttorneyKey"))
				{
					_view.GetSet_ddlDeedsOffice = (int)this.GlobalCacheData["DeedsOfficeKey"];
					_view.GetSet_ddlAttorney = (int)this.GlobalCacheData["AttorneyKey"];
					PopulateAttorneyControls((int)this.GlobalCacheData["AttorneyKey"]);
					ShowAttorneyDetails();
				}
				else
					SetAttorneyVisibility(false);

			}
		}

		#endregion

	}
}
