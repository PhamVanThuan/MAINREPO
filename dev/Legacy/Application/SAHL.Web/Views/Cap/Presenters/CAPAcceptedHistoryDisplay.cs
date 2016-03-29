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
using System.Collections;


namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CAPAcceptedHistoryDisplay : SAHLCommonBasePresenter<ICAPAcceptedHistory>
    {
        IDictionary<int,IFinancialAdjustment> _financialAdjustmentDict; 

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CAPAcceptedHistoryDisplay(ICAPAcceptedHistory view, SAHLCommonBaseController controller)
            : base(view, controller)
        {


        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage) return;

            _view.CancelCapRowVisible = false;
            _view.ButtonRowVisible = false;

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode != null)
            {
                _financialAdjustmentDict = new Dictionary<int,IFinancialAdjustment>();
                ICapRepository capRepo = RepositoryFactory.GetRepository<ICapRepository>();
                IList<ICapApplication> capOfferList = capRepo.GetCapOfferByAccountKeyAndStatus(cboNode.GenericKey, 2);
                
                IList<ICapApplicationDetail> capOfferDetailList = new List<ICapApplicationDetail>();

                foreach (ICapApplication _capApplication in capOfferList)
                {
                    foreach (ICapApplicationDetail _capApplicationDetail in _capApplication.CapApplicationDetails)
                    {
                        if (CanAdd(_capApplicationDetail))
                            capOfferDetailList.Add(_capApplicationDetail);
                    }
                }

                _view.FinancialAdjustmentDict = _financialAdjustmentDict;
                _view.BindGrid(capOfferDetailList);            
            }
        }

        
        public bool CanAdd(ICapApplicationDetail capAppDetail)
        {
            
            IMortgageLoanAccount mortgageLoanAcc = capAppDetail.CapApplication.Account as IMortgageLoanAccount;
            IMortgageLoan mortgageLoan = mortgageLoanAcc.SecuredMortgageLoan;

            foreach (IFinancialAdjustment _fa in mortgageLoan.FinancialAdjustments)
            {
                if (capAppDetail.CapTypeConfigurationDetail.CapTypeConfiguration.CapEffectiveDate == _fa.FromDate
                    && _fa.FixedRateAdjustment != null
                    && System.Math.Round(capAppDetail.CapTypeConfigurationDetail.Rate,5) == System.Math.Round(_fa.FixedRateAdjustment.Rate,5)
                    )
                {
                    _financialAdjustmentDict.Add(capAppDetail.Key, _fa);
                    return true;
                }
            }

            return false;
        }
    }
}
