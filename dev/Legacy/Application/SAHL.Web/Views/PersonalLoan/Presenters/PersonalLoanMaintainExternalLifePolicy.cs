using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanMaintainExternalLifePolicy : SAHLCommonBasePresenter<IPersonalLoanMaintainLifePolicy>
    {
        private int _applicationKey;
        private IApplicationUnsecuredLendingRepository _applicationUnsecuredLendingRepository;
        private IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository
        {
            get
            {
                if (_applicationUnsecuredLendingRepository == null)
                {
                    _applicationUnsecuredLendingRepository = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
                }
                return _applicationUnsecuredLendingRepository;
            }
        }

        private IApplicationRepository _applicationRepository;
        public IApplicationRepository applicationRepository
        {
            get
            {
                if (_applicationRepository == null)
                {
                    _applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
                }
                return _applicationRepository;
            }
        }

        private IRuleService _ruleService;
        private IRuleService ruleService
        {
            get
            {
                if (_ruleService == null)
                {
                    _ruleService = ServiceFactory.GetService<IRuleService>();
                }
                return _ruleService;
            }
        }

        private ILookupRepository _lookupRepository;
        private ILookupRepository lookupRepository 
        { 
            get
            {
                if(_lookupRepository == null)
                {
                    _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                }
                return _lookupRepository;
                } 
        }
           
        private IDictionary<string, string> lifePolicyStatuses;
        private IDictionary<string, string> insurers;

        public PersonalLoanMaintainExternalLifePolicy(IPersonalLoanMaintainLifePolicy view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
            {
                return;
            }

            var node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _applicationKey = node.GenericKey;

            this.View.OnCancelButtonClicked += View_OnCancelButtonClicked;
            this.View.OnSubmitButtonClicked += View_OnSubmitButtonClicked;
            this.View.OnPolicyStatusSelectedIndexChanged += View_OnPolicyStatusSelectedIndexChanged;

            lifePolicyStatuses = lookupRepository.LifePolicyStatuses.BindableDictionary.Where(x => x.Key == "3" || x.Key == "12").ToDictionary(x => x.Key, y => y.Value);
            this.View.BindStatus(lifePolicyStatuses);

            insurers = lookupRepository.Insurers.BindableDictionary.Where(i => (!(i.Key.Equals("1", StringComparison.InvariantCulture)))).ToDictionary(k => k.Key, v => v.Value);
            this.View.BindInsurers(insurers);

            if (_applicationKey > 0)
            {
                var applicationUnsecuredLending = applicationUnsecuredLendingRepository.GetApplicationByKey(_applicationKey);
                var latestAcceptedOfferInformation = applicationUnsecuredLending.GetLatestApplicationInformation();
                if (latestAcceptedOfferInformation == null)
                {
                    return;
                }

                var applicationProductPersonalLoan = latestAcceptedOfferInformation.ApplicationProduct as IApplicationProductPersonalLoan;
                var externalLifePolicy = applicationProductPersonalLoan.ExternalLifePolicy;
                if (externalLifePolicy != null)
                {
                    this.View.BindMaintainLifePolicyForReadWrite(externalLifePolicy);
                }
            }
        }

        private void View_OnPolicyStatusSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (PolicyStatusIsSetToClosed(e.Key.ToString()))
            {
                _view.ResetCeded();
            }
            else
            {
                _view.ResetCloseDate();
            }
        }

        private void View_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            ValidateScreenInput();

            if (_view.IsValid == false)
                return;

            if (_applicationKey > 0)
            {
                using (var transactionScope = new TransactionScope(OnDispose.Rollback))
                {
                    try
                    {
                        var applicationUnsecuredLending = applicationRepository.GetApplicationByKey(_applicationKey) as IApplicationUnsecuredLending;
                        if (applicationUnsecuredLending == null)
                            return;

                        var latestAcceptedOfferInformation = applicationUnsecuredLending.GetLatestApplicationInformation();
                        if (latestAcceptedOfferInformation == null)
                            return;

                        var applicationProductPersonalLoan = latestAcceptedOfferInformation.ApplicationProduct as IApplicationProductPersonalLoan;

                        var externalLifePolicy = applicationProductPersonalLoan.ExternalLifePolicy;
                        if (externalLifePolicy == null)
                        {
                            externalLifePolicy = applicationUnsecuredLendingRepository.GetEmptyExternalLifePolicy();
                            applicationProductPersonalLoan.ExternalLifePolicy = externalLifePolicy;
                        }

                        PopulateExternalPolicyWithViewData(externalLifePolicy);
                        applicationProductPersonalLoan.ExternalLifePolicy.LegalEntity = applicationUnsecuredLending.ActiveClients.First();

                        applicationUnsecuredLending.ValidateEntity();

                        if (!_view.IsValid)
                            return;

                        var sumInsured = applicationProductPersonalLoan.ExternalLifePolicy.SumInsured;
                        ruleService.ExecuteRule(_view.Messages, "CheckCededAmountCoversApplicationAmount", new object[] { _applicationKey, sumInsured });

                        applicationRepository.SaveApplication(applicationUnsecuredLending);

                        if (_view.IsValid)
                        {
                            transactionScope.VoteCommit();
                        }
                        _view.Navigator.Navigate("Save");
                    }
                    catch
                    {
                        transactionScope.VoteRollBack();
                        if (_view.IsValid)
                            throw;
                    }
                }
            }
        }

        private void PopulateExternalPolicyWithViewData(IExternalLifePolicy externalLifePolicy)
        {
            var insurer = lookupRepository.Insurers.FirstOrDefault(i => i.Key == Convert.ToInt32(_view.Insurer));
            if (insurer == null)
                return;
            externalLifePolicy.Insurer = insurer;

            externalLifePolicy.PolicyNumber = _view.PolicyNumber.Trim();

            var lifePolicyStatus = lookupRepository.LifePolicyStatuses.FirstOrDefault(i => i.Key == Convert.ToInt32(_view.Status));
            if (lifePolicyStatus == null)
                return;
            externalLifePolicy.LifePolicyStatus = lifePolicyStatus;

            externalLifePolicy.CommencementDate = _view.CommencementDate.Value;
            externalLifePolicy.PolicyCeded = _view.PolicyCeded;
            externalLifePolicy.SumInsured = _view.SumInsured;

            if (_view.CloseDate.HasValue)
                externalLifePolicy.CloseDate = _view.CloseDate.Value;
            else
                externalLifePolicy.CloseDate = null;
        }

        private void ValidateScreenInput()
        {
            string errorMessage = "";

            int insurerKey;
            if (!int.TryParse(_view.Insurer, out insurerKey))
            {
                errorMessage = "Insurer name must be selected.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (string.IsNullOrEmpty(_view.PolicyNumber))
            {
                errorMessage = "Policy number must be entered.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (!_view.CommencementDate.HasValue)
            {
                errorMessage = "Commencement Date must be entered.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            int statusKey;
            if (!int.TryParse(_view.Status, out statusKey))
            {
                errorMessage = "Policy Status must be selected.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (_view.SumInsured <= 0)
            {
                errorMessage = "Sum insured value is not valid.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (PolicyStatusIsSetToClosed(_view.Status) && _view.CloseDate.HasValue == false)
            {
                errorMessage = "Close Date should be entered for a closed policy.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }
            else if (!PolicyStatusIsSetToClosed(_view.Status) && _view.CloseDate.HasValue)
            {
                errorMessage = "Close Date is not valid for inforce policy.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }
        }

        private bool PolicyStatusIsSetToClosed(string statusKey)
        {
            string statusValue;
            if (!lifePolicyStatuses.TryGetValue(statusKey, out statusValue))
            {
                statusValue = string.Empty;
            }
            return statusValue.Equals("Closed", StringComparison.InvariantCultureIgnoreCase);
        }

        private void View_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("PL_ExternalLifePolicySummary");
        }
    }
}