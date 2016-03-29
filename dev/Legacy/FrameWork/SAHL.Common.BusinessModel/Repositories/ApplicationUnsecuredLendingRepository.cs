using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    /// The concrete class for the Application repository.
    /// Implements <see cref="IApplicationUnsecuredLendingRepository"></see>.
    ///
    /// </summary>
    [FactoryType(typeof(IApplicationUnsecuredLendingRepository))]
    public class ApplicationUnsecuredLendingRepository : AbstractRepositoryBase, IApplicationUnsecuredLendingRepository
    {
        public ApplicationUnsecuredLendingRepository(IApplicationRepository applicationRepository,
                                                     ILookupRepository lookupRepository,
                                                     ICreditCriteriaUnsecuredLendingRepository creditCriteriaUnsecuredLendingRepository,
                                                     IMarketRateRepository marketRateRepository,
            ICastleTransactionsService castleTransactionsService)
        {
            this.applicationRepository = applicationRepository;
            this.lookupRepository = lookupRepository;
            this.creditCriteriaUnsecuredLendingRepository = creditCriteriaUnsecuredLendingRepository;
            this.marketRateRepository = marketRateRepository;
            this.castleTransactionService = castleTransactionsService;
        }

        public ApplicationUnsecuredLendingRepository()
            : this(RepositoryFactory.GetRepository<IApplicationRepository>(),
                   RepositoryFactory.GetRepository<ILookupRepository>(),
                   RepositoryFactory.GetRepository<ICreditCriteriaUnsecuredLendingRepository>(),
                   RepositoryFactory.GetRepository<IMarketRateRepository>(),
                   RepositoryFactory.GetRepository<ILegalEntityRepository>(),
                   new CastleTransactionsService(),
                   ServiceFactory.GetService<IRuleService>())
        {
        }

        public ApplicationUnsecuredLendingRepository(IApplicationRepository applicationRepository,
                                                     ILookupRepository lookupRepository,
                                                     ICreditCriteriaUnsecuredLendingRepository creditCriteriaUnsecuredLendingRepository,
                                                     IMarketRateRepository marketRateRepository,
                                                     ILegalEntityRepository legalEntityRepository,
                                                     ICastleTransactionsService castleTransactionsService,
                                                     IRuleService ruleService)
        {
            this.applicationRepository = applicationRepository;
            this.lookupRepository = lookupRepository;
            this.creditCriteriaUnsecuredLendingRepository = creditCriteriaUnsecuredLendingRepository;
            this.marketRateRepository = marketRateRepository;
            this.castleTransactionService = castleTransactionsService;
            this.legalEntityRepository = legalEntityRepository;
            this.ruleService = ruleService;
        }

        private IApplicationRepository applicationRepository;
        private ILookupRepository lookupRepository;
        private ICreditCriteriaUnsecuredLendingRepository creditCriteriaUnsecuredLendingRepository;
        private IMarketRateRepository marketRateRepository;
        private ILegalEntityRepository legalEntityRepository;
        private ICastleTransactionsService castleTransactionService;
        private IRuleService ruleService;

        public IResult CalculateUnsecuredLending(double amount, List<int> terms, bool creditLifePolicySelected)
        {
            // Move to constructor for IOC container to handle
            IControlRepository controlRepo = RepositoryFactory.GetRepository<IControlRepository>();
            ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
            IMarketRateRepository marketRateRepo = RepositoryFactory.GetRepository<IMarketRateRepository>();
            ICreditCriteriaUnsecuredLendingRepository unsecureLendingRepo = RepositoryFactory.GetRepository<ICreditCriteriaUnsecuredLendingRepository>();

            // Get static/lookup values
            IControl monthlyFee = controlRepo.GetControlByDescription("PersonalLoanMonthlyFee");
            IControl initiationFee = controlRepo.GetControlByDescription("PersonalLoanInitiationFee");
            IMarketRate marketRate = marketRateRepo.GetMarketRateByKey((int)MarketRates.PrimeLendingRate); ;

            // Setup the DTO
            IResult result = new Result();
            result.Amount = amount;
            result.CreditLifePolicy = creditLifePolicySelected;
            result.InitiationFee = initiationFee.ControlNumeric.Value;
            result.MonthlyFee = monthlyFee.ControlNumeric.Value;
            var loanAmountWithFees = amount + initiationFee.ControlNumeric.Value;

            if (creditLifePolicySelected)
            {
                result.CreditLifePremium = lifeRepo.CalculateLifePremiumForUnsecuredLending(loanAmountWithFees);
            }
            else
            {
                result.CreditLifePremium = 0D;
            }

            IReadOnlyEventList<ICreditCriteriaUnsecuredLending> creditCriteriaItems = unsecureLendingRepo.GetCreditCriteriaUnsecuredLendingByLoanAmount(result.Amount);

            int uniqueIDCounter = 0;
            foreach (ICreditCriteriaUnsecuredLending creditCriteriaUnsecuredLending in creditCriteriaItems)
            {
                double loanInstalment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment((result.Amount + result.InitiationFee), (marketRate.Value + creditCriteriaUnsecuredLending.Margin.Value), creditCriteriaUnsecuredLending.Term, false);
                double totalInstalment = loanInstalment + result.CreditLifePremium + result.MonthlyFee;
                result.CalculatedItems.Add(new CalculatedItem(uniqueIDCounter, amount, creditCriteriaUnsecuredLending.Term, (marketRate.Value + creditCriteriaUnsecuredLending.Margin.Value), totalInstalment, creditCriteriaUnsecuredLending.Key, loanInstalment));
                if (terms.Contains(creditCriteriaUnsecuredLending.Term))
                {
                    terms.Remove(creditCriteriaUnsecuredLending.Term);
                }
                uniqueIDCounter++;
            }

            // If there is a term entered that does not match the list of return terms (GetCreditCriteriaUnsecuredLendingByLoanAmount)
            // then use the first term greater than the term entered by user (ascending order of terms)

            foreach (var term in terms)
            {
                ICreditCriteriaUnsecuredLending creditCriteriaUnsecuredLending = unsecureLendingRepo.GetCreditCriteriaUnsecuredLendingByLoanAmountAndTerm(result.Amount, term);
                double loanInstalment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment((result.Amount + result.InitiationFee), (marketRate.Value + creditCriteriaUnsecuredLending.Margin.Value), term, false);
                double totalInstalment = loanInstalment + result.CreditLifePremium + result.MonthlyFee;
                result.CalculatedItems.Add(new CalculatedItem(uniqueIDCounter, amount, term, (marketRate.Value + creditCriteriaUnsecuredLending.Margin.Value), totalInstalment, creditCriteriaUnsecuredLending.Key, loanInstalment));
                uniqueIDCounter++;
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IApplicationUnsecuredLending GetEmptyApplicationUnsecuredLending()
        {
            return base.CreateEmpty<IApplicationUnsecuredLending, ApplicationUnsecuredLending_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IApplicationInformationPersonalLoan GetEmptyApplicationInformationPersonalLoan()
        {
            return base.CreateEmpty<IApplicationInformationPersonalLoan, ApplicationInformationPersonalLoan_DAO>();
        }

        /// <summary>
        /// Save an IApplication object and its properties to the database (SQLUpdate)
        /// </summary>
        /// <param name="applicationInformationPersonalLoan"></param>
        public void CreateApplicationInformationPersonalLoan(IApplicationInformationPersonalLoan applicationInformationPersonalLoan)
        {
            base.Save<IApplicationInformationPersonalLoan, ApplicationInformationPersonalLoan_DAO>(applicationInformationPersonalLoan);
        }

        /// <summary>
        /// Gets summary from ApplicationInformationPersonalLoan_DAO by offer key.
        /// </summary>
        /// <param name="genericKey">The Offer Key.</param>
        /// <returns>Application Information Personal Loan List.</returns>
        public IReadOnlyEventList<IApplicationInformationPersonalLoan> GetApplicationInformationPersonalLoanSummaryByKey(int genericKey)
        {
            // Create HQL query to get information from ApplicationInformationPersonalLoan_DAO
            // to be displayed on application summary screen
            string hql = "select aipl from ApplicationInformationPersonalLoan_DAO aipl where aipl.ApplicationInformation.Application.Key = ?";

            SimpleQuery<ApplicationInformationPersonalLoan_DAO> query
                = new SimpleQuery<ApplicationInformationPersonalLoan_DAO>(hql, genericKey);

            ApplicationInformationPersonalLoan_DAO[] applicationInformationPersonalLoan
                = ApplicationInformationPersonalLoan_DAO.ExecuteQuery(query) as ApplicationInformationPersonalLoan_DAO[];

            if (applicationInformationPersonalLoan != null)
            {
                IList<ApplicationInformationPersonalLoan_DAO> list
                    = new List<ApplicationInformationPersonalLoan_DAO>(applicationInformationPersonalLoan);

                DAOEventList<ApplicationInformationPersonalLoan_DAO, IApplicationInformationPersonalLoan, ApplicationInformationPersonalLoan> daoList
                    = new DAOEventList<ApplicationInformationPersonalLoan_DAO, IApplicationInformationPersonalLoan, ApplicationInformationPersonalLoan>(list);

                return new ReadOnlyEventList<IApplicationInformationPersonalLoan>(daoList);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Implements <see cref="IApplicationUnsecuredLendingRepository.GetApplicationByKey"></see>.
        /// </summary>
        public IApplicationUnsecuredLending GetApplicationByKey(int applicationKey)
        {
            ApplicationUnsecuredLending_DAO application = Application_DAO.TryFind(applicationKey) as ApplicationUnsecuredLending_DAO;

            if (application != null)
            {
                IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IApplicationUnsecuredLending, ApplicationUnsecuredLending_DAO>(application);
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="application"></param>
        /// <param name="creditLifePolicy"></param>
        /// <param name="result"></param>
        /// <param name="selectedPersonalLoanOption"></param>
        /// <returns></returns>
        public IApplicationUnsecuredLending SetupPersonalLoanApplication(IDomainMessageCollection messages, IApplication application, bool creditLifePolicy, IResult result, ICalculatedItem selectedPersonalLoanOption)
        {
            IApplicationUnsecuredLending applicationUnsecuredLending = application as IApplicationUnsecuredLending;

            // Populate Application Expenses
            /*Add PersonalLoanInitiationFee*/
            var applicationExpenses = application.ApplicationExpenses.Where(x => x.ExpenseType.Key == (int)ExpenseTypes.PersonalLoanInitiationFee).FirstOrDefault();
            if (applicationExpenses == null)
            {
                IApplicationExpense applicationExpense = applicationRepository.GetEmptyApplicationExpense();
                applicationExpense.Application = applicationUnsecuredLending;
                applicationExpense.ExpenseType = lookupRepository.ExpenseTypes.ObjectDictionary[((int)ExpenseTypes.PersonalLoanInitiationFee).ToString()];
                applicationExpense.MonthlyPayment = 0D;
                applicationExpense.ToBeSettled = true;
                applicationExpense.TotalOutstandingAmount = result.InitiationFee;
                applicationUnsecuredLending.ApplicationExpenses.Add(messages, applicationExpense);
            }

            // Populate Application Attributes
            /*Add Life Selected Attribute*/
            IApplicationAttribute applicationAttribute = application.ApplicationAttributes.Where(x => x.ApplicationAttributeType.Key == (int)OfferAttributeTypes.Life).FirstOrDefault();
            if (creditLifePolicy && applicationAttribute == null)
            {
                applicationAttribute = applicationRepository.GetEmptyApplicationAttribute();
                applicationAttribute.ApplicationAttributeType = lookupRepository.ApplicationAttributesTypes.ObjectDictionary[((int)OfferAttributeTypes.Life).ToString()];
                applicationAttribute.Application = applicationUnsecuredLending;
                applicationUnsecuredLending.ApplicationAttributes.Add(messages, applicationAttribute);
            }
            /*Remove Life Selected Attribute*/
            if (!creditLifePolicy && applicationAttribute != null)
            {
                applicationUnsecuredLending.ApplicationAttributes.Remove(messages, applicationAttribute);
            }

            double totalFees = applicationUnsecuredLending.ApplicationExpenses.Sum(x => x.TotalOutstandingAmount);

            var latestApplicationInformation = applicationUnsecuredLending.GetLatestApplicationInformation();
            IApplicationProductPersonalLoan applicationProductPersonalLoan = null;
            if (latestApplicationInformation == null)
            {
                // Set up the Personal Loan Product
                applicationUnsecuredLending.SetProduct(ProductsUnsecuredLending.PersonalLoan);
            }

            applicationProductPersonalLoan = applicationUnsecuredLending.CurrentProduct as IApplicationProductPersonalLoan;

            applicationProductPersonalLoan.ApplicationInformationPersonalLoan.LifePremium = result.CreditLifePremium;
            applicationProductPersonalLoan.ApplicationInformationPersonalLoan.FeesTotal = totalFees;

            applicationProductPersonalLoan.ApplicationInformationPersonalLoan.LoanAmount = selectedPersonalLoanOption.Amount;
            applicationProductPersonalLoan.ApplicationInformationPersonalLoan.Term = selectedPersonalLoanOption.Term;
            applicationProductPersonalLoan.ApplicationInformationPersonalLoan.MonthlyInstalment = selectedPersonalLoanOption.LoanInstalment;

            applicationProductPersonalLoan.ApplicationInformationPersonalLoan.CreditCriteriaUnsecuredLending = creditCriteriaUnsecuredLendingRepository.GetCreditCriteriaUnsecuredLendingByKey(selectedPersonalLoanOption.CreditCriteriaUnsecuredLendingKey);
            applicationProductPersonalLoan.ApplicationInformationPersonalLoan.MarketRate = marketRateRepository.GetMarketRateByKey((int)MarketRates.PrimeLendingRate);
            applicationProductPersonalLoan.ApplicationInformationPersonalLoan.Margin = applicationProductPersonalLoan.ApplicationInformationPersonalLoan.CreditCriteriaUnsecuredLending.Margin;

            return applicationUnsecuredLending;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userID"></param>
        public void CreateAndOpenPersonalLoan(int accountKey, string userID)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@ReservedAccountKey", accountKey));
            prms.Add(new SqlParameter("@UserID", userID));
            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.ApplicationUnsecuredLendingRepository", "CreateAndOpenPersonalLoan", prms);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userID"></param>
        public void DisbursePersonalLoan(int accountKey, string userID)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@ReservedAccountKey", accountKey));
            prms.Add(new SqlParameter("@UserID", userID));
            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.ApplicationUnsecuredLendingRepository", "DisbursePersonalLoan", prms);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        public void ReturnDisbursedPersonalLoanToApplication(int accountKey)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountKey));
            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.ApplicationUnsecuredLendingRepository", "ReturnDisbursedPersonalLoanToApplication", prms);
        }

        public bool CreatePersonalLoanLead(string legalEntityIDNumber)
        {
            var successfullyCreated = false;
            var legalEntity = this.legalEntityRepository.GetNaturalPersonByIDNumber(legalEntityIDNumber);

            if (legalEntity != null && LegalEntityQualifies(legalEntity))
            {
                var personalLoanLead = this.applicationRepository.CreateUnsecuredLendingLead();
                personalLoanLead.ApplicationSource = lookupRepository.ApplicationSources.ObjectDictionary[((int)OfferSources.CampaignLead).ToString()];
                this.applicationRepository.SaveApplication(personalLoanLead);
                // Add the client role - this must be added after the application is saved and has been assigned a key
                this.legalEntityRepository.InsertExternalRole(ExternalRoleTypes.Client, personalLoanLead.Key, GenericKeyTypes.Offer, legalEntity.Key, false);
                this.applicationRepository.CreatePersonalLoanWorkflowCase(personalLoanLead.Key);
                successfullyCreated = true;
            }

            return successfullyCreated;
        }

        private bool LegalEntityQualifies(ILegalEntity legalEntity)
        {
            bool legalEntityQualifies = false;
            var principalCache = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            var messages = principalCache.DomainMessages;
            var initialMessageCount = messages.Count;

            ruleService.ExecuteRule(messages, "CheckUniquePersonalLoanApplication", new object[] { legalEntity.Key });
            ruleService.ExecuteRule(messages, "LegalEntityUnderDebtCounselling", new object[] { legalEntity });
            ruleService.ExecuteRule(messages, "CheckIfCapitecClient", new object[] { legalEntity });

            legalEntityQualifies = (messages.Count == initialMessageCount);

            return legalEntityQualifies;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IExternalLifePolicy GetEmptyExternalLifePolicy()
        {
            return base.CreateEmpty<IExternalLifePolicy, ExternalLifePolicy_DAO>();
        }

        public bool ApplicationHasSAHLLifeApplied(int applicationKey)
        {
            bool hasSAHLLifeApplied = false;
            var applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
            var application = applicationRepository.GetApplicationByKey(applicationKey);

            IApplicationUnsecuredLending applicationUnsecuredLending = (IApplicationUnsecuredLending)application;
            if (applicationUnsecuredLending != null)
            {
                if (applicationUnsecuredLending.GetLatestApplicationInformation() == null)
                    throw new System.Exception("Life Policy cannot be captured for a lead.");

                var applicationProductPersonalLoan = applicationUnsecuredLending.CurrentProduct as IApplicationProductPersonalLoan;
                var applicationInformationPersonalLoan = applicationProductPersonalLoan.ApplicationInformationPersonalLoan;
                hasSAHLLifeApplied = applicationInformationPersonalLoan.LifePremium > 0;
            }

            return hasSAHLLifeApplied;
        }
    }
}