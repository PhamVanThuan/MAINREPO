using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// </summary>
    [FactoryType(typeof(IGroupExposureRepository))]
    public class GroupExposureRepository : AbstractRepositoryBase, IGroupExposureRepository
    {
        private ICastleTransactionsService castleTransactionsService;
        private ILegalEntityRepository legalEntityRepository;
        private IApplicationRepository applicationRepository;
        private IAccountRepository accountRepository;

        private ILegalEntityRepository LegalEntityRepo
        {
            get
            {
                if (legalEntityRepository == null)
                {
                    legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                }
                return legalEntityRepository;
            }
        }

        private IApplicationRepository ApplicationRepo
        {
            get
            {
                if (applicationRepository == null)
                {
                    applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
                }
                return applicationRepository;
            }
        }

        private IAccountRepository AccountRepo
        {
            get
            {
                if (accountRepository == null)
                {
                    accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
                }
                return accountRepository;
            }
        }

        public GroupExposureRepository()
        {
            if (castleTransactionsService == null)
            {
                castleTransactionsService = new CastleTransactionsService();
            }
        }

        public GroupExposureRepository(ICastleTransactionsService castleTransactionsService, ILegalEntityRepository legalEntityRepository, IApplicationRepository applicationRepository, IAccountRepository accountRepository)
        {
            this.castleTransactionsService = castleTransactionsService;
            this.legalEntityRepository = legalEntityRepository;
            this.applicationRepository = applicationRepository;
            this.accountRepository = accountRepository;
        }

        public List<IGroupExposureItem> GetGroupExposureInfoByLegalEntity(int legalEntityKey)
        {
            var legalEntity = LegalEntityRepo.GetLegalEntityByKey(legalEntityKey);
            var legalEntityGroupExposure = new List<IGroupExposureItem>();
            legalEntityGroupExposure.AddRange(GetUnSecuredLendingApplicationExposure(legalEntity.Key));
            legalEntityGroupExposure.AddRange(GetSecuredLendingApplicationExposure(legalEntity));
            legalEntityGroupExposure.AddRange(GetLoanExposureInfoByLegalEntity(legalEntity));
            return legalEntityGroupExposure;
        }

        private IEnumerable<IGroupExposureItem> GetLoanExposureInfoByLegalEntity(ILegalEntity legalEntity)
        {
            var loanExposureItems = new List<IGroupExposureItem>();
            foreach (IRole accountRole in legalEntity.Roles.Where(r => r.GeneralStatus.Key != (int)GeneralStatuses.Inactive))
            {
                // re-get the account - this will prevent lazy load exceptions (ie. no session)
                IAccount account = AccountRepo.GetAccountByKey(accountRole.Account.Key);

                if (account as IMortgageLoanAccount != null && account.AccountStatus.Key == Convert.ToInt32(AccountStatuses.Open))
                {
                    GroupExposureItem mortgageExposureItem = GetMortgageLoanExposure(account, accountRole.RoleType.Description);
                    loanExposureItems.Add(mortgageExposureItem);
                }
                else if (account as IAccountPersonalLoan != null && account.AccountStatus.Key == Convert.ToInt32(AccountStatuses.Open))
                {
                    GroupExposureItem personalLoanExposure = GetPersonalLoanExposure(account, accountRole.RoleType.Description);
                    loanExposureItems.Add(personalLoanExposure);
                }
            }
            return loanExposureItems;
        }

        private GroupExposureItem GetPersonalLoanExposure(IAccount account, string accountRoleDescription)
        {
            IAccountPersonalLoan personalLoanAccount = account as IAccountPersonalLoan;

            GroupExposureItem personalLoanExposureItem = new GroupExposureItem();
            personalLoanExposureItem.RoleDescription = accountRoleDescription;
            personalLoanExposureItem.Product = account.Product.Description;
            personalLoanExposureItem.AccountKey = account.Key;
            personalLoanExposureItem.OfferKey = null;
            personalLoanExposureItem.Status = account.AccountStatus.Description;

            personalLoanExposureItem.CurrentBalance = personalLoanAccount.InstallmentSummary.CurrentBalance;
            personalLoanExposureItem.ArrearBalance = personalLoanAccount.InstallmentSummary.TotalArrearsBalance;
            personalLoanExposureItem.HouseholdIncome = personalLoanAccount.GetHouseholdIncome();
            personalLoanExposureItem.InstalmentAmount = personalLoanAccount.InstallmentSummary.TotalLoanInstallment;
            personalLoanExposureItem.LoanAgreementAmount = personalLoanAccount.CommittedLoanValue;

            return personalLoanExposureItem;
        }

        private GroupExposureItem GetMortgageLoanExposure(IAccount account, string accountRoleDescription)
        {
            IMortgageLoanAccount mortgageLoanAccount = account as IMortgageLoanAccount;

            GroupExposureItem mortgageLoanExposureItem = new GroupExposureItem();
            mortgageLoanExposureItem.RoleDescription = accountRoleDescription;
            mortgageLoanExposureItem.Product = account.Product.Description;
            mortgageLoanExposureItem.AccountKey = account.Key;
            mortgageLoanExposureItem.OfferKey = null;
            mortgageLoanExposureItem.Status = account.AccountStatus.Description;

            IMortgageLoan mortgageLoan = mortgageLoanAccount.SecuredMortgageLoan;

            if (mortgageLoan.Property != null)
            {
                if (mortgageLoan.Property.OccupancyType.Key == (int)SAHL.Common.Globals.OccupancyTypes.OwnerOccupied)
                    mortgageLoanExposureItem.OwnerOccupied = true;
                else
                    mortgageLoanExposureItem.OwnerOccupied = false;
                mortgageLoanExposureItem.PropertyKey = mortgageLoan.Property.Key;
            }

            double currentBalance = mortgageLoan.CurrentBalance;
            double latestValuation = mortgageLoan.GetActiveValuationAmount();
            double householdIncome = account.GetHouseholdIncome();
            double arrearBalance = mortgageLoan.ArrearBalance;
            double loanAgreementAmount = 0;
            for (int bondIndex = 0; bondIndex < mortgageLoan.Bonds.Count; bondIndex++)
            {
                loanAgreementAmount += mortgageLoan.Bonds[bondIndex].BondLoanAgreementAmount;
            }

            double payment = 0;

            foreach (IFinancialService fs in account.FinancialServices)
            {
                if (fs.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Open)
                    payment += fs.Payment;
            }

            IAccountVariFixLoan varifixLoanAccount = account as IAccountVariFixLoan;
            if (varifixLoanAccount != null)
            {
                IMortgageLoan fixedmortgageLoan = varifixLoanAccount.FixedSecuredMortgageLoan;
                if (fixedmortgageLoan != null)
                {
                    currentBalance += fixedmortgageLoan.CurrentBalance;
                    arrearBalance += fixedmortgageLoan.ArrearBalance;
                }
            }

            mortgageLoanExposureItem.InstalmentAmount = payment;
            mortgageLoanExposureItem.HouseholdIncome = householdIncome;
            mortgageLoanExposureItem.LatestValuationAmount = latestValuation;
            mortgageLoanExposureItem.LoanAgreementAmount = loanAgreementAmount;
            mortgageLoanExposureItem.CurrentBalance = currentBalance;
            mortgageLoanExposureItem.ArrearBalance = arrearBalance;

            if (householdIncome != 0)
                mortgageLoanExposureItem.PTI = payment / householdIncome;
            if (latestValuation != 0)
                mortgageLoanExposureItem.LTV = currentBalance / latestValuation;
            return mortgageLoanExposureItem;
        }

        // TODO - Refactor to make use of UIStatement like is done in GetUnSecuredLendingApplicationExposure
        private IEnumerable<IGroupExposureItem> GetSecuredLendingApplicationExposure(ILegalEntity legalEntity)
        {
            var securedLendingApplicationExposure = new List<GroupExposureItem>();
            foreach (IApplicationRole applicationRole in legalEntity.GetApplicationRolesByRoleTypeGroups(OfferRoleTypeGroups.Client))
            {
                // ignore closed and/or accepted applications or inactive role
                if (applicationRole.Application.ApplicationStatus.Key == (int)SAHL.Common.Globals.OfferStatuses.Closed
                    || applicationRole.Application.ApplicationStatus.Key == (int)SAHL.Common.Globals.OfferStatuses.Accepted
                    || applicationRole.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Inactive)
                    continue;

                GroupExposureItem applicationExposureItem = new GroupExposureItem();

                // re-get the application - this will prevent lazy load exceptions (ie. no session)
                IApplication application = ApplicationRepo.GetApplicationByKey(applicationRole.Application.Key);

                applicationExposureItem.RoleDescription = applicationRole.ApplicationRoleType.Description;
                applicationExposureItem.AccountKey = application.ReservedAccount != null ? application.ReservedAccount.Key : (int?)null;
                applicationExposureItem.OfferKey = application.Key;
                applicationExposureItem.Status = application.ApplicationStatus.Description;
                applicationExposureItem.Product = application.Account != null ? application.Account.Product.Description : application.ApplicationType.Description; 

                IApplicationInformation latestApplicationInformation = application.GetLatestApplicationInformation();

                if (latestApplicationInformation != null)
                {
                    IApplicationInformationVariableLoan applicationInformationVariableLoan = ApplicationRepo.GetApplicationInformationVariableLoan(latestApplicationInformation.Key);
                    IApplicationMortgageLoan applicationMortgageLoan = application as IApplicationMortgageLoan;

                    double? latestValuation = 0;
                    if (applicationMortgageLoan != null && applicationMortgageLoan.Property != null)
                    {
                        if (applicationMortgageLoan.Property.OccupancyType.Key == (int)SAHL.Common.Globals.OccupancyTypes.OwnerOccupied)
                            applicationExposureItem.OwnerOccupied = true;
                        else
                            applicationExposureItem.OwnerOccupied = false;

                        applicationExposureItem.PropertyKey = applicationMortgageLoan.Property.Key;

                        if (applicationMortgageLoan.Property.LatestCompleteValuation != null)
                            latestValuation = applicationMortgageLoan.Property.LatestCompleteValuation.ValuationAmount;

                    }
                    // Use property value from offer information if property latest valuation doesn't exist
                    latestValuation = latestValuation.HasValue && latestValuation.Value > 0 ? latestValuation 
                        : applicationInformationVariableLoan.PropertyValuation.HasValue ? applicationInformationVariableLoan.PropertyValuation : 0;

                    applicationExposureItem.LatestValuationAmount = latestValuation.Value;

                    double currentBalance = 0;
                    double arrearBalance = 0;
                    
                    double householdIncome = applicationInformationVariableLoan.HouseholdIncome.HasValue ? applicationInformationVariableLoan.HouseholdIncome.Value : 0;
                    double loanAgreementAmount = applicationInformationVariableLoan.LoanAgreementAmount.HasValue ? applicationInformationVariableLoan.LoanAgreementAmount.Value : 0;
                    double payment = applicationInformationVariableLoan.MonthlyInstalment.HasValue ? applicationInformationVariableLoan.MonthlyInstalment.Value : 0;

                    applicationExposureItem.InstalmentAmount = payment;
                    applicationExposureItem.HouseholdIncome = householdIncome;
                    
                    applicationExposureItem.LoanAgreementAmount = loanAgreementAmount;
                    applicationExposureItem.CurrentBalance = currentBalance;
                    applicationExposureItem.ArrearBalance = arrearBalance;

                    if (householdIncome != 0)
                        applicationExposureItem.PTI = payment / householdIncome;
                    if (latestValuation.Value != 0)
                        applicationExposureItem.LTV = loanAgreementAmount / latestValuation.Value;

                    securedLendingApplicationExposure.Add(applicationExposureItem);
                }
            }
            return securedLendingApplicationExposure;
        }

        private IEnumerable<IGroupExposureItem> GetUnSecuredLendingApplicationExposure(int legalEntityKey)
        {
            string sql = UIStatementRepository.GetStatement("Repositories.LegalEntityRepository", "GetPersonalLoanExposureByLegalEntityKey");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@LegalEntityKey", legalEntityKey));

            DataTable unsecuredLendingExposureDT = new DataTable();
            var dataSet = castleTransactionsService.ExecuteQueryOnCastleTran(sql, Databases.TwoAM, parameters);
            unsecuredLendingExposureDT = dataSet.Tables[0];

            var unsecuredLendingExposureItems = new List<IGroupExposureItem>();
            foreach (DataRow row in unsecuredLendingExposureDT.Rows)
            {
                unsecuredLendingExposureItems.Add(new GroupExposureItem
                {
                    AccountKey = int.Parse(row["AccountKey"].ToString()),
                    Product = row["ProductDescription"].ToString(),
                    ArrearBalance = double.Parse(row["ArrearBalance"].ToString()),
                    CurrentBalance = double.Parse(row["CurrentBalance"].ToString()),
                    HouseholdIncome = double.Parse(row["HouseholdIncome"].ToString()),
                    InstalmentAmount = double.Parse(row["InstalmentAmount"].ToString()),
                    LoanAgreementAmount = double.Parse(row["LoanAgreementAmount"].ToString()),
                    OfferKey = int.Parse(row["OfferKey"].ToString()),
                    PTI = double.Parse(row["PTI"].ToString()),
                    RoleDescription = row["RoleDescription"].ToString(),
                    Status = row["Status"].ToString()
                });
            }

            return unsecuredLendingExposureItems;
        }

        public double GetAccountGroupExposurePTI(int LegalEntityKey, double HouseholdIncome)
        {
            string sql = UIStatementRepository.GetStatement("Common", "GetAccountGroupExposurePTI");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@LegalEntityKey", LegalEntityKey));
            parameters.Add(new SqlParameter("@ConfirmedIncome", HouseholdIncome));

            var dataSet = castleTransactionsService.ExecuteQueryOnCastleTran(sql, Databases.TwoAM, parameters);
            if (dataSet.Tables[0].Rows.Count == 1)
            {
                return double.Parse(dataSet.Tables[0].Rows[0]["GroupExposurePTI"].ToString());
            }
            return -1;
        }

        public double GetAccountGroupExposureLTV(int accountKey, double valuation, double furtherLendingAmount)
        {
            string sql = UIStatementRepository.GetStatement("Common", "GetAccountGroupExposureLTV");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));
            parameters.Add(new SqlParameter("@Valuation", valuation));
            parameters.Add(new SqlParameter("@FurtherLendingAmount", furtherLendingAmount));

            var dataSet = castleTransactionsService.ExecuteQueryOnCastleTran(sql, Databases.TwoAM, parameters);
            if (dataSet.Tables[0].Rows.Count == 1)
            {
                return double.Parse(dataSet.Tables[0].Rows[0]["GroupExposureLTV"].ToString());
            }
            return -1;
        }
    }
}