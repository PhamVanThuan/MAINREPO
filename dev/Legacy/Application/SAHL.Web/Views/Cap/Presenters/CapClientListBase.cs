using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Cap.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CapClientListBase : SAHLCommonBasePresenter<ICAPClientList>
    {

        #region Protected Members

        protected ICapRepository _capRepo;
        protected DataTable _resetDates;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CapClientListBase(ICAPClientList view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage) return;

            _capRepo = RepositoryFactory.GetRepository<ICapRepository>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetDate"></param>
        protected  void BindOfferDateDropDown(DateTime resetDate)
        {
            IList<ICapTypeConfiguration> capTypeConfig = _capRepo.GetCapTypeConfigByResetDate(resetDate);
            if (capTypeConfig != null && capTypeConfig.Count > 0)
            {
                Dictionary<int, string> offerDateList = new Dictionary<int, string>();

                for (int i = 0; i < capTypeConfig.Count; i++)
                {
                    if (capTypeConfig[i].GeneralStatus.Key == (int)GeneralStatuses.Active)
                        offerDateList.Add(capTypeConfig[i].Key, capTypeConfig[i].ApplicationStartDate.ToString(SAHL.Common.Constants.DateFormat) + " - " + capTypeConfig[i].ApplicationEndDate.ToString(SAHL.Common.Constants.DateFormat));
                }

                _view.BindOfferDateDropDown(offerDateList);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void BindResetDateDropDown()
        {
            _resetDates = _capRepo.GetCurrentCAPResetConfigDates();
            _view.BindResetDateDropDown(_resetDates);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="captypeKey"></param>
        protected void BindGrid(int captypeKey)
        {
            ICapTypeConfiguration capTypeConfig = _capRepo.GetCapTypeConfigByKey(captypeKey);
            _view.BindGrid(new List<ICapTypeConfigurationDetail>(capTypeConfig.CapTypeConfigurationDetails));
        }


        /// <summary>
        /// This method is a bit of a hack - all it does is change the names of some of the mandatory 
        /// domain messages so they are a little mroe user friendly.
        /// </summary>
        protected void FixDomainMessages()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);

            foreach (IDomainMessage msg in spc.DomainMessages)
            {
                if (msg.Message.ToLower().IndexOf("identifier reference key") > -1)
                {
                    spc.DomainMessages.Remove(msg);
                    spc.DomainMessages.Add(new Error("Offer Dates is a mandatory field", ""));
                    break;
                }
            }
        }

       
    }
}
