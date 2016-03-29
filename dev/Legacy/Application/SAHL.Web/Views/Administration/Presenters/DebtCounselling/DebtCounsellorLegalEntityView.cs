using System;
using SAHL.Common.Web.UI;
using SAHL.Common.DomainMessages;
using SAHL.Web.Views.Administration.Interfaces;

namespace SAHL.Web.Views.Administration.Presenters.DebtCounselling
{
	/// <summary>
	/// DebtCounsellorLegalEntity View
	/// </summary>
	public class DebtCounsellorLegalEntityView : DebtCounsellorLegalEntityBase
	{
        /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
        public DebtCounsellorLegalEntityView(IExternalOrganisationStructureLegalEntity view, SAHLCommonBaseController controller)
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
            if (!_view.ShouldRunPage) return;

            _view.AddressCaptureEnabled = false;
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage) return;
            SetUpControlsForUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            SetUpViewForDisplay();
        }


		#endregion

	}
}
