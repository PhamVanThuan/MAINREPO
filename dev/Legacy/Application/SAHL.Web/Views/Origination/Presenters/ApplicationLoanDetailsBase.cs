using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationLoanDetailsBase : SAHLCommonBasePresenter<IApplicationLoanDetails>
    {
        protected CBOMenuNode _node;
        protected IApplicationMortgageLoan _application;
        protected IApplicationRepository _applicationRepository;
        protected IFinancialAdjustmentRepository _fadjRepo;
        private int _genericKey;
        private ILookupRepository _lookupRepository;
        private IReasonRepository _reasonRepo;
        private IRuleService _ruleServ;
        private IControlRepository _ctrlRepo;

        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }

        public ApplicationLoanDetailsBase(IApplicationLoanDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.ApplicationOptionsChange += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(ApplicationOptionsChange);
            _view.OnCancelClicked += new EventHandler(OnCancelClicked);
            _view.OnCalculateMaximumFixedPercentage += new EventHandler(OnCalculateMaximumFixedPercentage);
            _view.OverrideFeesChange += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(OverrideFeesChange);
            _view.OnRecalculateApplication += new EventHandler(OnRecalculateApplication);
            _view.CapitaliseFeesChange += new KeyChangedEventHandler(CapitiliseFeesChange);
            _view.OnQCDeclineReasons += new EventHandler(OnQCDeclineReasons);
            _view.QuickPayFeeCheckedChange += new KeyChangedEventHandler(_view_QuickPayFeeCheckedChange);
            _view.CapitaliseInitiationFeesChange += new KeyChangedEventHandler(CapitaliseInitiationFeesChange);
        }

        private void OnQCDeclineReasons(object sender, EventArgs e)
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.Application))
                GlobalCacheData.Remove(ViewConstants.Application);

            GlobalCacheData.Add(ViewConstants.Application, _application, new List<ICacheObjectLifeTime>());

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
                GlobalCacheData.Remove(ViewConstants.NavigateTo);

            GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, new List<ICacheObjectLifeTime>());

            Navigator.Navigate("Orig_QCDecline");
        }

        protected virtual void _view_QuickPayFeeCheckedChange(object sender, KeyChangedEventArgs e)
        {
            if (_application == null)
                return;

            // Check if a Quick Pay Loan then apply Quick Pay Loan Attribute so the calc can apply correct fees
            if (!_view.IsQuickPayFeeReadOnly)
                SetApplicationAttribute(OfferAttributeTypes.QuickPayLoan, _view.QuickPayFeeChecked);

            Recalculate();
        }

        protected virtual void CapitiliseFeesChange(object sender, KeyChangedEventArgs e)
        {
            if (_application == null)
                return;

            Recalculate();
        }

        protected virtual void CapitaliseInitiationFeesChange(object sender, KeyChangedEventArgs e)
        {
            if (_application == null)
                return;

            Recalculate();
        }

        protected virtual void OnRecalculateApplication(object sender, EventArgs e)
        {
            Recalculate();
        }

        private void Recalculate()
        {
            _view.GetApplicationDetails(_application);

            // Check if a Quick Pay Loan then apply Quick Pay Loan Attribute so the calc can apply correct fees
            if (!_view.IsQuickPayFeeReadOnly)
                SetApplicationAttribute(OfferAttributeTypes.QuickPayLoan, _view.QuickPayFeeChecked);

            //errors could have been loaded in the previous call
            if (!_view.IsValid || !PassValidation(_application))
                return;

            // Recalc
            _application.CalculateApplicationDetail(_view.IsBondToRegisterExceptionAction, false);

            //Display selected product stuff
            SetupProduct(_application.CurrentProduct.ProductType);

            RunRules();

            if (!_view.IsValid)
                return;

            // Rebind ...
            _view.BindApplicationDetails(_application);
        }

        private void RunRules()
        {
            //Run Product specific rules
            RuleServ.ExecuteRule(_view.Messages, "ProductSuperLoNewCat1", _application);
            RuleServ.ExecuteRule(_view.Messages, "ProductVarifixApplicationMinLoanAmount", _application);
            RuleServ.ExecuteRule(_view.Messages, "ProductVarifixApplicationFixedMinimum", _application);
            RuleServ.ExecuteRule(_view.Messages, "AlphaHousingLoanMustBeNewVariableLoan", _application);
            RuleServ.ExecuteRule(_view.Messages, "AlphaHousingLoanMustNotBeInterestOnlyLoan", _application);
            RuleServ.ExecuteRule(_view.Messages, Rules.ApplicationProductEdgeLTVWarning, _application);
            RuleServ.ExecuteRule(_view.Messages, Rules.CapitaliseInitiationFeeLTV, _application);
        }

        protected bool PassValidation(IApplicationMortgageLoan app)
        {
            IApplicationProductMortgageLoan appProduct = app.CurrentProduct as IApplicationProductMortgageLoan;
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = appProduct as ISupportsVariableLoanApplicationInformation;
            IApplicationInformationVariableLoan aivl = supportsVariableLoanApplicationInformation.VariableLoanInformation;

            double propertyValue = (aivl.PropertyValuation.HasValue ? aivl.PropertyValuation.Value : 0.0);
            double householdIncome = aivl.HouseholdIncome.HasValue ? aivl.HouseholdIncome.Value : 0.0;

            double purchasePrice = 0;
            double cashDeposit = 0;
            double cashOut = 0;
            double existingLoan = 0;
            double cancellationFee = 0;

            double loanAmount = 0;

            switch ((OfferTypes)app.ApplicationType.Key)
            {
                case OfferTypes.NewPurchaseLoan:
                    IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = app as IApplicationMortgageLoanNewPurchase;
                    purchasePrice = applicationMortgageLoanNewPurchase.PurchasePrice.HasValue ? applicationMortgageLoanNewPurchase.PurchasePrice.Value : 0;
                    cashDeposit = applicationMortgageLoanNewPurchase.CashDeposit.HasValue ? applicationMortgageLoanNewPurchase.CashDeposit.Value : 0;

                    if (purchasePrice < propertyValue || propertyValue == 0)
                        propertyValue = purchasePrice;

                    loanAmount = purchasePrice - cashDeposit;

                    break;
                case OfferTypes.RefinanceLoan:
                    IApplicationMortgageLoanRefinance applicationMortgageLoanRefinance = app as IApplicationMortgageLoanRefinance;
                    cashOut = applicationMortgageLoanRefinance.CashOut.HasValue ? applicationMortgageLoanRefinance.CashOut.Value : 0;

                    loanAmount = cashOut;

                    break;
                case OfferTypes.SwitchLoan:
                    IApplicationMortgageLoanSwitch applicationMortgageLoanSwitch = app as IApplicationMortgageLoanSwitch;
                    existingLoan = applicationMortgageLoanSwitch.ExistingLoan.HasValue ? applicationMortgageLoanSwitch.ExistingLoan.Value : 0;
                    cashOut = applicationMortgageLoanSwitch.CashOut.HasValue ? applicationMortgageLoanSwitch.CashOut.Value : 0;
                    cancellationFee = applicationMortgageLoanSwitch.CancellationFee;

                    loanAmount = existingLoan + cashOut + cancellationFee;

                    break;
                default:
                    break;
            }

            //Checks fail for Further lending, exclude for now till new screens/presenters are in place so we can get some stuff through
            if (app.ApplicationType.Key == (int)OfferTypes.FurtherAdvance || app.ApplicationType.Key == (int)OfferTypes.FurtherLoan || app.ApplicationType.Key == (int)OfferTypes.ReAdvance)
                return true;

            if (aivl.EmploymentType == null)
                aivl.EmploymentType = LookupRepository.EmploymentTypes.ObjectDictionary[((int)EmploymentTypes.Salaried).ToString()]; ;

            ApplicationRepository.CreditDisqualifications(false, 0, 0, householdIncome, loanAmount, propertyValue, aivl.EmploymentType.Key, false, 0, false);

            // Do loan term validation
            RuleServ.ExecuteRule(_view.Messages, "ApplicationProductMortgageLoanTerm", appProduct.Term, (Products)appProduct.ProductType);

            if (!_view.IsValid)
                return false;

            return true;
        }

        protected virtual void OverrideFeesChange(object sender, KeyChangedEventArgs e)
        {
            if (_application == null)
                return;

            bool overriden = (bool)e.Key;

            foreach (IApplicationExpense applicationExpense in _application.ApplicationExpenses)
            {
                if (applicationExpense.ExpenseType.Key == (int)ExpenseTypes.CancellationFee)
                {
                    applicationExpense.OverRidden = overriden;
                    break;
                }
            }
        }

        protected virtual void OnCalculateMaximumFixedPercentage(object sender, EventArgs e)
        {
            double fixedPercentage = 0.0;
            double houseHoldIncome = 0.0;
            double totalLoanAmount = 0.0;
            double annualFixedInterestRate = 0.0;
            double annualVariableInterestRate = 0.0;

            houseHoldIncome = _application.GetHouseHoldIncome();
            totalLoanAmount = _application.LoanAgreementAmount.Value;

            IApplicationProductVariFixLoan applicationProductVariFixLoan = _application.CurrentProduct as IApplicationProductVariFixLoan;
            if (applicationProductVariFixLoan != null)
            {
                fixedPercentage = applicationProductVariFixLoan.FixedPercentage.Value;
                annualFixedInterestRate = applicationProductVariFixLoan.FixedEffectiveRate.Value;
                annualVariableInterestRate = applicationProductVariFixLoan.VariableEffectiveRate.Value;
            }

            fixedPercentage = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateMaximumFixedPercentage(houseHoldIncome, totalLoanAmount, annualFixedInterestRate, annualVariableInterestRate);
            applicationProductVariFixLoan.FixedPercentage = fixedPercentage;

            // Recalc
            _application.CalculateApplicationDetail(_view.IsBondToRegisterExceptionAction, false);

            RunRules();

            // Rebind ...
            _view.BindApplicationDetails(_application);
        }

        private void SaveApplication()
        {
            ISupportsVariableLoanApplicationInformation vlai = _application.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            _application.CalculateApplicationDetail(_view.IsBondToRegisterExceptionAction, false);

            RunRules();

            //Set user selected SPV, would have been recalculated in CalculateApplicationDetail()
            if (!_view.SelectedSPV.Equals(vlai.VariableLoanInformation.SPV.Key.ToString()))
                vlai.VariableLoanInformation.SPV = LookupRepository.SPVList.ObjectDictionary[_view.SelectedSPV];

            ApplicationRepository.SaveApplication(_application);
        }

        private void WorkFlowNavigate()
        {
            try
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                this.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
                if (_view.IsValid)
                    base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
            catch (Exception)
            {
                if (_view.IsValid)
                    throw;
            }
        }

        protected virtual void OnUpdateClicked(object sender, EventArgs e)
        {
            _view.GetApplicationDetails(_application);

            RuleServ.ExecuteRule(_view.Messages, "ProductSuperLoNewCat1", _application);
            //errors could have been loaded in the previous calls
            if (!_view.IsValid || !PassValidation(_application))
                return;

            TransactionScope trnScope = new TransactionScope();
            try
            {
                SaveApplication();

                if (_view.IsValid)
                {
                    trnScope.VoteCommit();
                }
            }
            catch (Exception)
            {
                trnScope.VoteRollBack();

                if (_view.IsValid)
                    throw;
            }
            finally
            {
                trnScope.Dispose();
            }
            //After saving the application, let's attempt to complete the activity
            if (_view.IsValid)
            {
                WorkFlowNavigate();
            }
        }

        protected virtual void OnUpdateRevisionClicked(object sender, EventArgs e)
        {
            ISupportsVariableLoanApplicationInformation vlai = _application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            TransactionScope trnScope = new TransactionScope();
            try
            {
                //this all needs to be in a transaction so the create revision can be rolled back
                //if anything fails
                //create the revision and save it
                ExclusionSets.Add(RuleExclusionSets.LoanDetailsUpdate);

                //errors could have been loaded in the previous calls
                if (!_view.IsValid || !PassValidation(_application))
                    return;

                Application.CreateRevision();
                SaveApplication();

                ExclusionSets.Remove(RuleExclusionSets.LoanDetailsUpdate);

                //now process the revision
                _view.GetApplicationDetails(_application);

                // Check if a Quick Pay Loan then apply Quick Pay Loan Attribute so the calc can apply correct fees
                if (!_view.IsQuickPayFeeReadOnly)
                    SetApplicationAttribute(OfferAttributeTypes.QuickPayLoan, _view.QuickPayFeeChecked);

                _application.CalculateApplicationDetail(_view.IsBondToRegisterExceptionAction, false);

                RuleServ.ExecuteRule(_view.Messages, "ProductSuperLoNewCat1", _application);
                //errors could have been loaded in the previous calls
                if (!_view.IsValid)
                {
                    trnScope.VoteRollBack();
                    return;
                }

                //Set user selected SPV, would have been recalculated in CalculateApplicationDetail()
                if (!_view.SelectedSPV.Equals(vlai.VariableLoanInformation.SPV.Key.ToString()))
                    vlai.VariableLoanInformation.SPV = LookupRepository.SPVList.ObjectDictionary[_view.SelectedSPV];

                ApplicationRepository.SaveApplication(_application);

                //and commit
                if (_view.IsValid)
                {
                    trnScope.VoteCommit();
                }
            }
            catch (Exception)
            {
                trnScope.VoteRollBack();

                if (_view.IsValid)
                    throw;
            }
            finally
            {
                trnScope.Dispose();
            }
            if (_view.IsValid)
            {
                WorkFlowNavigate();
            }
        }

        protected virtual void OnCancelClicked(object sender, EventArgs e)
        {
            X2Service.CancelActivity(_view.CurrentPrincipal);
            X2Service.WorkflowNavigate(_view.CurrentPrincipal, Navigator);
        }

        protected virtual void ApplicationOptionsChange(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            if (_application == null)
                return;

            string checkBoxName = sender as String;

            switch (checkBoxName)
            {
                case "chkQuickCash":
                    SetApplicationAttribute(OfferAttributeTypes.QuickCash, (bool)e.Key);
                    break;

                case "chkHOC":
                    SetApplicationAttribute(OfferAttributeTypes.HOC, (bool)e.Key);
                    break;

                case "chkStaffLoan":
                    SetApplicationAttribute(OfferAttributeTypes.StaffHomeLoan, (bool)e.Key);
                    break;

                case "chkLife":
                    SetApplicationAttribute(OfferAttributeTypes.Life, (bool)e.Key);
                    break;

                case "chkCAP":
                    SetApplicationAttribute(OfferAttributeTypes.CAP2, (bool)e.Key);
                    break;

                case "chkInterestOnly":
                    if ((bool)e.Key)
                    {
                        bool ionlyexists = false;
                        foreach (IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment in _application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments)
                        {
                            if (applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.InterestOnly)
                            {
                                ionlyexists = true;
                                break;
                            }
                        }

                        if (!ionlyexists)
                        {
                            IApplicationInformationFinancialAdjustment afo = ApplicationRepository.GetEmptyApplicationInformationFinancialAdjustment();
                            afo.ApplicationInformation = _application.GetLatestApplicationInformation();
                            
                            // Hack to prevent adding interest only to an Edge Product
                            if (afo.ApplicationInformation.Product.Key != (int)Products.Edge)
                            {
                                afo.FinancialAdjustmentTypeSource = FinancialAdjustmentRepository.GetFinancialAdjustmentTypeSourceByKey((int)FinancialAdjustmentTypeSources.InterestOnly);
                                _application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments.Add(_view.Messages, afo);
                            }
                        }
                    }
                    else
                    {
                        foreach (IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment in _application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments)
                        {
                            if (applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.InterestOnly)
                            {
                                _application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments.Remove(_view.Messages, applicationInformationFinancialAdjustment);
                                ISupportsInterestOnlyApplicationInformation ioai = _application.CurrentProduct as ISupportsInterestOnlyApplicationInformation;
                                ioai.InterestOnlyInformation.Installment = null;
                                ioai.InterestOnlyInformation.MaturityDate = null;
                                return;
                            }
                        }
                    }
                    break;

                case "chkDiscountedLinkRate":
                    if ((bool)e.Key)
                    {
                        bool set = true;
                        if (_view.Discount == 0)
                            set = false;

                        if (set)
                            SetDiscount(_application.CurrentProduct, _view.Discount);
                    }
                    else
                    {
                        //if the presenter is approve, dont do any changes
                        //if (!_view.IsDiscountReadonly)
                        if (_view.CurrentPresenter != "SAHL.Web.Views.Origination.Presenters.ApplicationLoanDetailsApprove" &&
                            _view.CurrentPresenter != "SAHL.Web.Views.Origination.Presenters.ApplicationLoanDetailsDeclineCounterRateExceptionAction")
                            if (_application.HasFinancialAdjustment(FinancialAdjustmentTypeSources.DiscountedLinkrate)) // Only bother if there is a rate override to remove
                                SetDiscount(_application.CurrentProduct, null); //Make sure the discount rate override is removed
                    }

                    break;
                case "SetUpEdge":
                    bool ehlRateOverrideExists = false;
                    foreach (IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment in _application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments)
                    {
                        if (applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.Edge)
                        {
                            ehlRateOverrideExists = true;
                            break;
                        }
                    }

                    if (!ehlRateOverrideExists)
                    {
                        IApplicationProductEdge appProdEHL = _application.CurrentProduct as IApplicationProductEdge;
                        IApplicationInformationFinancialAdjustment iofa = ApplicationRepository.GetEmptyApplicationInformationFinancialAdjustment();
                        iofa.Term = appProdEHL.EdgeInformation.InterestOnlyTerm;
                        iofa.ApplicationInformation = _application.GetLatestApplicationInformation();
                        iofa.FinancialAdjustmentTypeSource = FinancialAdjustmentRepository.GetFinancialAdjustmentTypeSourceByKey((int)FinancialAdjustmentTypeSources.Edge);
                        _application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments.Add(_view.Messages, iofa);
                    }

                    break;
                default:
                    break;
            }
        }

        private void SetDiscount(IApplicationProduct cp, double? discount)
        {
            IApplicationProductMortgageLoan applicationProductMortgageLoan = cp as IApplicationProductMortgageLoan;

            applicationProductMortgageLoan.SetManualDiscount(_view.Messages, discount, FinancialAdjustmentTypeSources.DiscountedLinkrate);
        }

        protected void SetApplicationAttribute(OfferAttributeTypes OfferAttribute, bool Active)
        {
            if (Application == null)
                return;

            //if user has changed an option do the work, else ignore
            if (Active != Application.HasAttribute(OfferAttribute))
            {
                if (Active)
                {
                    IApplicationAttribute applicationAttribute = ApplicationRepository.GetEmptyApplicationAttribute();
                    applicationAttribute.ApplicationAttributeType = LookupRepository.ApplicationAttributesTypes.ObjectDictionary[Convert.ToString((int)OfferAttribute)];
                    applicationAttribute.Application = _application;
                    Application.ApplicationAttributes.Add(_view.Messages, applicationAttribute);
                }
                else
                {
                    foreach (IApplicationAttribute applicationAttribute in Application.ApplicationAttributes)
                    {
                        if (applicationAttribute.ApplicationAttributeType.Key == (int)OfferAttribute)
                        {
                            Application.ApplicationAttributes.Remove(_view.Messages, applicationAttribute);
                            return;
                        }
                    }
                }
            }
        }

        protected IFinancialAdjustmentRepository FinancialAdjustmentRepository
        {
            get
            {
                if (_fadjRepo == null)
                {
                    _fadjRepo = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();
                }
                return _fadjRepo;
            }
        }

        protected IControlRepository ControlRepository
        {
            get
            {
                if (_ctrlRepo == null)
                    _ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();

                return _ctrlRepo;
            }
        }

        protected IApplicationRepository ApplicationRepository
        {
            get
            {
                if (_applicationRepository == null)
                    _applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _applicationRepository;
            }
        }

        protected IApplicationMortgageLoan Application
        {
            set { _application = value; }
            get { return _application; }
        }

        protected ILookupRepository LookupRepository
        {
            get
            {
                if (_lookupRepository == null)
                    _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepository;
            }
        }

        protected IReasonRepository ReasonRepo
        {
            get
            {
                if (_reasonRepo == null)
                    _reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();

                return _reasonRepo;
            }
        }

        protected IRuleService RuleServ
        {
            get
            {
                if (_ruleServ == null)
                    _ruleServ = ServiceFactory.GetService<IRuleService>();

                return _ruleServ;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;
        }

        protected void GetApplicationFromCBO()
        {
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node != null)
                _genericKey = Convert.ToInt32(_node.GenericKey);

            _application = (IApplicationMortgageLoan)ApplicationRepository.GetApplicationByKey(GenericKey);

            //if it is not a New business application type, dont continue processing cause the view will fail
            if (_application.ApplicationType.Key != (int)OfferTypes.NewPurchaseLoan
                && _application.ApplicationType.Key != (int)OfferTypes.RefinanceLoan
                && _application.ApplicationType.Key != (int)OfferTypes.SwitchLoan)
            {
                _view.HideAll = true;
                _view.Messages.Add(new Error("This is not a new business application.", "This is not a new business application."));
                _view.ShouldRunPage = false;
                return;
            }

            //check to see if the selected cbo application is in the cache (back from QC decline)
            //if so then use the cached application and remove the application from the cache
            if (!_view.IsPostBack)
            {
                if (GlobalCacheData.ContainsKey(ViewConstants.Application))
                {
                    //#16102 We already have a populated _application
                    //      This code is replacing _application with a GlobalCache item that is not
                    //      connected to a session, and results in Lazy Initialisation errors

                    //IApplicationMortgageLoan gApp = GlobalCacheData[ViewConstants.Application] as IApplicationMortgageLoan;

                    //if (gApp != null && gApp.Key == _application.Key) //make sure this is the selected application
                    //    _application = gApp;

                    GlobalCacheData.Remove(ViewConstants.Application);
                }
            }
        }

        protected void BindLookups()
        {
            #region Old Dropdowns

            // Bind all the lookups
            //_view.BindCategories(LookupRepository.Categories.BindableDictionary, String.Empty);

            //Uses a UIStatement to populate the collection - COMMON.GetOriginatableSPVList
            IDictionary<string, string> spvs = new Dictionary<string, string>();
            var originatableSPVList = ApplicationRepository.GetOriginatableSPVList();
            foreach (ISPV spv in originatableSPVList)
            {
                    spvs.Add(new KeyValuePair<string, string>(spv.Key.ToString(), spv.Description));
            }
            _view.BindSPVs(spvs, String.Empty);

            #endregion Old Dropdowns

            // Populate the list of originatable products plus the product the client is currently on.
            IDictionary<string, string> products = new Dictionary<string, string>();
            IReadOnlyEventList<IProduct> prods = ApplicationRepository.GetOriginationProducts((int)_application.CurrentProduct.ProductType);
            foreach (IProduct product in prods)
            {
                if (product.Originate
                    || product.Key == (int)_application.CurrentProduct.ProductType)
                    products.Add(new KeyValuePair<string, string>(product.Key.ToString(), product.Description));
            }
            _view.BindProducts(products, String.Empty);

            //IApplicationMortgageLoan applicationMortgageLoan = _application as IApplicationMortgageLoan;
            //IDictionary<string, string> linkRates = new Dictionary<string, string>();
            //foreach (IMargin margin in applicationMortgageLoan.GetMargins())
            //{
            //    linkRates.Add(new KeyValuePair<string, string>(margin.Key.ToString(), margin.Description));
            //}
            //_view.BindLinkRates(linkRates, String.Empty);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.ShowRateAdjustment = false;

            // Display the correct panel
            switch ((OfferTypes)_application.ApplicationType.Key)
            {
                case OfferTypes.NewPurchaseLoan:
                    _view.IsNewPurchasePanelVisible = true;
                    _view.IsSwitchPanelVisible = false;
                    _view.IsRefinancePanelVisible = false;
                    // _view.

                    break;
                case OfferTypes.RefinanceLoan:
                    _view.IsRefinancePanelVisible = true;
                    _view.IsNewPurchasePanelVisible = false;
                    _view.IsSwitchPanelVisible = false;

                    break;
                case OfferTypes.SwitchLoan:
                    _view.IsSwitchPanelVisible = true;
                    _view.IsRefinancePanelVisible = false;
                    _view.IsNewPurchasePanelVisible = false;

                    break;
                default:
                    break;
            }

            // Display product specific stuff
            SetupProduct(_application.CurrentProduct.ProductType);

            // Set Up the Edge Home Loan Product
            _view.SetEdgeTerm = Convert.ToInt32(ControlRepository.GetControlByDescription("Edge Max Term").ControlNumeric);

            ISupportsEdgeApplicationInformation ehlApplicationInformation = _application.CurrentProduct as ISupportsEdgeApplicationInformation;
            if (ehlApplicationInformation != null)
                _view.IsEdgeVisible = true;

            // The quickcash control takes presedence over the superlo control in case of a clash
            if (_application.HasAttribute(OfferAttributeTypes.QuickCash) && _view.CanShowQuickCash)
            {
                _view.IsQuickCashVisible = true;
                _view.IsSuperLoInfoVisible = false;
            }

            _view.IsCancellationFeeVisible = false;
            _view.IsCancellationFeesReadOnly = true;

            if (_application.ApplicationType.Key == (int)OfferTypes.SwitchLoan)
            {
                foreach (IApplicationExpense applicationExpense in _application.ApplicationExpenses)
                {
                    if (applicationExpense.ExpenseType.Key == (int)ExpenseTypes.CancellationFee)
                    {
                        _view.IsCancellationFeeVisible = true;
                        _view.IsCancellationFeesReadOnly = !applicationExpense.OverRidden;
                        break;
                    }
                }
            }

            //bool isPropertyValueReadOnly = false;
            //if (_application is IApplicationMortgageLoanNewPurchase)
            //    isPropertyValueReadOnly = true;

            //_view.IsPropertyValueReadOnly = isPropertyValueReadOnly;

            if (_application.Property != null && _application.Property.LatestCompleteValuation != null && _application.Property.LatestCompleteValuation.IsActive == true)
                _view.IsPropertyValueValuation = true; //this will also set the _view.IsPropertyValueReadOnly = true
        }

        private void SetupProduct(Products prod)
        {
            // Display product specific stuff
            switch (prod)
            {
                case Products.DefendingDiscountRate:

                    break;
                case Products.HomeOwnersCover:

                    break;
                case Products.LifePolicy:

                    break;
                case Products.NewVariableLoan:
                    _view.IsLoanDetailsVisible = true;
                    _view.IsVarifixDetailsVisible = false;
                    _view.IsEdgeVisible = false;

                    break;
                case Products.QuickCash:

                    break;
                case Products.SuperLo:
                    _view.IsLoanDetailsVisible = true;
                    _view.IsVarifixDetailsVisible = false;
                    _view.IsSuperLoInfoVisible = true;
                    _view.IsEdgeVisible = false;
                    break;
                case Products.VariFixLoan:
                    _view.IsVarifixDetailsVisible = true;
                    _view.IsLoanDetailsVisible = false;
                    _view.IsEdgeVisible = false;
                    break;
                case Products.VariableLoan:
                    _view.IsLoanDetailsVisible = true;
                    _view.IsVarifixDetailsVisible = false;
                    _view.IsEdgeVisible = false;
                    break;
                case Products.Edge:
                    _view.IsLoanDetailsVisible = false;
                    _view.IsVarifixDetailsVisible = false;
                    _view.IsEdgeVisible = true;
                    break;
                default:
                    break;
            }
        }

        protected void CheckQCReasons()
        {
            _view.EnforceQuickCash = true;

            IReadOnlyEventList<IReason> reasons = ReasonRepo.GetReasonByGenericKeyAndReasonTypeKey(Application.GetLatestApplicationInformation().Key, (int)ReasonTypes.QuickCashDecline);
            if (reasons != null && reasons.Count > 0)
            {
                _view.HasQuickCashDeclineReasons = true;
            }

            ISupportsQuickCashApplicationInformation suppQCash = _application as ISupportsQuickCashApplicationInformation;

            if (suppQCash != null && suppQCash.ApplicationInformationQuickCash.CreditApprovedAmount > 0)
            {
                _view.HasQuickCashValue = true;
            }
        }

        protected void DoITCCheck()
        {
            RuleServ.ExecuteRule(_view.Messages, "ITCApplication", _application);
        }
    }
}