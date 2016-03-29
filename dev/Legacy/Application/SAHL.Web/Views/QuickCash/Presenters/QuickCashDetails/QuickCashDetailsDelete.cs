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
using SAHL.Web.Views.QuickCash.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using System.Collections;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.QuickCash.Presenters.QuickCashDetails
{
    public class QuickCashDetailsDelete : QuickCashDetailsBase
    {

        public QuickCashDetailsDelete(IQuickCashDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.SetSubmitButtonText = "Delete";
            _view.ShowApprovedPanel = true;
            _view.QuickCashInformation = quickCashApplicationInformation;
            _view.BindApprovedPanel();
            _view.BindQuickCashPaymentsGrid(true);
            _view.ShowBankAccountPanel = false;
            _view.ShowButtons = true;

            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
        }

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("QuickCashDetails");
        }

        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            IApplicationInformationQuickCashDetail appInfoQCDetail = GetAppInfoQuickCashDetailMarkedForDeletion();
            
            IEventList<IApplicationExpense> appExpenseLst = appInfoQCDetail.ApplicationExpenses;

            TransactionScope txn = new TransactionScope();
          
            try
            {
                appInfoQuickCash.ApplicationInformationQuickCashDetails.Remove(_view.Messages, appInfoQCDetail);
                qcRepo.SaveApplicationInformationQuickCash(appInfoQuickCash);

                if (appExpenseLst != null && appExpenseLst.Count > 0)
                {
                    for (int i = 0; i < appExpenseLst.Count; i++)
                    {
                        qcRepo.DeleteApplicationExpense(appExpenseLst[i]);
                    }     
                }
                txn.VoteCommit();
            }

            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }

            finally
            {
                txn.Dispose();
            }

        }

        private IApplicationInformationQuickCashDetail GetAppInfoQuickCashDetailMarkedForDeletion()
        {
            IApplicationInformationQuickCashDetail appQCDetail = null;

            for (int i = 0; i < appInfoQuickCash.ApplicationInformationQuickCashDetails.Count; i++)
            {
                if (appInfoQuickCash.ApplicationInformationQuickCashDetails[i].Key == _view.GetSetSelectedGridItem)
                {
                    appQCDetail = appInfoQuickCash.ApplicationInformationQuickCashDetails[i];
                    break;
                }
            }
            return appQCDetail;
        }
    }
}
