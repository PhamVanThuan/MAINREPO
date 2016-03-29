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

namespace SAHL.Web.Views.Common.Presenters.ManualDebitOrder
{   
    /// <summary>
    /// 
    /// </summary>
    public class ManualDebitOrder : ManualDebitOrderBase
    {

        //private CBOMenuNode _node;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Controller"></param>
        public ManualDebitOrder(IManualDebitOrder View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {
            _node = CBOService.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSet.CBONODESET) as CBOMenuNode;    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new KeyChangedEventHandler(_view_OnSubmitButtonClicked);
            _view.OnGridSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnGridSelectedIndexChanged);

            IFinancialServiceRepository FSR = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            IFinancialService fs = FSR.GetFinancialServiceByKey(10052374);// account.GetFinancialServiceByType(SAHL.Common.Globals.FinancialServiceTypes.VariableLoan);
            IEventList<IFinancialServiceRecurringTransaction> recurringTransactions = fs.FinancialServiceRecurringTransactions;            
            if (recurringTransactions.Count > 0)
            {
                _view.GridPostbackType = GridPostBackType.SingleClick;
                _view.BindOrdersToGrid(recurringTransactions);

                EventList<ILegalEntityBankAccount> bankAccounts = new EventList<ILegalEntityBankAccount>();


                //todo: check role types are valid
                int[] roleTypes = new int[3] { (int)SAHL.Common.Globals.RoleTypes.AssuredLife, (int)SAHL.Common.Globals.RoleTypes.MainApplicant, (int)SAHL.Common.Globals.RoleTypes.Suretor };

                IReadOnlyEventList<ILegalEntity> lstLegalEntities = fs.Account.GetLegalEntitiesByRoleType(_view.Messages, roleTypes);


                for (int x = 0; x < lstLegalEntities.Count; x++)
                {
                    for (int y = 0; y < lstLegalEntities[x].LegalEntityBankAccounts.Count; y++)
                    {
                        bankAccounts.Add(_view.Messages, lstLegalEntities[x].LegalEntityBankAccounts[y]);
                    }
                }
                _view.ShowLabels = true;
                _view.LegalEntityBankAccounts = bankAccounts;
            }              
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            _view.ShowButtons = false;
            _view.ArrearBalanceRowVisible = false;
            _view.ControlsVisible = true;
        }

        void _view_OnGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            
        }

    }
}
