using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class AccountHOC : Account, IAccountHOC
    {
        #region Properties

        /// <summary>
        /// Returns the HOC object that is linked to this acount. This property will throw an exception if there are more that 1 Financial Service and Null if there are none.
        /// </summary>
        public IHOC HOC
        {
            get
            {
                foreach (IFinancialService service in this.FinancialServices)
                {
                    if (service.FinancialServiceType.Key == (int)FinancialServiceTypes.HomeOwnersCover)
                    {
                        IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
                        return hocRepo.GetHOCByKey(service.Key);
                    }
                }
                // this may happen if the HOC account is in origination phase.
                return null;
            }
        }

        public IMortgageLoanAccount MortgageLoanAccount
        {
            get
            {
                return this.ParentAccount as IMortgageLoanAccount;
            }
        }

        public override SAHL.Common.Globals.AccountTypes AccountType
        {
            get { return SAHL.Common.Globals.AccountTypes.HOC; }
        }

        #endregion Properties

        #region Event Handlers

        #endregion Event Handlers

		public double MonthlyPremium
		{
			get
			{
				return RepositoryFactory.GetRepository<IHOCRepository>().GetMonthlyPremium(this.Key);
			}
		}
    }
}