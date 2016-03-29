using System;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Administration.Presenters.DebtCounselling
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentDistributionAgentSelect : PaymentDistributionAgentBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PaymentDistributionAgentSelect(IExternalOrganisationStructure view, SAHLCommonBaseController controller)
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
            _view.SelectButtonClicked += new EventHandler(SelectClick);

            // Set Button Visibility
            _view.AddButtonVisible = false;
            _view.RemoveButtonVisible = false;
            _view.UpdateButtonVisible = false;
            _view.ViewButtonVisible = false;
            _view.SelectButtonVisible = true;
            _view.CancelButtonVisible = true;

            // Set DragDrop Functionality
            _view.AllowNodeDragging = false;

            // Set "Add to CBO" button visibility
            _view.AllowAddToCBO = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "New rule added to FxCop, avoiding refactor")]
        protected virtual void SelectClick(object sender, EventArgs e)
        {
			int leKey;
			Int32.TryParse(GetSelectedItemValue(DataTableColumn.LegalEntityKey), out leKey);

			if (GlobalCacheData.ContainsKey(ViewConstants.PaymentDistributionAgenctLegalEntityKey))
				GlobalCacheData.Remove(ViewConstants.PaymentDistributionAgenctLegalEntityKey);

			if (leKey > 0)
			{
				GlobalCacheData.Add(ViewConstants.PaymentDistributionAgenctLegalEntityKey, leKey, LifeTimes);
			}
			else
			{
				_view.Messages.Add(new Error("No item selected to update.", "No item selected to update."));
			}

			if (_view.IsValid)
			{
				if (GlobalCacheData.ContainsKey(ViewConstants.SelectView))
					Navigator.Navigate(GlobalCacheData[ViewConstants.SelectView].ToString());
				else
					Navigator.Navigate("Select");
			}
        }

        #endregion

        #region Methods

        #endregion

		#region Properties
		List<ICacheObjectLifeTime> _lifeTimes;
		protected List<ICacheObjectLifeTime> LifeTimes
		{
			get
			{
				if (_lifeTimes == null)
				{
					List<string> views = new List<string>();
					views.Add("AdminPaymentDistributionAgentSelect");
					views.Add("PDAUpdate");

					_lifeTimes = new List<ICacheObjectLifeTime>();
					_lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
				}
				return _lifeTimes;
			}
		}
		#endregion
	}
}
