using System;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.FurtherLending.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using System.Collections.Generic;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.Utils;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.CacheData;
using System.Security.Principal;
using SAHL.Common.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using System.Linq;
using System.Text;
using System.IO;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace SAHL.Web.Views.FurtherLending.Presenters
{
    public class CalculatorBase : SAHLCommonBasePresenter<IFurtherLendingCalculator>
    {
        #region locals

        private string SUBJECT = "Further lending application form/s";
        private string BODY = "As discussed, please find the application forms attached.";

        enum ReportType
        {
            Readvance = 0,
            FurtherAdvance,
            FurtherLoan
        }

        private CBONode _cboNode;
        private InstanceNode _instanceNode;

        protected int _accountKey = -1;
        protected IApplication _app;
        protected IAccount _account;
        private double _accCurrBalance;
        protected IMortgageLoan _vML;
        protected IMortgageLoan _fML;
        protected int _ArrearMonthCheck = 6;
        protected float _ArrearMinimumValueCheck = 500;
        private int _accAppType;
        private ISPVService _spvService;
        private IMessageService _msgSvc;
        private IRuleService _ruleServ;
        protected IMortgageLoanAccount _mla;
        private bool _ncaCompliant = true;

        private IX2Info _XI;

        private string _activityName; 
        private string _currentState;
        private long _instanceID; 
        private string _sessionID;

        #region Application Inputs - Reused for each app type

        //Application common data:
        private double _rate;
        private int _ratekey;
        private double _loanAgreementAmount;
        private double _valuationAmount;
        private double _bondToRegister;
        private double _householdIncome;

        private string _adUserName;
        #endregion

        #region Applications

        IApplicationFurtherLoan _fl;
        IApplicationReAdvance _ra;
        IApplicationFurtherAdvance _fa;

        #endregion

        #region Repositories/Services


        private IAccountRepository _accountRepo;
        private ILookupRepository _lookupRepo;
        private IApplicationRepository _applicationRepo;
        private ILegalEntityRepository _leRepository;
        private ICreditMatrixRepository _cmRepo;
        private ICorrespondenceRepository _correspondRepo;
        private IOrganisationStructureRepository _orgRepo;
        private IReasonRepository _reasonRepo;
        private IReportRepository _repRepo;
        private IControlRepository _controlRepo;
        private IX2Repository _x2Repo;

        #endregion

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CalculatorBase(IFurtherLendingCalculator view, SAHLCommonBaseController controller)
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

            _cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal);

            if (_cboNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            // Get the AccountKey from the CBO
            if (_cboNode is InstanceNode)
            {
                _instanceNode = _cboNode as InstanceNode;
                _app = ApplicationRepo.GetApplicationByKey(Convert.ToInt32(_instanceNode.GenericKey));

                //if it is not a FL application type, dont continue processing cause the view will fail
                if (_app.ApplicationType.Key != (int)OfferTypes.ReAdvance
                    && _app.ApplicationType.Key != (int)OfferTypes.FurtherAdvance
                    && _app.ApplicationType.Key != (int)OfferTypes.FurtherLoan)
                {
                    _view.HideAll = true;
                    _view.Messages.Add(new Error("This is not a Further Lending application.", "This is not a Further Lending application."));
                    _view.ShouldRunPage = false;
                    return;
                }

                _view.ApplicationType = (OfferTypes)_app.ApplicationType.Key;
                _accountKey = _app.Account.Key;
            }
            else
            {
                _accountKey = Convert.ToInt32(_cboNode.GenericKey);
            }

            // Get the Account Object
            _account = AccountRepo.GetAccountByKey(_accountKey);

            //Make sure we have a Valid account 
            if ((_account as IMortgageLoanAccount) == null)
            {
                throw new Exception("Not a Mortgage Loan Account!");
            }

            // if we are a 30yr and there are more then 240months remaining, then we want also show 20yr figures
            _view.ShowTwentyYearFigures = ApplicationRepo.Display20YearFiguresOn30YearLoan(_account); ;

            foreach (IAccountInformation ai in _account.AccountInformations)
            {
                if (ai.AccountInformationType.Key == (int)AccountInformationTypes.NotNCACompliant)
                {
                    _ncaCompliant = false;
                    break;
                }
            }
            _view.NCACompliant = _ncaCompliant;

            //Run rule checks: potentially no application e.g.: for create
            // so test directly on Account.
            IRuleService rules = ServiceFactory.GetService<IRuleService>();
            List<string> rulesToRun = new List<string>();
            // Opt in checks
            rulesToRun.Add("ProductSuperLoOptInLoanTransaction"); // Super Lo
            rulesToRun.Add("ProductVarifixOptInLoanTransaction"); // VariFix
            rulesToRun.Add("ProductInterestOnlyOptInLoanTransaction"); //Interest Only
            //Debt counselling transitions
            rulesToRun.Add("AccountDebtCounseling");
            rulesToRun.Add("LegalEntitiesUnderDebtCounsellingForAccount");
            //Check for any detail types
            rulesToRun.Add("AccountDetailTypeCheck");
            //non performing
            rulesToRun.Add("ApplicationFurtherLendingNonperformingLoan");
            rulesToRun.Add("AccountBaselIIDecline");
            rulesToRun.Add("AccountBaselIIRefer");
            rulesToRun.Add("AccountIsAlphaHousing");
            //Detail Type Warnings
            rulesToRun.Add("AccountDetailTypeWarning");
            //Execute RuleSet
            rulesToRun.Add("LoanHas30YearTermAndRemainingInstalmentsCheck");
            rules.ExecuteRuleSet(_view.Messages, rulesToRun, new object[] { _account });

            //warn if no title deeds from the deeds office
            rules.ExecuteRule(_view.Messages, "TitleDeedsOnFile", new object[] { _account.Key });

            foreach (IApplication app in _account.Applications)
            {
                if (app.ApplicationStatus.Key == (int)OfferStatuses.Accepted && app.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan)
                {
                    IApplicationMortgageLoanNewPurchase mlNP = app as IApplicationMortgageLoanNewPurchase;
                    if (mlNP != null)
                    {
                        _accAppType = app.ApplicationType.Key;
                        _view.AccountApplicationType = _accAppType;
                        _view.AccountPurchasePrice = mlNP.PurchasePrice.HasValue ? mlNP.PurchasePrice.Value : 0.00;
                    }
                }
            }


            //Get the variable ML
            IMortgageLoanAccount mla = _account as IMortgageLoanAccount;
            _vML = mla.SecuredMortgageLoan;
            _accCurrBalance = _vML.CurrentBalance;

            //Get the Fixed ML  
            if ((_account as IAccountVariFixLoan) != null)
            {
                IAccountVariFixLoan _fAccount = _account as IAccountVariFixLoan;
                _fML = _fAccount.FixedSecuredMortgageLoan;
                _accCurrBalance += _fML.CurrentBalance;
            }

            _view.HasArrears = _account.HasBeenInArrears(_ArrearMonthCheck, _ArrearMinimumValueCheck);
            _view.IsInterestOnly = _vML.HasInterestOnly();

            //Existing Further Lending applications
            CheckExistingApplications();

            //Lookups
            IEventList<IEmploymentType> _EmpTypes = LookupRepo.EmploymentTypes;
            IEventList<IOccupancyType> _occTypes = LookupRepo.OccupancyTypes;
            IList<ISPV> _spvList = SPVService.GetSPVListForFurtherLending(); //(_vML.SPV.Key);
            int occupancyType = 0;
            if (_vML.Property.OccupancyType != null)
            {
                occupancyType = _vML.Property.OccupancyType.Key;
            }
            //Bind the data we have collected
            _view.BindEmploymentTypes(_EmpTypes, _account.GetEmploymentTypeKey());
            _view.BindOccupancyTypes(_occTypes, occupancyType);
            _view.BindSPVs(_spvList);
            _view.BindDisplay(_vML, _fML, _account, _app, false);

            _view.OnCalculateButtonClicked += new EventHandler(_view_OnCalculateButtonClicked);
            _view.OnContactUpdateButtonClicked += new EventHandler(_view_ContactUpdateButtonClicked);
            _view.OnQuickCashButtonClicked += new EventHandler(_view_OnQuickCashButtonClicked);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnQuickCashButtonClicked(object sender, EventArgs e)
        {
            //need to set the view to go back to...
            if (GlobalCacheData.ContainsKey("NavigateBack"))
                GlobalCacheData.Remove("NavigateBack");

            GlobalCacheData.Add("NavigateBack", _view.ViewName, new List<ICacheObjectLifeTime>());

            _account = AccountRepo.GetAccountByKey(_accountKey);
            _mla = _account as IMortgageLoanAccount;

            foreach (IApplication app in _account.Applications)
            {
                if (app.ApplicationStatus.Key == (int)OfferStatuses.Open && app.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
                {
                    _fl = app as IApplicationFurtherLoan;
                    break;
                }
            }

            if (null != _mla)
            {
                TransactionScope txn = new TransactionScope(TransactionMode.Inherits);
                try
                {
                    //Save Further Loan application
                    if (_view.FurtherLoanRequired > 0)
                        CreateFurtherLoanAndReport(!_view.FurtherLoanInProgress, _view.HelpDeskRework);

                    if (_view.IsValid)
                        Navigator.Navigate("FurtherLendingQuickCash");

                    txn.VoteCommit();
                }
                catch (Exception)
                {
                    txn.VoteRollBack();

                    if (_view.IsValid)
                        throw;  // not a domain validation exception.
                }
                finally
                {
                    txn.Dispose();
                }
            }
        }

        private void PopulateApplicationData(IApplicationProduct cp, OfferTypes appType)
        {
            if (_view.NewOccupancyTypeKey != 0)
                ((IMortgageLoanAccount)cp.Application.Account).SecuredMortgageLoan.Property.OccupancyType = LookupRepo.OccupancyTypes.FirstOrDefault(x => x.Key == _view.NewOccupancyTypeKey);

            ISupportsVariableLoanApplicationInformation svli = cp as ISupportsVariableLoanApplicationInformation;
            ISupportsVariableLoanApplicationInformation oldSvli = _mla.CurrentMortgageLoanApplication.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            //Only update the Employment type if we are not at credit
            if (_view.ApprovalMode == ApprovalTypes.None)
                svli.VariableLoanInformation.EmploymentType = LookupRepo.EmploymentTypes.ObjectDictionary[_view.EmploymentTypeKeySelected.ToString()];

            svli.VariableLoanInformation.HouseholdIncome = _view.NewIncome;
            svli.VariableLoanInformation.RateConfiguration =
                CMRepo.GetRateConfigurationByMarginKeyAndMarketRateKey(_view.MarginKeySelected, oldSvli.VariableLoanInformation.RateConfiguration.MarketRate.Key);

            if (appType == OfferTypes.FurtherLoan)
                svli.VariableLoanInformation.BondToRegister = _view.BondToRegister;

            switch (appType)
            {
                case OfferTypes.ReAdvance:
                    svli.VariableLoanInformation.LoanAmountNoFees = _view.ReadvanceRequired;
                    break;
                case OfferTypes.FurtherAdvance:
                    svli.VariableLoanInformation.LoanAmountNoFees = _view.FurtherAdvRequired;
                    break;
                case OfferTypes.FurtherLoan:
                    svli.VariableLoanInformation.LoanAmountNoFees = _view.FurtherLoanRequired;// +_view.Fees;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnCalculateButtonClicked(object sender, EventArgs e)
        {
            // Do calcs
            ApplicationCalculate(false);

            IRuleService rules = ServiceFactory.GetService<IRuleService>();
            List<string> rulesToRun = new List<string>();
            //run FLoan specific rules
            if (_view.FurtherLoanRequired > 0)
            {
                //Run rule checks: potentially no application e.g.: for create
                // so test directly on Account.
                rulesToRun.Add("ProductVarifixOptInLoanTransaction"); // VariFix
                rulesToRun.Add("AccountDetailTypeVarifixOptOut90DayPendingHoldCheck");
                rules.ExecuteRuleSet(_view.Messages, rulesToRun, new object[] { _account, _ra, _fa, _fl });
            }
            else
            {
                rulesToRun.Add("AccountDetailTypeVarifixOptOut90DayPendingHoldCheck");
                rules.ExecuteRuleSet(_view.Messages, rulesToRun, new object[] { _account, _ra, _fa, _fl });
            }
        }

        private void ApplicationCalculate(bool save)
        {
            if (_view.TotalCashRequired > 0)
            {
                //populate all possible applications ra, fa, fl
                if (_account == null)
                    _account = AccountRepo.GetAccountByKey(_accountKey);

                _mla = _account as IMortgageLoanAccount;

                GetOpenApplicationsFromAccount(_account);

                //Create Readvance application
                if (_view.ReadvanceRequired > 0 || _view.ReadvanceInProgress)
                {
                    //if no application exists go create one to pass to the Framework Calc
                    //if the app key = 0, and we need to save the application then redo the create
                    //because additional work is done setting up roles etc...
                    if (_ra == null || (_ra.Key == 0 && save))
                        _ra = ApplicationRepo.CreateReAdvanceApplication(_mla, save);

                    _ra.ClientEstimatePropertyValuation = _view.LatestValuationAmount;

                    //only change values if the app has not passed credit
                    if (_ra.GetLatestApplicationInformation().ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                        PopulateApplicationData(_ra.CurrentProduct, OfferTypes.ReAdvance);

                }
                //Create Further advance application
                if (_view.FurtherAdvRequired > 0 || _view.FurtherAdvanceInProgress)
                {
                    if (_fa == null || (_fa.Key == 0 && save))
                        _fa = ApplicationRepo.CreateFurtherAdvanceApplication(_mla, save);

                    _fa.ClientEstimatePropertyValuation = _view.LatestValuationAmount;

                    //only change values if the app has not passed credit
                    if (_fa.GetLatestApplicationInformation().ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                        PopulateApplicationData(_fa.CurrentProduct, OfferTypes.FurtherAdvance);
                }
                //Create Further Loan application
                if (_view.FurtherLoanRequired > 0 || _view.FurtherLoanInProgress)
                {
                    if (_fl == null || (_fl.Key == 0 && save))
                        _fl = ApplicationRepo.CreateFurtherLoanApplication(_mla, save);

                    _fl.ClientEstimatePropertyValuation = _view.LatestValuationAmount;

                    //only change values if the app has not passed credit
                    if (_fl.GetLatestApplicationInformation().ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                        PopulateApplicationData(_fl.CurrentProduct, OfferTypes.FurtherLoan);
                }

                //recalc apps with FLAppCalculateHelper
                FLAppCalculateHelper flH = new FLAppCalculateHelper(_account, _ra, _fa, _fl);
                flH.CalculateFurtherLending(true);
                _view.NewLTV = flH.LTV;
                _view.NewAMInstalment = flH.AmortisingInstalment;
                _view.NewPTI = flH.PTI;

                _view.NewInstalment = flH.Instalment;
                _view.NewSPV = flH.SPV;

                _view.IsExceptionCreditCriteria = flH.IsExceptionCreditCriteria;
                _view.Fees = flH.Fees;
                _view.NewCategory = flH.AppCategory;


                var variableFinancialService = _account.GetFinancialServicesByType(FinancialServiceTypes.VariableLoan, new AccountStatuses[] { AccountStatuses.Open }).First();
                _view.CurrentLinkRate = variableFinancialService.Balance.LoanBalance.RateConfiguration.Margin.Value;
                _view.CalculatedLinkRate = flH.CalculatedLinkRate;

                if (_accAppType == (int)OfferTypes.NewPurchaseLoan)
                {
                    _view.NewLTP = (_view.NewCurrentBalance + _view.CapitalisedLife) / _view.AccountPurchasePrice;
                }

                if (_view.ShowTwentyYearFigures)
                {
                    _view.TwentyYearInstalment = flH.Calculated20YearLoanDetailsFor30YearTermLoan.Instalment;
                    _view.TwentyYearPTI = flH.Calculated20YearLoanDetailsFor30YearTermLoan.PTI;
                }
            }
            else
            {
                _view.NewCurrentBalance = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CheckExistingApplications()
        {
            bool selectedAppOnly = false;

            if (_view.ApprovalMode != ApprovalTypes.None) //Credit screens, only load selected application
                selectedAppOnly = true;

            IEventList<IApplication> applications = ApplicationRepo.GetApplicationByAccountKey(_accountKey);

            foreach (IApplication app in applications)
            {
                if ((app.ApplicationStatus.Key == Convert.ToInt16(OfferStatuses.Open))
                        && (
                            app.ApplicationType.Key == (int)OfferTypes.ReAdvance ||
                            app.ApplicationType.Key == (int)OfferTypes.FurtherAdvance ||
                            app.ApplicationType.Key == (int)OfferTypes.FurtherLoan))
                {
                    //16486Remove start
                    //check the start date of the readvance to determine what calc to use in the view
                    //this can be removed a month after go live: select * from control where controlDescription = 'CLVReadvanceStart'
                    switch ((OfferTypes)app.ApplicationType.Key)
                    {
                        case OfferTypes.ReAdvance:
                            _view.ReadvanceStartDate = app.ApplicationStartDate.HasValue ? app.ApplicationStartDate.Value : DateTime.Now;
                            break;
                        case OfferTypes.FurtherAdvance:
                            IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                            IControl ctrl = ctrlRepo.GetControlByDescription("CLVReadvanceStart");
                            DateTime dt = DateTime.Now;
                            if (ctrl != null && !String.IsNullOrEmpty(ctrl.ControlText))
                                dt = Convert.ToDateTime(ctrl.ControlText);

                            if ((app.ApplicationStartDate.HasValue ? app.ApplicationStartDate.Value : DateTime.Now) < dt)
                                _view.HideCLV = true;
                            break;
                        default:
                            break;
                    }
                    //16486Remove end

                    if (!selectedAppOnly || _app == app) //Credit screens, only load selected application
                    {
                        GetApplicationData(app.CurrentProduct);

                        switch ((OfferTypes)app.ApplicationType.Key)
                        {
                            case OfferTypes.ReAdvance:
                                _view.ReadvanceInProgress = true;
                                _view.ReadvanceIsAccepted = IsApplicationAccepted(app);
                                _view.ReadvanceInProgressAmount = _loanAgreementAmount;

                                SetExistingApplicationValues(OfferTypes.ReAdvance, app.Key);
                                break;
                            case OfferTypes.FurtherAdvance:
                                _view.FurtherAdvanceInProgress = true;
                                _view.FurtherAdvanceIsAccepted = IsApplicationAccepted(app);
                                _view.FurtherAdvanceInProgressAmount = _loanAgreementAmount;
                                SetExistingApplicationValues(OfferTypes.FurtherAdvance, app.Key);
                                break;
                            case OfferTypes.FurtherLoan:
                                _view.FurtherLoanInProgress = true;
                                _view.FurtherLoanIsAccepted = IsApplicationAccepted(app);
                                _view.FurtherLoanInProgressAmount = _loanAgreementAmount;
                                SetExistingApplicationValues(OfferTypes.FurtherLoan, app.Key);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private static bool IsApplicationAccepted(IApplication app)
        {
            if (app.GetLatestApplicationInformation() != null && app.GetLatestApplicationInformation().ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                return true;

            return false;
        }

        private void SetExistingApplicationValues(OfferTypes appType, int appKey)
        {
            //order of importance is FL, FA, RA
            if (appType == OfferTypes.FurtherLoan)
            {
                CheckQCReasonsAndValues();
                updateView();
            }
            else if (appType == OfferTypes.FurtherAdvance && _view.FurtherLoanInProgress == false)
                updateView();
            else if (appType == OfferTypes.ReAdvance && _view.FurtherLoanInProgress == false && _view.FurtherAdvanceInProgress == false)
                updateView();

            if (_view.ExistingApplicationMessage.Length == 0)
                _view.ExistingApplicationMessage = ApplicationRepo.GetFurtherLendingX2Message(appKey);
        }

        private void updateView()
        {
            _view.ApplicationValuationAmount = _valuationAmount;
            _view.ApplicationRate = _rate;
            _view.ApplicationRateKey = _ratekey;
            _view.ApplicationBondToRegister = _bondToRegister;
            _view.ApplicationHouseholdIncome = _householdIncome;
        }

        /// <summary>
        /// Update the Contact details to the db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_ContactUpdateButtonClicked(object sender, EventArgs e)
        {
            ILegalEntity lE = _view.SelectedLegalEntity;

            lE.HomePhoneCode = _view.HomePhoneCode;
            lE.HomePhoneNumber = _view.HomePhoneNumber;
            lE.WorkPhoneCode = _view.WorkPhoneCode;
            lE.WorkPhoneNumber = _view.WorkPhoneNumber;
            lE.FaxCode = _view.FaxCode;
            lE.FaxNumber = _view.FaxNumber;
            lE.CellPhoneNumber = _view.CellNumber;
            lE.EmailAddress = _view.Email;

            TransactionScope txn = new TransactionScope(TransactionMode.Inherits);
            try
            {
                LERepository.SaveLegalEntity(lE, false);
                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();

                if (_view.IsValid)
                    throw;  // not a domain validation exception.
            }
            finally
            {
                txn.Dispose();
            }
        }

        private void CreateWorkflow(int ApplicationKey, int OfferTypeKey)
        {
            bool created = false;
            try
            {
                // once we have an application create a workflow case
                Dictionary<string, string> Inputs = new Dictionary<string, string>();
                Inputs.Add("ApplicationKey", ApplicationKey.ToString());
                Inputs.Add("ApplicationCaptureOwner", _view.CurrentPrincipal.Identity.Name);
                Inputs.Add("CaseOwnerName", _view.CurrentPrincipal.Identity.Name);
                Inputs.Add("OfferTypeKey", OfferTypeKey.ToString());

                IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
                if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                    X2Service.LogIn(_view.CurrentPrincipal);

                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                X2Service.CreateWorkFlowInstance(_view.CurrentPrincipal, SAHL.Common.Constants.WorkFlowProcessName.Origination, (-1).ToString(), SAHL.Common.Constants.WorkFlowName.Origination, SAHL.Common.Constants.WorkFlowActivityName.ApplicationFurtherLendingCreate, Inputs, false);
                created = true;
                X2Service.CreateCompleteActivity(_view.CurrentPrincipal, Inputs, spc.IgnoreWarnings, null);
            }
            catch (Exception)
            {
                if (created)
                    X2Service.CancelActivity(_view.CurrentPrincipal);

                if (_view.IsValid)
                    throw;
            }
        }

        private void GetOpenApplicationsFromAccount(IAccount acc)
        {
            foreach (IApplication app in acc.Applications)
            {
                if (_view.ApprovalMode == ApprovalTypes.None || (_app.Key == app.Key))
                {
                    if (app.ApplicationStatus.Key == (int)OfferStatuses.Open)
                    {
                        switch (app.ApplicationType.Key)
                        {
                            case (int)OfferTypes.ReAdvance:
                                _ra = app as IApplicationReAdvance;
                                break;
                            case (int)OfferTypes.FurtherAdvance:
                                _fa = app as IApplicationFurtherAdvance;
                                break;
                            case (int)OfferTypes.FurtherLoan:
                                _fl = app as IApplicationFurtherLoan;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generate the applications and send application forms to the client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_GenerateButtonClicked(object sender, EventArgs e)
        {
            _view.AddTrace(this, "_view_GenerateButtonClicked start");
            if (_account == null)
                _account = AccountRepo.GetAccountByKey(_accountKey);
            _mla = _account as IMortgageLoanAccount;

            List<string> rptLocation = new List<string>();

            if (null != _mla)
            {
                IADUser aduser = OrgRepo.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);
                if (aduser != null && aduser.LegalEntity != null)
                    _adUserName = aduser.LegalEntity.DisplayName;
                else
                    _adUserName = _view.CurrentPrincipal.Identity.Name;

                TransactionScope txn = new TransactionScope(TransactionMode.Inherits);
                try
                {
                    bool completeActivity = false;
                    //Exclude LE rules
                    ExclusionSets.Add(RuleExclusionSets.LegalEntityExcludeAll);
                    ExclusionSets.Add(RuleExclusionSets.FurtherLendingCreateView);

                    _XI = X2Service.GetX2Info(_view.CurrentPrincipal);
                    if (!String.IsNullOrEmpty(_XI.ActivityName))
                    {
                        _activityName = _XI.ActivityName;
                        _currentState = _XI.CurrentState;
                        _instanceID = _XI.InstanceID;
                        _sessionID = _XI.SessionID;
                    }

                    //need to change this to create all FL app's then create rpt's
                    //Call recalc with save = true?
                    ApplicationCalculate(true);

                    // if we are sending any of the application forms then we must send the confirmation of enquiry letter too
                    if (_view.SendReadvanceApplicationForm || _view.SendFurtherAdvanceApplicationForm || _view.SendFurtherLoanApplicationForm)
                    {
                        var confirmationOfEnquiryReport = GenerateConfirmationOfEnquiry();
                        rptLocation.Add(confirmationOfEnquiryReport);
                    }

                    //Create Readvance application in X2
                    if (_view.ReadvanceRequired > 0 || _view.ReadvanceInProgress)
                    {
                        //if there is no readvance or the readvance is not yet past credit
                        if (_view.HelpDeskRework || !_view.ReadvanceInProgress || _ra.GetLatestApplicationInformation().ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                        {
                            string rpt = CreateReadvanceAndReport(!_view.ReadvanceInProgress, _view.HelpDeskRework);
                            if (!String.IsNullOrEmpty(rpt))
                                rptLocation.Add(rpt);

                            if (!_view.ReadvanceInProgress)
                            {
                                CreateWorkflow(_ra.Key, (int)OfferTypes.ReAdvance);

                                if (aduser != null && aduser.LegalEntity != null)
                                {
                                    _ra.AddRole((int)OfferRoleTypes.FLOriginatorD, aduser.LegalEntity);
                                    ApplicationRepo.SaveApplication(_ra);
                                }
                            }
                            else
                                completeActivity = true;
                        }
                        else
                            completeActivity = true;

                    }
                    //Create Further advance application in X2
                    if (_view.FurtherAdvRequired > 0 || _view.FurtherAdvanceInProgress)
                    {
                        //if there is no f-advance or the f-advance is not yet past credit
                        if (!_view.FurtherAdvanceInProgress || _fa.GetLatestApplicationInformation().ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                        {
                            string rpt = CreateFurtherAdvanceAndReport(!_view.FurtherAdvanceInProgress, _view.HelpDeskRework);
                            if (!String.IsNullOrEmpty(rpt))
                                rptLocation.Add(rpt);

                            if (!_view.FurtherAdvanceInProgress)
                            {
                                CreateWorkflow(_fa.Key, (int)OfferTypes.FurtherAdvance);

                                if (aduser != null && aduser.LegalEntity != null)
                                {
                                    _fa.AddRole((int)OfferRoleTypes.FLOriginatorD, aduser.LegalEntity);
                                    ApplicationRepo.SaveApplication(_fa);
                                }
                            }
                            else
                                completeActivity = true;
                        }
                        else
                            completeActivity = true;
                    }
                    //Create Further Loan application in X2
                    if (_view.FurtherLoanRequired > 0 || _view.FurtherLoanInProgress)
                    {
                        //if there is no f-advance or the f-advance is not yet past credit
                        if (!_view.FurtherLoanInProgress || _fl.GetLatestApplicationInformation().ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                        {
                            string rpt = CreateFurtherLoanAndReport(!_view.FurtherLoanInProgress, _view.HelpDeskRework);
                            if (!String.IsNullOrEmpty(rpt))
                                rptLocation.Add(rpt);

                            if (!_view.FurtherLoanInProgress)
                            {
                                CreateWorkflow(_fl.Key, (int)OfferTypes.FurtherLoan);

                                if (aduser != null && aduser.LegalEntity != null)
                                {
                                    _fl.AddRole((int)OfferRoleTypes.FLOriginatorD, aduser.LegalEntity);
                                    ApplicationRepo.SaveApplication(_fl);
                                }
                            }
                            else
                                completeActivity = true;
                        }
                        else
                            completeActivity = true;
                    }

                    if (_view.IncludeNaedoForm)
                    {
                        var naedoDebitOrderAuthorizationReport = GenerateNaedoDebitOrderAuthorization();
                        rptLocation.Add(naedoDebitOrderAuthorizationReport);
                    }

                    //Exclude LE rules
                    ExclusionSets.Remove(RuleExclusionSets.LegalEntityExcludeAll);
                    ExclusionSets.Remove(RuleExclusionSets.FurtherLendingCreateView);
                    if (_view.IsValid)
                    {
                        //send application forms to client
                        SendToClient(rptLocation);

                        if (completeActivity)
                        {
                            _view.AddTrace(this, "_view_GenerateButtonClicked CompleteActivity");
                            CompleteActivity();
                        }
                        else
                        {
                            _view.AddTrace(this, "_view_GenerateButtonClicked Navigate");
                            Navigator.Navigate("ClientSuperSearch"); // navigate to workflow 
                        }

                        _view.AddTrace(this, "_view_GenerateButtonClicked Commit");
                        txn.VoteCommit();
                    }
                }
                catch (Exception)
                {
                    txn.VoteRollBack();

                    if (_view.IsValid)
                        throw;  // not a domain validation exception.
                }
                finally
                {
                    txn.Dispose();
                }
            }
        }

        private string GenerateConfirmationOfEnquiry()
        {
            var fileName = "ConfirmationOfEnquiry_" + _accountKey.ToString() + ".pdf";
            var reportParams = new Dictionary<string, string>();

            reportParams.Add("Format", _view.EnquiryReportParameters.Format.ToString());
            reportParams.Add("AccountKey", _view.EnquiryReportParameters.AccountKey.ToString());
            reportParams.Add("EstimatedValuationAmount", _view.EnquiryReportParameters.EstimatedValuationAmount.ToString());
            reportParams.Add("ReadvanceMaxAvailable", _view.EnquiryReportParameters.ReadvanceMax.ToString());
            reportParams.Add("FurtherAdvanceMaxLoanAgreementAmountAvailable", _view.EnquiryReportParameters.FurtherAdvanceMaxLAA.ToString());
            reportParams.Add("FurtherAdvanceMaxBondAmountAvailable", _view.EnquiryReportParameters.FurtherAdvanceMax.ToString());
            reportParams.Add("FurtherLoanMaxAvailable", _view.EnquiryReportParameters.FurtherLoanMax.ToString());

            reportParams.Add("ReadvanceAmountRequested", _view.EnquiryReportParameters.ReadvanceRequired.ToString());
            reportParams.Add("FurtherAdvanceLoanAgreementAmountAmountRequested", _view.EnquiryReportParameters.FurtherAdvanceLAARequired.ToString());
            reportParams.Add("FurtherAdvanceBondAmountAmountRequested", _view.EnquiryReportParameters.FurtherAdvanceRequired.ToString());
            reportParams.Add("FurtherLoanAmountRequested", _view.EnquiryReportParameters.FurtherLoanRequired.ToString());

            reportParams.Add("NewLinkRate", _view.EnquiryReportParameters.NewLinkRate.ToString());
            reportParams.Add("FurtherBondAmount", _view.EnquiryReportParameters.FurtherBondAmount.ToString());
            reportParams.Add("FurtherLoanFees", _view.EnquiryReportParameters.FurtherLoanFees.ToString());
            reportParams.Add("ADUserName", _view.EnquiryReportParameters.ADUserName.ToString());


            var report = GenerateSQLReport(fileName, (int)ReportStatements.ConfirmationOfEnquiry, reportParams, true);
            return report;
        }

        private string GenerateNaedoDebitOrderAuthorization()
        {
            var fileName = "NaedoDebitOrderAuthorization_" + _accountKey.ToString() + ".pdf";
            var reportParams = new Dictionary<string, string>();
            var report = GenerateSQLReport(fileName, (int)ReportStatements.NaedoDebitOrderAuthorization, reportParams);
            return report;
        }

        /// <summary>
        /// Cancel button has been clicked, but no X2 activity has been started
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnCancelButtonClickedNoActivity(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }

        /// <summary>
        /// Cancel button has been clicked and X2 activity needs to be cancelled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            CancelActivity();
        }

        protected void CancelActivity()
        {
            IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
            if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                X2Service.LogIn(_view.CurrentPrincipal);

            X2Service.CancelActivity(_view.CurrentPrincipal);
            X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        protected void CreditStageComplete(bool save)
        {
            TransactionScope trnScope = new TransactionScope();

            try
            {
                if (save)
                    CreditStageSave();

                CompleteActivity();

                trnScope.VoteCommit();
            }
            catch (Exception)
            {
                trnScope.VoteRollBack();
                _view.ShouldRunPage = false;

                if (_view.IsValid)
                    throw;
            }
            finally
            {
                trnScope.Dispose();
            }
        }

        private void CompleteActivity()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            if (_XI != null)
            {
                _XI.ActivityName = _activityName;
                _XI.CurrentState = _currentState;
                _XI.InstanceID = _instanceID;
                _XI.SessionID = _sessionID;
                spc.X2Info = _XI;
            }

            IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
            if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                X2Service.LogIn(_view.CurrentPrincipal);

            X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);

            // reset the 30 Day Timer for all related FL Instances for this account
            IList<IInstance> instances = X2Repo.GetFLInstancesForAccountAtState(_accountKey, SAHL.Common.WorkflowState.AwaitingApplication);
            foreach (var instance in instances)
            {
                if (instance.ID != _instanceID) // ignore the instance we are on as the complete activity will reset its timer
                {
                    X2Repo.CreateAndSaveActiveExternalActivity(SAHL.Common.Constants.WorkFlowExternalActivity.Reset30DayTimer, instance.ID, SAHL.Common.Constants.WorkFlowName.ApplicationManagement, SAHL.Common.Constants.WorkFlowProcessName.Origination, string.Empty);
                }
            }

            //This navigates to ClientSuperSearch, which falls over if used....
            X2Service.WorkflowNavigate(_view.CurrentPrincipal, Navigator);
        }

        protected void CheckEmploymentTypeForCredit()
        {
            //Check employment type is valid
            if (_view.EmploymentTypeKeySelected == (int)EmploymentTypes.Unemployed
                || _view.EmploymentTypeKeySelected == (int)EmploymentTypes.Unknown)
            {
                _view.BtnSubmitEnabled = false;
                _view.Messages.Add(new SAHL.Common.DomainMessages.Error("The application employment type is inavlid, further information is required.", "The application employment type is inavlid, further information is required."));
            }
        }

        private string CreateReadvanceAndReport(bool createnew, bool helpDeskRework)
        {
            _ra.SetApplicantType();
            _ra.SetEmploymentType();

            ApplicationRepo.SaveApplication(_ra);

            if (createnew || helpDeskRework)          
                return CreateSQLReport(_ra.Key, "Readvance_" + _accountKey.ToString() + ".pdf", ReportType.Readvance);

            return "";
        }

        private string CreateFurtherAdvanceAndReport(bool createnew, bool helpDeskRework)
        {
            _fa.SetApplicantType();
            _fa.SetEmploymentType();

            ApplicationRepo.SaveApplication(_fa);

            if (createnew || helpDeskRework)
                return CreateSQLReport(_fa.Key, "FurtherAdvance_" + _accountKey.ToString() + ".pdf", ReportType.FurtherAdvance);

            return "";
        }

        private string CreateFurtherLoanAndReport(bool createnew, bool helpDeskRework)
        {
            _fl.SetApplicantType();
            _fl.SetEmploymentType();

            ApplicationRepo.SaveApplication(_fl);

            if (createnew || helpDeskRework)
                return CreateSQLReport(_fl.Key, "FurtherLoan_" + _accountKey.ToString() + ".pdf", ReportType.FurtherLoan);

            return "";
        }

        private void GetApplicationData(IApplicationProduct cp)
        {
            ISupportsVariableLoanApplicationInformation prod = cp as ISupportsVariableLoanApplicationInformation;
            _loanAgreementAmount = prod.VariableLoanInformation.LoanAmountNoFees.HasValue ? prod.VariableLoanInformation.LoanAmountNoFees.Value : 0;
            _valuationAmount = prod.VariableLoanInformation.PropertyValuation.HasValue ? prod.VariableLoanInformation.PropertyValuation.Value : 0;
            _rate = prod.VariableLoanInformation.RateConfiguration.Margin.Value;
            _ratekey = prod.VariableLoanInformation.RateConfiguration.Margin.Key;
            _bondToRegister = prod.VariableLoanInformation.BondToRegister.HasValue ? prod.VariableLoanInformation.BondToRegister.Value : 0;
            _householdIncome = prod.VariableLoanInformation.HouseholdIncome.HasValue ? prod.VariableLoanInformation.HouseholdIncome.Value : 0;

            IApplicationMortgageLoan appML = (IApplicationMortgageLoan)cp.Application;
            double estimatedVal = appML.ClientEstimatePropertyValuation.HasValue ? appML.ClientEstimatePropertyValuation.Value : 0;

            if (estimatedVal > _valuationAmount)
                _valuationAmount = estimatedVal;
        }

        private void CreditStageSave()
        {
            ExclusionSets.Add(RuleExclusionSets.LoanDetailsUpdate);
            //If we are here, then a revision and recalc is required
            //This will be for credit approval screens only, so _app 
            //is only loaded once with the application being assessed

            //reload the app from the db before we create a revision, 
            //so that we can see the changes in the original vs revised offer
            _app.Refresh();
            //Create the revision
            _app.CreateRevision();
            _app.SetEmploymentType();
            ApplicationRepo.SaveApplication(_app);

            //And recalc
            ApplicationCalculate(true);
            ApplicationRepo.SaveApplication(_app);

            ExclusionSets.Remove(RuleExclusionSets.LoanDetailsUpdate);
        }

        private string CreateSQLReport(int appKey, string fileName, ReportType reportType)
        {
            int reportKey = 0;

            IDictionary<string, string> prmList = new Dictionary<string, string>();
            prmList.Add("CoverLetter", "YES");
            prmList.Add("Section", "Help Desk");
            prmList.Add("Name", _adUserName);
            prmList.Add("Client", _view.SelectedLegalEntity.DisplayName);
            prmList.Add("AccountKey", _accountKey.ToString());
            prmList.Add("OfferKey", appKey.ToString());

            switch (reportType)
            {
                case ReportType.Readvance:
                    prmList.Add("OfferType", "3");
                    reportKey = (int)ReportStatements.RapidReadvanceApplication;
                    break;
                case ReportType.FurtherAdvance:
                    prmList.Add("OfferType", "1");
                    reportKey = (int)ReportStatements.FurtherAdvanceApplication;
                    break;
                case ReportType.FurtherLoan:
                    prmList.Add("OfferType", "2");
                    reportKey = (int)ReportStatements.FurtherLoanApplication;
                    break;
                default:
                    break;
            }

            return GenerateSQLReport(fileName, reportKey, prmList);
        }

        private string GenerateSQLReport(string fileName, int reportKey, IDictionary<string, string> prmList)
        {
            return GenerateSQLReport(fileName, reportKey, prmList, false);
        }

        private string GenerateSQLReport(string fileName, int reportKey, IDictionary<string, string> prmList, bool writeToDataStor)
        {
            IReportStatement rs = RepRepo.GetReportStatementByKey(reportKey);

            //1. Create SQL Report
            string renderedMessage;
            byte[] rpt = RepRepo.RenderSQLReport(rs.StatementName, prmList, out renderedMessage);

            if (!String.IsNullOrEmpty(renderedMessage))
            {
                throw new NullReferenceException("Report " + rs.StatementName + " could not be created, description is: " + renderedMessage);
            }

            // start impersonation 
            ISecurityService securityService = ServiceFactory.GetService<ISecurityService>();
            WindowsImpersonationContext wic = securityService.BeginImpersonation();

            try
            {
                //2. Save Report
                SaveFile(rpt, rs.ReportOutputPath, fileName);
            }
            catch
            {
                throw;
            }
            finally
            {
                // end impersonation 
                securityService.EndImpersonation(wic);
            }

            //4. Write the correspondence information & datastor (if required) for History purposes
            #region Correspondence History & DataSTOR

            // setup the variables to use for datastor
            string outputFileDataSTOR = "", dataSTORDirectory = "", guid = "", dataStageDirectory = "";
            string dataStorName = SAHL.Common.Constants.DataSTOR.PersonalLoans; // HALO Correspondence
            IDataStorRepository dataStorRepository = RepositoryFactory.GetRepository<IDataStorRepository>();
            ISTOR stor = dataStorRepository.GetSTORByName(dataStorName);
            dataStageDirectory = _lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.CorrespondenceDataStagingFolder].ControlText;
            IADUser adUser = RepositoryFactory.GetRepository<ISecurityRepository>().GetADUserByPrincipal(_view.CurrentPrincipal);
            guid = System.Guid.NewGuid().ToString("B").ToUpper();
            dataSTORDirectory = stor.Folder + @"\" + DateTime.Today.Year.ToString() + @"\" + DateTime.Today.Month.ToString("00");
            outputFileDataSTOR = dataSTORDirectory + @"\" + guid;

            foreach (var correspondenceSelection in _view.SendingInformation)
            {
                ICorrespondenceMedium correspondenceMedium = LookupRepo.CorrespondenceMediums.ObjectDictionary[Convert.ToString(correspondenceSelection.CorrespondenceMediumKey)];

                ICorrespondence corr = CorrespondRepo.CreateEmptyCorrespondence();
                corr.ChangeDate = DateTime.Now;
                corr.CompletedDate = DateTime.Now;
                corr.CorrespondenceMedium = correspondenceMedium;
                corr.DestinationValue = correspondenceSelection.ContactInfo;
                corr.DueDate = DateTime.Now;
                corr.GenericKey = _accountKey;
                corr.GenericKeyType = LookupRepo.GenericKeyType.ObjectDictionary[Convert.ToString((int)GenericKeyTypes.Account)];
                corr.LegalEntity = LERepository.GetLegalEntityByKey(correspondenceSelection.LegalEntityKey);
                corr.ReportStatement = rs;
                corr.UserID = _view.CurrentPrincipal.Identity.Name;

                // if we are writing to data STOR then we need to store the output file path in the correspondence record
                if (writeToDataStor)
                    corr.OutputFile = outputFileDataSTOR;

                CorrespondRepo.SaveCorrespondence(corr);

                // write to DataSTOR if required
                if (writeToDataStor)
                {
                    #region Write to DataSTOR

                    // create an empty data object
                    IData data = dataStorRepository.CreateEmptyData();

                    // populate the data object
                    DateTime dateNow = System.DateTime.Now;
                    string now = dateNow.ToString("yyyy-MM-dd hh:mm:ss");
                    string userName = adUser.LegalEntity == null ? adUser.ADUserName : adUser.LegalEntity.GetLegalName(LegalNameFormat.FullNoSalutation) + " (" + adUser.ADUserName + ")";
                    ILegalEntity le = _leRepository.GetLegalEntityByKey(correspondenceSelection.LegalEntityKey);

                    string originationSource = _account.OriginationSource.Description + ":" + _account.Product.Description;

                    data.ArchiveDate = now;
                    data.STOR = stor.Key;
                    data.GUID = guid;
                    data.Extension = "PDF";
                    data.MsgSubject = _accountKey.ToString();
                    data.OriginalFilename = fileName;
                    data.Key1 = _accountKey.ToString();
                    data.Key2 = originationSource;
                    data.Key3 = _accountKey.ToString();
                    data.Key4 = rs.ReportName;
                    data.Key5 = correspondenceMedium.Description;
                    data.Key6 = correspondenceSelection.ContactInfo;
                    data.Key7 = now;
                    data.Key8 = userName;
                    data.Key9 = "";
                    data.Key10 = rs.Key.ToString();
                    data.Key11 = fileName;
                    data.Key12 = le.GetLegalName(LegalNameFormat.Full);
                    switch (le.LegalEntityType.Key)
                    {
                        case (int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson:
                            data.Key13 = ((ILegalEntityNaturalPerson)le).IDNumber;
                            break;
                        case (int)SAHL.Common.Globals.LegalEntityTypes.CloseCorporation:
                            data.Key13 = ((ILegalEntityCloseCorporation)le).RegistrationNumber;
                            break;
                        case (int)SAHL.Common.Globals.LegalEntityTypes.Company:
                            data.Key13 = ((ILegalEntityCompany)le).RegistrationNumber;
                            break;
                        case (int)SAHL.Common.Globals.LegalEntityTypes.Trust:
                            data.Key13 = ((ILegalEntityTrust)le).RegistrationNumber;
                            break;
                        case (int)SAHL.Common.Globals.LegalEntityTypes.Unknown:
                            data.Key13 = "";
                            break;
                        default:
                            break;
                    }

                    // save the data
                    dataStorRepository.SaveData(data);

                    // save the rendered pdf - note that we need to impersonate a user with local permissions and rights
                    // to the remote folder otherwise we get authentication issues thanks to Kerberos vs. the SAHL network setup
                    wic = securityService.BeginImpersonation();

                    try
                    {
                        // Check if our output directory exists - if not then create
                        if (!Directory.Exists(dataSTORDirectory))
                            Directory.CreateDirectory(dataSTORDirectory);

                        // save to the report to the dataSTOR server path
                        FileStream fs = new FileStream(outputFileDataSTOR, FileMode.Create);
                        fs.Write(rpt, 0, rpt.Length);
                        fs.Close();
                        fs.Dispose();
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        // end impersonation 
                        securityService.EndImpersonation(wic);
                    }

                    #endregion
                }
            }


            #endregion

            return rs.ReportOutputPath + fileName;
        }

        private void SendToClient(List<string> attachments)
        {
            string adminEmail = ControlRepo.GetControlByDescription("SAHL Admin Email").ControlText;

            foreach (var correspondenceSelection in _view.SendingInformation)
            {
                // Get CorrespondenceMediumKey from value

                switch (correspondenceSelection.CorrespondenceMediumKey)
                {
                    case (int)SAHL.Common.Globals.CorrespondenceMediums.Email:
                        // send the email to the Client using the attachement created above
                        MsgSvc.SendEmailExternal(_accountKey
                           , adminEmail
                           , correspondenceSelection.ContactInfo   //_view.SendTo
                           , ""
                           , ""
                           , SUBJECT
                           , BODY
                           , attachments.ToArray());

                        break;
                    case (int)SAHL.Common.Globals.CorrespondenceMediums.Fax:
                        // send the fax to the Client using the attachement created above
                        MsgSvc.SendFaxMultipleDocs(_accountKey
                           , adminEmail
                           , correspondenceSelection.ContactInfo
                           , ""
                           , ""
                           , SUBJECT
                           , BODY
                           , attachments.ToArray());

                        break;
                    default:
                        break;
                }
            }


        }

        private static void SaveFile(byte[] fileContent, string path, string fileName)
        {
            //todo: remove the null check, rather fallover properly if there is a problem
            if (fileContent != null)
            {
                IOUtils.Save(fileContent, path, fileName);
            }
        }

        protected void CheckQCReasonsAndValues()
        {
            _view.BtnSubmitEnabled = false;

            ISupportsQuickCashApplicationInformation qcAI = _app as ISupportsQuickCashApplicationInformation;

            if (qcAI == null)
            {
                _view.BtnSubmitEnabled = true;
            }
            else
            {
                if (qcAI.ApplicationInformationQuickCash.CreditApprovedAmount > 0 || qcAI.ApplicationInformationQuickCash.CreditUpfrontApprovedAmount > 0)
                    _view.BtnSubmitEnabled = true;
                else
                {
                    IReadOnlyEventList<IReason> reasons = ReasonRepo.GetReasonByGenericKeyAndReasonTypeKey(_app.GetLatestApplicationInformation().Key, (int)ReasonTypes.QuickCashDecline);
                    if (reasons != null && reasons.Count > 0)
                        _view.BtnSubmitEnabled = true;
                }
            }
        }

        protected void CheckOpenX2NTU()
        {
            _view.ExistingApplicationMessage = ApplicationRepo.GetFurtherLendingX2NTU(_accountKey);

            //if there is a message then update these 
            //to make sure the waring displays and processing is disabled
            if (_view.ExistingApplicationMessage.Length > 0)
            {
                _view.ReadvanceInProgress = true;
                _view.CanUpdate = false;
            }
        }

        protected void DoITCCheck()
        {
            int errMsgCount = _view.Messages.HasErrorMessages ? _view.Messages.ErrorMessages.Count : 0;

            RuleServ.ExecuteRule(_view.Messages, "ITCApplication", _app);

            int errMsgCountAfter = _view.Messages.HasErrorMessages ? _view.Messages.ErrorMessages.Count : 0;

            if (!_view.IsValid && errMsgCount != errMsgCountAfter)
                _view.BtnSubmitEnabled = false; ;
        }


        #region Repos & Svcs

        protected IReasonRepository ReasonRepo
        {
            get
            {
                if (_reasonRepo == null)
                    _reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();

                return _reasonRepo;
            }
        }

        protected IAccountRepository AccountRepo
        {
            get
            {
                if (_accountRepo == null)
                    _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                return _accountRepo;
            }
        }

        private ILegalEntityRepository LERepository
        {
            get
            {
                if (_leRepository == null)
                    _leRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                return _leRepository;
            }
        }

        private IApplicationRepository ApplicationRepo
        {
            get
            {
                if (_applicationRepo == null)
                    _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _applicationRepo;
            }
        }

        protected ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
        }

        private ICreditMatrixRepository CMRepo
        {
            get
            {
                if (_cmRepo == null)
                    _cmRepo = RepositoryFactory.GetRepository<ICreditMatrixRepository>();

                return _cmRepo;
            }
        }

        private ICorrespondenceRepository CorrespondRepo
        {
            get
            {
                if (_correspondRepo == null)
                    _correspondRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();

                return _correspondRepo;
            }
        }

        private IOrganisationStructureRepository OrgRepo
        {
            get
            {
                if (_orgRepo == null)
                    _orgRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

                return _orgRepo;
            }
        }

        private ISPVService SPVService
        {
            get
            {
                if (_spvService == null)
                    _spvService = ServiceFactory.GetService<ISPVService>();

                return _spvService;
            }
        }

        //private IFinancialsService FinsService
        //{
        //    get
        //    {
        //        if (_finsService == null)
        //            _finsService = ServiceFactory.GetService<IFinancialsService>();

        //        return _finsService;
        //    }
        //}

        private IReportRepository RepRepo
        {
            get
            {
                if (_repRepo == null)
                    _repRepo = RepositoryFactory.GetRepository<IReportRepository>();

                return _repRepo;
            }
        }

        private IMessageService MsgSvc
        {
            get
            {
                if (_msgSvc == null)
                    _msgSvc = ServiceFactory.GetService<IMessageService>();

                return _msgSvc;
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

        private IControlRepository ControlRepo
        {
            get
            {
                if (_controlRepo == null)
                    _controlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                return _controlRepo;
            }
        }

        private IX2Repository X2Repo
        {
            get
            {
                if (_x2Repo == null)
                    _x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
                return _x2Repo;
            }
        }


        #endregion
    }
}
