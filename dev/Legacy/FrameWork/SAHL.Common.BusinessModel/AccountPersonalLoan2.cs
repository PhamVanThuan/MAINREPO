using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from Account_DAO and is instantiated to represent Personal Loan accounts.
    /// </summary>
    public partial class AccountPersonalLoan : Account, IAccountPersonalLoan
    {
        public override SAHL.Common.Globals.AccountTypes AccountType
        {
            get { return SAHL.Common.Globals.AccountTypes.Unsecured; }
        }

        public double AccruedInterestMTD
        {
            get
            {
                var openPersonalLoanFinancialServices = this.GetFinancialServicesByType(SAHL.Common.Globals.FinancialServiceTypes.PersonalLoan, new SAHL.Common.Globals.AccountStatuses[] { SAHL.Common.Globals.AccountStatuses.Open, SAHL.Common.Globals.AccountStatuses.Dormant });
                var openPersonalLoanFinancialService = openPersonalLoanFinancialServices.First();
                return openPersonalLoanFinancialService.Balance.LoanBalance.MTDInterest;
            }
        }

        public void CalculateInterest(out double interestMonthToDate, out double interestTotalforMonth)
        {
            interestTotalforMonth = 0;
            interestMonthToDate = 0;

            var openPersonalLoanFinancialServices = this.GetFinancialServicesByType(SAHL.Common.Globals.FinancialServiceTypes.PersonalLoan, new SAHL.Common.Globals.AccountStatuses[] { SAHL.Common.Globals.AccountStatuses.Open, SAHL.Common.Globals.AccountStatuses.Dormant });

            //We should just have 1 open personal loan financial service
            var openPersonalLoanFinancialService = openPersonalLoanFinancialServices.First();

            interestMonthToDate = openPersonalLoanFinancialService.Balance.LoanBalance.MTDInterest;

            DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime toDate = new DateTime(DateTime.Today.AddMonths(1).Year, DateTime.Today.AddMonths(1).Month, 1);
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@FSKey", openPersonalLoanFinancialService.Key));
            prms.Add(new SqlParameter("@FromDate", fromDate));
            prms.Add(new SqlParameter("@ToDate", toDate));
            SqlParameter amount = new SqlParameter("@IntAmt", SqlDbType.Float);
            amount.Direction = ParameterDirection.Output;
            prms.Add(amount);

            try
            {
                CastleTransactionsServiceHelper.ExecuteHaloAPI_uiS_OnCastleTranForRead("MortgageLoan", "CalculateInterestByFinancialServiceKey", prms);
            }
            catch
            {
            }

            if (amount.Value != DBNull.Value)
            {
                interestTotalforMonth = Convert.ToDouble(prms[3].Value);
            }
        }

        public int MaxNewRemainingInstalmentsAllowed
        {
            get
            {
                IControlRepository controlRepository = RepositoryFactory.GetRepository<IControlRepository>();
                var maxPersonalLoanTerm = Convert.ToInt32(controlRepository.GetControlByDescription(SAHL.Common.Constants.ControlTable.PersonalLoan.MaxPersonalLoanTerm).ControlNumeric);
                var loanBalance = this.FinancialServices.First(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.PersonalLoan &&
                                                                                                    x.AccountStatus.Key == (int)AccountStatuses.Open).Balance.LoanBalance;
                var maxTermExtensionAllowed = maxPersonalLoanTerm - loanBalance.Term + loanBalance.RemainingInstalments;
                return maxTermExtensionAllowed;
            }
        }

        public int MaxTermExtension
        {
            get
            {
                IControlRepository controlRepository = RepositoryFactory.GetRepository<IControlRepository>();
                var maxPersonalLoanTerm = Convert.ToInt32(controlRepository.GetControlByDescription(SAHL.Common.Constants.ControlTable.PersonalLoan.MaxPersonalLoanTerm).ControlNumeric);
                var loanBalance = this.FinancialServices.First(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.PersonalLoan &&
                                                                                                    x.AccountStatus.Key == (int)AccountStatuses.Open).Balance.LoanBalance;
                var maxTermExtensionAllowed = maxPersonalLoanTerm - loanBalance.Term + loanBalance.RemainingInstalments;
                var maxTermExtension = maxTermExtensionAllowed - loanBalance.RemainingInstalments;
                return maxTermExtension;
            }
        }

        private IAccountInstallmentSummary personalLoanInstallmentSummary;

        public override IAccountInstallmentSummary InstallmentSummary
        {
            get
            {
                if (this.personalLoanInstallmentSummary == null)
                    this.personalLoanInstallmentSummary = new UnsecuredLendingAccountSummary(this);
                return this.personalLoanInstallmentSummary;
            }
        }

        /// <summary>
        ///  We should have one external policy per account
        /// </summary>
        public IExternalLifePolicy ExternalLifePolicy
        {
            get
            {
                IExternalLifePolicy externalLifePolicy = null;
                if (_DAO.ExternalLifePolicy != null && _DAO.ExternalLifePolicy.Count > 0)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    externalLifePolicy = BMTM.GetMappedType<IExternalLifePolicy, ExternalLifePolicy_DAO>(_DAO.ExternalLifePolicy.First());
                }
                return externalLifePolicy;
            }
        }
    }
}