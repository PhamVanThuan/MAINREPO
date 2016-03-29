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
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common.Presenters.SettlementBanking
{
    public class SettlementBankingDetailsDelete : SettlementBankingDetailsBase
    {
        public SettlementBankingDetailsDelete(IBankingDetails View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.BankAccountGridEnabled = true;
            //_view.HideGridStatusColumn = false;
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnCancelClicked += new EventHandler(_view_OnCancelClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            _view.SubmitButtonText = "Delete";
        
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();

            try
            {
                if (_view.SelectedBankAccount != null)
                {
                    int bankAccountKey = _view.SelectedBankAccount.Key;

                    IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
                    foreach (IApplicationExpense ae in Application.ApplicationExpenses)
                    {
                        foreach (IApplicationDebtSettlement ads in ae.ApplicationDebtSettlements)
                        {
                            if (ads.BankAccount != null && ads.BankAccount.Key == bankAccountKey)
                            {
                                BAR.DeleteApplicationDebtSettlementBankAccount(ads.Key);
                            }
                        }
                    }
                    ts.VoteCommit();
                    _view.Navigator.Navigate("SettlementBankingDetailsDisplay");
                }
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

        void _view_OnCancelClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("SettlementBankingDetailsDisplay");
        }


    }
}
