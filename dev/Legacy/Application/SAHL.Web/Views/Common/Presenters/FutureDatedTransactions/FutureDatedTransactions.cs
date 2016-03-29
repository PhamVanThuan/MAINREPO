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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections;
using SAHL.Common.UI;
using SAHL.Web.Views.Common.Presenters.FutureDatedTransactions;


namespace SAHL.Web.Views.Common.Presenters.FutureDatedTransactions
{   
    /// <summary>
    /// 
    /// </summary>
    public class FutureDatedTransactions : FutureDatedTransactionsBase
    {

        //private CBOMenuNode _node;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public FutureDatedTransactions(IFutureDatedTransactions view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;    
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

            IFinancialService fs = FinancialService;
            IEventList<IManualDebitOrder> manualDebitOrdersPrev = _manualDebitOrderRepository.GetCollectedManualDebitOrdersByFinancialServiceKey(FinancialService.Key);

            if (manualDebitOrdersPrev.Count > 0)
                _view.BindOrdersToPreviousGrid(manualDebitOrdersPrev);

            if (RecurringTransactions.Count > 0)
            {
                _view.GridPostbackType = GridPostBackType.SingleClick;
                int[] roleTypes = new int[3] { (int)SAHL.Common.Globals.RoleTypes.AssuredLife, (int)SAHL.Common.Globals.RoleTypes.MainApplicant, (int)SAHL.Common.Globals.RoleTypes.Suretor };
                IReadOnlyEventList<ILegalEntity> lstLegalEntities = fs.Account.GetLegalEntitiesByRoleType(_view.Messages, roleTypes);

                foreach (ILegalEntity le in lstLegalEntities)
                {
                    foreach (ILegalEntityBankAccount ba in le.LegalEntityBankAccounts)
                    {
                        _view.LegalEntityBankAccounts.Add(ba);
                    }
                }

            }
            _view.ShowLabels = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.ShowButtons = false;
            _view.ArrearBalanceRowVisible = false;
            _view.RecordsGridPrvVisible = true;

            int recordCount = base.FinancialService.ManualDebitOrders.Count;
            _view.ControlsVisible = (recordCount > 0);
        }

    }
}
