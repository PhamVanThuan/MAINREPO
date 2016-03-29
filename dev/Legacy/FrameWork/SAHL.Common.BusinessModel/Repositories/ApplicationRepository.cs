using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;
using NHibernate;
using NHibernate.Criterion;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Utils;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Services.Capitec.Models.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    /// The concrete class for the Application repository.
    /// Implements <see cref="IApplicationRepository"></see>.
    /// </summary>
    [FactoryType(typeof(IApplicationRepository))]
    public class ApplicationRepository : AbstractRepositoryBase, IApplicationRepository
    {
        #region other

        private IUIStatementService uiStatementService;
        private ICastleTransactionsService castleTransactionsService;
        private ILookupRepository lookupRepository;
        private ILegalEntityRepository legalEntityRepository;
        private IAddressRepository addressRepository;
        private IITCRepository itcRepository;
        private IX2Repository x2Repository;
        private IReasonRepository reasonRepository;
        private IMemoRepository memoRepository;
        private IOrganisationStructureRepository organisationStructureRepository;
        private IControlRepository controlRepository;

        public ApplicationRepository()
        {
            if (uiStatementService == null)
            {
                uiStatementService = new UIStatementService();
            }
            if (castleTransactionsService == null)
            {
                castleTransactionsService = new CastleTransactionsService();
            }
            if (lookupRepository == null)
            {
                lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            }
            if (x2Repository == null)
            {
                x2Repository = RepositoryFactory.GetRepository<IX2Repository>();
            }
            if (legalEntityRepository == null)
            {
                legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            }
            if (addressRepository == null)
            {
                addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
            }
            if (itcRepository == null)
            {
                itcRepository = RepositoryFactory.GetRepository<IITCRepository>();
            }
            if (reasonRepository == null)
            {
                reasonRepository = RepositoryFactory.GetRepository<IReasonRepository>();
            }
            if (memoRepository == null)
            {
                memoRepository = RepositoryFactory.GetRepository<IMemoRepository>();
            }
            if (organisationStructureRepository == null)
            {
                organisationStructureRepository = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            }
            if (controlRepository == null)
            {
                controlRepository = RepositoryFactory.GetRepository<IControlRepository>();
            }
        }

        public ApplicationRepository(IUIStatementService uiStatementService, ICastleTransactionsService castleTransactionsService, ILookupRepository lookupRepositorysitory, IX2Repository x2Repository, ILegalEntityRepository legalEntityRepository, IAddressRepository addressRepository, IITCRepository itcRepository, IReasonRepository reasonRepository, IMemoRepository memoRepository, IOrganisationStructureRepository organisationStructureRepository)
        {
            this.uiStatementService = uiStatementService;
            this.castleTransactionsService = castleTransactionsService;
            this.lookupRepository = lookupRepositorysitory;
            this.legalEntityRepository = legalEntityRepository;
            this.addressRepository = addressRepository;
            this.x2Repository = x2Repository;
            this.itcRepository = itcRepository;
            this.reasonRepository = reasonRepository;
            this.memoRepository = memoRepository;
            this.organisationStructureRepository = organisationStructureRepository;
        }

        private void AddDomainErrorMessage(IDomainMessage item)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Add(item);
        }

        private void PopulateFurtherLendingBasicDetail(IApplicationProduct cp, IMortgageLoanAccount mla)
        {
            if (cp.ProductType == SAHL.Common.Globals.Products.DefendingDiscountRate)
            {
                IApplicationProductDefendingDiscountLoan prod = cp as IApplicationProductDefendingDiscountLoan;
                prod.VariableLoanInformation.Term = mla.SecuredMortgageLoan.RemainingInstallments;
                prod.VariableLoanInformation.Category = mla.SecuredMortgageLoan.Category;
                prod.VariableLoanInformation.HouseholdIncome = mla.GetHouseholdIncome();
                prod.VariableLoanInformation.MarketRate = mla.SecuredMortgageLoan.ActiveMarketRate;
                prod.VariableLoanInformation.SPV = mla.SecuredMortgageLoan.Account.SPV;
                prod.VariableLoanInformation.EmploymentType = lookupRepository.EmploymentTypes.ObjectDictionary[(mla.GetEmploymentTypeKey()).ToString()];
                prod.VariableLoanInformation.RateConfiguration = mla.SecuredMortgageLoan.RateConfiguration;
                prod.VariableLoanInformation.CreditMatrix = mla.SecuredMortgageLoan.CreditMatrix;

                if (mla.SecuredMortgageLoan.HasInterestOnly())
                {
                    prod.InterestOnlyInformation.MaturityDate = mla.SecuredMortgageLoan.InterestOnly.MaturityDate;
                }
            }
            else if (cp.ProductType == SAHL.Common.Globals.Products.NewVariableLoan)
            {
                IApplicationProductNewVariableLoan prod = cp as IApplicationProductNewVariableLoan;
                prod.VariableLoanInformation.Term = mla.SecuredMortgageLoan.RemainingInstallments;
                prod.VariableLoanInformation.Category = mla.SecuredMortgageLoan.Category;
                prod.VariableLoanInformation.HouseholdIncome = mla.GetHouseholdIncome();
                prod.VariableLoanInformation.MarketRate = mla.SecuredMortgageLoan.ActiveMarketRate;
                prod.VariableLoanInformation.SPV = mla.SecuredMortgageLoan.Account.SPV;
                prod.VariableLoanInformation.EmploymentType = lookupRepository.EmploymentTypes.ObjectDictionary[(mla.GetEmploymentTypeKey()).ToString()];
                prod.VariableLoanInformation.RateConfiguration = mla.SecuredMortgageLoan.RateConfiguration;
                prod.VariableLoanInformation.CreditMatrix = mla.SecuredMortgageLoan.CreditMatrix;

                if (mla.SecuredMortgageLoan.HasInterestOnly())
                {
                    prod.InterestOnlyInformation.MaturityDate = mla.SecuredMortgageLoan.InterestOnly.MaturityDate;
                }
            }
            else if (cp.ProductType == SAHL.Common.Globals.Products.SuperLo)
            {
                IApplicationProductSuperLoLoan prod = cp as IApplicationProductSuperLoLoan;
                prod.VariableLoanInformation.Term = mla.SecuredMortgageLoan.RemainingInstallments;
                prod.VariableLoanInformation.Category = mla.SecuredMortgageLoan.Category;
                prod.VariableLoanInformation.HouseholdIncome = mla.GetHouseholdIncome();
                prod.VariableLoanInformation.MarketRate = mla.SecuredMortgageLoan.ActiveMarketRate;
                prod.VariableLoanInformation.SPV = mla.SecuredMortgageLoan.Account.SPV;
                prod.VariableLoanInformation.EmploymentType = lookupRepository.EmploymentTypes.ObjectDictionary[(mla.GetEmploymentTypeKey()).ToString()];
                prod.VariableLoanInformation.RateConfiguration = mla.SecuredMortgageLoan.RateConfiguration;
                prod.VariableLoanInformation.CreditMatrix = mla.SecuredMortgageLoan.CreditMatrix;

                if (mla.SecuredMortgageLoan.HasInterestOnly())
                {
                    prod.InterestOnlyInformation.MaturityDate = mla.SecuredMortgageLoan.InterestOnly.MaturityDate;
                }
            }
            else if (cp.ProductType == SAHL.Common.Globals.Products.VariableLoan)
            {
                IApplicationProductVariableLoan prod = cp as IApplicationProductVariableLoan;
                prod.VariableLoanInformation.Term = mla.SecuredMortgageLoan.RemainingInstallments;
                prod.VariableLoanInformation.Category = mla.SecuredMortgageLoan.Category;
                prod.VariableLoanInformation.HouseholdIncome = mla.GetHouseholdIncome();
                prod.VariableLoanInformation.MarketRate = mla.SecuredMortgageLoan.ActiveMarketRate;
                prod.VariableLoanInformation.SPV = mla.SecuredMortgageLoan.Account.SPV;
                prod.VariableLoanInformation.EmploymentType = lookupRepository.EmploymentTypes.ObjectDictionary[(mla.GetEmploymentTypeKey()).ToString()];
                prod.VariableLoanInformation.RateConfiguration = mla.SecuredMortgageLoan.RateConfiguration;
                prod.VariableLoanInformation.CreditMatrix = mla.SecuredMortgageLoan.CreditMatrix;

                if (mla.SecuredMortgageLoan.HasInterestOnly())
                {
                    prod.InterestOnlyInformation.MaturityDate = mla.SecuredMortgageLoan.InterestOnly.MaturityDate;
                }
            }
            else if (cp.ProductType == SAHL.Common.Globals.Products.VariFixLoan)
            {
                IApplicationProductVariFixLoan prod = cp as IApplicationProductVariFixLoan;
                prod.VariableLoanInformation.Term = mla.SecuredMortgageLoan.RemainingInstallments;
                prod.VariableLoanInformation.Category = mla.SecuredMortgageLoan.Category;
                prod.VariableLoanInformation.HouseholdIncome = mla.GetHouseholdIncome();
                prod.VariableLoanInformation.MarketRate = mla.SecuredMortgageLoan.ActiveMarketRate;
                prod.VariableLoanInformation.SPV = mla.SecuredMortgageLoan.Account.SPV;
                prod.VariableLoanInformation.EmploymentType = lookupRepository.EmploymentTypes.ObjectDictionary[(mla.GetEmploymentTypeKey()).ToString()];
                prod.VariableLoanInformation.RateConfiguration = mla.SecuredMortgageLoan.RateConfiguration;
                prod.VariableLoanInformation.CreditMatrix = mla.SecuredMortgageLoan.CreditMatrix;
            }
            else if (cp.ProductType == SAHL.Common.Globals.Products.Edge)
            {
                IApplicationProductEdge prod = cp as IApplicationProductEdge;
                prod.VariableLoanInformation.Term = mla.SecuredMortgageLoan.RemainingInstallments;
                prod.VariableLoanInformation.Category = mla.SecuredMortgageLoan.Category;
                prod.VariableLoanInformation.HouseholdIncome = mla.GetHouseholdIncome();
                prod.VariableLoanInformation.MarketRate = mla.SecuredMortgageLoan.ActiveMarketRate;
                prod.VariableLoanInformation.SPV = mla.SecuredMortgageLoan.Account.SPV;
                prod.VariableLoanInformation.EmploymentType = lookupRepository.EmploymentTypes.ObjectDictionary[(mla.GetEmploymentTypeKey()).ToString()];
                prod.VariableLoanInformation.RateConfiguration = mla.SecuredMortgageLoan.RateConfiguration;
                prod.VariableLoanInformation.CreditMatrix = mla.SecuredMortgageLoan.CreditMatrix;

                if (mla.SecuredMortgageLoan.HasInterestOnly())
                {
                    //Edge can have:
                    //              FinancialAdjustmentSource = Edge, FinancialAdjustmentType = InterestOnly
                    //      and/or  FinancialAdjustmentSource = InterestOnly, FinancialAdjustmentType = InterestOnly
                    //  we only want to use the IO/IO configuration

                    if (mla.SecuredMortgageLoan.InterestOnly != null)
                        prod.InterestOnlyInformation.MaturityDate = mla.SecuredMortgageLoan.InterestOnly.MaturityDate;
                }
            }
        }

        #region IApplicationRepository Members

        //Domain implementation of SAHLDB.p_CreateDummyProspect and t_CreateTakeOnRecords
        public void CreateAccountFromApplication(int offerKey, string adUserName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.ExclusionSets.Add(RuleExclusionSets.CreateAccountFromApplication);

            //Create Account
            this.castleTransactionsService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Account", "CreateAccountFromApplication", new ParameterCollection
            {
                new SqlParameter("@offerKey", offerKey),
                new SqlParameter("@userID", adUserName)
            });

            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            int newLegalAgreementDetailTypeKey = accRepo.GetDetailTypeKeyByDescription(SAHL.Common.Constants.DetailTypes.NewLegalAgreementSigned);

            IApplication offer = GetApplicationByKey(offerKey);

            IApplicationInformation offerInfo = offer.GetLatestApplicationInformation();

            IApplicationInformationVariableLoan offerInfoVL = GetApplicationInformationVariableLoan(offerInfo.Key);

            IApplicationProductEdge applicationProductEdge = offer.CurrentProduct as IApplicationProductEdge;
            /*  For Edge, we dont need to set the Maturity Date
                as after a certain period the Loan will be opted out
                of Interest Only and set as an amortising Variable Loan. */
            if (applicationProductEdge == null)
            {
                foreach (IApplicationInformationFinancialAdjustment financialAdjustment in offerInfo.ApplicationInformationFinancialAdjustments)
                {
                    if (financialAdjustment.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.InterestOnly)
                    {
                        IApplicationInformationInterestOnly aiInterestOnly = GetApplicationInformationInterestOnly(offerInfo.Key);

                        if (aiInterestOnly == null)
                            throw new Exception("The Offer has a FinancialAdjustmentTypeSource of InterestOnly, but no OfferInformationInterestOnly record exists");

                        if (offer.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan)
                        {
                            int remainingTerm = Convert.ToInt16(offerInfoVL.Term.HasValue ? offerInfoVL.Term.Value : 0);
                            DateTime maturityDate = DateTime.Now.AddMonths(remainingTerm + 7);
                            aiInterestOnly.MaturityDate = new DateTime(maturityDate.Year, maturityDate.Month, 1);
                            base.Save<IApplicationInformationInterestOnly, ApplicationInformationInterestOnly_DAO>(aiInterestOnly);
                        }
                        else if (offer.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
                        {
                            IMortgageLoanAccount mla = offer.Account as IMortgageLoanAccount;
                            if (mla.SecuredMortgageLoan.HasInterestOnly())
                            {
                                aiInterestOnly.MaturityDate = mla.SecuredMortgageLoan.InterestOnly.MaturityDate;
                                base.Save<IApplicationInformationInterestOnly, ApplicationInformationInterestOnly_DAO>(aiInterestOnly);
                            }
                        }
                        break;
                    }
                }
            }

            spc.ExclusionSets.Remove(RuleExclusionSets.CreateAccountFromApplication);
        }

        public void CalculateOriginationFees(double LoanAmount, double BondRequired, OfferTypes applicationType, double cashOut, double overrideCancelFeeAmount, bool capitaliseFees, bool NCACompliant, bool IsBondExceptionAction, bool IsDiscountedInitiationFee, out double? initiationFeeDiscount, out double InitiationFee, out double RegistrationFee, out double CancelFee, out double InterimInterest, out double BondToRegister, bool IsQuickPayLoan, double HouseholdIncome, int EmploymentTypeKey, double PropertyValue, int ApplicationParentAccountKey, bool IsStaffLoan, DateTime ApplicationStartDate, bool capitaliseInitiationFee, bool isGEPF)
        {
            ApplicationCalculateMortgageLoanHelper.CalculateOriginationFees(LoanAmount, BondRequired, applicationType, cashOut, overrideCancelFeeAmount, capitaliseFees, NCACompliant, IsBondExceptionAction, IsDiscountedInitiationFee, out initiationFeeDiscount, out InitiationFee, out RegistrationFee, out CancelFee, out InterimInterest, out BondToRegister, IsQuickPayLoan, HouseholdIncome, EmploymentTypeKey, PropertyValue, ApplicationParentAccountKey, IsStaffLoan, ApplicationStartDate, capitaliseInitiationFee, isGEPF);
        }

        /// <summary>
        /// Implements <see cref="IApplicationRepository.GetApplicationByKey"></see>.
        /// </summary>
        public IApplication GetApplicationByKey(int Key)
        {
            Application_DAO App = Application_DAO.TryFind(Key);
            if (App != null)
            {
                IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IApplication, Application_DAO>(App);
            }
            return null;
        }

        /// <summary>
        /// Gets a list of application keys starting with <c>prefix</c>.
        /// </summary>
        /// <param name="prefix">The starting characters.</param>
        /// <param name="maxCount">The maximum number of rows to return.</param>
        /// <returns>A list of application keys.  If prefix is null or empty, and empty list is returned.</returns>
        public IList<int> GetApplicationKeys(string prefix, int maxCount)
        {
            IList<int> keys = new List<int>();
            if (String.IsNullOrEmpty(prefix) || (prefix.Trim().Length == 0))
                return keys;

            IDbConnection conn = Helper.GetSQLDBConnection();
            conn.Open();

            IDataReader reader = null;
            try
            {
                string sql = String.Format("select top {0} o.OfferKey from Offer o where o.OfferKey like '{1}%'", maxCount, prefix);
                reader = Helper.ExecuteReader(conn, sql);
                while (reader.Read())
                {
                    keys.Add(reader.GetInt32(0));
                }
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Dispose();
            }
            return keys;
        }

        /// <summary>
        /// Implements <see cref="IApplicationRepository.GetApplicationLifeByKey"></see>.
        /// </summary>
        public IApplicationLife GetApplicationLifeByKey(int Key)
        {
            return base.GetByKey<IApplicationLife, ApplicationLife_DAO>(Key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        public IEventList<IApplication> GetApplicationByAccountKey(int AccountKey)
        {
            AccountRepository Rep = new AccountRepository();
            IAccount Acc = Rep.GetAccountByKey(AccountKey);
            return Acc.Applications;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reservedAccountKey"></param>
        /// <returns></returns>
        public IApplication GetApplicationByReservedAccountKey(int reservedAccountKey)
        {
            Application_DAO[] appDAO = Application_DAO.FindAllByProperty("ReservedAccount.Key", reservedAccountKey);

            if (appDAO != null && appDAO.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return bmtm.GetMappedType<IApplication>(appDAO[0]);
            }

            return null;
        }

        /// <summary>
        /// Get the last disbursed application.
        /// Readvance, Life and unknown applications are ignored.
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        public IApplication GetLastDisbursedApplicationByAccountKey(int AccountKey)
        {
            string HQL = "from Application_DAO a where a.Account.Key = ? and a.ApplicationStatus.Key = ? and a.ApplicationType.Key not in (?, ?, ?) order by a.ApplicationEndDate desc";
            SimpleQuery<Application_DAO> query
                = new SimpleQuery<Application_DAO>(HQL, AccountKey, (int)OfferStatuses.Accepted, (int)OfferTypes.Unknown, (int)OfferTypes.ReAdvance, (int)OfferTypes.Life);
            Application_DAO[] appDAO = query.Execute();

            if (appDAO.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return bmtm.GetMappedType<IApplication>(appDAO[0]);
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IApplicationExpense GetEmptyApplicationExpense()
        {
            return base.CreateEmpty<IApplicationExpense, ApplicationExpense_DAO>();

            //return new ApplicationExpense(new ApplicationExpense_DAO());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IApplicationDebtSettlement GetEmptyApplicationDebtSettlement()
        {
            return base.CreateEmpty<IApplicationDebtSettlement, ApplicationDebtSettlement_DAO>();

            //return new ApplicationDebtSettlement(new ApplicationDebtSettlement_DAO());
        }

        /// <summary>
        /// Implements <see cref="IApplicationRepository.GetEmptyApplicationInformation"></see>.
        /// </summary>
        public IApplicationInformation GetEmptyApplicationInformation()
        {
            return base.CreateEmpty<IApplicationInformation, ApplicationInformation_DAO>();

            //return new ApplicationInformation(new ApplicationInformation_DAO());
        }

        /// <summary>
        /// Implements <see cref="IApplicationRepository.GetEmptyApplicationLife"></see>.
        /// </summary>
        public IApplicationLife GetEmptyApplicationLife()
        {
            ApplicationLife_DAO applicationLife = new ApplicationLife_DAO();
            ApplicationLifeDetail_DAO applicationLifeDetail = new ApplicationLifeDetail_DAO();

            applicationLife.ApplicationLifeDetail = applicationLifeDetail;
            applicationLifeDetail.Application = applicationLife;

            return new ApplicationLife(applicationLife);
        }

        /// <summary>
        /// Gets an empty application role
        /// </summary>
        /// <returns>The <see cref="IApplicationRole"></see>.</returns>
        public IApplicationRole GetEmptyApplicationRole()
        {
            return base.CreateEmpty<IApplicationRole, ApplicationRole_DAO>();

            //return new ApplicationRole(new ApplicationRole_DAO());
        }

        /// <summary>
        /// Get an empty ApplicationRateOverride, for SL and IO
        /// </summary>
        /// <returns></returns>
        public IApplicationInformationFinancialAdjustment GetEmptyApplicationInformationFinancialAdjustment()
        {
            return base.CreateEmpty<IApplicationInformationFinancialAdjustment, ApplicationInformationFinancialAdjustment_DAO>();

            //return new ApplicationInformationRateOverride(new ApplicationInformationRateOverride_DAO());
        }

        /// <summary>
        /// SAves and IApplication object and its properties to the database (SQLUpdate)
        /// </summary>
        /// <param name="applicationToSave"></param>
        public void SaveApplication(IApplication applicationToSave)
        {
            DetermineStopOrderDiscountAttribute(applicationToSave);

            //Save the applcation to partially persist application/applicationInformation/ApplicationInformationVariableLoan
            base.Save<IApplication, Application_DAO>(applicationToSave);

            //Determine & Update ApplicationAttributes that need to be applied based on the partially persisted data
            //We don't want to do the application attributes calculation on further lending
            var isFurtherLending = applicationToSave is IApplicationFurtherLending;
            if (!isFurtherLending)
            {
                UpdateApplicationRoleAttribute(applicationToSave, OfferRoleAttributeTypes.ReturningClient);
                UpdateApplicationAttributes(applicationToSave);
            }

            // Determine the SPV on the Application
            UpdateSPV(applicationToSave);

            base.Save<IApplication, Application_DAO>(applicationToSave);
        }

        private void UpdateApplicationAttributes(IApplication applicationToSave)
        {
            // Don't determine Application Attributes if the application or applicationInformation has been accepted
            IApplicationInformation latestApplicationInformation = applicationToSave.GetLatestApplicationInformation();
            if (!applicationToSave.IsOpen
                || latestApplicationInformation == null
                || latestApplicationInformation.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                return;

            var applicationAttributeTypeKeys = DetermineApplicationAttributeTypes(applicationToSave);
            UpdateApplicationAttributeTypes(applicationAttributeTypeKeys, applicationToSave);
        }

        private void UpdateApplicationRoleAttribute(IApplication applicationToSave, OfferRoleAttributeTypes OfferRoleAttributeType)
        {
            // Don't determine Application Role Attributes if the application or applicationInformation has been accepted
            IApplicationInformation latestApplicationInformation = applicationToSave.GetLatestApplicationInformation();
            if (!applicationToSave.IsOpen
                || latestApplicationInformation == null
                || latestApplicationInformation.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                return;

            IRuleService ruleServ = ServiceFactory.GetService<IRuleService>();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            foreach (IApplicationRole applicationRole in applicationToSave.ApplicationRoles)
            {
                if (OfferRoleAttributeType == OfferRoleAttributeTypes.ReturningClient)
                {
                    if ((applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant ||
                        applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant ||
                        applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor ||
                        applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor) &&
                        ruleServ.ExecuteRule(spc.DomainMessages, "CheckIsReturningClient", applicationRole) == 1 &&
                        !applicationRole.HasAttribute(OfferRoleAttributeType))
                    {
                        IApplicationRoleAttribute applicationRoleAttribute = GetEmptyApplicationRoleAttribute();
                        applicationRoleAttribute.OfferRole = applicationRole;
                        applicationRoleAttribute.OfferRoleAttributeType = lookupRepository.ApplicationRoleAttributesTypes.First(x => x.Key == (int)OfferRoleAttributeType);
                        applicationRole.ApplicationRoleAttributes.Add(spc.DomainMessages, applicationRoleAttribute);
                    }
                }
            }
        }

        private void UpdateSPV(IApplication application)
        {
            ISPVService spvService = ServiceFactory.GetService<ISPVService>();
            spvService.DetermineSPVOnApplication(application);
        }

        public void SaveApplicationUnsecuredLending(IApplicationUnsecuredLending ApplicationToSave)
        {
            base.Save<IApplicationUnsecuredLending, ApplicationUnsecuredLending_DAO>(ApplicationToSave);
        }

        /// <summary>
        /// Saves and ICallback to the database (SQLUpdate)
        /// </summary>
        /// <param name="callback"></param>
        public void SaveCallback(ICallback callback)
        {
            base.Save<ICallback, Callback_DAO>(callback);

            //IDAOObject dao = callback as IDAOObject;
            //Callback_DAO cb = (Callback_DAO)dao.GetDAOObject();
            //cb.SaveAndFlush();
            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        /// <summary>
        /// Gets the newest open callback for an ApplicationKey.
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="OpenCallbacksOnly">boolean to specify whether you only want open callbacks returned.</param>
        /// <returns></returns>
        public ICallback GetLatestCallBackByApplicationKey(int ApplicationKey, bool OpenCallbacksOnly)
        {
            ApplicationCallbackQuery query = new ApplicationCallbackQuery(ApplicationKey, true, OpenCallbacksOnly);
            object o = Callback_DAO.ExecuteQuery(query);

            List<Callback_DAO> lst = o as List<Callback_DAO>;
            if (lst.Count > 0)
                return new Callback(lst[0]);
            else
                return null;
        }

        /// <summary>
        /// Gets all the callbacks for an ApplicationKey.
        /// </summary>
        /// <param name="ApplicationKey">The integer Application key.</param>
        /// <param name="OpenCallbacksOnly">boolean to specify whether you only want open callbacks returned.</param>
        /// <returns></returns>
        public IEventList<ICallback> GetCallBacksByApplicationKey(int ApplicationKey, bool OpenCallbacksOnly)
        {
            object results = null;

            ApplicationCallbackQuery query = new ApplicationCallbackQuery(ApplicationKey, false, OpenCallbacksOnly);
            results = Callback_DAO.ExecuteQuery(query);

            IList<Callback_DAO> callbacks = results as IList<Callback_DAO>;

            DAOEventList<Callback_DAO, ICallback, Callback> daoList = new DAOEventList<Callback_DAO, ICallback, Callback>(callbacks);

            return daoList;
        }

        /// <summary>
        /// Completes a Callback
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="CompletedDate"></param>
        public bool CompleteCallback(int ApplicationKey, DateTime CompletedDate)
        {
            IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
            IEventList<ICallback> callbacks = AR.GetCallBacksByApplicationKey(ApplicationKey, true);

            bool callBackCompleted = false;

            foreach (ICallback cb in callbacks)
            {
                // only update callbacks that havent been completed
                if (cb.CompletedDate.HasValue == false)
                {
                    cb.CompletedDate = DateTime.Now;
                    cb.CompletedUser = cb.EntryUser;

                    // save callback
                    AR.SaveCallback(cb);

                    callBackCompleted = true;
                }
            }

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

            return callBackCompleted;
        }

        /// <summary>
        /// Get the dkefault reset configuration (this is currently hardcoded to return the 18th reset).
        /// </summary>
        /// <returns></returns>
        public IResetConfiguration GetApplicationDefaultResetConfiguration()
        {
            return base.GetByKey<IResetConfiguration, ResetConfiguration_DAO>(2);
        }

        /// <summary>
        /// Get the reset configuration based on the SPV and Product configuration.
        /// </summary>
        /// <returns></returns>
        public IResetConfiguration GetApplicationResetConfiguration(int SPVKey, int ProductKey)
        {
            int resetConfigKey = 2;

            string sql = string.Format(@"SELECT [2AM].[dbo].[fResetConfigurationDetermine] ({0}, {1})", SPVKey.ToString(), ProductKey.ToString());
            using (IDbConnection con = Helper.GetSQLDBConnection())
            {
                object o = Helper.ExecuteScalar(con, sql);
                if (o != null)
                    resetConfigKey = (int)o;

                return base.GetByKey<IResetConfiguration, ResetConfiguration_DAO>(resetConfigKey);
            }
        }

        public IApplicationUnknown GetEmptyUnknownApplicationType(int originationSourceKey)
        {
            AccountSequence_DAO AS = new AccountSequence_DAO();
            AS.SaveAndFlush();

            // Create an application
            ApplicationUnknown_DAO App = new ApplicationUnknown_DAO();

            // set the applications reserved account key
            App.ReservedAccount = AS;
            IApplicationUnknown Unk = new ApplicationUnknown(App);
            Unk.ApplicationStatus = lookupRepository.ApplicationStatuses.ObjectDictionary["1"];
            OriginationSource_DAO dao = OriginationSource_DAO.Find(originationSourceKey);
            Unk.ApplicationStartDate = DateTime.Now;
            Unk.OriginationSource = new OriginationSource(dao);
            SaveApplication(Unk);
            return Unk;
        }

        /// <summary>
        /// Remove the application from memory so that we can re-load it as a different type.
        /// </summary>
        /// <param name="businessObj"></param>
        protected void EvictApplication(object businessObj)
        {
            ICommonRepository commRepo = RepositoryFactory.GetRepository<ICommonRepository>();
            commRepo.ClearFromNHibernateSession(businessObj);
        }

        protected void UpdateApplicationType(int ApplicationKey, int ApplicationTypeKey)
        {
            string sqlQuery = String.Format("update offer set offertypekey={0} where offerkey={1}", ApplicationTypeKey, ApplicationKey);
            castleTransactionsService.ExecuteNonQueryOnCastleTran(sqlQuery, typeof(Application_DAO), null);
        }

        public IApplicationMortgageLoanSwitch CreateSwitchLoanApplication(IOriginationSource OriginationSource, ProductsSwitchLoanAtCreation ProductType, IApplicationUnknown AppUnk)
        {
            ApplicationMortgageLoanSwitch_DAO App;
            if (null != AppUnk)
            {
                int appKey = AppUnk.Key;
                EvictApplication(AppUnk);
                UpdateApplicationType(appKey, 6);
                App = ApplicationMortgageLoanSwitch_DAO.Find(appKey);
            }
            else
            {
                // reserve an account key
                AccountSequence_DAO AS = new AccountSequence_DAO();
                AS.SaveAndFlush();

                // Create an application
                App = new ApplicationMortgageLoanSwitch_DAO();

                // set the applications reserved account key
                App.ReservedAccount = AS;
            }

            // Create a mortgageloan detail
            ApplicationMortgageLoanDetail_DAO AMD = new ApplicationMortgageLoanDetail_DAO();
            AMD.MortgageLoanPurpose = MortgageLoanPurpose_DAO.Find(2);

            // set the applications mortgageloan detail and link the mortgageloan detail to its owning application
            App.ApplicationMortgageLoanDetail = AMD;
            AMD.Application = App;

            IApplicationMortgageLoanSwitch AppMLS = new ApplicationMortgageLoanSwitch(App);
            AppMLS.OriginationSource = OriginationSource;
            AppMLS.SetProduct((ProductsSwitchLoan)ProductType);
            AppMLS.ApplicationStatus = lookupRepository.ApplicationStatuses.ObjectDictionary["1"];
            AppMLS.ApplicationStartDate = DateTime.Now;

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

            return AppMLS;
        }

        //public void SetApplicationTypeToUnKnown(IApplication app)
        //{
        //    int appKey = app.Key;
        //    EvictApplication(app);
        //    UpdateApplicationType(appKey, (int)OfferTypes.Unknown);
        //}

        public IApplicationMortgageLoanNewPurchase CreateNewPurchaseApplication(IOriginationSource OriginationSource, ProductsNewPurchaseAtCreation ProductType, IApplicationUnknown AppUnk)
        {
            ApplicationMortgageLoanNewPurchase_DAO App = null;
            if (null != AppUnk)
            {
                int appKey = AppUnk.Key;
                EvictApplication(AppUnk);
                UpdateApplicationType(appKey, 7);
                App = ApplicationMortgageLoanNewPurchase_DAO.Find(appKey);
            }
            else
            {
                // reserve an account key
                AccountSequence_DAO AS = new AccountSequence_DAO();
                AS.SaveAndFlush();

                // Create an application
                App = new ApplicationMortgageLoanNewPurchase_DAO();

                // set the applications reserved account key
                App.ReservedAccount = AS;
            }

            // Create a mortgageloan detail
            ApplicationMortgageLoanDetail_DAO AMD = new ApplicationMortgageLoanDetail_DAO();
            AMD.MortgageLoanPurpose = MortgageLoanPurpose_DAO.Find(3);

            // set the applications mortgageloan detail and link the mortgageloan detail to its owning application
            App.ApplicationMortgageLoanDetail = AMD;
            AMD.Application = App;

            IApplicationMortgageLoanNewPurchase AppMNP = new ApplicationMortgageLoanNewPurchase(App);
            AppMNP.OriginationSource = OriginationSource;
            AppMNP.SetProduct((ProductsNewPurchase)ProductType);
            AppMNP.ApplicationStatus = lookupRepository.ApplicationStatuses.ObjectDictionary["1"];
            AppMNP.ApplicationStartDate = DateTime.Now;

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

            return AppMNP;
        }

        public IApplicationMortgageLoanRefinance CreateRefinanceApplication(IOriginationSource p_OriginationSource, ProductsRefinanceAtCreation ProductType, IApplicationUnknown AppUnk)
        {
            ApplicationMortgageLoanRefinance_DAO App = null;
            if (null != AppUnk)
            {
                int appKey = AppUnk.Key;
                EvictApplication(AppUnk);
                UpdateApplicationType(AppUnk.Key, 8);
                App = ApplicationMortgageLoanRefinance_DAO.Find(AppUnk.Key);
            }
            else
            {
                // reserve an account key
                AccountSequence_DAO AS = new AccountSequence_DAO();
                AS.SaveAndFlush();

                // Create an application
                App = new ApplicationMortgageLoanRefinance_DAO();

                // set the applications reserved account key
                App.ReservedAccount = AS;
            }

            // Create a mortgageloan detail
            ApplicationMortgageLoanDetail_DAO AMD = new ApplicationMortgageLoanDetail_DAO();
            AMD.MortgageLoanPurpose = MortgageLoanPurpose_DAO.Find(4);

            // set the applications mortgageloan detail and link the mortgageloan detail to its owning application
            App.ApplicationMortgageLoanDetail = AMD;
            AMD.Application = App;

            IApplicationMortgageLoanRefinance AppMNP = new ApplicationMortgageLoanRefinance(App);
            AppMNP.OriginationSource = p_OriginationSource;
            AppMNP.SetProduct((ProductsRefinance)ProductType);
            AppMNP.ApplicationStatus = lookupRepository.ApplicationStatuses.ObjectDictionary["1"];
            AppMNP.ApplicationStartDate = DateTime.Now;

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

            return AppMNP;
        }

        public IApplicationFurtherLoan CreateFurtherLoanApplication(IMortgageLoanAccount mla, bool save)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            ApplicationFurtherLoan_DAO fldao = new ApplicationFurtherLoan_DAO();

            ApplicationMortgageLoanDetail_DAO AMD = new ApplicationMortgageLoanDetail_DAO();
            AMD.MortgageLoanPurpose = MortgageLoanPurpose_DAO.Find((int)MortgageLoanPurposes.FurtherLoan);

            // set the applications mortgageloan detail and link the mortgageloan detail to its owning application
            fldao.ApplicationMortgageLoanDetail = AMD;
            AMD.Application = fldao;

            IApplicationFurtherLoan fl = new ApplicationFurtherLoan(fldao);
            fl.ApplicationStatus = lookupRepository.ApplicationStatuses.ObjectDictionary["1"];

            // get the product type so the FL can have the same product type (non-changeable)
            // set its own mort loan purpose (5) (and offertype 4)
            // set fields based on lara's spread
            IProduct product = mla.Product;
            IApplicationRepository apRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            //IApplicationType appType = lookupRepository.ApplicationTypes.ObjectDictionary["4"];
            IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            IAccountSequence ass = accRepo.GetAccountSequenceByKey(mla.Key);
            fl.ReservedAccount = ass;

            //IMortgageLoanPurpose mlp = mlRepo.GetMortgageLoanPurposeByKey((int)MortgageLoanPurposes.FurtherLoan);
            fl.Account = mla;
            fl.SetProduct(product);
            fl.OriginationSource = mla.OriginationSource;
            fl.ApplicationStartDate = DateTime.Now;

            fl.ResetConfiguration = mla.CurrentMortgageLoanApplication.ResetConfiguration;
            fl.TransferringAttorney = mla.CurrentMortgageLoanApplication.TransferringAttorney;
            fl.ClientEstimatePropertyValuation = mla.CurrentMortgageLoanApplication.ClientEstimatePropertyValuation;

            // copy in the rate overrides
            IEventList<IFinancialAdjustment> adjustments = mla.SecuredMortgageLoan.FinancialAdjustments;
            IApplicationInformation info = fl.GetLatestApplicationInformation();
            foreach (IFinancialAdjustment financialAdjustment in adjustments)
            {
                if ((financialAdjustment.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.DefendingCancellations
                    || financialAdjustment.FinancialAdjustmentTypeSource.Key != (int)FinancialAdjustmentTypeSources.DiscountedLinkrate
                    || financialAdjustment.FinancialAdjustmentTypeSource.Key != (int)FinancialAdjustmentTypeSources.InterestOnly
                    || financialAdjustment.FinancialAdjustmentTypeSource.Key != (int)FinancialAdjustmentTypeSources.SAHLStaff
                    || financialAdjustment.FinancialAdjustmentTypeSource.Key != (int)FinancialAdjustmentTypeSources.SuperLo)
                    && financialAdjustment.FinancialAdjustmentStatus.Key == (int)FinancialAdjustmentStatuses.Active)
                {
                    IApplicationInformationFinancialAdjustment aro = new ApplicationInformationFinancialAdjustment(new ApplicationInformationFinancialAdjustment_DAO());
                    aro.ApplicationInformation = info;
                    aro.ApplicationInformation = info;
                    aro.Discount = financialAdjustment.InterestRateAdjustment != null ? financialAdjustment.InterestRateAdjustment.Adjustment : 0;
                    aro.FixedRate = financialAdjustment.FixedRateAdjustment != null ? financialAdjustment.FixedRateAdjustment.Rate : 0;
                    aro.FinancialAdjustmentTypeSource = financialAdjustment.FinancialAdjustmentTypeSource;
                    aro.Term = financialAdjustment.EndDate.HasValue && financialAdjustment.FromDate.HasValue ?
                                                  financialAdjustment.EndDate.Value.MonthDifference(financialAdjustment.FromDate.Value, 1) : 0;
                    aro.FromDate = financialAdjustment.FromDate;

                    fl.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments.Add(spc.DomainMessages, aro);
                }
            }

            fl.Property = mla.SecuredMortgageLoan.Property;
            fl.NumApplicants = mla.Roles.Count;

            PopulateFurtherLendingBasicDetail(fl.CurrentProduct, mla);

            if (!save)
            {
                //FLoans always have capitalised fees
                IApplicationAttribute apAt = GetEmptyApplicationAttribute();
                apAt.ApplicationAttributeType = lookupRepository.ApplicationAttributesTypes.ObjectDictionary[Convert.ToString((int)OfferAttributeTypes.CapitalizeFees)];
                apAt.Application = fl;
                fl.ApplicationAttributes.Add(spc.DomainMessages, apAt);

                //if (ValidationHelper.PrincipalHasValidationErrors())
                //    throw new DomainValidationException();

                return fl;
            }

            SaveApplication(fl);

            Application_MortgageLoan_Role role;
            foreach (IRole r in mla.Roles)
            {
                if (r.GeneralStatus.Key == (int)GeneralStatuses.Active && r.RoleType.Key != (int)RoleTypes.AssuredLife)
                {
                    if (r.RoleType.Key == (int)RoleTypes.MainApplicant)
                        role = Application_MortgageLoan_Role.APPLICATION_MAIN_APPLICANT;
                    else
                        role = Application_MortgageLoan_Role.APPLICATION_SURETOR;

                    fl.AddApplicationRole(role, r.LegalEntity);
                }
            }

            //need to check to see if any le's are marked as income contributors,
            //if none, mark all as contributors
            bool hasIncomeContributor = false;

            // Get the latest IMortgageLoanApplication,
            // For all the corresponding LEs copy the corresponding Attributetypes
            foreach (IApplicationRole applicationRole in mla.CurrentMortgageLoanApplication.ApplicationRoles)
            {
                // Find the corresponding LE in the further loan
                foreach (IApplicationRole flApplicationRole in fl.ApplicationRoles)
                {
                    if (flApplicationRole.LegalEntity.Key == applicationRole.LegalEntity.Key)
                    {
                        // Add the Attributes
                        foreach (IApplicationRoleAttribute applicationRoleAttribute in applicationRole.ApplicationRoleAttributes)
                        {
                            IApplicationRoleAttribute newAttribute = GetEmptyApplicationRoleAttribute();
                            newAttribute.OfferRole = flApplicationRole;
                            newAttribute.OfferRoleAttributeType = applicationRoleAttribute.OfferRoleAttributeType;
                            flApplicationRole.ApplicationRoleAttributes.Add(spc.DomainMessages, newAttribute);

                            if (applicationRoleAttribute.OfferRoleAttributeType.Key == (int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor)
                                hasIncomeContributor = true;
                        }
                    }
                }
            }

            //need to check to see if any le's are marked as income contributors,
            //if none, mark all as contributors
            if (hasIncomeContributor == false)
            {
                foreach (IApplicationRole appRole in fl.ApplicationRoles)
                {
                    IApplicationRoleAttribute applicationRoleAttribute = GetEmptyApplicationRoleAttribute();
                    applicationRoleAttribute.OfferRole = appRole;
                    applicationRoleAttribute.OfferRoleAttributeType = lookupRepository.ApplicationRoleAttributesTypes.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor)];
                    appRole.ApplicationRoleAttributes.Add(spc.DomainMessages, applicationRoleAttribute);
                }
            }

            //FLoans always have capitalised fees
            IApplicationAttribute appAt = GetEmptyApplicationAttribute();
            appAt.ApplicationAttributeType = lookupRepository.ApplicationAttributesTypes.ObjectDictionary[Convert.ToString((int)OfferAttributeTypes.CapitalizeFees)];
            appAt.Application = fl;
            fl.ApplicationAttributes.Add(spc.DomainMessages, appAt);

            return fl;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mla"></param>
        /// <param name="save"></param>
        /// <returns></returns>
        public IApplicationReAdvance CreateReAdvanceApplication(IMortgageLoanAccount mla, bool save)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            ApplicationReAdvance_DAO raDAO = new ApplicationReAdvance_DAO();

            // get ML details stuff
            ApplicationMortgageLoanDetail_DAO AMD = new ApplicationMortgageLoanDetail_DAO();
            AMD.MortgageLoanPurpose = MortgageLoanPurpose_DAO.Find((int)MortgageLoanPurposes.ReAdvance);
            raDAO.ApplicationMortgageLoanDetail = AMD;
            AMD.Application = raDAO;

            IApplicationReAdvance ra = new ApplicationReAdvance(raDAO);

            ra.ApplicationStatus = lookupRepository.ApplicationStatuses.ObjectDictionary["1"];

            // get the product type so the FL can have the same product type (non-changeable)
            // set its own mort loan purpose (5) (and offertype 4)
            // set fields based on lara's spread
            IProduct product = mla.Product;
            IApplicationRepository apRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            //IApplicationType appType = lookupRepository.ApplicationTypes.ObjectDictionary["2"];
            IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            IAccountSequence ass = accRepo.GetAccountSequenceByKey(mla.Key);
            ra.ReservedAccount = ass;

            // set ML purpose
            //IMortgageLoanPurpose mlp = mlRepo.GetMortgageLoanPurposeByKey((int)MortgageLoanPurposes.ReAdvance);
            ra.Account = mla;
            ra.SetProduct(product);
            ra.OriginationSource = mla.OriginationSource;
            ra.ApplicationStartDate = DateTime.Now;

            // pull in the rate overrides.
            IEventList<IFinancialAdjustment> financialAdjustments = mla.SecuredMortgageLoan.FinancialAdjustments;
            IApplicationInformation info = ra.GetLatestApplicationInformation();
            foreach (IFinancialAdjustment financialAdjustment in financialAdjustments)
            {
                if ((financialAdjustment.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.DefendingCancellations
                    || financialAdjustment.FinancialAdjustmentTypeSource.Key != (int)FinancialAdjustmentTypeSources.DiscountedLinkrate
                    || financialAdjustment.FinancialAdjustmentTypeSource.Key != (int)FinancialAdjustmentTypeSources.InterestOnly
                    || financialAdjustment.FinancialAdjustmentTypeSource.Key != (int)FinancialAdjustmentTypeSources.SAHLStaff
                    || financialAdjustment.FinancialAdjustmentTypeSource.Key != (int)FinancialAdjustmentTypeSources.SuperLo)
                    && financialAdjustment.FinancialAdjustmentStatus.Key == (int)FinancialAdjustmentStatuses.Active)
                {
                    IApplicationInformationFinancialAdjustment newFinancialAdjustment = new ApplicationInformationFinancialAdjustment(new ApplicationInformationFinancialAdjustment_DAO());
                    newFinancialAdjustment.ApplicationInformation = info;
                    newFinancialAdjustment.Discount = financialAdjustment.InterestRateAdjustment != null ? financialAdjustment.InterestRateAdjustment.Adjustment : 0;
                    newFinancialAdjustment.FixedRate = financialAdjustment.FixedRateAdjustment != null ? financialAdjustment.FixedRateAdjustment.Rate : 0;
                    newFinancialAdjustment.FinancialAdjustmentTypeSource = financialAdjustment.FinancialAdjustmentTypeSource;
                    newFinancialAdjustment.Term = financialAdjustment.EndDate.HasValue && financialAdjustment.FromDate.HasValue ?
                                                  financialAdjustment.EndDate.Value.MonthDifference(financialAdjustment.FromDate.Value, 1) : 0;
                    newFinancialAdjustment.FromDate = financialAdjustment.FromDate;

                    ra.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments.Add(spc.DomainMessages, newFinancialAdjustment);
                }
            }

            ra.Property = mla.SecuredMortgageLoan.Property;
            ra.NumApplicants = mla.Roles.Count;

            PopulateFurtherLendingBasicDetail(ra.CurrentProduct, mla);

            if (!save)
            {
                //if (ValidationHelper.PrincipalHasValidationErrors())
                //    throw new DomainValidationException();

                return ra;
            }

            SaveApplication(ra);

            Application_MortgageLoan_Role role;
            foreach (IRole r in mla.Roles)
            {
                if (r.GeneralStatus.Key == (int)GeneralStatuses.Active && r.RoleType.Key != (int)RoleTypes.AssuredLife)
                {
                    if (r.RoleType.Key == (int)RoleTypes.MainApplicant)
                        role = Application_MortgageLoan_Role.APPLICATION_MAIN_APPLICANT;
                    else
                        role = Application_MortgageLoan_Role.APPLICATION_SURETOR;

                    ra.AddApplicationRole(role, r.LegalEntity);
                }
            }

            //need to check to see if any le's are marked as income contributors,
            //if none, mark all as contributors
            bool hasIncomeContributor = false;

            // Get the latest IMortgageLoanApplication,
            // For all the corresponding LEs copy the corresponding Attributetypes
            foreach (IApplicationRole applicationRole in mla.CurrentMortgageLoanApplication.ApplicationRoles)
            {
                // Find the corresponding LE in the further loan
                foreach (IApplicationRole raApplicationRole in ra.ApplicationRoles)
                {
                    if (raApplicationRole.LegalEntity.Key == applicationRole.LegalEntity.Key)
                    {
                        // Add the Attributes
                        foreach (IApplicationRoleAttribute applicationRoleAttribute in applicationRole.ApplicationRoleAttributes)
                        {
                            IApplicationRoleAttribute newAttribute = GetEmptyApplicationRoleAttribute();
                            newAttribute.OfferRole = raApplicationRole;
                            newAttribute.OfferRoleAttributeType = applicationRoleAttribute.OfferRoleAttributeType;
                            raApplicationRole.ApplicationRoleAttributes.Add(spc.DomainMessages, newAttribute);

                            if (applicationRoleAttribute.OfferRoleAttributeType.Key == (int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor)
                                hasIncomeContributor = true;
                        }
                    }
                }
            }

            //need to check to see if any le's are marked as income contributors,
            //if none, mark all as contributors
            if (hasIncomeContributor == false)
            {
                foreach (IApplicationRole appRole in ra.ApplicationRoles)
                {
                    IApplicationRoleAttribute applicationRoleAttribute = GetEmptyApplicationRoleAttribute();
                    applicationRoleAttribute.OfferRole = appRole;
                    applicationRoleAttribute.OfferRoleAttributeType = lookupRepository.ApplicationRoleAttributesTypes.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor)];
                    appRole.ApplicationRoleAttributes.Add(spc.DomainMessages, applicationRoleAttribute);
                }
            }

            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();

            return ra;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mla"></param>
        /// <param name="save"></param>
        /// <returns></returns>
        public IApplicationFurtherAdvance CreateFurtherAdvanceApplication(IMortgageLoanAccount mla, bool save)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            ApplicationFurtherAdvance_DAO faDAO = new ApplicationFurtherAdvance_DAO();

            // get ML details stuff
            ApplicationMortgageLoanDetail_DAO AMD = new ApplicationMortgageLoanDetail_DAO();
            AMD.MortgageLoanPurpose = MortgageLoanPurpose_DAO.Find((int)MortgageLoanPurposes.FurtherAdvance);
            faDAO.ApplicationMortgageLoanDetail = AMD;
            AMD.Application = faDAO;

            IApplicationFurtherAdvance fa = new ApplicationFurtherAdvance(faDAO);

            fa.ApplicationStatus = lookupRepository.ApplicationStatuses.ObjectDictionary["1"];

            // get the product type so the FL can have the same product type (non-changeable)
            // set its own mort loan purpose (5) (and offertype 4)
            // set fields based on lara's spread
            IProduct product = mla.Product;
            IApplicationRepository apRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            //IApplicationType appType = lookupRepository.ApplicationTypes.ObjectDictionary["3"];
            IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            IAccountSequence ass = accRepo.GetAccountSequenceByKey(mla.Key);
            fa.ReservedAccount = ass;

            // set ML purpose
            //IMortgageLoanPurpose mlp = mlRepo.GetMortgageLoanPurposeByKey((int)MortgageLoanPurposes.FurtherAdvance);
            fa.Account = mla;
            fa.SetProduct(product);
            fa.OriginationSource = mla.OriginationSource;
            fa.ApplicationStartDate = DateTime.Now;

            // pull in the rate overrides.
            IEventList<IFinancialAdjustment> overrides = mla.SecuredMortgageLoan.FinancialAdjustments;
            IApplicationInformation info = fa.GetLatestApplicationInformation();
            foreach (IFinancialAdjustment ro in overrides)
            {
                if ((ro.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.DefendingCancellations
                        || ro.FinancialAdjustmentTypeSource.Key != (int)FinancialAdjustmentTypeSources.DiscountedLinkrate
                        || ro.FinancialAdjustmentTypeSource.Key != (int)FinancialAdjustmentTypeSources.InterestOnly
                        || ro.FinancialAdjustmentTypeSource.Key != (int)FinancialAdjustmentTypeSources.SAHLStaff
                        || ro.FinancialAdjustmentTypeSource.Key != (int)FinancialAdjustmentTypeSources.SuperLo)
                        && ro.FinancialAdjustmentStatus.Key == (int)GeneralStatuses.Active)
                {
                    IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment = new ApplicationInformationFinancialAdjustment(new ApplicationInformationFinancialAdjustment_DAO());
                    applicationInformationFinancialAdjustment.ApplicationInformation = info;
                    applicationInformationFinancialAdjustment.Discount = ro.InterestRateAdjustment != null ? ro.InterestRateAdjustment.Adjustment : 0;
                    applicationInformationFinancialAdjustment.FixedRate = ro.FixedRateAdjustment != null ? ro.FixedRateAdjustment.Rate : 0;
                    applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource = ro.FinancialAdjustmentTypeSource;
                    applicationInformationFinancialAdjustment.Term = ro.EndDate.HasValue && ro.FromDate.HasValue ?
                                                                     ro.EndDate.Value.MonthDifference(ro.FromDate.Value, 1) : 0;
                    applicationInformationFinancialAdjustment.FromDate = ro.FromDate;

                    fa.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments.Add(spc.DomainMessages, applicationInformationFinancialAdjustment);
                }
            }

            fa.Property = mla.SecuredMortgageLoan.Property;
            fa.NumApplicants = mla.Roles.Count;

            PopulateFurtherLendingBasicDetail(fa.CurrentProduct, mla);

            if (!save)
            {
                //if (ValidationHelper.PrincipalHasValidationErrors())
                //    throw new DomainValidationException();

                return fa;
            }

            SaveApplication(fa);

            Application_MortgageLoan_Role role;
            foreach (IRole r in mla.Roles)
            {
                if (r.GeneralStatus.Key == (int)GeneralStatuses.Active && r.RoleType.Key != (int)RoleTypes.AssuredLife)
                {
                    if (r.RoleType.Key == (int)RoleTypes.MainApplicant)
                        role = Application_MortgageLoan_Role.APPLICATION_MAIN_APPLICANT;
                    else
                        role = Application_MortgageLoan_Role.APPLICATION_SURETOR;

                    fa.AddApplicationRole(role, r.LegalEntity);
                }
            }

            //need to check to see if any le's are marked as income contributors,
            //if none, mark all as contributors
            bool hasIncomeContributor = false;

            // Get the latest IMortgageLoanApplication,
            // For all the corresponding LEs copy the corresponding Attributetypes
            foreach (IApplicationRole mlaApplicationRole in mla.CurrentMortgageLoanApplication.ApplicationRoles)
            {
                // Find the corresponding LE in the further loan
                foreach (IApplicationRole faApplicationRole in fa.ApplicationRoles)
                {
                    if (faApplicationRole.LegalEntity.Key == mlaApplicationRole.LegalEntity.Key)
                    {
                        // Add the Attributes
                        foreach (IApplicationRoleAttribute applicationRoleAttribute in mlaApplicationRole.ApplicationRoleAttributes)
                        {
                            IApplicationRoleAttribute newAttribute = GetEmptyApplicationRoleAttribute();
                            newAttribute.OfferRole = faApplicationRole;
                            newAttribute.OfferRoleAttributeType = applicationRoleAttribute.OfferRoleAttributeType;
                            faApplicationRole.ApplicationRoleAttributes.Add(spc.DomainMessages, newAttribute);

                            if (applicationRoleAttribute.OfferRoleAttributeType.Key == (int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor)
                                hasIncomeContributor = true;
                        }
                    }
                }
            }

            //need to check to see if any le's are marked as income contributors,
            //if none, mark all as contributors
            if (hasIncomeContributor == false)
            {
                foreach (IApplicationRole appRole in fa.ApplicationRoles)
                {
                    IApplicationRoleAttribute applicationRoleAttribute = GetEmptyApplicationRoleAttribute();
                    applicationRoleAttribute.OfferRole = appRole;
                    applicationRoleAttribute.OfferRoleAttributeType = lookupRepository.ApplicationRoleAttributesTypes.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor)];
                    appRole.ApplicationRoleAttributes.Add(spc.DomainMessages, applicationRoleAttribute);
                }
            }

            return fa;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationInformationKey"></param>
        /// <returns></returns>
        public IApplicationInformationInterestOnly GetApplicationInformationInterestOnly(int ApplicationInformationKey)
        {
            ApplicationInformationInterestOnly_DAO dao = ApplicationInformationInterestOnly_DAO.TryFind(ApplicationInformationKey);
            if (dao == null)
                return null;

            IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return bmtm.GetMappedType<IApplicationInformationInterestOnly>(dao);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationInformationKey"></param>
        /// <returns></returns>
        public IApplicationInformationSuperLoLoan GetApplicationInformationSuperLoLoan(int ApplicationInformationKey)
        {
            ApplicationInformationSuperLoLoan_DAO dao = ApplicationInformationSuperLoLoan_DAO.TryFind(ApplicationInformationKey);
            if (dao == null)
                return null;

            IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return bmtm.GetMappedType<IApplicationInformationSuperLoLoan>(dao);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationInformationKey"></param>
        /// <returns></returns>
        public IApplicationInformationEdge GetApplicationInformationEdge(int ApplicationInformationKey)
        {
            ApplicationInformationEdge_DAO dao = ApplicationInformationEdge_DAO.TryFind(ApplicationInformationKey);
            if (dao == null)
                return null;

            IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return bmtm.GetMappedType<IApplicationInformationEdge>(dao);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationInformationKey"></param>
        /// <returns></returns>
        public IApplicationInformationVarifixLoan GetApplicationInformationVarifixLoan(int ApplicationInformationKey)
        {
            ApplicationInformationVarifixLoan_DAO dao = ApplicationInformationVarifixLoan_DAO.TryFind(ApplicationInformationKey);
            if (dao == null)
                return null;

            IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return bmtm.GetMappedType<IApplicationInformationVarifixLoan>(dao);
        }

        /// <summary>
        /// Returns the variable loan information for an applicationInformation key
        /// </summary>
        /// <param name="ApplicationInformationKey"></param>
        /// <returns></returns>
        public IApplicationInformationVariableLoan GetApplicationInformationVariableLoan(int ApplicationInformationKey)
        {
            ApplicationInformationVariableLoan_DAO dao = ApplicationInformationVariableLoan_DAO.TryFind(ApplicationInformationKey);
            if (dao == null)
                return null;

            IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return bmtm.GetMappedType<IApplicationInformationVariableLoan>(dao);
        }

        /// <summary>
        /// Creates a Life Application with its Account for the specified MortgageLoan
        /// </summary>
        /// <param name="mortgageLoanAccountKey">The accountkey of the loan for which the application will be created.</param>
        /// <param name="mortgageLoanApplicationKey">The OfferKey of the loan for which the application will be created.</param>
        /// <param name="adUserName">The adUserName of the person to assign the application to.</param>
        /// <returns>The <see cref="IApplicationLife"></see>.</returns>
        public IApplicationLife CreateLifeApplication(int mortgageLoanAccountKey, int mortgageLoanApplicationKey, string adUserName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            IOrganisationStructureRepository organisationStructureRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();

            DateTime today = System.DateTime.Now;

            IApplicationLife applicationLife = null;

            // Get the Mortgage Loan
            IMortgageLoanAccount mortgageLoanAccount = accountRepo.GetAccountByKey(mortgageLoanAccountKey) as IMortgageLoanAccount;

            if (mortgageLoanAccount == null)
            {
                throw new Exception(string.Format("A mortgage loan account does not exist for loan number : {0}", mortgageLoanAccountKey));
            }

            // Get the Life Policy Account
            if (mortgageLoanAccount.LifePolicyAccount != null) // if there is already a life policy account then check to see if there is an open application
            {
                // Check to see if there is a already an offer - if there is then exit
                // Get the Life Policy Application
                IApplicationLife application = mortgageLoanAccount.LifePolicyAccount.GetLatestApplicationByType(OfferTypes.Life) as IApplicationLife;
                if (application != null && application.ApplicationStatus.Key == (int)SAHL.Common.Globals.OfferStatuses.Open)
                {
                    throw new Exception(string.Format("An Open Life Application already exists for this loan : ApplicationKey:{0}", application.Key));
                }
            }

            // Create an empty life application
            applicationLife = this.GetEmptyApplicationLife();

            // Reserve an account key
            AccountSequence_DAO accountSequence_DAO = new AccountSequence_DAO();
            accountSequence_DAO.SaveAndFlush();

            // Get the origination source product for the life policy account
            IOriginationSourceProduct osp = GetOriginationSourceProductBySourceAndProduct(mortgageLoanAccount.SecuredMortgageLoan.Account.OriginationSource.Key, (int)SAHL.Common.Globals.Products.LifePolicy);

            // Create the Application Role for the Consultant
            IApplicationRole applicationRole = GetEmptyApplicationRole();
            applicationRole.ApplicationRoleType = GetApplicationRoleTypeByKey(OfferRoleTypes.Consultant);
            applicationRole.GeneralStatus = lookupRepository.GeneralStatuses[SAHL.Common.Globals.GeneralStatuses.Active];
            applicationRole.StatusChangeDate = System.DateTime.Now;
            IADUser adUser = organisationStructureRepo.GetAdUserForAdUserName(adUserName);
            applicationRole.LegalEntity = adUser.LegalEntity;

            // Populate the Life Application
            applicationLife.ApplicationStatus = lookupRepository.ApplicationStatuses.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.OfferStatuses.Open)];
            applicationLife.ApplicationStartDate = System.DateTime.Now;
            applicationLife.OriginationSource = mortgageLoanAccount.SecuredMortgageLoan.Account.OriginationSource;

            applicationLife.DeathBenefit = 0;
            applicationLife.InstallmentProtectionBenefit = 0;
            applicationLife.DeathBenefitPremium = 0;
            applicationLife.InstallmentProtectionPremium = 0;
            applicationLife.DateOfExpiry = mortgageLoanAccount.SecuredMortgageLoan.OpenDate.HasValue ?
                                           mortgageLoanAccount.SecuredMortgageLoan.OpenDate.Value.AddMonths(mortgageLoanAccount.SecuredMortgageLoan.InitialInstallments) :
                                           DateTime.Now.AddMonths(mortgageLoanAccount.SecuredMortgageLoan.InitialInstallments);
            applicationLife.UpliftFactor = 0;
            applicationLife.JointDiscountFactor = 0;
            applicationLife.MonthlyPremium = 0;
            applicationLife.YearlyPremium = 0;

            applicationLife.SumAssured = mortgageLoanAccount.LoanCurrentBalance;

            applicationLife.DateLastUpdated = today;
            applicationLife.Insurer = lookupRepository.Insurers.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.Insurers.SAHLLife)];

            foreach (IPriority pr in osp.Priorities)
            {
                if (pr.Description == "Quote Required")
                {
                    applicationLife.Priority = pr;
                    break;
                }
            }

            // set the consultant on the offerlife
            applicationLife.ConsultantADUserName = adUser.ADUserName;

            // set the applications reserved account key
            applicationLife.ReservedAccount = new AccountSequence(accountSequence_DAO);

            // set the life policy type
            applicationLife.LifePolicyType = lookupRepository.LifePolicyTypes.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.LifePolicyTypes.StandardCover)];

            // Create a new account using the reserved account key from above
            //Add the required parameters
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@ReservedAccountKey", accountSequence_DAO.Key));
            parameters.Add(new SqlParameter("@OriginationSourceProductKey", osp.Key));
            parameters.Add(new SqlParameter("@SPVKey", mortgageLoanAccount.SPV.Key));
            parameters.Add(new SqlParameter("@UserID", "System"));
            parameters.Add(new SqlParameter("@ParentAccountKey", mortgageLoanAccount.Key));

            this.castleTransactionsService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Account", "CreateAccount", parameters);

            // get the newly created account from above
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            IAccountLifePolicy accountLifePolicy = accRepo.GetAccountByKey(accountSequence_DAO.Key) as IAccountLifePolicy;

            // Link the Application to the Application Role
            applicationRole.Application = applicationLife;

            // Link the Application to the Account
            applicationLife.Account = accountLifePolicy;
            accountLifePolicy.Applications.Add(spc.DomainMessages, applicationLife);

            // Exclude the Rules which check the roles because we dont create roles upfront on a new life policy
            spc.ExclusionSets.Add(RuleExclusionSets.LifeApplicationCreate);
            spc.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);

            // Save the Life Account
            accountRepo.CreateAccount(accountLifePolicy);

            // Save the Life Application
            this.SaveApplication(applicationLife);

            // add the consultant role - this must be added after the application is saved and has been assigned a key
            applicationLife.AddRole((int)SAHL.Common.Globals.OfferRoleTypes.Consultant, adUser.LegalEntity);

            // Save the Life Application again - must be saved again in order to save the role created above
            this.SaveApplication(applicationLife);

            // Add the OfferAccountRelationship link between the loan application and the LifeAccount
            IApplication mortgageLoanApplication = this.GetApplicationByKey(mortgageLoanApplicationKey);
            if (mortgageLoanApplication != null)
            {
                mortgageLoanApplication.RelatedAccounts.Add(spc.DomainMessages, accountLifePolicy);
                this.SaveApplication(mortgageLoanApplication);
            }

            // remove the rule exclusion set
            spc.ExclusionSets.Remove(RuleExclusionSets.LifeApplicationCreate);
            spc.ExclusionSets.Remove(RuleExclusionSets.LegalEntityLeadApplicants);

            // populate the LifeOfferAssignment record
            ILifeOfferAssignment lifeOfferAssignment = lifeRepo.CreateEmptyLifeOfferAssignment();
            lifeOfferAssignment.OfferKey = applicationLife.Key;
            lifeOfferAssignment.LoanAccountKey = mortgageLoanAccountKey;
            lifeOfferAssignment.LoanOfferKey = mortgageLoanApplicationKey;
            lifeOfferAssignment.LoanOfferTypeKey = mortgageLoanApplication.ApplicationType.Key;
            lifeOfferAssignment.DateAssigned = DateTime.Now;
            lifeOfferAssignment.ADUserName = adUserName;

            // save the LifeOfferAssignment record
            lifeRepo.SaveLifeOfferAssignment(lifeOfferAssignment);

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

            return applicationLife;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        public IEventList<IApplicationInformation> GetApplicationRevisionHistory(int ApplicationKey)
        {
            IApplication app = GetApplicationByKey(ApplicationKey);
            IEventList<IApplicationInformation> revisions = app.ApplicationInformations;
            return revisions;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="calculationDone"></param>
        /// <param name="ltv"></param>
        /// <param name="pti"></param>
        /// <param name="income"></param>
        /// <param name="loanAmount"></param>
        /// <param name="valuationAmount"></param>
        /// <param name="employmentTypeKey"></param>
        /// <param name="isFurtherLending"></param>
        /// <param name="term"></param>
        /// <param name="readvanceOnly"></param>
        public void CreditDisqualifications(bool calculationDone, double ltv, double pti, double income, double loanAmount, double valuationAmount, int employmentTypeKey, bool isFurtherLending, int term, bool readvanceOnly)
        {
            IRuleService ruleServ = ServiceFactory.GetService<IRuleService>();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            if (calculationDone)
            {
                //GaryD: readvances need to processed outside of the current business lending criteria
                //this should be removed as soon as possible
                if (!readvanceOnly)
                    ruleServ.ExecuteRule(spc.DomainMessages, "CalcCreditDisqualificationMaxLTV", ltv);

                ruleServ.ExecuteRule(spc.DomainMessages, "CalcCreditDisqualificationMaxPTI", pti);
            }

            ruleServ.ExecuteRule(spc.DomainMessages, "CalcCreditDisqualificationMinIncome", income);

            if (!isFurtherLending)
            {
                ruleServ.ExecuteRule(spc.DomainMessages, "CalcCreditDisqualificationMinLAA", loanAmount);
            }
            else
            {
                ruleServ.ExecuteRule(spc.DomainMessages, "CalcCreditDisqualificationMinLAFL", loanAmount);
            }

            ruleServ.ExecuteRule(spc.DomainMessages, "CalcCreditDisqualificationMaxLAA", loanAmount);

            ruleServ.ExecuteRule(spc.DomainMessages, "CalcCreditDisqualificationMinValuation", valuationAmount);

            if (employmentTypeKey == (int)SAHL.Common.Globals.EmploymentTypes.Unemployed)
            {
                AddDomainErrorMessage(new Error("Employment type can not be unemployed.", "Employment type can not be unemployed."));
            }
        }

        /// <summary>
        /// Implements <see cref="IApplicationRepository.SearchApplications"/>
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="maxRowCount"></param>
        /// <param name="mustExistInWorkflow"></param>
        /// <returns></returns>
        public IEventList<IApplication> SearchApplications(IApplicationSearchCriteria searchCriteria, int maxRowCount, bool mustExistInWorkflow)
        {
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

            #region build sql query

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT TOP ");
            sbQuery.Append(maxRowCount);
            sbQuery.Append(" o.OfferKey FROM [2am].dbo.Offer o (nolock)");

            // build up where clause
            sbQuery.Append(" WHERE o.ReservedAccountKey > 0 ");

            // application type : if no application types have been specified then it will return all
            if (searchCriteria.ApplicationTypes.Count > 0)
            {
                sbQuery.Append(" AND o.OfferTypeKey in (");
                for (int i = 0; i < searchCriteria.ApplicationTypes.Count; i++)
                {
                    if (i > 0)
                        sbQuery.Append(",");

                    sbQuery.Append((int)searchCriteria.ApplicationTypes[i]);
                }
                sbQuery.Append(")");
            }

            // application status : if no application statuses have been specified then it will return all
            if (searchCriteria.ApplicationStatuses.Count > 0)
            {
                sbQuery.Append(" AND o.OfferStatusKey in (");
                for (int i = 0; i < searchCriteria.ApplicationStatuses.Count; i++)
                {
                    if (i > 0)
                        sbQuery.Append(",");
                    sbQuery.Append((int)searchCriteria.ApplicationStatuses[i]);
                }
                sbQuery.Append(")");
            }

            // account
            if (searchCriteria.AccountKey.HasValue && searchCriteria.AccountKey.Value > 0)
            {
                sbQuery.Append(" AND o.ReservedAccountKey = ");
                sbQuery.Append(searchCriteria.AccountKey);
            }

            // join to workflow
            if (mustExistInWorkflow)
            {
                sbQuery.Append(" AND (EXISTS");

                string processName = "", workflowName = "";
                int applicationTypeCount = 0;
                foreach (KeyValuePair<string, string> nv in searchCriteria.WorkflowsAndProcesses)
                {
                    applicationTypeCount += 1;

                    workflowName = nv.Key;
                    processName = nv.Value.ToString();

                    IWorkFlow workflow = x2Repo.GetWorkFlowByName(workflowName, processName);

                    if (applicationTypeCount > 1)
                        sbQuery.Append(" OR EXISTS ");

                    sbQuery.Append(" ( SELECT 1 FROM [x2].[X2DATA].");
                    sbQuery.Append(workflow.StorageTable);
                    sbQuery.Append(" xd  (nolock) JOIN [x2].[x2].WorkList wl  (nolock) ON wl.instanceid = xd.instanceid");

                    if (!String.IsNullOrEmpty(searchCriteria.ClientName))
                        sbQuery.Append(" JOIN [x2].[x2].Instance i (nolock) ON i.id = wl.instanceid ");

                    sbQuery.Append(" WHERE xd.");
                    sbQuery.Append(workflow.StorageKey);
                    sbQuery.Append(" = o.OfferKey ");

                    // consultant
                    if (!String.IsNullOrEmpty(searchCriteria.ConsultantADUserName) && searchCriteria.ConsultantADUserName.ToLower() != "all")
                    {
                        sbQuery.Append(" AND wl.ADuserName = '");
                        sbQuery.Append(searchCriteria.ConsultantADUserName);
                        sbQuery.Append("'");
                    }

                    // client name
                    if (!String.IsNullOrEmpty(searchCriteria.ClientName))
                    {
                        string fullname = searchCriteria.ClientName.Replace(" ", "%");
                        sbQuery.Append(" AND i.Subject like '%");
                        sbQuery.Append(fullname);
                        sbQuery.Append("%'");
                    }
                    sbQuery.Append(" )");
                }
                sbQuery.Append(" )");
            }

            #endregion build sql query

            // for performance reasons, don't run ISession.CreateSQLQuery - this ends up running a query
            // for all the records AND a query per object - rather get the list of keys using standard
            // sql and then use NHibernate criterion to pull back all the objects in one go
            IDbConnection conn = Helper.GetSQLDBConnection();
            IDataReader reader = null;
            ArrayList appKeys = new ArrayList();
            try
            {
                conn.Open();
                reader = Helper.ExecuteReader(conn, sbQuery.ToString());
                while (reader.Read())
                {
                    appKeys.Add(reader.GetInt32(0));
                }
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            // if there are no results, exit now
            if (appKeys.Count == 0)
                return new DAOEventList<Application_DAO, IApplication, Application>(new List<Application_DAO>());

            // we've got the keys, now we can grab the applications
            ICriterion[] criteria = new ICriterion[]
                {
                    Expression.In("Key", appKeys)
                };
            Application_DAO[] applications = Application_DAO.FindAll(criteria);
            return new DAOEventList<Application_DAO, IApplication, Application>(applications);
        }

        public IEventList<IApplicationDebitOrder> GetApplicationDebitOrdersByApplicationKey(int ApplicationKey)
        {
            string query = string.Format("SELECT odb FROM ApplicationDebitOrder_DAO odb WHERE odb.Application.Key = {0}", ApplicationKey);

            SimpleQuery q = new SimpleQuery(typeof(ApplicationDebitOrder_DAO), query);
            ApplicationDebitOrder_DAO[] result = ApplicationDebitOrder_DAO.ExecuteQuery(q) as ApplicationDebitOrder_DAO[];

            if (result == null)
                result = new ApplicationDebitOrder_DAO[0];

            IList<ApplicationDebitOrder_DAO> list = new List<ApplicationDebitOrder_DAO>(result);
            return new DAOEventList<ApplicationDebitOrder_DAO, IApplicationDebitOrder, ApplicationDebitOrder>(list);
        }

        /// <summary>
        /// Get an empty Application Attribute
        /// </summary>
        /// <returns></returns>
        public IApplicationAttribute GetEmptyApplicationAttribute()
        {
            return base.CreateEmpty<IApplicationAttribute, ApplicationAttribute_DAO>();

            //return new ApplicationAttribute(new ApplicationAttribute_DAO());
        }

        /// <summary>
        /// Get an empty Application Role Attribute
        /// </summary>
        /// <returns></returns>
        public IApplicationRoleAttribute GetEmptyApplicationRoleAttribute()
        {
            return base.CreateEmpty<IApplicationRoleAttribute, ApplicationRoleAttribute_DAO>();

            //return new ApplicationRoleAttribute(new ApplicationRoleAttribute_DAO());
        }

        /// <summary>
        /// Gets a new account sequence object.
        /// </summary>
        /// <param name="reserve">If true, the account sequence will be saved and therefore reserved.</param>
        /// <returns></returns>
        public IAccountSequence GetEmptyAccountSequence(bool reserve)
        {
            AccountSequence_DAO accountSequence_DAO = new AccountSequence_DAO();

            if (reserve)
                accountSequence_DAO.SaveAndFlush();

            return new AccountSequence(accountSequence_DAO);
        }

        public IList<IApplicationDeclarationQuestionAnswerConfiguration> GetApplicationDeclarationsByLETypeAndApplicationRoleAndOSP(int LegalEntityTypeKey, int GenericKey, int GenericKeyTypeKey, int OSPKey)
        {
            string HQL = "from ApplicationDeclarationQuestionAnswerConfiguration_DAO a where a.LegalEntityType.Key = ? and a.GenericKey = ? and a.GenericKeyType.Key = ? and a.OriginationSourceProduct.Key = ?";
            SimpleQuery<ApplicationDeclarationQuestionAnswerConfiguration_DAO> q = new SimpleQuery<ApplicationDeclarationQuestionAnswerConfiguration_DAO>(HQL, LegalEntityTypeKey, GenericKey, GenericKeyTypeKey, OSPKey);
            ApplicationDeclarationQuestionAnswerConfiguration_DAO[] res = q.Execute();

            IList<IApplicationDeclarationQuestionAnswerConfiguration> retval = new List<IApplicationDeclarationQuestionAnswerConfiguration>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new ApplicationDeclarationQuestionAnswerConfiguration(res[i]));
            }
            return retval;
        }

        /// <summary>
        /// Get the application declaration answer to a declaration question.
        /// Returns the answer key, 0 by default.
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="applicationKey"></param>
        /// <param name="appDeclarationQuestionKey"></param>
        /// <returns>ApplicationDeclarationAnswer.Key</returns>
        public int GetApplicationDeclarationAnswerToQuestion(int legalEntityKey, int applicationKey, int appDeclarationQuestionKey)
        {
            IApplication app = GetApplicationByKey(applicationKey);
            int retval = 0;

            if (app is IApplicationUnsecuredLending)
            {
                // Using an HQL query to minimise business\DAO object loading
                string hql = @"select erd.ApplicationDeclarationAnswer
                            from ExternalRole_DAO er
                            inner join er.ExternalRoleDeclarations erd
                            where er.LegalEntity.Key = ?
                            and er.GenericKey = ?
                            and er.GenericKeyType.Key = ?
                            and er.ExternalRoleType.Key = ?
                            and er.GeneralStatus.Key = ?
                            and erd.ApplicationDeclarationQuestion.Key = ?
                            order by erd.ApplicationDeclarationAnswer.Key Desc";

                SimpleQuery<ApplicationDeclarationAnswer_DAO> query = new SimpleQuery<ApplicationDeclarationAnswer_DAO>(hql,
                                                                                legalEntityKey,
                                                                                applicationKey,
                                                                                (int)GenericKeyTypes.Offer,
                                                                                (int)ExternalRoleTypes.Client,
                                                                                (int)GeneralStatuses.Active,
                                                                                appDeclarationQuestionKey);

                query.SetQueryRange(1); // Limiting the results to 1 and returning the latest answer (max key)
                ApplicationDeclarationAnswer_DAO[] applicationDeclarationAnswer = query.Execute();

                if (applicationDeclarationAnswer != null && applicationDeclarationAnswer.Length > 0)
                {
                    retval = (int)applicationDeclarationAnswer[0].Key;
                }
            }
            else
            {
                IApplicationRole appRole = null;

                foreach (IApplicationRole ar in app.ApplicationRoles)
                {
                    if (ar.LegalEntity.Key == legalEntityKey &&
                        (ar.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                        || ar.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || ar.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor
                        || ar.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor))
                    {
                        appRole = ar;
                        break;
                    }
                }

                if (appRole != null)
                {
                    foreach (IApplicationDeclaration appDeclaration in appRole.ApplicationDeclarations)
                    {
                        if (appDeclaration.ApplicationDeclarationQuestion.Key == appDeclarationQuestionKey)
                        {
                            retval = appDeclaration.ApplicationDeclarationAnswer.Key;
                        }
                    }
                }
            }

            return retval;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IApplicationDeclaration GetEmptyApplicationDeclaration()
        {
            return base.CreateEmpty<IApplicationDeclaration, ApplicationDeclaration_DAO>();

            //return new ApplicationDeclaration(new ApplicationDeclaration_DAO());
        }

        public void SaveApplicationDeclaration(IApplicationDeclaration appDeclaration)
        {
            base.Save<IApplicationDeclaration, ApplicationDeclaration_DAO>(appDeclaration);

            //IDAOObject dao = appDeclaration as IDAOObject;
            //ApplicationDeclaration_DAO cb = (ApplicationDeclaration_DAO)dao.GetDAOObject();
            //cb.SaveAndFlush();
            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        public IExternalRoleDeclaration GetEmptyExternalRoleDeclaration()
        {
            return base.CreateEmpty<IExternalRoleDeclaration, ExternalRoleDeclaration_DAO>();
        }

        public void SaveExternalRoleDeclaration(IExternalRoleDeclaration externalRoleDeclaration)
        {
            base.Save<IExternalRoleDeclaration, ExternalRoleDeclaration_DAO>(externalRoleDeclaration);
        }

        public IList<IApplicationDeclaration> GetApplicationDeclarationsByapplicationRoleKey(int appRoleKey)
        {
            string HQL = "from ApplicationDeclaration_DAO a where a.ApplicationRole.Key = ?";
            SimpleQuery<ApplicationDeclaration_DAO> q = new SimpleQuery<ApplicationDeclaration_DAO>(HQL, appRoleKey);
            ApplicationDeclaration_DAO[] res = q.Execute();

            IList<IApplicationDeclaration> retval = new List<IApplicationDeclaration>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new ApplicationDeclaration(res[i]));
            }
            return retval;
        }

        public IApplication GetApplicationFromInstance(IInstance instance)
        {
            IApplication application = null;

            string sql = "SELECT o.* FROM [x2].[X2DATA]." + instance.WorkFlow.StorageTable + " xd ";
            sql += " JOIN [2am].[dbo].Offer o ON o.OfferKey = xd." + instance.WorkFlow.StorageKey;
            sql += " WHERE xd.InstanceID = " + instance.ID;

            ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = sessionHolder.CreateSession(typeof(Application_DAO));
            IQuery sqlQuery = session.CreateSQLQuery(sql).AddEntity(typeof(Application_DAO));
            sqlQuery.SetMaxResults(1);

            IList<Application_DAO> applications = sqlQuery.List<Application_DAO>();

            if (applications != null && applications.Count > 0)
            {
                IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                application = bmtm.GetMappedType<IApplication>(applications[0]);
            }
            return application;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IApplication GetApplicationByInstanceAndAddCriteria(IInstance instance, Hashtable criteria)
        {
            IApplication application = null;
            int AppRoleTypeKey = Convert.ToInt32(criteria["ApplicationRoleTypeKey"]);
            string adusername = criteria["ADUserName"].ToString();

            string sql = "SELECT o.* FROM [x2].[X2DATA]." + instance.WorkFlow.StorageTable + " xd (nolock) ";
            sql += "  JOIN [2am].[dbo].Offer o (nolock) ON o.OfferKey = xd." + instance.WorkFlow.StorageKey;
            sql += "  JOIN [2am].[dbo].OfferRole ofr (nolock) ON o.OfferKey = ofr.OfferKey";
            sql += "  JOIN [2am].[dbo].OfferRoleType ort (nolock) ON ort.OfferRoleTypeKey = ofr.OfferRoleTypeKey";
            sql += "  JOIN [2am].[dbo].ADUser ad (nolock) ON ad.legalEntityKey = ofr.legalEntityKey";
            if (criteria.ContainsKey("OrganisationStructureKey"))
            {
                if (Convert.ToInt32(criteria["OrganisationStructureKey"]) > 0)
                {
                    // Only returns OfferRole created during the period the person served in the DELETED from area
                    sql += @" JOIN [2am].[dbo].vUserOrganisationStructureHistory vsh
                          on vsh.ADUserKey = ad.ADUserKey and vsh.EndDate is not null and (ofr.StatusChangeDate >= vsh.StartDate and ofr.StatusChangeDate < vsh.EndDate)";
                }
                else
                {
                    // Only returns OfferRole created during the period the person served in the INSERTED into area
                    sql += @" JOIN [2am].[dbo].vUserOrganisationStructureHistory vsh
                          on vsh.ADUserKey = ad.ADUserKey and vsh.EndDate is null and (ofr.StatusChangeDate >= vsh.StartDate and ofr.StatusChangeDate < getdate())";
                }
            }
            sql += "  JOIN X2.X2.INSTANCE INST (nolock) ON INST.ID = xd.InstanceID";
            sql += "  JOIN X2.X2.STATE ST (nolock) ON INST.STATEID = ST.ID";
            sql += "  JOIN X2.X2.STATEWORKLIST STWL (nolock) ON ST.ID = STWL.STATEID";
            sql += "  JOIN X2.X2.SECURITYGROUP SG (nolock) ON STWL.SECURITYGROUPID = SG.ID AND SG.name = ort.description";
            sql += "  WHERE xd.InstanceID = " + instance.ID;
            sql += "  AND ofr.OfferRoleTypeKey = " + AppRoleTypeKey;
            sql += "  AND ofr.GeneralStatusKey = 1";
            sql += "  AND ad.adUserName = '" + adusername.Trim() + "'";
            if (criteria.ContainsKey("OrganisationStructureKey"))
            {
                sql += "AND vsh.OrganisationStructureKey = " + Math.Abs(Convert.ToInt32(criteria["OrganisationStructureKey"]));
            }

            ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = sessionHolder.CreateSession(typeof(Application_DAO));
            IQuery sqlQuery = session.CreateSQLQuery(sql).AddEntity(typeof(Application_DAO));
            sqlQuery.SetMaxResults(1);

            IList<Application_DAO> applications = sqlQuery.List<Application_DAO>();

            if (applications != null && applications.Count > 0)
            {
                IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                application = bmtm.GetMappedType<IApplication>(applications[0]);
            }
            return application;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IApplicationDebitOrder GetEmptyApplicationDebitOrder()
        {
            return base.CreateEmpty<IApplicationDebitOrder, ApplicationDebitOrder_DAO>();

            //return new ApplicationDebitOrder(new ApplicationDebitOrder_DAO());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IApplicationInternetReferrer GetEmptyApplicationInternetReferrer()
        {
            return base.CreateEmpty<IApplicationInternetReferrer, ApplicationInternetReferrer_DAO>();

            //return new ApplicationDebitOrder(new ApplicationDebitOrder_DAO());
        }

        public void SaveApplicationExpense(IApplicationExpense appExpense)
        {
            base.Save<IApplicationExpense, ApplicationExpense_DAO>(appExpense);

            //IDAOObject dao = appExpense as IDAOObject;
            //ApplicationExpense_DAO cb = (ApplicationExpense_DAO)dao.GetDAOObject();
            //cb.SaveAndFlush();
            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        /// <summary>
        /// Save the application internet referrer record
        /// </summary>
        /// <param name="InternetReferrer"></param>
        public void SaveApplicationInternetReferrer(IApplicationInternetReferrer InternetReferrer)
        {
            base.Save<IApplicationInternetReferrer, ApplicationInternetReferrer_DAO>(InternetReferrer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="appDebtSettlement"></param>
        public void SaveApplicationDebtSettlement(IApplicationDebtSettlement appDebtSettlement)
        {
            base.Save<IApplicationDebtSettlement, ApplicationDebtSettlement_DAO>(appDebtSettlement);

            //IDAOObject dao = appDebtSettlement as IDAOObject;
            //ApplicationDebtSettlement_DAO cb = (ApplicationDebtSettlement_DAO)dao.GetDAOObject();
            //cb.SaveAndFlush();
            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        public IDictionary<string, string> GetApplicantRoleTypesForApplication(IApplication application)
        {
            string roleTypeKey;
            IDictionary<string, string> RoleTypes = new Dictionary<string, string>();

            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

            int compositeCount = stageDefinitionRepository.CountCompositeStageOccurance(application.Key, stageDefinitionRepository.GetStageDefinitionStageDefinitionGroup((int)StageDefinitionGroups.ApplicationCapture, (int)StageDefinitions.ApplicationCaptureSubmitted).Key);

            if (compositeCount > 0 || application is IApplicationFurtherLoan || application is IApplicationFurtherAdvance || application is IApplicationReAdvance)
            {
                roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.MainApplicant);
                RoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, lookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.MainApplicant]));

                roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.Suretor);
                RoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, lookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.Suretor]));
            }
            else
            {
                roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant);
                RoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, lookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.LeadMainApplicant]));

                roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.LeadSuretor);
                RoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, lookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.LeadSuretor]));
            }

            return RoleTypes;
        }

        #endregion IApplicationRepository Members

        /// <summary>
        /// Gets all open applications that a property is loaded against.
        /// </summary>
        /// <param name="PropertyKey"></param>
        /// <returns></returns>
        public IEventList<IApplication> GetOpenApplicationsForPropertyKey(int PropertyKey)
        {
            string HQL = "select a from ApplicationMortgageLoanDetail_DAO d  join d.Application a where d.Property.Key=? and d.Application.ApplicationStatus.Key=1";
            SimpleQuery<Application_DAO> q = new SimpleQuery<Application_DAO>(HQL, PropertyKey);
            Application_DAO[] arr = q.Execute();
            IEventList<IApplication> list = new DAOEventList<Application_DAO, IApplication, Application>(arr);
            return list;
        }

        public IApplicationExpense GetApplicationExpenseByBankAccountNameAndBankAccountNumber(string accountName, string accountNumber)
        {
            string HQL = "select ae from ApplicationExpense_DAO ae where ae.ExpenseAccountName = ? and ae.ExpenseAccountNumber = ?";

            SimpleQuery<ApplicationExpense_DAO> q = new SimpleQuery<ApplicationExpense_DAO>(HQL, accountName, accountNumber);
            q.SetQueryRange(1); //although there should never be more than 1 anyway...
            ApplicationExpense_DAO[] res = q.Execute();

            if (res != null && res.Length > 0)
                return new ApplicationExpense(res[0]);

            return null;
        }

        public IApplicationRole GetApplicationRoleByKey(int Key)
        {
            IApplicationRole appRole = base.GetByKey<IApplicationRole, ApplicationRole_DAO>(Key);

            if (appRole == null)
                return base.CreateEmpty<IApplicationRole, ApplicationRole_DAO>();

            return appRole;

            //    ApplicationRole_DAO appRole = ApplicationRole_DAO.Find(applicationRoleKey);

            //    if (appRole != null)
            //        return new ApplicationRole(appRole);

            //    return null;
        }

        /// <summary>
        /// Gets an <see cref="IApplicationRoleType"/> according to the key.
        /// </summary>
        /// <param name="roleType"></param>
        /// <returns></returns>
        public IApplicationRoleType GetApplicationRoleTypeByKey(OfferRoleTypes roleType)
        {
            return GetApplicationRoleTypeByKey((int)roleType);
        }

        /// <summary>
        /// Gets an <see cref="IApplicationRoleType"/> according to the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IApplicationRoleType GetApplicationRoleTypeByKey(int key)
        {
            return base.GetByKey<IApplicationRoleType, ApplicationRoleType_DAO>(key);
        }

        public IEventList<IApplicationRole> GetApplicationRoleTypesForKeys(List<int> Keys, int ApplicationKey)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Keys.Count; i++)
            {
                sb.AppendFormat(",{0}", Keys[i]);
            }
            sb.Remove(0, 1);

            // order by the offerroletypekey they are ordered from senior to least senior
            string HQL = string.Format("from ApplicationRole_DAO d where d.ApplicationKey=? and d.ApplicationRoleType.Key in ({0}) order by d.ApplicationRoleType.Key", sb.ToString());
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey);
            ApplicationRole_DAO[] arr = q.Execute();
            return new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(arr);
        }

        /// <summary>
        /// Gets an <see cref="IApplicationRoleAttributeType"/> according to the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IApplicationRoleAttributeType GetApplicationRoleAttributeTypeByKey(int key)
        {
            return base.GetByKey<IApplicationRoleAttributeType, ApplicationRoleAttributeType_DAO>(key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IOriginationSourceProduct GetOriginationSourceProductByKey(int Key)
        {
            return base.GetByKey<IOriginationSourceProduct, OriginationSourceProduct_DAO>(Key);

            //return new OriginationSourceProduct(OriginationSourceProduct_DAO.Find(OSPKey));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IOriginationSource GetOriginationSource(OriginationSources source)
        {
            OriginationSource_DAO dao = OriginationSource_DAO.Find(Convert.ToInt32(source));
            return new OriginationSource(dao);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="OriginationSourceKey"></param>
        /// <param name="ProductKey"></param>
        /// <returns></returns>
        public IOriginationSourceProduct GetOriginationSourceProductBySourceAndProduct(int OriginationSourceKey, int ProductKey)
        {
            string HQL = "from OriginationSourceProduct_DAO osp where osp.OriginationSource.Key = ? and osp.Product.Key = ?";
            SimpleQuery query = new SimpleQuery(typeof(OriginationSourceProduct_DAO), HQL, OriginationSourceKey, ProductKey);

            object o = OriginationSourceProduct_DAO.ExecuteQuery(query);
            OriginationSourceProduct_DAO[] originationSourceProducts = o as OriginationSourceProduct_DAO[];
            if (originationSourceProducts != null && originationSourceProducts.Length >= 1)
            {
                return new OriginationSourceProduct(originationSourceProducts[0]);
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ReadOnlyEventList<IProduct> GetOriginationProducts()
        {
            Product_DAO[] res = Product_DAO.FindAllByProperty("Originate", "Y");
            IEventList<IProduct> list = new DAOEventList<Product_DAO, IProduct, Product>(res);
            return new ReadOnlyEventList<IProduct>(list);
        }

        /// <summary>
        /// Return a list of Origination Products, including the ProductKey passed in
        /// An application can be originated for a product, and in the process of origination
        /// the product could be discontinued, but existing applications of this type would need to
        /// continue through the origination process
        /// </summary>
        /// <returns></returns>
        /// <param name="ProductKey"></param>
        public ReadOnlyEventList<IProduct> GetOriginationProducts(int ProductKey)
        {
            string HQL = String.Format("from Product_DAO p where p.Key in ({0}) or p.Originate = 'Y'", ProductKey);

            SimpleQuery<Product_DAO> q = new SimpleQuery<Product_DAO>(HQL);
            Product_DAO[] res = q.Execute();

            IEventList<IProduct> list = new DAOEventList<Product_DAO, IProduct, Product>(res);
            return new ReadOnlyEventList<IProduct>(list);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="MortgageLoanPurposeKeys"></param>
        /// <returns></returns>
        public ReadOnlyEventList<IMortgageLoanPurpose> GetMortgageLoanPurposes(int[] MortgageLoanPurposeKeys)
        {
            string keys = "";

            for (int i = 0; i < MortgageLoanPurposeKeys.Length; i++)
            {
                keys += "," + MortgageLoanPurposeKeys[i].ToString();
            }

            if (keys.StartsWith(","))
                keys = keys.Remove(0, 1);

            string HQL = String.Format("from MortgageLoanPurpose_DAO mlp where mlp.Key in ({0})", keys);

            SimpleQuery<MortgageLoanPurpose_DAO> q = new SimpleQuery<MortgageLoanPurpose_DAO>(HQL);
            MortgageLoanPurpose_DAO[] res = q.Execute();

            IEventList<IMortgageLoanPurpose> list = new DAOEventList<MortgageLoanPurpose_DAO, IMortgageLoanPurpose, MortgageLoanPurpose>(res);
            return new ReadOnlyEventList<IMortgageLoanPurpose>(list);
        }

        public IApplicationRole GetApplicationRoleForTypeAndKey(int ApplicationKey, int ApplicationRoleTypeKey)
        {
            string HQL = "from ApplicationRole_DAO d where d.ApplicationKey=? and d.ApplicationRoleType.Key=? order by d.Key desc";
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey, ApplicationRoleTypeKey);
            ApplicationRole_DAO[] arr = q.Execute();
            return new ApplicationRole(arr[0]);

            //return new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(arr);
        }

        public IApplicationRole GetActiveApplicationRoleForTypeAndKey(int ApplicationKey, int ApplicationRoleTypeKey)
        {
            string HQL = "from ApplicationRole_DAO d where d.ApplicationKey=? and d.ApplicationRoleType.Key=? and d.GeneralStatus.Key = 1 order by d.Key desc";
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey, ApplicationRoleTypeKey);
            ApplicationRole_DAO[] arr = q.Execute();

            if (arr != null & arr.Length > 0)
                return new ApplicationRole(arr[0]);
            else
                return null;
        }

        public IEventList<IApplicationRole> GetApplicationRolesForKey(int ApplicationKey)
        {
            string HQL = "from ApplicationRole_DAO d where d.ApplicationKey=? and d.ApplicationRoleType.ApplicationRoleTypeGroup.Key = 1 order by d.Key desc";
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey);
            ApplicationRole_DAO[] arr = q.Execute();
            return new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(arr);
        }

        public string GetFurtherLendingX2Message(int applicationKey)
        {
            //UIStatement FurtherLendingGetX2Message
            string query = UIStatementRepository.GetStatement("COMMON", "FurtherLendingGetX2Message");
            ParameterCollection prms = new ParameterCollection();
            Helper.AddVarcharParameter(prms, "@ApplicationKey", applicationKey.ToString());

            IDbConnection con = Helper.GetSQLDBConnection();

            object o = Helper.ExecuteScalar(con, query, prms);
            if (o != null)
                return (string)o;

            return String.Format("No X2 instance found. Data is invalid for Application Number: {0}", applicationKey);
        }

        public string GetFurtherLendingX2NTU(int accountKey)
        {
            //UIStatement FurtherLendingGetX2NTU
            string query = UIStatementRepository.GetStatement("COMMON", "FurtherLendingGetX2NTU");
            ParameterCollection prms = new ParameterCollection();
            Helper.AddVarcharParameter(prms, "@AccountKey", accountKey.ToString());
            IDbConnection con = Helper.GetSQLDBConnection();

            object o = Helper.ExecuteScalar(con, query, prms);
            if (o != null)
                return (string)o;

            return "";
        }

        public IList<IApplicationAttributeType> GetApplicationAttributeTypeByIsGeneric(bool isGeneric)
        {
            string HQL = "from ApplicationAttributeType_DAO a where a.IsGeneric = ?";
            SimpleQuery<ApplicationAttributeType_DAO> q = new SimpleQuery<ApplicationAttributeType_DAO>(HQL, isGeneric);
            ApplicationAttributeType_DAO[] res = q.Execute();

            IList<IApplicationAttributeType> retval = new List<IApplicationAttributeType>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new ApplicationAttributeType(res[i]));
            }
            return retval;
        }

        public IOriginationSourceProduct GetOriginationSourceProduct(IApplication Application)
        {
            string HQL = "select osp from OriginationSourceProduct_DAO osp where osp.OriginationSource.Key = ? and osp.Product.Key = ?";
            SimpleQuery<OriginationSourceProduct_DAO> q = new SimpleQuery<OriginationSourceProduct_DAO>(HQL, Application.OriginationSource.Key, Convert.ToInt32(Application.CurrentProduct.ProductType));
            OriginationSourceProduct_DAO[] res = q.Execute();
            return (new OriginationSourceProduct(res[0]));
        }

        public IApplication GetLastestAcceptedApplication(IAccount account)
        {
            int appKey = -1;
            string sql = UIStatementRepository.GetStatement("Application", "GetLastestAcceptedApplicationGivenAccountkey");
            ParameterCollection prms = new ParameterCollection();
            Helper.AddIntParameter(prms, "@AccountKey", account.Key);
            DataSet ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), prms);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                appKey = Convert.ToInt32(dr[0]);
            }

            IApplicationRepository AppRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplication app = AppRepo.GetApplicationByKey(appKey);
            return app;
        }

        /// <summary>
        /// Gets audit information pertaining to an application.
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        public IEventList<IAudit> GetApplicationAuditData(int applicationKey)
        {
            string sql = UIStatementRepository.GetStatement("COMMON", "GetApplicationAuditInfo");

            SimpleQuery<Audit_DAO> q = new SimpleQuery<Audit_DAO>(QueryLanguage.Sql, sql);
            q.AddSqlReturnDefinition(typeof(Audit_DAO), "a");
            q.SetParameter("OfferKey", applicationKey);
            Audit_DAO[] arr = q.Execute();
            return new DAOEventList<Audit_DAO, IAudit, Audit>(arr);
        }

        /// <summary>
        /// Return the last credit decision and the user who completed the action
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="CurrentCreditDecision"></param>
        /// <param name="adUserKey"></param>
        /// <param name="decisionDate"></param>
        public void GetCurrentCreditDecision(int appKey, out string CurrentCreditDecision, out int adUserKey, out DateTime decisionDate)
        {
            CurrentCreditDecision = String.Empty;
            adUserKey = 0;
            decisionDate = DateTime.Now;

            IDbConnection conn = Helper.GetSQLDBConnection();
            conn.Open();

            IDataReader reader = null;
            try
            {
                string sql = UIStatementRepository.GetStatement("Application", "GetCreditDecision");
                ParameterCollection prms = new ParameterCollection();
                Helper.AddIntParameter(prms, "@OfferKey", appKey);

                reader = Helper.ExecuteReader(conn, sql, prms);
                while (reader.Read())
                {
                    CurrentCreditDecision = reader.GetString(1);
                    adUserKey = reader.GetInt32(2);
                    decisionDate = reader.GetDateTime(3);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Dispose();
            }

            //remove any nulls
            CurrentCreditDecision = String.IsNullOrEmpty(CurrentCreditDecision) ? String.Empty : CurrentCreditDecision;

            return;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationSourceKey"></param>
        /// <returns></returns>
        public IApplicationSource GetApplicationSourceByKey(int ApplicationSourceKey)
        {
            ApplicationSource_DAO AppSource = ApplicationSource_DAO.TryFind(ApplicationSourceKey);
            if (AppSource != null)
            {
                IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IApplicationSource, ApplicationSource_DAO>(AppSource);
            }
            return null;
        }

        /// <summary>
        /// If there is an Open Further Lending (Readvance, Further Advance or Further Loan) offer or
        /// if there is an NTU or Decline Further Lending offer that does not have an NTU Offer or Decline Offer Stage Transition Composite
        /// then this account has a Further Lending offer in progress
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns>bool</returns>
        public bool HasFurtherLendingInProgress(int accountKey)
        {
            IEventList<IApplication> apps = GetApplicationByAccountKey(accountKey);
            if (apps == null)
                return false;

            foreach (IApplication a in apps)
            {
                if ((a.ApplicationType.Key == (int)OfferTypes.ReAdvance || a.ApplicationType.Key == (int)OfferTypes.FurtherLoan || a.ApplicationType.Key == (int)OfferTypes.FurtherAdvance))
                {
                    if (a.ApplicationStatus.Key == (int)OfferStatuses.Open)
                    {
                        return true;
                    }
                    else
                    {
                        if (a.ApplicationStatus.Key == (int)OfferStatuses.NTU || a.ApplicationStatus.Key == (int)OfferStatuses.Declined)
                        {
                            IStageDefinitionRepository SDRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                            if (!SDRepo.CheckCompositeStageDefinition(a.Key, (int)StageDefinitionStageDefinitionGroups.NTUOffer) && !SDRepo.CheckCompositeStageDefinition(a.Key, (int)StageDefinitionStageDefinitionGroups.DeclineOffer))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Get a RateAdjustmentGroup by its primary key
        /// </summary>
        /// <param name="rateAdjustmentGroupKey"></param>
        /// <returns></returns>
        public IRateAdjustmentGroup GetRateAdjustmentGroupByKey(int rateAdjustmentGroupKey)
        {
            return base.GetByKey<IRateAdjustmentGroup, RateAdjustmentGroup_DAO>(rateAdjustmentGroupKey);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IApplicationInformationAppliedRateAdjustment GetEmptyApplicationInformationAppliedRateAdjustment()
        {
            return base.CreateEmpty<IApplicationInformationAppliedRateAdjustment, ApplicationInformationAppliedRateAdjustment_DAO>();
        }

        /// <summary>
        /// Gets the highest and second highest income contributors on an offer
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="primaryApplicantLEKey">LegalEntityKey of the highest income earner or -1 if none</param>
        /// <param name="secondaryApplicantLEKey">LegalEntityKey of the 2nd highest income earner or -1 if none</param>
        /// <param name="numApplicants">The number of main applicants (or suretors if main applicant is a company)</param>
        public void GetPrimaryAndSecondaryApplicants(int offerKey, out int primaryApplicantLEKey, out int secondaryApplicantLEKey, out int numApplicants)
        {
            primaryApplicantLEKey = -1;
            secondaryApplicantLEKey = -1;
            numApplicants = 0;
            KeyValuePair<int, double> primary = new KeyValuePair<int, double>(-1, -1);
            KeyValuePair<int, double> secondary = new KeyValuePair<int, double>(-1, -1);
            List<IApplicationRole> mainApplicants = new List<IApplicationRole>();
            List<IApplicationRole> suretors = new List<IApplicationRole>();
            IApplication offer = GetApplicationByKey(offerKey);

            foreach (IApplicationRole applicationRole in offer.ApplicationRoles)
            {
                if (applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active && applicationRole.LegalEntity.LegalEntityType.Key == (int)LegalEntityTypes.NaturalPerson)
                {
                    // Find the Income Contributor attribute
                    foreach (IApplicationRoleAttribute applicationRoleAttribute in applicationRole.ApplicationRoleAttributes)
                    {
                        if (applicationRoleAttribute.OfferRoleAttributeType.Key == (int)OfferRoleAttributeTypes.IncomeContributor)
                        {
                            if (applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant || applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant)
                                mainApplicants.Add(applicationRole);
                            else if (applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor || applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor)
                                suretors.Add(applicationRole);

                            break;
                        }
                    }
                }
            }

            if (mainApplicants.Count == 0)
            {
                if (suretors.Count == 0)
                {
                    return;
                }
                else
                {
                    mainApplicants.Clear();
                    mainApplicants.AddRange(suretors);
                }
            }

            if (mainApplicants.Count == 1)
            {
                //If only 1 main applicant - That applicant will automatically be the primary applicant regardless of income of other applicants.
                //If there are 1 or more suretors as well then the suretor with the highest income is the secondary applicant.
                //N.b. Even if main applicant income is R0 (for whatever reason) they must still be the primary applicant.
                primary = new KeyValuePair<int, double>(mainApplicants[0].LegalEntityKey, double.MaxValue);
                numApplicants = 1;
                mainApplicants.Clear();
                mainApplicants.AddRange(suretors);
            }

            numApplicants += mainApplicants.Count;

            //ok, we are left with one or more main applicants (or suretors taking on that role)
            //of these, the highest income contributor is primary
            foreach (IApplicationRole role in mainApplicants)
            {
                double income = 0;

                foreach (IEmployment employment in role.LegalEntity.Employment)
                {
                    if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current
                        && (employment.EmploymentEndDate.HasValue ? employment.EmploymentEndDate.Value : DateTime.Now.AddDays(1)) > DateTime.Now)
                    {
                        if (employment.IsConfirmed)
                            income += employment.ConfirmedIncome;
                        else
                            income += employment.MonthlyIncome;
                    }
                }

                if (income > primary.Value)
                {
                    secondary = primary;
                    primary = new KeyValuePair<int, double>(role.LegalEntityKey, income);
                }
                else if (income > secondary.Value)
                {
                    secondary = new KeyValuePair<int, double>(role.LegalEntityKey, income);
                }
            }

            primaryApplicantLEKey = primary.Key;
            secondaryApplicantLEKey = secondary.Key;

            if (secondaryApplicantLEKey == primaryApplicantLEKey)
                secondaryApplicantLEKey = -1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public bool RapidGoToCreditCheckLTV(IApplication application)
        {
            IApplicationMortgageLoan appML = application as IApplicationMortgageLoan;
            double LTV = 0;
            double LoanAmount = appML.LoanAgreementAmount.Value;

            IApplicationFurtherLending appfl = application as IApplicationFurtherLending;
            if (null != appfl)
            {
                LTV = appfl.EstimatedDisbursedLTV;

                // if rapid and LAA > 1.5 mill goto credit
                //if (app.ApplicationType.Key == 2)
                if (application.ApplicationType.Key == (int)OfferTypes.ReAdvance && LoanAmount > 1500000)
                {
                    // goto credit
                    return false;
                }
                if (LoanAmount < 1500000 && LTV < 0.8)
                {
                    // dont goto credit
                    return true;
                }
                if (LoanAmount > 1500000 && LTV >= 0.8)
                {
                    // goto credit
                    return false;
                }
                return false;
            }
            else
                throw new Exception("RapidGoToCreditCheckLTV - does not contain Further Lending Application.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public DataTable GetOfferRolesNotInAccount(IApplication application)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@offerKey", application.Key));
            string query = UIStatementRepository.GetStatement("Repositories.ApplicationRepository", "GetOfferRolesNotInAccount");
            DataSet ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public void ConvertAcceptedApplicationToExtendedTerm(IApplication application, int term, bool hasPassedTheDecisionTree, double adjustmentAmount)
        {
            //Callback DT to confirm convert is allowed and get adjustment amount
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            if (hasPassedTheDecisionTree)
            {
                application.CreateRevision();

                var applicationAttribute = GetEmptyApplicationAttribute();
                applicationAttribute.ApplicationAttributeType = lookupRepository.ApplicationAttributesTypes.ObjectDictionary[Convert.ToString((int)OfferAttributeTypes.Loanwith30YearTerm)];
                applicationAttribute.Application = application;
                application.ApplicationAttributes.Add(spc.DomainMessages, applicationAttribute);

                var supportVariableLoan = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                supportVariableLoan.VariableLoanInformation.Term = term;

                var applicationProductMortgageLoan = application.CurrentProduct as IApplicationProductMortgageLoan;
                applicationProductMortgageLoan.SetManualDiscount(spc.DomainMessages, adjustmentAmount, FinancialAdjustmentTypeSources.Loanwith30YearTerm);

                application.CalculateApplicationDetail(false, true);

                SaveApplication(application);

                UpdateOfferStatusWithNoValidation(application.Key, application.ApplicationStatus.Key, (int)OfferInformationTypes.AcceptedOffer);
            }
            else
            {
                spc.DomainMessages.Add(new Error("The application must pass the 30 year term decision tree", "The application must pass the 30 year term decision tree"));
            }
        }

        public void RevertToPreviousTermAsAcceptedApplication(IApplication application)
        {
            if (application.HasAttribute(OfferAttributeTypes.Loanwith30YearTerm))
            {
                RevertToPreviousAcceptedTerm(application, true);

                UpdateOfferStatusWithNoValidation(application.Key, application.ApplicationStatus.Key, (int)OfferInformationTypes.AcceptedOffer);
            }
        }

        public void RevertToPreviousTermAsRevisedApplication(IApplication application)
        {
            RevertToPreviousAcceptedTerm(application, false);
        }

        private void RevertToPreviousAcceptedTerm(IApplication application, bool keepMarketRate)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            IApplicationAttribute applicationAttribute = application.ApplicationAttributes.Where(x => x.ApplicationAttributeType.Key == (int)OfferAttributeTypes.Loanwith30YearTerm).FirstOrDefault();
            if (applicationAttribute != null)
            {
                var term = GetPreviousAcceptedRevisionTermExcluding30yrTermRevisions(application);

                application.CreateRevision();

                application.ApplicationAttributes.Remove(spc.DomainMessages, applicationAttribute);

                var supportVariableLoan = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                supportVariableLoan.VariableLoanInformation.Term = term;

                var appFinancialAdjustment = application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments.Where(x =>
                        x.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.Loanwith30YearTerm).FirstOrDefault();

                if (appFinancialAdjustment != null)
                {
                    application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments.Remove(spc.DomainMessages, appFinancialAdjustment);
                }

                application.CalculateApplicationDetail(false, keepMarketRate);

                SaveApplication(application);
            }
        }

        private static int GetPreviousAcceptedRevisionTermExcluding30yrTermRevisions(IApplication application)
        {
            var term = LoanTerms.LoanTerm20YearInMonths;
            var latestAcceptedAppInformationPriorToConvertTo30yrTerm = application.ApplicationInformations.Where(x =>
                x.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer
                && x.Key != application.GetLatestApplicationInformation().Key
                && (x.ApplicationProduct as ISupportsVariableLoanApplicationInformation).VariableLoanInformation.Term.HasValue
                && (x.ApplicationProduct as ISupportsVariableLoanApplicationInformation).VariableLoanInformation.Term.Value != LoanTerms.LoanTerm30YearInMonths)
                .OrderByDescending(y => y.Key).FirstOrDefault();

            if (latestAcceptedAppInformationPriorToConvertTo30yrTerm != null)
            {
                term = ((ISupportsVariableLoanApplicationInformation)latestAcceptedAppInformationPriorToConvertTo30yrTerm.ApplicationProduct).VariableLoanInformation.Term.Value;
            }
            return term;
        }

        public void RemoveDetailFromApplication(IApplication application, List<int> detailTypes)
        {
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

            string sql = string.Format(@"
                    SELECT DT.*
                    FROM [2am].[dbo].Detail DT (nolock)
                    INNER JOIN [2am].[dbo].Account ACC (nolock)
                        ON DT.AccountKey = ACC.AccountKey
                    INNER JOIN [2am].[dbo].Offer OFR (nolock)
                        ON OFR.AccountKey = ACC.AccountKey
                    WHERE
                        OFR.OfferKey = {0} AND
                        DT.DetailTypeKey in (:dtypes) AND
                        DT.DetailDate > OFR.OfferStartDate", application.Key);

            SimpleQuery<Detail_DAO> detailQuery = new SimpleQuery<Detail_DAO>(QueryLanguage.Sql, sql);
            detailQuery.AddSqlReturnDefinition(typeof(Detail_DAO), "DT");
            detailQuery.SetParameterList("dtypes", detailTypes);
            Detail_DAO[] details = detailQuery.Execute();

            for (int i = details.Length - 1; i > -1; i--)
            {
                details[i].DeleteAndFlush();
            }
        }

        #region Helper Methods

        private void AddDetailToAccount(IAccount account, int detailTypeKey)
        {
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            IReadOnlyEventList<IDetail> detailList = accRepo.GetDetailByAccountKeyAndDetailType(account.Key, detailTypeKey);

            // Don't ceate duplicates.
            // Make sure the DetailType does not exist against the Account.
            if (detailList.Count > 0)
                return;

            // Create the DetailType & Add against the Account
            IDetail detail = base.CreateEmpty<IDetail, Detail_DAO>();
            detail.DetailType = lookupRepository.DetailTypes.ObjectDictionary[detailTypeKey.ToString()];
            detail.Account = account;
            detail.Amount = 0;
            detail.DetailDate = DateTime.Now;
            detail.UserID = "System";
            detail.ChangeDate = DateTime.Now;
            account.Details.Add(null, detail);
        }

        #endregion Helper Methods

        /// <summary>
        /// Opt Out of Super Lo
        /// This method MUST be used inside of a transaction
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationReasonKey"></param>
        /// <returns></returns>
        public bool OptOutOfSuperLo(int applicationKey, string userId, int cancellationReasonKey)
        {
            IAccountRepository accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            IAccount account = accountRepository.GetAccountByApplicationKey(applicationKey);

            //Execute Opt Out Of Super Lo Script
            if (account == null)
            {
                AddDomainErrorMessage(new Error(String.Format("Account for Application ({0}) could not be found", applicationKey), String.Format("Account for Application ({0}) could not be found", applicationKey)));
            }

            ExecuteOptOutOfSuperLoScript(account.Key, userId, cancellationReasonKey);

            // Since the type of the DAO Discriminator Type has been changed, this object needs to be removed and reloaded
            ICommonRepository comRepo = RepositoryFactory.GetRepository<ICommonRepository>();
            comRepo.ClearFromNHibernateSession(account);
            account = accountRepository.GetAccountByApplicationKey(applicationKey);

            /*
             * Change the product on all open Further Applications to Variable Loan
             * Recalculate the application details
             * Save the application
             */
            bool openApps = false;
            IApplicationFurtherLoan furtherLoanApplication = null;
            IApplicationFurtherAdvance furtherAdvanceApplication = null;
            IApplicationReAdvance reAdvanceApplication = null;

            foreach (IApplication application in account.Applications)
            {
                if (application.IsOpen)
                {
                    if (application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance)
                    {
                        openApps = true;

                        furtherAdvanceApplication = application as IApplicationFurtherAdvance;
                        furtherAdvanceApplication.SetProduct(GetByKey<IProduct, Product_DAO>((int)Products.NewVariableLoan));
                    }
                    else if (application.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
                    {
                        openApps = true;

                        furtherLoanApplication = application as IApplicationFurtherLoan;
                        furtherLoanApplication.SetProduct(GetByKey<IProduct, Product_DAO>((int)Products.NewVariableLoan));
                    }
                    else if (application.ApplicationType.Key == (int)OfferTypes.ReAdvance)
                    {
                        openApps = true;

                        reAdvanceApplication = application as IApplicationReAdvance;
                        reAdvanceApplication.SetProduct(GetByKey<IProduct, Product_DAO>((int)Products.NewVariableLoan));
                    }
                }
            }

            if (openApps)
            {
                FLAppCalculateHelper flH = new FLAppCalculateHelper(account, reAdvanceApplication, furtherAdvanceApplication, furtherLoanApplication);
                flH.CalculateFurtherLending(false);

                if (reAdvanceApplication != null)
                    SaveApplication(reAdvanceApplication);
                if (furtherAdvanceApplication != null)
                    SaveApplication(furtherAdvanceApplication);
                if (furtherLoanApplication != null)
                    SaveApplication(furtherLoanApplication);
            }

            return true;
        }

        /// <summary>
        /// Execute Opt Out Of Super Lo Script
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userID"></param>
        /// <param name="cancellationreasonkey"></param>
        private void ExecuteOptOutOfSuperLoScript(int accountKey, string userID, int cancellationreasonkey)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountKey));
            prms.Add(new SqlParameter("@UserID", userID));
            prms.Add(new SqlParameter("@Cancellationreasonkey", cancellationreasonkey));

            this.castleTransactionsService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.ApplicationRepository", "ProcessOptOutSuperLo", prms);
        }

        /// <summary>
        /// Domain implementation of Stored Proc [Process].[halo].[pReturnNonDisbursedLoanToApplication]
        /// </summary>
        /// <param name="offerKey"></param>
        public void ReturnNonDisbursedLoanToProspect(int offerKey)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@OfferKey", offerKey));
            this.castleTransactionsService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.ApplicationRepository", "ReturnNonDisbursedLoanToApplication", prms);
        }

        /// <summary>
        /// Create an empty instance of the Application
        /// </summary>
        /// <returns></returns>
        public IApplicationUnsecuredLending GetEmptyApplicationUnsecuredLending()
        {
            return base.CreateEmpty<IApplicationUnsecuredLending, ApplicationUnsecuredLending_DAO>();
        }

        /// <summary>-
        /// Create Unsecured Lending Lead
        /// </summary>
        public IApplicationUnsecuredLending CreateUnsecuredLendingLead()
        {
            //non standard method of creation
            //using DAO objects directly to setup/create the offer with its discrimiated type

            // reserve an account key
            AccountSequence_DAO AS = new AccountSequence_DAO();

            //the call to save the AccountSequence is to get the reserved key from the DB identity Field
            AS.SaveAndFlush();

            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            //Create an empty instance of an application (Unsecured Lending)
            IApplicationUnsecuredLending applicationUnsecuredLending = appRepo.GetEmptyApplicationUnsecuredLending();

            //create the BusinessModel object, because working with DAO's is not standard and should be avoided at all times

            applicationUnsecuredLending.OriginationSource = lookupRepository.OriginationSources.ObjectDictionary[((int)OriginationSources.SAHomeLoans).ToString()];
            applicationUnsecuredLending.ApplicationStatus = lookupRepository.ApplicationStatuses.ObjectDictionary[((int)OfferStatuses.Open).ToString()];
            applicationUnsecuredLending.ApplicationStartDate = DateTime.Now;
            applicationUnsecuredLending.ReservedAccount = new AccountSequence(AS);
            return applicationUnsecuredLending;
        }

        public void PromoteLeadToMain(IApplication app)
        {
            IOrganisationStructureRepository organisationStructureRepository = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

            // get the application roles and migrate leads to normal LE's
            foreach (IApplicationRole role in app.ApplicationRoles)
            {
                switch (role.ApplicationRoleType.Key)
                {
                    case (int)OfferRoleTypes.LeadMainApplicant:
                        {
                            role.ApplicationRoleType = GetApplicationRoleTypeByKey((int)OfferRoleTypes.MainApplicant);
                            organisationStructureRepository.SaveApplicationRole(role);
                            break;
                        }
                    case (int)OfferRoleTypes.LeadSuretor:
                        {
                            role.ApplicationRoleType = GetApplicationRoleTypeByKey((int)OfferRoleTypes.Suretor);
                            organisationStructureRepository.SaveApplicationRole(role);
                            break;
                        }
                }
            }
        }

        public void DemoteMainToLead(IApplication app)
        {
            IOrganisationStructureRepository organisationStructureRepository = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

            // get the application roles and migrate leads to normal LE's
            foreach (IApplicationRole role in app.ApplicationRoles)
            {
                switch (role.ApplicationRoleType.Key)
                {
                    case (int)OfferRoleTypes.MainApplicant:
                        {
                            role.ApplicationRoleType = GetApplicationRoleTypeByKey((int)OfferRoleTypes.LeadMainApplicant);
                            organisationStructureRepository.SaveApplicationRole(role);
                            break;
                        }
                    case (int)OfferRoleTypes.Suretor:
                        {
                            role.ApplicationRoleType = GetApplicationRoleTypeByKey((int)OfferRoleTypes.LeadSuretor);
                            organisationStructureRepository.SaveApplicationRole(role);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Creates and saves an active application role
        /// No business rules will be run on save
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="offerRoleTypeKey"></param>
        /// <param name="legalEntityKey"></param>
        [Obsolete("If you want to save without rules being executed use rule exclusions.")]
        public void CreateAndSaveApplicationRole_WithoutRules(int applicationKey, int offerRoleTypeKey, int legalEntityKey)
        {
            ApplicationRole_DAO role = new ApplicationRole_DAO();
            role.ApplicationKey = applicationKey;
            role.ApplicationRoleType = ApplicationRoleType_DAO.TryFind(offerRoleTypeKey);
            role.GeneralStatus = GeneralStatus_DAO.TryFind((int)GeneralStatuses.Active);
            role.LegalEntityKey = legalEntityKey;
            role.StatusChangeDate = DateTime.Now;

            role.SaveAndFlush();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="offerStatusKey"></param>
        /// <param name="offerInformationTypeKey"></param>
        public void UpdateOfferStatusWithNoValidation(int applicationKey, int offerStatusKey, int offerInformationTypeKey)
        {
            Application_DAO app = null;

            try
            {
                app = Application_DAO.Find(applicationKey);
            }
            catch (Castle.ActiveRecord.NotFoundException)
            {
                throw new Exception("Could not update offer status as no offer record exists.");
            }

            app.ApplicationStatus = ApplicationStatus_DAO.Find(offerStatusKey);

            switch (offerStatusKey)
            {
                case (int)OfferStatuses.Closed:
                case (int)OfferStatuses.Accepted:
                case (int)OfferStatuses.NTU:
                case (int)OfferStatuses.Declined:
                    app.ApplicationEndDate = DateTime.Now;
                    break;

                case (int)OfferStatuses.Open:
                    app.ApplicationEndDate = null;
                    break;

                default:
                    break;
            }

            if (offerInformationTypeKey != -1)
            {
                // Get the Latest ApplicationInformationsa
                ApplicationInformation_DAO[] arr = ApplicationInformation_DAO.FindAllByProperty("Key", "Application.Key", applicationKey);
                ApplicationInformation_DAO info = arr[arr.Length - 1];

                info.ApplicationInformationType = ApplicationInformationType_DAO.Find(offerInformationTypeKey);
                info.SaveAndFlush();
                app.SaveAndFlush();
            }
            else
            {
                app.SaveAndFlush();
            }
        }

        /// <summary>
        /// Specifically used for an application that is partially committed or persisted
        /// e.g. ApplicationSave -> Application Wizard
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public List<ApplicationAttributeToApply> DetermineApplicationAttributeTypes(IApplication application)
        {
            var sql = uiStatementService.GetStatement("Application", "GetOfferAttributeTypes");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@OfferKey", application.Key));

            DataTable possibleApplicationAttributeTypeKeys = new DataTable();
            var dataSet = castleTransactionsService.ExecuteQueryOnCastleTran(sql, Databases.TwoAM, parameters);
            possibleApplicationAttributeTypeKeys = dataSet.Tables[0];

            var applicationAttributeTypeKeys = new List<ApplicationAttributeToApply>();
            foreach (DataRow row in possibleApplicationAttributeTypeKeys.Rows)
            {
                applicationAttributeTypeKeys.Add(new ApplicationAttributeToApply
                {
                    ApplicationAttributeTypeKey = int.Parse(row["OfferAttributeTypeKey"].ToString()),
                    ToBeRemoved = bool.Parse(row["Remove"].ToString())
                });
            }
            return applicationAttributeTypeKeys;
        }

        /// <summary>
        /// Specifically used for an application that is not partially committed nor persisted
        /// e.g. Update Loan Details -> New Origination OR Further Lending Calculator
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="LTV"></param>
        /// <param name="employmentTypekey"></param>
        /// <param name="houseHoldIncome"></param>
        /// <param name="isStaffLoan"></param>
        /// <param name="isGEPF"></param>
        /// <returns></returns>
        public List<ApplicationAttributeToApply> DetermineApplicationAttributeTypes(int applicationKey, double LTV, int employmentTypekey, double houseHoldIncome, bool isStaffLoan, bool isGEPF)
        {
            var sql = uiStatementService.GetStatement("Application", "GetOfferAttributeTypesByParameters");

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@OfferKey", applicationKey));
            parameters.Add(new SqlParameter("@ltv", LTV));
            parameters.Add(new SqlParameter("@employmentTypekey", employmentTypekey));
            parameters.Add(new SqlParameter("@houseHoldIncome", houseHoldIncome));
            parameters.Add(new SqlParameter("@isStaffLoan", isStaffLoan));
            parameters.Add(new SqlParameter("@isGEPF", isGEPF));

            DataTable possibleApplicationAttributeTypeKeys = new DataTable();
            var dataSet = castleTransactionsService.ExecuteQueryOnCastleTran(sql, Databases.TwoAM, parameters);
            possibleApplicationAttributeTypeKeys = dataSet.Tables[0];

            var applicationAttributeTypeKeys = new List<ApplicationAttributeToApply>();
            foreach (DataRow row in possibleApplicationAttributeTypeKeys.Rows)
            {
                applicationAttributeTypeKeys.Add(new ApplicationAttributeToApply
                {
                    ApplicationAttributeTypeKey = int.Parse(row["OfferAttributeTypeKey"].ToString()),
                    ApplicationAttributeTypeGroupKey = int.Parse(row["OfferAttributeTypeGroupKey"].ToString()),
                    ToBeRemoved = bool.Parse(row["Remove"].ToString())
                });
            }
            return applicationAttributeTypeKeys;
        }

        public void UpdateApplicationAttributeTypes(List<ApplicationAttributeToApply> applicationAttributeTypes, IApplication application)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            foreach (var applicationAttributeType in applicationAttributeTypes)
            {
                if (application.HasAttribute((OfferAttributeTypes)applicationAttributeType.ApplicationAttributeTypeKey) && applicationAttributeType.ToBeRemoved)
                {
                    var applicationAttributeToRemove = application.ApplicationAttributes.FirstOrDefault(x => x.ApplicationAttributeType.Key == applicationAttributeType.ApplicationAttributeTypeKey);
                    application.ApplicationAttributes.Remove(spc.DomainMessages, applicationAttributeToRemove);
                }
                else if (!application.HasAttribute((OfferAttributeTypes)applicationAttributeType.ApplicationAttributeTypeKey) && !applicationAttributeType.ToBeRemoved)
                {
                    IApplicationAttribute applicationAttribute = GetEmptyApplicationAttribute();
                    applicationAttribute.ApplicationAttributeType = lookupRepository.ApplicationAttributesTypes.First(x => x.Key == applicationAttributeType.ApplicationAttributeTypeKey);
                    applicationAttribute.Application = application;
                    application.ApplicationAttributes.Add(spc.DomainMessages, applicationAttribute);
                }
            }
        }

        #region CreateWebLeadAndApplication

        #region Create DB Data for web lead\application

        public int CreateWebLead(ILeadInputInformation leadInput)
        {
            int applicationKey = -1;

            //1. Reserve an application\offer key the client\user can use as a reference
            applicationKey = CreateOfferRecord();

            //2. then create an X2 case with the user\client input to generate the BM objects and calculate the application if required

            if (CreateExternalActivity(applicationKey, leadInput))
                return applicationKey;

            //to indicate that something bad happend to the caller
            return -1;
        }

        /// <summary>
        ///
        /// </summary>
        protected int CreateOfferRecord()
        {
            string sql = SAHL.Common.DataAccess.UIStatementRepository.GetStatement("Repositories.ApplicationRepository", "CreateNetApplication");
            object obj = AbstractRepositoryBase.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

            return Convert.ToInt32(obj);
        }

        //This is the new method for creating a workflowcase
        // See Trac ticket #11675
        private bool CreateExternalActivity(int applicationkey, ILeadInputInformation leadInput)
        {
            string externalActivityType = leadInput.OfferTypeKey == (int)OfferTypes.Unknown ? "EXTCreateNetLead" : "EXTCreateNetApplication";

            // Insert record into x2.ActiveExternalActivity
            IX2Repository x2Repository = RepositoryFactory.GetRepository<IX2Repository>();

            IActiveExternalActivity activity = x2Repository.CreateActiveExternalActivity();
            activity.ActivatingInstanceID = -1;
            activity.ActivationTime = DateTime.Now;
            activity.ActivityXMLData = string.Format(
                "<FieldInputs><FieldInput Name = \"applicationKey\">{0}</FieldInput><FieldInput Name = \"netLeadXML\">{1}</FieldInput></FieldInputs>",
                applicationkey,
                CreateNetLeadXML(leadInput)
                );
            IWorkFlow wf = x2Repository.GetWorkFlowByName("Application Capture", "Origination");
            activity.ExternalActivity = x2Repository.GetExternalActivityByName(externalActivityType, wf.ID);
            activity.WorkFlowID = wf.ID;
            activity.WorkFlowProviderName = "";

            x2Repository.SaveActiveExternalActivity(activity);

            return true;
        }

        public string CreateNetLeadXML(ILeadInputInformation leadInput)
        {
            StringWriter stringWriter = new StringWriter();

            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;

            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, writerSettings))
            {
                XmlSerializer x = new XmlSerializer(leadInput.GetType());
                x.Serialize(xmlWriter, leadInput);
            }
            return stringWriter.GetStringBuilder().ToString();
        }

        public ILeadInputInformation DeserializeNetLeadXML(string leadInput)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LeadInputInformation));
            object result;

            using (TextReader reader = new StringReader(leadInput))
            {
                result = serializer.Deserialize(reader);
            }

            return result as ILeadInputInformation;
        }

        #endregion Create DB Data for web lead\application

        #region Generateapplication

        public bool GenerateApplicationFromWeb(int offerKey, ILeadInputInformation leadInput)
        {
            bool ret = false;
            IADUser adUser = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetAdUserForAdUserName(@"SAHL\WebLeads");
            IApplicationRepository applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            try
            {
                //bug fix for #24412, fix percent is not being set from sahomeloans.com
                if (leadInput.ProductKey == (int)Products.VariFixLoan && leadInput.FixPercent == 0)
                    leadInput.FixPercent = 100;

                spc.ExclusionSets.Add(RuleExclusionSets.WebLeadExclusionSet);
                spc.ExclusionSets.Add(RuleExclusionSets.LegalEntityExcludeAll);

                //create offer by type and populate the data
                IApplication application = SetupApplication(offerKey, leadInput);
                //calculate the offer to populate all the calculated properties

                //save the offer
                applicationRepository.SaveApplication(application);

                //create the legal entity
                ILegalEntity legalEntity = CreateAndSaveNewLegalEntity(leadInput, adUser);
                //Employment
                AddLegalEntityEmploymentrecord(leadInput, legalEntity, adUser);
                //offer roles
                application.AddLeadRole(Lead_MortgageLoan_Role.LEAD_MAIN_APPLICANT, legalEntity);

                applicationRepository.SaveApplication(application);

                //Referrer detail
                ApplicationAddInternetSourceInformation(application, leadInput);

                ret = true;
            }
            finally
            {
                spc.ExclusionSets.Remove(RuleExclusionSets.WebLeadExclusionSet);
                spc.ExclusionSets.Remove(RuleExclusionSets.LegalEntityExcludeAll);
            }
            return ret;
        }

        public bool GenerateLeadFromWeb(int offerKey, ILeadInputInformation leadInput)
        {
            bool ret = false;

            IADUser adUser = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetAdUserForAdUserName(@"SAHL\WebLeads");
            IApplicationRepository applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
            ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            try
            {
                spc.ExclusionSets.Add(RuleExclusionSets.WebLeadExclusionSet);
                spc.ExclusionSets.Add(RuleExclusionSets.LegalEntityExcludeAll);

                //create offer by type and populate the data
                IApplication application = applicationRepository.GetApplicationByKey(offerKey);

                application.ApplicationStartDate = DateTime.Now;
                application.ApplicationSource = lookupRepository.ApplicationSources.ObjectDictionary[Convert.ToString(leadInput.OfferSourceKey)];
                application.EstimateNumberApplicants = leadInput.NumberOfApplicants;
                application.ApplicationStatus = lookupRepository.ApplicationStatuses.ObjectDictionary[Convert.ToString((int)OfferStatuses.Open)];

                //create the legal entity
                ILegalEntity legalEntity = CreateAndSaveNewLegalEntity(leadInput, adUser);
                //Employment

                AddLegalEntityEmploymentrecord(leadInput, legalEntity, adUser);
                //offer roles
                application.AddLeadRole(Lead_MortgageLoan_Role.LEAD_MAIN_APPLICANT, legalEntity);

                applicationRepository.SaveApplication(application);

                //MemoSave
                MemoSave(leadInput, adUser, offerKey);

                //Referrer detail
                ApplicationAddInternetSourceInformation(application, leadInput);

                ret = true;
            }
            finally
            {
                spc.ExclusionSets.Remove(RuleExclusionSets.WebLeadExclusionSet);
                spc.ExclusionSets.Remove(RuleExclusionSets.LegalEntityExcludeAll);
            }
            return ret;
        }

        private static void MemoSave(ILeadInputInformation leadInput, IADUser adUser, int offerKey)
        {
            IMemoRepository memoRepository = RepositoryFactory.GetRepository<IMemoRepository>();
            ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

            // Add a Legal entity Comments to store Affordability Calculator information
            string memoText = "Internet Lead Information : " +
                              " Household Income = " +
                              (leadInput.HouseholdIncome).ToString(Common.Constants.CurrencyFormatNoCents) +
                              ", Maximum instalment = " +
                              (leadInput.MonthlyInstalment).ToString(Common.Constants.CurrencyFormatNoCents) +
                              ", Loan Term = " + leadInput.Term + " months " +
                              ", Deposit = " + (leadInput.Deposit).ToString(Common.Constants.CurrencyFormatNoCents) +
                              ", Loan Amount Required (excl Deposit) = " +
                              (leadInput.LoanAmountRequired).ToString(Common.Constants.CurrencyFormatNoCents) +
                              ", Maximum Bond Amount  = " +
                              (leadInput.TotalPrice).ToString(Common.Constants.CurrencyFormatNoCents) +
                              ", Interest Rate = " + (leadInput.InterestRate).ToString(Common.Constants.RateFormat);

            IMemo memo = memoRepository.CreateMemo();
            memo.ADUser = adUser;
            memo.GenericKey = offerKey;
            memo.GenericKeyType = lookupRepository.GenericKeyType.ObjectDictionary[Convert.ToString((int)GenericKeyTypes.Offer)];
            memo.GeneralStatus = lookupRepository.GeneralStatuses[GeneralStatuses.Active];
            memo.InsertedDate = DateTime.Now.Date;
            memo.Description = memoText;

            memoRepository.SaveMemo(memo);
        }

        #region Create Legal Entity

        /// <summary>
        ///
        /// </summary>
        protected ILegalEntity CreateAndSaveNewLegalEntity(ILeadInputInformation leadInput, IADUser adUser)
        {
            ILegalEntityRepository leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            ICommonRepository commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();

            ILegalEntity legalEntity = leRepo.GetEmptyLegalEntity(LegalEntityTypes.NaturalPerson);
            legalEntity.IntroductionDate = DateTime.Now;
            legalEntity.LegalEntityStatus = lookupRepo.LegalEntityStatuses.ObjectDictionary[Convert.ToString((int)LegalEntityStatuses.Alive)];
            legalEntity.HomePhoneCode = leadInput.HomePhoneCode;
            legalEntity.HomePhoneNumber = leadInput.HomePhoneNumber;
            legalEntity.DocumentLanguage = commonRepo.GetLanguageByKey((int)Languages.English);
            legalEntity.EmailAddress = leadInput.EmailAddress;
            //legalEntity.ResidenceStatus = lookupRepo.ResidenceStatuses[1]; //this has been deprecated in favour of citizen type
            legalEntity.UserID = adUser.ADUserName;
            legalEntity.ChangeDate = DateTime.Now;

            ILegalEntityNaturalPerson lenp = legalEntity as ILegalEntityNaturalPerson;
            if (lenp != null)
            {
                lenp.FirstNames = leadInput.FirstNames;
                lenp.Surname = leadInput.Surname;
            }

            leRepo.SaveLegalEntity(lenp, false);

            return legalEntity;
        }

        private void AddLegalEntityEmploymentrecord(ILeadInputInformation leadInput, ILegalEntity lenp, IADUser adUser)
        {
            ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            IEmploymentRepository employmentRepository = RepositoryFactory.GetRepository<IEmploymentRepository>();

            int employmentTypeKey = leadInput.EmploymentTypeKey > 0 ? leadInput.EmploymentTypeKey : (int)EmploymentTypes.Salaried;
            IEmploymentType employmentType = lookupRepository.EmploymentTypes.ObjectDictionary[employmentTypeKey.ToString()];

            IEmployment employment = employmentRepository.GetEmptyEmploymentByType(employmentType);
            employment.BasicIncome = leadInput.HouseholdIncome;
            employment.ChangeDate = DateTime.Now;

            //This is a fudge - required in the Database and we dont have it.
            employment.EmploymentStartDate = DateTime.Now;

            employment.LegalEntity = lenp;
            employment.EmploymentStatus = lookupRepository.EmploymentStatuses.ObjectDictionary[((int)EmploymentStatuses.Current).ToString()];
            // Defaults to Salaried
            employment.RemunerationType = lookupRepository.RemunerationTypes.ObjectDictionary[((int)RemunerationTypes.Salaried).ToString()];
            employment.UserID = adUser.ADUserName;

            employmentRepository.SaveEmployment(employment);
        }

        #endregion Create Legal Entity

        #region SetupApplication

        private IApplication SetupApplication(int offerKey, ILeadInputInformation leadInput)
        {
            #region ApplicationCreateDefaults

            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IApplication app = appRepo.GetApplicationByKey(offerKey);

            var os = appRepo.GetOriginationSource(OriginationSources.SAHomeLoans); //(OriginationSources)OriginationSourceHelper.PrimaryOriginationSourceKey(leadInput.CurrentPrincipal)
            var resetConfig = appRepo.GetApplicationDefaultResetConfiguration();
            var statusOpen = lookupRepo.ApplicationStatuses.ObjectDictionary[((int)OfferStatuses.Open).ToString()];
            //var marketRateKey = (int)MarketRates.ThreeMonthJIBARRounded;

            #endregion ApplicationCreateDefaults

            switch (leadInput.MortgageLoanPurposeKey)
            {
                case (int)MortgageLoanPurposes.Refinance:
                    app = appRepo.CreateRefinanceApplication(os, (ProductsRefinanceAtCreation)leadInput.ProductKey, (IApplicationUnknown)app);
                    IApplicationMortgageLoanRefinance ar = app as IApplicationMortgageLoanRefinance;
                    ar.CapitaliseFees = leadInput.CapitaliseFees;
                    //ar.ClientEstimatePropertyValuation = leadInput.EstimatedPropertyValue;

                    break;

                case (int)MortgageLoanPurposes.Switchloan:
                    app = appRepo.CreateSwitchLoanApplication(os, (ProductsSwitchLoanAtCreation)leadInput.ProductKey, (IApplicationUnknown)app);
                    IApplicationMortgageLoanSwitch asw = app as IApplicationMortgageLoanSwitch;
                    asw.CapitaliseFees = leadInput.CapitaliseFees;
                    break;

                default: //Defaults to new purchase
                    app = appRepo.CreateNewPurchaseApplication(os, (ProductsNewPurchaseAtCreation)leadInput.ProductKey, (IApplicationUnknown)app);
                    IApplicationMortgageLoanNewPurchase anp = app as IApplicationMortgageLoanNewPurchase;
                    anp.PurchasePrice = leadInput.PropertyValue - leadInput.Deposit;
                    //anp.CashDeposit = leadInput.Deposit;
                    break;
            }

            //this must be done after the application type has been created, the unknown app is evicted from memory, so any properties that have been set will be lost.
            app.ApplicationSource = appRepo.GetApplicationSourceByKey(Convert.ToInt32(leadInput.OfferSourceKey));
            app.EstimateNumberApplicants = leadInput.NumberOfApplicants;

            //do the product specific stuff
            switch (leadInput.ProductKey)
            {
                //we no longer originate SuperLo
                //case (int)Products.SuperLo:
                //    SetupSuperLo(app.CurrentProduct);
                //    break;
                case (int)Products.VariFixLoan:
                    SetupVariFix(app.CurrentProduct, leadInput);
                    break;
                //Web Calculators can not originat Edge
                //case (int)Products.Edge:
                //    SetupEHL(app.CurrentProduct);
                //    break;
                default: //case (int)Products.NewVariableLoan:
                    SetupNVL(app.CurrentProduct, leadInput);
                    break;
            }

            //Mortgage Loan details
            IApplicationMortgageLoan appML = (IApplicationMortgageLoan)app;
            appML.ApplicationStartDate = DateTime.Now;
            appML.ApplicationStatus = statusOpen;
            appML.ClientEstimatePropertyValuation = leadInput.PropertyValue;
            appML.ResetConfiguration = resetConfig;

            appML.CalculateApplicationDetail(false, false);

            return app;
        }

        #region SetupProductType

        /// <summary>
        ///
        /// </summary>
        /// <param name="aivl"></param>
        protected void SetupVariableInformation(IApplicationInformationVariableLoan aivl, ILeadInputInformation leadInput)
        {
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            if (aivl != null)
            {
                aivl.BondToRegister = Common.BusinessModel.Helpers.LoanCalculator.CalculateBondAmount(leadInput.LoanAmountRequired);
                aivl.CashDeposit = leadInput.Deposit;
                aivl.EmploymentType = lookupRepo.EmploymentTypes.ObjectDictionary[leadInput.EmploymentTypeKey.ToString()]; ;
                aivl.ExistingLoan = leadInput.CurrentLoan; //this should always be 0 for a new purchase loan
                //aivl.FeesTotal = leadInput.TotalFee;
                aivl.HouseholdIncome = leadInput.HouseholdIncome;
                //aivl.InterimInterest = leadInput.InterimInterest;
                //aivl.LoanAmountNoFees = leadInput.LoanAmountRequired - (leadInput.CapitaliseFees ? leadInput.TotalFee : 0D);
                //aivl.MarketRate = leadInput.ActiveMarketRate;
                //aivl.MonthlyInstalment = leadInput.InstalmentTotal;
                aivl.PropertyValuation = leadInput.PropertyValue;
                //aivl.PTI = leadInput.PTI;
                //aivl.RateConfiguration = CMRepo.GetRateConfigurationByMarginKeyAndMarketRateKey(leadInput.MarginKey, leadInput.marketrateKey);
                //leadInput.rateConfigKey = aivl.RateConfiguration.Key;
                //aivl.SPV = calculatorBase.NewSPVFromLTV(leadInput.LTV);
                aivl.Term = leadInput.Term;
            }

            if (leadInput.MortgageLoanPurposeKey == (int)MortgageLoanPurposes.Switchloan || leadInput.MortgageLoanPurposeKey == (int)MortgageLoanPurposes.Refinance)
            {
                IApplicationInformationVariableLoanForSwitchAndRefinance aivlsr = (IApplicationInformationVariableLoanForSwitchAndRefinance)aivl;
                if (aivlsr != null)
                    aivlsr.RequestedCashAmount = leadInput.CashOut;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aiio"></param>
        /// <param name="messages"></param>
        protected void SetupInterestOnly(IApplicationInformationInterestOnly aiio, ILeadInputInformation leadInput)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IApplicationRepository applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
            IFinancialAdjustmentRepository FinancialAdjustmentRepo = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();

            //CalculatorBase calculatorBase = new CalculatorBase();
            if (leadInput.InterestOnly)
            {
                if (aiio != null)
                {
                    //if (isEHL)
                    //    aiio.MaturityDate = DateTime.Now.AddMonths(36);
                    //else
                    aiio.MaturityDate = MaturityDate(DateTime.Now, leadInput.Term);

                    IApplicationInformationFinancialAdjustment iofa = applicationRepository.GetEmptyApplicationInformationFinancialAdjustment();
                    iofa.Discount = 0;
                    iofa.Term = -1;
                    iofa.FinancialAdjustmentTypeSource = FinancialAdjustmentRepo.GetFinancialAdjustmentTypeSourceByKey((int)FinancialAdjustmentTypeSources.InterestOnly);
                    iofa.ApplicationInformation = aiio.ApplicationInformation;
                    aiio.ApplicationInformation.ApplicationInformationFinancialAdjustments.Add(spc.DomainMessages, iofa);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected DateTime MaturityDate(DateTime fromDate, int Term)
        {
            DateTime maturity = fromDate.AddMonths(Term + 6);

            if (maturity.Day > 15) //get the next month
                maturity.AddMonths(1);

            // return the last of this month
            return new DateTime(maturity.Year, maturity.Month, 1).AddMonths(1).AddDays(-1);
        }

        protected IApplicationProductNewVariableLoan SetupNVL(IApplicationProduct prod, ILeadInputInformation leadInput)
        {
            ISPVService spvService = ServiceFactory.GetService<ISPVService>();
            IApplicationProductNewVariableLoan nvl = prod as IApplicationProductNewVariableLoan;
            if (null != nvl)
            {
                IApplicationMortgageLoanNewPurchase npml = nvl.Application as IApplicationMortgageLoanNewPurchase;
                if (npml != null)
                {
                    npml.PurchasePrice = leadInput.PropertyValue;
                }
                //nvl.LoanAmountNoFees = leadInput.LoanAmountRequired - (leadInput.CapitaliseFees ? leadInput.TotalFee : 0D); //TODO get rid of the fees here
                nvl.Term = leadInput.Term;

                SetupVariableInformation(nvl.VariableLoanInformation, leadInput);
                spvService.DetermineSPVOnApplication(nvl.Application);
                SetupInterestOnly(nvl.InterestOnlyInformation, leadInput);
            }
            return nvl;
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="prod"></param>
        ///// <param name="messages"></param>
        ///// <returns></returns>
        //protected IApplicationProductSuperLoLoan SetupSuperLo(IApplicationProduct prod)
        //{
        //    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
        //    IApplicationProductSuperLoLoan sl = prod as IApplicationProductSuperLoLoan;
        //    if (null != sl)
        //    {
        //        IApplicationMortgageLoanNewPurchase npml = sl.Application as IApplicationMortgageLoanNewPurchase;
        //        if (npml != null)
        //        {
        //            npml.PurchasePrice = leadInput.TotalPrice;
        //            npml.ClientEstimatePropertyValuation = leadInput.EstimatedPropertyValue;
        //        }
        //        sl.LoanAmountNoFees = leadInput.LoanAmountRequired - (leadInput.CapitaliseFees ? leadInput.TotalFee : 0D);
        //        sl.Term = leadInput.Term;

        //        SetupVariableInformation(sl.VariableLoanInformation);
        //        SPVService.DetermineSPVOnApplication(sl.Application);
        //        SetupInterestOnly(sl.InterestOnlyInformation, messages);

        //        IApplicationInformationSuperLoLoan sli = sl.SuperLoInformation;
        //        double annualThreshold = leadInput.LoanAmountRequired * 0.04;
        //        sli.PPThresholdYr1 = annualThreshold;
        //        sli.PPThresholdYr2 = annualThreshold;
        //        sli.PPThresholdYr3 = annualThreshold;
        //        sli.PPThresholdYr4 = annualThreshold;
        //        sli.PPThresholdYr5 = annualThreshold;
        //        sli.ElectionDate = DateTime.Now;

        //        IApplicationInformationFinancialAdjustment slro = appRepo.GetEmptyApplicationInformationFinancialAdjustment();

        //        IMarginProduct mp = CMRepo.GetMarginProductByRateConfigAndOSP(leadInput.rateConfigKey, OriginationSource.Key, leadInput.ProductKey);
        //        slro.Discount = mp != null ? mp.Discount : 0;
        //        slro.Term = -1;
        //        slro.FinancialAdjustmentTypeSource = FinancialAdjustmentRepo.GetFinancialAdjustmentTypeSourceByKey((int)FinancialAdjustmentTypeSources.SuperLo);
        //        slro.ApplicationInformation = sli.ApplicationInformation;
        //        sli.ApplicationInformation.ApplicationInformationFinancialAdjustments.Add(spc.DomainMessages, slro);
        //    }

        //    return sl;

        //}

        /// <summary>sure
        ///
        ///
        /// </summary>
        /// <param name="prod"></param>
        /// <param name="leadInput"></param>
        /// <returns></returns>
        protected IApplicationProductVariFixLoan SetupVariFix(IApplicationProduct prod, ILeadInputInformation leadInput)
        {
            ISPVService spvService = ServiceFactory.GetService<ISPVService>();
            ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            IApplicationProductVariFixLoan vfl = prod as IApplicationProductVariFixLoan;
            if (null != vfl)
            {
                IApplicationMortgageLoanNewPurchase npml = vfl.Application as IApplicationMortgageLoanNewPurchase;
                if (npml != null)
                {
                    npml.PurchasePrice = leadInput.PropertyValue;
                    npml.ClientEstimatePropertyValuation = leadInput.PropertyValue;
                }
                //vfl.LoanAmountNoFees = leadInput.LoanAmountRequired - (leadInput.CapitaliseFees ? leadInput.TotalFee : 0D);
                vfl.Term = leadInput.Term;

                SetupVariableInformation(vfl.VariableLoanInformation, leadInput);
                spvService.DetermineSPVOnApplication(vfl.Application);
                IApplicationInformationVarifixLoan aivf = vfl.VariFixInformation;

                aivf.FixedPercent = leadInput.FixPercent / 100;
                aivf.ElectionDate = DateTime.Now;
                aivf.MarketRate = lookupRepository.MarketRates.ObjectDictionary[((int)MarketRates.FiveYearResetFixedMortgageRate).ToString()];
            }

            return vfl;
        }

        #endregion SetupProductType

        #endregion SetupApplication

        /// <summary>
        /// This method adds the internet source information to OfferInternetReferrer
        /// </summary>
        protected void ApplicationAddInternetSourceInformation(IApplication application, ILeadInputInformation leadInput)
        {
            IApplicationRepository applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();

            IApplicationInternetReferrer internetreferrer = applicationRepository.GetEmptyApplicationInternetReferrer();
            internetreferrer.Application = application;

            internetreferrer.Parameters = string.IsNullOrEmpty(leadInput.AdvertisingCampaignID) ? "" : leadInput.AdvertisingCampaignID;

            if (internetreferrer.Parameters.Length > 49)
            {
                internetreferrer.Parameters = internetreferrer.Parameters.Substring(0, 49);
            }

            internetreferrer.ReferringServerURL = leadInput.ReferringServerURL;
            internetreferrer.UserURL = leadInput.UserURL;
            internetreferrer.UserURL = leadInput.UserAddress;

            applicationRepository.SaveApplicationInternetReferrer(internetreferrer);
        }

        #endregion Generateapplication

        #endregion CreateWebLeadAndApplication

        public bool IsAlphaHousingLoan(double ltv, int employmentTypeKey, double householdIncome)
        {
            var offerAttributesToApply = DetermineApplicationAttributeTypes(0, ltv, employmentTypeKey, householdIncome, false, false);
            return offerAttributesToApply.Any(x => x.ApplicationAttributeTypeKey == (int)OfferAttributeTypes.AlphaHousing && x.ToBeRemoved == false);
        }

        public IApplicationRoleDomicilium CreateEmptyApplicationRoleDomicilium()
        {
            return base.CreateEmpty<IApplicationRoleDomicilium, ApplicationRoleDomicilium_DAO>();
        }

        public IEventList<IApplicationRoleDomicilium> GetApplicationsThatUseLegalEntityDomicilium(ILegalEntityAddress activeLegalEntityDomicilium)
        {
            string hql = "select applicationRoleDomicilium from ApplicationRoleDomicilium_DAO applicationRoleDomicilium where applicationRoleDomicilium.LegalEntityDomicilium.LegalEntityAddress.Key = ?";

            SimpleQuery<ApplicationRoleDomicilium_DAO> q = new SimpleQuery<ApplicationRoleDomicilium_DAO>(hql, activeLegalEntityDomicilium.Key);
            ApplicationRoleDomicilium_DAO[] applicationRoleDomiciliums = q.Execute();

            return new DAOEventList<ApplicationRoleDomicilium_DAO, IApplicationRoleDomicilium, ApplicationRoleDomicilium>(applicationRoleDomiciliums);
        }

        public IEventList<IApplicationRole> GetActiveApplicationRolesByLegalEntityKey(int LegalEntityKey)
        {
            string hql = "select d from ApplicationRole_DAO d where d.LegalEntityKey = ? and d.GeneralStatus.Key=1";
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(hql, LegalEntityKey);
            ApplicationRole_DAO[] activeApplicationRoles = q.Execute();
            return new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(activeApplicationRoles);
        }

        /// <summary>
        /// Check the application is ready to submit to credit
        /// 1. Validate the application DAO and rules
        /// 2. Validate all the client role legal entity DAO's and rules
        /// 3. Check the process related rule sets
        /// 4. Update the document Checklist
        /// </summary>
        /// <param name="application"></param>
        /// <param name="ruleSet"></param>
        /// <returns></returns>
        private bool IsApplicationInOrderCommon(IApplicationMortgageLoan application, string ruleSet)
        {
            if (!application.ValidateEntity())
                return false;

            if (!IsApplicationRolesValid(application))
                return false;

            if (!RefreshApplicationDocumentCheckList(application.Key))
                return false;

            //This must run after RefreshApplicationDocumentCheckList as the rules check that data
            //return RunApplicationRuleSet(application, RuleSets.ApplicationManagementApplicationInOrder);
            return RunApplicationRuleSet(application, ruleSet);
        }

        /// <summary>
        /// Check the application is ready to submit to credit
        ///
        /// Then run the common application in order rules: bool IsApplicationInOrderCommon(int ApplicationKey)
        /// 1. Validate the application DAO and rules
        /// 2. Validate all the client role legal entity DAO's and rules
        /// 3. Check the process related rule sets
        /// 4. Update the document Checklist
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        public bool ResubmitToCredit(int applicationKey)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IApplicationMortgageLoan application = GetApplicationByKey(applicationKey) as IApplicationMortgageLoan;

            if (application == null)
                return false;

            Int64 instanceID;
            bool requireValuation;
            bool isFL;

            if (!GetApplicationManagementData(applicationKey, out instanceID, out requireValuation, out isFL))
                return false;

            if (x2Repository.HasInstancePerformedActivity(instanceID, "Override Check"))
            {
                if (!isFL)
                {
                    if (!RunApplicationRuleSet(application, RuleSets.ApplicationManagementCreditOverrideCheck))
                        return false;
                }

                if (!RunApplicationRuleSet(application, RuleSets.ApplicationManagementResubOverRideCheck))
                    return false;
            }
            else
            {
                if (!IsApplicationInOrderCommon(application, RuleSets.ApplicationManagementResubmitToCredit))
                    return false;

                if (requireValuation &&
                    !RunApplicationRuleSet(application, RuleSets.ApplicationManagementValuationRequired))
                    return false;
            }

            //All of the above rules have completed without any rule errors.
            //This method should return the result of Warning messages, so that
            //the ignoreWarnings property is managed by user interaction

            // if the user has selected ignore warnings, no warning rules will be run
            return !spc.DomainMessages.HasWarningMessages;
        }

        /// <summary>
        /// Check the application is ready to submit to credit
        ///
        /// Then run the common application in order rules: bool IsApplicationInOrderCommon(int ApplicationKey)
        /// 1. Validate the application DAO and rules
        /// 2. Validate all the client role legal entity DAO's and rules
        /// 3. Check the process related rule sets
        /// 4. Update the document Checklist
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        public bool IsApplicationInOrder(int applicationKey)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            IApplicationMortgageLoan application = GetApplicationByKey(applicationKey) as IApplicationMortgageLoan;

            if (application == null)
                return false;

            if (!IsApplicationInOrderCommon(application, RuleSets.ApplicationManagementApplicationInOrder))
                return false;

            if (!CheckValuationRequiredRules(application))
                return false;

            //All of the above rules have completed without any rule errors.
            //This method should return the result of Warning messages, so that
            //the ignoreWarnings property is managed by user interaction

            // if the user has selected ignore warnings, no warning rules will be run
            return !spc.DomainMessages.HasWarningMessages;
        }

        private bool CheckValuationRequiredRules(IApplication application)
        {
            bool requireValuation = true;

            if (!GetApplicationManagementData(application.Key, out requireValuation)) // no x2 data exists, problems here
                return false;

            if (requireValuation)
            {
                return RunApplicationRuleSet(application, RuleSets.ApplicationManagementValuationRequired);
            }

            return true;
        }

        private static bool RunApplicationRuleSet(IApplication application, string ruleSet)
        {
            ISAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IRuleService svc = ServiceFactory.GetService<IRuleService>();

            svc.ExecuteRuleSet(spc.DomainMessages, ruleSet, new object[] { application });

            // return the result of running all the rules
            return !spc.DomainMessages.HasErrorMessages;
        }

        private bool GetApplicationManagementData(int applicationKey, out bool requireValuation)
        {
            Int64 instanceID;
            bool isFL;

            return GetApplicationManagementData(applicationKey, out instanceID, out requireValuation, out isFL);
        }

        private bool GetApplicationManagementData(int applicationKey, out Int64 instanceID, out bool requireValuation, out bool isFL)
        {
            instanceID = -1;
            requireValuation = true;
            isFL = false;

            DataRow dr = x2Repository.GetApplicationManagementDataForApplicationKey(applicationKey);
            if (dr == null)
                return false;

            instanceID = Int64.Parse(dr["InstanceID"].ToString());
            isFL = bool.Parse(dr["IsFL"].ToString());
            requireValuation = bool.Parse(dr["RequireValuation"].ToString());

            return true;
        }

        public bool IsApplicationRolesValid(IApplication application)
        {
            bool retVal = application.ApplicationRoles.Where(x => x.ApplicationRoleType.Key == (int)SAHL.Common.Globals.OfferRoleTypes.MainApplicant
                                                                  || x.ApplicationRoleType.Key == (int)SAHL.Common.Globals.OfferRoleTypes.Suretor)
                                                      .All(x => x.LegalEntity.ValidateEntity() == true);
            return retVal;
        }

        public bool RefreshApplicationDocumentCheckList(int ApplicationKey)
        {
            IDocumentCheckListService documentCheckListService = ServiceFactory.GetService<IDocumentCheckListService>();
            documentCheckListService.GetList(ApplicationKey);

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            return !spc.DomainMessages.HasErrorMessages;
        }

        public IExternalRoleDomicilium CreateEmptyExternalRoleDomicilium()
        {
            return base.CreateEmpty<IExternalRoleDomicilium, ExternalRoleDomicilium_DAO>();
        }

        public void ActivatePendingDomiciliumAddress(int ApplicationKey)
        {
            ParameterCollection parameters = new ParameterCollection();

            parameters.Add(new SqlParameter("@OfferKey", ApplicationKey));

            this.castleTransactionsService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.ApplicationRepository", "ProcessPendingDomiciliumAddress", parameters);
        }

        public void CreatePersonalLoanWorkflowCase(int applicationKey)
        {
            IX2Service X2Service = ServiceFactory.GetService<IX2Service>();

            try
            {
                // once we have an application create a workflow case
                Dictionary<string, string> Inputs = new Dictionary<string, string>();
                Inputs.Add("ApplicationKey", applicationKey.ToString());

                var currentPrincipal = SAHLPrincipal.GetCurrent();

                var currentX2ActivityInfo = X2Service.GetX2Info(currentPrincipal);
                if (currentX2ActivityInfo == null || String.IsNullOrEmpty(currentX2ActivityInfo.SessionID))
                    X2Service.LogIn(currentPrincipal);

                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(currentPrincipal);

                X2Service.CreateWorkFlowInstance(currentPrincipal,
                                                    SAHL.Common.Constants.WorkFlowProcessName.PersonalLoan,
                                                    (-1).ToString(),
                                                    SAHL.Common.Constants.WorkFlowName.PersonalLoans,
                                                    SAHL.Common.Constants.WorkFlowActivityName.BatchCreatePersonalLoanLead,
                                                    Inputs,
                                                    spc.IgnoreWarnings);

                X2Service.CreateCompleteActivity(currentPrincipal, null, false, null);
            }
            catch
            {
                throw;
            }
        }

        public string GeneratePasswordFromAccountNumber(int accountNumber)
        {
            ParameterCollection prms = new ParameterCollection();
            Helper.AddIntParameter(prms, "@AccountKey", accountNumber);
            string uiStatement = uiStatementService.GetStatement("Repositories.ApplicationRepository", "HashPasswordForMyNewHomeGuide");
            object password = this.castleTransactionsService.ExecuteScalarOnCastleTran(uiStatement, typeof(GeneralStatus_DAO), prms);
            return password.ToString();
        }

        #endregion other

        #region Capitec Create Applications

        private bool CapitecCreateExternalActivity(int applicationkey)
        {
            IActiveExternalActivity activity = x2Repository.CreateActiveExternalActivity();
            activity.ActivatingInstanceID = -1;
            activity.ActivationTime = DateTime.Now;
            activity.ActivityXMLData = string.Format("<FieldInputs><FieldInput Name = \"ApplicationKey\">{0}</FieldInput></FieldInputs>", applicationkey);

            IWorkFlow wf = x2Repository.GetWorkFlowByName(SAHL.Common.Constants.WorkFlowName.ApplicationCapture, SAHL.Common.Constants.WorkFlowProcessName.Origination);
            activity.ExternalActivity = x2Repository.GetExternalActivityByName(SAHL.Common.Constants.WorkFlowExternalActivity.EXTCreateCapitecInstance, wf.ID);
            activity.WorkFlowID = wf.ID;
            activity.WorkFlowProviderName = "";

            x2Repository.SaveActiveExternalActivity(activity);

            return true;
        }

        public IApplication CapitecCreateApplicationWithKey(int reservedApplicationKey)
        {
            ParameterCollection prms = new ParameterCollection();
            Helper.AddIntParameter(prms, "@ReservedApplicationKey", reservedApplicationKey);

            string uiStatement = uiStatementService.GetStatement("Repositories.ApplicationRepository", "CreateApplicationWithKey");
            this.castleTransactionsService.ExecuteNonQueryOnCastleTran(uiStatement, typeof(GeneralStatus_DAO), prms);

            return GetApplicationByKey(reservedApplicationKey);
        }

        private void CapitecAddLegalEntityEmployment(ApplicantEmploymentDetails leEmploymentDetails, ILegalEntity lenp, IADUser adUser)
        {
            ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            IEmploymentRepository employmentRepository = RepositoryFactory.GetRepository<IEmploymentRepository>();

            IEmploymentType employmentType = lookupRepository.EmploymentTypes.ObjectDictionary[leEmploymentDetails.EmploymentTypeKey.ToString()];
            IEmployment employment = employmentRepository.GetEmptyEmploymentByType(employmentType);

            employment.BasicIncome = 0D;
            if (leEmploymentDetails.Employment != null && leEmploymentDetails.Employment.BasicMonthlyIncome != null)
                employment.BasicIncome = (double)leEmploymentDetails.Employment.BasicMonthlyIncome;

            employment.ChangeDate = DateTime.Now;
            employment.EmploymentStartDate = DateTime.Now.AddDays(-1);  //This is a fudge - required in the Database and we dont have it. Have to set to 1 day ago so emplyoment start date rule passes
            employment.LegalEntity = lenp;
            employment.EmploymentStatus = lookupRepository.EmploymentStatuses.ObjectDictionary[((int)EmploymentStatuses.Current).ToString()];
            employment.RemunerationType = lookupRepository.RemunerationTypes.ObjectDictionary[((int)RemunerationTypes.Salaried).ToString()];
            employment.UserID = adUser.ADUserName;
            employment.Employer = employmentRepository.GetEmployerByKey(1); // set this to unknown

            //set unemployed first, so no remuneration gets set for unemployed
            if (leEmploymentDetails.EmploymentTypeKey == (int)EmploymentTypes.Unemployed)
            {
                employment.RemunerationType = lookupRepository.RemunerationTypes.ObjectDictionary[((int)RemunerationTypes.Unknown).ToString()];
            }
            else if (leEmploymentDetails.Employment is SalariedWithHousingAllowanceDetails)
            {
                //add subsidy to basic income as we have no subsidy provider
                employment.BasicIncome += (double)((SalariedWithHousingAllowanceDetails)leEmploymentDetails.Employment).HousingAllowance;
            }
            else if (leEmploymentDetails.Employment is SalariedWithCommissionDetails)
            {
                employment.RemunerationType = lookupRepository.RemunerationTypes.ObjectDictionary[((int)RemunerationTypes.BasicAndCommission).ToString()];
                employment.ExtendedEmployment.Commission = (double)((SalariedWithCommissionDetails)leEmploymentDetails.Employment).ThreeMonthAverageCommission;
            }
            else if (leEmploymentDetails.Employment is SelfEmployedDetails)
            {
                employment.RemunerationType = lookupRepository.RemunerationTypes.ObjectDictionary[((int)RemunerationTypes.Drawings).ToString()];
            }

            employmentRepository.SaveEmployment(employment);
        }

        /// <summary>
        ///
        /// </summary>
        protected ILegalEntity CapitecCreateOrUpdateLegalEntity(ApplicantInformation applicantInfo, bool marriedInCommunityOfProperty, IADUser adUser)
        {
            ILegalEntityNaturalPerson lenp = null;

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            ICommonRepository commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();

            ILegalEntityNaturalPerson existingLegalEntity = legalEntityRepository.GetNaturalPersonByIDNumber(applicantInfo.IdentityNumber);
            if (existingLegalEntity != null)
            {
                lenp = existingLegalEntity;
                //save of an existing legalentity will fail if any of these are empty: Salutation, FirstNames, Surname, DateOfBirth
                if (lenp.Salutation == null || String.IsNullOrEmpty(lenp.FirstNames) || String.IsNullOrEmpty(lenp.Surname) || lenp.DateOfBirth == null)
                {
                    if (!legalEntityRepository.HasNonLeadRoles(lenp)) // update the missing details passed from capitec
                    {
                        lenp.Salutation = lookupRepository.Salutations.ObjectDictionary[Convert.ToString(applicantInfo.SalutationTypeKey)];
                        lenp.DateOfBirth = applicantInfo.DateOfBirth;
                        lenp.FirstNames = applicantInfo.FirstName;
                        lenp.Surname = applicantInfo.Surname;
                    }
                }
            }
            else
            {
                // if this person does not exist on our db then use details passed from capitec
                lenp = legalEntityRepository.GetEmptyLegalEntity(LegalEntityTypes.NaturalPerson) as ILegalEntityNaturalPerson;
                lenp.IntroductionDate = DateTime.Now;
                lenp.DocumentLanguage = commonRepo.GetLanguageByKey((int)Languages.English);
                lenp.Salutation = lookupRepository.Salutations.ObjectDictionary[Convert.ToString(applicantInfo.SalutationTypeKey)];
                lenp.DateOfBirth = applicantInfo.DateOfBirth;
                lenp.FirstNames = applicantInfo.FirstName;
                lenp.Surname = applicantInfo.Surname;
                lenp.IDNumber = applicantInfo.IdentityNumber;
                //Client has answered COP, is a natural person and does not yet exist in SAHL
                if (marriedInCommunityOfProperty) lenp.MaritalStatus = lookupRepository.MaritalStatuses.ObjectDictionary[Convert.ToString((int)MaritalStatuses.MarriedCommunityofProperty)];
            }

            // set the following values for all legalentities
            lenp.LegalEntityStatus = lookupRepository.LegalEntityStatuses.ObjectDictionary[Convert.ToString((int)LegalEntityStatuses.Alive)];
            lenp.UserID = adUser.ADUserName;
            lenp.ChangeDate = DateTime.Now;

            if (!String.IsNullOrEmpty(applicantInfo.EmailAddress)) lenp.EmailAddress = applicantInfo.EmailAddress;
            if (!String.IsNullOrEmpty(applicantInfo.CellPhoneNumber)) lenp.CellPhoneNumber = applicantInfo.CellPhoneNumber;

            var cleanedWorkPhone = (String.IsNullOrEmpty(applicantInfo.WorkPhoneNumber) ? "" : applicantInfo.WorkPhoneNumber).Trim().Replace(" ", "");
            if (cleanedWorkPhone.Length > 9)
            {
                lenp.WorkPhoneCode = cleanedWorkPhone.Substring(0, 3);
                lenp.WorkPhoneNumber = cleanedWorkPhone.Substring(3);
            }

            var cleanedHomePhone = (String.IsNullOrEmpty(applicantInfo.HomePhoneNumber) ? "" : applicantInfo.HomePhoneNumber).Trim().Replace(" ", "");
            if (cleanedHomePhone.Length > 9)
            {
                lenp.HomePhoneCode = cleanedHomePhone.Substring(0, 3);
                lenp.HomePhoneNumber = cleanedHomePhone.Substring(3);
            }

            legalEntityRepository.SaveLegalEntity(lenp, false);
            return lenp;
        }

        // Adapted from SetupVariableInformation(IApplicationInformationVariableLoan aivl, ILeadInput leadInput)
        private void CapitecSetupVariableInformation(IApplicationInformationVariableLoan aivl, MortgageLoanPurposes mortgageLoanPurpose, double loanAmountRequired, double deposit, EmploymentTypes employmentType, double currentLoan, double houseHoldIncome, double propertyValue, int term, double cashOut)
        {
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            if (aivl != null)
            {
                aivl.BondToRegister = Common.BusinessModel.Helpers.LoanCalculator.CalculateBondAmount(loanAmountRequired);
                aivl.CashDeposit = deposit;
                aivl.EmploymentType = lookupRepo.EmploymentTypes.ObjectDictionary[((int)employmentType).ToString()];
                aivl.ExistingLoan = currentLoan; //this should always be 0 for a new purchase loan

                aivl.HouseholdIncome = houseHoldIncome;
                aivl.PropertyValuation = propertyValue;
                aivl.Term = term;
            }

            if (mortgageLoanPurpose == MortgageLoanPurposes.Switchloan || mortgageLoanPurpose == MortgageLoanPurposes.Refinance)
            {
                IApplicationInformationVariableLoanForSwitchAndRefinance aivlsr = (IApplicationInformationVariableLoanForSwitchAndRefinance)aivl;
                if (aivlsr != null)
                    aivlsr.RequestedCashAmount = cashOut;
            }
        }

        // Adapted from SetupNVL(IApplicationProduct prod, ILeadInput leadInput)
        private IApplicationProductNewVariableLoan CapitecSetupNVL(IApplicationProduct prod, MortgageLoanPurposes mortgageLoanPurpose, double loanAmountRequired, double deposit, EmploymentTypes employmentType, double currentLoan, double houseHoldIncome, double propertyValue, int term, double cashOut)
        {
            ISPVService spvService = ServiceFactory.GetService<ISPVService>();
            IApplicationProductNewVariableLoan nvl = prod as IApplicationProductNewVariableLoan;
            if (null != nvl)
            {
                IApplicationMortgageLoanNewPurchase npml = nvl.Application as IApplicationMortgageLoanNewPurchase;
                if (npml != null)
                {
                    npml.PurchasePrice = propertyValue;
                }
                //nvl.LoanAmountNoFees = leadInput.LoanAmountRequired - (leadInput.CapitaliseFees ? leadInput.TotalFee : 0D); //TODO get rid of the fees here
                nvl.Term = term;

                CapitecSetupVariableInformation(nvl.VariableLoanInformation, mortgageLoanPurpose, loanAmountRequired, deposit, employmentType, currentLoan, houseHoldIncome, propertyValue, term, cashOut);
                spvService.DetermineSPVOnApplication(nvl.Application);
                //SetupInterestOnly(nvl.InterestOnlyInformation, leadInput); // Not for Capitec
            }
            return nvl;
        }

        // Adapted from SetupApplication(int offerKey, ILeadInput leadInput)
        private IApplication CapitecSetupApplication(IApplication app, MortgageLoanPurposes mortgageLoanPurpose, int productKey, bool capitaliseFees, int term, double loanAmountRequired, double propertyValue, double deposit, double currentLoanAmount, double cashOut, int numberOfApplicants, EmploymentTypes employmentType, double householdIncome)
        {
            #region ApplicationCreateDefaults

            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            var os = GetOriginationSource(OriginationSources.SAHomeLoans);
            var resetConfig = GetApplicationDefaultResetConfiguration();
            var statusOpen = lookupRepo.ApplicationStatuses.ObjectDictionary[((int)OfferStatuses.Open).ToString()];
            //var marketRateKey = (int)MarketRates.ThreeMonthJIBARRounded;

            #endregion ApplicationCreateDefaults

            switch (mortgageLoanPurpose)
            {
                case MortgageLoanPurposes.Refinance:
                    app = CreateRefinanceApplication(os, (ProductsRefinanceAtCreation)productKey, (IApplicationUnknown)app);
                    IApplicationMortgageLoanRefinance ar = app as IApplicationMortgageLoanRefinance;
                    ar.CapitaliseFees = capitaliseFees;
                    //ar.ClientEstimatePropertyValuation = leadInput.EstimatedPropertyValue;

                    break;

                case MortgageLoanPurposes.Switchloan:
                    app = CreateSwitchLoanApplication(os, (ProductsSwitchLoanAtCreation)productKey, (IApplicationUnknown)app);
                    IApplicationMortgageLoanSwitch asw = app as IApplicationMortgageLoanSwitch;
                    asw.CapitaliseFees = capitaliseFees;
                    break;

                default: //Defaults to new purchase
                    app = CreateNewPurchaseApplication(os, (ProductsNewPurchaseAtCreation)productKey, (IApplicationUnknown)app);
                    IApplicationMortgageLoanNewPurchase anp = app as IApplicationMortgageLoanNewPurchase;
                    anp.PurchasePrice = propertyValue - deposit;
                    break;
            }

            //this must be done after the application type has been created, the unknown app is evicted from memory, so any properties that have been set will be lost.
            app.ApplicationSource = GetApplicationSourceByKey((int)OfferSources.Capitec);
            app.EstimateNumberApplicants = numberOfApplicants;

            //do the product specific stuff
            switch (productKey)
            {
                //we no longer originate SuperLo
                //case (int)Products.SuperLo:
                //    SetupSuperLo(app.CurrentProduct);
                //    break;
                //case (int)Products.VariFixLoan:
                //    SetupVariFix(app.CurrentProduct, leadInput);
                //    break;
                //Web Calculators can not originat Edge
                //case (int)Products.Edge:
                //    SetupEHL(app.CurrentProduct);
                //    break;
                default: //case (int)Products.NewVariableLoan:
                    CapitecSetupNVL(app.CurrentProduct, mortgageLoanPurpose, loanAmountRequired, deposit, employmentType, currentLoanAmount, householdIncome, propertyValue, term, cashOut);
                    break;
            }

            //Mortgage Loan details
            IApplicationMortgageLoan appML = (IApplicationMortgageLoan)app;
            appML.ApplicationStartDate = DateTime.Now;
            appML.ApplicationStatus = statusOpen;
            appML.ClientEstimatePropertyValuation = propertyValue;
            appML.ResetConfiguration = resetConfig;

            appML.CalculateApplicationDetail(false, false);

            return app;
        }

        public void CreateCapitecApplication(NewPurchaseApplication newPurchaseApplication)
        {
            IApplication application = CapitecCreateApplicationWithKey(newPurchaseApplication.ReservedApplicationKey);

            var loanDetails = newPurchaseApplication.NewPurchaseLoanDetails;

            application = CapitecSetupApplication(application,
                                    MortgageLoanPurposes.Newpurchase,
                                    (int)Products.NewVariableLoan,
                                    loanDetails.CapitaliseFees,                                     // Capitalise Fees
                                    loanDetails.Term,                                               // Term
                                    (double)(loanDetails.PurchasePrice - loanDetails.Deposit),     // Loan Amount
                                    (double)loanDetails.PurchasePrice,                             // Property Value
                                    (double)loanDetails.Deposit,                                   // Deposit
                                    0,                                                              // Current Loan Amount
                                    0,                                                              // Cash Out
                                    newPurchaseApplication.Applicants.Count(),                      // Number of Applicants
                                    (EmploymentTypes)newPurchaseApplication.EmploymentTypeKey,      // Employment Type
                                    (double)loanDetails.HouseholdIncome);                           // Household Income

            CreateCapitecApplication(application, newPurchaseApplication.Applicants, newPurchaseApplication.ConsultantDetails, newPurchaseApplication.ApplicationStatusKey, newPurchaseApplication.Messages);
        }

        public void CreateCapitecApplication(SwitchLoanApplication switchApplication)
        {
            IApplication application = CapitecCreateApplicationWithKey(switchApplication.ReservedApplicationKey);

            var loanDetails = switchApplication.SwitchLoanDetails;

            application = CapitecSetupApplication(application,
                                    loanDetails.CurrentBalance > 0 ? MortgageLoanPurposes.Switchloan : MortgageLoanPurposes.Refinance, // if current balance has been captured as 0 on captic website then we want to create a refinance application fb2752
                                    (int)Products.NewVariableLoan,
                                    loanDetails.CapitaliseFees,                                         // Capitalise Fees
                                    loanDetails.Term,                                                   // Term
                                    (double)(loanDetails.CashRequired + loanDetails.CurrentBalance),    // Loan Amount
                                    (double)loanDetails.EstimatedMarketValueOfTheHome,                  // Property Value
                                    0,                                                                  // Deposit
                                    (double)loanDetails.CurrentBalance,                                 // Current Loan Amount
                                    (double)loanDetails.CashRequired,                                   // Cash Out
                                    switchApplication.Applicants.Count(),                               // Number of Applicants
                                    (EmploymentTypes)switchApplication.EmploymentTypeKey,               // Employment Type
                                    (double)loanDetails.HouseholdIncome);                               // Household Income

            CreateCapitecApplication(application, switchApplication.Applicants, switchApplication.ConsultantDetails, switchApplication.ApplicationStatusKey, switchApplication.Messages);
        }

        private void CreateCapitecApplication(IApplication application, IEnumerable<Applicant> applicants, ConsultantDetails consultantDetails, int applicationStatusKey, IList<string> messages)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.ExclusionSets.Add(RuleExclusionSets.LegalEntityExcludeAll);

            var adUser = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetAdUserForAdUserName(@"SAHL\WebLeads");

            // add a capitec offer attribute to the offer
            AddCapitecApplicationAttribute(spc, application);
            foreach (var applicant in applicants)
            {
                var applicantInfo = applicant.Information;

                var legalEntity = CapitecCreateOrUpdateLegalEntity(applicant.Information, applicant.Declarations.MarriedInCommunityOfProperty, adUser);

                SaveApplicantAddress(applicant.ResidentialAddress, legalEntity);

                CapitecAddLegalEntityEmployment(applicant.EmploymentDetails, legalEntity, adUser);

                AddCapitecApplicationRoles(spc, application, applicant, applicantInfo, legalEntity);

                if (applicant.ITC != null && !String.IsNullOrEmpty(applicant.ITC.Response))
                {
                    SaveCapitecApplicantITC(adUser.ADUserName, application, applicant.ITC, legalEntity);
                }
            }

            SaveCapitecConsultant(consultantDetails, application.Key);

            var appML = application as IApplicationMortgageLoan;
            SaveApplication(application);//apply the adjustments required for pricing for risk
            if (appML != null)
                appML.PricingForRisk();

            // The offer status must be set last
            application.ApplicationStatus = lookupRepository.ApplicationStatuses.ObjectDictionary[applicationStatusKey.ToString()];
            application.ApplicationEndDate = (applicationStatusKey == (int)OfferStatuses.Declined) ? application.ApplicationStartDate : null;

            SaveCapitecBranchDeclineReasons(messages, application.GetLatestApplicationInformation().Key);

            SaveApplication(application);

            spc.ExclusionSets.Remove(RuleExclusionSets.LegalEntityExcludeAll);

            // Fire X2 Create Case
            CapitecCreateExternalActivity(application.Key);
        }

        private void SaveCapitecBranchDeclineReasons(IList<string> Messages, int applicationInformationKey)
        {
            if (Messages.Count() == 0)
                return;

            var capitecReasonDefinitions = reasonRepository.GetReasonDefinitionsByReasonTypeKey((int)ReasonTypes.CapitecBranchDecline);

            foreach (var message in Messages.Distinct())
            {
                var reasonDefinition = capitecReasonDefinitions.Where(x => x.ReasonDescription.Description == message).FirstOrDefault();
                if (reasonDefinition == null)
                {
                    reasonDefinition = reasonRepository.AddNewReasonDefinition(message, ReasonTypes.CapitecBranchDecline);
                }

                //add reason
                IReason reason = reasonRepository.CreateEmptyReason();
                reason.GenericKey = applicationInformationKey;
                reason.ReasonDefinition = reasonDefinition;
                reasonRepository.SaveReason(reason);
            }
        }

        private void AddCapitecApplicationRoles(SAHLPrincipalCache spc, IApplication application, Applicant applicant, ApplicantInformation applicantInfo, ILegalEntity legalEntity)
        {
            var applicationRole = application.AddRole((int)Lead_MortgageLoan_Role.LEAD_MAIN_APPLICANT, legalEntity);
            if (applicantInfo.MainContact)
            {
                IApplicationRoleAttribute applicationRoleAttribute = GetEmptyApplicationRoleAttribute();
                applicationRoleAttribute.OfferRole = applicationRole;
                applicationRoleAttribute.OfferRoleAttributeType = GetApplicationRoleAttributeTypeByKey((int)SAHL.Common.Globals.OfferRoleAttributeTypes.MainContact);
                applicationRole.ApplicationRoleAttributes.Add(spc.DomainMessages, applicationRoleAttribute);
            }
            if (applicant.Declarations.IncomeContributor)
            {
                IApplicationRoleAttribute applicationRoleAttribute = GetEmptyApplicationRoleAttribute();
                applicationRoleAttribute.OfferRole = applicationRole;
                applicationRoleAttribute.OfferRoleAttributeType = GetApplicationRoleAttributeTypeByKey((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor);
                applicationRole.ApplicationRoleAttributes.Add(spc.DomainMessages, applicationRoleAttribute);
            }
        }

        private void SaveCapitecApplicantITC(string adUserName, IApplication application, ApplicantITC capitecITC, ILegalEntity legalEntity)
        {
            var itc = itcRepository.GetEmptyITC();
            itc.ChangeDate = capitecITC.ITCDate;
            itc.LegalEntity = legalEntity;
            itc.RequestXML = !String.IsNullOrEmpty(capitecITC.Request) ? capitecITC.Request : "";
            itc.ReservedAccount = application.ReservedAccount;
            itc.ResponseStatus = "Success";
            itc.UserID = adUserName;
            itc.ResponseXML = !String.IsNullOrEmpty(capitecITC.Response) ? capitecITC.Response : "";

            itcRepository.SaveITC(itc);
        }

        private void SaveCapitecConsultant(ConsultantDetails consultantDetails, int applicationKey)
        {
            var applicationCapitecDetail = CreateEmptyApplicationCapitecDetail();
            applicationCapitecDetail.ApplicationKey = applicationKey;
            applicationCapitecDetail.Branch = consultantDetails.Branch;
            applicationCapitecDetail.Consultant = consultantDetails.Name;
            SaveApplicationCapitecDetail(applicationCapitecDetail);
        }

        private void AddCapitecApplicationAttribute(SAHLPrincipalCache spc, IApplication application)
        {
            var applicationAttribute = GetEmptyApplicationAttribute();
            applicationAttribute.ApplicationAttributeType = lookupRepository.ApplicationAttributesTypes.ObjectDictionary[Convert.ToString((int)OfferAttributeTypes.CapitecLoan)];
            applicationAttribute.Application = application;
            application.ApplicationAttributes.Add(spc.DomainMessages, applicationAttribute);
        }

        private void SaveApplicantAddress(ApplicantResidentialAddress address, ILegalEntity legalEntity)
        {
            IAddressType addressType = addressRepository.GetAddressTypeByKey((int)AddressTypes.Residential);
            IAddressStreet addressStreet = addressRepository.GetEmptyAddress(typeof(IAddressStreet)) as IAddressStreet;
            addressStreet.BuildingName = address.BuildingName;
            addressStreet.BuildingNumber = address.BuildingNumber;
            addressStreet.StreetName = address.StreetName;
            addressStreet.StreetNumber = address.StreetNumber;
            addressStreet.Suburb = addressRepository.GetSuburbByKey(address.SuburbKey);
            addressStreet.UnitNumber = address.UnitNumber;
            addressStreet.ChangeDate = DateTime.Now;
            addressStreet.UserID = @"SAHL\WebLeads";

            legalEntityRepository.SaveAddress(addressType, legalEntity, addressStreet, DateTime.Now);
        }

        public IApplicationCapitecDetail CreateEmptyApplicationCapitecDetail()
        {
            return base.CreateEmpty<IApplicationCapitecDetail, ApplicationCapitecDetail_DAO>();
        }

        public IApplicationCapitecDetail GetApplicationCapitecDetail(int offerKey)
        {
            string HQL = "select ac from ApplicationCapitecDetail_DAO ac where ac.ApplicationKey = ?";
            SimpleQuery<ApplicationCapitecDetail_DAO> q = new SimpleQuery<ApplicationCapitecDetail_DAO>(HQL, offerKey);
            ApplicationCapitecDetail_DAO[] res = q.Execute();
            if (res.Length > 0)
                return (new ApplicationCapitecDetail(res[0]));
            else
                return null;
        }

        public void SaveApplicationCapitecDetail(IApplicationCapitecDetail appCapitec)
        {
            base.Save<IApplicationCapitecDetail, ApplicationCapitecDetail_DAO>(appCapitec);
        }

        public void DeleteApplicationCapitecDetail(int offerKey)
        {
            string query = string.Format("OfferKey = {0}", offerKey);

            // remove the ApplicationCapitecDetail records
            ApplicationCapitecDetail_DAO.DeleteAll(query);
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        #endregion Capitec Create Applications

        public void SaveComcorpLiveRepyMemo(int offerKey, string comment)
        {
            //do the memo insert
            var memo = memoRepository.CreateMemo();

            var adUser = organisationStructureRepository.GetAdUserForAdUserName(SAHLPrincipal.GetCurrent().Identity.Name);
            if (adUser == null)
                adUser = organisationStructureRepository.GetAdUserForAdUserName("System");

            memo.ADUser = adUser;
            memo.Description = comment;
            memo.GeneralStatus = lookupRepository.GeneralStatuses[GeneralStatuses.Active];
            memo.GenericKey = offerKey;
            memo.GenericKeyType = lookupRepository.GenericKeyType.ObjectDictionary[Convert.ToString((int)GenericKeyTypes.Offer)];
            memo.InsertedDate = DateTime.Now;

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.ExclusionSets.Add(RuleExclusionSets.MemoMandatoryDateExclude);
            memoRepository.SaveMemo(memo);
            spc.ExclusionSets.Remove(RuleExclusionSets.MemoMandatoryDateExclude);
        }

        public bool DoesApplicationRequire30YearLoanTermConversion(IApplication application)
        {
            if (application.HasAttribute(OfferAttributeTypes.Loanwith30YearTerm))
            {
                return false;
            }

            var latestAcceptedAppInformation = application.ApplicationInformations.Where(x =>
                    x.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer).OrderByDescending(y => y.Key).FirstOrDefault();

            var latestAcceptedAppInformationPriorToResub = application.ApplicationInformations.Where(x =>
                x.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer && x.Key != latestAcceptedAppInformation.Key)
                .OrderByDescending(y => y.Key).FirstOrDefault();

            var appFinancialAdjustment = latestAcceptedAppInformationPriorToResub.ApplicationInformationFinancialAdjustments.Where(x =>
                    x.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.Loanwith30YearTerm).FirstOrDefault();

            return (appFinancialAdjustment != null ? true : false);
        }

        public IApplicationInformation GetLatestAcceptedOfferInformationForApplication(int applicationKey)
        {
            // get the application
            IApplication application = this.GetApplicationByKey(applicationKey);

            // get the latest accepted offer information
            var latestAcceptedOfferInformation = application.ApplicationInformations.Where(x => x.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                                                                        .OrderByDescending(x => x.ApplicationInsertDate)
                                                                        .FirstOrDefault();

            return latestAcceptedOfferInformation;
        }

        public bool Display20YearFiguresOn30YearLoan(IAccount account)
        {
            bool show20YearFigures = false;
            IMortgageLoanAccount mortgageLoanAccount = account as IMortgageLoanAccount;
            if (mortgageLoanAccount != null && mortgageLoanAccount.IsThirtyYearTerm)
            {
                IMortgageLoan mortgageLoan = mortgageLoanAccount.FinancialServices.Where(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan).FirstOrDefault() as IMortgageLoan;
                if (mortgageLoan != null && mortgageLoan.RemainingInstallments > 240)
                    show20YearFigures = true;
            }

            return show20YearFigures;
        }

        public void DetermineGEPFAttribute(IApplication application)
        {
            if (application.ApplicationType != null &&
                (application.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan ||
                application.ApplicationType.Key == (int)OfferTypes.RefinanceLoan ||
                application.ApplicationType.Key == (int)OfferTypes.SwitchLoan))
            {
                bool shouldAddAttribute = false;
                if (application.HasAttribute(OfferAttributeTypes.DisqualifiedforGEPF))
                {
                    RemoveApplicationAttributeIfExists(application, OfferAttributeTypes.GovernmentEmployeePensionFund);
                    return;
                }

                ISupportsVariableLoanApplicationInformation vlAppInfo = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                if (vlAppInfo != null &&
                    vlAppInfo.VariableLoanInformation != null &&
                    vlAppInfo.VariableLoanInformation.EmploymentType != null &&
                    vlAppInfo.VariableLoanInformation.EmploymentType.Key == ((int)EmploymentTypes.SalariedwithDeduction))
                {
                    var mainApplicants = new List<IApplicationRole>(application.GetApplicationRolesByType(OfferRoleTypes.LeadMainApplicant, GeneralStatuses.Active));
                    mainApplicants.AddRange(application.GetApplicationRolesByType(OfferRoleTypes.MainApplicant, GeneralStatuses.Active));

                    foreach (var subsidy in application.Subsidies)
                    {
                        if (subsidy.GeneralStatus.Key != (int)GeneralStatuses.Active)
                        {
                            continue;
                        }

                        bool subsidyLegalEntityIsMainApplicant = false;
                        bool providerIsGEPF = false;
                        bool applicantIsGEPFMember = false;
                        bool workingForMoreThanAYear = false;

                        subsidyLegalEntityIsMainApplicant = mainApplicants.Any(x => x.LegalEntityKey == subsidy.LegalEntity.Key);
                        providerIsGEPF = subsidy.SubsidyProvider.GEPFAffiliate;
                        applicantIsGEPFMember = subsidy.GEPFMember;

                        if (subsidy.Employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current && subsidy.Employment.EmploymentStartDate.HasValue)
                        {
                            var dateOneYearPostStart = subsidy.Employment.EmploymentStartDate.Value.AddYears(1);
                            workingForMoreThanAYear = DateTime.Now > dateOneYearPostStart;
                        }

                        if (subsidyLegalEntityIsMainApplicant && providerIsGEPF && applicantIsGEPFMember && workingForMoreThanAYear)
                        {
                            shouldAddAttribute = true;
                            break;
                        }
                    }

                    if (shouldAddAttribute)
                    {
                        AddApplicationAttributeIfNotExists(application, OfferAttributeTypes.GovernmentEmployeePensionFund);
                    }
                    else
                    {
                        RemoveApplicationAttributeIfExists(application, OfferAttributeTypes.GovernmentEmployeePensionFund);
                    }
                }
                else
                {
                    RemoveApplicationAttributeIfExists(application, OfferAttributeTypes.GovernmentEmployeePensionFund);
                }
            }
        }

        private void DetermineStopOrderDiscountAttribute(IApplication application)
        {
            // The application.CurrentProduct call below was falling because of there 
            // not being a LatestOfferInformation for a Personal Loans application
            if (application.GetLatestApplicationInformation() == null)
            {
                return;
            }

            bool shouldAddAttribute = false;

            ISupportsVariableLoanApplicationInformation vlAppInfo = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            if (vlAppInfo != null &&
                vlAppInfo.VariableLoanInformation != null &&
                vlAppInfo.VariableLoanInformation.EmploymentType != null &&
                vlAppInfo.VariableLoanInformation.EmploymentType.Key == ((int)EmploymentTypes.SalariedwithDeduction))
            {
                IControl control = controlRepository.GetControlByDescription(Constants.ControlTable.CreditMatrixKeyWithGEPF_Introduced);

                if (control != null &&
                    control.ControlNumeric.HasValue)
                {
                    int creditMatrixKeyWithGEPF_Introduced = int.MaxValue;

                    if (int.TryParse(control.ControlNumeric.Value.ToString(), out creditMatrixKeyWithGEPF_Introduced))
                    {
                        if (vlAppInfo.VariableLoanInformation.CreditMatrix.Key >= creditMatrixKeyWithGEPF_Introduced)
                        {
                            shouldAddAttribute = true;
                        }
                    }
                }

                if (shouldAddAttribute)
                {
                    AddApplicationAttributeIfNotExists(application, OfferAttributeTypes.StopOrderDiscount);
                }
                else
                {
                    RemoveApplicationAttributeIfExists(application, OfferAttributeTypes.StopOrderDiscount);
                }
            }
            else
            {
                RemoveApplicationAttributeIfExists(application, OfferAttributeTypes.StopOrderDiscount);
            }
        }

        public void AddApplicationAttributeIfNotExists(IApplication application, OfferAttributeTypes offerAttributeType)
        {
            if (!application.HasAttribute(offerAttributeType))
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                IApplicationAttribute applicationAttributeNew = GetEmptyApplicationAttribute();
                applicationAttributeNew.ApplicationAttributeType = lookupRepository.ApplicationAttributesTypes.ObjectDictionary[Convert.ToString((int)offerAttributeType)];
                applicationAttributeNew.Application = application;
                application.ApplicationAttributes.Add(spc.DomainMessages, applicationAttributeNew);
            }
        }

        private void RemoveApplicationAttributeIfExists(IApplication application, OfferAttributeTypes offerAttributeType)
        {
            if (application.HasAttribute(offerAttributeType))
            {
                IApplicationAttribute applicationAttribute =
                    application.ApplicationAttributes.Where(x => x.ApplicationAttributeType.Key == (int)offerAttributeType).
                    FirstOrDefault();

                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                application.ApplicationAttributes.Remove(spc.DomainMessages, applicationAttribute);
            }
        }

        public IEventList<ISPV> GetOriginatableSPVList()
        {
            string query = UIStatementRepository.GetStatement("COMMON", "GetOriginatableSPVList");
            SimpleQuery<SPV_DAO> q = new SimpleQuery<SPV_DAO>(QueryLanguage.Sql, query);
            q.AddSqlReturnDefinition(typeof(SPV_DAO), "a");
            SPV_DAO[] arr = q.Execute();
            return new DAOEventList<SPV_DAO, ISPV, SPV>(arr);
        }
    }
}