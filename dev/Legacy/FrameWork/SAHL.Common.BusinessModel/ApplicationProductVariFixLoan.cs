using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel
{
    public class ApplicationProductVariFixLoan : ApplicationProductMortgageLoan, IApplicationProductVariFixLoan
    {
        protected override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            if (!(this.Application is IApplicationFurtherLending))
            {
                Rules.Add("ProductVarifixApplicationMinLoanAmount");
                Rules.Add("ProductVarifixApplicationFixedMinimum");
                Rules.Add("ProductVarifixOptInLoanTransaction");
            }
        }

        public ApplicationProductVariFixLoan(IApplication Application, bool CreateNew)
            : base(Application, CreateNew)
        {
            if (CreateNew)
            {
                // create an specific informations required by this product
                // variable loan
                ApplicationInformationVariableLoan_DAO VL = new ApplicationInformationVariableLoan_DAO();
                VL.ApplicationInformation = _appInfoDAO;
                _appInfoDAO.ApplicationInformationVariableLoan = VL;
                // varifix
                ApplicationInformationVarifixLoan_DAO VF = new ApplicationInformationVarifixLoan_DAO();
                VF.ApplicationInformation = _appInfoDAO;
                _appInfoDAO.ApplicationInformationVarifixLoan = VF;

                if (this.Application is IApplicationFurtherLending)
                    VF.ConversionStatus = 3;

                if (clone)
                {
                    if (_appInfoDAOPrevious.ApplicationInformationVariableLoan != null)
                        _appInfoDAOPrevious.ApplicationInformationVariableLoan.Clone(VL);
                    // possibly need an else for initialization.

                    if (_appInfoDAOPrevious.ApplicationInformationVarifixLoan != null)
                        _appInfoDAOPrevious.ApplicationInformationVarifixLoan.Clone(VF);
                    else
                    {
                        VF.FixedPercent = 1.0d;
                    }
                    GetLatestApplicationInformation().ApplicationInsertDate = DateTime.Now;

                    // Need to loop through the AppInfoRateOverrides and clone them.
                    if ((int)Application.CurrentProduct.ProductType == (int)Products.VariFixLoan)
                    {
                        // Need to loop through the AppInfoRateOverrides and clone them.
                        foreach (ApplicationInformationFinancialAdjustment_DAO financialAdjustment in _appInfoDAOPrevious.ApplicationInformationFinancialAdjustments)
                        {
                            ApplicationInformationFinancialAdjustment_DAO newRateOverride = new ApplicationInformationFinancialAdjustment_DAO();
                            newRateOverride.ApplicationInformation = _appInfoDAO;
                            financialAdjustment.Clone(newRateOverride);
                            _appInfoDAO.ApplicationInformationFinancialAdjustments.Add(newRateOverride);
                        }
                    }
                }
            }
        }

        public ApplicationProductVariFixLoan(IApplicationInformation ApplicationInformation)
            : base(ApplicationInformation)
        {
        }

        #region IApplicationProductVariFixLoan Members

        public IApplicationInformationVarifixLoan VariFixInformation
        {
            get
            {
                if (_appInfoDAO != null)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationVarifixLoan, ApplicationInformationVarifixLoan_DAO>(_appInfoDAO.ApplicationInformationVarifixLoan);
                }
                throw new NullReferenceException("Application Information could not be retrieved.");
            }
        }

        public IApplicationInformationVariableLoan VariableLoanInformation
        {
            get
            {
                if (_appInfoDAO != null)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationVariableLoan, ApplicationInformationVariableLoan_DAO>(_appInfoDAO.ApplicationInformationVariableLoan);
                }
                throw new NullReferenceException("Application Information could not be retrieved.");
            }
        }

        #endregion IApplicationProductVariFixLoan Members

        #region IApplicationProductMortgageLoan

        public override void RecalculateMortgageLoanDetails()
        {
        }

        public override int? Term
        {
            get
            {
                return (_appInfoDAO.ApplicationInformationVariableLoan.Term);
            }
            set
            {
                _appInfoDAO.ApplicationInformationVariableLoan.Term = value;
            }
        }

        public override double? LoanAgreementAmount
        {
            get
            {
                return _appInfoDAO.ApplicationInformationVariableLoan.LoanAgreementAmount;
            }
            //set
            //{
            //    _appInfoDAO.ApplicationInformationVariableLoan.LoanAgreementAmount = value;
            //}
        }

        public override double? LoanAmountNoFees
        {
            get
            {
                return _appInfoDAO.ApplicationInformationVariableLoan.LoanAmountNoFees;
            }
            set
            {
                _appInfoDAO.ApplicationInformationVariableLoan.LoanAmountNoFees = value;
                ApplicationProductMortgageLoanHelper.SetLoanAgreementAmount(this);
            }
        }

        public override void SetManualDiscount(IDomainMessageCollection Messages, double? discount, FinancialAdjustmentTypeSources roType)
        {
            ApplicationProductMortgageLoanHelper.SetManualDiscount(this, discount, roType, Messages);
        }

        public override double? ManualDiscount
        {
            get
            {
                return ApplicationProductMortgageLoanHelper.GetManualDiscount(this);
            }
        }

        public override double? DiscountedLinkRate
        {
            get
            {
                return ApplicationProductMortgageLoanHelper.GetDiscountedLinkRate(this);
            }
        }

        public override double? MarketRate
        {
            get
            {
                return ApplicationProductMortgageLoanHelper.GetMarketRate(this);
            }
        }

        public override double? EffectiveRate
        {
            get
            {
                return ApplicationProductMortgageLoanHelper.GetEffectiveRate(this);
            }
        }

        #endregion IApplicationProductMortgageLoan

        #region IApplicationProductVariFixLoan Members

        public double? FixedEffectiveRate
        {
            get
            {
                return (ApplicationProductMortgageLoanHelper.GetFixedMarketRate(VariFixInformation.MarketRate.Key) ?? 0.0) + (ApplicationProductMortgageLoanHelper.GetDiscountedLinkRate(this) ?? 0.0);
            }
        }

        public double? FixedInstalment
        {
            get
            {
                return this.VariFixInformation.FixedInstallment;
            }
        }

        public double? FixedMarketRate
        {
            get
            {
                return ApplicationProductMortgageLoanHelper.GetFixedMarketRate(VariFixInformation.MarketRate.Key);
            }
        }

        public double? FixedPercentage
        {
            get
            {
                return this.VariFixInformation.FixedPercent;
            }
            set
            {
                this.VariFixInformation.FixedPercent = value.Value;
            }
        }

        public double? FixedRandValue
        {
            get
            {
                return (ApplicationProductMortgageLoanHelper.GetLoanAgreementAmount(this) ?? 0) * (this.VariFixInformation.FixedPercent);
            }
        }

        public double? VariableEffectiveRate
        {
            get
            {
                return (ApplicationProductMortgageLoanHelper.GetMarketRate(this) ?? 0.0) + (ApplicationProductMortgageLoanHelper.GetDiscountedLinkRate(this) ?? 0.0);
            }
        }

        public double? VariableInstalment
        {
            get
            {
                return (this.VariableLoanInformation.MonthlyInstalment - this.FixedInstalment);
            }
        }

        public double? LinkRate
        {
            get
            {
                return ApplicationProductMortgageLoanHelper.GetLinkRate(this) ?? 0.0;
            }
        }

        public double? VariableMarketRate
        {
            get
            {
                return ApplicationProductMortgageLoanHelper.GetMarketRate(this);
            }
        }

        public double? VariablePercentage
        {
            get
            {
                return (1.0 - this.VariFixInformation.FixedPercent);
            }
        }

        public double? VariableRandValue
        {
            get
            {
                return (ApplicationProductMortgageLoanHelper.GetLoanAgreementAmount(this) ?? 0) * (1.0 - this.VariFixInformation.FixedPercent);
            }
        }

        #endregion IApplicationProductVariFixLoan Members
    }
}