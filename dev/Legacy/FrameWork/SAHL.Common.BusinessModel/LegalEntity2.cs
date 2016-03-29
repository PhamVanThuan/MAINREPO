using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel
{
    public abstract partial class LegalEntity : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntity_DAO>, ILegalEntity
    {
        /// <summary>
        /// Added constructors to the base class to prevent the method CheckUpdateOpenAccount being called
        /// upon every set.
        /// </summary>

        public override void ExtendedConstructor()
        {
        }

        public virtual string DisplayName
        {
            get { return GetLegalName(LegalNameFormat.Full); }
        }

        /// <summary>
        /// Implements <see cref="ILegalEntity.LegalNumber"/>.
        /// </summary>
        public virtual string LegalNumber
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public ILegalEntityType LegalEntityType
        {
            get
            {
                ILookupRepository rep = RepositoryFactory.GetRepository<ILookupRepository>();
                if (this is ILegalEntityCloseCorporation)
                    return rep.LegalEntityTypes.ObjectDictionary[((int)LegalEntityTypes.CloseCorporation).ToString()];
                else if (this is ILegalEntityCompany)
                    return rep.LegalEntityTypes.ObjectDictionary[((int)LegalEntityTypes.Company).ToString()];
                else if (this is ILegalEntityNaturalPerson)
                    return rep.LegalEntityTypes.ObjectDictionary[((int)LegalEntityTypes.NaturalPerson).ToString()];
                else if (this is ILegalEntityTrust)
                    return rep.LegalEntityTypes.ObjectDictionary[((int)LegalEntityTypes.Trust).ToString()];
                else if (this is ILegalEntityUnknown)
                    return rep.LegalEntityTypes.ObjectDictionary[((int)LegalEntityTypes.Unknown).ToString()];
                else
                    throw new NotSupportedException("Unsupported legal entity type.");
            }
        }

        public ILegalEntityLogin LegalEntityLogin
        {
            get
            {
                if (_DAO.LegalEntityLogins != null &&
                    _DAO.LegalEntityLogins.Count > 0)
                {
                    LegalEntityLogin_DAO legalEntityLogin = _DAO.LegalEntityLogins[0];
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILegalEntityLogin, LegalEntityLogin_DAO>(legalEntityLogin);
                }
                return null;
            }
            set
            {
                IDAOObject daoObject = value as IDAOObject;
                LegalEntityLogin_DAO dao = (LegalEntityLogin_DAO)daoObject.GetDAOObject();

                if (_DAO.LegalEntityLogins == null)
                {
                    _DAO.LegalEntityLogins = new List<LegalEntityLogin_DAO>();
                }

                if (_DAO.LegalEntityLogins.Count > 0)
                {
                    _DAO.LegalEntityLogins[0] = dao;
                }
                else
                {
                    _DAO.LegalEntityLogins.Add(dao);
                }
            }
        }

        public /*virtual*/abstract string GetLegalName(LegalNameFormat Format);

        //{
        //    switch (Format)
        //    {
        //        case LegalNameFormat.Full:

        //            return "";
        //        case LegalNameFormat.InitialsOnly:

        //            return "";
        //        case LegalNameFormat.SurnamesOnly:

        //            return "";
        //        default:
        //            return "";
        //    }
        //}

        private bool _isUpdatable = true;
        private bool _updatableDetermined = false;

        public bool IsUpdatable
        {
            get
            {
                if (_updatableDetermined == false)
                {
                    CheckUpdateOpenAccount();
                }

                return _isUpdatable;
            }
            set
            {
                _isUpdatable = value;
                _updatableDetermined = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        public ILifeInsurableInterest GetInsurableInterest(int AccountKey)
        {
            string HQL = "from LifeInsurableInterest_DAO LII where LII.Account.Key=? and LII.LegalEntity.Key=?";
            SimpleQuery<LifeInsurableInterest_DAO> query = new SimpleQuery<LifeInsurableInterest_DAO>(HQL, AccountKey, this.Key);
            LifeInsurableInterest_DAO[] lifeInsurableInterests = LifeInsurableInterest_DAO.ExecuteQuery(query) as LifeInsurableInterest_DAO[];
            if (lifeInsurableInterests != null && lifeInsurableInterests.Length > 0)
                return new LifeInsurableInterest(lifeInsurableInterests[0]);
            else
                return null;
        }

        /// <summary>
        /// Gets the IRole for the legal entity on the specified Account.
        /// </summary>
        /// <remarks></remarks>
        ///

        public IRole GetRole(int accountKey)
        {
            IRole _role = null;

            Account_DAO _account = Account_DAO.TryFind(accountKey);

            if (_account == null)
                return null;

            foreach (IRole role in this.Roles)
            {
                if (role.Account.Key == accountKey)
                {
                    _role = role;
                    break;
                }
            }

            return _role;
        }

        /// <summary>
        /// Gets the IApplicationRole of group Client for the legal entity on the specified Application.
        /// </summary>
        public IApplicationRole GetApplicationRoleClient(int applicationKey)
        {
            IApplicationRole applicationRole = null;

            // use an HQL query as some legal entities can have thousands of roles (e.g. Attornies)
            string hql = "from ApplicationRole_DAO ar where ar.LegalEntityKey = ? and ar.ApplicationKey = ? and ar.ApplicationRoleType.ApplicationRoleTypeGroup.Key = ?";
            SimpleQuery<ApplicationRole_DAO> query = new SimpleQuery<ApplicationRole_DAO>(hql, this.Key, applicationKey, (int)OfferRoleTypeGroups.Client);
            ApplicationRole_DAO[] daoRoles = query.Execute();
            if (daoRoles.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                applicationRole = bmtm.GetMappedType<IApplicationRole>(daoRoles[0]);
            }

            return applicationRole;
        }

        /// <summary>
        /// Gets a list of all application roles for the legal entity for a supplied list of role type groups.
        /// </summary>
        /// <param name="roleTypeGroups"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IApplicationRole> GetApplicationRolesByRoleTypeGroups(params OfferRoleTypeGroups[] roleTypeGroups)
        {
            string hql = UIStatementRepository.GetStatement("LegalEntity", "GetApplicationRolesByRoleTypeGroups");
            SimpleQuery<ApplicationRole_DAO> query = new SimpleQuery<ApplicationRole_DAO>(hql);
            // query.AddSqlReturnDefinition(typeof(ApplicationRole_DAO), "ofr");
            query.SetParameterList("OfferRoleTypeGroups", roleTypeGroups);
            query.SetParameter("LegalEntityKey", this.Key);
            ApplicationRole_DAO[] daoRoles = query.Execute();

            DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole> result = new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(daoRoles);
            return new ReadOnlyEventList<IApplicationRole>(result);
        }

        /// <summary>
        /// Gets a list of all application roles for the legal entity for a supplied list of role types.
        /// </summary>
        /// <param name="roleTypes"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IApplicationRole> GetApplicationRolesByRoleTypes(params OfferRoleTypes[] roleTypes)
        {
            string hql = UIStatementRepository.GetStatement("LegalEntity", "GetApplicationRolesByRoleTypes");
            SimpleQuery<ApplicationRole_DAO> query = new SimpleQuery<ApplicationRole_DAO>(hql);
            // query.AddSqlReturnDefinition(typeof(ApplicationRole_DAO), "ofr");
            query.SetParameterList("OfferRoleTypes", roleTypes);
            query.SetParameter("LegalEntityKey", this.Key);
            ApplicationRole_DAO[] daoRoles = query.Execute();

            DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole> result = new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(daoRoles);
            return new ReadOnlyEventList<IApplicationRole>(result);
        }

        /// <summary>
        /// Gets a list of all application roles for the legal entity for a particular application.
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IApplicationRole> GetApplicationRolesByApplicationKey(int applicationKey)
        {
            string hql = UIStatementRepository.GetStatement("LegalEntity", "GetApplicationRolesByApplicationKey");
            SimpleQuery<ApplicationRole_DAO> query = new SimpleQuery<ApplicationRole_DAO>(hql, applicationKey, this.Key);
            ApplicationRole_DAO[] daoRoles = query.Execute();

            DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole> result = new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(daoRoles);
            return new ReadOnlyEventList<IApplicationRole>(result);

            // LegalEntity_DAO le;
            // le.App
        }

        /// <summary>
        ///
        /// </summary>
        public bool UnderDebtCounselling
        {
            get
            {
                return (DebtCounsellingCases != null);
            }
        }

        /// <summary>
        /// Get active Debt Counselling cases that the Legal Entity plays an active role in
        /// </summary>
        public IEventList<IDebtCounselling> DebtCounsellingCases
        {
            get
            {
                string sql = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "UnderDebtCounsellingByLegalEntityKey");
                SimpleQuery<DebtCounselling_DAO> query = new SimpleQuery<DebtCounselling_DAO>(QueryLanguage.Sql, sql, this.Key);
                query.AddSqlReturnDefinition(typeof(DebtCounselling_DAO), "dc");
                DebtCounselling_DAO[] cases = query.Execute();

                if (cases != null && cases.Length > 0)
                {
                    DAOEventList<DebtCounselling_DAO, IDebtCounselling, DebtCounselling> result = new DAOEventList<DebtCounselling_DAO, IDebtCounselling, DebtCounselling>(cases);
                    return new EventList<IDebtCounselling>(result);
                }
                return null;
            }
        }

        public IApplicationUnsecuredLending PersonalLoanApplication
        {
            get
            {
                var castleTransactionService = new CastleTransactionsService();

                string sql = @"select offer.*
							   from ExternalRole er
							   join Offer offer on er.GenericKey = offer.OfferKey and offer.OfferTypeKey = 11
							   where er.LegalEntityKey = ?";

                var application = castleTransactionService.Single<IApplication>(QueryLanguages.Sql, sql, "offer", Databases.TwoAM, this.Key);
                return application as IApplicationUnsecuredLending;
            }
        }

        public ILegalEntityAddress ActiveDomiciliumAddress
        {
            get
            {
                if (LegalEntityAddresses == null || LegalEntityAddresses.Count() == 0)
                    return null;
                var legalEntityAddress = LegalEntityAddresses.FirstOrDefault(x => x.IsActiveDomicilium);
                return legalEntityAddress;
            }
        }

        public ILegalEntityDomicilium ActiveDomicilium
        {
            get
            {
                if (ActiveDomiciliumAddress == null)
                    return null;
                return ActiveDomiciliumAddress.LegalEntityDomiciliums.FirstOrDefault(x => x.LegalEntityAddress.Key == ActiveDomiciliumAddress.Key);
            }
        }

        /// <summary>
        /// Returns whether or not the Legal Entity can be updated depending on whether or not
        /// (s)he has open accounts and/or the exception status loaded.
        /// As per rule spec: Domain Legal Entity.
        /// </summary>
        /// <returns></returns>
        private bool CheckUpdateOpenAccount()
        {
            _isUpdatable = true;
            _updatableDetermined = true;

            if (!(this.LegalEntityExceptionStatus != null && this.LegalEntityExceptionStatus.Key == (int)LegalEntityExceptionStatuses.InvalidIDNumber))
            {
                foreach (IRole role in this.Roles)
                {
                    if (role.Account.AccountStatus.Key == (int)AccountStatuses.Open
                        && role.Account is IMortgageLoanAccount)
                    {
                        _isUpdatable = false;
                        break;
                    }
                }
            }

            return _isUpdatable;
        }

        protected virtual void OnLegalEntityMemos_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityMemos_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityAddresses_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityAddresses_BeforeRemove(ICancelDomainArgs args, object Item)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            int rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityAddressDoNotDeleteOnRole", this);
            if (rulePassed == 0)
                args.Cancel = true;

            //check if the LE is a role on account for which the address is a domicilium
            rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityAddressCantBeMadeInactiveIfLinkedToALegalEntityDomicilium", Item);
            if (rulePassed == 0)
                args.Cancel = true;

            //If we are cancelling a request, throw an error so that we know it has been canccelled.
            //This needs to be done after all items have been validated so that multiple/all messages are
            //reported to the UI
            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();
        }

        protected virtual void OnLegalEntityLogins_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityLogins_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityLogins_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityLogins_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnFailedLegalEntityAddresses_BeforeAdd(ICancelDomainArgs args, object Item)
        {
            // always prevent new items from being added
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IDomainMessageCollection dmc = spc.DomainMessages;
            dmc.Add(new Error("Dirty legal entity addresses cannot be added.", ""));
            args.Cancel = true;
        }

        protected virtual void OnFailedLegalEntityAddresses_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnEmployment_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnEmployment_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityAffordabilities_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityAffordabilities_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityAssetLiabilities_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityAssetLiabilities_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityBankAccounts_BeforeAdd(ICancelDomainArgs args, object Item)
        {
            // Business Rule: A bank account may be linked once to a legal entity
            ILegalEntityBankAccount NewLegalEntityBankAccount = Item as ILegalEntityBankAccount;
            foreach (ILegalEntityBankAccount legalEntityBankAccount in this.LegalEntityBankAccounts)
            {
                if (legalEntityBankAccount.BankAccount.Key == NewLegalEntityBankAccount.BankAccount.Key)
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    string errorMessage = "A Valid Bank Account may only be linked once.";
                    spc.DomainMessages.Add(new Error(errorMessage, errorMessage));
                    args.Cancel = true;
                }
            }

            //If we are cancelling a request, throw an error so that we know it has been canccelled.
            //This needs to be done after all items have been validated so that multiple/all messages are
            //reported to the UI
            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();

            // Business Rule: If an inactive Bank account is being added, reactivate the Bank Account if inactive
            if (NewLegalEntityBankAccount.GeneralStatus.Key == (int)GeneralStatuses.Inactive)
            {
                ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                IGeneralStatus activeGeneralStatus = lookupRepository.GeneralStatuses[GeneralStatuses.Active];

                NewLegalEntityBankAccount.GeneralStatus = activeGeneralStatus;
            }
        }

        protected virtual void OnLegalEntityBankAccounts_BeforeRemove(ICancelDomainArgs args, object Item)
        {
            ILegalEntityBankAccount leba = Item as ILegalEntityBankAccount;
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            int rulePassed = svc.ExecuteRule(spc.DomainMessages, "BankAccountDebitOrderDoNotDelete", leba);
            if (rulePassed == 0)
                args.Cancel = true;

            //If we are cancelling a request, throw an error so that we know it has been canccelled.
            //This needs to be done after all items have been validated so that multiple/all messages are
            //reported to the UI
            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();
        }

        protected virtual void OnLegalEntityMarketingOptions_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityMarketingOptions_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnRoles_BeforeAdd(ICancelDomainArgs args, object Item)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            // warning validation : check that a legalentity does not play a role on another account/application belong to a different origination source
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            int rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityOriginationSource", this);
            if (rulePassed == 0)
                args.Cancel = true;

            //If we are cancelling a request, throw an error so that we know it has been canccelled.
            //This needs to be done after all items have been validated so that multiple/all messages are
            //reported to the UI
            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();
        }

        protected virtual void OnRoles_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityRelationships_BeforeAdd(ICancelDomainArgs args, object Item)
        {
            ILegalEntityRelationship legalEntityRelationship = Item as ILegalEntityRelationship;

            // fire rules
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            // check that a legalentity is not added as a relationship to itself
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            int rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityRelationshipCircular", legalEntityRelationship);
            if (rulePassed == 0)
                args.Cancel = true;

            //If we are cancelling a request, throw an error so that we know it has been canccelled.
            //This needs to be done after all items have been validated so that multiple/all messages are
            //reported to the UI
            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();
        }

        protected virtual void OnLegalEntityRelationships_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnApplicationRoles_BeforeAdd(ICancelDomainArgs args, object Item)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            // warning validation : check that a legalentity does not play a role on another account/application belong to a different origination source
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            int rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityOriginationSource", this);
            if (rulePassed == 0)
                args.Cancel = true;

            //If we are cancelling a request, throw an error so that we know it has been canccelled.
            //This needs to be done after all items have been validated so that multiple/all messages are
            //reported to the UI
            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();
        }

        protected virtual void OnApplicationRoles_BeforeRemove(ICancelDomainArgs args, object Item)
        {
            IApplicationRole applicationRole = Item as IApplicationRole;

            // if the maling address of the application belongs to the legalentity we are removing and it does not
            // belong to anothe legalentity on the application then stop the role from being removed
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            int rulePassed = svc.ExecuteRule(spc.DomainMessages, "ValidateRemovedRoleMailingAddress", applicationRole);
            if (rulePassed == 0)
                args.Cancel = true;

            // Nazir J - 2008.07.28
            svc = ServiceFactory.GetService<IRuleService>();
            rulePassed = svc.ExecuteRule(spc.DomainMessages, "ApplicationRoleRemoveLegalEntityMinimum", applicationRole);
            if (rulePassed == 0)
                args.Cancel = true;

            //If we are cancelling a request, throw an error so that we know it has been canccelled.
            //This needs to be done after all items have been validated so that multiple/all messages are
            //reported to the UI
            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();
        }

        protected virtual void OnLegalEntityMemos_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityMemos_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityAddresses_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityAddresses_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnFailedLegalEntityAddresses_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnFailedLegalEntityAddresses_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnEmployment_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnEmployment_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityAffordabilities_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityAffordabilities_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityAssetLiabilities_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityAssetLiabilities_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityBankAccounts_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityBankAccounts_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityMarketingOptions_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityMarketingOptions_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnRoles_AfterAdd(ICancelDomainArgs args, object Item)
        {
            // Nazir J => 2008-07-15
            CheckUpdateOpenAccount();
        }

        protected virtual void OnRoles_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityRelationships_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnLegalEntityRelationships_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnApplicationRoles_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnApplicationRoles_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnITCs_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnITCs_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnITCs_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnITCs_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected static void PrintErrorMessage(string msg)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Add(new Error(msg, msg));
        }

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            //Rules.Add("CheckFurtherLendingApplicationRoleBeforeUpdate");
            //  Natural Person rules Required differs from Company/CC/Trust
            //  Rules.Add("LegalEntityMandatoryName");
            //  Rules.Add("LegalEntityContactDetailsMandatory");
            // Rules.Add("AffordabilityAtLeastOneIncome");
        }

        private DAOEventList<DebtCounsellorDetail_DAO, IDebtCounsellorDetail, DebtCounsellorDetail> _debtCounsellorDetails;

        /// <summary>
        /// A Legal Entity can play many Roles in many different Applications at SA Home Loans. The OfferRole table stores a foreign key
        /// reference to the Legal Entity and links it to the Application.
        /// </summary>
        public IDebtCounsellorDetail DebtCounsellorDetail
        {
            get
            {
                if (null == _debtCounsellorDetails)
                {
                    if (null == _DAO.DebtCounsellorDetails)
                        _DAO.DebtCounsellorDetails = new List<DebtCounsellorDetail_DAO>();
                    _debtCounsellorDetails = new DAOEventList<DebtCounsellorDetail_DAO, IDebtCounsellorDetail, DebtCounsellorDetail>(_DAO.DebtCounsellorDetails);
                }
                return _debtCounsellorDetails.Count == 0 ? null : _debtCounsellorDetails[0];
            }
            set
            {
                if (value == null)
                {
                    _DAO.DebtCounsellorDetails[0] = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                {
                    if (_DAO.DebtCounsellorDetails.Count == 0)
                    {
                        _DAO.DebtCounsellorDetails.Add((DebtCounsellorDetail_DAO)obj.GetDAOObject());
                    }
                    else
                    {
                        _DAO.DebtCounsellorDetails[0] = (DebtCounsellorDetail_DAO)obj.GetDAOObject();
                    }
                }
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// Gets the active external role of group Client for the legal entity on the specified Application.
        /// </summary>
        public IExternalRole GetActiveClientExternalRoleForOffer(int applicationKey)
        {
            IExternalRole externalRole = null;

            string hql = "from ExternalRole_DAO er where er.LegalEntity.Key = ? and er.GenericKey = ? and er.GenericKeyType.Key = ? and er.ExternalRoleType.ExternalRoleTypeGroup.Key = ? and er.GeneralStatus.Key = ?";
            SimpleQuery<ExternalRole_DAO> query = new SimpleQuery<ExternalRole_DAO>(hql, this.Key, applicationKey, (int)GenericKeyTypes.Offer, (int)ExternalRoleTypeGroups.Client, (int)GeneralStatuses.Active);
            ExternalRole_DAO[] daoRoles = query.Execute();
            if (daoRoles.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                externalRole = bmtm.GetMappedType<IExternalRole>(daoRoles[0]);
            }

            return externalRole;
        }
    }
}