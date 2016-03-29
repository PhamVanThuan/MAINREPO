using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using presenter = SAHL.Web.Views.DebtCounselling.Interfaces;
using models = SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
	/// <summary>
	/// Attorney Update
	/// </summary>
	public class AttorneyUpdate : AttorneyBase
	{

		/// <summary>
		/// Constructor for AttorneyUpdate
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public AttorneyUpdate(presenter.IAttorney view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

		/// <summary>
		/// On View Initialized
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);

			_view.AttorneyUpdateClick += new EventHandler<EventArgs>(OnAttorneyUpdateClicked);

			_view.Readonly = false;
		}

		/// <summary>
		/// On Attorney Update Clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnAttorneyUpdateClicked(object sender, EventArgs e)
		{
            if (_view.IsValid)
			{
                LegalEntityRepository.InsertExternalRole(ExternalRoleTypes.LitigationAttorney, debtCounsellingKey, GenericKeyTypes.DebtCounselling2AM, _view.SelectedAttorneyKey,true);
                _view.Navigator.Navigate("ExternalRole");
			}
		}
	}
}