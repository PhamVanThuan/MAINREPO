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
    public class ApplicationProductEdge : ApplicationProductMortgageLoan, IApplicationProductEdge
    {
        protected override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("ApplicationProductEdgeTerm");
        }

        public ApplicationProductEdge(IApplication Application, bool CreateNew)
            : base(Application, CreateNew)
        {
            if (CreateNew)
            {
                // create an specific informations required by this product
                // variable loan
                ApplicationInformationVariableLoan_DAO VL = new ApplicationInformationVariableLoan_DAO();
                VL.ApplicationInformation = _appInfoDAO;
                _appInfoDAO.ApplicationInformationVariableLoan = VL;
                // interestonly
                ApplicationInformationInterestOnly_DAO IO = new ApplicationInformationInterestOnly_DAO();
                IO.ApplicationInformation = _appInfoDAO;
                _appInfoDAO.ApplicationInformationInterestOnly = IO;
                // Edge
                ApplicationInformationEdge_DAO Edge = new ApplicationInformationEdge_DAO();
                Edge.ApplicationInformation = _appInfoDAO;
                _appInfoDAO.ApplicationInformationEdge = Edge;
                if (clone)
                {
                    if (_appInfoDAOPrevious.ApplicationInformationInterestOnly != null)
                        _appInfoDAOPrevious.ApplicationInformationInterestOnly.Clone(IO);
                    // possibly need an else for initialization.
                    if (_appInfoDAOPrevious.ApplicationInformationVariableLoan != null)
                        _appInfoDAOPrevious.ApplicationInformationVariableLoan.Clone(VL);
                    // possibly need an else for initialization.
                    if (_appInfoDAOPrevious.ApplicationInformationEdge != null)
                        _appInfoDAOPrevious.ApplicationInformationEdge.Clone(Edge);
                    // possibly need an else for initialization.
                    GetLatestApplicationInformation().ApplicationInsertDate = DateTime.Now;
                    if ((int)Application.CurrentProduct.ProductType == (int)Products.Edge)
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
            else
            {
                // get the existing Vl and IO create new ones and copy their values to the newly created ones.
            }
        }

        public ApplicationProductEdge(IApplicationInformation ApplicationInformation)
            : base(ApplicationInformation)
        {
        }

        #region IApplicationProduct Members

        #endregion IApplicationProduct Members

        #region IApplicationProductEdge Members

        public IApplicationInformationInterestOnly InterestOnlyInformation
        {
            get
            {
                if (_appInfoDAO != null)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationInterestOnly, ApplicationInformationInterestOnly_DAO>(_appInfoDAO.ApplicationInformationInterestOnly);
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

        public IApplicationInformationEdge EdgeInformation
        {
            get
            {
                if (_appInfoDAO != null)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationEdge, ApplicationInformationEdge_DAO>(_appInfoDAO.ApplicationInformationEdge);
                }
                throw new NullReferenceException("Application Information could not be retrieved.");
            }
        }

        #endregion IApplicationProductEdge Members

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

        //public override void OnPopulateRules(List<string> Rules)
        //{
        //    base.OnPopulateRules(Rules);

        //    Rules.Add("NewVariableMinimumLoanAmount");
        //    Rules.Add("NewVariableMaximumLoanAmount");
        //}
    }
}