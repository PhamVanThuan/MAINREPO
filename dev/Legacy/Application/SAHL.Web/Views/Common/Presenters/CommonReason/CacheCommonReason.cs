using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Exceptions;
using Castle.ActiveRecord;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    public class CacheCommonReason : CommonReasonBase
    {
        List<ICacheObjectLifeTime> _lifeTimes;
        
        public CacheCommonReason(ICommonReason view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            _selectedReasons = (List<SelectedReason>)e.Key;

            Dictionary<int, string> SelectedReasons = new Dictionary<int, string>();
            foreach (SelectedReason sr in _selectedReasons)
            {
                SelectedReasons.Add(sr.ReasonDefinitionKey, sr.Comment);
            }

            if (GlobalCacheData.ContainsKey(ViewConstants.CacheReasons))
                GlobalCacheData.Remove(ViewConstants.CacheReasons);

            GlobalCacheData.Add(ViewConstants.CacheReasons, SelectedReasons, LifeTimes);

            if (_view.IsValid)
            {
                Navigator.Navigate("Submit");
            }
        }


        protected List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add("AdminDebtCounsellorSelect");
                    views.Add("DebtCounsellingInitiatorReason");
                    views.Add("DebtCounsellingCreateCase");
                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }
                return _lifeTimes;
            }
        }

        public override void CancelActivity()
        {
            Navigator.Navigate("Cancel");
        }

        public override void CompleteActivityAndNavigate()
        {
            throw new NotImplementedException();
        }
    }
}