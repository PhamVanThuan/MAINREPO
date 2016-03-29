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
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;


namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class Account : SAHLCommonBasePresenter<SAHL.Web.Views.Life.Interfaces.IAccount>
    {
        private CBOMenuNode _node;
        private IAccountLifePolicy _lifePolicyAccount;
        private IApplication _lifePolicyApplication;
        private IReadOnlyEventList<ILegalEntity> _lstLegalEntities;
        private SAHL.Common.BusinessModel.Interfaces.IMortgageLoanAccount _loanAccount;
        private IApplicationMortgageLoan _applicationMortgageLoan;
        private IApplicationInformationVariableLoan _applicationInformationVariableLoan;
        private IApplicationInformationVarifixLoan _applicationInformationVarifixLoan;
        private IMortgageLoan _mortgageLoanVariable;
        private IMortgageLoan _mortgageLoanFixed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public Account(SAHL.Web.Views.Life.Interfaces.IAccount view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo) == true)
                GlobalCacheData.Remove(ViewConstants.NavigateTo);

            GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, new List<ICacheObjectLifeTime>());

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;


            // Get the Life Application Object
            IApplicationRepository ApplicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _lifePolicyApplication = ApplicationRepo.GetApplicationByKey((int)_node.GenericKey);

            // Get the Life Account
            _lifePolicyAccount = _lifePolicyApplication.Account as IAccountLifePolicy;

            // Get the Loan Account
            _loanAccount = _lifePolicyAccount.ParentMortgageLoan as IMortgageLoanAccount; 

            // Get Variable leg of Mortgage Loan
            if (_loanAccount != null)
                _mortgageLoanVariable = _loanAccount.SecuredMortgageLoan;
            else
                throw new Exception(StaticMessages.NoVariableMortageLoan);

            // Get Fixed leg of Mortgage Loan
            IAccountVariFixLoan varifixLoanAccount = _loanAccount as IAccountVariFixLoan;
            if (varifixLoanAccount != null)
                _mortgageLoanFixed = varifixLoanAccount.FixedSecuredMortgageLoan;

            // Get the Loan Application Object
            _applicationMortgageLoan = _loanAccount.CurrentMortgageLoanApplication;

            if (_applicationMortgageLoan != null)
            {
                // Get OfferInformationVariabelLoan
                ISupportsVariableLoanApplicationInformation VLI = _applicationMortgageLoan.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                ISupportsVariFixApplicationInformation VFI = _applicationMortgageLoan.CurrentProduct as ISupportsVariFixApplicationInformation;
                if (VLI != null)
                    _applicationInformationVariableLoan = VLI.VariableLoanInformation;
                if (VFI != null)
                    _applicationInformationVarifixLoan = VFI.VariFixInformation;
            }

            // Get the role plyers on Loan Account
            _lstLegalEntities = _loanAccount.GetLegalEntitiesByRoleType(_view.Messages, new int[] { (int)SAHL.Common.Globals.RoleTypes.MainApplicant, (int)SAHL.Common.Globals.RoleTypes.Suretor });

            // Bind the Applicants Grid
            _view.BindApplicantGrid(_lstLegalEntities,_loanAccount.Key);

            // Bind the Loan Summary Controls
            _view.BindLoanSummaryControls(_loanAccount, _applicationMortgageLoan, _applicationInformationVariableLoan, _applicationInformationVarifixLoan, _mortgageLoanVariable, _mortgageLoanFixed);
        }
    }
}
