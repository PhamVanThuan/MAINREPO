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
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Common.Presenters
{
    public class CreditScoringSummaryFull : CreditScoringSummaryBase
    {

        /// <summary>
        /// CreditScoringSummaryFull Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CreditScoringSummaryFull(ICreditScoringSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnScoreGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnScoreGridSelectedIndexChanged);
            _view.ShowGrids();
            _view.BindScoreGrid(_scores);

            if (_scores != null && _scores.Count > 0)
            {
                _view.BindApplicantGrid(_scores[0]);
                _view.BindRuleGrid(_scores[0]);
            }

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

        }

        protected void _view_OnScoreGridSelectedIndexChanged(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            int key = Convert.ToInt32(e.Key);

            if (key != -1)
            {
                IApplicationCreditScore score = _scores[key];
                _view.BindApplicantGrid(score);
                _view.BindRuleGrid(score);
            }
            else
            {
                _view.BindApplicantGrid(null);
                _view.BindRuleGrid(null);
            }
        }
    }
}
