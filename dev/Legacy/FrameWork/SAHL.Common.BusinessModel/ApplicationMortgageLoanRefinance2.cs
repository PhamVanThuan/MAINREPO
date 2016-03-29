using System;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class ApplicationMortgageLoanRefinance : Application, IApplicationMortgageLoanRefinance
    {
        ApplicationMortgageLoanHelper _applicationMortgageLoanHelper;
        ApplicationCalculateMortgageLoanHelper _appCalc;

        public void OnConstruction()
        {
            _applicationMortgageLoanHelper = new ApplicationMortgageLoanHelper(_DAO.ApplicationMortgageLoanDetail);
            _appCalc = new ApplicationCalculateMortgageLoanHelper(this as IApplicationMortgageLoan, OfferTypes.RefinanceLoan);
        }

        public override void CalculateApplicationDetail(bool IsBondExceptionAction, bool keepMarketRate)
        {
            _appCalc.CalculateApplicationDetail(IsBondExceptionAction, keepMarketRate);
        }

        #region IApplicationMortgageLoan Members

        public IApplicantType ApplicantType
        {
            get
            {
                return _applicationMortgageLoanHelper.ApplicantType;
            }
            set
            {
                _applicationMortgageLoanHelper.ApplicantType = value;
            }
        }

        public double? ClientEstimatePropertyValuation
        {
            get
            {
                return _applicationMortgageLoanHelper.ClientEstimatePropertyValuation;
            }
            set
            {
                _applicationMortgageLoanHelper.ClientEstimatePropertyValuation = value;
            }
        }

        public int? NumApplicants
        {
            get
            {
                return _applicationMortgageLoanHelper.NumApplicants;
            }
            set
            {
                _applicationMortgageLoanHelper.NumApplicants = value;
            }
        }

        public IResetConfiguration ResetConfiguration
        {
            get
            {
                return _applicationMortgageLoanHelper.ResetConfiguration;
            }
            set
            {
                _applicationMortgageLoanHelper.ResetConfiguration = value;
            }
        }

        public string TransferringAttorney
        {
            get
            {
                return _applicationMortgageLoanHelper.TransferringAttorney;
            }
            set
            {
                _applicationMortgageLoanHelper.TransferringAttorney = value;
            }
        }

        public IProperty Property
        {
            get
            {
                return _applicationMortgageLoanHelper.Property;
            }
            set
            {
                _applicationMortgageLoanHelper.Property = value;
            }
        }

        public double? RequestedCashAmount
        {
            get
            {
                ISupportsVariableLoanApplicationInformation VLI = this.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                if (VLI != null)
                {
                    IApplicationInformationVariableLoanForSwitchAndRefinance VL = VLI.VariableLoanInformation as IApplicationInformationVariableLoanForSwitchAndRefinance;
                    if (VL != null)
                        return VL.RequestedCashAmount;
                }

                throw new NullReferenceException();
            }
            set
            {
                ISupportsVariableLoanApplicationInformation VLI = this.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                if (VLI != null)
                {
                    IApplicationInformationVariableLoanForSwitchAndRefinance VL = VLI.VariableLoanInformation as IApplicationInformationVariableLoanForSwitchAndRefinance;
                    VL.RequestedCashAmount = value;
                }
            }
        }

        //public EmploymentTypes GetEmploymentType(bool UseAllCurrentEmployment)
        //{
        //    return _applicationMortgageLoanHelper.GetEmploymentType(UseAllCurrentEmployment);
        //}

        #endregion IApplicationMortgageLoan Members

        #region IApplicationMortgageLoanRefinance Members

        public void SetProduct(SAHL.Common.Globals.ProductsRefinance NewProduct)
        {
            //Manage previous conversion statuses so that they do not get converted into an old product
            SAHL.Common.BusinessModel.Helpers.ApplicationProductMortgageLoanHelper.SetConversionStatus(this, (int)NewProduct, false);

            switch (NewProduct)
            {
                case SAHL.Common.Globals.ProductsRefinance.DefendingDiscountRate:
                    base._currentProduct = new ApplicationProductDefendingDiscountLoan(this, true);
                    break;
                case SAHL.Common.Globals.ProductsRefinance.NewVariableLoan:
                    base._currentProduct = new ApplicationProductNewVariableLoan(this, true);
                    break;
                case SAHL.Common.Globals.ProductsRefinance.SuperLo:
                    base._currentProduct = new ApplicationProductSuperLoLoan(this, true);
                    break;
                case SAHL.Common.Globals.ProductsRefinance.VariableLoan:
                    base._currentProduct = new ApplicationProductVariableLoan(this, true);
                    break;
                case SAHL.Common.Globals.ProductsRefinance.VariFixLoan:
                    base._currentProduct = new ApplicationProductVariFixLoan(this, true);
                    break;
                case SAHL.Common.Globals.ProductsRefinance.Edge:
                    base._currentProduct = new ApplicationProductEdge(this, true);
                    break;
            }
            // set the applications product
            Product_DAO Prod = Product_DAO.Find(Convert.ToInt32(NewProduct));
            if (null == Prod)
                throw new Exception("Product could not be found, database may be missing data.");
            GetLatestApplicationInformation().Product = new Product(Prod);

            //This code is untested as per TRAC 12129
            //Development was halted awaiting further business investigation
            //ISupportsVariableLoanApplicationInformation vlai = _currentProduct as ISupportsVariableLoanApplicationInformation;
            //if (vlai != null)
            //    vlai.VariableLoanInformation.CreditMatrix = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Amount"></param>
        /// <param name="Override"></param>
        public void SetRegistrationFee(double? Amount, bool Override)
        {
            _appCalc.SetRegistrationFee(Amount, Override);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Amount"></param>
        /// <param name="Override"></param>
        public void SetInitiationFee(double? Amount, bool Override)
        {
            _appCalc.SetInitiationFee(Amount, Override);
        }

        /// <summary>
        /// Get or Set whether the client would like to capitalise any fees to the loan amount.
        /// </summary>
        public bool CapitaliseFees
        {
            get { return _appCalc.CapitaliseFees; }
            set { _appCalc.CapitaliseFees = value; }
        }

        public double? CashOut
        {
            get
            {
                return _appCalc.GetCashOut();
            }
            set
            {
                _appCalc.SetCashOut(value);
            }
        }

        public double? InitiationFee
        {
            get
            {
                return _appCalc.GetInitiationFee();
            }
            //set
            //{
            //    _appCalc.SetInitiationFee(value);
            //}
        }

        public double? RegistrationFee
        {
            get
            {
                return _appCalc.GetRegistrationFee();
            }
            //set
            //{
            //    _appCalc.SetRegistrationFee(value);
            //}
        }

        public double? TotalFees
        {
            get
            {
                return (RegistrationFee ?? 0.0)
                        + (InitiationFee ?? 0.0);
            }
        }

        public double? LoanAgreementAmount
        {
            get
            {
                return _appCalc.GetLoanAgreementAmount();
            }
        }

        #endregion IApplicationMortgageLoanRefinance Members

        #region IApplicationMortgageLoan Members

        public double MinBondRequired
        {
            get { return _appCalc.MinBondRequired; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEventList<IMargin> GetMargins()
        {
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = this.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (supportsVariableLoanApplicationInformation != null)
                return _applicationMortgageLoanHelper.GetMargins(supportsVariableLoanApplicationInformation.VariableLoanInformation.CreditMatrix.Key);
            else
                return null;
        }

        public int? DependentsPerHousehold
        {
            get
            {
                return _applicationMortgageLoanHelper.DependentsPerHousehold;
            }
            set
            {
                _applicationMortgageLoanHelper.DependentsPerHousehold = value;
            }
        }

        public int? ContributingDependents
        {
            get
            {
                return _applicationMortgageLoanHelper.ContributingDependents;
            }
            set
            {
                _applicationMortgageLoanHelper.ContributingDependents = value;
            }
        }

        #endregion IApplicationMortgageLoan Members

        public ILanguage Language
        {
            get
            {
                if (null == _DAO.ApplicationMortgageLoanDetail.Language) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILanguage, Language_DAO>(_DAO.ApplicationMortgageLoanDetail.Language);
                }
            }
            set
            {
                if (value == null)
                {
                    _DAO.ApplicationMortgageLoanDetail.Language = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationMortgageLoanDetail.Language = (Language_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public IApplicationInformationQuickCash ApplicationInformationQuickCash
        {
            get
            {
                return SupportsQuickCashApplicationInformationHelper.ApplicationInformationQuickCash(this.GetLatestApplicationInformation());
            }
        }
    }
}