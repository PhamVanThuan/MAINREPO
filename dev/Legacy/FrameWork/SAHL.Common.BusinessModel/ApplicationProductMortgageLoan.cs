using System.Collections.Generic;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel
{
    public abstract class ApplicationProductMortgageLoan : ApplicationProduct, IApplicationProductMortgageLoan
    {
        public ApplicationProductMortgageLoan(IApplication Application, bool CreateNew)
            : base(Application, CreateNew)
        {
            if (CreateNew)
            {
                if (this.Application is ISupportsQuickCashApplicationInformation)
                {
                    if (_appInfoDAO.ApplicationInformationQuickCash == null)
                    {
                        _appInfoDAO.ApplicationInformationQuickCash = new ApplicationInformationQuickCash_DAO();
                        _appInfoDAO.ApplicationInformationQuickCash.ApplicationInformation = _appInfoDAO;
                    }
                    if (clone)
                    {
                        if (_appInfoDAOPrevious.ApplicationInformationQuickCash != null)
                            _appInfoDAOPrevious.ApplicationInformationQuickCash.Clone(_appInfoDAO.ApplicationInformationQuickCash);
                    }
                }
            }
        }

        public ApplicationProductMortgageLoan(IApplicationInformation ApplicationInformation)
            : base(ApplicationInformation)
        {
        }

        #region IApplicationProductMortgageLoan

        public abstract void RecalculateMortgageLoanDetails();

        public abstract int? Term { get; set; }

        public abstract double? LoanAgreementAmount { get; }

        public abstract double? LoanAmountNoFees { get; set; }

        public abstract void SetManualDiscount(SAHL.Common.Collections.Interfaces.IDomainMessageCollection Messages, double? discount, FinancialAdjustmentTypeSources roType);

        public abstract double? ManualDiscount { get; }

        public abstract double? DiscountedLinkRate { get; }

        public abstract double? MarketRate { get; }

        public abstract double? EffectiveRate { get; }

        #endregion IApplicationProductMortgageLoan

        //public override void OnPopulateRules(List<string> Rules)
        //{
        //    base.OnPopulateRules(Rules);

        //    Rules.Add("ApplicationAssetLiabilityRequiredLoanAmount");
        //}

        protected override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            //Rules.Add("ApplicationAssetLiabilityRequiredLoanAmount");
            //Rules.Add("ApplicationProductMortgageLoanTerm");
        }
    }
}