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
using SAHL.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Factories;
using SAHL.Web.Views.Common.Presenters.FutureDatedTransactions;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters.DebitOrderDetailsApplication
{
    /// <summary>
    /// 
    /// </summary>
    public class DebitOrderDetailsAppAdd : DebitOrderDetailsAppBase
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Controller"></param>
        public DebitOrderDetailsAppAdd(IDebitOrderDetails View, SAHLCommonBaseController Controller)
            : base(View, Controller)
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
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new KeyChangedEventHandler(_view_OnSubmitButtonClicked);
           
            _view.gridPostBackType = GridPostBackType.None;

            _view.ShowControls = true;
            _view.ForceShowBankAccountControl = true;


            _view.BindBankAccountControl(base.BankAccounts);
            _view.BindGridForApplication(base.Application);
            _view.BindPaymentTypes();
            _view.BindDebitOrderDays();
          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
           
            _view.ShowButtons = true;
          
            _view.ShowLabels = false;
            _view.SubmitButtonText = "Add";
            _view.SetEffectiveDateToCurrentDate = true;
        }

        void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            TransactionScope ts = new TransactionScope();
            try
            {
                IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
                IApplicationDebitOrder debitOrder = AR.GetEmptyApplicationDebitOrder();
                ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                debitOrder.Application = base.Application;
                IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
                //todo : MUST SET TO THIS - BAR.GetBankAccountByKey(_view.BankAccountKey);
                IBankAccount ba = BAR.GetBankAccountByKey(17);
                debitOrder.BankAccount = ba;
                debitOrder.DebitOrderDay = int.Parse(_view.DODay);
                foreach (IFinancialServicePaymentType pt in LR.FinancialServicePaymentTypes)
                {
                    if (pt.Key.ToString() == _view.PaymentTypeKey)
                    {
                        debitOrder.FinancialServicePaymentType = pt;
                        break;
                    }
                }              
                base.Application.ApplicationDebitOrders.Add(_view.Messages,debitOrder);
                AR.SaveApplication(base.Application);
                ts.VoteCommit();
                _view.Navigator.Navigate("DebitOrderDetailsDisplay");
            }
            catch (DomainValidationException)
            {
                ts.VoteRollBack();
            }
            catch (Exception)
            {
                ts.VoteRollBack();
            }
            finally
            {
                ts.Dispose();
            }
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {

        }
    }
}
