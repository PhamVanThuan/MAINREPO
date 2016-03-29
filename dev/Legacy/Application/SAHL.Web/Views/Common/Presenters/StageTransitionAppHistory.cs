using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class StageTransitionAppHistory : SAHLCommonBasePresenter<IStageTransitionHistory>
    {
        private IStageDefinitionRepository _stageDefinitionRepo;
        private DataTable _dtStageTransitions;

		private const string StageTransitions = "DTstageTransitions";

		private List<ICacheObjectLifeTime> _lifeTimes;
		protected List<ICacheObjectLifeTime> LifeTimes
		{
			get
			{
				if (_lifeTimes == null)
				{
					List<string> views = new List<string>();
					views.Add("StageTransitionAppHistory");
					_lifeTimes = new List<ICacheObjectLifeTime>();
					_lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
				}
				return _lifeTimes;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public StageTransitionAppHistory(IStageTransitionHistory view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.ShowLifeWorkFlowHeader = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

            _view.onCancelButtonClicked += new EventHandler(_view_onCancelButtonClicked);

            // get the Stage Transitions
            IDictionary<GenericKeyTypes, List<int>> dicGenricKeyTypeAndKeys = new Dictionary<GenericKeyTypes, List<int>>();
            List<int> genericKeys = new List<int>();

            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
            {
                genericKeys.Add(Convert.ToInt32(GlobalCacheData[ViewConstants.ApplicationKey]));
                dicGenricKeyTypeAndKeys.Add(GenericKeyTypes.Offer, genericKeys);

                GlobalCacheData.Remove(ViewConstants.ApplicationKey);
            }

            if (!_view.IsPostBack)
            {
                _dtStageTransitions = _stageDefinitionRepo.GetStageTransitionDTByGenericKeyTypeAndKeys(dicGenricKeyTypeAndKeys);

				if (GlobalCacheData.ContainsKey(StageTransitions))
					this.GlobalCacheData[StageTransitions] = _stageDefinitionRepo;
				else
					this.GlobalCacheData.Add(StageTransitions, _dtStageTransitions, LifeTimes);
            }
            else
            {
				_dtStageTransitions = GlobalCacheData[StageTransitions] as DataTable;
            }

            _view.SetUpGrid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.BindHistoryGrid(_dtStageTransitions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_onCancelButtonClicked(object sender, EventArgs e)
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
            {
                string navigateTo = GlobalCacheData[ViewConstants.NavigateTo].ToString();
                GlobalCacheData.Remove(ViewConstants.NavigateTo);
                _view.Navigator.Navigate(navigateTo);
            }
            else
                _view.Navigator.Navigate("Cancel");
        }
    }
}
