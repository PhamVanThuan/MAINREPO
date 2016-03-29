using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NHibernate;
using NHibernate.Criterion;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    /// Repository for all methods to do with employment.
    /// </summary>
    [FactoryType(typeof(IEmploymentRepository))]
    public class EmploymentRepository : AbstractRepositoryBase, IEmploymentRepository
    {
        public EmploymentRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        public EmploymentRepository()
        {
            this.castleTransactionService = new CastleTransactionsService();
        }

        private ICastleTransactionsService castleTransactionService;

        /// <summary>
        /// Gets a list of accounts whose PTI is affected by the employment record - if the result set contains
        /// any accounts then the employment record should not be deactivated or deleted.
        /// </summary>
        /// <param name="employment">The employment record that is being deactivated.</param>
        /// <returns>A list of accounts that deactivating the employment record will affect.</returns>
        /// <remarks>This is a basic copy of the UIStatement IsAccountPTIValid - this can be done more efficiently.</remarks>
        public IList<IAccount> GetAccountsForPTI(IEmployment employment)
        {
            List<IAccount> affectedAccounts = new List<IAccount>();
            IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            // get a list of all account keys to which the employment record is attached
            string hql = @"select a from Account_DAO a
                inner join a.Roles as r
                inner join a.FinancialServices as fs
                where r.GeneralStatus.Key = ?
                and a.AccountStatus.Key in (?, ?)
                and r.LegalEntity.Key = ?
                and r.RoleType.Key in (:roleTypes)
                and fs.FinancialServiceType.Key in (:financialServiceTypes)";
            SimpleQuery<Account_DAO> q = new SimpleQuery<Account_DAO>(hql, (int)GeneralStatuses.Active, (int)AccountStatuses.Open, (int)AccountStatuses.Application, employment.LegalEntity.Key);
            q.SetParameterList("roleTypes", new int[] { (int)RoleTypes.MainApplicant, (int)RoleTypes.Suretor });
            q.SetParameterList("financialServiceTypes", new int[] { (int)FinancialServiceTypes.VariableLoan, (int)FinancialServiceTypes.FixedLoan });
            Account_DAO[] allAccounts = q.Execute();

            // if there are no open accounts, exit here
            if (allAccounts.Length == 0)
                return affectedAccounts;

            // loop through the accounts, and get all employment records attached to the account's legal entities, excluding
            // the employment record we're checking and employment records set to previous
            foreach (Account_DAO account in allAccounts)
            {
                hql = @"select e from Employment_DAO e
                    inner join e.LegalEntity.Roles r
                    where r.Account.Key = ?
                    and e.Key <> ?
                    and e.EmploymentStatus.Key = ?";
                SimpleQuery<Employment_DAO> qEmp = new SimpleQuery<Employment_DAO>(hql, account.Key,
                    employment.Key, (int)EmploymentStatuses.Current);
                Employment_DAO[] empRecords = qEmp.Execute();

                IAccount iAcc = bmtm.GetMappedType<IAccount>(account);

                // if there are no other employment records, add the account
                if (empRecords.Length == 0)
                {
                    affectedAccounts.Add(iAcc);
                    continue;
                }

                // check all the employment records found, and if we find one with a confirmed income > 0
                // then we're ok
                bool foundConfirmedIncome = false;
                foreach (Employment_DAO emp in empRecords)
                {
                    IEmployment e = bmtm.GetMappedType<IEmployment>(emp);
                    if (e.ConfirmedIncome > 0D)
                    {
                        foundConfirmedIncome = true;
                        break;
                    }
                }
                if (!foundConfirmedIncome)
                    affectedAccounts.Add(iAcc);
            }

            return affectedAccounts;
        }

        /// <summary>
        /// Implements <see cref="IEmploymentRepository.GetEmployerByKey"/>.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IEmployer GetEmployerByKey(int Key)
        {
            return base.GetByKey<IEmployer, Employer_DAO>(Key);
        }

        /// <summary>
        /// Implements <see cref="IEmploymentRepository.GetEmployersByPrefix"/>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IEmployer> GetEmployersByPrefix(string prefix, int maxRowCount)
        {
            IList<Employer_DAO> employers = Employer_DAO.FindByPrefix(prefix, maxRowCount);
            DAOEventList<Employer_DAO, IEmployer, Employer> daoList = new DAOEventList<Employer_DAO, IEmployer, Employer>(employers);
            return new ReadOnlyEventList<IEmployer>(daoList);
        }

        /// <summary>
        /// Implements <see cref="IEmploymentRepository.GetEmploymentByKey"/>.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IEmployment GetEmploymentByKey(int Key)
        {
            string sql = "select emp.* from [2am]..Employment emp (nolock) where emp.EmploymentKey = ?";
            SimpleQuery<Employment_DAO> q = new SimpleQuery<Employment_DAO>(QueryLanguage.Sql, sql, Key);
            q.AddSqlReturnDefinition(typeof(Employment_DAO), "emp");
            Employment_DAO[] res = q.Execute();

            if (res != null && res.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return bmtm.GetMappedType<IEmployment, Employment_DAO>(res[0]);
            }
            else
                return null;
        }

        /// <summary>
        /// Implements <see cref="IEmploymentRepository.GetEmploymentByApplicationKey(int,bool)"/>.
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="incomeContributorsOnly"></param>
        /// <returns></returns>
        public IEventList<IEmployment> GetEmploymentByApplicationKey(int applicationKey, bool incomeContributorsOnly)
        {
            int offerRoleTypeGroup = (int)OfferRoleTypeGroups.Client;

            string hql = @"from Employment_DAO emp WHERE emp.Key IN (select distinct e.Key from Employment_DAO e
                inner join e.LegalEntity le
                inner join le.ApplicationRoles r
                inner join r.ApplicationRoleType apr";

            if (incomeContributorsOnly)
                hql += " inner join r.ApplicationRoleAttributes ora";

            hql += " WHERE r.ApplicationKey = ?  and apr.ApplicationRoleTypeGroup.Key = ?";
            if (incomeContributorsOnly)
                hql += " and ora.OfferRoleAttributeType.Key = " + (int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor;

            hql += ")";
            SimpleQuery<Employment_DAO> q = new SimpleQuery<Employment_DAO>(hql, applicationKey, offerRoleTypeGroup);
            Employment_DAO[] res = q.Execute();

            IList<IEmployment> retval = new List<IEmployment>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new Employment(res[i]));
            }
            return new EventList<IEmployment>(retval);
        }

        /// <summary>
        /// Implements <see cref="IEmploymentRepository.GetEmptyEmployer()"></see>.
        /// </summary>
        public IEmployer GetEmptyEmployer()
        {
            return base.CreateEmpty<IEmployer, Employer_DAO>();
        }

        /// <summary>
        /// Implements <see cref="IEmploymentRepository.GetEmptyEmploymentByType(EmploymentTypes)"></see>.
        /// </summary>
        /// <param name="employmentType"></param>
        public IEmployment GetEmptyEmploymentByType(EmploymentTypes employmentType)
        {
            switch (employmentType)
            {
                case EmploymentTypes.Salaried:
                    return new EmploymentSalaried(new EmploymentSalaried_DAO());
                case EmploymentTypes.SelfEmployed:
                    return new EmploymentSelfEmployed(new EmploymentSelfEmployed_DAO());
                case EmploymentTypes.SalariedwithDeduction:
                    return new EmploymentSubsidised(new EmploymentSubsidised_DAO());
                case EmploymentTypes.Unemployed:
                    return new EmploymentUnemployed(new EmploymentUnemployed_DAO());
                default:
                    throw new NotSupportedException("Unsupported employment type.");
            }
        }

        /// <summary>
        /// Implements <see cref="IEmploymentRepository.GetEmptyEmploymentByType(IEmploymentType)"></see>.
        /// </summary>
        /// <param name="employmentType"></param>
        public IEmployment GetEmptyEmploymentByType(IEmploymentType employmentType)
        {
            return GetEmptyEmploymentByType((EmploymentTypes)employmentType.Key);
        }

        /// <summary>
        /// Implements <see cref="IEmploymentRepository.GetEmptySubsidy()"></see>.
        /// </summary>
        public ISubsidy GetEmptySubsidy()
        {
            return base.CreateEmpty<ISubsidy, Subsidy_DAO>();
        }

        /// <summary>
        /// Implements <see cref="IEmploymentRepository.GetSubsidyProviderByKey"/>.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public ISubsidyProvider GetSubsidyProviderByKey(int Key)
        {
            return base.GetByKey<ISubsidyProvider, SubsidyProvider_DAO>(Key);
        }

        /// <summary>
        /// Implements <see cref="IEmploymentRepository.GetSubsidyByKey"/>.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public ISubsidy GetSubsidyByKey(int Key)
        {
            return base.GetByKey<ISubsidy, Subsidy_DAO>(Key);
        }

        /// <summary>
        /// Implements <see cref="IEmploymentRepository.GetSubsidyProvidersByPrefix"/>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        public ReadOnlyCollection<ISubsidyProvider> GetSubsidyProvidersByPrefix(string prefix, int maxRowCount)
        {
            string sql = @"select sp.*
                from SubsidyProvider sp (nolock)
                inner join LegalEntity le (nolock) on sp.LegalEntityKey = le.LegalEntityKey
                where le.RegisteredName like ?
                and le.LegalEntityTypeKey in (:leTypes)";

            if (!prefix.EndsWith("%"))
                prefix = String.Concat(prefix, "%");

            SimpleQuery<SubsidyProvider_DAO> q = new SimpleQuery<SubsidyProvider_DAO>(QueryLanguage.Sql, sql, prefix);
            q.AddSqlReturnDefinition(typeof(SubsidyProvider_DAO), "sp");
            q.SetQueryRange(maxRowCount);
            q.SetParameterList("leTypes", new int[] { (int)LegalEntityTypes.CloseCorporation, (int)LegalEntityTypes.Company, (int)LegalEntityTypes.Trust });

            IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            List<ISubsidyProvider> result = new List<ISubsidyProvider>();

            foreach (SubsidyProvider_DAO sp in q.Execute())
                result.Add(bmtm.GetMappedType<ISubsidyProvider>(sp));

            return result.AsReadOnly();
        }

        public ISubsidyProvider CreateEmptySubsidyProvider()
        {
            return base.CreateEmpty<ISubsidyProvider, SubsidyProvider_DAO>();
        }

        public void SaveSubsidyProvider(ISubsidyProvider sp)
        {
            base.Save<ISubsidyProvider, SubsidyProvider_DAO>(sp);
        }

        /// <summary>
        /// Implements <see cref="IEmploymentRepository.SaveEmployer"></see>.
        /// </summary>
        /// <param name="employer"></param>
        /// <param name="principal"></param>
        public void SaveEmployer(IEmployer employer, SAHLPrincipal principal)
        {
            employer.UserID = principal.Identity.Name;
            employer.ChangeDate = DateTime.Now;

            base.Save<IEmployer, Employer_DAO>(employer);
        }

        /// <summary>
        /// Get list of subsidies by legal entity key
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        public IList<ISubsidy> GetSubsidiesByLegalEntityKey(int legalEntityKey)
        {
            string HQL = "from Subsidy_DAO s where s.LegalEntity.Key = ?";
            SimpleQuery<Subsidy_DAO> q = new SimpleQuery<Subsidy_DAO>(HQL, legalEntityKey);
            Subsidy_DAO[] res = q.Execute();

            IList<ISubsidy> retval = new List<ISubsidy>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new Subsidy(res[i]));
            }
            return retval;
        }

        /// <summary>
        /// Saves employment information to the database.
        /// </summary>
        /// <param name="employment"></param>
        public void SaveEmployment(IEmployment employment)
        {
            // we're relying on a legal entity existing at this point because it MUST be there - if it's not
            // just call validate so that the mandatory rules run, and exit
            if (employment.LegalEntity == null)
            {
                employment.ValidateEntity();
                return;
            }

            // How we save the employment record differs depending on whether it is new.  For new records, we need to
            // actually add the object to the legal entity and save THAT object, otherwise when we call
            // CalculateHouseholdIncome on the legal entity's applications, we cannot guarantee the employment object is
            // available in the application.ApplicationRoles.LegalEntity.Employment list.  For existing employment records,
            // just do the update on the employment object itself
            if (employment.Key == 0)
            {
                ILegalEntity le = employment.LegalEntity;
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                le.Employment.Add(spc.DomainMessages, employment);
                LegalEntityRepo.SaveLegalEntity(le, false);

                // TODO: We will either need to return out here, or use straight DAO calls depending on whether
                // legal entity also calls CalculateHouseHoldIncome() - at this stage this is still up for debate
                // by the BAs so for now just leaving this to run through (MS: 07/05/2008)
            }
            else
            {
                base.Save<IEmployment, Employment_DAO>(employment);

                //Employment_DAO daoEmp = (Employment_DAO)(employment as IDAOObject).GetDAOObject();
                //daoEmp.SaveAndFlush();
            }

            // run through any applications related to the legal entity and update household income
            IReadOnlyEventList<IApplicationRole> appRoles = employment.LegalEntity.GetApplicationRolesByRoleTypeGroups(OfferRoleTypeGroups.Client);
            foreach (IApplicationRole ar in appRoles)
            {
                // skip out if the role is inactive
                if (ar.GeneralStatus.Key == (int)GeneralStatuses.Inactive)
                    continue;

                //If the offer is open (statuskey == 1 and
                //application information has not been accepted then update income
                IApplicationInformation ai = ar.Application.GetLatestApplicationInformation();
                if (ar.Application.IsOpen
                    && ai != null
                    && ai.ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                {
                    ar.Application.CalculateHouseHoldIncome();
                    ar.Application.SetEmploymentType();

                    // recalculate application detail
                    IApplicationMortgageLoan appML = ar.Application as IApplicationMortgageLoan;
                    if (appML != null)
                    {
                        ApplicationRepo.DetermineGEPFAttribute(appML);
                        appML.CalculateApplicationDetail(false, false);
                    }
                    ApplicationRepo.SaveApplication(ar.Application);
                    appML.PricingForRisk();
                    ApplicationRepo.SaveApplication(appML);
                }
            }

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// Implements <see cref="IEmploymentRepository.GetEmployers"/>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IEmployer> GetEmployers(string prefix)
        {
            ICriterion[] criteria = new ICriterion[]
                {
                    Expression.Eq("Name", prefix).IgnoreCase()
                };

            IList<Employer_DAO> employers = Employer_DAO.FindAll(criteria);

            DAOEventList<Employer_DAO, IEmployer, Employer> daoList = new DAOEventList<Employer_DAO, IEmployer, Employer>(employers);
            return new ReadOnlyEventList<IEmployer>(daoList);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="employment"></param>
        /// <returns></returns>
        public DataTable GetVerificationProcessDT(IEmployment employment)
        {
            string query = UIStatementRepository.GetStatement("Repositories.EmploymentRepository", "GetVerificationProcessDT");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@EmploymentKey", employment.Key));

            DataTable dtLst = new DataTable();
            DataSet dsLst = new DataSet();
            dsLst = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (dsLst != null)
            {
                if (dsLst.Tables.Count > 0)
                {
                    dtLst = dsLst.Tables[0];
                }
            }
            return dtLst;
        }

        public IEmploymentVerificationProcess GetEmptyEmploymentVerificationProcess()
        {
            return base.CreateEmpty<IEmploymentVerificationProcess, EmploymentVerificationProcess_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="subsidy"></param>
        public void SaveSubsidy(ISubsidy subsidy)
        {
            base.Save<ISubsidy, Subsidy_DAO>(subsidy);
        }

        #region Repositories

        private IApplicationRepository _appRepo;
        private ILegalEntityRepository _leRep;

        private IApplicationRepository ApplicationRepo
        {
            get
            {
                if (_appRepo == null)
                    _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _appRepo;
            }
        }

        private ILegalEntityRepository LegalEntityRepo
        {
            get
            {
                if (_leRep == null)
                    _leRep = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                return _leRep;
            }
        }

        #endregion Repositories


        public IDictionary<ILegalEntity, string> GetSalaryPaymentDaysByGenericKey(int genericKey, int genericKeyTypeKey, bool includeDaySuffix = true)
        {
            IDictionary<ILegalEntity, string> dicSalaryPaymentDates = new Dictionary<ILegalEntity, string>();

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            IReadOnlyEventList<ILegalEntity> legalEntities = new ReadOnlyEventList<ILegalEntity>();

            switch (genericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.FinancialService:
                    IFinancialServiceRepository fsRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();

                    // get the financialservice by key
                    IFinancialService financialService = fsRepo.GetFinancialServiceByKey(genericKey);
                    if (financialService != null && financialService.Account != null)
                    {
                        // get the legalentities playing a role on the account
                        int[] roletypes = { (int)RoleTypes.MainApplicant, (int)RoleTypes.Suretor };
                        legalEntities = financialService.Account.GetLegalEntitiesByRoleType(spc.DomainMessages, roletypes, GeneralStatusKey.Active);
                    }
                    break;

                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                case (int)SAHL.Common.Globals.GenericKeyTypes.ParentAccount:
                    IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                    // get the account by key
                    IAccount account = accountRepo.GetAccountByKey(genericKey);
                    if (account != null)
                    {
                        // get the legalentities playing a role on the account
                        int[] roletypes = { (int)RoleTypes.MainApplicant, (int)RoleTypes.Suretor };
                        legalEntities = account.GetLegalEntitiesByRoleType(spc.DomainMessages, roletypes, GeneralStatusKey.Active);                      
                    }
                    break;

                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                    IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                    // get the account by key
                    IApplication application = applicationRepo.GetApplicationByKey(genericKey);
                    if (application != null)
                    {
                        // get the legalentities playing a role on the application
                        OfferRoleTypes[] offerRoleTypes = { OfferRoleTypes.MainApplicant, OfferRoleTypes.Suretor };
                        legalEntities = application.GetLegalEntitiesByRoleType(offerRoleTypes, GeneralStatusKey.Active);     
                    }
                    break;

                default:
                    break;
            }

            // get the employment records for each legalentity
            foreach (var legalEntity in legalEntities)
            {
                string salaryPaymentDays = string.Empty;
                int i = 0;
                IList<int> uniqueListOfDays = new List<int>();
                foreach (var employment in legalEntity.Employment)
                {
                    if (employment.IsConfirmed && employment.EmploymentStatus.Key == (int)SAHL.Common.Globals.EmploymentStatuses.Current)
                    {
                        if (employment.ExtendedEmployment != null && employment.ExtendedEmployment.SalaryPayDay.HasValue)
                        {
                            string day = employment.ExtendedEmployment.SalaryPayDay.Value.ToString();
                            if (includeDaySuffix == true)
                                day = SAHL.Common.Utils.DateUtils.AddDaySuffix(employment.ExtendedEmployment.SalaryPayDay.Value);

                            if (!salaryPaymentDays.Contains(day))
                            {
                                if (i == 0)
                                    salaryPaymentDays += day;
                                else
                                    salaryPaymentDays += ", " + day;

                                i++;
                            }
                        }
                    }
                }

                if (!String.IsNullOrEmpty(salaryPaymentDays))
                {
                    dicSalaryPaymentDates.Add(legalEntity, salaryPaymentDays);
                }
            }

            return dicSalaryPaymentDates;
        }


        public EmploymentTypes DetermineHighestIncomeContributersEmploymentType(ILegalEntityNaturalPerson highestIncomeContributer)
        {
            var employmentType = EmploymentTypes.Unknown;

            // get the remuneration type from the higest income contributers current employment record with the highest income
            IDictionary<int, double> dicEmploymentIncome = new Dictionary<int, double>();
            double confirmedIncome = 0, basicIncome = 0;
            foreach (IEmployment employment in highestIncomeContributer.Employment)
            {
                if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current && (employment.EmploymentEndDate.HasValue ? employment.EmploymentEndDate.Value : DateTime.Now.AddDays(1)) > DateTime.Now)
                {
                    confirmedIncome = employment.ConfirmedBasicIncome.HasValue ? employment.ConfirmedBasicIncome.Value : 0;
                    basicIncome = employment.BasicIncome.HasValue ? employment.BasicIncome.Value : 0;

                    if (employment.IsConfirmed)
                    {
                        if (dicEmploymentIncome.ContainsKey(employment.EmploymentType.Key))
                            dicEmploymentIncome[employment.EmploymentType.Key] += confirmedIncome;
                        else
                            dicEmploymentIncome.Add(employment.EmploymentType.Key, confirmedIncome);
                    }
                    else
                    {
                        if (dicEmploymentIncome.ContainsKey(employment.EmploymentType.Key))
                            dicEmploymentIncome[employment.EmploymentType.Key] += basicIncome;
                        else
                            dicEmploymentIncome.Add(employment.EmploymentType.Key, basicIncome);
                    }
                }
            }

            double highestIncome = 0;
            int employmentTypeKey = 0;

            foreach (KeyValuePair<int, double> incomes in dicEmploymentIncome)
            {
                if (incomes.Value > highestIncome)
                {
                    highestIncome = incomes.Value;
                    employmentTypeKey = incomes.Key;
                }
            }

            if (employmentTypeKey != 0)
            {
                employmentType = (EmploymentTypes)employmentTypeKey;
            }

            return employmentType;
        }
    }
}