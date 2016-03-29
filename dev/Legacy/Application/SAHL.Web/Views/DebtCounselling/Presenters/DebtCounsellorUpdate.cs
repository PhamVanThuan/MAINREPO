using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class DebtCounsellorUpdate : ExternalRoleUpdateBase
    {
        private IList<ICacheObjectLifeTime> _lifeTimes;
        public IList<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add("DebtCounsellorUpdateSelect");
                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }

                return _lifeTimes;
            }
        }

        /// <summary>
        /// Constructor for DebtCounsellorUpdate
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DebtCounsellorUpdate(IExternalRoleUpdate view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.RoleType = ExternalRoleTypes.DebtCounsellor;
            _view.GridHeader = "Update Debt Counsellor";

            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedTreeNodeKey))
                GlobalCacheData.Remove(ViewConstants.SelectedTreeNodeKey);

            GlobalCacheData.Add(ViewConstants.SelectedTreeNodeKey, DebtCounselling.DebtCounsellorLEOrganisationStructure.Key, LifeTimes);

            if (GlobalCacheData.ContainsKey(ViewConstants.DebtCounsellorLegalEntityKey))
            {
                _view.NewLegalEntity = LERepo.GetLegalEntityByKey((int)GlobalCacheData[ViewConstants.DebtCounsellorLegalEntityKey]);
            }
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }
    }
}