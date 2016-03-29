using System;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.ReleaseAndVariations
{
    public partial class WorkFlowHeader : System.Web.UI.UserControl
    {
        private InstanceNode _instanceNode;
        private CBONode _cboNode;
        private IApplication _offer;
        //private ICBOService CBOService;
        private IViewBase _view;
        private IReadOnlyEventList<IFinancialService> _financialServices;
        private IMortgageLoan _mortgageLoanVariableFS;
        ///// <summary>
        ///// 
        ///// </summary>
        //protected PresenterData PrivateCacheData
        //{
        //    get
        //    {
        //        SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
        //        return spc.GetPresenterData(_view.ViewName);
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //protected GlobalData GlobalCacheData
        //{
        //    get
        //    {
        //        SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
        //        return SPC.GetGlobalData();
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _view = Page as IViewBase;

            //CBOManager = ServiceFactory.GetService<ICBOService>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _cboNode = _view.CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2);

            if (_cboNode == null)
            {
                return;
            }

            if (_cboNode is InstanceNode)
            {
                //_instanceNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
                _instanceNode = _cboNode as InstanceNode;

                IApplicationRepository OfferRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                _offer = OfferRepo.GetApplicationByKey(_instanceNode.GenericKey);

                //IAccount _mortgageloanAccount = _offer.Account.GetRelatedAccountByType(AccountTypes.MortgageLoan, _offer.Account.RelatedParentAccounts);
                IAccount _mortgageloanAccount = _offer.Account.ParentAccount;
                
                // TODO: CRAIGF please check the accountstatuses below
                _financialServices = _mortgageloanAccount.GetFinancialServicesByType(FinancialServiceTypes.VariableLoan, new AccountStatuses[] { AccountStatuses.Open, AccountStatuses.Dormant});
                if (_financialServices != null && _financialServices.Count > 0)
                    _mortgageLoanVariableFS = _financialServices[0] as IMortgageLoan;

                lblClientName.Text = _mortgageloanAccount.GetLegalName(LegalNameFormat.InitialsOnly);
                lblLoanNumber.Text = _mortgageloanAccount.Key.ToString();

                if (_mortgageLoanVariableFS != null)
                {
                    lblLoanNumber.Text += " (" + _mortgageLoanVariableFS.MortgageLoanPurpose.Description + ")";
                }
            }
        }
    }
}