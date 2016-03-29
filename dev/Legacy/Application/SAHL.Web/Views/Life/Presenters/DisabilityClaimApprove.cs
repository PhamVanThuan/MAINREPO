using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.Life.Presenters
{
    public class DisabilityClaimApprove : SAHLCommonBasePresenter<IDisabilityClaimApprove>
    {
        private IAccountRepository accountRepo;
        private DisabilityClaimDetailModel disabilityClaim;
        private IMortgageLoanAccount loanAccount;
        private DateTime disbilityPaymentStartDate;
        private int loanDebitOrderDay;
        private IV3ServiceManager v3ServiceManager;
        private IMortgageLoanDomainService mortgageLoanDomainService;
        private ILifeDomainService lifeDomainService;

        public DisabilityClaimApprove(IDisabilityClaimApprove view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            _view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);

            InstanceNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            v3ServiceManager = V3ServiceManager.Instance;
            mortgageLoanDomainService = v3ServiceManager.Get<IMortgageLoanDomainService>();
            lifeDomainService = v3ServiceManager.Get<ILifeDomainService>();

            GetDisabilityClaimByKeyQuery getDisabilityClaimByKeyQuery = new GetDisabilityClaimByKeyQuery(node.GenericKey);
            ISystemMessageCollection systemMessageCollection = lifeDomainService.PerformQuery(getDisabilityClaimByKeyQuery);

            if (!systemMessageCollection.HasErrors)
            {
                disabilityClaim = getDisabilityClaimByKeyQuery.Result.Results.FirstOrDefault();
            }
            else
            {
                v3ServiceManager.HandleSystemMessages(systemMessageCollection);
                return;
            }

            accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            IAccountLifePolicy account = accountRepo.GetAccountByKey(disabilityClaim.LifeAccountKey) as IAccountLifePolicy;
            loanAccount = account.ParentMortgageLoan as IMortgageLoanAccount;

            loanDebitOrderDay = mortgageLoanDomainService.GetDebitOrderDayByAccount(loanAccount.Key);

            disbilityPaymentStartDate = CalculatePaymentStartDate(disabilityClaim.LastDateWorked, loanDebitOrderDay);

            _view.DateLastWorked = disabilityClaim.LastDateWorked.Value;
            _view.LoanDebitOrderDay = loanDebitOrderDay;
            _view.DisbilityPaymentStartDate = disbilityPaymentStartDate;

            GetFurtherLendingExclusionsByDisabilityClaimKeyQuery query = new GetFurtherLendingExclusionsByDisabilityClaimKeyQuery(disabilityClaim.DisabilityClaimKey);
            systemMessageCollection = SystemMessageCollection.Empty();
            systemMessageCollection = lifeDomainService.PerformQuery(query);

            if (!systemMessageCollection.HasErrors)
            {
                _view.BindFurtherLendingExclusions(query.Result.Results);
            }
            else
            {
                v3ServiceManager.HandleSystemMessages(systemMessageCollection);
            }
        }

        private void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            ValidateScreenInput();

            if (_view.IsValid == false)
                return;

            try
            {
                ISystemMessageCollection systemMessageCollection;

                ApproveDisabilityClaimCommand approveDisabilityClaimCommand = new ApproveDisabilityClaimCommand(disabilityClaim.DisabilityClaimKey, loanAccount.Key, disbilityPaymentStartDate, _view.NoOfInstalmentsAuthorised.Value, _view.DisbilityPaymentEndDate.Value);
                systemMessageCollection = lifeDomainService.PerformCommand(approveDisabilityClaimCommand);

                if (!systemMessageCollection.HasErrors)
                {
                    X2ServiceResponse response = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);

                    if (response.IsError)
                        throw new Exception("Error Completing Disability Claim Approve");

                    if (!systemMessageCollection.HasErrors && _view.IsValid)
                    {
                        base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                    }
                }
                else
                {
                    v3ServiceManager.HandleSystemMessages(systemMessageCollection);
                }
            }
            catch (Exception)
            {
                if (_view.IsValid)
                {
                    base.X2Service.CancelActivity(_view.CurrentPrincipal);
                    throw;
                }
            }
        }


        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            X2Service.CancelActivity(_view.CurrentPrincipal);
            X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        private void ValidateScreenInput()
        {
            string errorMessage = "";

            if (!_view.NoOfInstalmentsAuthorised.HasValue || _view.NoOfInstalmentsAuthorised.Value <= 0)
            {
                errorMessage = "No. of Authorised Disability Instalments must be entered.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (!_view.DisbilityPaymentEndDate.HasValue)
            {
                errorMessage = "Disability Payment End Date cannot be empty.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }
        }

        private DateTime CalculatePaymentStartDate(DateTime? lastDateWorked, int debitOrderDay)
        {
            // if the day of the last day worked is after the debit order day - add 4 months
            // if the day of the last day worked is before the debit order day - add 3 months
            // the day becomes the debit order day
            int monthsToAdd = 4;
            if (lastDateWorked.Value.Day <= debitOrderDay)
                monthsToAdd = 3;

            DateTime paymentStartDate = lastDateWorked.Value.AddMonths(monthsToAdd);

            return new DateTime(paymentStartDate.Year, paymentStartDate.Month, debitOrderDay);
        }


    }
}