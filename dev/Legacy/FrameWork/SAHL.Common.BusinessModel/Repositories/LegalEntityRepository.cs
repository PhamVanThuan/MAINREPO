using Castle.ActiveRecord.Queries;
using NHibernate.Criterion;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(ILegalEntityRepository))]
    public class LegalEntityRepository : AbstractRepositoryBase, ILegalEntityRepository
    {
        private ICastleTransactionsService castleTransactionsService;
        private IApplicationRepository applicationRepository;

        public LegalEntityRepository()
        {
            this.castleTransactionsService = new CastleTransactionsService();
        }

        public LegalEntityRepository(ICastleTransactionsService castleTransactionsService, IApplicationRepository applicationRepository)
        {
            this.castleTransactionsService = castleTransactionsService;
            this.applicationRepository = applicationRepository;
        }

        #region ILegalEntityRepository Members

        public ILegalEntityMarketingOption GetEmptyLegalEntityMarketingOption()
        {
            return base.CreateEmpty<ILegalEntityMarketingOption, LegalEntityMarketingOption_DAO>();

            //return new LegalEntityMarketingOption(new LegalEntityMarketingOption_DAO());
        }

        public ILegalEntityBankAccount GetLegalEntityBankAccountByKey(int Key)
        {
            return base.GetByKey<ILegalEntityBankAccount, LegalEntityBankAccount_DAO>(Key);

            //LegalEntityBankAccount_DAO App = LegalEntityBankAccount_DAO.Find(Key);
            //if (App != null)
            //{
            //    return new LegalEntityBankAccount(App);
            //}
            //else
            //{
            //    return null;
            //}
        }

        /// <summary>
        /// Gets all roles on all accounts that the given legal entity has a role on.  The origination
        /// source of the current user will be taken into account and AccountStatus 6 (Application
        /// prior to Instruct Attorney) is excluded.
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="LegalEntityKey"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IRole> GetRelatedRolesByLegalEntity(SAHLPrincipal principal, int LegalEntityKey)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            string osKeys = spc.OriginationSourceKeysStringForQuery;

            string subquery = String.Format("SELECT role2.Account.Key FROM Role_DAO role2 WHERE role2.LegalEntity.Key = {0}", LegalEntityKey);
            string HQL = String.Format("SELECT DISTINCT role FROM Role_DAO role WHERE role.Account.Key IN ({0}) AND role.Account.AccountStatus.Key IN (1,2,3,4,5) AND role.Account.OriginationSource.Key IN ({1})", subquery, osKeys);

            SimpleQuery query = new SimpleQuery(typeof(SAHL.Common.BusinessModel.DAO.Role_DAO), HQL);
            Role_DAO[] roles = Role_DAO.ExecuteQuery(query) as Role_DAO[];

            if (roles == null)
                return null;

            IList<Role_DAO> list = new List<Role_DAO>(roles);
            DAOEventList<Role_DAO, IRole, Role> daoList = new DAOEventList<Role_DAO, IRole, Role>(list);
            return new ReadOnlyEventList<IRole>(daoList);
        }

        [Obsolete("This should not be used - use account.Roles collection instead.")]
        public IReadOnlyEventList<IRole> GetRelatedRolesByAccount(SAHLPrincipal principal, int AccountKey)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            string osKeys = spc.OriginationSourceKeysStringForQuery;

            string HQL = String.Format("SELECT DISTINCT role FROM Role_DAO role WHERE role.Account.Key = {0} AND role.Account.AccountStatus.Key IN (1,2,3,4,5) AND role.Account.OriginationSource.Key IN ({1})", AccountKey.ToString(), osKeys);

            SimpleQuery query = new SimpleQuery(typeof(SAHL.Common.BusinessModel.DAO.Role_DAO), HQL);
            Role_DAO[] roles = Role_DAO.ExecuteQuery(query) as Role_DAO[];

            if (roles == null)
                return null;

            IList<Role_DAO> list = new List<Role_DAO>(roles);
            DAOEventList<Role_DAO, IRole, Role> daoList = new DAOEventList<Role_DAO, IRole, Role>(list);
            return new ReadOnlyEventList<IRole>(daoList);
        }

        public IRole GetEmptyRole()
        {
            IAccountRepository repo = RepositoryFactory.GetRepository<IAccountRepository>();
            return repo.CreateEmptyRole();
        }

        public void SaveRole(IRole role)
        {
            IAccountRepository repo = RepositoryFactory.GetRepository<IAccountRepository>();
            repo.SaveRole(role);
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        public void DeactivateRole(IRole role)
        {
            role.GeneralStatus = new GeneralStatus(GeneralStatus_DAO.Find((int)SAHL.Common.Globals.GeneralStatuses.Inactive));
            role.StatusChangeDate = System.DateTime.Now;
            SaveRole(role);
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        public void ActivateRole(IRole role)
        {
            role.GeneralStatus = new GeneralStatus(GeneralStatus_DAO.Find((int)SAHL.Common.Globals.GeneralStatuses.Active));
            role.StatusChangeDate = System.DateTime.Now;
            SaveRole(role);
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// Implements <see cref="ILegalEntityRepository.DeleteAddress(ILegalEntityAddress)"/>.
        /// </summary>
        /// <param name="legalEntityAddress"></param>
        public void DeleteAddress(ILegalEntityAddress legalEntityAddress)
        {
            LegalEntityAddress_DAO daoLegalEntityAddress = (LegalEntityAddress_DAO)(legalEntityAddress as IDAOObject).GetDAOObject();
            daoLegalEntityAddress.GeneralStatus = GeneralStatus_DAO.Find((int)GeneralStatuses.Inactive);
            daoLegalEntityAddress.SaveAndFlush();
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// Implements <see cref="ILegalEntityRepository.DeleteLegalEntityBankAccount"/>.
        /// </summary>
        /// <param name="legalEntityBankAccountKey"></param>
        /// <param name="user"></param>
        public void DeleteLegalEntityBankAccount(int legalEntityBankAccountKey, SAHLPrincipal user)
        {
            LegalEntityBankAccount_DAO leba = LegalEntityBankAccount_DAO.Find(legalEntityBankAccountKey);
            leba.GeneralStatus = GeneralStatus_DAO.Find((int)GeneralStatuses.Inactive);
            leba.ChangeDate = DateTime.Now;
            leba.UserID = user.Identity.Name;
            leba.SaveAndFlush();
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// Returns an employment type for a legal entity based on whether an application has been submitted to credit.
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public double GetLegalEntityIncomeForApplication(ILegalEntity legalEntity, IApplication application)
        {
            //A dictionary to store the employment types and related income.
            Dictionary<IEmploymentType, double> dict = new Dictionary<IEmploymentType, double>();

            IStageDefinitionRepository SDR = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

            //check whether this application has been submitted to credit
            //because calculations regarding income differ accordingly.
            int cnt = SDR.CountCompositeStageOccurance(application.Key, SDR.GetStageDefinitionStageDefinitionGroup((int)StageDefinitionGroups.ApplicationCapture, (int)StageDefinitions.ApplicationCaptureSubmitted).Key);
            bool submittedToCredit = cnt > 0;

            //iterate through each employment record.
            foreach (IEmployment employment in legalEntity.Employment)
            {
                double income = 0;
                if (submittedToCredit)
                {
                    //only use confirmed income
                    income = employment.ConfirmedIncome;
                }
                else
                {
                    // use whichever income is greater - confirmed or unconfirmed.
                    if (employment.ConfirmedIncome >= employment.MonthlyIncome)
                    {
                        income = employment.ConfirmedBasicIncome.HasValue ? employment.ConfirmedBasicIncome.Value : 0;
                    }
                    else
                    {
                        income = employment.MonthlyIncome;
                    }
                }
                bool foundKey = false;

                //check whether the employment type has been added to the dict.
                //If so, add the income amount to the amount stored in the dictionary
                foreach (KeyValuePair<IEmploymentType, double> t in dict)
                {
                    if (employment.EmploymentType == t.Key)
                    {
                        foundKey = true;
                        double newVal = t.Value + income;

                        //have to remove and re-add because t is readonly
                        dict.Remove(t.Key);
                        dict.Add(employment.EmploymentType, newVal);
                        break;
                    }
                }

                //If the employment type has not yet been added to the dictionary,
                //do so here
                if (!foundKey)
                {
                    dict.Add(employment.EmploymentType, income);
                }
            }

            //Now that the dictionary has been populated,
            double totalIncome = 0;
            if (submittedToCredit)
            {
                KeyValuePair<IEmploymentType, double> highestVal = new KeyValuePair<IEmploymentType, double>();
                foreach (KeyValuePair<IEmploymentType, double> pair in dict)
                {
                    if (pair.Value > highestVal.Value)
                    {
                        highestVal = pair;
                    }
                }
                totalIncome = highestVal.Value;
            }
            else
            {
                foreach (KeyValuePair<IEmploymentType, double> pair in dict)
                {
                    totalIncome += pair.Value;
                }
            }
            return totalIncome;
        }

        /// <summary>
        /// Returns an employment type for a legal entity based on whether an application has been submitted to credit.
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        public IEmploymentType GetLegalEntityEmploymentTypeForApplication(ILegalEntity legalEntity, IApplication application)
        {
            //A dictionary to store the employment types and related income.
            Dictionary<IEmploymentType, double> dict = new Dictionary<IEmploymentType, double>();

            IStageDefinitionRepository SDR = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

            //check whether this application has been submitted to credit
            //because calculations regarding income differ accordingly.
            int cnt = SDR.CountCompositeStageOccurance(application.Key, SDR.GetStageDefinitionStageDefinitionGroup((int)StageDefinitionGroups.ApplicationCapture, (int)StageDefinitions.ApplicationCaptureSubmitted).Key);
            bool submittedToCredit = cnt > 0;

            //iterate through each employment record.
            foreach (IEmployment employment in legalEntity.Employment)
            {
                double income = 0;
                double confirmedBasicIncome = employment.ConfirmedBasicIncome.HasValue ? employment.ConfirmedBasicIncome.Value : 0;
                double basicIncome = employment.BasicIncome.HasValue ? employment.BasicIncome.Value : 0;
                if (submittedToCredit)
                {
                    //only use confirmed income
                    income = confirmedBasicIncome;
                }
                else
                {
                    //use whichever income is greater - confirmed or unconfirmed.
                    income = confirmedBasicIncome > basicIncome ? confirmedBasicIncome : basicIncome;
                }
                bool foundKey = false;

                //check whether the employment type has been added to the dict.
                //If so, add the income amount to the amount stored in the dictionary
                foreach (KeyValuePair<IEmploymentType, double> t in dict)
                {
                    if (employment.EmploymentType == t.Key)
                    {
                        foundKey = true;
                        double newVal = t.Value + income;

                        //have to remove and re-add because t is readonly
                        dict.Remove(t.Key);
                        dict.Add(employment.EmploymentType, newVal);
                        break;
                    }
                }

                //If the employment type has not yet been added to the dictionary,
                //do so here
                if (!foundKey)
                {
                    dict.Add(employment.EmploymentType, income);
                }
            }

            //Now that the dictionary has been populated,
            KeyValuePair<IEmploymentType, double> highestVal = new KeyValuePair<IEmploymentType, double>();
            foreach (KeyValuePair<IEmploymentType, double> pair in dict)
            {
                if (pair.Value > highestVal.Value)
                {
                    highestVal = pair;
                }
            }
            return highestVal.Key;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public ILegalEntityAffordability GetEmptyLegalEntityAffordability()
        {
            return base.CreateEmpty<ILegalEntityAffordability, LegalEntityAffordability_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        public ILegalEntity GetEmptyLegalEntity(LegalEntityTypes legalEntityType)
        {
            ILegalEntity legalEntity = null;

            switch (legalEntityType)
            {
                case LegalEntityTypes.NaturalPerson:
                    legalEntity = new LegalEntityNaturalPerson(new LegalEntityNaturalPerson_DAO());
                    break;

                case LegalEntityTypes.Company:
                    legalEntity = new LegalEntityCompany(new LegalEntityCompany_DAO());
                    break;

                case LegalEntityTypes.CloseCorporation:
                    legalEntity = new LegalEntityCloseCorporation(new LegalEntityCloseCorporation_DAO());
                    break;

                case LegalEntityTypes.Trust:
                    legalEntity = new LegalEntityTrust(new LegalEntityTrust_DAO());
                    break;

                default:

                    // throw some kind of error.
                    break;
            }

            return legalEntity;
        }

        /// <summary>
        /// Implements the <see cref="ILegalEntityRepository.SaveLegalEntity"/>
        /// </summary>
        public void SaveLegalEntity(ILegalEntity legalEntity, bool recalculateApplicationDetail)
        {
            // Check if the Legal Entity ID Number has changed.
            NotifyChangeOfIDNumberLinkedToLifePolicy(legalEntity);

            // reset the legalentity exception status
            if (legalEntity.LegalEntityExceptionStatus != null)
            {
                switch (legalEntity.LegalEntityExceptionStatus.Key)
                {
                    case (int)LegalEntityExceptionStatuses.InvalidIDNumber:
                    case (int)LegalEntityExceptionStatuses.DuplicateIDNumbers:
                        legalEntity.LegalEntityExceptionStatus = this.LookupRepo.LegalEntityExceptionStatuses.ObjectDictionary[Convert.ToString((int)LegalEntityExceptionStatuses.Valid)];
                        break;

                    default:
                        break;
                }
            }

            // save the legalentity
            LegalEntity_DAO daoLegalEntity = (LegalEntity_DAO)(legalEntity as IDAOObject).GetDAOObject();
            daoLegalEntity.SaveAndFlush();

            if (recalculateApplicationDetail)
            {
                // run through any applications related to the legal entity and update household income
                foreach (IApplicationRole ar in legalEntity.GetApplicationRolesByRoleTypeGroups(OfferRoleTypeGroups.Client))
                {
                    IApplicationInformation ai = ar.Application.GetLatestApplicationInformation();

                    if (ar.Application.IsOpen
                        && ai != null
                        && ai.ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                    {
                        ar.Application.CalculateHouseHoldIncome();
                        ar.Application.SetApplicantType();
                        ar.Application.SetEmploymentType();

                        // recalculate application detail
                        IApplicationMortgageLoan appML = ar.Application as IApplicationMortgageLoan;
                        if (appML != null)
                            appML.CalculateApplicationDetail(false, false);

                        ApplicationRepo.SaveApplication(ar.Application);
                    }
                }
            }

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// Implements the <see cref="ILegalEntityRepository.SaveLegalEntityWithExceptionStatus"/>
        /// </summary>
        public void SaveLegalEntityWithExceptionStatus(ILegalEntity legalEntity, LegalEntityExceptionStatuses legalEntityExceptionStatus)
        {
            // Check if the Legal Entity ID Number has changed.
            NotifyChangeOfIDNumberLinkedToLifePolicy(legalEntity);

            // set the exception status
            legalEntity.LegalEntityExceptionStatus = this.LookupRepo.LegalEntityExceptionStatuses.ObjectDictionary[Convert.ToString((int)legalEntityExceptionStatus)];

            // save the legalentity
            LegalEntity_DAO daoLegalEntity = (LegalEntity_DAO)(legalEntity as IDAOObject).GetDAOObject();
            daoLegalEntity.SaveAndFlush();

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// Implements <see cref="ILegalEntityRepository.GetLegalEntityByKey"></see>.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public ILegalEntity GetLegalEntityByKey(int Key)
        {
            return base.GetByKey<ILegalEntity, LegalEntity_DAO>(Key);

            //LegalEntity_DAO LE = LegalEntity_DAO.Find(LegalEntityKey);
            //if (LE != null)
            //{
            //    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            //    return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(LE);

            //}
            //return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="IdentityNo"></param>
        /// <returns></returns>
        public ILegalEntityNaturalPerson GetNaturalPersonByIDNumber(string IdentityNo)
        {
            ICriterion[] criterion = new ICriterion[]
            {
                Expression.Eq("IDNumber", IdentityNo)
            };
            LegalEntityNaturalPerson_DAO legalNPEntity = null;
            LegalEntity_DAO legalEntity = null;
            legalEntity = LegalEntityNaturalPerson_DAO.FindOne(criterion);

            if (legalEntity != null)
            {
                legalNPEntity = legalEntity.As<LegalEntityNaturalPerson_DAO>();
                if (legalNPEntity != null)
                {
                    return new LegalEntityNaturalPerson(legalNPEntity);
                }
            }
            return null;
        }

        /// <summary>
        /// Implements <see cref="ILegalEntityRepository.GetNaturalPersonsByIDNumber"/>
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        public IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonsByIDNumber(string prefix, int maxRowCount)
        {
            IList<LegalEntityNaturalPerson_DAO> legalEntities = LegalEntityNaturalPerson_DAO.FindByIDNumber(prefix, maxRowCount);
            DAOEventList<LegalEntityNaturalPerson_DAO, ILegalEntityNaturalPerson, LegalEntityNaturalPerson> daoList = new DAOEventList<LegalEntityNaturalPerson_DAO, ILegalEntityNaturalPerson, LegalEntityNaturalPerson>(legalEntities);
            return new ReadOnlyEventList<ILegalEntityNaturalPerson>(daoList);
        }

        /// <summary>
        /// Implements <see cref="ILegalEntityRepository.GetNaturalPersonsByPassportNumber"/>
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        public IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonsByPassportNumber(string prefix, int maxRowCount)
        {
            IList<LegalEntityNaturalPerson_DAO> legalEntities = LegalEntityNaturalPerson_DAO.FindByPassportNumber(prefix, maxRowCount);
            DAOEventList<LegalEntityNaturalPerson_DAO, ILegalEntityNaturalPerson, LegalEntityNaturalPerson> daoList = new DAOEventList<LegalEntityNaturalPerson_DAO, ILegalEntityNaturalPerson, LegalEntityNaturalPerson>(legalEntities);
            return new ReadOnlyEventList<ILegalEntityNaturalPerson>(daoList);
        }

        /// <summary>
        /// Implements <see cref="ILegalEntityRepository.GetCompaniesByRegistrationNumber"/>
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        public IReadOnlyEventList<ILegalEntityCompany> GetCompaniesByRegistrationNumber(string prefix, int maxRowCount)
        {
            IList<LegalEntityCompany_DAO> legalEntities = LegalEntityCompany_DAO.FindByRegistrationNumber(prefix, maxRowCount);
            DAOEventList<LegalEntityCompany_DAO, ILegalEntityCompany, LegalEntityCompany> daoList = new DAOEventList<LegalEntityCompany_DAO, ILegalEntityCompany, LegalEntityCompany>(legalEntities);
            return new ReadOnlyEventList<ILegalEntityCompany>(daoList);
        }

        /// <summary>
        /// Implements <see cref="ILegalEntityRepository.GetCloseCorporationsByRegistrationNumber"/>
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        public IReadOnlyEventList<ILegalEntityCloseCorporation> GetCloseCorporationsByRegistrationNumber(string prefix, int maxRowCount)
        {
            IList<LegalEntityCloseCorporation_DAO> legalEntities = LegalEntityCloseCorporation_DAO.FindByRegistrationNumber(prefix, maxRowCount);
            DAOEventList<LegalEntityCloseCorporation_DAO, ILegalEntityCloseCorporation, LegalEntityCloseCorporation> daoList = new DAOEventList<LegalEntityCloseCorporation_DAO, ILegalEntityCloseCorporation, LegalEntityCloseCorporation>(legalEntities);
            return new ReadOnlyEventList<ILegalEntityCloseCorporation>(daoList);
        }

        /// <summary>
        /// Implements <see cref="ILegalEntityRepository.GetTrustsByRegistrationNumber"/>
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        public IReadOnlyEventList<ILegalEntityTrust> GetTrustsByRegistrationNumber(string prefix, int maxRowCount)
        {
            IList<LegalEntityTrust_DAO> legalEntities = LegalEntityTrust_DAO.FindByRegistrationNumber(prefix, maxRowCount);
            DAOEventList<LegalEntityTrust_DAO, ILegalEntityTrust, LegalEntityTrust> daoList = new DAOEventList<LegalEntityTrust_DAO, ILegalEntityTrust, LegalEntityTrust>(legalEntities);
            return new ReadOnlyEventList<ILegalEntityTrust>(daoList);
        }

        /// <summary>
        /// This method is used to remove the current affordability recordset before adding a fresh one for the offer
        /// </summary>
        /// <param name="OfferKey"></param>
        /// <param name="LegalEntityKey"></param>
        public void ClearLegalEntityOfferAffordabilityData(int OfferKey, int LegalEntityKey)
        {
            const string query = "DELETE FROM [2AM].[dbo].[LegalEntityAffordability] WHERE OfferKey = @OfferKey AND LegalEntityKey = @LegalEntityKey;";

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            //Add the required parameters
            parameters.Add(new SqlParameter("@OfferKey", OfferKey));
            parameters.Add(new SqlParameter("@LegalEntityKey", LegalEntityKey));

            // execute
            castleTransactionsService.ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
        }

        /// <summary>
        /// Implements <see cref="ILegalEntityRepository.GetRegistrationNumbersForCompanies"/>
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetRegistrationNumbersForCompanies(string prefix, int maxRowCount)
        {
            IDictionary<string, string> registrationNumbers = new Dictionary<string, string>();

            string query = "select top (" + maxRowCount + ") LegalEntityKey, RegistrationNumber from [2am]..LegalEntity";
            query += " where RegistrationNumber like '" + prefix + "%'";
            query += " and LegalEntityTypeKey in (" + (int)SAHL.Common.Globals.LegalEntityTypes.Company + "," + (int)SAHL.Common.Globals.LegalEntityTypes.CloseCorporation + "," + (int)SAHL.Common.Globals.LegalEntityTypes.Trust + ")";
            query += " order by RegistrationNumber";

            DataSet dsResults = this.castleTransactionsService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), null);
            if (dsResults != null && dsResults.Tables.Count > 0)
            {
                foreach (DataRow dr in dsResults.Tables[0].Rows)
                {
                    registrationNumbers.Add(dr[0].ToString(), dr[1].ToString());
                }
            }

            return registrationNumbers;
        }

        #region Attorney Methods

        public IDictionary<int, string> GetAttorneysByDeedsOffice(int deedsOfficeKey)
        {
            IDictionary<int, string> attorneys = new Dictionary<int, string>();

            string query = @"SELECT AttorneyKey, RegisteredName
                                    FROM [2am]..Attorney ATT (nolock)
                                    JOIN [2am]..LegalEntity LE (nolock) ON ATT.LegalEntityKey = LE.LegalEntityKey
                                    WHERE ATT.DeedsOfficeKey = @DeedsOfficeKey
                                    ORDER BY RegisteredName;";
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@DeedsOfficeKey", deedsOfficeKey));

            DataSet dsResults = this.castleTransactionsService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (dsResults != null && dsResults.Tables.Count > 0)
            {
                foreach (DataRow dr in dsResults.Tables[0].Rows)
                {
                    attorneys.Add((int)dr[0], dr[1].ToString());
                }
            }

            return attorneys;
        }

        /// <summary>
        ///
        /// </summary>
        public IAttorney GetAttorneyByKey(int attorneyKey)
        {
            return base.GetByKey<IAttorney, Attorney_DAO>(attorneyKey);
        }

        /// <summary>
        ///
        /// </summary>
        public IAttorney CreateEmptyAttorney()
        {
            return base.CreateEmpty<IAttorney, Attorney_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        public void SaveAttorney(IAttorney attorney)
        {
            base.Save<IAttorney, Attorney_DAO>(attorney);
        }

        #endregion Attorney Methods

        /// <summary>
        /// Implements <see cref="ILegalEntityRepository.SaveAddress"/>
        /// </summary>
        /// <param name="addressType">The address type.</param>
        /// <param name="legalEntity"></param>
        /// <param name="address"></param>
        /// <param name="effectiveDate"></param>
        public void SaveAddress(IAddressType addressType, ILegalEntity legalEntity, IAddress address, DateTime effectiveDate)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IAddressRepository addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
            addressRepository.SaveAddress(ref address);
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

            // now we need to determine if a relationship already exists - if so then use the existing one
            ILegalEntityAddress leAddress = null;
            foreach (LegalEntityAddress lea in legalEntity.LegalEntityAddresses)
            {
                if (lea.Address.Key == address.Key && lea.AddressType.Key == addressType.Key)
                {
                    leAddress = lea;
                    break;
                }
            }

            if (leAddress == null)
            {
                // create a new legal entity address object
                leAddress = new LegalEntityAddress(new LegalEntityAddress_DAO());
                leAddress.AddressType = addressType;
                leAddress.Address = address;
                leAddress.LegalEntity = legalEntity;
                legalEntity.LegalEntityAddresses.Add(spc.DomainMessages, leAddress);
            }

            ILookupRepository lookupRep = RepositoryFactory.GetRepository<ILookupRepository>();
            leAddress.GeneralStatus = lookupRep.GeneralStatuses[GeneralStatuses.Active];
            leAddress.EffectiveDate = effectiveDate;

            SaveLegalEntity(legalEntity, false);

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// Implements <see cref="ILegalEntityRepository.SaveBankAccount"/>.
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="bankAccount"></param>
        /// <param name="principal"></param>
        public void SaveBankAccount(ILegalEntity legalEntity, IBankAccount bankAccount, SAHLPrincipal principal)
        {
            // save the bank account if it's new
            if (bankAccount.Key == 0)
            {
                BankAccount_DAO daoBankAccount = (BankAccount_DAO)(bankAccount as IDAOObject).GetDAOObject();
                daoBankAccount.SaveAndFlush();
            }

            // create a new LegalEntityBankAccount object
            LegalEntityBankAccount_DAO daoLeBankAccount = new LegalEntityBankAccount_DAO();
            ILegalEntityBankAccount leBankAccount = new LegalEntityBankAccount(daoLeBankAccount);
            leBankAccount.BankAccount = bankAccount;
            leBankAccount.ChangeDate = DateTime.Now;
            leBankAccount.LegalEntity = legalEntity;
            leBankAccount.UserID = principal.Identity.Name;

            // save the LegalEntityBankAccount object
            daoLeBankAccount.SaveAndFlush();

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// Implements <see cref="ILegalEntityRepository.SaveLegalEntityBankAccount"/>.
        /// </summary>
        /// <param name="legalEntityBankAccount"></param>
        public void SaveLegalEntityBankAccount(ILegalEntityBankAccount legalEntityBankAccount)
        {
            base.Save<ILegalEntityBankAccount, LegalEntityBankAccount_DAO>(legalEntityBankAccount);
        }

        public ILegalEntityRelationship CreateLegalEntityRelationship()
        {
            return base.CreateEmpty<ILegalEntityRelationship, LegalEntityRelationship_DAO>();

            //return new LegalEntityRelationship(new LegalEntityRelationship_DAO());
        }

        public void SaveLegalEntityRelationship(ILegalEntityRelationship ler)
        {
            base.Save<ILegalEntityRelationship, LegalEntityRelationship_DAO>(ler);

            //LegalEntityRelationship_DAO dao = (LegalEntityRelationship_DAO)(ler as IDAOObject).GetDAOObject();
            //dao.SaveAndFlush();
            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        public void DeleteLegalEntityRelationship(ILegalEntityRelationship ler)
        {
            LegalEntityRelationship_DAO dao = (LegalEntityRelationship_DAO)(ler as IDAOObject).GetDAOObject();
            dao.Delete();
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// Implements <see cref="ILegalEntityRepository.SaveLegalEntityAddress"/>.
        /// </summary>
        /// <param name="legalEntityAddress"></param>
        /// <param name="address"></param>
        public void SaveLegalEntityAddress(ILegalEntityAddress legalEntityAddress, IAddress address)
        {
            // try and save the address first, but exit out if there are errors
            if (address.Key == 0)
            {
                IAddressRepository addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
                addressRepository.SaveAddress(ref address);
                if (ValidationHelper.PrincipalHasValidationErrors())
                    throw new DomainValidationException();
            }

            // if the address key is changing on an existing legal entity address, we need to ensure it isn't
            // a mailing address on an open application or account - if it is then we need to raise a warning
            if (legalEntityAddress.Key > 0 && address.Key != legalEntityAddress.Address.Key)
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAddressRepository addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();

                // first, check the offer mailing address
                string sql = string.Format(@"select ama.*
                                from [2am].dbo.OfferMailingAddress ama (nolock)
                                join [2am].dbo.Offer o (nolock)
                                    on o.offerKey = ama.offerKey
                                join [2am].dbo.OfferRole ofr (nolock)
                                    on ama.offerKey = ofr.offerKey
                                where ama.addressKey = ? and o.OfferStatusKey = ? and ofr.LegalEntityKey = ?");

                SimpleQuery<ApplicationMailingAddress_DAO> qo = new SimpleQuery<ApplicationMailingAddress_DAO>(QueryLanguage.Sql, sql, legalEntityAddress.Address.Key, (int)OfferStatuses.Open, legalEntityAddress.LegalEntity.Key);
                qo.AddSqlReturnDefinition(typeof(ApplicationMailingAddress_DAO), "ama");
                ApplicationMailingAddress_DAO[] appMailingAddresses = qo.Execute();

                foreach (ApplicationMailingAddress_DAO ama in appMailingAddresses)
                {
                    if (spc.IgnoreWarnings)
                    {
                        // update the application mailing address to have the new mail address key
                        var appMailingAddress = BMTM.GetMappedType<IApplicationMailingAddress, ApplicationMailingAddress_DAO>(ama);
                        appMailingAddress.Address = address;
                        addressRepository.SaveApplicationMailingAddress(appMailingAddress);
                    }
                    else
                        spc.DomainMessages.Add(new Warning(String.Format("The address being changed is already attached to offer {0} - the offer will be updated with the new address.", ama.Application.Key), ""));
                }

                // then check the open accounts
                sql = @"select ma.*
                            from [2am].dbo.MailingAddress ma (nolock)
                            join [2am].dbo.Account acc (nolock)
                                on acc.AccountKey = ma.AccountKey
                            join [2am].dbo.[Role] r
                                on r.AccountKey = acc.AccountKey
                            where ma.AddressKey = ? and acc.AccountStatusKey = ? and r.legalEntityKey = ? ";

                SimpleQuery<MailingAddress_DAO> qa = new SimpleQuery<MailingAddress_DAO>(QueryLanguage.Sql, sql, legalEntityAddress.Address.Key, (int)AccountStatuses.Open, legalEntityAddress.LegalEntity.Key);
                qa.AddSqlReturnDefinition(typeof(MailingAddress_DAO), "ma");
                MailingAddress_DAO[] accMailingAddresses = qa.Execute();

                foreach (MailingAddress_DAO ma in accMailingAddresses)
                {
                    if (spc.IgnoreWarnings)
                    {
                        var mailingAddress = BMTM.GetMappedType<IMailingAddress, MailingAddress_DAO>(ma);
                        mailingAddress.Address = address;
                        addressRepository.SaveMailingAddress(mailingAddress);
                    }
                    else
                        spc.DomainMessages.Add(new Warning(String.Format("The address being changed is already attached to account {0} - the account will be updated with the new address.", ma.Account.Key), ""));
                }

                if (ValidationHelper.PrincipalHasValidationErrors())
                    throw new DomainValidationException();
            }

            // update the legal entity address with the address, and save it - note that the address may have
            // changed from the address originally passed in if an existing address with the same details was
            // found
            legalEntityAddress.Address = address;
            LegalEntityAddress_DAO dao = (LegalEntityAddress_DAO)(legalEntityAddress as IDAOObject).GetDAOObject();
            dao.SaveAndFlush();

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        public IAssetLiability GetEmptyAssetLiability(AssetLiabilityTypes assetLiabilityTypes)
        {
            IAssetLiability al = null;

            switch (assetLiabilityTypes)
            {
                case AssetLiabilityTypes.FixedProperty:
                    al = new AssetLiabilityFixedProperty(new AssetLiabilityFixedProperty_DAO());
                    break;

                case AssetLiabilityTypes.LiabilityLoan:
                    al = new AssetLiabilityLiabilityLoan(new AssetLiabilityLiabilityLoan_DAO());
                    break;

                case AssetLiabilityTypes.LiabilitySurety:
                    al = new AssetLiabilityLiabilitySurety(new AssetLiabilityLiabilitySurety_DAO());
                    break;

                case AssetLiabilityTypes.LifeAssurance:
                    al = new AssetLiabilityLifeAssurance(new AssetLiabilityLifeAssurance_DAO());
                    break;

                case AssetLiabilityTypes.ListedInvestments:
                    al = new AssetLiabilityInvestmentPublic(new AssetLiabilityInvestmentPublic_DAO());
                    break;

                case AssetLiabilityTypes.UnlistedInvestments:
                    al = new AssetLiabilityInvestmentPrivate(new AssetLiabilityInvestmentPrivate_DAO());
                    break;

                case AssetLiabilityTypes.OtherAsset:
                    al = new AssetLiabilityOther(new AssetLiabilityOther_DAO());
                    break;

                case AssetLiabilityTypes.FixedLongTermInvestment:
                    al = new AssetLiabilityFixedLongTermInvestment(new AssetLiabilityFixedLongTermInvestment_DAO());
                    break;

                default:

                    // throw some kind of error.
                    break;
            }

            return al;
        }

        public ILegalEntityAssetLiability GetEmptyLegalEntityAssetLiability()
        {
            return base.CreateEmpty<ILegalEntityAssetLiability, LegalEntityAssetLiability_DAO>();

            //return new LegalEntityAssetLiability(new LegalEntityAssetLiability_DAO());
        }

        public ILegalEntityBankAccount GetEmptyLegalEntityBankAccount()
        {
            return base.CreateEmpty<ILegalEntityBankAccount, LegalEntityBankAccount_DAO>();

            //return new LegalEntityBankAccount(new LegalEntityBankAccount_DAO());
        }

        public void SaveLegalEntityAssetLiability(ILegalEntityAssetLiability leAssetLiability)
        {
            base.Save<ILegalEntityAssetLiability, LegalEntityAssetLiability_DAO>(leAssetLiability);

            //LegalEntityAssetLiability_DAO daoLegalEntityAL = (LegalEntityAssetLiability_DAO)(leAssetLiability as IDAOObject).GetDAOObject();

            //daoLegalEntityAL.SaveAndFlush();

            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        public void SaveLegalEntityEstateAgent(ILegalEntity legalEntity, bool recalculateApplicationDetail)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IRuleService svc = ServiceFactory.GetService<IRuleService>();

            // Only validate an LE if it is completely new OR
            // If existing and not linked to an Account or Application
            if ((legalEntity.Key > 0 && !HasNonLeadRoles(legalEntity)) || (legalEntity.Key == 0))
            {
                svc.ExecuteRule(spc.DomainMessages, "LegalEntityEstateAgentMandatoryFields", legalEntity);
            }

            SaveLegalEntity(legalEntity, recalculateApplicationDetail);
        }

        public void SaveLegalEntityPaymentDistributionAgent(ILegalEntity legalEntity, bool recalculateApplicationDetail)
        {
            //This is a complete hack to populate collections required by HALO views to do stuff in the PreRender method
            //If there are any spc DomainMessages the application falls over with LazyInitErros
            //and dies if the applicationRole's and count are not available.
            //this method call populates these
            HasNonLeadRoles(legalEntity);

            SaveLegalEntity(legalEntity, recalculateApplicationDetail);
        }

        /// <summary>
        /// Save Legal Entity Debt Counsellor
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="recalculateApplicationDetail"></param>
        public void SaveLegalEntityDebtCounsellor(ILegalEntity legalEntity, bool recalculateApplicationDetail)
        {
            //This is a complete hack to populate collections required by HALO views to do stuff in the PreRender method
            //If there are any spc DomainMessages the application falls over with LazyInitErros
            //and dies if the applicationRole's and count are not available.
            //this method call populates these
            HasNonLeadRoles(legalEntity);

            SaveLegalEntity(legalEntity, recalculateApplicationDetail);
        }

        #region Search methods

        /// <summary>
        /// Performs a simple search on legal entities with a straight query (does not use full-text technology).
        /// Note that if FirstNames is set, an or comparison is run against FirstNames, PreferredName and Initials.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="maxResults">The maximum number of rows.</param>
        /// <returns></returns>
        /// <remarks>
        /// This will current only search natural person type legal entities, but has been left
        /// as ILegalEntity in case this changes.
        /// </remarks>
        // TODO: THis really needs to use parameters for db caching of the query - ran out of time though
        // and didn't get to investigate "LIKE x%" queries, could only find "LIKE %x%" which is not what
        // we need
        public IEventList<ILegalEntity> SearchLegalEntities(IClientSearchCriteria searchCriteria, int maxResults)
        {
            if (searchCriteria.IsEmpty)
                return new DAOEventList<LegalEntity_DAO, ILegalEntity, LegalEntity>(new List<LegalEntity_DAO>());

            SAHLPrincipal principal = SAHLPrincipal.GetCurrent();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            string originationSources = spc.OriginationSourceKeysStringForQuery;

            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format(@"select distinct top {0} le.* from LegalEntity le (nolock)
                inner join [Role] r (nolock) on r.LegalEntityKey = le.LegalEntityKey
                inner join Account a (nolock) on a.AccountKey = r.AccountKey
                ", maxResults));

            // add the subsidy join if the salary number is supplied
            if (!String.IsNullOrEmpty(searchCriteria.SalaryNumber))
                sb.AppendLine(" inner join [Subsidy] s (nolock) on s.LegalEntityKey = le.LegalEntityKey ");

            // start building the criteria of the search
            sb.AppendLine(" where a.RRR_OriginationSourceKey in (:origSources) ");

            if (!String.IsNullOrEmpty(searchCriteria.FirstNames))
                sb.AppendLine(" and (le.FirstNames like :firstName or le.Initials like :firstName or le.PreferredName like :firstName) ");
            if (!String.IsNullOrEmpty(searchCriteria.Surname))
                sb.AppendLine(" and (le.Surname like :surname) ");
            if (!String.IsNullOrEmpty(searchCriteria.IDNumber))
                sb.AppendLine(" and (le.IDNumber like :idNumber) ");
            if (!String.IsNullOrEmpty(searchCriteria.AccountNumber))
                sb.AppendLine(" and (r.AccountKey like :accountKey) ");
            if (!String.IsNullOrEmpty(searchCriteria.SalaryNumber))
                sb.AppendLine(" and (s.SalaryNumber like :salaryNumber) ");

            SimpleQuery<LegalEntity_DAO> query = new SimpleQuery<LegalEntity_DAO>(QueryLanguage.Sql, sb.ToString().Replace("{", "").Replace("}", ""));
            query.AddSqlReturnDefinition(typeof(LegalEntity_DAO), "le");

            // set the parameters
            if (!String.IsNullOrEmpty(searchCriteria.FirstNames))
                query.SetParameter("firstName", searchCriteria.FirstNames + "%");
            if (!String.IsNullOrEmpty(searchCriteria.Surname))
                query.SetParameter("surname", searchCriteria.Surname + "%");
            if (!String.IsNullOrEmpty(searchCriteria.IDNumber))
                query.SetParameter("idNumber", searchCriteria.IDNumber + "%");
            if (!String.IsNullOrEmpty(searchCriteria.AccountNumber))
                query.SetParameter("accountKey", searchCriteria.AccountNumber + "%");
            if (!String.IsNullOrEmpty(searchCriteria.SalaryNumber))
                query.SetParameter("salaryNumber", searchCriteria.SalaryNumber + "%");

            query.SetParameterList("origSources", originationSources.Split(','));

            LegalEntity_DAO[] results = query.Execute();
            return new DAOEventList<LegalEntity_DAO, ILegalEntity, LegalEntity>(new List<LegalEntity_DAO>(results));
        }

        public IEventList<ILegalEntity> SuperSearchForClientLegalEntities(IClientSuperSearchCriteria SearchCriteria)
        {
            EventList<ILegalEntity> LEs = new EventList<ILegalEntity>();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            if (SearchCriteria == null)
            {
                spc.DomainMessages.Add(new DomainMessage(StaticMessages.NullSearchCriteria, ""));
                return LEs;
            }

            if (spc.UserOriginationSourceKeys.Count == 0)
            {
                spc.DomainMessages.Add(new Error(StaticMessages.NoOriginationSources, "Search cannot continue."));
                return LEs;
            }
            string UserOrgsOrigs = spc.OriginationSourceKeysStringForQuery;

            //for (int i = 0; i < spc.UserOriginationSourceKeys.Count; i++)
            //{
            //    UserOrgsOrigs += Convert.ToString(spc.UserOriginationSourceKeys[i]);
            //    if(i<spc.UserOriginationSources.Count -1)
            //        UserOrgsOrigs += ", ";
            //}
            string Q = string.Format("select top 51 le.* from [2am].dbo.ClientSearchRank ('{0}', '{1}', '{2}', '{3}') le order by le.RANK desc", CleanFreeTextQueryArgument(SearchCriteria.SearchText), UserOrgsOrigs, SearchCriteria.LegalEntityTypes, SearchCriteria.AccountType);
            SimpleQuery<LegalEntity_DAO> LESQ = new SimpleQuery<LegalEntity_DAO>(QueryLanguage.Sql, Q);

            //LESQ.SetQueryRange(51);

            LESQ.AddSqlReturnDefinition(typeof(LegalEntity_DAO), "le");
            LegalEntity_DAO[] iLEs = LESQ.Execute();
            return new DAOEventList<LegalEntity_DAO, ILegalEntity, LegalEntity>(new List<LegalEntity_DAO>(iLEs));

            //return LEs;
        }

        public IEventList<ILegalEntity> SuperSearchForAllLegalEntities(IClientSuperSearchCriteria SearchCriteria)
        {
            EventList<ILegalEntity> LEs = new EventList<ILegalEntity>();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SearchCriteria.principal);
            if (SearchCriteria == null)
            {
                spc.DomainMessages.Add(new DomainMessage(StaticMessages.NullSearchCriteria, ""));
                return LEs;
            }

            if (spc.UserOriginationSourceKeys.Count == 0)
            {
                spc.DomainMessages.Add(new Error(StaticMessages.NoOriginationSources, "Search cannot continue."));
                return LEs;
            }
            string UserOrgsOrigs = spc.OriginationSourceKeysStringForQuery;

            //for (int i = 0; i < spc.UserOriginationSources.Count; i++)
            //{
            //    UserOrgsOrigs += spc.UserOriginationSources[i].OriginationSource.Key.ToString();
            //    if (i < spc.UserOriginationSources.Count - 1)
            //        UserOrgsOrigs += ", ";
            //}

            string Q = string.Format("select top 51 le.* from [2am].dbo.LegalEntitySearchRank ('{0}', '{1}') le order by le.RANK desc", CleanFreeTextQueryArgument(SearchCriteria.SearchText), SearchCriteria.LegalEntityTypes);
            SimpleQuery<LegalEntity_DAO> LESQ = new SimpleQuery<LegalEntity_DAO>(QueryLanguage.Sql, Q);

            LESQ.AddSqlReturnDefinition(typeof(LegalEntity_DAO), "le");
            LegalEntity_DAO[] iLEs = LESQ.Execute();
            return new DAOEventList<LegalEntity_DAO, ILegalEntity, LegalEntity>(new List<LegalEntity_DAO>(iLEs));
        }

        /// <summary>
        /// Cleans a string being used in the free text query for inclusion in SQL.  This removes characters that can cause
        /// predicate errors and aren't really relevant to the search, and also replaces single apostrophes with escaped
        /// apostrophes.
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        private static string CleanFreeTextQueryArgument(string argument)
        {
            return Regex.Replace(argument, "[{}\\[\\]\"]", "").Replace("'", "''");
        }

        public void DeleteAssetLiability(IAssetLiability ae)
        {
            AssetLiability_DAO dao = (AssetLiability_DAO)(ae as IDAOObject).GetDAOObject();
            dao.Delete();
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        public IEventList<ILegalEntityAssetLiability> GetLegalEntityAssetLiabilitiesByAssetLiabilityKey(int assetLiabilityKey)
        {
            string HQL = "SELECT l from LegalEntityAssetLiability_DAO l WHERE l.AssetLiability.Key = ? ";
            SimpleQuery<LegalEntityAssetLiability_DAO> q = new SimpleQuery<LegalEntityAssetLiability_DAO>(HQL, assetLiabilityKey);
            LegalEntityAssetLiability_DAO[] res = q.Execute();
            IEventList<ILegalEntityAssetLiability> list = new DAOEventList<LegalEntityAssetLiability_DAO, ILegalEntityAssetLiability, LegalEntityAssetLiability>(res);
            return new EventList<ILegalEntityAssetLiability>(list);
        }

        public IEventList<ILegalEntityAssetLiability> GetLegalEntityAssetLiabilityList(int LegalEntityKey)
        {
            IEventList<ILegalEntityAssetLiability> leaLst = null;
            string sql = "SELECT lea.* FROM [2am].[dbo].LegalEntityAssetLiability lea where lea.LegalEntityKey = ?";
            SimpleQuery<LegalEntityAssetLiability_DAO> query = new SimpleQuery<LegalEntityAssetLiability_DAO>(QueryLanguage.Sql, sql, LegalEntityKey);
            query.AddSqlReturnDefinition(typeof(LegalEntityAssetLiability_DAO), "lea");
            LegalEntityAssetLiability_DAO[] res = query.Execute();
            if (res != null && res.Length > 0)
                leaLst = new DAOEventList<LegalEntityAssetLiability_DAO, ILegalEntityAssetLiability, LegalEntityAssetLiability>(res);
            else
                leaLst = new EventList<ILegalEntityAssetLiability>();

            return leaLst;
        }

        #endregion Search methods

        public void UpdateLegalEntityType(int legalEntityKey, int legalEntityTypeKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("update [2am]..LegalEntity set LegalEntityTypeKey={0} where LegalEntityKey={1}", legalEntityTypeKey, legalEntityKey);
            castleTransactionsService.ExecuteNonQueryOnCastleTran(sb.ToString(), typeof(GeneralStatus_DAO), null);
        }

        /// <summary>
        /// Checks to see whether a legal entity has account roles or application roles other than leadmainapplicant or leadsuretor
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <returns></returns>
        public bool HasNonLeadRoles(ILegalEntity legalEntity)
        {
            bool nonLeadRolesExist = false;

            // check for the existence of open account roles
            if (legalEntity.Roles != null && legalEntity.Roles.Count > 0)
            {
                foreach (IRole role in legalEntity.Roles)
                {
                    switch (role.Account.AccountStatus.Key)
                    {
                        case (int)SAHL.Common.Globals.AccountStatuses.Open:
                        case (int)SAHL.Common.Globals.AccountStatuses.Application:
                        case (int)SAHL.Common.Globals.AccountStatuses.ApplicationpriortoInstructAttorney:
                            nonLeadRolesExist = true;
                            break;

                        default:
                            break;
                    }

                    if (nonLeadRolesExist == true)
                        break;
                }
            }

            // if we havent found any account roles then check for application roles
            if (nonLeadRolesExist == false)
            {
                if (legalEntity.ApplicationRoles != null && legalEntity.ApplicationRoles.Count > 0)
                {
                    foreach (IApplicationRole applicationRole in legalEntity.GetApplicationRolesByRoleTypeGroups(OfferRoleTypeGroups.Client))
                    {
                        // only check open/accepted offer roles
                        if (applicationRole.Application.ApplicationStatus.Key == (int)OfferStatuses.Open || applicationRole.Application.ApplicationStatus.Key != (int)OfferStatuses.Accepted)
                        {
                            switch (applicationRole.ApplicationRoleType.Key)
                            {
                                case (int)SAHL.Common.Globals.OfferRoleTypes.MainApplicant:
                                case (int)SAHL.Common.Globals.OfferRoleTypes.Suretor:
                                case (int)SAHL.Common.Globals.OfferRoleTypes.AssuredLife:
                                    nonLeadRolesExist = true;
                                    break;

                                default:
                                    break;
                            }
                        }
                        if (nonLeadRolesExist == true)
                            break;
                    }
                }
            }

            return nonLeadRolesExist;
        }

        /// <summary>
        /// Life department require notification via email when any legal entitiy on an open account with an in force life policy,
        /// has had their id no changed. Since this will affect their life policy premiums if an in force life policy exists.
        /// </summary>
        /// <param name="legalEntity"></param>
        public void NotifyChangeOfIDNumberLinkedToLifePolicy(ILegalEntity legalEntity)
        {
            ILegalEntityNaturalPerson leNP = legalEntity as ILegalEntityNaturalPerson;

            if (leNP != null && LegalEntityNPHasIDNumberChanged(leNP))
            {
                IControlRepository controlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                IControl control = controlRepo.GetControlByDescription(Constants.ControlTable.Life.LifeNotifyChangeOfIDNumber);
                string toAddress = control.ControlText;
                string subject = "Legal Entity ID Number Change Notice";
                StringBuilder body = new StringBuilder();

                // Check if the Legal Entity is linked to a Loan Account that is open & is linked to an open Life Policy with Status = InForce
                foreach (IRole role in leNP.Roles)
                {
                    if (role.Account.AccountStatus.Key == (int)AccountStatuses.Open)
                    {
                        foreach (IAccount relatedAccount in role.Account.RelatedChildAccounts)
                        {
                            if (relatedAccount.Product.Key == (int)Products.LifePolicy && relatedAccount.AccountStatus.Key == (int)AccountStatuses.Open)
                            {
                                IAccountLifePolicy accountLifePolicy = relatedAccount as IAccountLifePolicy;
                                ILifePolicy lifePolicy = accountLifePolicy.LifePolicy;
                                if (lifePolicy.LifePolicyStatus.Key == (int)LifePolicyStatuses.Inforce)
                                {
                                    // Construct the Body of the EMail
                                    body.AppendFormat("This is a notice that the ID Number for Legal Entity ({0}) linked to Loan ({1}) and Life Policy ({2}) has been changed.<br/>", legalEntity.DisplayName, role.Account.Key, accountLifePolicy.Key);
                                    body.AppendFormat("Old ID Number   : {0}<br/>", leNP.GetPreviousValue<ILegalEntityNaturalPerson, string>((legalEntityNaturalPerson) => legalEntityNaturalPerson.IDNumber));
                                    body.AppendFormat("New ID Number   : {0}<br/>", leNP.IDNumber);

                                    // Send the Mail
                                    IMessageService messageService = ServiceFactory.GetService<IMessageService>();
                                    messageService.SendEmailInternal("HALO@SAHomeloans.com", toAddress, null, null, subject, body.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if the first 6 digits of the Legal Entity's ID Number has changed.
        /// </summary>
        /// <param name="leNP"></param>
        /// <returns></returns>
        private bool LegalEntityNPHasIDNumberChanged(ILegalEntityNaturalPerson leNP)
        {
            if (String.IsNullOrEmpty(leNP.IDNumber))
            {
                return false;
            }

            string previousIDNumber = leNP.GetPreviousValue<ILegalEntityNaturalPerson, string>((legalEntityNaturalPerson) => legalEntityNaturalPerson.IDNumber);

            if (String.IsNullOrEmpty(previousIDNumber))
            {
                return false;
            }
            if (leNP.IDNumber.Substring(0, 6) != previousIDNumber.Substring(0, 6))
                return true;
            else
                return false;
        }

        #endregion ILegalEntityRepository Members

        #region Repositories

        private IApplicationRepository _appRepo;

        private IApplicationRepository ApplicationRepo
        {
            get
            {
                if (_appRepo == null)
                    _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _appRepo;
            }
        }

        private ILookupRepository _lookupRepo;

        private ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
        }

        #endregion Repositories

        #region Legal Entity Role

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="GenericTypeKey"></param>
        ///// <param name="GenericKey"></param>
        ///// <returns></returns>
        //public IList<ILegalEntityRole> GetLegalEntityRoles(int GenericTypeKey, int GenericKey)
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="GenericTypeKey"></param>
        ///// <param name="GenericKey"></param>
        ///// <param name="GeneralStatusKey"></param>
        ///// <returns></returns>
        //public IList<ILegalEntityRole> GetLegalEntityRoles(int GenericTypeKey, int GenericKey, int GeneralStatusKey)
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// Save Legal Entity Role
        ///// </summary>
        ///// <param name="legalEntityRole"></param>
        //public void SaveLegalEntityRole(ILegalEntityRole legalEntityRole)
        //{
        //    base.Save<ILegalEntityRole, LegalEntityRole_DAO>(legalEntityRole);
        //}

        #endregion Legal Entity Role

        #region External Role

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="externalRoleTypeGroup"></param>
        /// <param name="generalStatus"></param>
        /// <returns></returns>
        public IReadOnlyEventList<ILegalEntity> GetLegalEntitiesByExternalRoleTypeGroup(int genericKey, int genericKeyTypeKey, ExternalRoleTypeGroups externalRoleTypeGroup, GeneralStatuses generalStatus)
        {
            string hql = "select er.LegalEntity from ExternalRole_DAO er where genericKey = ? and er.GenericKeyType.Key = ? and er.ExternalRoleType.ExternalRoleTypeGroup.Key = ? and er.GeneralStatus.Key = ?";
            SimpleQuery<LegalEntity_DAO> query = new SimpleQuery<LegalEntity_DAO>(hql, genericKey, genericKeyTypeKey, (int)externalRoleTypeGroup, (int)generalStatus);
            LegalEntity_DAO[] legalEntities = ExternalRole_DAO.ExecuteQuery(query) as LegalEntity_DAO[];

            if (legalEntities != null)
            {
                IList<LegalEntity_DAO> list = new List<LegalEntity_DAO>(legalEntities);
                DAOEventList<LegalEntity_DAO, ILegalEntity, LegalEntity> daoList = new DAOEventList<LegalEntity_DAO, ILegalEntity, LegalEntity>(list);
                return new ReadOnlyEventList<ILegalEntity>(daoList);
            }
            else
                return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="externalRoleTypeGroup"></param>
        /// <param name="generalStatus"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IExternalRole> GetExternalRoles(int genericKey, int genericKeyTypeKey, ExternalRoleTypeGroups externalRoleTypeGroup, GeneralStatuses generalStatus)
        {
            string hql = "select er from ExternalRole_DAO er where genericKey = ? and er.GenericKeyType.Key = ? and er.ExternalRoleType.ExternalRoleTypeGroup.Key = ? and er.GeneralStatus.Key = ?";
            SimpleQuery<ExternalRole_DAO> query = new SimpleQuery<ExternalRole_DAO>(hql, genericKey, genericKeyTypeKey, (int)externalRoleTypeGroup, (int)generalStatus);
            ExternalRole_DAO[] externalRoles = ExternalRole_DAO.ExecuteQuery(query) as ExternalRole_DAO[];

            if (externalRoles != null)
            {
                IList<ExternalRole_DAO> list = new List<ExternalRole_DAO>(externalRoles);
                DAOEventList<ExternalRole_DAO, IExternalRole, ExternalRole> daoList = new DAOEventList<ExternalRole_DAO, IExternalRole, ExternalRole>(list);
                return new ReadOnlyEventList<IExternalRole>(daoList);
            }
            else
                return null;
        }

        /// <summary>
        /// Get External Roles
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyType"></param>
        /// <param name="externalRoleType"></param>
        /// <param name="generalStatus"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IExternalRole> GetExternalRoles(int genericKey, GenericKeyTypes genericKeyType, ExternalRoleTypes externalRoleType, GeneralStatuses generalStatus)
        {
            string hql = "select er from ExternalRole_DAO er where genericKey = ? and er.GenericKeyType.Key = ? and er.ExternalRoleType.Key = ? and er.GeneralStatus.Key = ?";
            SimpleQuery<ExternalRole_DAO> query = new SimpleQuery<ExternalRole_DAO>(hql, genericKey, (int)genericKeyType, (int)externalRoleType, (int)generalStatus);
            ExternalRole_DAO[] externalRoles = ExternalRole_DAO.ExecuteQuery(query) as ExternalRole_DAO[];

            if (externalRoles != null)
            {
                IList<ExternalRole_DAO> list = new List<ExternalRole_DAO>(externalRoles);
                DAOEventList<ExternalRole_DAO, IExternalRole, ExternalRole> daoList = new DAOEventList<ExternalRole_DAO, IExternalRole, ExternalRole>(list);
                return new ReadOnlyEventList<IExternalRole>(daoList);
            }
            else
                return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyType"></param>
        /// <param name="externalRoleType"></param>
        /// <param name="generalStatus"></param>
        /// <param name="LegalEntityKey"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IExternalRole> GetExternalRoles(int genericKey, GenericKeyTypes genericKeyType, ExternalRoleTypes externalRoleType, GeneralStatuses generalStatus, int LegalEntityKey)
        {
            string hql = "select er from ExternalRole_DAO er where genericKey = ? and er.GenericKeyType.Key = ? and er.ExternalRoleType.Key = ? and er.GeneralStatus.Key = ? and er.LegalEntity.Key = ?";
            SimpleQuery<ExternalRole_DAO> query = new SimpleQuery<ExternalRole_DAO>(hql, genericKey, (int)genericKeyType, (int)externalRoleType, (int)generalStatus, LegalEntityKey);
            ExternalRole_DAO[] externalRoles = ExternalRole_DAO.ExecuteQuery(query) as ExternalRole_DAO[];

            if (externalRoles != null)
            {
                IList<ExternalRole_DAO> list = new List<ExternalRole_DAO>(externalRoles);
                DAOEventList<ExternalRole_DAO, IExternalRole, ExternalRole> daoList = new DAOEventList<ExternalRole_DAO, IExternalRole, ExternalRole>(list);
                return new ReadOnlyEventList<IExternalRole>(daoList);
            }
            else
                return null;
        }

        public IReadOnlyEventList<IExternalRole> GetExternalRoles(GenericKeyTypes genericKeyType, ExternalRoleTypes externalRoleType, int LegalEntityKey)
        {
            string hql = "select er from ExternalRole_DAO er where  er.GenericKeyType.Key = ? and er.ExternalRoleType.Key = ?  and er.LegalEntity.Key = ?";
            SimpleQuery<ExternalRole_DAO> query = new SimpleQuery<ExternalRole_DAO>(hql, (int)genericKeyType, (int)externalRoleType, LegalEntityKey);
            ExternalRole_DAO[] externalRoles = ExternalRole_DAO.ExecuteQuery(query) as ExternalRole_DAO[];

            if (externalRoles != null)
            {
                IList<ExternalRole_DAO> list = new List<ExternalRole_DAO>(externalRoles);
                DAOEventList<ExternalRole_DAO, IExternalRole, ExternalRole> daoList = new DAOEventList<ExternalRole_DAO, IExternalRole, ExternalRole>(list);
                return new ReadOnlyEventList<IExternalRole>(daoList);
            }
            else
                return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="LegalEntityKey"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IExternalRole> GetExternalRolesByLegalEntity(int LegalEntityKey)
        {
            string hql = "select er from ExternalRole_DAO er where er.LegalEntity.Key = ?";
            SimpleQuery<ExternalRole_DAO> query = new SimpleQuery<ExternalRole_DAO>(hql, LegalEntityKey);
            ExternalRole_DAO[] externalRoles = ExternalRole_DAO.ExecuteQuery(query) as ExternalRole_DAO[];

            if (externalRoles != null)
            {
                IList<ExternalRole_DAO> list = new List<ExternalRole_DAO>(externalRoles);
                DAOEventList<ExternalRole_DAO, IExternalRole, ExternalRole> daoList = new DAOEventList<ExternalRole_DAO, IExternalRole, ExternalRole>(list);
                return new ReadOnlyEventList<IExternalRole>(daoList);
            }
            else
                return null;
        }

        public IReadOnlyEventList<IApplication> GetOpenFurtherLendingApplicationsByLegalEntity(ILegalEntity legalEntity)
        {
            var FurtherLending = new int[] { (int)OfferTypes.FurtherAdvance, (int)OfferTypes.FurtherLoan, (int)OfferTypes.ReAdvance };
            var applications = from approles in legalEntity.ApplicationRoles
                               where FurtherLending.Contains(approles.Application.ApplicationType.Key)
                               && approles.Application.IsOpen
                               select approles.Application;

            return new ReadOnlyEventList<IApplication>(applications.ToList());
        }

        /// <summary>
        /// Save External Role
        /// </summary>
        /// <param name="ExternalRole"></param>
        public void SaveExternalRole(IExternalRole ExternalRole)
        {
            base.Save<IExternalRole, ExternalRole_DAO>(ExternalRole);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ExternalRoleType"></param>
        /// <param name="GenericKey"></param>
        /// <param name="GenericKeyType"></param>
        /// <param name="LegalEntityKey"></param>
        /// <param name="DeactivateExisitingRoles"></param>
        /// <returns></returns>
        public IExternalRole InsertExternalRole(ExternalRoleTypes ExternalRoleType, int GenericKey, GenericKeyTypes GenericKeyType, int LegalEntityKey, bool DeactivateExisitingRoles)
        {
            // First check if the Role that is being inserted already exists as an active role
            IReadOnlyEventList<IExternalRole> externalRoles = GetExternalRoles(GenericKey, GenericKeyType, ExternalRoleType, GeneralStatuses.Active, LegalEntityKey);
            if (externalRoles.Count > 0)
                return externalRoles[0];

            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            ILegalEntityRepository LegalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

            // Deactivate all exisiting Role for the type before inserting the new role
            if (DeactivateExisitingRoles)
            {
                IReadOnlyEventList<IExternalRole> externalRolesToDeactivate = GetExternalRoles(GenericKey, GenericKeyType, ExternalRoleType, GeneralStatuses.Active);
                foreach (IExternalRole role in externalRolesToDeactivate)
                {
                    role.GeneralStatus = lookupRepo.GeneralStatuses[GeneralStatuses.Inactive];
                    role.ChangeDate = DateTime.Now;
                    SaveExternalRole(role);
                }
            }

            // Insert the new Role
            IExternalRole externalRole = GetEmptyExternalRole();
            externalRole.ChangeDate = DateTime.Now;
            externalRole.ExternalRoleType = lookupRepo.ExternalRoleTypes.ObjectDictionary[((int)ExternalRoleType).ToString()];
            externalRole.GeneralStatus = lookupRepo.GeneralStatuses[GeneralStatuses.Active];
            externalRole.GenericKey = GenericKey;
            externalRole.GenericKeyType = lookupRepo.GenericKeyType.ObjectDictionary[((int)GenericKeyType).ToString()];
            externalRole.LegalEntity = LegalEntityRepo.GetLegalEntityByKey(LegalEntityKey);
            SaveExternalRole(externalRole);

            // Return the Role
            return externalRole;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IExternalRole GetEmptyExternalRole()
        {
            return base.CreateEmpty<IExternalRole, ExternalRole_DAO>();
        }

        public DataTable GetExternalRolesByAttorneyKey(int attorneyKey)
        {
            string sql = UIStatementRepository.GetStatement("Repositories.LegalEntityRepository", "GetAttorneyContacts");
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@AttorneyKey", attorneyKey));
            DataSet ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), pc);
            return ds.Tables[0];
        }

        #endregion External Role

        #region Legal Entity Login

        /// <summary>
        /// Get Empty Legal Entity Login
        /// </summary>
        /// <returns></returns>
        public ILegalEntityLogin GetEmptyLegalEntityLogin()
        {
            return base.CreateEmpty<ILegalEntityLogin, LegalEntityLogin_DAO>();
        }

        /// <summary>
        /// Get Legal Entity Logins
        /// </summary>
        /// <returns></returns>
        public IEventList<ILegalEntityLogin> GetLegalEntityLogins()
        {
            string query = "select lel from LegalEntityLogin_DAO lel";
            SimpleQuery<LegalEntityLogin_DAO> q = new SimpleQuery<LegalEntityLogin_DAO>(query);
            LegalEntityLogin_DAO[] result = q.Execute();
            IEventList<ILegalEntityLogin> list = new DAOEventList<LegalEntityLogin_DAO, ILegalEntityLogin, LegalEntityLogin>(result);
            if (list != null && list.Count > 0)
            {
                return list;
            }
            return null;
        }

        /// <summary>
        /// Get Legal Entity Login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ILegalEntityLogin GetLegalEntityLogin(string username, string password)
        {
            string query = "select lel from LegalEntityLogin_DAO lel where lel.Username = ? and lel.Password = ? ";
            SimpleQuery<LegalEntityLogin_DAO> q = new SimpleQuery<LegalEntityLogin_DAO>(query, username, password);
            LegalEntityLogin_DAO[] result = q.Execute();
            IEventList<ILegalEntityLogin> list = new DAOEventList<LegalEntityLogin_DAO, ILegalEntityLogin, LegalEntityLogin>(result);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// Get Legal Entity Login
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public ILegalEntityLogin GetLegalEntityLogin(string username)
        {
            string query = "select lel from LegalEntityLogin_DAO lel where lel.Username = ?";
            SimpleQuery<LegalEntityLogin_DAO> q = new SimpleQuery<LegalEntityLogin_DAO>(query, username);
            LegalEntityLogin_DAO[] result = q.Execute();
            IEventList<ILegalEntityLogin> list = new DAOEventList<LegalEntityLogin_DAO, ILegalEntityLogin, LegalEntityLogin>(result);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// Create Empty Legal Entity Login
        /// </summary>
        /// <returns></returns>
        public ILegalEntityLogin CreateEmptyLegalEntityLogin()
        {
            return base.CreateEmpty<ILegalEntityLogin, LegalEntityLogin_DAO>();
        }

        /// <summary>
        /// Save Legal Entity Login
        /// </summary>
        /// <param name="legalEntityLogin"></param>
        public void SaveLegalEntityLogin(ILegalEntityLogin legalEntityLogin)
        {
            base.Save<ILegalEntityLogin, LegalEntityLogin_DAO>(legalEntityLogin);
        }

        /// <summary>
        /// Get Legal Entity
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public ILegalEntityNaturalPerson GetWebAccessLegalEntity(string emailAddress)
        {
            string query = "select er.LegalEntity from ExternalRole_DAO er where er.ExternalRoleType.Key = 10 and er.GeneralStatus.Key = 1 and er.LegalEntity.EmailAddress = ?";
            SimpleQuery<LegalEntityNaturalPerson_DAO> q = new SimpleQuery<LegalEntityNaturalPerson_DAO>(query, emailAddress);
            LegalEntityNaturalPerson_DAO[] result = q.Execute();
            IEventList<ILegalEntityNaturalPerson> list = new DAOEventList<LegalEntityNaturalPerson_DAO, ILegalEntityNaturalPerson, LegalEntityNaturalPerson>(result);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// Get Legal Entity
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public ILegalEntityNaturalPerson GetLegalEntityClientByEmail(string emailAddress)
        {
            string query = UIStatementRepository.GetStatement("Repositories.LegalEntityRepository", "GetLegalEntityClientByEmail"); ;

            SimpleQuery<LegalEntityNaturalPerson_DAO> q = new SimpleQuery<LegalEntityNaturalPerson_DAO>(QueryLanguage.Sql, query, emailAddress);
            q.AddSqlReturnDefinition(typeof(LegalEntityNaturalPerson_DAO), "le");
            LegalEntityNaturalPerson_DAO[] result = q.Execute();
            IEventList<ILegalEntityNaturalPerson> list = new DAOEventList<LegalEntityNaturalPerson_DAO, ILegalEntityNaturalPerson, LegalEntityNaturalPerson>(result);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// GetAllClientAccountsByLegalEntityKey
        /// used via the client secure website
        /// this is regressed to work like the old website and should be amended asap!
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        public IList<Int32> GetAllClientAccountsByLegalEntityKey(int legalEntityKey)
        {
            var accountKeys = new List<Int32>();
            string query = String.Format(UIStatementRepository.GetStatement("Repositories.LegalEntityRepository", "GetAllClientAccountsByLegalEntityKey"), legalEntityKey.ToString()) ;

            // Execute Query
            DataSet dsResults = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), null);
            if (dsResults != null && dsResults.Tables.Count > 0)
            {
                int accountKey = 0;
                foreach (DataRow row in dsResults.Tables[0].Rows)
                {
                    if (Int32.TryParse(row[0].ToString(), out accountKey)
                        && accountKey > 0
                        && !accountKeys.Contains(accountKey))
                    {
                        accountKeys.Add(accountKey);
                    }
                }
            }

            return accountKeys;
        }




        /// <summary>
        /// Get External Role for a Legal Entity
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="externalRoleType"></param>
        public IExternalRole GetExternalRole(ILegalEntityNaturalPerson legalEntity, ExternalRoleTypes externalRoleType)
        {
            string query = "select er from ExternalRole_DAO er where er.LegalEntity.Key = ? and er.ExternalRoleType.Key = ?";
            SimpleQuery<ExternalRole_DAO> q = new SimpleQuery<ExternalRole_DAO>(query, legalEntity.Key, (int)externalRoleType);
            ExternalRole_DAO[] result = q.Execute();
            IEventList<IExternalRole> list = new DAOEventList<ExternalRole_DAO, IExternalRole, ExternalRole>(result);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// Get Attorney
        /// </summary>
        /// <param name="externalRole"></param>
        /// <returns></returns>
        public IAttorney GetAttorney(IExternalRole externalRole)
        {
            if (externalRole.GenericKeyType.Key != (int)GenericKeyTypes.Attorney)
            {
                return null;
            }
            string query = "select att from Attorney_DAO att where att.Key = ?";
            SimpleQuery<Attorney_DAO> q = new SimpleQuery<Attorney_DAO>(query, externalRole.GenericKey);
            Attorney_DAO[] result = q.Execute();
            IEventList<IAttorney> list = new DAOEventList<Attorney_DAO, IAttorney, Attorney>(result);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        #endregion Legal Entity Login

        #region Legal Entity By Offer

        ///#19669
        public IReadOnlyEventList<IExternalRole> GetLegalEntityInformationByOfferKey(int genericKey)
        {
            string hql = "select er from ExternalRole_DAO er where er.GenericKey = ?";
            SimpleQuery<ExternalRole_DAO> query = new SimpleQuery<ExternalRole_DAO>(hql, genericKey);
            ExternalRole_DAO[] externalRoles = ExternalRole_DAO.ExecuteQuery(query) as ExternalRole_DAO[];

            if (externalRoles != null)
            {
                IList<ExternalRole_DAO> list = new List<ExternalRole_DAO>(externalRoles);
                DAOEventList<ExternalRole_DAO, IExternalRole, ExternalRole> daoList = new DAOEventList<ExternalRole_DAO, IExternalRole, ExternalRole>(list);

                return new ReadOnlyEventList<IExternalRole>(daoList);
            }
            else
                return null;
        }

        #endregion Legal Entity By Offer

        /// <summary>
        /// There is no requirement to expose this publicly - this is used as a helper method
        /// </summary>
        /// <returns></returns>
        public IList<IAffordabilityType> GetAffordabilityTypes()
        {
            var daos = AffordabilityType_DAO.FindAll();
            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IAffordabilityType>(daos);
        }

        public ILegalEntityDomicilium CreateEmptyLegalEntityDomicilium()
        {
            return base.CreateEmpty<ILegalEntityDomicilium, LegalEntityDomicilium_DAO>();
        }

        public ILegalEntityDomicilium GetLegalEntityDomiciliumByKey(int legalEntityDomiciliumKey)
        {
            return base.GetByKey<ILegalEntityDomicilium, LegalEntityDomicilium_DAO>(legalEntityDomiciliumKey);
        }

        public void SaveLegalEntityDomicilium(ILegalEntityDomicilium legalEntityDomiciliumToSave)
        {
            base.Save<ILegalEntityDomicilium, LegalEntityDomicilium_DAO>(legalEntityDomiciliumToSave);
        }

        public IEventList<ILegalEntityDomicilium> GetLegalEntityDomiciliumsForAddressKey(int addressKey)
        {
            //LegalEntityDomicilium_DAO d;
            //d.LegalEntityAddress.Address.Key
            string HQL = "select d from LegalEntityDomicilium_DAO d where d.LegalEntityAddress.Address.Key=?";

            SimpleQuery<LegalEntityDomicilium_DAO> q = new SimpleQuery<LegalEntityDomicilium_DAO>(HQL, addressKey);
            LegalEntityDomicilium_DAO[] array = q.Execute();
            IList<LegalEntityDomicilium_DAO> list = new List<LegalEntityDomicilium_DAO>(array);
            return new DAOEventList<LegalEntityDomicilium_DAO, ILegalEntityDomicilium, LegalEntityDomicilium>(list);
        }

        [Obsolete("LegalEntityAddress2 contains a get/set property that will do the heavy lifting")]
        public void SaveAndActivateLegalEntityDomiciliumAndDeactivatePrevious(ILegalEntityDomicilium legalEntityDomicilium)
        {
            ILegalEntity legalEntity = legalEntityDomicilium.LegalEntityAddress.LegalEntity;
            foreach (var legalEntityAddress in legalEntity.LegalEntityAddresses)
            {
                foreach (var domicilium in legalEntityAddress.LegalEntityDomiciliums)
                {
                    if (domicilium != null && domicilium.GeneralStatus.Key != (int)SAHL.Common.Globals.GeneralStatuses.Inactive)
                    {
                        domicilium.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Inactive];
                    }
                }
            }
            SaveLegalEntity(legalEntity, false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool ClientRequestAdditionalFunds(int accountKey, decimal amount)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "ClientRequestFunds");

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));
            parameters.Add(new SqlParameter("@Amount", amount));

            object o = castleTransactionsService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

            if (o != null)
            {
                int r;
                Int32.TryParse(o.ToString(), out r);
                if (r == 0)
                    return true;
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityDomicilium"></param>
        public void DeleteLegalEntityDomicilium(ILegalEntityDomicilium legalEntityDomicilium)
        {
            LegalEntityDomicilium_DAO dao = (LegalEntityDomicilium_DAO)(legalEntityDomicilium as IDAOObject).GetDAOObject();
            dao.Delete();
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        public int? GetSubsidyAccountKey(int legalEntityKey)
        {
            var query = @"  select distinct asub.AccountKey
                            from [2AM].dbo.AccountSubsidy asub
                            join [2AM].dbo.Subsidy s on asub.SubsidyKey = s.SubsidyKey and s.GeneralStatusKey = 1
                            join [2AM].dbo.Account acc on asub.AccountKey = acc.AccountKey and acc.AccountStatusKey = 1
                            where s.LegalEntityKey = @LegalEntityKey";

            var prms = new ParameterCollection();
            Helper.AddIntParameter(prms, "@LegalEntityKey", legalEntityKey);

            DataSet results = this.castleTransactionsService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);
            if (results != null && results.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(results.Tables[0].Rows[0][0]);
            }
            else
            {
                return null;
            }
        }
    }
}