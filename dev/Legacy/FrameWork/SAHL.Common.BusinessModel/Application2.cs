using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class Application : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Application_DAO>, IApplication
    {
        private IApplicationStatus _applicationStatusPrevious;

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("ApplicationOpenSave");

            //Cannot run rule "ApplicationCreateLegalEntityMinimum" as when
            //creating an empty Application it will fail.
            //Rules.Add("ApplicationCreateLegalEntityMinimum");
            //Rules.Add("QuickCashCashOutReduce");
        }

        public override void ExtendedConstructor()
        {
            base.ExtendedConstructor();
            _applicationStatusPrevious = ApplicationStatus;
        }

        protected IApplicationProduct _currentProduct;
        private IEventList<IApplicationRole> _applicationRoles;
        private ILookupRepository _lookupRepo;
        private IStageDefinitionRepository _sdRepo;
        private IApplicationRepository applicationRepository;

        public IApplicationInformation GetLatestApplicationInformation()
        {
            DateTime maxDate = DateTime.MinValue;
            int idx = 0;

            for (int i = 0; i < this.ApplicationInformations.Count; i++)
            {
                if (this.ApplicationInformations[i].ApplicationInsertDate >= maxDate)
                {
                    maxDate = this.ApplicationInformations[i].ApplicationInsertDate;
                    idx = i;
                }
            }

            //ApplicationInformation_DAO
            //string HQL = "SELECT ai FROM ApplicationInformation_DAO ai WHERE ai.ApplicationKey = ? ORDER BY ai.Key desc";

            //SimpleQuery<ApplicationInformation_DAO> q = new SimpleQuery<ApplicationInformation_DAO>(HQL, this.Key);
            //ApplicationInformation_DAO[] list = q.Execute();

            //if (list.Length > 0)
            //    return new ApplicationInformation(list[0]);

            if (this.ApplicationInformations.Count > 0)
                return this.ApplicationInformations[idx];

            return null;
        }

        /// <summary>
        /// This property handles dealing with the ApplicationRoles collection internally, and
        /// is required so the DAO and BusinessModel object stay in sync.  Any application role
        /// manipulation should be done using this.
        /// </summary>
        private IEventList<IApplicationRole> ApplicationRolesInternal
        {
            get
            {
                if (null == _applicationRoles)
                {
                    if (null == _DAO.ApplicationRoles)
                        _DAO.ApplicationRoles = new List<ApplicationRole_DAO>();
                    _applicationRoles = new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(_DAO.ApplicationRoles);
                    _applicationRoles.BeforeAdd += new EventListHandler(OnApplicationRoles_BeforeAdd);
                    _applicationRoles.BeforeRemove += new EventListHandler(OnApplicationRoles_BeforeRemove);
                    _applicationRoles.AfterAdd += new EventListHandler(OnApplicationRoles_AfterAdd);
                    _applicationRoles.AfterRemove += new EventListHandler(OnApplicationRoles_AfterRemove);
                }
                return _applicationRoles;
            }
        }

        /// <summary>
        /// Gets the application roles associated with the Application.  This is read-only, so
        /// roles cannot be added or removed.
        /// </summary>
        public IReadOnlyEventList<IApplicationRole> ApplicationRoles
        {
            get
            {
                return new ReadOnlyEventList<IApplicationRole>(ApplicationRolesInternal);
            }
        }

        /// <summary>
        /// Gets the application status when the object is first loaded.  This will not change during
        /// the lifetime of the object.  If you wish to change the status of the application and
        /// persist, use <see cref="ApplicationStatus"/>.  For newly created applications, this will
        /// always be null.
        /// </summary>
        public IApplicationStatus ApplicationStatusPrevious
        {
            get
            {
                return _applicationStatusPrevious;
            }
        }

        public bool IsOpen
        {
            //Need to establish how to check if application is open
            //110 NTUFinalized
            //111 Declined

            get
            {
                //Open = 1,
                //Closed = 2,
                //Accepted = 3,
                //NTU = 4,
                //Declined = 5

                switch (this.ApplicationStatus.Key)
                {
                    case (int)SAHL.Common.Globals.OfferStatuses.Open:
                        return true;

                    case (int)SAHL.Common.Globals.OfferStatuses.Closed:
                        return false;

                    case (int)SAHL.Common.Globals.OfferStatuses.NTU:
                    case (int)SAHL.Common.Globals.OfferStatuses.Declined:
                        IStageDefinitionRepository SDRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                        if (SDRepo.CheckCompositeStageDefinition(this.Key, (int)StageDefinitionStageDefinitionGroups.NTUOffer) ||
                            SDRepo.CheckCompositeStageDefinition(this.Key, (int)StageDefinitionStageDefinitionGroups.DeclineOffer) ||
                            SDRepo.CheckCompositeStageDefinition(this.Key, (int)StageDefinitionStageDefinitionGroups.FL45DayTimer))
                            return false;
                        else
                            return true;

                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Gets all the ApplicationRoles of the specified type for this Application
        /// </summary>
        /// <param name="OfferRoleType"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IApplicationRole> GetApplicationRolesByType(SAHL.Common.Globals.OfferRoleTypes OfferRoleType)
        {
            int roleTypeKey = (int)OfferRoleType;

            List<ApplicationRole_DAO> list = new List<ApplicationRole_DAO>();

            for (int i = 0; i < this.ApplicationRoles.Count; i++)
            {
                if (this.ApplicationRoles[i].ApplicationRoleType.Key == roleTypeKey)
                {
                    list.Add(((IDAOObject)this.ApplicationRoles[i]).GetDAOObject() as ApplicationRole_DAO);
                }
            }

            IEventList<IApplicationRole> evList = new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(list);
            return new ReadOnlyEventList<IApplicationRole>(evList);
        }

        /// <summary>
        /// Gets all the ApplicationRoles of the specified type for this Application based on GeneralStatus
        /// </summary>
        /// <param name="OfferRoleType"></param>
        /// <param name="GeneralStatus"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IApplicationRole> GetApplicationRolesByType(SAHL.Common.Globals.OfferRoleTypes OfferRoleType, GeneralStatuses GeneralStatus)
        {
            int roleTypeKey = (int)OfferRoleType;

            List<ApplicationRole_DAO> list = new List<ApplicationRole_DAO>();

            for (int i = 0; i < this.ApplicationRoles.Count; i++)
            {
                if (this.ApplicationRoles[i].GeneralStatus.Key == (int)GeneralStatus &&
                    this.ApplicationRoles[i].ApplicationRoleType.Key == roleTypeKey)
                {
                    list.Add(((IDAOObject)this.ApplicationRoles[i]).GetDAOObject() as ApplicationRole_DAO);
                }
            }

            IEventList<IApplicationRole> evList = new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(list);
            return new ReadOnlyEventList<IApplicationRole>(evList);
        }

        public IApplicationRole GetLatestApplicationRoleByType(SAHL.Common.Globals.OfferRoleTypes OfferRoleType)
        {
            //http://sahls31:8181/trac/SAHL.db/ticket/12791
            //IApplicationRole _currentRole = null;
            //IReadOnlyEventList<IApplicationRole> _roles = GetApplicationRolesByType(OfferRoleType);

            //if (_roles.Count > 0)
            //    _currentRole = _roles[0];

            //for (int i = 0; i < _roles.Count; i++)
            //{
            //    if (_currentRole.Key < _roles[i].Key)
            //        _currentRole = _roles[i];
            //}
            //return _currentRole;

            IApplicationRole _currentRole = null;
            IReadOnlyEventList<IApplicationRole> _roles = GetApplicationRolesByType(OfferRoleType, GeneralStatuses.Active);

            // Latest Active Role
            if (_roles.Count > 0)
                _currentRole = _roles[0];

            for (int i = 0; i < _roles.Count; i++)
            {
                if (_currentRole.StatusChangeDate < _roles[i].StatusChangeDate)
                    _currentRole = _roles[i];
            }

            if (_currentRole == null)
            {
                // Latest Inactive Role
                _roles = GetApplicationRolesByType(OfferRoleType, GeneralStatuses.Inactive);

                if (_roles.Count > 0)
                    _currentRole = _roles[0];

                for (int i = 0; i < _roles.Count; i++)
                {
                    if (_currentRole.StatusChangeDate < _roles[i].StatusChangeDate)
                        _currentRole = _roles[i];
                }
            }
            return _currentRole;
        }

        public IApplicationRole GetFirstApplicationRoleByType(SAHL.Common.Globals.OfferRoleTypes OfferRoleType)
        {
            IApplicationRole _firstRole = null;
            IReadOnlyEventList<IApplicationRole> _roles = GetApplicationRolesByType(OfferRoleType);

            if (_roles.Count > 0)
                _firstRole = _roles[0];

            for (int i = 0; i < _roles.Count; i++)
            {
                if (_firstRole.Key > _roles[i].Key)
                    _firstRole = _roles[i];
            }
            return _firstRole;
        }

        /// <summary>
        /// Gets all the ApplicationRoles of the specified group for this Application
        /// </summary>
        /// <param name="OfferRoleTypeGroup"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IApplicationRole> GetApplicationRolesByGroup(SAHL.Common.Globals.OfferRoleTypeGroups OfferRoleTypeGroup)
        {
            int roleTypeGroupKey = (int)OfferRoleTypeGroup;

            List<ApplicationRole_DAO> list = new List<ApplicationRole_DAO>();

            for (int i = 0; i < this.ApplicationRoles.Count; i++)
            {
                if (this.ApplicationRoles[i].ApplicationRoleType.ApplicationRoleTypeGroup.Key == roleTypeGroupKey)
                {
                    list.Add(((IDAOObject)this.ApplicationRoles[i]).GetDAOObject() as ApplicationRole_DAO);
                }
            }

            // Refer to on the reason for removal - http://sahls31:8181/trac/SAHL.db/ticket/12325
            /*string sql = @"select ofr.*
            from [2am].[dbo].[OfferRole] ofr
            inner join [2am].[dbo].[OfferRoleType] ofrt
            on ofr.offerRoleTypeKey = ofrt.OfferRoleTypeKey
            where ofr.OfferKey = ? and ofrt.OfferRoleTypeGroupKey = ?";

            SimpleQuery<ApplicationRole_DAO> roleQ = new SimpleQuery<ApplicationRole_DAO>(QueryLanguage.Sql,sql, this.Key, roleTypeGroupKey);
            roleQ.AddSqlReturnDefinition(typeof(ApplicationRole_DAO), "ofr");
            ApplicationRole_DAO[] res = roleQ.Execute();

            foreach (ApplicationRole_DAO _appRole in res)
            {
                list.Add(_appRole);
            }
            */
            IEventList<IApplicationRole> evList = new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(list);
            return new ReadOnlyEventList<IApplicationRole>(evList);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="OfferRoleTypeGroup"></param>
        /// <returns></returns>
        public IApplicationRole GetLatestApplicationRoleByGroup(SAHL.Common.Globals.OfferRoleTypeGroups OfferRoleTypeGroup)
        {
            IApplicationRole _latestApplicationRole = null;
            IReadOnlyEventList<IApplicationRole> applicationRoles = GetApplicationRolesByGroup(OfferRoleTypeGroup);

            if (applicationRoles.Count > 0)
                _latestApplicationRole = applicationRoles[0];

            for (int i = 0; i < applicationRoles.Count; i++)
            {
                if (_latestApplicationRole.Key < applicationRoles[i].Key)
                    _latestApplicationRole = applicationRoles[i];
            }
            return _latestApplicationRole;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="OfferRoleTypeGroup"></param>
        /// <returns></returns>
        public IApplicationRole GetFirstApplicationRoleByGroup(SAHL.Common.Globals.OfferRoleTypeGroups OfferRoleTypeGroup)
        {
            IApplicationRole _firstApplicationRole = null;
            IReadOnlyEventList<IApplicationRole> applicationRoles = this.GetApplicationRolesByGroup(OfferRoleTypeGroup);

            if (applicationRoles.Count > 0)
                _firstApplicationRole = applicationRoles[0];

            for (int i = 0; i < applicationRoles.Count; i++)
            {
                if (_firstApplicationRole.Key > applicationRoles[i].Key)
                    _firstApplicationRole = applicationRoles[i];
            }
            return _firstApplicationRole;
        }

        /// <summary>
        /// Retrieves the current product for the application.
        /// </summary>
        public virtual IApplicationProduct CurrentProduct
        {
            get
            {
                if (null == _currentProduct)
                {
                    switch (GetLatestApplicationInformation().Product.Key)
                    {
                        case (int)SAHL.Common.Globals.Products.VariableLoan:
                            _currentProduct = new ApplicationProductVariableLoan(this, false);
                            break;

                        case (int)SAHL.Common.Globals.Products.VariFixLoan:
                            _currentProduct = new ApplicationProductVariFixLoan(this, false);
                            break;

                        case (int)SAHL.Common.Globals.Products.HomeOwnersCover:
                            break;

                        case (int)SAHL.Common.Globals.Products.LifePolicy:
                            break;

                        case (int)SAHL.Common.Globals.Products.SuperLo:
                            _currentProduct = new ApplicationProductSuperLoLoan(this, false);
                            break;

                        case (int)SAHL.Common.Globals.Products.DefendingDiscountRate:
                            _currentProduct = new ApplicationProductDefendingDiscountLoan(this, false);
                            break;

                        case (int)SAHL.Common.Globals.Products.NewVariableLoan:
                            _currentProduct = new ApplicationProductNewVariableLoan(this, false);
                            break;

                        case (int)SAHL.Common.Globals.Products.QuickCash:
                            throw new Exception("Quickcash product should not be instantiated.");
                        case (int)SAHL.Common.Globals.Products.Edge:
                            _currentProduct = new ApplicationProductEdge(this, false);
                            break;

                        case (int)SAHL.Common.Globals.Products.PersonalLoan:
                            _currentProduct = new ApplicationProductPersonalLoan(this, false);
                            break;
                    }
                }
                return _currentProduct;
            }
        }

        /// <summary>
        /// Retrieves the product history.
        /// </summary>
        public IApplicationProduct[] ProductHistory
        {
            get
            {
                int cnt = this.ApplicationInformations.Count;
                IApplicationProduct[] _ApplicationProducts = new IApplicationProduct[_ApplicationInformations.Count];

                ApplicationInformation currentAppInfo;
                for (int i = 0; i < _ApplicationInformations.Count; i++)
                {
                    currentAppInfo = _ApplicationInformations[i] as ApplicationInformation;

                    switch (currentAppInfo.Product.Key)
                    {
                        case (int)SAHL.Common.Globals.Products.VariableLoan:
                            _ApplicationProducts[i] = new ApplicationProductVariableLoan(currentAppInfo);
                            break;

                        case (int)SAHL.Common.Globals.Products.VariFixLoan:
                            _ApplicationProducts[i] = new ApplicationProductVariFixLoan(currentAppInfo);
                            break;

                        case (int)SAHL.Common.Globals.Products.HomeOwnersCover:
                            break;

                        case (int)SAHL.Common.Globals.Products.LifePolicy:
                            break;

                        case (int)SAHL.Common.Globals.Products.SuperLo:
                            _ApplicationProducts[i] = new ApplicationProductSuperLoLoan(currentAppInfo);
                            break;

                        case (int)SAHL.Common.Globals.Products.DefendingDiscountRate:
                            _ApplicationProducts[i] = new ApplicationProductDefendingDiscountLoan(currentAppInfo);
                            break;

                        case (int)SAHL.Common.Globals.Products.NewVariableLoan:
                            _ApplicationProducts[i] = new ApplicationProductNewVariableLoan(currentAppInfo);
                            break;

                        case (int)SAHL.Common.Globals.Products.Edge:
                            _ApplicationProducts[i] = new ApplicationProductEdge(currentAppInfo);
                            break;

                        case (int)SAHL.Common.Globals.Products.QuickCash:
                            throw new Exception("Quickcash product should not be instantiated.");
                    }
                }

                return _ApplicationProducts;
            }
        }

        /// <summary>
        /// returns the Active Estate Agent roles LegalEntity and related
        /// organisation structures at the time the role was assigned
        /// </summary>
        /// <param name="EstateAgent">The Active Estate Agent Role's LE</param>
        /// <param name="Company">The Active Estate Agents Company when the role was created</param>
        /// <param name="Branch">The Active Estate Agents Branch when the role was created</param>
        /// <param name="Principal">The Active Estate Agents Principal when the role was created</param>
        public void GetEsateAgentDetails(out ILegalEntity EstateAgent, out ILegalEntity Company, out ILegalEntity Branch, out ILegalEntity Principal)
        {
            foreach (IApplicationRole ar in this.ApplicationRoles)
            {
                if (ar.ApplicationRoleType.Key == (int)OfferRoleTypes.EstateAgent && ar.GeneralStatus.Key == (int)GeneralStatuses.Active)
                {
                    EstateAgent = ar.LegalEntity;
                    IEstateAgentRepository eaRepo = RepositoryFactory.GetRepository<IEstateAgentRepository>();

                    eaRepo.GetEstateAgentInfoWithHistory(ar.LegalEntity.Key, ar.StatusChangeDate, out Company, out Branch, out Principal);

                    return;
                }
            }

            EstateAgent = null;
            Company = null;
            Branch = null;
            Principal = null;
            return;
        }

        /// <summary>
        /// Creates an application revision.
        /// </summary>
        public void CreateRevision()
        {
            switch (GetLatestApplicationInformation().Product.Key)
            {
                case (int)SAHL.Common.Globals.Products.VariableLoan:
                    _currentProduct = new ApplicationProductVariableLoan(this, true);
                    break;

                case (int)SAHL.Common.Globals.Products.VariFixLoan:
                    SAHL.Common.BusinessModel.Helpers.ApplicationProductMortgageLoanHelper.SetConversionStatus(this, (int)Products.VariFixLoan, true);
                    _currentProduct = new ApplicationProductVariFixLoan(this, true);
                    break;

                case (int)SAHL.Common.Globals.Products.HomeOwnersCover:
                    break;

                case (int)SAHL.Common.Globals.Products.LifePolicy:
                    break;

                case (int)SAHL.Common.Globals.Products.SuperLo:
                    SAHL.Common.BusinessModel.Helpers.ApplicationProductMortgageLoanHelper.SetConversionStatus(this, (int)Products.SuperLo, true);
                    _currentProduct = new ApplicationProductSuperLoLoan(this, true);
                    break;

                case (int)SAHL.Common.Globals.Products.DefendingDiscountRate:
                    _currentProduct = new ApplicationProductDefendingDiscountLoan(this, true);
                    break;

                case (int)SAHL.Common.Globals.Products.NewVariableLoan:
                    _currentProduct = new ApplicationProductNewVariableLoan(this, true);
                    break;

                case (int)SAHL.Common.Globals.Products.QuickCash:
                    throw new Exception("Quickcash product should not be instantiated.");
                case (int)SAHL.Common.Globals.Products.Edge:
                    _currentProduct = new ApplicationProductEdge(this, true);
                    break;

                case (int)SAHL.Common.Globals.Products.PersonalLoan:
                    _currentProduct = new ApplicationProductPersonalLoan(this, true);
                    break;
            }
        }

        public IReadOnlyEventList<ILegalEntity> GetLegalEntitiesByRoleType(SAHL.Common.Globals.OfferRoleTypes[] OfferRoleTypes)
        {
            return GetLegalEntitiesByRoleType(OfferRoleTypes, GeneralStatusKey.All);
        }

        public IReadOnlyEventList<ILegalEntity> GetLegalEntitiesByRoleType(SAHL.Common.Globals.OfferRoleTypes[] OfferRoleTypes, GeneralStatusKey StatusKey)
        {
            ArrayList RTS = new ArrayList(OfferRoleTypes.Length);
            for (int i = 0; i < OfferRoleTypes.Length; i++)
            {
                RTS.Add(Convert.ToInt32(OfferRoleTypes[i]));
            }
            EventList<ILegalEntity> results = new EventList<ILegalEntity>();
            DomainMessageCollection DMC = new DomainMessageCollection();

            for (int i = 0; i < this.ApplicationRoles.Count; i++)
            {
                if (RTS.Contains(ApplicationRoles[i].ApplicationRoleType.Key))
                {
                    if (StatusKey == GeneralStatusKey.All)
                        results.Add(DMC, ApplicationRoles[i].LegalEntity);
                    else if (ApplicationRoles[i].GeneralStatus.Key == Convert.ToInt32(StatusKey))
                        results.Add(DMC, ApplicationRoles[i].LegalEntity);
                }
            }
            return new ReadOnlyEventList<ILegalEntity>(results);
        }

        public IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] OfferRoleTypes)
        {
            return GetNaturalPersonLegalEntitiesByRoleType(Messages, OfferRoleTypes, GeneralStatusKey.All);
        }

        /// <summary>
        /// Get External Roles for the Application
        /// NB: this is for non mortgage loan applications
        ///     i.e.: Personal Loans
        /// </summary>
        /// <param name="externalRoleType"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonsByExternalRoleType(SAHL.Common.Globals.ExternalRoleTypes externalRoleType, GeneralStatuses status)
        {
            EventList<ILegalEntityNaturalPerson> naturalPersons = new EventList<ILegalEntityNaturalPerson>();

            var legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            var externalRoles = legalEntityRepository.GetExternalRoles(this.Key, Globals.GenericKeyTypes.Offer, externalRoleType, status);

            foreach (var externalRole in externalRoles)
            {
                var naturalPerson = externalRole.LegalEntity as ILegalEntityNaturalPerson;

                if (naturalPerson != null)
                    naturalPersons.Add(null, naturalPerson);
            }

            return new ReadOnlyEventList<ILegalEntityNaturalPerson>(naturalPersons);
        }

        public IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] OfferRoleTypes, GeneralStatusKey StatusKey)
        {
            ArrayList RTS = new ArrayList(OfferRoleTypes.Length);
            for (int i = 0; i < OfferRoleTypes.Length; i++)
            {
                RTS.Add(Convert.ToInt32(OfferRoleTypes[i]));
            }
            EventList<ILegalEntityNaturalPerson> results = new EventList<ILegalEntityNaturalPerson>();
            DomainMessageCollection DMC = new DomainMessageCollection();

            for (int i = 0; i < this.ApplicationRoles.Count; i++)
            {
                if (RTS.Contains(ApplicationRoles[i].ApplicationRoleType.Key))
                {
                    if (ApplicationRoles[i].LegalEntity is ILegalEntityNaturalPerson)
                    {
                        if (StatusKey == GeneralStatusKey.All)
                            results.Add(DMC, ApplicationRoles[i].LegalEntity as ILegalEntityNaturalPerson);
                        else if (ApplicationRoles[i].GeneralStatus.Key == Convert.ToInt32(StatusKey))
                            results.Add(DMC, ApplicationRoles[i].LegalEntity as ILegalEntityNaturalPerson);
                    }
                }
            }
            return new ReadOnlyEventList<ILegalEntityNaturalPerson>(results);
        }

        public void AddLeadRole(Lead_MortgageLoan_Role roletype, ILegalEntity LE)
        {
            // run the LE rules for leads and any other rules that may need to be run.
            // SAHLPrincipalCache SPC = SAHLPrincipal.GetSAHLPrincipalCache();// .GetPrincipalCache(new SAHLPrincipal(WindowsIdentity.GetCurrent()));
            // IRuleService scv = ServiceFactory.GetService<IRuleService>();
            //scv.ExecuteRule(spc.DomainMessages, "", true, LE);

            // if rules pass add the suckers to the roles collection
            AddLegalEntityToRolesCollection(LE, Convert.ToInt32(roletype));
        }

        public void AddApplicationRole(Application_MortgageLoan_Role roletype, ILegalEntity LE)
        {
            // run the LE rules for leads and any other rules that may need to be run.
            // SAHLPrincipalCache SPC = SAHLPrincipal.GetSAHLPrincipalCache(); //SAHLPrincipalCache.GetPrincipalCache(new SAHLPrincipal(WindowsIdentity.GetCurrent()));
            IRuleService scv = ServiceFactory.GetService<IRuleService>();

            //scv.ExecuteRule(spc.DomainMessages, "", true, LE);

            // if rules pass check this guy isnt playing a lead role. If he is remove him from the Roles Collection as
            // a lead and add as an Application Role Type
            List<ApplicationRole_DAO> ToRemove = new List<ApplicationRole_DAO>();
            foreach (ApplicationRole_DAO role in _DAO.ApplicationRoles)
            {
                if ((role.LegalEntityKey == LE.Key) && (role.ApplicationRoleType.Key == (int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant || role.ApplicationRoleType.Key == (int)SAHL.Common.Globals.OfferRoleTypes.LeadSuretor))
                {
                    ToRemove.Add(role);
                }
            }
            foreach (ApplicationRole_DAO role in ToRemove)
            {
                bool b = _DAO.ApplicationRoles.Remove(role);
                if (!b)
                    throw new Exception("Unable to add applicant role. LegalEntity already a LEAD role but unable to remove the lead role.");
            }
            AddLegalEntityToRolesCollection(LE, Convert.ToInt32(roletype));
        }

        public IApplicationRole AddRole(int applicationRoleTypeKey, ILegalEntity LE)
        {
            return AddLegalEntityToRolesCollection(LE, applicationRoleTypeKey);
        }

        public void RemoveRolesForLegalEntity(IDomainMessageCollection Messages, ILegalEntity LegalEntity, int[] ApplicationRoleTypes)
        {
            ArrayList RTS = new ArrayList(ApplicationRoleTypes);

            bool removeRoleAllowed = true;

            // remove the roles from the application
            foreach (IApplicationRole role in this.ApplicationRoles)
            {
                if (RTS.Contains(role.ApplicationRoleType.Key) && role.LegalEntity.Key == LegalEntity.Key)
                {
                    // if the maling address of the application belongs to the legalentity we are removing and it does not
                    // belong to anothe legalentity on the application then stop the role from being removed
                    IRuleService svc = ServiceFactory.GetService<IRuleService>();
                    int rulePassed = svc.ExecuteRule(Messages, "ValidateRemovedRoleMailingAddress", role);
                    int ruleDebitOrderPassed = svc.ExecuteRule(Messages, "ValidateRemovedRoleDebitOrder", role);

                    if (rulePassed == 0 || ruleDebitOrderPassed == 0)
                    {
                        removeRoleAllowed = false;
                        break;
                    }

                    if (removeRoleAllowed)
                        if (ApplicationRolesInternal.Remove(Messages, role) == false)
                            return;
                }
            }

            if (removeRoleAllowed)
            {
                // remove the roles from the legalentity
                foreach (IApplicationRole role in LegalEntity.ApplicationRoles)
                {
                    if (RTS.Contains(role.ApplicationRoleType.Key) && role.ApplicationKey == this.Key)
                    {
                        LegalEntity.ApplicationRoles.Remove(Messages, role);

                        // roles will only be removed one at a time, so do the application updates here so we
                        // only run the common methods if a relevant client role type is being changed
                        // SO: for 'client' role types, perform any application updates that might be necessary
                        if (role.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)SAHL.Common.Globals.OfferRoleTypeGroups.Client)
                        {
                            //If the offer is open (statuskey == 1 and
                            //application information has not been accepted then update income
                            IApplicationInformation ai = this.GetLatestApplicationInformation();

                            if (this.IsOpen
                                && ai != null
                                && ai.ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                            {
                                // Recalculate
                                CalculateHouseHoldIncome();

                                // Set Application Applicant Type
                                SetApplicantType();

                                // And the employment type
                                SetEmploymentType();
                            }
                        }
                    }
                }
            }
        }

        private IApplicationRole AddLegalEntityToRolesCollection(ILegalEntity LE, int ApplicationRoleType)
        {
            IApplicationRoleType rt = null;

            if (ApplicationRoleType > 0)
            {
                ApplicationRoleType_DAO art = ApplicationRoleType_DAO.Find(ApplicationRoleType);
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();

                rt = bmtm.GetMappedType<IApplicationRoleType>(art);
            }
            IGeneralStatus gs = RepositoryFactory.GetRepository<ILookupRepository>().GeneralStatuses[GeneralStatuses.Active];

            IApplicationRole role = new ApplicationRole(new ApplicationRole_DAO());

            role.Application = this;
            role.ApplicationRoleType = rt;
            role.GeneralStatus = gs;
            role.LegalEntity = LE;
            role.StatusChangeDate = DateTime.Now;

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            // for 'client' role types run some vaLidation rules
            if (rt.ApplicationRoleTypeGroup.Key == (int)SAHL.Common.Globals.OfferRoleTypeGroups.Client)
            {
                IRuleService svc = ServiceFactory.GetService<IRuleService>();

                // check that a role for the legal entity does not already exist
                int rulePassed = svc.ExecuteRule(spc.DomainMessages, "ValidateUniqueClientRole", role);
                if (rulePassed == 0)
                    return null;

                // warning validation : check that a legalentity does not play a role on another account/application belong to a different origination source
                rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityOriginationSource", LE);
                if (rulePassed == 0)
                    return null;
            }

            if (!ApplicationRolesInternal.Add(spc.DomainMessages, role))
                return null;

            // for 'client' role types, perform any application updates that might be necessary
            if (rt.ApplicationRoleTypeGroup.Key == (int)SAHL.Common.Globals.OfferRoleTypeGroups.Client)
            {
                IApplicationMortgageLoan oml = role.Application as IApplicationMortgageLoan;
                IRuleService svcRule = ServiceFactory.GetService<IRuleService>();
                if (oml != null && oml.Property != null)
                {
                    //Removed rule for time being
                    svcRule.ExecuteRule(spc.DomainMessages, "LegalEntityDuplicateApplication", LE.Key, oml.Key, oml.Property.Key);
                    svcRule.ExecuteRule(spc.DomainMessages, "LegalEntityPropertyPreviousDecline", LE.Key, oml.Property.Key);
                }

                //If the offer is open (statuskey == 1 and
                //application information has not been accepted then update income
                IApplicationInformation ai = this.GetLatestApplicationInformation();
                if (this.IsOpen
                    && ai != null
                    && ai.ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                {
                    // Recalculate Household Income
                    CalculateHouseHoldIncome();

                    // Set Application Applicant Type
                    SetApplicantType();

                    // And the employment type
                    SetEmploymentType();
                }
            }

            return role;
        }

        public string GetLegalName(LegalNameFormat Format)
        {
            string legalName = "";

            // 1. check for roles on application using LeadMainApplicant, MainApplicant or AssuredLife
            int roleTypeKey = -1;
            switch (this.ApplicationType.Key)
            {
                case (int)SAHL.Common.Globals.OfferTypes.Unknown:
                    roleTypeKey = (int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant;
                    break;

                case (int)SAHL.Common.Globals.OfferTypes.Life:
                    roleTypeKey = (int)SAHL.Common.Globals.OfferRoleTypes.AssuredLife;
                    break;

                default:
                    roleTypeKey = (int)SAHL.Common.Globals.OfferRoleTypes.MainApplicant;
                    break;
            }

            legalName = FindApplicationLegalName(this, roleTypeKey, Format);

            if (String.IsNullOrEmpty(legalName))
            {
                // 2. if no roles found and application is not a life application
                //    then check for roles on application using Lead - MainApplicant
                if (this.ApplicationType.Key != (int)SAHL.Common.Globals.OfferTypes.Life)
                {
                    roleTypeKey = (int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant;
                    legalName = FindApplicationLegalName(this, roleTypeKey, Format);
                }

                if (String.IsNullOrEmpty(legalName))
                {
                    if (this.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.Life)
                        roleTypeKey = (int)SAHL.Common.Globals.RoleTypes.AssuredLife;
                    else
                        roleTypeKey = (int)SAHL.Common.Globals.RoleTypes.MainApplicant;

                    // 3. if no roles on application then check for roles on account (if there is one)
                    if (this.Account != null)
                    {
                        legalName = FindAccountLegalName(this.Account, roleTypeKey, Format);

                        if (String.IsNullOrEmpty(legalName))
                        {
                            // 4. if no roles on account then check for roles on parent account (if there is one)
                            if (this.Account.ParentAccount != null)
                            {
                                roleTypeKey = (int)SAHL.Common.Globals.RoleTypes.MainApplicant;
                                legalName = FindAccountLegalName(this.Account.ParentAccount, roleTypeKey, Format);
                            }
                        }
                    }
                }
            }

            return String.IsNullOrEmpty(legalName) ? "No Legal Name found" : legalName;
        }

        private string FindApplicationLegalName(IApplication application, int roleTypeKey, LegalNameFormat Format)
        {
            string legalName = "";
            IReadOnlyEventList<IApplicationRole> roles = application.ApplicationRoles;
            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].ApplicationRoleType.Key == roleTypeKey)
                {
                    if (roles[i].LegalEntity != null)
                        legalName += roles[i].LegalEntity.GetLegalName(Format);
                    if (i < roles.Count - 1)
                        legalName += " & ";
                }
            }
            if (legalName.EndsWith(" & "))
                legalName = legalName.Substring(0, legalName.Length - 3);

            return legalName;
        }

        private string FindAccountLegalName(IAccount account, int roleTypeKey, LegalNameFormat Format)
        {
            string legalName = "";
            IEventList<IRole> roles = account.Roles;
            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].RoleType.Key == roleTypeKey)
                {
                    if (roles[i].LegalEntity != null)
                        legalName += roles[i].LegalEntity.GetLegalName(Format);
                    if (i < roles.Count - 1)
                        legalName += " & ";
                }
            }
            if (legalName.EndsWith(" & "))
                legalName = legalName.Substring(0, legalName.Length - 3);

            return legalName;
        }

        protected void OnApplicationRoles_AfterAdd(ICancelDomainArgs args, object Item)
        {
            // Refresh the Number of Applicants
            if (this is IApplicationMortgageLoan)
                ApplicationMortgageLoanHelper.RefreshNumberofApplicants((IApplicationMortgageLoan)this);

            // Refresh the Applicant Type

            // Refresh the Household Income
        }

        protected void OnApplicationRoles_BeforeAdd(ICancelDomainArgs args, object Item)
        {
            if (this.ApplicationType != null && (this.ApplicationType.Key == (int)OfferTypes.ReAdvance || this.ApplicationType.Key == (int)OfferTypes.FurtherAdvance || this.ApplicationType.Key == (int)OfferTypes.FurtherLoan))
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                int rulePassed = svc.ExecuteRule(spc.DomainMessages, "OfferRoleFLAddMainApplicant", Item);

                if (rulePassed == 0)
                    args.Cancel = true;

                //If we are cancelling a request, throw an error so that we know it has been canccelled.
                //This needs to be done after all items have been validated so that multiple/all messages are
                //reported to the UI
                if (args.Cancel)
                    throw new SAHL.Common.Exceptions.DomainValidationException();
            }
        }

        protected void OnApplicationRoles_AfterRemove(ICancelDomainArgs args, object Item)
        {
            // Refresh the Number of Applicants
            if (this is IApplicationMortgageLoan)
                ApplicationMortgageLoanHelper.RefreshNumberofApplicants((IApplicationMortgageLoan)this);

            // Refresh the Applicant Type

            // Refresh the Household Income

            //Nazir J - 2008.07.28
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            int rulePassed = svc.ExecuteRule(spc.DomainMessages, "ApplicationCreateLegalEntityMinimum", this);

            if (rulePassed == 0)
                args.Cancel = true;

            //If we are cancelling a request, throw an error so that we know it has been canccelled.
            //This needs to be done after all items have been validated so that multiple/all messages are
            //reported to the UI
            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();
        }

        protected void OnApplicationRoles_BeforeRemove(ICancelDomainArgs args, object Item)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            int rulePassed = svc.ExecuteRule(spc.DomainMessages, "CheckFurtherLendingApplicationRoleBeforeDelete", Item);

            if (rulePassed == 0)
                args.Cancel = true;

            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();
        }

        protected void OnApplicationMailingAddresses_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationMailingAddresses_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationDebitOrders_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationDebitOrders_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationDebitOrders_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationDebitOrders_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationAttributes_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationAttributes_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationAttributes_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationAttributes_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnRelatedAccounts_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnRelatedAccounts_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnRelatedAccounts_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnRelatedAccounts_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationExpenses_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationExpenses_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationExpenses_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationExpenses_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnSubsidies_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnSubsidies_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnSubsidies_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnSubsidies_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnCallbacks_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnCallbacks_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationConditions_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationConditions_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationInformations_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationInformations_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationMarketingSurveyTypes_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationMarketingSurveyTypes_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationMailingAddresses_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationMailingAddresses_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnCallbacks_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnCallbacks_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationConditions_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationConditions_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationInformations_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationInformations_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationMarketingSurveyTypes_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplicationMarketingSurveyTypes_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        #region IApplication Members

        public virtual void CalculateApplicationDetail(bool IsBondExceptionAction, bool keepMarketRate)
        {
            throw new NotImplementedException("Please implement this method in the class that derives from Application");
        }

        /// <summary>
        /// See <see cref="IApplication.CalculateHouseHoldIncome"/>
        /// </summary>
        public void CalculateHouseHoldIncome()
        {
            // Get the stage in workflow
            bool isApplicationSubmitted = false;

            if (this.ApplicationType == null || this.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.Unknown)
                return;

            IApplicationInformation ai = this.GetLatestApplicationInformation();
            if (ai == null)
                throw new Exception("Missing data - no application information exists.");

            //If the offer is not open (statuskey != 1 or
            //application information has been accepted then no update allowed

            if (!this.IsOpen || ai == null || ai.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                return;

            // If we have gone into credit more times than we have come out of credit,
            // then we can be sure that we are past credit, and should only use confirmed income
            IList<IStageTransition> transistions = SDRepo.GetStageTransitionsByGenericKey(this.Key);
            if (transistions != null && transistions.Count > 0)
            {
                int count = 0;
                foreach (IStageTransition st in transistions)
                {
                    if (st.StageDefinitionStageDefinitionGroup.Key == (int)StageDefinitionStageDefinitionGroups.GoToCredit)
                        count += 1;

                    if (st.StageDefinitionStageDefinitionGroup.Key == (int)StageDefinitionStageDefinitionGroups.ReturnToProcessor)
                        count -= 1;
                }
                if (count > 0)
                    isApplicationSubmitted = true;
            }

            CalculateHouseHoldIncome(isApplicationSubmitted);
        }

        /// <summary>
        /// Calculates the HouseIncome, taking into account all Legal Entities that have Income Contributor Roles.
        /// </summary>
        private void CalculateHouseHoldIncome(bool ConfirmedOnly)
        {
            double income = 0.0;
            bool update = false; // Only update the Income if there is at least one valid LE, contributing income with valid employment

            foreach (IApplicationRole applicationRole in this.ApplicationRoles)
            {
                if (applicationRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client
                    && applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
                {
                    // Find the Income Contributor attribute
                    foreach (IApplicationRoleAttribute applicationRoleAttribute in applicationRole.ApplicationRoleAttributes)
                    {
                        if (applicationRoleAttribute.OfferRoleAttributeType.Key == (int)OfferRoleAttributeTypes.IncomeContributor)
                        {
                            foreach (IEmployment employment in applicationRole.LegalEntity.Employment)
                            {
                                if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current
                                    && (employment.EmploymentEndDate.HasValue ? employment.EmploymentEndDate.Value : DateTime.Now.AddDays(1)) > DateTime.Now)
                                {
                                    update = true; // Only update the Income if there is at least one valid LE, contributing income with valid employment
                                    if (ConfirmedOnly)
                                    {
                                        if (employment.IsConfirmed)
                                        {
                                            income += employment.ConfirmedIncome;
                                        }
                                    }
                                    else
                                    {
                                        if (employment.IsConfirmed)
                                        {
                                            income += employment.ConfirmedIncome;
                                        }
                                        else
                                        {
                                            income += employment.MonthlyIncome;
                                        }
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }

            // Update the Confirmed Income
            if (update)
            {
                ISupportsVariableLoanApplicationInformation svlai = this.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                svlai.VariableLoanInformation.HouseholdIncome = income;
                svlai.VariableLoanInformation.PTI = LoanCalculator.CalculatePTI((svlai.VariableLoanInformation.MonthlyInstalment.HasValue ? svlai.VariableLoanInformation.MonthlyInstalment.Value : 0), income);

                IApplicationMortgageLoan appML = this as IApplicationMortgageLoan;
                if (appML != null)
                    appML.CalculateApplicationDetail(false, false);
            }
        }

        /// <summary>
        /// Returns the sum of confirmed income, taking into account all Legal Entities that have Income Contributor Roles
        /// </summary>
        public double DetermineConfirmedHouseHoldIncome()
        {
            double income = 0.0;

            foreach (IApplicationRole applicationRole in this.ApplicationRoles)
            {
                if (applicationRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client
                    && applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
                {
                    // Find the Income Contributor attribute
                    foreach (IApplicationRoleAttribute applicationRoleAttribute in applicationRole.ApplicationRoleAttributes)
                    {
                        if (applicationRoleAttribute.OfferRoleAttributeType.Key == (int)OfferRoleAttributeTypes.IncomeContributor)
                        {
                            foreach (IEmployment employment in applicationRole.LegalEntity.Employment)
                            {
                                if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current
                                    && (employment.EmploymentEndDate.HasValue ? employment.EmploymentEndDate.Value : DateTime.Now.AddDays(1)) > DateTime.Now)
                                {
                                    if (employment.IsConfirmed)
                                        income += employment.ConfirmedIncome;
                                }
                            }

                            break;
                        }
                    }
                }
            }

            return income;
        }

        /// <summary>
        /// Gets the highest and second highest income contributors on an offer
        /// </summary>
        /// <param name="primaryApplicantLEKey">LegalEntityKey of the highest income earner or -1 if none</param>
        /// <param name="secondaryApplicantLEKey">LegalEntityKey of the 2nd highest income earner or -1 if none</param>
        public void GetPrimaryAndSecondaryApplicants(out int primaryApplicantLEKey, out int secondaryApplicantLEKey)
        {
            List<KeyValuePair<int, double>> applicants = new List<KeyValuePair<int, double>>();
            int mainApplicant = -1;

            foreach (IApplicationRole applicationRole in this.ApplicationRoles)
            {
                if (applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active
                    && applicationRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client
                    && (applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                        || applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor)
                    )
                {
                    // Find the Income Contributor attribute
                    foreach (IApplicationRoleAttribute applicationRoleAttribute in applicationRole.ApplicationRoleAttributes)
                    {
                        if (applicationRoleAttribute.OfferRoleAttributeType.Key == (int)OfferRoleAttributeTypes.IncomeContributor)
                        {
                            //right, got a valid contender
                            if (mainApplicant == -1 && applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant)
                                mainApplicant = applicationRole.LegalEntityKey;

                            double income = 0;

                            foreach (IEmployment employment in applicationRole.LegalEntity.Employment)
                            {
                                if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current
                                    && (employment.EmploymentEndDate.HasValue ? employment.EmploymentEndDate.Value : DateTime.Now.AddDays(1)) > DateTime.Now)
                                {
                                    if (employment.IsConfirmed)
                                    {
                                        income += employment.ConfirmedIncome;
                                    }
                                    else
                                    {
                                        income += employment.MonthlyIncome;
                                    }
                                }
                            }

                            applicants.Add(new KeyValuePair<int, double>(applicationRole.LegalEntityKey, income));
                            break;
                        }
                    }
                }
            }

            primaryApplicantLEKey = -1;
            secondaryApplicantLEKey = -1;

            if (applicants.Count == 1)
            {
                primaryApplicantLEKey = applicants[0].Key;
                return;
            }

            if (applicants.Count > 1)
            {
                List<KeyValuePair<int, double>> sorted = new List<KeyValuePair<int, double>>();
                sorted.Add(applicants[0]);

                foreach (KeyValuePair<int, double> pair in applicants)
                {
                    bool added = false;

                    for (int i = 0; i < sorted.Count; i++)
                    {
                        if (pair.Value > sorted[i].Value)
                        {
                            sorted.Insert(i, pair);
                            added = true;
                            break;
                        }

                        if (pair.Value == sorted[i].Value)
                        {
                            if (pair.Key == mainApplicant)
                            {
                                sorted.Insert(i, pair);
                                added = true;
                                break;
                            }
                        }
                    }

                    if (!added)
                        sorted.Add(pair);
                }

                primaryApplicantLEKey = sorted[0].Key;
                secondaryApplicantLEKey = sorted[1].Key;
            }
        }

        /// <summary>
        /// See <see cref="IApplication.SetEmploymentType"/>
        /// </summary>
        public void SetEmploymentType()
        {
            IApplicationInformation applicationInfo = this.GetLatestApplicationInformation();
            if (NoEmploymentTypeUpdatesAllowed(applicationInfo))
                return;

            if (HasAttribute(OfferAttributeTypes.ManuallySelectedEmploymentType))
                return;

            bool isApplicationSubmitted = ApplicationSubmitted(applicationInfo);

            SetEmploymentType(isApplicationSubmitted);
        }

        private bool NoEmploymentTypeUpdatesAllowed(IApplicationInformation applicationInformation)
        {
            //If the offer is not open (statuskey != 1 or
            //application information has been accepted then no update allowed
            if (!this.IsOpen
                || applicationInformation == null
                || applicationInformation.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                return true;

            IApplicationMortgageLoan aml = this as IApplicationMortgageLoan;
            if (aml == null)
                return true;

            if (this.ApplicationType != null)
                if (this.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.Unknown || this.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.Life)
                    return true;

            return false;
        }

        private bool ApplicationSubmitted(IApplicationInformation applicationInformation)
        {
            var submitted = false;
            // Get the stage in workflow
            if (applicationInformation != null && applicationInformation.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                submitted = true;

            return submitted;
        }

        public void SetManuallySelectedEmploymentType(int selectedEmploymentTypeKey)
        {
            IApplicationInformation applicationInfo = this.GetLatestApplicationInformation();
            if (NoEmploymentTypeUpdatesAllowed(applicationInfo))
                return;

            IEmploymentType selectedEmploymentType;
            if (LookupRepo.EmploymentTypes.ObjectDictionary.TryGetValue(selectedEmploymentTypeKey.ToString(), out selectedEmploymentType))
            {
                ISupportsVariableLoanApplicationInformation svlai = this.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                svlai.VariableLoanInformation.EmploymentType = selectedEmploymentType;

                if (!HasAttribute(OfferAttributeTypes.ManuallySelectedEmploymentType))
                    AddApplicationAttribute(OfferAttributeTypes.ManuallySelectedEmploymentType);
            }
        }

        private void AddApplicationAttribute(OfferAttributeTypes offerAttribute)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IApplicationAttribute applicationAttributeNew = ApplicationRepository.GetEmptyApplicationAttribute();
            applicationAttributeNew.ApplicationAttributeType = LookupRepo.ApplicationAttributesTypes.ObjectDictionary[Convert.ToString((int)offerAttribute)];
            applicationAttributeNew.Application = this;
            this.ApplicationAttributes.Add(spc.DomainMessages, applicationAttributeNew);
        }

        /// <summary>
        /// Groups all monthly income from contributing LegalEntities by employment type.
        /// Uses the greatest income to determine and set the Application income type.
        /// </summary>
        private void SetEmploymentType(bool ConfirmedOnly)
        {
            //Create a dictionary for income types - holds employmenttypekey and salary
            IDictionary<int, double> dicEmploymentIncome = new Dictionary<int, double>(LookupRepo.EmploymentTypes.ObjectDictionary.Count);

            bool update = false; // Only update the Income if there is at least one valid LE, contributing income with valid employment
            double confirmedIncome = 0, basicIncome = 0;
            foreach (IApplicationRole applicationRole in this.ApplicationRoles)
            {
                if (applicationRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client
                    && applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
                {
                    // Find the Income Contributor attribute
                    foreach (IApplicationRoleAttribute applicationRoleAttribute in applicationRole.ApplicationRoleAttributes)
                    {
                        if (applicationRoleAttribute.OfferRoleAttributeType.Key == (int)OfferRoleAttributeTypes.IncomeContributor)
                        {
                            foreach (IEmployment employment in applicationRole.LegalEntity.Employment)
                            {
                                if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current
                                    && (employment.EmploymentEndDate.HasValue ? employment.EmploymentEndDate.Value : DateTime.Now.AddDays(1)) > DateTime.Now)
                                {
                                    update = true; // Only update the Income if there is at least one valid LE, contributing income with valid employment

                                    confirmedIncome = employment.ConfirmedBasicIncome.HasValue ? employment.ConfirmedBasicIncome.Value : 0;
                                    basicIncome = employment.BasicIncome.HasValue ? employment.BasicIncome.Value : 0;

                                    // update the collection item with the index of the employment type
                                    if (employment.IsConfirmed)
                                    {
                                        if (dicEmploymentIncome.ContainsKey(employment.EmploymentType.Key))
                                            dicEmploymentIncome[employment.EmploymentType.Key] += confirmedIncome;
                                        else
                                            dicEmploymentIncome.Add(employment.EmploymentType.Key, confirmedIncome);
                                    }
                                    else if (ConfirmedOnly == false)
                                    {
                                        if (dicEmploymentIncome.ContainsKey(employment.EmploymentType.Key))
                                            dicEmploymentIncome[employment.EmploymentType.Key] += basicIncome;
                                        else
                                            dicEmploymentIncome.Add(employment.EmploymentType.Key, basicIncome);
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }

            // Update the EmploymentType
            if (update)
            {
                double income = 0;
                int employmentTypeKey = 0;

                foreach (KeyValuePair<int, double> incomes in dicEmploymentIncome)
                {
                    if (incomes.Value > income)
                    {
                        income = incomes.Value;
                        employmentTypeKey = incomes.Key;
                    }
                }

                if (employmentTypeKey != 0)
                {
                    ISupportsVariableLoanApplicationInformation svlai = this.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                    svlai.VariableLoanInformation.EmploymentType = LookupRepo.EmploymentTypes.ObjectDictionary[(employmentTypeKey).ToString()];
                }
            }
        }

        //public EmploymentTypes GetEmploymentType()
        //{
        //    ISupportsVariableLoanApplicationInformation svlai = this.CurrentProduct as ISupportsVariableLoanApplicationInformation;

        //    if (svlai == null)
        //        return EmploymentTypes.Unknown;

        //    return svlai.VariableLoanInformation.EmploymentType;
        //}

        #region SetEmploymentTypeOld

        //private void SetEmploymentTypeOld(bool ConfirmedOnly)
        //{
        //    // this could be done more efficiently with HQL...
        //    //incomes[0] = not used
        //    //incomes[1] = Salaried
        //    //incomes[2] = SelfEmployed = 0.0;
        //    //incomes[3] = Subsidised = 0.0;
        //    //incomes[4] = Unemployed = 0.0;
        //    //incomes[5] = Unknown = 0.0;

        //    //Create a collection for income types
        //    List<double> incomes = new List<double>();
        //    for (int i = 0; i < 6; i++)
        //    {
        //        incomes.Add(0);
        //    }

        //    bool update = false; // Only update the Income if there is at least one valid LE, contributing income with valid employment

        //    foreach (IApplicationRole applicationRole in this.ApplicationRoles)
        //    {
        //        if (applicationRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client
        //            && applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
        //        {
        //            // Find the Income Contributor attribute
        //            foreach (IApplicationRoleAttribute applicationRoleAttribute in applicationRole.ApplicationRoleAttributes)
        //            {
        //                if (applicationRoleAttribute.OfferRoleAttributeType.Key == (int)OfferRoleAttributeTypes.IncomeContributor)
        //                {
        //                    foreach (IEmployment employment in applicationRole.LegalEntity.Employment)
        //                    {
        //                        if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current
        //                            && (employment.EmploymentEndDate.HasValue ? employment.EmploymentEndDate.Value : DateTime.Now.AddDays(1)) > DateTime.Now)
        //                        {
        //                            update = true; // Only update the Income if there is at least one valid LE, contributing income with valid employment

        //                            // update the collection item with the index of the employment type
        //                            if (ConfirmedOnly)
        //                            {
        //                                if (employment.IsConfirmed)
        //                                    incomes[employment.EmploymentType.Key] += employment.ConfirmedBasicIncome.HasValue ? employment.ConfirmedBasicIncome.Value : 0.0;
        //                            }
        //                            else
        //                            {
        //                                if (employment.IsConfirmed)
        //                                    incomes[employment.EmploymentType.Key] += employment.ConfirmedBasicIncome.HasValue ? employment.ConfirmedBasicIncome.Value : 0.0;
        //                                else
        //                                    incomes[employment.EmploymentType.Key] += employment.BasicIncome.HasValue ? employment.BasicIncome.Value : 0.0;
        //                            }
        //                        }
        //                    }
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    // Update the EmploymentType
        //    if (update)
        //    {
        //        double income = 0;
        //        int employmentTypeKey = 0;

        //        for (int i = 0; i < 6; i++)
        //        {
        //            if (incomes[i] > income)
        //                employmentTypeKey = i;
        //        }

        //        if (employmentTypeKey != 0)
        //        {
        //            ISupportsVariableLoanApplicationInformation svlai = this.CurrentProduct as ISupportsVariableLoanApplicationInformation;
        //            svlai.VariableLoanInformation.EmploymentType = LookupRepo.EmploymentTypes.ObjectDictionary[(employmentTypeKey - 1).ToString()];
        //        }
        //    }
        //}

        #endregion SetEmploymentTypeOld

        #region CalculateHouseHoldIncomeConfirmed

        ///// <summary>
        ///// See <see cref="IApplication.CalculateHouseHoldIncomeConfirmed"/>
        ///// </summary>
        //private void CalculateHouseHoldIncomeConfirmed()
        //{
        //    double income = 0.0;

        //    foreach (IApplicationRole applicationRole in this.ApplicationRoles)
        //    {
        //        if ((applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
        //            || applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
        //            || applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
        //            || applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor)
        //            && applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
        //        {
        //            // Find the Income Contributor attribute
        //            foreach (IApplicationRoleAttribute applicationRoleAttribute in applicationRole.ApplicationRoleAttributes)
        //            {
        //                if (applicationRoleAttribute.OfferRoleAttributeType.Key == (int)OfferRoleAttributeTypes.IncomeContributor)
        //                {
        //                    foreach (IEmployment employment in applicationRole.LegalEntity.Employment)
        //                    {
        //                        if (employment.IsConfirmed)
        //                            income += employment.ConfirmedBasicIncome.HasValue ? employment.ConfirmedBasicIncome.Value : 0.0;
        //                    }

        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    // Update the Confirmed Income
        //    ISupportsVariableLoanApplicationInformation svlai = this.CurrentProduct as ISupportsVariableLoanApplicationInformation;
        //    svlai.VariableLoanInformation.HouseholdIncome = income;

        //}

        #endregion CalculateHouseHoldIncomeConfirmed

        /// <summary>
        /// See <see cref="IApplication.GetHouseHoldIncome"/>
        /// </summary>
        /// <returns></returns>
        public double GetHouseHoldIncome()
        {
            double householdIncome = 0.0;

            // HouseHoldIncome is a stored (DB) field against the ApplicationInformationVariableLoan object
            ISupportsVariableLoanApplicationInformation svlai = this.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (svlai != null)
                householdIncome = svlai.VariableLoanInformation.HouseholdIncome.HasValue ? svlai.VariableLoanInformation.HouseholdIncome.Value : 0.0;

            return householdIncome;
        }

        /// <summary>
        /// See <see cref="IApplication.HasAttribute"/>
        /// </summary>
        /// <param name="OfferAttributeType"></param>
        /// <returns></returns>
        public bool HasAttribute(OfferAttributeTypes OfferAttributeType)
        {
            bool hasAttribute = false;

            foreach (IApplicationAttribute applicationAttribute in this.ApplicationAttributes)
            {
                if (applicationAttribute.ApplicationAttributeType.Key == (int)OfferAttributeType)
                {
                    hasAttribute = true;
                    break;
                }
            }

            return hasAttribute;
        }

        /// <summary>
        /// See <see cref="IApplication.HasFinancialAdjustment"/>
        /// </summary>
        /// <param name="fats"></param>
        /// <returns></returns>
        public bool HasFinancialAdjustment(FinancialAdjustmentTypeSources fats)
        {
            bool hasRateOverride = false;

            IApplicationInformation applicationInformation = this.GetLatestApplicationInformation();

            foreach (var financialAdjustments in applicationInformation.ApplicationInformationFinancialAdjustments)
            {
                if (financialAdjustments.FinancialAdjustmentTypeSource.Key == (int)fats)
                {
                    hasRateOverride = true;
                    break;
                }
            }

            return hasRateOverride;
        }

        /// <summary>
        /// See <see cref="IApplication.SetApplicantType()"/>
        /// </summary>
        public void SetApplicantType()
        {
            //If the offer is not open (statuskey != 1 or
            //application information has been accepted then no update allowed
            IApplicationInformation ai = this.GetLatestApplicationInformation();
            if (!this.IsOpen
                || ai == null
                || ai.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                return;

            IApplicationMortgageLoan aml = this as IApplicationMortgageLoan;

            if (aml != null)
            {
                int mainNatPersons = 0;
                int appTypeKey = (int)ApplicantTypes.Single; //assume at worst the application will be single
                IReadOnlyEventList<IApplicationRole> appRoles = GetApplicationRolesByGroup(OfferRoleTypeGroups.Client);

                foreach (IApplicationRole applicationRole in appRoles)
                {
                    if ((applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                            || applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant)
                        && applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
                    {
                        switch (applicationRole.LegalEntity.LegalEntityType.Key)
                        {
                            case (int)LegalEntityTypes.CloseCorporation:
                                appTypeKey = (int)ApplicantTypes.ClosedCorporation;
                                break;

                            case (int)LegalEntityTypes.Company:
                                appTypeKey = (int)ApplicantTypes.Company;
                                break;

                            case (int)LegalEntityTypes.Trust:
                                appTypeKey = (int)ApplicantTypes.Trust;
                                break;

                            case (int)LegalEntityTypes.NaturalPerson:
                                mainNatPersons += 1;
                                break;

                            default:
                                break;
                        }
                    }

                    if (appTypeKey > (int)ApplicantTypes.Joint)
                        break;

                    if (mainNatPersons > 1)
                        appTypeKey = (int)ApplicantTypes.Joint;
                }

                aml.ApplicantType = LookupRepo.ApplicantTypes.ObjectDictionary[appTypeKey.ToString()];
            }
        }

        private ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
        }

        private IStageDefinitionRepository SDRepo
        {
            get
            {
                if (_sdRepo == null)
                    _sdRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

                return _sdRepo;
            }
        }

        private IApplicationRepository ApplicationRepository
        {
            get
            {
                if (applicationRepository == null)
                    applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();

                return applicationRepository;
            }
        }

        #endregion IApplication Members

        #region Pricing for Risk methods

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public void PricingForRisk()
        {
            //If Empirica V4 has gone live, and the offer started before EM V4 go live
            //dont do any updates
            IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();

            IControl ctrl = ctrlRepo.GetControlByDescription("ITCEmpirica4GoLive");

            //Empirica V4 is live
            if (ctrl != null && ctrl.ControlNumeric.HasValue && ctrl.ControlNumeric.Value == 4)
            {
                DateTime dt = Convert.ToDateTime(ctrl.ControlText);

                //this offer was started before V4 go live, do not make any amendments
                if (this.ApplicationStartDate < dt)
                    return;
            }

            IApplicationInformation oi = this.GetLatestApplicationInformation();

            if (!this.IsOpen
                || oi == null
                || oi.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer
                || (this.ApplicationType.Key != (int)Globals.OfferTypes.NewPurchaseLoan && this.ApplicationType.Key != (int)Globals.OfferTypes.SwitchLoan && this.ApplicationType.Key != (int)Globals.OfferTypes.RefinanceLoan))
                return;

            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IRateAdjustmentGroup rateAdjustmentGroup = appRepo.GetRateAdjustmentGroupByKey((int)RateAdjustmentGroups.GroupA);
            IEventList<IRateAdjustmentElement> raeList = rateAdjustmentGroup.ActiveRateAdjustmentElements;

            if (raeList.Count > 0)
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                IOrganisationStructureRepository organisationStructureRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IADUser adUser = organisationStructureRepo.GetAdUserForAdUserName("X2");

                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                int rulePassed;

                // remove all RateAdjustment ro
                foreach (IRateAdjustmentElement rae in raeList)
                {
                    foreach (IApplicationInformationFinancialAdjustment financialAdjustment in oi.ApplicationInformationFinancialAdjustments)
                    {
                        if (financialAdjustment.FinancialAdjustmentTypeSource.Key == rae.FinancialAdjustmentTypeSource.Key)
                        {
                            //oi.ApplicationInformationRateOverrides.Remove(spc.DomainMessages, oiro);
                            this.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments.Remove(spc.DomainMessages, financialAdjustment);
                        }
                    }
                }

                foreach (IRateAdjustmentElement rae in raeList)
                {
                    rulePassed = svc.ExecuteRule(spc.DomainMessages, rae.RuleItem.Name, this, rae);
                    if (rulePassed >= 0)
                    {
                        IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment = appRepo.GetEmptyApplicationInformationFinancialAdjustment();
                        applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource = rae.FinancialAdjustmentTypeSource;
                        applicationInformationFinancialAdjustment.Discount = rae.RateAdjustmentValue;
                        applicationInformationFinancialAdjustment.Term = -1;
                        applicationInformationFinancialAdjustment.FromDate = DateTime.Now;
                        applicationInformationFinancialAdjustment.ApplicationInformation = oi;
                        oi.ApplicationInformationFinancialAdjustments.Add(spc.DomainMessages, applicationInformationFinancialAdjustment);

                        IApplicationInformationAppliedRateAdjustment applicationInformationAppliedRateAdjustment = appRepo.GetEmptyApplicationInformationAppliedRateAdjustment();
                        applicationInformationAppliedRateAdjustment.ApplicationElementValue = rulePassed.ToString();
                        applicationInformationAppliedRateAdjustment.RateAdjustmentElement = rae;
                        applicationInformationAppliedRateAdjustment.ADUser = adUser;
                        applicationInformationAppliedRateAdjustment.ChangeDate = DateTime.Now;
                        applicationInformationAppliedRateAdjustment.ApplicationInformationFinancialAdjustment = applicationInformationFinancialAdjustment;
                        applicationInformationFinancialAdjustment.ApplicationInformationAppliedRateAdjustments.Add(spc.DomainMessages, applicationInformationAppliedRateAdjustment);
                    }
                }
            }
            IApplicationMortgageLoan aml = this as IApplicationMortgageLoan;
            if (aml != null)
                aml.CalculateApplicationDetail(false, false);
        }

        /// <summary>
        /// Get Rate Adjustments
        /// </summary>
        /// <returns></returns>
        public double GetRateAdjustments()
        {
            double rateAdjustments = 0;

            IApplicationInformation oi = this.GetLatestApplicationInformation();
            foreach (IApplicationInformationFinancialAdjustment oifa in oi.ApplicationInformationFinancialAdjustments)
            {
                if (oifa.FinancialAdjustmentTypeSource.Key != (int)FinancialAdjustmentTypeSources.DiscountedLinkrate
                    && oifa.FromDate.HasValue
                    && oifa.FromDate <= DateTime.Now
                    && oifa.Discount.HasValue)
                {
                    rateAdjustments = rateAdjustments + (double)oifa.Discount;
                }
            }

            return rateAdjustments;
        }

        #endregion Pricing for Risk methods

        public bool IsAlphaHousing()
        {
            if (this is IApplicationFurtherLending)
                return false;

            return (HasAttribute(OfferAttributeTypes.AlphaHousing) || IsAlphaHousingUncommitted());
        }

        public bool IsCapitec()
        {
            if (this is IApplicationFurtherLending)
                return false;

            return HasAttribute(OfferAttributeTypes.CapitecLoan);
        }

        public bool IsComcorp()
        {
            if (this is IApplicationFurtherLending)
                return false;

            return HasAttribute(OfferAttributeTypes.ComcorpLoan);
        }

        private bool IsAlphaHousingUncommitted()
        {
            if (this.GetLatestApplicationInformation() == null)
            {
                return false;
            }
            IApplicationProductMortgageLoan applicationProductMortgageLoan = this.CurrentProduct as IApplicationProductMortgageLoan;
            if (applicationProductMortgageLoan == null)
            {
                return false;
            }
            ISupportsVariableLoanApplicationInformation variableLoanInformation = applicationProductMortgageLoan as ISupportsVariableLoanApplicationInformation;
            if (variableLoanInformation != null && ApplicationRepository.IsAlphaHousingLoan(variableLoanInformation.VariableLoanInformation.LTV ?? 0, variableLoanInformation.VariableLoanInformation.EmploymentType.Key, variableLoanInformation.VariableLoanInformation.HouseholdIncome ?? 0))
            {
                return true;
            }
            return false;
        }

        private IReadOnlyEventList<ILegalEntity> _activeClients;

        public virtual IReadOnlyEventList<ILegalEntity> ActiveClients
        {
            get
            {
                if (_activeClients == null)
                {
                    string HQL = @"select le
                            from LegalEntity_DAO le
                            inner join le.ApplicationRoles as ar
                            where ar.ApplicationKey = ?
                            and ar.ApplicationRoleType.ApplicationRoleTypeGroup.Key = ?";
                    SimpleQuery<LegalEntity_DAO> q = new SimpleQuery<LegalEntity_DAO>(HQL, this.Key, (int)OfferRoleTypeGroups.Client);
                    LegalEntity_DAO[] arr = q.Execute();
                    _activeClients = new ReadOnlyEventList<ILegalEntity>(new DAOEventList<LegalEntity_DAO, ILegalEntity, LegalEntity>(arr));
                }
                return _activeClients;
            }
        }

        public IVendor GetComcorpVendor()
        {
            var externalVendorRole = this._DAO.ApplicationRoles
                                         .FirstOrDefault(x => x.ApplicationRoleType.Key == (int)OfferRoleTypes.ExternalVendor);

            if (externalVendorRole == null)
            {
                return null;
            }

            var vendorRepository = RepositoryFactory.GetRepository<IVendorRepository>();
            return vendorRepository.GetVendorByLegalEntityKey(externalVendorRole.LegalEntityKey);
        }

        public bool HasCondition(string ConditionName)
        {
            bool hasCondition = false;

            foreach (IApplicationCondition applicationCondition in this.ApplicationConditions)
            {
                if (applicationCondition.Condition.ConditionName.Equals(ConditionName, StringComparison.InvariantCultureIgnoreCase))
                {
                    hasCondition = true;
                    break;
                }
            }

            return hasCondition;
        }

        public bool IsGEPF()
        {
            return HasAttribute(OfferAttributeTypes.GovernmentEmployeePensionFund);
        }
    }
}