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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Globals;

using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Service.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Presenters
{
    public class PostArrearTransaction : PostTransactionBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PostArrearTransaction(IPostTransaction view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.OnPostButtonClicked += new EventHandler(_view_OnPostButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            TransactionTypeList = BulkBatchRepo.GetLoanTransactionTypesArrears(_view.CurrentPrincipal);
            base.OnViewInitialised(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnPostButtonClicked(object sender, EventArgs e)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                try
                {
                    Validate();

                    //Special case for the presenter is to check if the following transactions are being captured
                    if (_view.TransactionType == (int)TransactionTypes.DebtReviewArrangementCredit ||
                           _view.TransactionType == (int)TransactionTypes.DebtReviewArrangementDebit)
                    {
                        IAccount account = AccRepo.GetAccountByKey(AccountKey);
                        IRuleService svcRule = ServiceFactory.GetService<IRuleService>();
                        svcRule.ExecuteRule(_view.Messages, "AccountNotInDebtCounselling", account);
                        if (!_view.IsValid)
                            return;
                    }

                    PostTransaction();

					if (_view.IsValid)
					{
						tx.VoteCommit();
					}
                }
                catch (Exception)
                {
                    tx.VoteRollBack();

                    if (_view.IsValid)
                        throw;
                }
            }

            if (_view.IsValid)
                _view_OnCancelButtonClicked(null, null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("TransactionArrearView");
        }
    }
}
