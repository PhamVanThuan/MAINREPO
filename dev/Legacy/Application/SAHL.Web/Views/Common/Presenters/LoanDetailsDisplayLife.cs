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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class LoanDetailDisplayLife : SAHLCommonBasePresenter<ILoanDetail>
    {
        private IReadOnlyEventList<IDetail> lstDetail;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LoanDetailDisplayLife(ILoanDetail view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnCancelButtonClicked += null;
            _view.OnSubmitButtonClicked += null;
            _view.OnGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnGridIndexChanged); ;

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode != null)
            {
                int loanAccountKey = -1;
                IAccountLifePolicy accountLifePolicy = null;
                switch (cboNode.GenericKeyTypeKey)
                {
                    case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                        IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                        // get the life account
                        accountLifePolicy = accRepo.GetAccountByKey(cboNode.GenericKey) as IAccountLifePolicy;
                        // get the loan accountkey
                        loanAccountKey = accountLifePolicy.ParentMortgageLoan.Key;
                        break;
                    case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                        IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                        // get the life application
                        IApplication application = appRepo.GetApplicationByKey(cboNode.GenericKey);
                        // get the life account
                        accountLifePolicy = application.Account as IAccountLifePolicy;
                        // get the loan accountkey
                        loanAccountKey = accountLifePolicy.ParentMortgageLoan.Key;
                        break;
                    default:
                        break;
                }

                if (loanAccountKey > 0)
                    SetupDisplay(loanAccountKey);
            }
        }


        #region Private Methods

        private void SetupDisplay(int accountKey)
        {
            IAccountRepository accRepository = RepositoryFactory.GetRepository<IAccountRepository>();

            lstDetail = accRepository.GetDetailByAccountKey(accountKey);

            _view.ShowButtons = false;
            _view.ShowLabels = true;
            _view.DetailGridPostBackType = GridPostBackType.SingleClick;

            if (lstDetail != null && lstDetail.Count > 0)
            {
                _view.BindDetailGrid(lstDetail);
                _view.BindData(lstDetail[0]);
            }
        }

        #endregion


        #region Events Handlers

        void _view_OnGridIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (e != null)
            {
                if (lstDetail != null && lstDetail.Count > Convert.ToInt32(e.Key))
                    _view.BindData(lstDetail[Convert.ToInt32(e.Key)]);
            }
        }

        #endregion


    }
}
