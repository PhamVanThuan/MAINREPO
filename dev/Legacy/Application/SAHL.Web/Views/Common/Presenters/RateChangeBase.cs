using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Collections;
using SAHL.Common.Security;
using System;

using SAHL.Common.UI;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.CacheData;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// Presenter for RateChangeBase presenter 
    /// </summary>
    public class RateChangeBase : SAHLCommonBasePresenter<IRateChange>
    {
        /// <summary>
        /// account interface 
        /// </summary>
        protected IAccount _account;
        /// <summary>
        /// Account Repository
        /// </summary>
        protected IAccountRepository _accRepo;

        /// <summary>
        /// Used in Tests
        /// </summary>
        public IAccountRepository AccountRepo
        {
            get { return _accRepo; }
            set { _accRepo = value; }
        }

        public int MaximumTerm;
        private IControlRepository controlrepository;
        /// <summary>
        /// Gets an Instance of the control Repository
        /// </summary>
        public IControlRepository ControlRepository
        {
            get
            {
                if (controlrepository == null)
                    controlrepository = RepositoryFactory.GetRepository<IControlRepository>();
                return controlrepository;
            }
        }
	
        /// <summary>
        /// List of Financial Service Type of Variable Loan
        /// </summary>
        protected IReadOnlyEventList<IFinancialService> _mlVarLst;
        /// <summary>
        /// List of Financial Service Type of Fixed Loan
        /// </summary>
        protected IReadOnlyEventList<IFinancialService> _mlFixedLst;
        /// <summary>
        /// Variable portion of a Mortgage Loan
        /// </summary>
        protected IMortgageLoan _mlVar;
        /// <summary>
        /// Fixed portion of a Mortgage Loan
        /// </summary>
        protected IMortgageLoan _mlFixed;
        /// <summary>
        /// Mortgage Loan Repository
        /// </summary>
        protected IMortgageLoanRepository _mlRepo;
        /// <summary>
        /// Current logged on user
        /// </summary>
        protected SAHLPrincipal _principal;
        /// <summary>
        /// List of Mortgage Loans - holds the variable and fixed Mortgage Loan records
        /// </summary>
        protected IEventList<IMortgageLoan> _lstMortgageLoans;
        /// <summary>
        /// Life Policy - used to get life balance and process life update transactions
        /// </summary>
        protected IAccountLifePolicy _lifePolicyAccount;
        protected IAccountHOC _hocAccount;
        
        /// <summary>
        /// CBO Node
        /// </summary>
        /// <summary>
        /// CBO Menu Node
        /// </summary>
        protected CBOMenuNode _node;
       

        /// <summary>
        /// Constructor for RatechangeBase
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public RateChangeBase(IRateChange view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }
        /// <summary>
        /// OnView Initialised event - retrieve data for use by presenters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _account = _accRepo.GetAccountByKey(Convert.ToInt32(_node.GenericKey));

            if (_account != null)
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                svc.ExecuteRule(spc.DomainMessages, "AccountDebtCounseling", _account);
                svc.ExecuteRule(spc.DomainMessages, "LegalEntitiesUnderDebtCounsellingForAccount", _account);
                
                IMortgageLoanAccount MLLife = _account as IMortgageLoanAccount;
                if (MLLife != null)
                {
                    _lifePolicyAccount = MLLife.LifePolicyAccount;
                    _hocAccount = MLLife.HOCAccount;
                }
 
                _mlVarLst = _account.GetFinancialServicesByType(FinancialServiceTypes.VariableLoan, new AccountStatuses[] { AccountStatuses.Open });
                _mlFixedLst = _account.GetFinancialServicesByType(FinancialServiceTypes.FixedLoan, new AccountStatuses[] { AccountStatuses.Open });
            }

            _mlVar = _mlVarLst[0] as IMortgageLoan;
            if (_mlFixedLst.Count > 0 && _mlFixedLst != null)
                _mlFixed = _mlFixedLst[0] as IMortgageLoan;
        
            _lstMortgageLoans = new EventList<IMortgageLoan>();

            IControl ctrl = ControlRepository.GetControlByDescription("Calc - maxTerm");
            MaximumTerm = Convert.ToInt32(ctrl.ControlNumeric);
            View.MaximumTerm = MaximumTerm;

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (_view.Messages.ErrorMessages.Count > 0)
            {
                //disable the submit button
                _view.SetAbilityofSubmitButton = false;
            }
        }
    }
}
