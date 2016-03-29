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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Factories;
using SAHL.Web.Views.Common.Presenters.FutureDatedTransactions;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.DomainMessages;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Presenters.DebitOrderDetailsApplication
{
    /// <summary>
    /// 
    /// </summary>
    public class DebitOrderDetailsAppUpdate : DebitOrderDetailsAppBase
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DebitOrderDetailsAppUpdate(IDebitOrderDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
 
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

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnUpdateButtonClicked += new KeyChangedEventHandler(_view_OnUpdateButtonClicked);
            _view.OnGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnGridSelectedIndexChanged);


            _view.SetControlsToGrid = true;

            _view.BindBankAccountControl(base.BankAccounts);

            _view.BindGridForApplication(base.Application);
            _view.BindPaymentTypes();
            _view.BindDebitOrderDays();

            _view.gridPostBackType = GridPostBackType.SingleClick;
        }

        void _view_OnGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
         
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
            _view.ShowButtons = true;
            _view.ShowControls = true;
            _view.ShowLabels = false;
            _view.ButtonUpdateVisible = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "New rule added to FxCop, avoiding refactor")]
        void _view_OnUpdateButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
			IApplicationDebitOrder iAppDo = null ;
			IAccountRepository accrepo = RepositoryFactory.GetRepository<IAccountRepository>();
            TransactionScope ts = new TransactionScope();
            try
            {
                int doDay = -1;
                int.TryParse(_view.DODay, out doDay);

                IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();

                if (e != null && e.Key != null)
                {
                    //update mode
                    int key = int.Parse(e.Key.ToString());
                    IEventList<IApplicationDebitOrder> lstDebitOrders = AR.GetApplicationDebitOrdersByApplicationKey(base.Application.Key);

                    foreach (IApplicationDebitOrder order in lstDebitOrders)
                    {
                        if (order.Key == key)
                        {
                            order.DebitOrderDay = doDay;
							iAppDo = order;
                            IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
                            IBankAccount ba = null;
                            int BankAccountKey = -1;
                            bool validBankAccount = int.TryParse(_view.BankAccountKey,out BankAccountKey);
                            if (validBankAccount)
                            {
                                ba = BAR.GetBankAccountByKey(BankAccountKey);
                            }
                            ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                            foreach (IFinancialServicePaymentType pt in LR.FinancialServicePaymentTypes)
                            {
                                if (pt.Key.ToString() == _view.PaymentTypeKey)
                                {
                                    order.FinancialServicePaymentType = pt;
                                    break;
                                }
                            }
                            order.BankAccount = ba;                            
                            break;
                        }
                    }
                }
                else
                {
                    //add mode
                    IApplicationDebitOrder debitOrder = AR.GetEmptyApplicationDebitOrder();
                    ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                    debitOrder.Application = base.Application;
					iAppDo = debitOrder;
                    IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
                    IBankAccount ba = null;
                    int BankAccountKey = -1;
                    bool validBankAccount = int.TryParse(_view.BankAccountKey, out BankAccountKey);
                    if (validBankAccount)
                    {
                        ba = BAR.GetBankAccountByKey(BankAccountKey);
                    }                    
                    debitOrder.BankAccount = ba;

                    debitOrder.DebitOrderDay = doDay;

                    foreach (IFinancialServicePaymentType pt in LR.FinancialServicePaymentTypes)
                    {
                        if (pt.Key.ToString() == _view.PaymentTypeKey)
                        {
                            debitOrder.FinancialServicePaymentType = pt;
                            break;
                        }
                    }                    
                    base.Application.ApplicationDebitOrders.Add(_view.Messages, debitOrder);
                }
				accrepo.UpdateAccountDebitOrderFromApplicationDebitOrder(iAppDo);
                AR.SaveApplication(base.Application);
                ts.VoteCommit();
                _view.Navigator.Navigate("DebitOrderDetailsAppDisplay");
            }          
            catch (Exception)
            {
                ts.VoteRollBack();
                if (_view.IsValid)
                {
                    throw;
                }
            }
            finally
            {
                ts.Dispose();
            }

        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("DebitOrderDetailsAppDisplay");
        }
    }
}
