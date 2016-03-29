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
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

using SAHL.Common.Globals;
using System.Collections;
using System.Collections.Generic;
using SAHL.Common.DomainMessages;
using SAHL.Common.UI;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class LastRateChange : SAHLCommonBasePresenter<ILastRateChange>
    {
        private CBONode _cboNode;
        private IAccount _account;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LastRateChange(ILastRateChange view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal);//, CBONodeSetType.CBO);
            if (_cboNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            if (_cboNode.GenericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.Account)
            {
                IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                _account = accountRepo.GetAccountByKey(_cboNode.GenericKey);
                IMortgageLoanAccount mla = _account as IMortgageLoanAccount;
                if (mla == null)
                {
                    _view.Messages.Add(new Error("This is not a Loan Account.", "This is not a Loan Account."));
                    return;
                }

                IList<IMortgageLoan> mortgageLoanList = new List<IMortgageLoan>();
                IMortgageLoan _vML = mla.SecuredMortgageLoan;
                mortgageLoanList.Add(_vML);

                IMortgageLoan _fML = null;

                if ((_account as IAccountVariFixLoan) != null)
                {
                    IAccountVariFixLoan _fAccount = _account as IAccountVariFixLoan;
                    _fML = _fAccount.FixedSecuredMortgageLoan;
                    mortgageLoanList.Add(_fML);
                }

                _view.BindGrid(mortgageLoanList);
            }
        }
    }
}

