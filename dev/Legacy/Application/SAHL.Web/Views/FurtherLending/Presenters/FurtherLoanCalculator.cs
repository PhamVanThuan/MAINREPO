using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.FurtherLending.Interfaces;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common;
using Castle.ActiveRecord;


namespace SAHL.Web.Views.FurtherLending.Presenters
{
    public class FurtherLoanCalculator : SAHLCommonBasePresenter<IFurtherLoanCalculator>
    {
        private CBONode _cboNode;
        private InstanceNode _instanceNode;
        private int _accountKey = -1;
        private IAccountRepository _accountRepo;
        private IAccount _account;
        private IMortgageLoan _vML;
        private IMortgageLoan _fML;
        private int _ArrearMonthCheck = 6;
        private float _ArrearMinimumValueCheck = 500;
        private double _NewInstalment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Controller"></param>
        public FurtherLoanCalculator(IFurtherLoanCalculator View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            //// Set the CBO Node manually for testing purposes : NB !!! MUST BE REMOVED ONCE CBO IS WORKING
            Int32 AccountKey;
            //AccountKey = 1509295; //Staff
            //AccountKey = 1235894; //Std Var
            //AccountKey = 1247804; // VariFix 1247804
            //AccountKey = 1236449; //Active Life
            //AccountKey = 1236390; //Inactive Life
            AccountKey = 1406200; //Subsidy
            InstanceNode IN = new InstanceNode(0, null, "A LoanAccount", "A sample Loan account", AccountKey, null);
            _cboNode = IN;

            //// Get the CBO Node  -- uncomment the line below once the cbo is working     
            ///_cboNode = base.CBOService.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSet.X2NODESET);
            if (_cboNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            // Get the AccountKey from the CBO
            if (_cboNode is InstanceNode)
            {
                _instanceNode = _cboNode as InstanceNode;
                _accountKey = Convert.ToInt32(_instanceNode.GenericKey); //Not sure if the BusinessKey is correct here...
            }
            else
            {
                _accountKey = Convert.ToInt32(_cboNode.GenericKey);
            }

            _accountKey = AccountKey;
            // Get the Account Object
            _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _account = _accountRepo.GetAccountByKey(_accountKey);
            
            //Make sure we have a Valid account 
            if ((_account as IMortgageLoanAccount) == null)
            {
                throw new Exception("Not a Mortgage Loan Account!");
            }

            //Get the variable ML
            IMortgageLoanAccount _MortgageLoanAccount = _account as IMortgageLoanAccount;
            _vML = _MortgageLoanAccount.SecuredMortgageLoan;

            //Get the Fixed ML  
            if ((_account as IAccountVariFixLoan) != null)
            {
                IAccountVariFixLoan _fAccount = _account as IAccountVariFixLoan;
                _fML = _fAccount.FixedSecuredMortgageLoan;
            }

            _view.HasArrears = _account.ArrearsCheck(_view.Messages, _ArrearMonthCheck, _ArrearMinimumValueCheck);
            _view.IsInterestOnly = _vML.HasInterestOnly();
            
            //Existing Further Lending applications
            CheckExistingApplications();

            //Lookups
            ILookupRepository _LookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IEventList<IEmploymentType> _EmpTypes = _LookupRepo.EmploymentTypes;
            IEventList<IMargin> _Margins = _vML.GetMargins();
  
            //Bind the data we have collected
            _view.BindEmploymentTypes(_EmpTypes);
            _view.BindLinkRates(_Margins);
            _view.BindDisplay(_vML, _fML, _account, false);

            _view.OnCalculateButtonClicked += new EventHandler(_view_OnCalculateButtonClicked);
            _view.OnContactUpdateButtonClicked += new EventHandler(_view_ContactUpdateButtonClicked);
            _view.OnGenerateButtonClicked += new EventHandler(_view_GenerateButtonClicked);
        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnCalculateButtonClicked(object sender, EventArgs e)
        {
         
            // Do calcs
            if (_view.TotalCashRequired > 0)
            {
                //First calc LTV for other calcs
                _view.NewCurrentBalance = _view.NewCurrentBalance;
                double newLTV = _view.NewCurrentBalance / _view.EstimatedValuationAmount;
                _view.NewLTV = newLTV;

                //Get new Amortising instalment
                _NewInstalment = LoanCalculator.CalculateFurtherLendingInstallment(_view.NewCurrentBalance, (_view.RateSelected + _view.BaseRateVariable + _view.DiscountVariable), _view.RemainingTerm, _view.CurrentBalanceVF, (_view.RateSelected + _view.BaseRateFixed + _view.DiscountFixed), false);

                //PTI is always calculated against amortising installment
                _view.NewPTI = _NewInstalment / (_view.NewIncome1 + _view.NewIncome2 > 0 ? _view.NewIncome1 + _view.NewIncome2 : _view.HouseholdIncome);

                // Calculate the Interest Only instalment if the account is Interest Only
                if (_view.IsInterestOnly)
                    _NewInstalment = LoanCalculator.CalculateFurtherLendingInstallment(_view.NewCurrentBalance, (_view.RateSelected + _view.BaseRateVariable + _view.DiscountVariable), _view.RemainingTerm, _view.CurrentBalanceVF, (_view.RateSelected + _view.BaseRateFixed + _view.DiscountFixed), true);

                _view.NewInstalment = _NewInstalment;
                _view.NewSPV = NewSPVDescriptionFromLTV(newLTV, _view.SPVCompany);

                _view.Fees = LoanCalculator.CalculateInitiationFees(_view.FurtherLoanRequired);

            }
            else
            {
                _view.NewCurrentBalance = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LTV"></param>
        /// <param name="SPVCompany"></param>
        /// <returns></returns>
        private static string NewSPVDescriptionFromLTV(double LTV, int SPVCompany)
        {
            ISPVService SPVService = ServiceFactory.GetService<ISPVService>();
            int newSPV = SPVService.GetSPVByLTV(LTV, SPVCompany);

            ILookupRepository _LookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IEventList<ISPV> _SPVList = _LookupRepo.SPVList;

            foreach (ISPV spv in _SPVList)
            {
                if (spv.Key == newSPV)
                    return spv.Description;
            }

            return "No SPV found";
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckExistingApplications()
        {
            IApplicationRepository ApplicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            IEventList<IApplication> _application = ApplicationRepo.GetApplicationByAccountKey(_accountKey);

            foreach (IApplication app in _application)
            {
                if (app.ApplicationStatus.Key == Convert.ToInt16(OfferStatuses.Open))
                {

                    switch ((OfferTypes)app.Key)
                    {
                        case OfferTypes.ReAdvance:
                            _view.ReadvanceInProgress = true;
                            //app.ApplicationMortgageLoan.ApplicationAmount;
                            break;
                        case OfferTypes.FurtherAdvance:
                            _view.FurtherAdvanceInProgress = true;
                            break;
                        case OfferTypes.FurtherLoan:
                            _view.FurtherLoanInProgress = true;
                            break;
                        default:
                            break;
                    }
                }
                
            }
        }

        /// <summary>
        /// Update the Contact details to the db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_ContactUpdateButtonClicked(object sender, EventArgs e)
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

            ILegalEntityRepository leRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();

            using (new TransactionScope(TransactionMode.Inherits))
            {
                leRepository.SaveLegalEntity(lE);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_GenerateButtonClicked(object sender, EventArgs e)
        {
            #warning TODO: Create applications when Attie has done the domain updates
            throw new NotImplementedException();

        }
    }
}
