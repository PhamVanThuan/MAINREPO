using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.V3.Framework;
using SAHL.V3.Framework.DomainServices;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.X2.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.Life.Presenters
{
    public class DisabilityClaimCreate : SAHLCommonBasePresenter<IDisabilityClaimCreate>
    {
        private CBOMenuNode cboNode;
        private IAccountRepository accountRepo;
        private IAccountLifePolicy accountLifePolicy;
        private IMortgageLoanAccount loanAccount;
        private IV3ServiceManager v3ServiceManager;
        private IMortgageLoanDomainService mortgageLoanDomainService;
        private ILifeDomainService lifeDomainService;

        public DisabilityClaimCreate(IDisabilityClaimCreate view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            v3ServiceManager = V3ServiceManager.Instance;
            mortgageLoanDomainService = v3ServiceManager.Get<IMortgageLoanDomainService>();
            lifeDomainService = v3ServiceManager.Get<ILifeDomainService>();

            _view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);

            accountLifePolicy = accountRepo.GetAccountByKey(cboNode.GenericKey) as IAccountLifePolicy;
            loanAccount = accountLifePolicy.ParentMortgageLoan as IMortgageLoanAccount;

            PopulateScreenFields();

            IReadOnlyEventList<ILegalEntity> assuredLives = accountLifePolicy.GetLegalEntitiesByRoleType(_view.Messages, new int[] { (int)SAHL.Common.Globals.RoleTypes.AssuredLife });
            _view.BindAssuredLivesGrid(assuredLives);
            _view.BindClaimants(assuredLives);
        }

        private void PopulateScreenFields()
        {
            double bondInstalment = 0, ipbAmount = 0, ipBenefitRetention = 0, retainedIPBSumAssured = 0;

            IApplicationLife applicationLife = accountLifePolicy.CurrentLifeApplication;
            ILifePolicy lifePolicy = accountLifePolicy.LifePolicy;

            #region Loan Data

            IMortgageLoan mortgageLoanVariableFS = loanAccount.SecuredMortgageLoan;
            IMortgageLoan mortgageLoanFixedFS = null;
            // see if we have a fixed portion
            IAccountVariFixLoan varifixLoanAccount = loanAccount as IAccountVariFixLoan;
            if (varifixLoanAccount != null)
                mortgageLoanFixedFS = varifixLoanAccount.FixedSecuredMortgageLoan;

            _view.LoanNumber = loanAccount.Key;
            _view.LoanStatus = loanAccount.AccountStatus.Description;
            _view.LoanTerm = mortgageLoanVariableFS.InitialInstallments;

            IMortgageLoanAccount mortgageLoanAccount = loanAccount as IMortgageLoanAccount;
            IApplicationMortgageLoan applicationMortgageLoan = mortgageLoanAccount.CurrentMortgageLoanApplication;
            IApplicationInformationVariableLoan applicationInformationVariableLoan = null;
            if (applicationMortgageLoan != null)
            {
                ISupportsVariableLoanApplicationInformation VLI = applicationMortgageLoan.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                if (VLI != null)
                    applicationInformationVariableLoan = VLI.VariableLoanInformation;
            }

            _view.LoanAmount = applicationInformationVariableLoan != null && applicationInformationVariableLoan.BondToRegister.HasValue ? Convert.ToDouble(applicationInformationVariableLoan.BondToRegister) : 0;

            _view.DebitOrderDay = mortgageLoanDomainService.GetDebitOrderDayByAccount(loanAccount.Key);

            _view.BondInstalment = bondInstalment = mortgageLoanFixedFS != null && mortgageLoanFixedFS.Payment > 0 ? mortgageLoanFixedFS.Payment + mortgageLoanVariableFS.Payment : mortgageLoanVariableFS.Payment;
            _view.OutstandingBondAmount = loanAccount.LoanCurrentBalance;

            #endregion Loan Data

            #region LifePolicy Data

            _view.PolicyNumber = accountLifePolicy.Key;

            if (lifePolicy != null) // Get the data from the LifePolicy table
            {
                _view.PolicyType = lifePolicy.LifePolicyType.Description;
                _view.PolicyStatus = lifePolicy.LifePolicyStatus.Description;
                _view.DateOfAcceptance = lifePolicy.DateOfAcceptance;
                _view.CommencementDate = lifePolicy.DateOfCommencement;

                _view.CurrentSumAssured = lifePolicy.CurrentSumAssured.HasValue ? lifePolicy.CurrentSumAssured.Value : 0;

                ipbAmount = bondInstalment;
                ipBenefitRetention = lifePolicy.IPBReassuranceRetention.HasValue ? lifePolicy.IPBReassuranceRetention.Value : 0;
                retainedIPBSumAssured = ipbAmount > 0 && ipbAmount < ipBenefitRetention ? ipbAmount : ipBenefitRetention;
                _view.ReassuredIPBAmount = ipbAmount - retainedIPBSumAssured;
            }
            else // Get the data from the applicationLife table
            {
                _view.PolicyType = applicationLife.LifePolicyType.Description;
                _view.PolicyStatus = "Application";
                _view.DateOfAcceptance = null;
                _view.CommencementDate = null;

                _view.CurrentSumAssured = applicationLife.CurrentSumAssured.HasValue ? applicationLife.CurrentSumAssured.Value : 0;
                _view.ReassuredIPBAmount = 0;
            }

            #endregion LifePolicy Data
        }

        private void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (!ValidateSelection())
                return;

            try
            {
                long instanceId = 0;
                bool error = lifeDomainService.CreateClaim(accountLifePolicy.Key, _view.SelectedLegalEntityKey, out instanceId);
                if (!error)
                {
                    // add the instanceID to the global cache for our redirect view to use
                    GlobalCacheData.Remove(ViewConstants.InstanceID);
                    GlobalCacheData.Add(ViewConstants.InstanceID, instanceId, new List<ICacheObjectLifeTime>());

                    // navigate to the workflow redirect view
                    Navigator.Navigate("Worklist");
                }
            }
            catch (Exception)
            {
                if (_view.IsValid)
                {
                    throw;
                }
            }
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        private bool ValidateSelection()
        {
            bool valid = true;

            if (_view.SelectedLegalEntityKey <= 0)
            {
                _view.Messages.Add(new Error("Must select a Claimant.", "Must select a Claimant."));
                valid = false;
            }

            return valid;
        }
    }
}