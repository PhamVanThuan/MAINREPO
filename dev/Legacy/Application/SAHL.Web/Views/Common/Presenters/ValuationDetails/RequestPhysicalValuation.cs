using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using System.Data;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{
    public class RequestPhysicalValuation : SAHLCommonBasePresenter<IRequestPhysicalValuation>
    {
        private IReasonRepository _reasonRepo;
        private IAccountRepository _accountRepo;
        private IPropertyRepository _propertyRepo;

        private CBOMenuNode _selectedNode;

        private IMortgageLoanAccount _mlAccount;
        private IPropertyAccessDetails _propertyAccessDetails;
        private IProperty _property;


        private IAccountRepository AccountRepository
        {
            get
            {
                if (_accountRepo == null) _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                return _accountRepo;
            }
        }

        private IReasonRepository ReasonRepository
        {
            get
            {
                if (_reasonRepo == null) _reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();
                return _reasonRepo;
            }
        }

        private IPropertyRepository PropertyRepository
        {
            get
            {
                if (_propertyRepo == null) _propertyRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
                return _propertyRepo;
            }
        }


        public RequestPhysicalValuation(IRequestPhysicalValuation view, SAHLCommonBaseController controller)
            : base(view, controller)
        { }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.SubmitClicked += BtnSubmitClicked;
            _view.CancelClicked += BtnCancelClicked;

            _selectedNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (_selectedNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            int accountKey = _selectedNode.GenericKey;

            _mlAccount = AccountRepository.GetAccountByKey(accountKey) as IMortgageLoanAccount;

            _property = _mlAccount.SecuredMortgageLoan.Property;

            _propertyAccessDetails = _property.PropertyAccessDetails;

            IReadOnlyEventList<IReasonDefinition> reasonDefinitions = ReasonRepository.GetReasonDefinitionsByReasonTypeKey((int)ReasonTypes.RequestPhysicalValuation);
            Dictionary<int, string> reasonsDict = new Dictionary<int, string>();
            foreach (IReasonDefinition definition in reasonDefinitions)
            {
                reasonsDict.Add(definition.Key, definition.ReasonDescription.Description);
            }

            DataTable valuationsData = PropertyRepository.GetValuationsAndReasons(_property.Key);

            _view.BindPropertyAccessDetails(_propertyAccessDetails);
            _view.BindValuations(valuationsData);
            _view.BindReasons(reasonsDict);
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
        }

        private void BtnSubmitClicked(object sender, EventArgs e)
        {
            using (TransactionScope txn = new TransactionScope())
            {
                try
                {
                    CheckRules();
                    Validate();

                    bool isReview = false;
                    string lightstonePropertyId = String.Empty;
                    int valuationKey = 0;

                    ExclusionSets.Add(RuleExclusionSets.ValuationUpdatePreviousToInactive);

                    // Save propert access details
                    UpdatePropertyAccessDetailsFromView();
                    PropertyRepository.SavePropertyAccessDetails(_propertyAccessDetails);

                    // Call LS web svc 
                    PropertyRepository.RequestLightStoneValuation(_propertyAccessDetails, _property, _mlAccount.Key, (int)GenericKeyTypes.Account, isReview, _view.AssessmentDate.Value, _view.SelectedValuationReasonDescription, _view.SpecialInstructions, out lightstonePropertyId);

                    // Save valuation
                    valuationKey = PropertyRepository.CreateValuationLightStone(_property).Key;

                    IReason reason = ReasonRepository.CreateEmptyReason();
                    reason.Comment = _view.SpecialInstructions;
                    reason.GenericKey = valuationKey;
                    reason.ReasonDefinition = ReasonRepository.GetReasonDefinitionByKey(_view.SelectedValuationReasonDefinitionKey);
                    ReasonRepository.SaveReason(reason);

                    txn.VoteCommit();
                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (View.IsValid)
                        throw;
                }
            }

            if (_view.IsValid)
            {
                _view.Navigator.Navigate("ValuationDetailsView");
            }
        }

        private void UpdatePropertyAccessDetailsFromView()
        {
            if (_propertyAccessDetails == null)
                _propertyAccessDetails = PropertyRepository.CreateEmptyPropertyAccessDetails();

            //Store Information in PropertyAccessDetails
            _propertyAccessDetails.Property = _property;
            _propertyAccessDetails.Contact1 = _view.Contact1Name;
            _propertyAccessDetails.Contact1MobilePhone = _view.Contact1MobilePhone;
            _propertyAccessDetails.Contact1Phone = _view.Contact1Phone;
            _propertyAccessDetails.Contact1WorkPhone = _view.Contact1WorkPhone;
            _propertyAccessDetails.Contact2 = _view.Contact2Name;
            _propertyAccessDetails.Contact2Phone = _view.Contact2Phone;
        }


        private void Validate()
        {
            string errorMsg = "";

            if (!View.AssessmentDate.HasValue)
            {
                errorMsg = "Please select an Assessment Date.";
                _view.Messages.Add(new Error(errorMsg, errorMsg));
            }

            if (View.SelectedValuationReasonDefinitionKey == -1)
            {
                errorMsg = "Please select an Assessment Reason.";
                _view.Messages.Add(new Error(errorMsg, errorMsg));
            }

            if (!_view.IsValid) 
            {
                throw new DomainValidationException();
            }
        }


        private void CheckRules()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            svc.ExecuteRule(spc.DomainMessages, "ValuationRequestPending", _property);

            if (!_view.IsValid)
            {
                throw new DomainValidationException();
            }
        }

        private void BtnCancelClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("ValuationDetailsView");
        }

    }

}