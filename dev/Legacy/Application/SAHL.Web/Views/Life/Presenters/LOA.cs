using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Collections;
using SAHL.Common.CacheData;


namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class LOA : SAHLCommonBasePresenter<ILOA>
    {
        private CBOMenuNode _node;
        private IApplicationLife _applicationLife;
        private SAHL.Common.BusinessModel.Interfaces.IMortgageLoanAccount _loanAccount;
        private IMortgageLoan _mortgageLoanVariable;
        private IMortgageLoan _mortgageLoanFixed;
        private IApplicationInformationVariableLoan _applicationInformationVariableLoan;
        private IApplicationInformationVarifixLoan _applicationInformationVarifixLoan;
        private IApplicationMortgageLoan _applicationMortgageLoan;
        private List<string> _loanConditions;
        private IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
        private IConditionsRepository _conditionsRepo = RepositoryFactory.GetRepository<IConditionsRepository>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LOA(ILOA view, SAHLCommonBaseController controller) : base(view, controller)
        {
            // Get the CBO Node 
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


            _view.OnNextButtonClicked += new EventHandler(OnNextButtonClicked);

            // Get the Life Application Object
            _applicationLife = applicationRepo.GetApplicationLifeByKey((int)_node.GenericKey);

            // Get the Life Account
            IAccountLifePolicy accountLifePolicy = _applicationLife.Account as IAccountLifePolicy;

            // Get the Loan Account Object
            _loanAccount = accountLifePolicy.ParentMortgageLoan as IMortgageLoanAccount;

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
                ISupportsVariableLoanApplicationInformation VLI = _applicationMortgageLoan.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                ISupportsVariFixApplicationInformation VFI = _applicationMortgageLoan.CurrentProduct as ISupportsVariFixApplicationInformation;

                // Get OfferInformationVariabelLoan
                if (VLI != null)
                    _applicationInformationVariableLoan = VLI.VariableLoanInformation;

                    // Get OfferInformationVariFixLoan
                if(VFI != null)
                    _applicationInformationVarifixLoan = VFI.VariFixInformation;
           }

            // Get the HOC Object
            IAccountHOC hocAccount = _loanAccount.GetRelatedAccountByType(AccountTypes.HOC, _loanAccount.RelatedChildAccounts) as IAccountHOC;

            // Bind the LOA Details
            _view.BindLOADetails(_applicationLife, _applicationMortgageLoan, _applicationInformationVariableLoan, _applicationInformationVarifixLoan, _mortgageLoanVariable, _mortgageLoanFixed, hocAccount);

            // Get the Loan Conditions
            //_loanConditions = _conditionsRepo.GetLastDisbursedApplicationConditions(_loanAccount.Key);
            if (_loanAccount.CurrentMortgageLoanApplication != null)
                _loanConditions = _conditionsRepo.GetLoanConditions(_loanAccount.CurrentMortgageLoanApplication.Key);

            // Bind the Loan Conditions Grid
            _view.BindLoanConditions(_loanConditions);
        }


        /// <summary>
        /// Handles the event fired by the view when the Next Button is Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnNextButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                // Send email if email text has been entered.
                if (_view.EmailBody.Length > 1)
                {
                    ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                    IMessageService messageService = ServiceFactory.GetService<IMessageService>();

                    string from = lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.LOAMailFrom].ControlText;
                    string to = lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.LOAMailTo].ControlText;
                    string cc = lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.LOAMailCC].ControlText;
                    string subject = lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.LOAMailSubject].ControlText;
                    string body = _view.EmailBody;

                    messageService.SendEmailInternal(from, to, cc, "", subject, body, false);
                }

                // Navigate to the next State
                if (_view.IsValid)
                {
                    X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                    svc.WorkFlowWizardNext(_view.CurrentPrincipal, _view.ViewName, _view.Navigator);
                }
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
    }
}
