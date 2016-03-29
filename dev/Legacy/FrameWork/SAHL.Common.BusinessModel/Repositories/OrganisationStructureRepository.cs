using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;
using NHibernate;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    /// Provides methods for getting and updating object that are part of the organisation structure and feature lists
    /// Used by workflow to resolve dynamic rolse
    /// Used by workflo to assign cases to users
    /// </summary>
    [FactoryType(typeof(IOrganisationStructureRepository))]
    public class OrganisationStructureRepository : AbstractRepositoryBase, IOrganisationStructureRepository
    {
        public OrganisationStructureRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public OrganisationStructureRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        #region ADUsers

        /// <summary>
        ///
        /// </summary>
        /// <param name="OrganisationStructureKey"></param>
        /// <returns></returns>
        public IEventList<IADUser> GetUsersForOrganisationStructureKey(int OrganisationStructureKey)
        {
            OrganisationStructure_DAO OS_DOA = OrganisationStructure_DAO.Find(OrganisationStructureKey);
            IList<ADUser_DAO> ADUsers = OS_DOA.ADUsers;
            return new DAOEventList<ADUser_DAO, IADUser, ADUser>(ADUsers);
        }

        public IEventList<IADUser> GetUsersForOrganisationStructureKey(int OrganisationStructureKey, bool recursive)
        {
            if (!recursive)
                return GetUsersForOrganisationStructureKey(OrganisationStructureKey);

            ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = sessionHolder.CreateSession(typeof(ADUser_DAO));
            string query = @"
            with os (Description, OrganisationStructureKey, ParentKey) as
	            (
	            select Description, OrganisationStructureKey, ParentKey
	            from   dbo.OrganisationStructure
	            where  OrganisationStructureKey =  {0}

	            union all

	            select org.Description, org.OrganisationStructureKey, org.ParentKey
	            from   dbo.OrganisationStructure org
			            inner join
				            os
			            on
				            os.OrganisationStructureKey = org.ParentKey
	            where  org.OrganisationStructureKey <> {0}

	            )

            select distinct	a.ADUserKey, a.ADUserName, a.GeneralStatusKey, a.Password, a.PasswordQuestion, a.PasswordAnswer, a.LegalEntityKey
            from os
	            inner join
		            dbo.UserOrganisationStructure uos
			            inner join
				            dbo.Aduser a
			            on
				            a.ADUserKey = uos.ADUserKey
	            on
		            uos.OrganisationStructureKey = os.OrganisationStructureKey
            order by a.ADUserName";
            IQuery sqlQuery = session.CreateSQLQuery(String.Format(query, OrganisationStructureKey)).AddEntity(typeof(ADUser_DAO));
            IList<ADUser_DAO> results = sqlQuery.List<ADUser_DAO>();
            return new DAOEventList<ADUser_DAO, IADUser, ADUser>(results);
        }

        public IADUser GetADUserByKey(int Key)
        {
            return base.GetByKey<IADUser, ADUser_DAO>(Key);

            //ADUser_DAO DAO = ADUser_DAO.Find(ADUserKey);
            //IADUser user = new ADUser(DAO);
            //return user;
        }

        /// <summary>
        /// <see href="IOrganisationStructureRepository.GetAdUsersByPartialName">IOrganisationStructureRepository.GetAdUsersByPartialName</see>
        /// </summary>
        /// <param name="PartialName"></param>
        /// <param name="maxRecords"></param>
        /// <returns></returns>
        public IEventList<IADUser> GetAdUsersByPartialName(string PartialName, int maxRecords)
        {
            string hql = string.Format("select a from ADUser_DAO a where a.ADUserName like '%{0}%'", PartialName);
            SimpleQuery query = new SimpleQuery(typeof(ADUser_DAO), hql);
            query.SetQueryRange(maxRecords);
            object o = ADUser_DAO.ExecuteQuery(query);
            ADUser_DAO[] Users = o as ADUser_DAO[];
            List<ADUser_DAO> users = new List<ADUser_DAO>();
            for (int i = 0; i < Users.Length; i++)
            {
                users.Add(Users[i]);
            }
            return new DAOEventList<ADUser_DAO, IADUser, ADUser>(users);
        }

        /// <summary>
        /// Gets an AdUser object for a legalentitykey. Null if not found. LEKey should always be unique
        /// </summary>
        /// <param name="LEKey"></param>
        /// <returns></returns>
        public IADUser GetAdUserByLegalEntityKey(int LEKey)
        {
            string HQL = "from ADUser_DAO o where o.LegalEntity.Key=?";
            SimpleQuery<ADUser_DAO> query = new SimpleQuery<ADUser_DAO>(HQL, LEKey);
            ADUser_DAO[] Users = query.Execute();
            if (null != Users && Users.Length > 0)
            {
                return new ADUser(Users[0]);
            }
            return null;
        }

        /// <summary>
        /// <see href="IOrganisationStructureRepository.GetAdUserForAdUserName">IOrganisationStructureRepository.GetAdUserForAdUserName</see>
        /// </summary>
        /// <param name="SAHLName"></param>
        /// <returns></returns>
        public IADUser GetAdUserForAdUserName(string SAHLName)
        {
            string hql = "select a from ADUser_DAO a where a.ADUserName = ?";
            SimpleQuery query = new SimpleQuery(typeof(ADUser_DAO), hql, SAHLName);
            object o = ADUser_DAO.ExecuteQuery(query);
            ADUser_DAO[] Users = o as ADUser_DAO[];
            if (Users.Length > 0)
                return new ADUser(Users[0]);
            else
                return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IADUser CreateEmptyAdUser()
        {
            return base.CreateEmpty<IADUser, ADUser_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AdUser"></param>
        public void SaveAdUser(IADUser AdUser)
        {
            base.Save<IADUser, ADUser_DAO>(AdUser);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEventList<IADUser> GetCompleteAdUserList()
        {
            ADUser_DAO[] daoUsers = ADUser_DAO.FindAll();
            List<ADUser_DAO> Users = new List<ADUser_DAO>();
            for (int i = 0; i < daoUsers.Length; i++)
            {
                Users.Add(daoUsers[i]);
            }
            return new DAOEventList<ADUser_DAO, IADUser, ADUser>(Users);
        }

        #endregion ADUsers

        #region OrgStructure and Features

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEventList<IFeature> GetTopLevelFeatureList()
        {
            string hql = string.Format("select f from Feature_DAO f where f.ParentFeature.Key is null");
            SimpleQuery query = new SimpleQuery(typeof(Feature_DAO), hql);
            object o = Feature_DAO.ExecuteQuery(query);
            Feature_DAO[] features = o as Feature_DAO[];
            IEventList<IFeature> Features = new DAOEventList<Feature_DAO, IFeature, Feature>(features);
            return Features;
        }

        public IFeature CreateEmptyFeature()
        {
            return base.CreateEmpty<IFeature, Feature_DAO>();

            //return new Feature(new Feature_DAO());
        }

        public void SaveFeature(IFeature feature)
        {
            base.Save<IFeature, Feature_DAO>(feature);

            //IDAOObject dao = feature as IDAOObject;
            //Feature_DAO o = (Feature_DAO)dao.GetDAOObject();
            //o.SaveAndFlush();
            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        /// <summary>
        /// Gets the top level organisation structure for an originationsource. 1(SAHL) will return
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IEventList<IOrganisationStructure> GetTopLevelOrganisationStructureForOriginationSource(int Key)
        {
            string HQL = "from OrganisationStructure_DAO o where o.Parent.Key=?";
            SimpleQuery<OrganisationStructure_DAO> q = new SimpleQuery<OrganisationStructure_DAO>(HQL, Key);
            OrganisationStructure_DAO[] res = q.Execute();
            return new DAOEventList<OrganisationStructure_DAO, IOrganisationStructure, OrganisationStructure>(res);
        }

        /// <summary>
        /// Returns parent OrganisationStructure if the orgtype matches the orgType param
        /// otherwise call method recursivly until we climb up to the level needed.
        /// </summary>
        /// <returns></returns>
        public IOrganisationStructure GetOrgstructureParentLevel(IOrganisationStructure orgItem, int orgTypeKey)
        {
            IOrganisationStructure orgParent;
            if (orgItem == null || orgItem.Parent == null)
            {
                //We are at top item as it either has no parent and we can not find the orgTypeKey
                return null;
            }
            else
            {
                //we return the parent
                orgParent = orgItem.Parent;
            }

            //Found the org type we are looking for
            if (orgParent.OrganisationType.Key == orgTypeKey)
                return orgParent;
            else

                //Have another go using the parent
                return GetOrgstructureParentLevel(orgParent, orgTypeKey);
        }

        /// <summary>
        /// Returns top level parent OrganisationStructure if the orgtype matches the orgType param
        /// otherwise call method recursivly until we climb up to the level needed.
        /// </summary>
        /// <returns></returns>
        public IOrganisationStructure GetOrgstructureTopParentLevel(IOrganisationStructure orgItem, int orgTypeKey, int orgStructureRootKey)
        {
            if (orgItem == null || orgItem.Parent == null) // We are at top item as it either has no parent and we can not find the orgTypeKey
                return null;

            // if the org types match and we are at the top of the tree then we have found what we are looking for
            if (orgItem.OrganisationType.Key == orgTypeKey && orgItem.Parent.Key == orgStructureRootKey)
                return orgItem;
            else
                return GetOrgstructureTopParentLevel(orgItem.Parent, orgTypeKey, orgStructureRootKey);
        }

        public IOrganisationStructure GetOrganisationStructureForLE(int leKey)
        {
            string sql = UIStatementRepository.GetStatement("Common", "GetOrganisationStructureForLE");

            SimpleQuery<OrganisationStructure_DAO> q = new SimpleQuery<OrganisationStructure_DAO>(QueryLanguage.Sql, sql, leKey);
            q.AddSqlReturnDefinition(typeof(OrganisationStructure_DAO), "OrgStruc");
            OrganisationStructure_DAO[] res = q.Execute();

            if (res != null && res.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return bmtm.GetMappedType<IOrganisationStructure, OrganisationStructure_DAO>(res[0]);
            }
            else
                return null;
        }

        public IEventList<IOrganisationStructure> GetCompanyList()
        {
            string HQL = "from OrganisationStructure_DAO o where o.OrganisationType.Key=1  and o.Parent.Key is null";
            SimpleQuery<OrganisationStructure_DAO> q = new SimpleQuery<OrganisationStructure_DAO>(HQL);
            OrganisationStructure_DAO[] res = q.Execute();
            return new DAOEventList<OrganisationStructure_DAO, IOrganisationStructure, OrganisationStructure>(res);
        }

        public IOrganisationStructure GetOrganisationStructureForKey(int Key)
        {
            OrganisationStructure_DAO dao = OrganisationStructure_DAO.Find(Key);
            return new OrganisationStructure(dao);
        }

        public DataSet GetOrganisationStructureAllDSForKey(int Key)
        {
            string SQL = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetOrganisationStructureForTree");

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@OrganisationStructureKey", Key));

            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(SQL, typeof(GeneralStatus_DAO), prms);

            return ds;
        }

        public IOrganisationStructure GetOrganisationStructureForDescription(string Description)
        {
            OrganisationStructure_DAO[] daos = OrganisationStructure_DAO.FindAllByProperty("Description", Description);
            return new OrganisationStructure(daos[0]);
        }

        public IOrganisationStructure GetRootOrganisationStructureForDescription(string Description)
        {
            string HQL = "from OrganisationStructure_DAO o where o.Description=? and o.Parent.Key is null";
            SimpleQuery<OrganisationStructure_DAO> q = new SimpleQuery<OrganisationStructure_DAO>(HQL, Description);
            OrganisationStructure_DAO[] res = q.Execute();
            return new OrganisationStructure(res[0]);
        }

        public IOrganisationStructure CreateEmptyOrganisationStructure()
        {
            return base.CreateEmpty<IOrganisationStructure, OrganisationStructure_DAO>();
        }

        public void SaveOrganisationStructure(IOrganisationStructure os)
        {
            base.Save<IOrganisationStructure, OrganisationStructure_DAO>(os);
        }

        public void SaveLegalEntityOrganisationNode(ILegalEntityOrganisationNode leos)
        {
            base.Save<ILegalEntityOrganisationNode, OrganisationStructure_DAO>(leos);
        }

        /// <summary>
        ///
        /// </summary>
        public IEventList<IFeature> GetCompleteFeatureList()
        {
            Feature_DAO[] dao = Feature_DAO.FindAll();
            List<Feature_DAO> features = new List<Feature_DAO>();
            for (int i = 0; i < dao.Length; i++)
            {
                features.Add(dao[i]);
            }
            return new DAOEventList<Feature_DAO, IFeature, Feature>(features);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEventList<IFeatureGroup> GetDistinctFeatureGroupList()
        {
            // will get the max verions
            /*
             * select max(ui.version), ui.statementname
from
uistatement ui
group by ui.statementname
             * */
            string HQL = "";
            SimpleQuery<FeatureGroup_DAO> q = new SimpleQuery<FeatureGroup_DAO>(HQL);
            FeatureGroup_DAO[] res = q.Execute();
            return new DAOEventList<FeatureGroup_DAO, IFeatureGroup, FeatureGroup>(res);
        }

        public IEventList<IFeatureGroup> GetCompleteFeatureGroupList()
        {
            FeatureGroup_DAO[] fgs = FeatureGroup_DAO.FindAll();
            List<FeatureGroup_DAO> featureGroups = new List<FeatureGroup_DAO>();
            for (int i = 0; i < fgs.Length; i++)
            {
                featureGroups.Add(fgs[i]);
            }
            return new DAOEventList<FeatureGroup_DAO, IFeatureGroup, FeatureGroup>(featureGroups);
        }

        public IEventList<IFeatureGroup> GetFeatureGroupsForUserRoles(string userRoles)
        {
            string HQL = "from FeatureGroup_DAO fg where fg.ADUserGroup in (" + userRoles + ")";
            SimpleQuery<FeatureGroup_DAO> q = new SimpleQuery<FeatureGroup_DAO>(HQL);
            FeatureGroup_DAO[] res = q.Execute();
            return new DAOEventList<FeatureGroup_DAO, IFeatureGroup, FeatureGroup>(res);
        }

        public IEventList<IFeature> GetFeaturesForUserRoles(string userRoles)
        {
            string HQL = "select distinct fg.Feature from FeatureGroup_DAO fg where fg.ADUserGroup in (" + userRoles + ")";
            SimpleQuery<Feature_DAO> q = new SimpleQuery<Feature_DAO>(HQL);
            Feature_DAO[] res = q.Execute();
            return new DAOEventList<Feature_DAO, IFeature, Feature>(res);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEventList<ICBOMenu> GetTopLevelCBONodes()
        {
            string HQL = "from CBOMenu_DAO o where o.ParentMenu.Key is null";
            SimpleQuery<CBOMenu_DAO> q = new SimpleQuery<CBOMenu_DAO>(HQL);
            CBOMenu_DAO[] res = q.Execute();
            return new DAOEventList<CBOMenu_DAO, ICBOMenu, CBOMenu>(res);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ICBOMenu CreateEmptyCBO()
        {
            return base.CreateEmpty<ICBOMenu, CBOMenu_DAO>();

            //return new CBOMenu(new CBOMenu_DAO());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cbo"></param>
        public void SaveCBO(ICBOMenu cbo)
        {
            base.Save<ICBOMenu, CBOMenu_DAO>(cbo);

            //IDAOObject dao = cbo as IDAOObject;
            //CBOMenu_DAO o = (CBOMenu_DAO)dao.GetDAOObject();
            //o.SaveAndFlush();
            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IFeature GetFeatureByKey(int Key)
        {
            return base.GetByKey<IFeature, Feature_DAO>(Key);

            //Feature_DAO dao = Feature_DAO.Find(Key);
            //return new Feature(dao);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public ICBOMenu GetCBOByKey(int Key)
        {
            return base.GetByKey<ICBOMenu, CBOMenu_DAO>(Key);
        }

        public IEventList<IContextMenu> GetTopLevelContextMenuNodes()
        {
            string HQL = "from ContextMenu_DAO o where o.ParentMenu.Key is null order by o.Description";
            SimpleQuery<ContextMenu_DAO> q = new SimpleQuery<ContextMenu_DAO>(HQL);
            ContextMenu_DAO[] res = q.Execute();
            return new DAOEventList<ContextMenu_DAO, IContextMenu, ContextMenu>(res);
        }

        public void SaveContextMenu(IContextMenu cm)
        {
            base.Save<IContextMenu, ContextMenu_DAO>(cm);
        }

        public IContextMenu CreateEmptyContextMenu()
        {
            return base.CreateEmpty<IContextMenu, ContextMenu_DAO>();
        }

        public IContextMenu GetContextMenuByKey(int Key)
        {
            return base.GetByKey<IContextMenu, ContextMenu_DAO>(Key);
        }

        #endregion OrgStructure and Features

        #region IApplicationRole

        public IOrganisationStructure GetBranchForConsultant(IApplicationRole appRole)
        {
            string sql = @"select top 1 os.OrganisationStructureKey
                from OfferRole ofr (nolock)
                inner join ADUser a (nolock) on a.LegalEntityKey = ofr.LegalEntityKey
                inner join UserOrganisationStructure uos (nolock) on a.ADUserKey = uos.ADUserKey
                inner join OrganisationStructure os (nolock) on os.OrganisationStructureKey = uos.OrganisationStructureKey
                where ofr.OfferRoleKey = @OfferRoleKey";

            //IDbConnection conn = Helper.GetSQLDBConnection();
            //IDataReader reader = null;
            int orgStructKey = -1;

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@OfferRoleKey", appRole.Key));

            Object o = castleTransactionService.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

            //reader = Helper.ExecuteReader(conn, sql, parameters);
            // if (reader.Read())
            if (o != null)
            {
                orgStructKey = Convert.ToInt32(o);

                // if there are no results, exit now
                if (orgStructKey == -1)
                    return null;
            }
            else
            {
                return null;
            }

            return GetOrganisationStructureForKey(orgStructKey);

            #region Old ActiveRecord Code - removed for performance reasons

            //IADUser user = GetAdUserByLegalEntityKey(appRole.LegalEntity.Key);
            //if (user != null)
            //{
            //    foreach (IUserOrganisationStructure uos in user.UserOrganisationStructure)
            //    {
            //        foreach (IOrganisationStructure os in appRole.ApplicationRoleType.OfferRoleTypeOrganisationStructures)
            //        {
            //            if (uos.OrganisationStructure.Key == os.Key)
            //            {
            //                return os;
            //            }
            //        }
            //    }
            //}
            //return null;

            #endregion Old ActiveRecord Code - removed for performance reasons
        }

        public IApplicationRoleType GetApplicationRoleTypeByName(string Name)
        {
            string HQL = "from ApplicationRoleType_DAO o where o.Description=?";
            SimpleQuery<ApplicationRoleType_DAO> q = new SimpleQuery<ApplicationRoleType_DAO>(HQL, Name);
            ApplicationRoleType_DAO[] res = q.Execute();
            return new ApplicationRoleType(res[0]);
        }

        public IApplicationRole GetApplicationRoleByKey(int Key)
        {
            return base.GetByKey<IApplicationRole, ApplicationRole_DAO>(Key);
        }

        public IApplicationRoleType GetApplicationRoleTypeByKey(int Key)
        {
            return base.GetByKey<IApplicationRoleType, ApplicationRoleType_DAO>(Key);

            //ApplicationRoleType_DAO dao = ApplicationRoleType_DAO.Find(Key);
            //return new ApplicationRoleType(dao);
        }

        public IApplicationRole CreateNewApplicationRole()
        {
            return base.CreateEmpty<IApplicationRole, ApplicationRole_DAO>();

            //return new ApplicationRole(new ApplicationRole_DAO());
        }

        public void SaveApplicationRole(IApplicationRole approle)
        {
            base.Save<IApplicationRole, ApplicationRole_DAO>(approle);

            //IDAOObject dao = approle as IDAOObject;
            //ApplicationRole_DAO o = (ApplicationRole_DAO)dao.GetDAOObject();
            //o.SaveAndFlush();
            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        public IApplicationRole GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(int ApplicationKey, int ApplicationRoleTypeKey, int GeneralStatusKey)
        {
            string HQL = "from ApplicationRole_DAO o where o.ApplicationKey = ? and o.ApplicationRoleType.Key=? and o.GeneralStatus.Key=? order by o.StatusChangeDate desc";
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey, ApplicationRoleTypeKey, GeneralStatusKey);
            q.SetQueryRange(5);
            ApplicationRole_DAO[] res = q.Execute();
            if (res.Length == 0)
                return null;
            return new ApplicationRole(res[0]);
        }

        public IApplicationRole GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(int applicationKey, int applicationRoleTypeKey)
        {
            string HQL = "from ApplicationRole_DAO o where o.ApplicationKey = ? and o.ApplicationRoleType.Key=? order by o.StatusChangeDate desc";
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, applicationKey, applicationRoleTypeKey);
            q.SetQueryRange(5);
            ApplicationRole_DAO[] res = q.Execute();
            if (res.Length == 0)
                return null;
            return new ApplicationRole(res[0]);
        }

        public IApplicationRole GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyAndLegalEntityKey(int ApplicationKey, int ApplicationRoleTypeKey, int LegalEntityKey, int GeneralStatusKey)
        {
            string HQL = "from ApplicationRole_DAO o where o.ApplicationKey = ? and o.ApplicationRoleType.Key=? and o.LegalEntityKey = ? and o.GeneralStatus.Key=? order by o.StatusChangeDate desc";
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey, ApplicationRoleTypeKey, LegalEntityKey, GeneralStatusKey);
            q.SetQueryRange(5);
            ApplicationRole_DAO[] res = q.Execute();
            if (res.Length == 0)
                return null;
            return new ApplicationRole(res[0]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="LegalEntityKey"></param>
        /// <param name="ApplicationRoleTypeKey"></param>
        /// <param name="GeneralStatusKey"></param>
        /// <returns></returns>
        public IApplicationRole GetLatestApplicationRoleByLegalEntityAndApplicationRoleTypeKey(int LegalEntityKey, int ApplicationRoleTypeKey, int GeneralStatusKey)
        {
            string HQL = "from ApplicationRole_DAO o where o.ApplicationRoleType.Key=? and o.LegalEntityKey = ? and o.GeneralStatus.Key=? order by o.StatusChangeDate desc";
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationRoleTypeKey, LegalEntityKey, GeneralStatusKey);
            q.SetQueryRange(5);
            ApplicationRole_DAO[] res = q.Execute();
            if (res.Length == 0)
                return null;

            return new ApplicationRole(res[0]);
        }

        public IEventList<IApplicationRole> GetTopXApplicationRolesForApplicationKey(int ApplicationKey, int MaxResults)
        {
            string HQL = string.Format("from ApplicationRole_DAO o where o.OfferKey =? order by o.StatusChangeDate desc");
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey);
            q.SetQueryRange(MaxResults);
            ApplicationRole_DAO[] res = q.Execute();
            return new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(res);
        }

        public IEventList<IApplicationRole> GetActiveAssignedApplicationRoles(int ApplicationKey, int AppRoleTypeKey)
        {
            string HQL = string.Format("from ApplicationRole_DAO o where o.ApplicationKey =? and o.ApplicationRoleType.Key =?");
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey, AppRoleTypeKey);
            ApplicationRole_DAO[] res = q.Execute();
            return new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(res);
        }

        public IEventList<IApplicationRole> GetTopXApplicationRolesForLEKeys(List<int> LEKeys)
        {
            string HQL = string.Format("from ApplicationRole_DAO o where o.LegalEntity.Key in (:lekeys) order by o.StatusChangeDate desc");
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL);
            q.SetParameterList("lekeys", LEKeys);
            q.SetQueryRange(30);
            ApplicationRole_DAO[] res = q.Execute();
            return new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(res);
        }

        public void MarkRoleAsInactive(IApplicationRole role)
        {
            role.GeneralStatus = RepositoryFactory.GetRepository<ILookupRepository>().GeneralStatuses[GeneralStatuses.Inactive];
            SaveApplicationRole(role);
        }

        #endregion IApplicationRole

        /// <summary>
        /// Gets the latest entry from the OfferRole table for an offerkey
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        public IApplicationRole GetTopApplicationRoleForApplicationKey(int ApplicationKey)
        {
            string HQL = string.Format("from ApplicationRole_DAO o where o.ApplicationKey=? and o.ApplicationRoleType.ApplicationRoleTypeGroup.Key=? order by o.StatusChangeDate desc");
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey, (int)SAHL.Common.Globals.OfferRoleTypeGroups.Operator);
            ApplicationRole_DAO[] arr = q.Execute();
            return new ApplicationRole(arr[0]);
        }

        #region Generic Re-Assign Methods

        /*

        /// 1. Fetch currently active assigned user against the application. (If there is no currently active assigned user against the application it should not be allowed to be re-assigned)
        /// 2. There could be many active operator roles against an application.
        /// 3. Therefore select the top 1 Offer Role from the application based on OfferRoleKey descending order.
        /// 4. This is in context of the current state/map and the valid offer roles for this state. other wise we may get the cloned versions offer roles for say valuations,
        /// but we are trying to reassign from the "Manage Application" stage in Application Management as a New Business Processor D role and the top one latest will possibly.
        /// So get latest top one offer role for a given set of valid state offerrole types.
        /// 5. There is also a filter on this based on the Role Types that have access for the state retrieved via the Instance ID.
        /// 6. This takes care of the part where we only need the most recent active operator role on an application.
        /// 7. Get the parent organisation structure of the organisation structure the user is part of. (retrieve user through OfferRole)
        /// 8. User might be linked to more than one Organisation Structure.
        /// 9. We filter organisation structure/s on the role type based on the role type of the OfferRole.
        /// 10. This gives us just one organisation structure to work with.
        /// 11. Determine all role types that role into this parent.
        /// 12. Filter these roles by the roles retrieved from X2 state. (Role Types that have access for the state retrieved via the Instance ID)
        /// 13. Allow User to select a Role, display list of user for that role that application can be assigned to.
        /// 14. If the user selects a role type that is already active on the application (OfferRole), it will set that OfferRole to Inactive before assigning the new OfferRole.
        */

        /// <summary>
        ///
        /// </summary>
        /// <param name="adUserName"></param>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        public IApplicationRole GetApplicationRoleForADUserAndApplication(string adUserName, int applicationKey)
        {
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            int _offerRoleTypeGroupKey = (int)SAHL.Common.Globals.OfferRoleTypeGroups.Operator;
            int _generalStatusKey = (int)SAHL.Common.Globals.GeneralStatuses.Active;

            string query = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetApplicationRoleForADUserAndApplication");
            string sql = string.Format(query, applicationKey, adUserName, _offerRoleTypeGroupKey, _generalStatusKey);
            SimpleQuery<ApplicationRole_DAO> arQ = new SimpleQuery<ApplicationRole_DAO>(QueryLanguage.Sql, sql);
            arQ.AddSqlReturnDefinition(typeof(ApplicationRole_DAO), "OFR");
            ApplicationRole_DAO[] arRes = arQ.Execute();

            if (arRes != null && arRes.Length > 0)
                return BMTM.GetMappedType<IApplicationRole, ApplicationRole_DAO>(arRes[0]);
            else
                return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationRole"></param>
        /// <returns></returns>
        public IOrganisationStructure GetOrganisationStructForADUser(IApplicationRole applicationRole)
        {
            IOrganisationStructure _orgStruct = null;
            string query = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetOrganisationStructForADUser");

            SimpleQuery<OrganisationStructure_DAO> osQ = new SimpleQuery<OrganisationStructure_DAO>(QueryLanguage.Sql, query, applicationRole.Key, applicationRole.LegalEntity.Key, applicationRole.ApplicationRoleType.Key, applicationRole.Key);
            osQ.AddSqlReturnDefinition(typeof(OrganisationStructure_DAO), "OS");
            OrganisationStructure_DAO[] osRes = osQ.Execute();

            if (osRes.Length == 0)
                return _orgStruct;

            _orgStruct = new OrganisationStructure(osRes[0]);
            return _orgStruct;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="orgStruct"></param>
        /// <param name="ignoreX2Filter"></param>
        /// <returns></returns>
        public IEventList<IApplicationRoleType> GetAppRoleTypesBasedOnAppAndState(long instanceID, IOrganisationStructure orgStruct, bool ignoreX2Filter)
        {
            List<string> x2SecGrpList = new List<string>();
            IEventList<IApplicationRoleType> appRoleTypeList = new EventList<IApplicationRoleType>();
            Dictionary<int, IApplicationRoleType> appRoleTypes = new Dictionary<int, IApplicationRoleType>();
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            string sql = string.Empty;

            // Determine which Security Groups in X2 have access to the given State via the Instance ID.
            sql = @"SELECT SG.*
                FROM X2.X2.INSTANCE INST (nolock)
                INNER JOIN X2.X2.STATE ST (nolock)
                ON INST.STATEID = ST.ID
                INNER JOIN X2.X2.STATEWORKLIST STWL (nolock)
                ON ST.ID = STWL.STATEID
                INNER JOIN X2.X2.SECURITYGROUP SG (nolock)
                ON STWL.SECURITYGROUPID = SG.ID
                WHERE INST.ID = ?";

            SimpleQuery<SecurityGroup_DAO> secQ = new SimpleQuery<SecurityGroup_DAO>(QueryLanguage.Sql, sql, instanceID);
            secQ.AddSqlReturnDefinition(typeof(SecurityGroup_DAO), "SG");
            SecurityGroup_DAO[] secRes = secQ.Execute();

            if (secRes.Length == 0)
                return appRoleTypeList;

            foreach (SecurityGroup_DAO secGroup in secRes)
            {
                x2SecGrpList.Add(secGroup.Name);
            }

            // Find all the role types that rolls up into that parent
            if (ignoreX2Filter)
            {
                sql = string.Format(@"
                ;with os (Description, OrganisationStructureKey, ParentKey) as
                (
                    select Description, OrganisationStructureKey, ParentKey
                    from   dbo.OrganisationStructure (nolock)
                    where  OrganisationStructureKey =  {0}
                    union all
                    select org.Description, org.OrganisationStructureKey, org.ParentKey
                    from   dbo.OrganisationStructure org (nolock)
                    inner join
                    os
                    on
                    os.OrganisationStructureKey = org.ParentKey
                    where  org.OrganisationStructureKey <> {1}
                )
                SELECT ORT.*
                FROM os
                INNER JOIN [2AM].[DBO].OfferRoleTypeOrganisationStructureMapping OSM (nolock)
                ON OSM.oRGANISATIONsTRUCTUREkEY = OS.oRGANISATIONsTRUCTUREkEY
                INNER JOIN [2AM].[DBO].[OfferRoleType] ORT (nolock)
                ON OSM.OfferRoleTypeKey = ORT.OfferRoleTypeKey
                WHERE ORT.OfferRoleTypeGroupKey = 1", orgStruct.Parent.Key, orgStruct.Parent.Key);
            }
            else
            {
                sql = string.Format(@"
                ;with os (Description, OrganisationStructureKey, ParentKey) as
                (
                    select Description, OrganisationStructureKey, ParentKey
                    from   dbo.OrganisationStructure (nolock)
                    where  OrganisationStructureKey =  {0}
                    union all
                    select org.Description, org.OrganisationStructureKey, org.ParentKey
                    from   dbo.OrganisationStructure org (nolock)
                    inner join
                    os
                    on
                    os.OrganisationStructureKey = org.ParentKey
                    where  org.OrganisationStructureKey <> {1}
                )
                SELECT DISTINCT ORT.*
                FROM os
                INNER JOIN [2AM].[DBO].OfferRoleTypeOrganisationStructureMapping OSM (nolock)
                ON OSM.oRGANISATIONsTRUCTUREkEY = OS.oRGANISATIONsTRUCTUREkEY
                INNER JOIN [2AM].[DBO].[OfferRoleType] ORT (nolock)
                ON OSM.OfferRoleTypeKey = ORT.OfferRoleTypeKey
                WHERE ORT.OfferRoleTypeGroupKey = 1 and ORT.Description in (:secgrps)", orgStruct.Parent.Key, orgStruct.Parent.Key);
            }

            SimpleQuery<ApplicationRoleType_DAO> artQ = new SimpleQuery<ApplicationRoleType_DAO>(QueryLanguage.Sql, sql);
            artQ.AddSqlReturnDefinition(typeof(ApplicationRoleType_DAO), "ORT");

            if (!ignoreX2Filter)
                artQ.SetParameterList("secgrps", x2SecGrpList);

            ApplicationRoleType_DAO[] artRes = artQ.Execute();

            if (artRes.Length == 0)
                return appRoleTypeList;

            foreach (ApplicationRoleType_DAO _appRoleType in artRes)
            {
                appRoleTypeList.Add(null, BMTM.GetMappedType<IApplicationRoleType, ApplicationRoleType_DAO>(_appRoleType));
            }

            return appRoleTypeList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="ActivityName"></param>
        /// <param name="orgStruct"></param>
        /// <param name="ignoreX2Filter"></param>
        /// <returns></returns>
        public IEventList<IApplicationRoleType> GetAppRoleTypesBasedOnAppAndNextState(long instanceID, string ActivityName, IOrganisationStructure orgStruct, bool ignoreX2Filter)
        {
            List<string> x2SecGrpList = new List<string>();
            IEventList<IApplicationRoleType> appRoleTypeList = new EventList<IApplicationRoleType>();
            int ParentKey = orgStruct.Parent == null ? orgStruct.Key : orgStruct.Parent.Key;

            string query = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetAppRoleTypesBasedOnAppAndNextState");
            string sql = string.Format(query, instanceID, ActivityName, ParentKey, ParentKey);

            SimpleQuery<ApplicationRoleType_DAO> artQ = new SimpleQuery<ApplicationRoleType_DAO>(QueryLanguage.Sql, sql);
            artQ.AddSqlReturnDefinition(typeof(ApplicationRoleType_DAO), "ORT");
            ApplicationRoleType_DAO[] artRes = artQ.Execute();

            if (artRes != null && artRes.Length > 0)
                appRoleTypeList = new DAOEventList<ApplicationRoleType_DAO, IApplicationRoleType, ApplicationRoleType>(artRes);

            return appRoleTypeList;
        }

        #endregion Generic Re-Assign Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="art"></param>
        /// <param name="orgStruct"></param>
        /// <returns></returns>
        public IEventList<IADUser> GetUsersForRoleTypeAndOrgStruct(IApplicationRoleType art, IOrganisationStructure orgStruct)
        {
            IEventList<IADUser> adUserList = new EventList<IADUser>();
            string query = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetUsersForRoleTypeAndOrgStruct");
            SimpleQuery<ADUser_DAO> adQ = new SimpleQuery<ADUser_DAO>(QueryLanguage.Sql, query, orgStruct.Parent.Key, art.Key);
            adQ.AddSqlReturnDefinition(typeof(ADUser_DAO), "AD");
            ADUser_DAO[] adRes = adQ.Execute();

            if (adRes != null && adRes.Length > 0)
                adUserList = new DAOEventList<ADUser_DAO, IADUser, ADUser>(adRes);

            return adUserList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="OrganisationStructureKey"></param>
        /// <returns></returns>
        public IEventList<IADUser> GetUsersPerOrgStruct(int OrganisationStructureKey)
        {
            IEventList<IADUser> adUserList = new EventList<IADUser>();
            string query = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetUsersPerOrgStruct");
            SimpleQuery<ADUser_DAO> adQ = new SimpleQuery<ADUser_DAO>(QueryLanguage.Sql, query, OrganisationStructureKey, OrganisationStructureKey);
            adQ.AddSqlReturnDefinition(typeof(ADUser_DAO), "ad");
            ADUser_DAO[] adRes = adQ.Execute();

            if (adRes != null && adRes.Length > 0)
                adUserList = new DAOEventList<ADUser_DAO, IADUser, ADUser>(adRes);

            return adUserList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="art"></param>
        /// <returns></returns>
        public IEventList<IADUser> GetUsersForDynamicRole(IApplicationRoleType art)
        {
            return GetUsersForDynamicRole(art, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="art"></param>
        /// <param name="LookAtParent"></param>
        /// <returns></returns>
        public IEventList<IADUser> GetAllUsersForDynamicRole(IApplicationRoleType art, bool LookAtParent)
        {
            IEventList<IADUser> adUsers = new EventList<IADUser>();
            List<int> adUserKeys = new List<int>();

            IEventList<IOrganisationStructure> structs = art.OfferRoleTypeOrganisationStructures;

            foreach (IOrganisationStructure orgStruct in structs)
            {
                if (LookAtParent)
                {
                    // get parent org structure
                    IOrganisationStructure parentOrgStructure = orgStruct.Parent;

                    // loop thru each child and get the adusers
                    foreach (IOrganisationStructure childStruct in parentOrgStructure.ChildOrganisationStructures)
                    {
                        // add the adusers to the collection
                        foreach (IADUser user in childStruct.ADUsers)
                        {
                            // exclude the user if it's already been added
                            if (adUserKeys.Contains(user.Key))
                                continue;

                            adUsers.Add(null, user);
                            adUserKeys.Add(user.Key);
                        }
                    }
                }
                else
                {
                    // add the adusers to the collection
                    foreach (IADUser user in orgStruct.ADUsers)
                    {
                        // exclude the user if it's already been added
                        if (adUserKeys.Contains(user.Key))
                            continue;

                        adUsers.Add(null, user);
                        adUserKeys.Add(user.Key);
                    }
                }
            }

            SortedList<string, IADUser> sortedList = new SortedList<string, IADUser>();
            foreach (IADUser adUser in adUsers)
            {
                /*if (!(string.IsNullOrEmpty(adUser.LegalEntity.FirstNames)))
                    sortedList.Add(adUser.LegalEntity.FirstNames, adUser);
                else if (!(string.IsNullOrEmpty(adUser.LegalEntity.Initials)))
                    sortedList.Add(adUser.LegalEntity.Initials, adUser);
                else if (!(string.IsNullOrEmpty(adUser.LegalEntity.Surname)))
                    sortedList.Add(adUser.LegalEntity.Surname, adUser);
                else
                 */

                // Since the only value that can be sorted by and is unqiue

                //If there is no legal entity for a corresponding AdUser, then do not select the user
                if (adUser.LegalEntity != null)
                {
                    sortedList.Add(adUser.ADUserName, adUser);
                }
            }

            adUsers = new EventList<IADUser>();

            foreach (KeyValuePair<string, IADUser> kv in sortedList)
            {
                adUsers.Add(null, kv.Value);
            }
            return adUsers;
        }

        /// <summary>
        /// Gets the users in a dynamic roel via the org structure
        /// NB THIS ONLY WORKS WHERE THE DYNAMIC ROLE IS MAPPED TO ONE AND ONLY ONE ORGSTRUCTURE BRANCH (x) WILL NOT WORK
        /// </summary>
        /// <param name="art"></param>
        /// <param name="LookAtParent"></param>
        /// <returns></returns>
        public IEventList<IADUser> GetUsersForDynamicRole(IApplicationRoleType art, bool LookAtParent)
        {
            IEventList<IADUser> adUsers = new EventList<IADUser>();
            List<int> adUserKeys = new List<int>();

            IEventList<IOrganisationStructure> structs = art.OfferRoleTypeOrganisationStructures;

            foreach (IOrganisationStructure orgStruct in structs)
            {
                if (LookAtParent)
                {
                    // get parent org structure
                    IOrganisationStructure parentOrgStructure = orgStruct.Parent;

                    // loop thru each child and get the adusers
                    foreach (IOrganisationStructure childStruct in parentOrgStructure.ChildOrganisationStructures)
                    {
                        // add the adusers to the collection
                        foreach (IADUser user in childStruct.ADUsers)
                        {
                            // exclude the user if it's already been added or is inactive
                            if (adUserKeys.Contains(user.Key) || user.GeneralStatusKey.Key == (int)GeneralStatuses.Inactive)
                                continue;

                            adUsers.Add(null, user);
                            adUserKeys.Add(user.Key);
                        }
                    }
                }
                else
                {
                    // add the adusers to the collection
                    foreach (IADUser user in orgStruct.ADUsers)
                    {
                        // exclude the user if it's already been added or is inactive
                        if (adUserKeys.Contains(user.Key) || user.GeneralStatusKey.Key == (int)GeneralStatuses.Inactive)
                            continue;

                        adUsers.Add(null, user);
                        adUserKeys.Add(user.Key);
                    }
                }
            }

            SortedList<string, IADUser> sortedList = new SortedList<string, IADUser>();
            foreach (IADUser adUser in adUsers)
            {
                // Since the only value that can be sorted by and is unqiue
                sortedList.Add(adUser.ADUserName, adUser);
            }

            adUsers = new EventList<IADUser>();

            foreach (KeyValuePair<string, IADUser> kv in sortedList)
            {
                adUsers.Add(null, kv.Value);
            }
            return adUsers;
        }

        /// <summary>
        /// Gets the users in a dynamic role via the org structure
        /// </summary>
        /// <param name="art"></param>
        /// <param name="orgStructure"></param>
        /// <returns></returns>
        public IEventList<IADUser> GetUsersForDynamicRole(IApplicationRoleType art, IOrganisationStructure orgStructure)
        {
            IEventList<IADUser> adUsers = new EventList<IADUser>();
            List<int> adUserKeys = new List<int>();

            // get parent org structure
            IOrganisationStructure parentOrgStructure = orgStructure.Parent;

            if (parentOrgStructure != null)
            {
                // loop thru each child and get the adusers
                foreach (IOrganisationStructure childStruct in parentOrgStructure.ChildOrganisationStructures)
                {
                    // add the adusers to the collection
                    foreach (IADUser user in childStruct.ADUsers)
                    {
                        // exclude the user if it's already been added or is inactive
                        if (adUserKeys.Contains(user.Key) || user.GeneralStatusKey.Key == (int)GeneralStatuses.Active)
                            continue;

                        adUsers.Add(null, user);
                        adUserKeys.Add(user.Key);
                    }
                }
            }

            return adUsers;
        }

        /// <summary>
        /// Gets the dynamic role types
        /// </summary>
        /// <param name="userOrgansationStructures"></param>
        /// <returns></returns>
        public IList<IApplicationRoleType> GetDynamicRoleTypes(IEventList<IUserOrganisationStructure> userOrgansationStructures)
        {
            IList<IApplicationRoleType> dynamicRoles = new List<IApplicationRoleType>();

            string hql = "select ar from ApplicationRoleType_DAO ar where ar.Description like '% D'";
            SimpleQuery<ApplicationRoleType_DAO> q = new SimpleQuery<ApplicationRoleType_DAO>(hql);
            ApplicationRoleType_DAO[] res = q.Execute();

            for (int i = 0; i < res.Length; i++)
            {
                dynamicRoles.Add(new ApplicationRoleType(res[i]));
            }

            return dynamicRoles;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aduser"></param>
        /// <returns></returns>
        public IList<IOrganisationStructure> GetOrgStructsPerADUser(IADUser aduser)
        {
            string sql = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetOrgStructsPerADUser");
            IList<IOrganisationStructure> orgList = new List<IOrganisationStructure>();
            SimpleQuery<OrganisationStructure_DAO> osQ = new SimpleQuery<OrganisationStructure_DAO>(QueryLanguage.Sql, sql, aduser.LegalEntity.Key);
            osQ.AddSqlReturnDefinition(typeof(OrganisationStructure_DAO), "OS");
            OrganisationStructure_DAO[] osRes = osQ.Execute();

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            foreach (OrganisationStructure_DAO _orgStruct in osRes)
            {
                orgList.Add(BMTM.GetMappedType<IOrganisationStructure, OrganisationStructure_DAO>(_orgStruct));
            }
            return orgList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ADUserKey"></param>
        /// <param name="OrganisationStructureKey"></param>
        /// <returns></returns>
        public IList<IOrganisationStructure> GetOrgStructsPerADUserAndCompany(int ADUserKey, int OrganisationStructureKey)
        {
            string sql = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetOrgStructsPerADUserAndCompany");
            sql = string.Format(sql, OrganisationStructureKey, ADUserKey);
            IList<IOrganisationStructure> orgList = new List<IOrganisationStructure>();
            SimpleQuery<OrganisationStructure_DAO> osQ = new SimpleQuery<OrganisationStructure_DAO>(QueryLanguage.Sql, sql);
            osQ.AddSqlReturnDefinition(typeof(OrganisationStructure_DAO), "OS");
            OrganisationStructure_DAO[] osRes = osQ.Execute();

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            foreach (OrganisationStructure_DAO _orgStruct in osRes)
            {
                orgList.Add(BMTM.GetMappedType<IOrganisationStructure, OrganisationStructure_DAO>(_orgStruct));
            }
            return orgList;
        }

        /// <summary>
        /// Gets a list of IOrganisationStructureOriginationSource for an ADUser.
        /// </summary>
        /// <param name="adUserName"></param>
        /// <returns></returns>
        public IList<IOrganisationStructureOriginationSource> GetOrgStructureOriginationSourcesPerADUser(string adUserName)
        {
            IList<IUserOrganisationStructure> UOS = new List<IUserOrganisationStructure>();
            SimpleQuery<OrganisationStructureOriginationSource_DAO> OSOS = new SimpleQuery<OrganisationStructureOriginationSource_DAO>(QueryLanguage.Sql,
                    @"with OSTopLevels (OrganisationStructureKey, ParentKey)
                    as
                    (
	                    select OS.OrganisationStructureKey, OS.ParentKey
	                    from
		                    OrganisationStructure OS (nolock)
	                    inner join
		                    UserOrganisationStructure UOS (nolock)
	                    on
		                    OS.OrganisationStructureKey = UOS.OrganisationStructureKey
	                    inner join
		                    ADUser A (nolock)
	                    on
		                    A.ADUserKey = UOS.ADUserKey
	                    where
		                    A.ADUserName = ?
                    UNION ALL
	                    select OS.OrganisationStructureKey, OS.ParentKey
	                    from
		                    OrganisationStructure OS (nolock)
	                    join
		                    OSTopLevels
	                    on
		                    OS.OrganisationStructureKey = OSTopLevels.ParentKey
                    )
                    select distinct OSOS.* from OSTopLevels
                    inner join
	                    OrganisationStructureOriginationSource OSOS (nolock)
                    on
	                    OSOS.OrganisationStructureKey = OSTopLevels.OrganisationStructureKey", adUserName);
            OSOS.AddSqlReturnDefinition(typeof(OrganisationStructureOriginationSource_DAO), "OSOS");
            OrganisationStructureOriginationSource_DAO[] items = OSOS.Execute();

            IList<IOrganisationStructureOriginationSource> result = new List<IOrganisationStructureOriginationSource>();
            IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            foreach (OrganisationStructureOriginationSource_DAO o in items)
                result.Add(bmtm.GetMappedType<IOrganisationStructureOriginationSource>(o));

            return result;
        }

        /// <summary>
        /// Gets a list of OriginationSourceKeys from OrganisationStructureOriginationSource for an ADUser.
        /// </summary>
        /// <param name="adUserName"></param>
        /// <returns></returns>
        public List<int> GetOriginationSourceKeysPerADUser(string adUserName)
        {
            IList<IOrganisationStructureOriginationSource> osos = GetOrgStructureOriginationSourcesPerADUser(adUserName);

            List<int> keys = new List<int>();

            foreach (IOrganisationStructureOriginationSource os in osos)
            {
                if (!keys.Contains(os.OriginationSource.Key))
                    keys.Add(os.OriginationSource.Key);
            }

            return keys;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="orgList"></param>
        /// <returns></returns>
        public IList<IApplicationRoleType> GetAppRoleTypesForOrgStructList(IList<IOrganisationStructure> orgList)
        {
            string sql = string.Empty;
            Dictionary<int, IApplicationRoleType> appRoleTypeDict = new Dictionary<int, IApplicationRoleType>();
            IList<IApplicationRoleType> appRoleTypeList = new List<IApplicationRoleType>();
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            foreach (IOrganisationStructure _orgStruct in orgList)
            {
                int _orgStructKey = _orgStruct.Key;

                if (_orgStruct.Parent != null)
                    _orgStructKey = _orgStruct.Parent.Key;

                sql = string.Format(@";with os (Description, OrganisationStructureKey, ParentKey) as
                (
                    select Description, OrganisationStructureKey, ParentKey
                    from   dbo.OrganisationStructure (nolock)
                    where  OrganisationStructureKey =  {0}
                    union all
                    select org.Description, org.OrganisationStructureKey, org.ParentKey
                    from   dbo.OrganisationStructure org (nolock)
                    inner join
                    os
                    on
                    os.OrganisationStructureKey = org.ParentKey
                    where  org.OrganisationStructureKey <> {1}
                )
                SELECT DISTINCT ORT.*
                FROM os
                INNER JOIN [2AM].[DBO].OfferRoleTypeOrganisationStructureMapping OSM (nolock)
                ON OSM.oRGANISATIONsTRUCTUREkEY = OS.oRGANISATIONsTRUCTUREkEY
                INNER JOIN [2AM].[DBO].[OfferRoleType] ORT (nolock)
                ON OSM.OfferRoleTypeKey = ORT.OfferRoleTypeKey
                WHERE ORT.OfferRoleTypeGroupKey = 1", _orgStructKey, _orgStructKey);

                SimpleQuery<ApplicationRoleType_DAO> artQ = new SimpleQuery<ApplicationRoleType_DAO>(QueryLanguage.Sql, sql);
                artQ.AddSqlReturnDefinition(typeof(ApplicationRoleType_DAO), "ORT");
                ApplicationRoleType_DAO[] artRes = artQ.Execute();

                foreach (ApplicationRoleType_DAO _appRoleType in artRes)
                {
                    if (!appRoleTypeDict.ContainsKey(_appRoleType.Key))
                    {
                        IApplicationRoleType _addAppRoleType = BMTM.GetMappedType<IApplicationRoleType, ApplicationRoleType_DAO>(_appRoleType);
                        appRoleTypeDict.Add(_addAppRoleType.Key, _addAppRoleType);
                    }
                }
            }

            foreach (KeyValuePair<int, IApplicationRoleType> kv in appRoleTypeDict)
            {
                appRoleTypeList.Add(kv.Value);
            }

            return appRoleTypeList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="orgList"></param>
        /// <returns></returns>
        public IList<IWorkflowRoleType> GetWorkflowRoleTypesForOrgStructList(IList<IOrganisationStructure> orgList)
        {
            string sql = string.Empty;
            Dictionary<int, IWorkflowRoleType> workflowRoleTypeDict = new Dictionary<int, IWorkflowRoleType>();
            IList<IWorkflowRoleType> workflowRoleTypeList = new List<IWorkflowRoleType>();
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            foreach (IOrganisationStructure _orgStruct in orgList)
            {
                int _orgStructKey = _orgStruct.Key;

                if (_orgStruct.Parent != null)
                    _orgStructKey = _orgStruct.Parent.Key;

                sql = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetWorkflowRoleTypesForOrgStructList");
                SimpleQuery<WorkflowRoleType_DAO> artQ = new SimpleQuery<WorkflowRoleType_DAO>(QueryLanguage.Sql, sql, _orgStructKey);
                artQ.AddSqlReturnDefinition(typeof(WorkflowRoleType_DAO), "ORT");
                WorkflowRoleType_DAO[] wrtRes = artQ.Execute();

                foreach (WorkflowRoleType_DAO _workflowRoleType in wrtRes)
                {
                    if (!workflowRoleTypeDict.ContainsKey(_workflowRoleType.Key))
                    {
                        IWorkflowRoleType _addWorkflowRoleType = BMTM.GetMappedType<IWorkflowRoleType, WorkflowRoleType_DAO>(_workflowRoleType);
                        workflowRoleTypeDict.Add(_addWorkflowRoleType.Key, _addWorkflowRoleType);
                    }
                }
            }

            foreach (KeyValuePair<int, IWorkflowRoleType> kv in workflowRoleTypeDict)
            {
                workflowRoleTypeList.Add(kv.Value);
            }

            return workflowRoleTypeList;
        }

        public IAttorney GetAttorneyByLegalEntityKey(int key)
        {
            string HQL = "from Attorney_DAO d where d.LegalEntity.Key=?";
            SimpleQuery<Attorney_DAO> q = new SimpleQuery<Attorney_DAO>(HQL, key);
            Attorney_DAO[] arr = q.Execute();
            if (null == arr || arr.Length == 0) return null;
            return new Attorney(arr[0]);
        }

        #region Branch Admin, Consultant stuff

        public IEventList<IApplicationRole> FindApplicationRolesForApplicationKeyAndLEKey(int ApplicationKey, int LegalEntityKey)
        {
            string HQL = "from ApplicationRole_DAO o where o.LegalEntityKey=? and o.ApplicationKey = ?";
            SimpleQuery<ApplicationRole_DAO> query = new SimpleQuery<ApplicationRole_DAO>(HQL, LegalEntityKey, ApplicationKey);
            ApplicationRole_DAO[] arr = query.Execute();
            return new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(arr);
        }

        public IEventList<IADUser> GetBranchUsersForUserInThisBranch(IADUser user, OrganisationStructureGroup orgStructure, string Prefix)
        {
            IApplicationRoleType art = null;
            switch (orgStructure)
            {
                case OrganisationStructureGroup.Consultant:
                    {
                        art = GetApplicationRoleTypeByName(string.Format("{0} Consultant D", Prefix));
                        break;
                    }
                case OrganisationStructureGroup.Admin:
                    {
                        art = GetApplicationRoleTypeByName(string.Format("{0} Admin D", Prefix));
                        break;
                    }
                case OrganisationStructureGroup.Manager:
                    {
                        art = GetApplicationRoleTypeByName(string.Format("{0} Manager D", Prefix));
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }

            // loop through the orgstructures for this app role and find the user we were passed in the org structure.
            // start by going to the parent and looking in all of the parents children
            // heres why
            // you looking for a consultant but you are an admin. There will never be an intersection between Branch Consultant D and Admin
            // Three will be an intersection of Branch Consultant D and Consultant in OS
            // SO
            // Goto the parent of the Consultant OS and loop through all the parents children. Althoguh it will mena that
            // we search Admin, Consultant and Manager it does mean we will find the intersection to the branch.
            foreach (IOrganisationStructure os in art.OfferRoleTypeOrganisationStructures)
            {
                // loop throught the children of this groups parents. This will search the current and all on the same level
                foreach (IOrganisationStructure osTmp in os.Parent.ChildOrganisationStructures)
                {
                    // check if the user we are looking for is in the current group
                    foreach (IADUser tUsr in osTmp.ADUsers)
                    {
                        if (tUsr.Key == user.Key)
                        {
                            // we have found the aduser. We dont care if he is a Consultant, Admin or Manager all we want
                            // is the list of consultants. Go to the parent and loop through the children looking for "Admin"
                            foreach (IOrganisationStructure osTmp1 in os.Parent.ChildOrganisationStructures)
                            {
                                if (osTmp1.Description == orgStructure.ToString())
                                {
                                    return osTmp1.ADUsers;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// This will return a Dictionary of OfferRoleType/s and WorkflowRoleType/s linked to an ADUser.
        /// The Key has a descriptor prefixed at the end to identify if you working in the context of a WorkflowRoleType or OfferRoleType.
        /// The reason for this "HACK" was OfferRoleType and WorkflowRoleType don't share a common interface so we have to code against implementation.
        /// </summary>
        /// <param name="ADUSerName"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetRoleTypesForADUser(string ADUSerName)
        {
            Dictionary<string, string> RoleTypes = new Dictionary<string, string>();
            string sql = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetRoleTypesForADUser");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@ADUserName", ADUSerName));

            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    RoleTypes.Add(dr[0].ToString(), dr[1].ToString());
                }
            }
            return RoleTypes;
        }

        /// <summary>
        /// This will return a Dictionary of OfferRoleType/s and WorkflowRoleType/s linked to an Organisation Structure.
        /// The Key has a descriptor prefixed at the end to identify if you working in the context of a WorkflowRoleType or OfferRoleType.
        /// The reason for this "HACK" was OfferRoleType and WorkflowRoleType don't share a common interface so we have to code against implementation.
        /// </summary>
        /// <param name="OrganisationStructureKeys"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetRoleTypesByOrganisationStructureKey(List<int> OrganisationStructureKeys)
        {
            Dictionary<string, string> RoleTypes = new Dictionary<string, string>();
            string sql = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetRoleTypesByOrganisationStructureKey");
            sql = string.Format(sql, String.Join(",", OrganisationStructureKeys.Select((o) => o.ToString()).ToArray()));
            ParameterCollection parameters = new ParameterCollection();
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    RoleTypes.Add(dr["RoleTypeKey"].ToString(), dr["Description"].ToString());
                }
            }
            return RoleTypes;
        }

        /// <summary>
        /// This will return a Dictionary of OfferRoleType/s and WorkflowRoleType/s linked for an ADUser through their UserOrganisationStructure.
        /// The Key has a descriptor prefixed at the end to identify if you working in the context of a WorkflowRoleType or OfferRoleType.
        /// The reason for this "HACK" was OfferRoleType and WorkflowRoleType don't share a common interface so we have to code against implementation.
        /// </summary>
        /// <param name="OrganisationStructureKeys"></param>
        /// <param name="ADUserKey"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetRoleTypesByOrganisationStructureKeyAndADUserKey(List<int> OrganisationStructureKeys, int ADUserKey)
        {
            Dictionary<string, string> RoleTypes = new Dictionary<string, string>();
            string sql = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetRoleTypesByOrganisationStructureKeyAndADUserKey");
            sql = string.Format(sql, String.Join(",", OrganisationStructureKeys.Select((o) => o.ToString()).ToArray()));
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@ADUserKey", ADUserKey.ToString()));

            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    RoleTypes.Add(dr["RoleTypeKey"].ToString(), dr["Description"].ToString());
                }
            }
            return RoleTypes;
        }

        //        public List<IApplicationRoleType> GetApplicationRolesForUser(string ADUSerName)
        //        {
        //            string sql = String.Format(@"with OrgStructKeys(ParentKey) AS
        //                (
        //                select distinct os.ParentKey
        //                from OrganisationStructure os
        //                inner join UserOrganisationStructure uos on uos.OrganisationStructureKey = os.OrganisationStructureKey
        //                inner join ADUser au on au.ADUserKey = uos.ADUserKey
        //                where au.ADUserName = @ADUserName
        //                )
        //                select ort.OfferRoleTypeKey
        //                from OfferRoleType ort
        //                inner join OfferRoleTypeOrganisationStructureMapping ortosm on ortosm.OfferRoleTypeKey = ort.OfferRoleTypeKey
        //                inner join OrganisationStructure os on ortosm.OrganisationStructureKey = os.OrganisationStructureKey
        //                inner join OrgStructKeys temp on temp.ParentKey = os.ParentKey",
        //                ADUSerName);

        //            List<int> offerRoleTypeKeys = new List<int>();
        //            //IDbConnection conn = Helper.GetSQLDBConnection();
        //            //IDataReader reader = null;

        //            //conn.Open();

        //            ParameterCollection parameters = new ParameterCollection();
        //            parameters.Add(new SqlParameter("@ADUserName", ADUSerName));

        //            DataSet ds = ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

        //            //reader = Helper.ExecuteReader(conn, sql, parameters);
        //            // if (reader.Read())
        //            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in ds.Tables[0].Rows)
        //                {
        //                    offerRoleTypeKeys.Add(Convert.ToInt32(dr[0]));
        //                }
        //            }

        //            // if there are no results, exit now
        //            if (offerRoleTypeKeys.Count == 0)
        //                return new List<IApplicationRoleType>();

        //            // we've got the keys, now we can grab the applications
        //            ICriterion[] criteria = new ICriterion[]
        //                {
        //                    Expression.In("Key", offerRoleTypeKeys)
        //                };
        //            ApplicationRoleType_DAO[] offerRoleTypes = ApplicationRoleType_DAO.FindAll(criteria);
        //            return new List<IApplicationRoleType>(new DAOEventList<ApplicationRoleType_DAO, IApplicationRoleType, ApplicationRoleType>(offerRoleTypes));

        //            #region Old NHibernate Code - Performance was terrible
        //            /*
        //            IADUser user = GetAdUserForAdUserName(ADUSerName);
        //            List<IApplicationRoleType> RoleTypeList = new List<IApplicationRoleType>();
        //            foreach (IUserOrganisationStructure uos in user.UserOrganisationStructure)
        //            {
        //                // get the parent of this guy
        //                IOrganisationStructure parent = uos.OrganisationStructure.Parent;

        //                if (parent == null || parent.ChildOrganisationStructures == null)
        //                    continue;

        //                // loop through ALL the children
        //                foreach (IOrganisationStructure os in parent.ChildOrganisationStructures)
        //                {
        //                    IEventList<IApplicationRoleType> roletypes = os.ApplicationRoleTypes;
        //                    foreach (IApplicationRoleType rt in roletypes)
        //                    {
        //                        if (!RoleTypeList.Contains(rt))
        //                            RoleTypeList.Add(rt);
        //                    }
        //                }
        //            }
        //            return RoleTypeList;
        //            //os.GetUsersForDynamicRole(
        //             */
        //            #endregion
        //        }

        #endregion Branch Admin, Consultant stuff

        #region Helpdesk

        public IList<IADUser> GetAllHelpdeskConsultants(string OrgStructureName, IADUser currentUser)
        {
            IOrganisationStructure os = this.GetOrganisationStructureForDescription(OrgStructureName);
            IEventList<IOrganisationStructure> osBrats = os.ChildOrganisationStructures;

            // why a seperate collection? So the consumer can remove the existingly assigned person without
            // modifying the ieventlist which will change the DB.

            SortedList<string, IADUser> sortedList = new SortedList<string, IADUser>();
            foreach (IOrganisationStructure osBrat in osBrats)
            {
                IEventList<IADUser> users = osBrat.ADUsers;
                foreach (IADUser a in users)
                {
                    if (currentUser != null)
                    {
                        if (!sortedList.ContainsKey(a.ADUserName) && a.Key != currentUser.Key)
                            sortedList.Add(a.ADUserName, a);
                    }
                    else
                    {
                        if (!sortedList.ContainsKey(a.Key.ToString()))
                            sortedList.Add(a.ADUserName, a);
                    }
                }
            }

            IList<IADUser> helpDeskUsers = new List<IADUser>();
            foreach (KeyValuePair<string, IADUser> kv in sortedList)
            {
                helpDeskUsers.Add(kv.Value);
            }
            return helpDeskUsers;
        }

        #endregion Helpdesk

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="adUserName"></param>
        /// <returns></returns>
        public IApplicationRole GetApplicationRoleForADUser(int applicationKey, string adUserName)
        {
            int _offerRoleTypeGroupKey = (int)SAHL.Common.Globals.OfferRoleTypeGroups.Operator;
            int _generalStatusKey = (int)SAHL.Common.Globals.GeneralStatuses.Active;
            IApplicationRole _appRole = null;
            string sql = string.Empty;

            sql = string.Format(@"select TOP 1 OFR.*
            from [2AM].DBO.[OfferRole] OFR (nolock)
            INNER JOIN [2AM].[DBO].[OfferRoleType] ORT (nolock)
            ON ORT.OfferRoleTypeKey = OFR.OfferRoleTypeKey
            INNER JOIN [2AM].[DBO].OfferRoleTypeGroup ORTG (nolock)
            ON ORT.OfferRoleTypeGroupKey = ORTG.OfferRoleTypeGroupKey
            INNER JOIN [2AM].[DBO].[ADUSER] AD (nolock)
            ON OFR.LegalEntityKey = AD.LegalEntityKey
            WHERE OFR.OfferKey = {0} AND AD.ADUserName = '{1}' AND ORTG.OfferRoleTypeGroupKey = {2} AND OFR.GeneralStatusKey = {3}
            ORDER BY OFR.StatusChangeDate DESC",
            applicationKey, adUserName, _offerRoleTypeGroupKey, _generalStatusKey);

            SimpleQuery<ApplicationRole_DAO> arQ = new SimpleQuery<ApplicationRole_DAO>(QueryLanguage.Sql, sql);
            arQ.AddSqlReturnDefinition(typeof(ApplicationRole_DAO), "OFR");
            ApplicationRole_DAO[] arRes = arQ.Execute();

            if (arRes.Length == 0)
                return _appRole;
            else
            {
                _appRole = new ApplicationRole(arRes[0]);
                return _appRole;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="adUserName"></param>
        /// <param name="appRoleTypeKey"></param>
        /// <returns></returns>
        [Obsolete("Method GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyAndLegalEntityKey should be used.")]
        public IApplicationRole GetApplicationRoleByADUserRoleType(int applicationKey, string adUserName, int appRoleTypeKey)
        {
            int _offerRoleTypeGroupKey = (int)SAHL.Common.Globals.OfferRoleTypeGroups.Operator;
            int _generalStatusKey = (int)SAHL.Common.Globals.GeneralStatuses.Active;
            IApplicationRole _appRole = null;
            string sql = string.Empty;

            sql = string.Format(@"select TOP 1 OFR.*
            from [2AM].DBO.[OfferRole] OFR (nolock)
            INNER JOIN [2AM].[DBO].[OfferRoleType] ORT (nolock)
            ON ORT.OfferRoleTypeKey = OFR.OfferRoleTypeKey
            INNER JOIN [2AM].[DBO].OfferRoleTypeGroup ORTG (nolock)
            ON ORT.OfferRoleTypeGroupKey = ORTG.OfferRoleTypeGroupKey
            INNER JOIN [2AM].[DBO].[ADUSER] AD (nolock)
            ON OFR.LegalEntityKey = AD.LegalEntityKey
            WHERE OFR.OfferKey = {0} AND AD.ADUserName = '{1}' AND ORTG.OfferRoleTypeGroupKey = {2} AND OFR.GeneralStatusKey = {3}
            AND ORT.OfferRoleTypeKey = {4}
            ORDER BY OFR.StatusChangeDate DESC",
            applicationKey, adUserName, _offerRoleTypeGroupKey, _generalStatusKey, appRoleTypeKey);

            SimpleQuery<ApplicationRole_DAO> arQ = new SimpleQuery<ApplicationRole_DAO>(QueryLanguage.Sql, sql);
            arQ.AddSqlReturnDefinition(typeof(ApplicationRole_DAO), "OFR");
            ApplicationRole_DAO[] arRes = arQ.Execute();

            if (arRes.Length == 0)
                return _appRole;
            else
            {
                _appRole = new ApplicationRole(arRes[0]);
                return _appRole;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="adUserName"></param>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        public IEventList<IApplicationRole> GetApplicationRolesByAppKey(int applicationKey, string adUserName, long instanceID)
        {
            int _offerRoleTypeGroupKey = (int)SAHL.Common.Globals.OfferRoleTypeGroups.Operator;
            int _generalStatusKey = (int)SAHL.Common.Globals.GeneralStatuses.Active;
            IEventList<IApplicationRole> _appRoleList = new EventList<IApplicationRole>();
            List<string> x2SecGrpList = new List<string>();
            string sql = string.Empty;

            sql = @"SELECT SG.*
                FROM X2.X2.INSTANCE INST (nolock)
                INNER JOIN X2.X2.STATE ST (nolock)
                ON INST.STATEID = ST.ID
                INNER JOIN X2.X2.STATEWORKLIST STWL (nolock)
                ON ST.ID = STWL.STATEID
                INNER JOIN X2.X2.SECURITYGROUP SG (nolock)
                ON STWL.SECURITYGROUPID = SG.ID
                WHERE INST.ID = ?";

            SimpleQuery<SecurityGroup_DAO> secQ = new SimpleQuery<SecurityGroup_DAO>(QueryLanguage.Sql, sql, instanceID);
            secQ.AddSqlReturnDefinition(typeof(SecurityGroup_DAO), "SG");
            SecurityGroup_DAO[] secRes = secQ.Execute();

            if (secRes.Length == 0)
                return _appRoleList;

            foreach (SecurityGroup_DAO secGroup in secRes)
            {
                x2SecGrpList.Add(secGroup.Name);
            }

            // Retrieve the active top 1 role on the application of the logged in user
            sql = string.Format(@"select TOP 1 OFR.*
            from [2AM].DBO.[OfferRole] OFR (nolock)
            INNER JOIN [2AM].[DBO].[OfferRoleType] ORT (nolock)
            ON ORT.OfferRoleTypeKey = OFR.OfferRoleTypeKey
            INNER JOIN [2AM].[DBO].OfferRoleTypeGroup ORTG (nolock)
            ON ORT.OfferRoleTypeGroupKey = ORTG.OfferRoleTypeGroupKey
            INNER JOIN [2AM].[DBO].[ADUSER] AD (nolock)
            ON OFR.LegalEntityKey = AD.LegalEntityKey
            WHERE OFR.OfferKey = {0} AND AD.ADUserName = '{1}' AND ORTG.OfferRoleTypeGroupKey = {2} AND OFR.GeneralStatusKey = {3}
            AND ORT.Description in (:secgrps)
            ORDER BY OFR.StatusChangeDate DESC",
            applicationKey, adUserName, _offerRoleTypeGroupKey, _generalStatusKey);

            SimpleQuery<ApplicationRole_DAO> arQ = new SimpleQuery<ApplicationRole_DAO>(QueryLanguage.Sql, sql);
            arQ.AddSqlReturnDefinition(typeof(ApplicationRole_DAO), "OFR");
            arQ.SetParameterList("secgrps", x2SecGrpList);
            ApplicationRole_DAO[] arRes = arQ.Execute();

            // If the logged in User does not play a role in the application
            // then find the top 1 operator role on the application
            if (arRes.Length == 0)
            {
                sql = string.Format(@"select OFR.*
                from [2AM].DBO.[OfferRole] OFR (nolock)
                INNER JOIN [2AM].[DBO].[OfferRoleType] ORT (nolock)
                ON ORT.OfferRoleTypeKey = OFR.OfferRoleTypeKey
                INNER JOIN [2AM].[DBO].OfferRoleTypeGroup ORTG (nolock)
                ON ORT.OfferRoleTypeGroupKey = ORTG.OfferRoleTypeGroupKey
                WHERE OFR.OfferKey = {0} AND ORTG.OfferRoleTypeGroupKey = {1} AND OFR.GeneralStatusKey = {2}
                AND ORT.Description in (:secgrps)
                ORDER BY OFR.StatusChangeDate DESC",
                applicationKey, _offerRoleTypeGroupKey, _generalStatusKey);

                arQ = new SimpleQuery<ApplicationRole_DAO>(QueryLanguage.Sql, sql);
                arQ.AddSqlReturnDefinition(typeof(ApplicationRole_DAO), "OFR");
                arQ.SetParameterList("secgrps", x2SecGrpList);
                arRes = arQ.Execute();

                // If there is still no operator application roles against an application then can't be re-assigned
                if (arRes.Length == 0)
                    return _appRoleList;
            }

            // app role has been found
            foreach (ApplicationRole_DAO _appRole in arRes)
            {
                _appRoleList.Add(null, new ApplicationRole(_appRole));
            }
            return _appRoleList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="orgStruct"></param>
        /// <param name="appRole"></param>
        /// <returns></returns>
        public IEventList<IADUser> GetADUserPerOrgStructAndAppRole(IOrganisationStructure orgStruct, IApplicationRole appRole)
        {
            IEventList<IADUser> adUserList = new EventList<IADUser>();
            SortedList<string, IADUser> sortedList = new SortedList<string, IADUser>();
            string sql = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetADUserPerOrgStructAndAppRole");
            SimpleQuery<ADUser_DAO> adQ = new SimpleQuery<ADUser_DAO>(QueryLanguage.Sql, sql, orgStruct.Key, appRole.ApplicationRoleType.Key, appRole.LegalEntity.Key);
            adQ.AddSqlReturnDefinition(typeof(ADUser_DAO), "AD");
            ADUser_DAO[] adRes = adQ.Execute();

            foreach (ADUser_DAO aduser in adRes)
            {
                if (!sortedList.ContainsKey(aduser.ADUserName))
                    sortedList.Add(aduser.ADUserName, new ADUser(aduser));
            }

            foreach (KeyValuePair<string, IADUser> kv in sortedList)
            {
                adUserList.Add(null, kv.Value);
            }

            return adUserList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="ApplicationRoleTypeKey"></param>
        /// <returns></returns>
        public DataTable GetReassignUserApplicationRoleList(int ApplicationKey, int ApplicationRoleTypeKey)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "ReassignUserApplicationRoleList");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@OfferKey", ApplicationKey));
            parameters.Add(new SqlParameter("@OfferRoleTypeKey", ApplicationRoleTypeKey));

            DataTable dtRoles = new DataTable();
            DataSet dsRoles = new DataSet();
            dsRoles = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (dsRoles != null)
            {
                if (dsRoles.Tables.Count > 0)
                {
                    dtRoles = dsRoles.Tables[0];
                }
            }
            return dtRoles;
        }

        public IEventList<IApplicationRoleType> GetCreditRoleTypes(int OrganisationStructureKey)
        {
            string sql = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetCreditRoleTypes");
            IEventList<IApplicationRoleType> artList = new EventList<IApplicationRoleType>();
            SimpleQuery<ApplicationRoleType_DAO> artQ = new SimpleQuery<ApplicationRoleType_DAO>(QueryLanguage.Sql, sql, OrganisationStructureKey, OrganisationStructureKey);
            artQ.AddSqlReturnDefinition(typeof(ApplicationRoleType_DAO), "ort");
            ApplicationRoleType_DAO[] artRes = artQ.Execute();

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            foreach (ApplicationRoleType_DAO _appRoleType in artRes)
            {
                artList.Add(null, BMTM.GetMappedType<IApplicationRoleType, ApplicationRoleType_DAO>(_appRoleType));
            }
            return artList;
        }

        #region Generic Reassign Methods

        /// <summary>
        /// If the reuse of OfferRole is required then a TRUE should be passed.
        /// However if a new OfferRole is required to be created for historical purposes or...then pass FALSE
        /// http://sahls31:8181/trac/sahl.db/ticket/12791
        /// </summary>
        /// <param name="ApplicationRoleTypeKey"></param>
        /// <param name="ApplicationKey"></param>
        /// <param name="LegalEntityKey"></param>
        /// <param name="ReUse"></param>
        /// <returns></returns>
        public IApplicationRole GenerateApplicationRole(int ApplicationRoleTypeKey, int ApplicationKey, int LegalEntityKey, bool ReUse)
        {
            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            ILegalEntityRepository leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            IApplicationRole newAppRole = null;
            IApplication app = appRepo.GetApplicationByKey(ApplicationKey);
            IApplicationRoleType appRoleType = GetApplicationRoleTypeByKey(ApplicationRoleTypeKey);
            ILegalEntity le = leRepo.GetLegalEntityByKey(LegalEntityKey);

            // If the reuse of OfferRole is required then a TRUE should be passed.
            // However if a new OfferRole is required to be created for historical purposes or...then pass FALSE
            if (ReUse)
            {
                newAppRole = GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyAndLegalEntityKey(ApplicationKey, ApplicationRoleTypeKey, LegalEntityKey, (int)GeneralStatuses.Active);
                if (newAppRole == null)
                    newAppRole = GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyAndLegalEntityKey(ApplicationKey, ApplicationRoleTypeKey, LegalEntityKey, (int)GeneralStatuses.Inactive);
            }

            // We only interested in creating/upating a new OfferRole or an Inactive OfferRole
            // If the OfferRole already exists it should be returned back to the application.
            if (newAppRole == null)
            {
                newAppRole = CreateNewApplicationRole();
                newAppRole.Application = app;
                newAppRole.ApplicationRoleType = appRoleType;
                newAppRole.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Active];
                newAppRole.StatusChangeDate = DateTime.Now;
                newAppRole.LegalEntity = le;
                SaveApplicationRole(newAppRole);
            }
            else if (newAppRole.GeneralStatus.Key == (int)GeneralStatuses.Inactive)
            {
                newAppRole.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Active];
                newAppRole.StatusChangeDate = DateTime.Now;
                SaveApplicationRole(newAppRole);
            }

            // This is fail safe call to make sure we only have one active OfferRole
            // for the given OfferRoleType against the Application.
            DeactivateExistingApplicationRoles(ApplicationKey, ApplicationRoleTypeKey, newAppRole.Key);

            // Need to ensure the statuschangedate on the approle is the latest date.
            newAppRole.StatusChangeDate = DateTime.Now;
            SaveApplicationRole(newAppRole);

            return newAppRole;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationRoleKey"></param>
        public void DeactivateApplicationRole(int ApplicationRoleKey)
        {
            IApplicationRole appRole = GetApplicationRoleByKey(ApplicationRoleKey);
            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            appRole.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Inactive];
            appRole.StatusChangeDate = DateTime.Now;
            SaveApplicationRole(appRole);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="ApplicationRoleTypeKey"></param>
        public void DeactivateExistingApplicationRoles(int ApplicationKey, int ApplicationRoleTypeKey)
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplication app = appRepo.GetApplicationByKey(ApplicationKey);

            foreach (IApplicationRole appRole in app.ApplicationRoles)
            {
                if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active
                    && appRole.ApplicationRoleType.Key == ApplicationRoleTypeKey)
                {
                    DeactivateApplicationRole(appRole.Key);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="ApplicationRoleTypeKey"></param>
        /// <param name="ApplicationRoleKey"></param>
        public void DeactivateExistingApplicationRoles(int ApplicationKey, int ApplicationRoleTypeKey, int ApplicationRoleKey)
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplication app = appRepo.GetApplicationByKey(ApplicationKey);

            foreach (IApplicationRole appRole in app.ApplicationRoles)
            {
                if (appRole.Key != ApplicationRoleKey
                    && appRole.GeneralStatus.Key == (int)GeneralStatuses.Active
                    && appRole.ApplicationRoleType.Key == ApplicationRoleTypeKey)
                {
                    DeactivateApplicationRole(appRole.Key);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationRoleKey"></param>
        /// <returns></returns>
        public int GetOfferRoleTypeOrganisationStructureMappingKey(int ApplicationRoleKey)
        {
            string query = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetOfferRoleTypeOrganisationStructureMapping");
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@OfferRoleKey", ApplicationRoleKey));
            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(OrganisationStructure_DAO), pc);
            return Convert.ToInt32(o);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="organisationStructureKey"></param>
        /// <param name="offerRoleTypeKey"></param>
        /// <returns></returns>
        public int GetOfferRoleTypeOrganisationStructureMappingKey(int organisationStructureKey, int offerRoleTypeKey)
        {
            string query = string.Format("select OfferRoleTypeOrganisationStructureMappingKey from [2am]..OfferRoleTypeOrganisationStructureMapping where OrganisationStructureKey={0} and OfferRoleTypeKey='{1}'", organisationStructureKey, offerRoleTypeKey);
            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), new ParameterCollection());
            if (null != o)
                return Convert.ToInt32(o);

            return -1;
        }

        public void DeactivateWorkflowAssignment(IApplicationRole AppRole, int InstanceID)
        {
            // Deactive all WorkflowAssignment records for InstanceID
            string query = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "DeactivateWorkflowAssignmentByInstanceID");
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@InstanceID", InstanceID));
            pc.Add(new SqlParameter("@ApplicationRoleTypeKey", AppRole.ApplicationRoleType.Key));
            castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(OrganisationStructure_DAO), pc);
        }

        public void CreateWorkflowAssignment(IApplicationRole AppRole, int InstanceID, GeneralStatuses GeneralStatus)
        {
            int offerRoleTypeOrganisationStructureMappingKey = GetOfferRoleTypeOrganisationStructureMappingKey(AppRole.Key);
            IADUser aduser = GetAdUserByLegalEntityKey(AppRole.LegalEntity.Key);

            // Deactive all WorkflowAssignment records for InstanceID
            DeactivateWorkflowAssignment(AppRole, InstanceID);

            // Insert record for create application role
            ParameterCollection paramsWFA = new ParameterCollection();
            paramsWFA.Add(new SqlParameter("@InstanceID", InstanceID));
            paramsWFA.Add(new SqlParameter("@OfferRoleTypeOrganisationStructureMappingKey", offerRoleTypeOrganisationStructureMappingKey));
            paramsWFA.Add(new SqlParameter("@ADUserKey", aduser.Key));
            paramsWFA.Add(new SqlParameter("@GeneralStatusKey", (int)GeneralStatus));
            paramsWFA.Add(new SqlParameter("@InsertDate", DateTime.Now));

            string query = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "CreateWorkflowAssignment");
            castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(OrganisationStructure_DAO), paramsWFA);
        }

        public DataTable GetWorkflowAssignment(int InstanceID, int OfferRoleTypeOrganisationStructureMappingKey, int ADUserKey, int GeneralStatusKey)
        {
            string query = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetWorkflowAssignment");
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@instanceid", InstanceID));
            pc.Add(new SqlParameter("@OfferRoleTypeOrganisationStructureMappingKey", OfferRoleTypeOrganisationStructureMappingKey));
            pc.Add(new SqlParameter("@ADUserKey", ADUserKey));
            pc.Add(new SqlParameter("@GeneralStatusKey", GeneralStatusKey));

            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), pc);
            if (ds.Tables != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public bool HasActiveWorkflowAssignment(int InstanceID, IApplicationRole AppRole)
        {
            int ortosmKey = GetOfferRoleTypeOrganisationStructureMappingKey(AppRole.Key);
            IADUser aduser = GetAdUserByLegalEntityKey(AppRole.LegalEntity.Key);
            DataTable dt = GetWorkflowAssignment(InstanceID, ortosmKey, aduser.Key, (int)GeneralStatuses.Active);
            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public void ReassignUser(int applicationKey, long instanceID, int aduserKey, IApplicationRole applicationRole, out string message)
        {
            // Deactivate existing workflow assignment records
            this.DeactivateWorkflowAssignment(applicationRole, (int)instanceID);

            // Get the IADUser to assign to
            IADUser userSelected = this.GetADUserByKey(aduserKey);

            // Get the IApplication object
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplication application = appRepo.GetApplicationByKey(applicationKey);

            // Create the new application role
            IApplicationRole newAppRole = this.GenerateApplicationRole(applicationRole.ApplicationRoleType.Key, applicationKey, userSelected.LegalEntity.Key, true);

            // Do X2 Stuff
            this.CreateWorkflowAssignment(newAppRole, (int)instanceID, GeneralStatuses.Active);

            // Create History from X2
            IADUser fromUser = this.GetAdUserByLegalEntityKey(applicationRole.LegalEntity.Key);
            IADUser toUser = this.GetAdUserByLegalEntityKey(newAppRole.LegalEntity.Key);
            message = string.Format("Reassigned from {0} to {1}", fromUser.ADUserName, toUser.ADUserName);

            // Reassigning FLProcessorD ApplicationRole
            if (applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.FLProcessorD)
            {
                IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
                foreach (IApplication flApplication in application.Account.Applications)
                {
                    if ((flApplication.Key != application.Key && flApplication.ApplicationStatus.Key == (int)OfferStatuses.Open)
                        && (flApplication.ApplicationType.Key == (int)OfferTypes.ReAdvance
                        || flApplication.ApplicationType.Key == (int)OfferTypes.FurtherAdvance
                        || flApplication.ApplicationType.Key == (int)OfferTypes.FurtherLoan))
                    {
                        // Create the new FLProcessorD application role
                        IApplicationRole flAppRole = this.GenerateApplicationRole(applicationRole.ApplicationRoleType.Key, flApplication.Key, userSelected.LegalEntity.Key, true);

                        // create the workflowassignment records - these must be Inactive as they are for tracking purposes only
                        IList<IInstance> instances = x2Repo.GetInstancesForGenericKey(flApplication.Key, SAHL.Common.Constants.WorkFlowProcessName.Origination);
                        foreach (IInstance instance in instances)
                        {
                            this.CreateWorkflowAssignment(flAppRole, (int)instance.ID, GeneralStatuses.Inactive);
                        }
                    }
                }
            }
        }

        #endregion Generic Reassign Methods

        #region Administration : UserOrganisationStructure

        public DataTable GetOrganisationStructureConfirmationList(List<int> selectedNodesLst)
        {
            string delimitedList = string.Empty;
            for (int i = 0; i < selectedNodesLst.Count; i++)
            {
                if (i == 0)
                    delimitedList += selectedNodesLst[i].ToString();
                else
                    delimitedList += "," + selectedNodesLst[i].ToString();
            }

            string query = string.Format(UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetOrganisationStructureConfirmationList"), delimitedList);

            ParameterCollection parameters = new ParameterCollection();

            DataTable dtList = new DataTable();
            DataSet dsList = new DataSet();
            dsList = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (dsList != null)
            {
                if (dsList.Tables.Count > 0)
                {
                    dtList = dsList.Tables[0];
                }
            }
            return dtList;
        }

        public DataTable GetUserOrganisationStructureHistory(int ADUserKey)
        {
            string sql = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetUserOrganisationStructureHistory");

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@ADUserKey", ADUserKey));

            DataTable dtStructure = new DataTable();
            DataSet dsStructure = new DataSet();
            dsStructure = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);
            if (dsStructure != null)
            {
                if (dsStructure.Tables.Count > 0)
                {
                    dtStructure = dsStructure.Tables[0];
                }
            }
            return dtStructure;
        }

        public IUserOrganisationStructure GetEmptyUserOrganisationStructure()
        {
            return base.CreateEmpty<IUserOrganisationStructure, UserOrganisationStructure_DAO>();
        }

        public IUserOrganisationStructureHistory GetEmptyUserOrganisationStructureHistory()
        {
            return base.CreateEmpty<IUserOrganisationStructureHistory, UserOrganisationStructureHistory_DAO>();
        }

        public void SaveUserOrganisationStructure(IUserOrganisationStructure uos)
        {
            base.Save<IUserOrganisationStructure, UserOrganisationStructure_DAO>(uos);
        }

        public void SaveUserOrganisationStructureHistory(IUserOrganisationStructureHistory uosh)
        {
            base.Save<IUserOrganisationStructureHistory, UserOrganisationStructureHistory_DAO>(uosh);
        }

        public IAllocationMandateSetGroup GetAllocationMandateSetGroupByUserOrganisationStructureKey(int UserOrganisationStructureKey)
        {
            string sql = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetAllocationMandateSetGrpByUserOrgStructKey");
            sql = string.Format(sql, UserOrganisationStructureKey);

            SimpleQuery<AllocationMandateSetGroup_DAO> query = new SimpleQuery<AllocationMandateSetGroup_DAO>(QueryLanguage.Sql, sql);
            query.AddSqlReturnDefinition(typeof(AllocationMandateSetGroup_DAO), "amsg");
            AllocationMandateSetGroup_DAO[] res = query.Execute();

            if (res != null && res.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IAllocationMandateSetGroup, AllocationMandateSetGroup_DAO>(res[0]);
            }
            else
                return null;
        }

        #endregion Administration : UserOrganisationStructure

        #region Workflow Batch Reassign

        public Dictionary<IADUser, int> GetADUsersPerRoleTypeAndOrgStructDictionary(IApplicationRoleType appRoleType, IList<IOrganisationStructure> orgList)
        {
            string sql = string.Empty;
            List<int> adUserKeyLst = new List<int>();
            Dictionary<IADUser, int> adUserDict = new Dictionary<IADUser, int>();
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            foreach (IOrganisationStructure _orgStruct in orgList)
            {
                int _orgStructKey = _orgStruct.Key;

                if (_orgStruct.Parent != null)
                    _orgStructKey = _orgStruct.Parent.Key;

                // Retrieves ADUsers that are currently linked through the UserOrganisationStructure
                sql = string.Format(@";WITH OS (DESCRIPTION, ORGANISATIONSTRUCTUREKEY, PARENTKEY) AS
                                    (
                                        SELECT DESCRIPTION, ORGANISATIONSTRUCTUREKEY, PARENTKEY
                                        FROM   DBO.ORGANISATIONSTRUCTURE
                                        WHERE  ORGANISATIONSTRUCTUREKEY =  {0}
                                        UNION ALL
                                        SELECT ORG.DESCRIPTION, ORG.ORGANISATIONSTRUCTUREKEY, ORG.PARENTKEY
                                        FROM   DBO.ORGANISATIONSTRUCTURE ORG
                                        INNER JOIN
                                        OS
                                        ON
                                        OS.ORGANISATIONSTRUCTUREKEY = ORG.PARENTKEY
                                        WHERE  ORG.ORGANISATIONSTRUCTUREKEY <> {0}
                                    )
                                    SELECT DISTINCT UOS.OrganisationStructureKey, AD.ADUserKey
                                    FROM OS
                                    INNER JOIN [2AM].[DBO].OFFERROLETYPEORGANISATIONSTRUCTUREMAPPING OSM
                                        ON OSM.ORGANISATIONSTRUCTUREKEY = OS.ORGANISATIONSTRUCTUREKEY
                                    INNER JOIN [2AM].[DBO].[USERORGANISATIONSTRUCTURE] UOS
                                        ON OSM.ORGANISATIONSTRUCTUREKEY = UOS.ORGANISATIONSTRUCTUREKEY
                                    INNER JOIN [2AM].[DBO].[ADUSER] AD
                                        ON UOS.ADUSERKEY = AD.ADUSERKEY
                                    WHERE OSM.OFFERROLETYPEKEY = {1}", _orgStructKey, appRoleType.Key);

                ParameterCollection parameters = new ParameterCollection();

                DataSet dsUsers = new DataSet();
                dsUsers = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);
                if (dsUsers != null)
                {
                    if (dsUsers.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsUsers.Tables[0].Rows)
                        {
                            int OrganisationStructureKey = Convert.ToInt32(dr[0]);
                            int ADUserKey = Convert.ToInt32(dr[1]);

                            if (!adUserKeyLst.Contains(ADUserKey))
                            {
                                adUserKeyLst.Add(ADUserKey);
                                adUserDict.Add(GetADUserByKey(ADUserKey), (OrganisationStructureKey * -1));
                            }
                        }
                    }
                }

                // This section of the code retrives ADUsers that have been moved out of this part of the organisation structure
                // Allowing for their cases to be batch reassigned.
                sql = string.Format(@";WITH OS (DESCRIPTION, ORGANISATIONSTRUCTUREKEY, PARENTKEY) AS
                                        (
                                            SELECT DESCRIPTION, ORGANISATIONSTRUCTUREKEY, PARENTKEY
                                            FROM   DBO.ORGANISATIONSTRUCTURE
                                            WHERE  ORGANISATIONSTRUCTUREKEY =  {0}
                                            UNION ALL
                                            SELECT ORG.DESCRIPTION, ORG.ORGANISATIONSTRUCTUREKEY, ORG.PARENTKEY
                                            FROM   DBO.ORGANISATIONSTRUCTURE ORG
                                            INNER JOIN
                                            OS
                                            ON
                                            OS.ORGANISATIONSTRUCTUREKEY = ORG.PARENTKEY
                                            WHERE  ORG.ORGANISATIONSTRUCTUREKEY <> {0}
                                        )
                                        SELECT DISTINCT UOSH.OrganisationStructureKey, AD.ADUserKey
                                        FROM OS
                                        INNER JOIN [2AM].[DBO].USERORGANISATIONSTRUCTUREHISTORY UOSH (NOLOCK)
	                                        ON OS.ORGANISATIONSTRUCTUREKEY = UOSH.ORGANISATIONSTRUCTUREKEY
                                        INNER JOIN [2AM].[DBO].OFFERROLETYPEORGANISATIONSTRUCTUREMAPPING ORTOSM (NOLOCK)
	                                        ON ORTOSM.ORGANISATIONSTRUCTUREKEY = UOSH.ORGANISATIONSTRUCTUREKEY
                                        INNER JOIN [2AM].[DBO].ADUSER AD (NOLOCK)
	                                        ON UOSH.ADUSERKEY = AD.ADUSERKEY
                                        WHERE UOSH.ACTION = 'D' AND ORTOSM.OFFERROLETYPEKEY = {1}", _orgStructKey, appRoleType.Key);

                //conn = Helper.GetSQLDBConnection();
                //try
                //{
                //    conn.Open();
                //    reader = Helper.ExecuteReader(conn, sql);
                //    while (reader.Read())
                //    {
                //        int OrganisationStructureKey = Convert.ToInt32(reader[0]);
                //        int ADUserKey = Convert.ToInt32(reader[1]);

                //        if (!adUserKeyLst.Contains(ADUserKey))
                //        {
                //            adUserKeyLst.Add(ADUserKey);
                //            adUserDict.Add(GetADUserByKey(ADUserKey), OrganisationStructureKey);
                //        }
                //    }
                //}
                //finally
                //{
                //    if (reader != null)
                //        reader.Dispose();
                //    if (conn != null)
                //        conn.Dispose();
                //}
                dsUsers = new DataSet();
                dsUsers = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

                if (dsUsers != null)
                {
                    if (dsUsers.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsUsers.Tables[0].Rows)
                        {
                            int OrganisationStructureKey = Convert.ToInt32(dr[0]);
                            int ADUserKey = Convert.ToInt32(dr[1]);

                            if (!adUserKeyLst.Contains(ADUserKey))
                            {
                                adUserKeyLst.Add(ADUserKey);
                                adUserDict.Add(GetADUserByKey(ADUserKey), OrganisationStructureKey);
                            }
                        }
                    }
                }
            }

            return adUserDict;
        }

        public Dictionary<IADUser, int> GetADUsersPerWorkflowRoleTypeAndOrgStructDictionary(IWorkflowRoleType workflowRoleType, IList<IOrganisationStructure> orgList)
        {
            string sql = string.Empty;
            List<int> adUserKeyLst = new List<int>();
            Dictionary<IADUser, int> adUserDict = new Dictionary<IADUser, int>();
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            foreach (IOrganisationStructure _orgStruct in orgList)
            {
                int _orgStructKey = _orgStruct.Key;

                if (_orgStruct.Parent != null)
                    _orgStructKey = _orgStruct.Parent.Key;

                sql = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetADUsersPerWorkflowRoleTypeAndOrgStructure");
                ParameterCollection parameters = new ParameterCollection();
                parameters.Add(new SqlParameter("@OrganisationStructureKey", _orgStructKey));
                parameters.Add(new SqlParameter("@WorkflowRoleTypeKey", workflowRoleType.Key));

                DataSet dsUsers = new DataSet();
                dsUsers = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);
                if (dsUsers != null)
                {
                    if (dsUsers.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsUsers.Tables[0].Rows)
                        {
                            int OrganisationStructureKey = Convert.ToInt32(dr[0]);
                            int ADUserKey = Convert.ToInt32(dr[1]);

                            if (!adUserKeyLst.Contains(ADUserKey))
                            {
                                adUserKeyLst.Add(ADUserKey);
                                adUserDict.Add(GetADUserByKey(ADUserKey), (OrganisationStructureKey * -1));
                            }
                        }
                    }
                }

                // This section of the code retrives ADUsers that have been moved out of this part of the organisation structure
                // Allowing for their cases to be batch reassigned.
                sql = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetADUsersPerWorkflowRoleTypeAndOSHistory");
                parameters = new ParameterCollection();
                parameters.Add(new SqlParameter("@OrganisationStructureKey", _orgStructKey));
                parameters.Add(new SqlParameter("@WorkflowRoleTypeKey", workflowRoleType.Key));

                dsUsers = new DataSet();
                dsUsers = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

                if (dsUsers != null)
                {
                    if (dsUsers.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsUsers.Tables[0].Rows)
                        {
                            int OrganisationStructureKey = Convert.ToInt32(dr[0]);
                            int ADUserKey = Convert.ToInt32(dr[1]);

                            if (!adUserKeyLst.Contains(ADUserKey))
                            {
                                adUserKeyLst.Add(ADUserKey);
                                adUserDict.Add(GetADUserByKey(ADUserKey), OrganisationStructureKey);
                            }
                        }
                    }
                }
            }

            return adUserDict;
        }

        public IEventList<IADUser> GetADUsersPerRoleTypeAndOrgStructList(IApplicationRoleType appRoleType, IList<IOrganisationStructure> orgList, bool filtered)
        {
            string sql = string.Empty;
            IEventList<IADUser> adUserList = new EventList<IADUser>();
            SortedList<string, IADUser> sortedList = new SortedList<string, IADUser>();
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            IADUser _addADUser = null;

            // Retrieves ADUsers that are currently linked through the UserOrganisationStructure
            foreach (IOrganisationStructure _orgStruct in orgList)
            {
                int _orgStructKey = _orgStruct.Key;

                if (_orgStruct.Parent != null)
                    _orgStructKey = _orgStruct.Parent.Key;

                sql = string.Format(@";WITH OS (DESCRIPTION, ORGANISATIONSTRUCTUREKEY, PARENTKEY) AS
                                    (
                                        SELECT DESCRIPTION, ORGANISATIONSTRUCTUREKEY, PARENTKEY
                                        FROM   DBO.ORGANISATIONSTRUCTURE (nolock)
                                        WHERE  ORGANISATIONSTRUCTUREKEY =  {0}
                                        UNION ALL
                                        SELECT ORG.DESCRIPTION, ORG.ORGANISATIONSTRUCTUREKEY, ORG.PARENTKEY
                                        FROM   DBO.ORGANISATIONSTRUCTURE ORG (nolock)
                                        INNER JOIN
                                        OS
                                        ON
                                        OS.ORGANISATIONSTRUCTUREKEY = ORG.PARENTKEY
                                        WHERE  ORG.ORGANISATIONSTRUCTUREKEY <> {0}
                                    )
                                    SELECT DISTINCT AD.*
                                    FROM OS
                                    INNER JOIN [2AM].[DBO].OFFERROLETYPEORGANISATIONSTRUCTUREMAPPING OSM (nolock)
                                        ON OSM.ORGANISATIONSTRUCTUREKEY = OS.ORGANISATIONSTRUCTUREKEY
                                    INNER JOIN [2AM].[DBO].[USERORGANISATIONSTRUCTURE] UOS (nolock)
                                        ON OSM.ORGANISATIONSTRUCTUREKEY = UOS.ORGANISATIONSTRUCTUREKEY
                                    INNER JOIN [2AM].[DBO].[ADUSER] AD (nolock)
                                        ON UOS.ADUSERKEY = AD.ADUSERKEY
                                    WHERE OSM.OFFERROLETYPEKEY = {1}", _orgStructKey, appRoleType.Key);

                SimpleQuery<ADUser_DAO> adQ = new SimpleQuery<ADUser_DAO>(QueryLanguage.Sql, sql);
                adQ.AddSqlReturnDefinition(typeof(ADUser_DAO), "AD");
                ADUser_DAO[] adRes = adQ.Execute();

                foreach (ADUser_DAO aduser in adRes)
                {
                    _addADUser = BMTM.GetMappedType<IADUser, ADUser_DAO>(aduser);

                    if (filtered)
                    {
                        if (!sortedList.ContainsKey(_addADUser.ADUserName) && _addADUser.GeneralStatusKey.Key == (int)GeneralStatuses.Active)
                            sortedList.Add(_addADUser.ADUserName, _addADUser);
                    }
                    else
                    {
                        if (!sortedList.ContainsKey(_addADUser.ADUserName))
                            sortedList.Add(_addADUser.ADUserName, _addADUser);
                    }
                }

                if (!filtered)
                {
                    // This section of the code retrives ADUsers that have been moved out of this part of the organisation structure
                    // Allowing for their cases to be batch reassigned.
                    sql = string.Format(@";WITH OS (DESCRIPTION, ORGANISATIONSTRUCTUREKEY, PARENTKEY) AS
                                        (
                                            SELECT DESCRIPTION, ORGANISATIONSTRUCTUREKEY, PARENTKEY
                                            FROM   DBO.ORGANISATIONSTRUCTURE (nolock)
                                            WHERE  ORGANISATIONSTRUCTUREKEY =  {0}
                                            UNION ALL
                                            SELECT ORG.DESCRIPTION, ORG.ORGANISATIONSTRUCTUREKEY, ORG.PARENTKEY
                                            FROM   DBO.ORGANISATIONSTRUCTURE ORG (nolock)
                                            INNER JOIN
                                            OS
                                            ON
                                            OS.ORGANISATIONSTRUCTUREKEY = ORG.PARENTKEY
                                            WHERE  ORG.ORGANISATIONSTRUCTUREKEY <> {0}
                                        )
                                        SELECT DISTINCT AD.*
                                        FROM OS
                                        INNER JOIN [2AM].[DBO].USERORGANISATIONSTRUCTUREHISTORY UOSH (NOLOCK)
	                                        ON OS.ORGANISATIONSTRUCTUREKEY = UOSH.ORGANISATIONSTRUCTUREKEY
                                        INNER JOIN [2AM].[DBO].OFFERROLETYPEORGANISATIONSTRUCTUREMAPPING ORTOSM (NOLOCK)
	                                        ON ORTOSM.ORGANISATIONSTRUCTUREKEY = UOSH.ORGANISATIONSTRUCTUREKEY
                                        INNER JOIN [2AM].[DBO].ADUSER AD (NOLOCK)
	                                        ON UOSH.ADUSERKEY = AD.ADUSERKEY
                                        WHERE UOSH.ACTION = 'D' AND ORTOSM.OFFERROLETYPEKEY = {1}
                                        ORDER BY AD.ADUSERNAME", _orgStructKey, appRoleType.Key);

                    adQ = new SimpleQuery<ADUser_DAO>(QueryLanguage.Sql, sql);
                    adQ.AddSqlReturnDefinition(typeof(ADUser_DAO), "AD");
                    adRes = adQ.Execute();

                    foreach (ADUser_DAO aduser in adRes)
                    {
                        _addADUser = BMTM.GetMappedType<IADUser, ADUser_DAO>(aduser);
                        if (!sortedList.ContainsKey(_addADUser.ADUserName))
                            sortedList.Add(_addADUser.ADUserName, _addADUser);
                    }
                }
            }

            foreach (KeyValuePair<string, IADUser> kv in sortedList)
            {
                adUserList.Add(null, kv.Value);
            }

            return adUserList;
        }

        #endregion Workflow Batch Reassign

        #region Round Robin

        /// <summary>
        /// Get ADUsers linked via the Offer RoleType Organisation Structure Mapping
        /// </summary>
        /// <param name="appRoleTypeKey"></param>
        /// <param name="orgList"></param>
        /// <returns></returns>
        public DataTable GetADUsersPerRoleTypeAndOrgStructListDT(int appRoleTypeKey, IList<IOrganisationStructure> orgList)
        {
            string sql = string.Empty;
            string query = string.Empty;
            List<int> osKeys = new List<int>();
            DataSet ds = null;

            foreach (IOrganisationStructure _orgStruct in orgList)
            {
                int _orgStructKey = _orgStruct.Key;
                if (_orgStruct.Parent != null)
                    _orgStructKey = _orgStruct.Parent.Key;

                query = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetChildOrganisationStructures");
                sql = string.Format(query, _orgStructKey, _orgStructKey);

                ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int organisationstructureKey = Convert.ToInt32(dr[0]);
                        if (!osKeys.Contains(organisationstructureKey))
                            osKeys.Add(organisationstructureKey);
                    }
                }
            }

            string delimitedList = string.Empty;
            for (int i = 0; i < osKeys.Count; i++)
            {
                if (i == 0)
                    delimitedList += osKeys[i].ToString();
                else
                    delimitedList += "," + osKeys[i].ToString();
            }

            query = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetADUsersPerRoleTypeAndOrgStructListDT");
            sql = string.Format(query, appRoleTypeKey, delimitedList);

            ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
            if (ds.Tables != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        /// <summary>
        /// /// Get ADUsers linked via the Workflow RoleType Organisation Structure Mapping
        /// </summary>
        /// <param name="workflowRoleTypeKey"></param>
        /// <param name="orgList"></param>
        /// <returns></returns>
        public DataTable GetADUsersByWorkflowRoleTypeAndOrgStructList(int workflowRoleTypeKey, IList<IOrganisationStructure> orgList)
        {
            string sql = string.Empty;
            string query = string.Empty;
            List<int> osKeys = new List<int>();
            DataSet ds = null;

            foreach (IOrganisationStructure _orgStruct in orgList)
            {
                int _orgStructKey = _orgStruct.Key;
                if (_orgStruct.Parent != null)
                    _orgStructKey = _orgStruct.Parent.Key;

                query = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetChildOrganisationStructures");
                sql = string.Format(query, _orgStructKey, _orgStructKey);

                ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int organisationstructureKey = Convert.ToInt32(dr[0]);
                        if (!osKeys.Contains(organisationstructureKey))
                            osKeys.Add(organisationstructureKey);
                    }
                }
            }

            string delimitedList = string.Empty;
            for (int i = 0; i < osKeys.Count; i++)
            {
                if (i == 0)
                    delimitedList += osKeys[i].ToString();
                else
                    delimitedList += "," + osKeys[i].ToString();
            }

            query = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetADUsersByWorkflowRoleTypeAndOrgStructList");
            sql = string.Format(query, workflowRoleTypeKey, delimitedList);

            ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
            if (ds.Tables != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public IUserOrganisationStructureRoundRobinStatus GetUserOrganisationStructureRoundRobinStatus(int uosrrsKey)
        {
            return base.GetByKey<IUserOrganisationStructureRoundRobinStatus, UserOrganisationStructureRoundRobinStatus_DAO>(uosrrsKey);
        }

        public void SaveUserOrganisationStructureRoundRobinStatus(IUserOrganisationStructureRoundRobinStatus userOrganisationStructureRoundRobinStatus)
        {
            base.Save<IUserOrganisationStructureRoundRobinStatus, UserOrganisationStructureRoundRobinStatus_DAO>(userOrganisationStructureRoundRobinStatus);
        }

        public void SaveUserOrganisationStructure(IUserOrganisationStructure uos, DateTime startDate, IDomainMessageCollection messages)
        {
            ILookupRepository lookUpRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IUserOrganisationStructureRoundRobinStatus userOrganisationStructureRoundRobinStatus = this.GetEmptyUserOrganisationStructureRoundRobinStatus();

            if (startDate.Date > DateTime.Today)
                userOrganisationStructureRoundRobinStatus.GeneralStatus = lookUpRepo.GeneralStatuses[GeneralStatuses.Inactive];
            else
                userOrganisationStructureRoundRobinStatus.GeneralStatus = lookUpRepo.GeneralStatuses[GeneralStatuses.Active];

            userOrganisationStructureRoundRobinStatus.CapitecGeneralStatus = lookUpRepo.GeneralStatuses[GeneralStatuses.Inactive];

            userOrganisationStructureRoundRobinStatus.UserOrganisationStructure = uos;
            uos.UserOrganisationStructureRoundRobinStatus.Add(messages, userOrganisationStructureRoundRobinStatus);
            this.SaveUserOrganisationStructure(uos);
        }

        public IUserOrganisationStructureRoundRobinStatus GetEmptyUserOrganisationStructureRoundRobinStatus()
        {
            return base.CreateEmpty<IUserOrganisationStructureRoundRobinStatus, UserOrganisationStructureRoundRobinStatus_DAO>();
        }

        #endregion Round Robin

        public ILegalEntityOrganisationNode GetLegalEntityOrganisationNodeForKey(int Key)
        {
            OrganisationStructure_DAO dao = OrganisationStructure_DAO.Find(Key);

            if (dao != null)
            {
                OrganisationStructureFactory factory = new OrganisationStructureFactory();
                return factory.GetLEOSNode(dao);
            }
            else
                return null;
        }

        public IList<ILegalEntityOrganisationStructure> GetLegalEntityOrganisationStructuresForLegalEntityKey(int legalEntityKey)
        {
            string HQL = "select leos from LegalEntityOrganisationStructure_DAO leos where leos.LegalEntity.Key = ?";

            SimpleQuery<LegalEntityOrganisationStructure_DAO> q = new SimpleQuery<LegalEntityOrganisationStructure_DAO>(HQL, legalEntityKey);
            LegalEntityOrganisationStructure_DAO[] list = q.Execute();

            IEventList<ILegalEntityOrganisationStructure> listM = new DAOEventList<LegalEntityOrganisationStructure_DAO, ILegalEntityOrganisationStructure, SAHL.Common.BusinessModel.LegalEntityOrganisationStructure>(list);

            IList<ILegalEntityOrganisationStructure> listLE = listM.ToList<ILegalEntityOrganisationStructure>();

            return listLE;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="roleTypeKey"></param>
        /// <returns></returns>
        public int GetLegalEntityOrganisationKeyByApplicationAndRoleType(int appKey, int roleTypeKey)
        {
            int retval = -1;
            string sql = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetLEOSKeyByAppKey");

            // Create a collection and add the required parameters
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@OfferKey", appKey));
            prms.Add(new SqlParameter("@OfferRoleTypeKey", roleTypeKey));

            object obj = castleTransactionService.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), prms);

            if (obj != null)
            {
                Int32.TryParse(obj.ToString(), out retval);
            }

            return retval;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="OrganisationStructureKey"></param>
        /// <returns></returns>
        public IEventList<ILegalEntity> GetLegalEntitiesForOrganisationStructureKey(int OrganisationStructureKey)
        {
            OrganisationStructure_DAO OS_DOA = OrganisationStructure_DAO.Find(OrganisationStructureKey);
            IList<LegalEntity_DAO> les = OS_DOA.LegalEntities;
            return new DAOEventList<LegalEntity_DAO, ILegalEntity, LegalEntity>(les);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="OrganisationStructureKey"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public IEventList<ILegalEntity> GetLegalEntitiesForOrganisationStructureKey(int OrganisationStructureKey, bool recursive)
        {
            if (!recursive)
                return GetLegalEntitiesForOrganisationStructureKey(OrganisationStructureKey);

            ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = sessionHolder.CreateSession(typeof(LegalEntity_DAO));
            string query = @"
            with os (Description, OrganisationStructureKey, ParentKey) as
	            (
	            select Description, OrganisationStructureKey, ParentKey
	            from   [2am].dbo.OrganisationStructure (nolock)
	            where  OrganisationStructureKey =  {0} and GeneralStatusKey = 1

	            union all

	            select org.Description, org.OrganisationStructureKey, org.ParentKey
	            from   [2am].dbo.OrganisationStructure org
			    inner join os on os.OrganisationStructureKey = org.ParentKey
	            where  org.OrganisationStructureKey <> {0} and org.GeneralStatusKey = 1
	            )

            select distinct	le.*
            from os
	        inner join [2am].dbo.LegalEntityOrganisationStructure los (nolock) on los.OrganisationStructureKey = os.OrganisationStructureKey
			inner join [2am].dbo.Legalentity le (nolock) on le.LegalEntityKey = los.LegalEntityKey
            order by le.LegalEntityKey";
            IQuery sqlQuery = session.CreateSQLQuery(String.Format(query, OrganisationStructureKey)).AddEntity(typeof(LegalEntity_DAO));
            IList<LegalEntity_DAO> results = sqlQuery.List<LegalEntity_DAO>();
            return new DAOEventList<LegalEntity_DAO, ILegalEntity, LegalEntity>(results);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflowRoleTypes"></param>
        /// <returns></returns>
        public DataTable GetUsersForWorkflowRoleType(IList<WorkflowRoleTypes> workflowRoleTypes)
        {
            // get a comma delimited list of workflowRoleType keys
            string workflowRoleTypeKeys = "";
            foreach (var wrt in workflowRoleTypes)
            {
                workflowRoleTypeKeys += (int)wrt + ",";
            }
            workflowRoleTypeKeys = workflowRoleTypeKeys.TrimEnd(',');

            string SQL = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "GetUsersForWorkflowRoleType");

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@WorkflowRoleTypeKeys", workflowRoleTypeKeys));

            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(SQL, typeof(GeneralStatus_DAO), prms);

            return ds.Tables[0];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mortgageLoanApplication"></param>
        /// <returns></returns>
        public string GetLifeConsultantADUserNameFromMandate(IApplication mortgageLoanApplication)
        {
            string assignToADUserName = "";

            // get the CCC - Consultant orgstructure
            int orgStructureKey = -1;

            IOrganisationStructure parentStructure = GetOrganisationStructureForDescription("CCC");
            if (parentStructure != null)
            {
                foreach (IOrganisationStructure childStructure in parentStructure.ChildOrganisationStructures)
                {
                    if (childStructure.Description == "Consultant")
                    {
                        orgStructureKey = childStructure.Key;
                        break;
                    }
                }
            }

            // get the life lead allocation mandates
            string HQL = string.Format("from AllocationMandateSetGroup_DAO d where d.OrganisationStructure.Key in ({0})", orgStructureKey.ToString());
            SimpleQuery<AllocationMandateSetGroup_DAO> query = new SimpleQuery<AllocationMandateSetGroup_DAO>(HQL);
            AllocationMandateSetGroup_DAO[] mandates = query.Execute();

            // build a string list of life lead allocation mandates
            string[] MandateList = new string[mandates.Length];
            for (int i = 0; i < MandateList.Length; i++)
            {
                MandateList[i] = mandates[i].AllocationGroupName;
            }

            // get a mandate service
            IMandateService mandateService = ServiceFactory.GetService<IMandateService>();

            ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();

            // get a list of users belonging to the mandates that qualify based on the loans application type
            Dictionary<string, DateTime> dicConsultants = new Dictionary<string, DateTime>();
            foreach (string MandateGroup in MandateList)
            {
                // check if this mandate qualifies - if it does not then no users will be returned
                IList<IADUser> usersBelongingToMandate = mandateService.ExecuteMandateSet(MandateGroup, new object[] { mortgageLoanApplication });
                if (usersBelongingToMandate.Count > 0)
                {
                    // this mandate qualifies so lets add the users to our collection
                    foreach (IADUser usr in usersBelongingToMandate)
                    {
                        if (!dicConsultants.ContainsKey(usr.ADUserName))
                        {
                            // when we add the user to our list, add the datetime of their latest LifeOfferAssignment record
                            // for this offer type so we can use it in round robin
                            ILifeOfferAssignment latestLifeOfferAssignment = lifeRepo.GetLatestLifeOfferAssignment(usr.ADUserName, mortgageLoanApplication.ApplicationType.Key);
                            dicConsultants.Add(usr.ADUserName, latestLifeOfferAssignment == null ? DateTime.MinValue : latestLifeOfferAssignment.DateAssigned);
                        }
                    }
                }
            }

            if (dicConsultants.Count > 0)
            {
                // we have a list of users now so lets do round robin to get user to assign to
                // we basically look for the person who's latest LifeOfferAssignment record is the earlist of the list
                DateTime earliestAssignedDate = DateTime.MaxValue;
                foreach (KeyValuePair<string, DateTime> userEntry in dicConsultants)
                {
                    if (userEntry.Value < earliestAssignedDate)
                    {
                        earliestAssignedDate = userEntry.Value;
                        assignToADUserName = userEntry.Key;
                    }
                }
            }

            return assignToADUserName;
        }

        /// <summary>
        /// Organisation Structure Factory
        /// </summary>
        public class OrganisationStructureFactory : IOrganisationStructureFactory
        {
            public OrganisationStructureNodeTypes OrganisationStructureNodeType { get; set; }

            /// <summary>
            /// Create Legal Entity Organisation Node
            /// </summary>
            /// <param name="orgType"></param>
            /// <param name="organisationStructureNodeType"></param>
            /// <returns></returns>
            public ILegalEntityOrganisationNode CreateLegalEntityOrganisationNode(IOrganisationType orgType, OrganisationStructureNodeTypes organisationStructureNodeType)
            {
                ILegalEntityOrganisationNode leon = null;

                OrganisationStructure_DAO osDAO = new OrganisationStructure_DAO();

                if (osDAO == null || orgType == null)
                    return null;

                if (orgType.Key == (int)OrganisationTypes.Designation)
                {
                    switch (organisationStructureNodeType)
                    {
                        case OrganisationStructureNodeTypes.EstateAgent:
                            leon = (ILegalEntityOrganisationNode)new EstateAgentDesignationOrganisationNode(osDAO);
                            break;
                        case OrganisationStructureNodeTypes.PaymentDistributionAgency:
                            leon = (ILegalEntityOrganisationNode)new PaymentDistributionAgentDesignationOrganisationNode(osDAO);
                            break;
                        case OrganisationStructureNodeTypes.DebtCounsellor:
                            leon = (ILegalEntityOrganisationNode)new DebtCounsellorDesignationOrganisationNode(osDAO);
                            break;
                        default:
                            leon = new LegalEnitityDesignationOrganisationNode(osDAO);
                            break;
                    }
                }
                else
                {
                    switch (organisationStructureNodeType)
                    {
                        case OrganisationStructureNodeTypes.EstateAgent:
                            leon = (ILegalEntityOrganisationNode)new EstateAgentOrganisationNode(osDAO);
                            break;
                        case OrganisationStructureNodeTypes.PaymentDistributionAgency:
                            leon = (ILegalEntityOrganisationNode)new PaymentDistributionAgentOrganisationNode(osDAO);
                            break;
                        case OrganisationStructureNodeTypes.DebtCounsellor:
                            leon = (ILegalEntityOrganisationNode)new DebtCounsellorOrganisationNode(osDAO);
                            break;
                        default:
                            leon = new LegalEntityOrganisationNode(osDAO);
                            break;
                    }
                }

                if (leon != null)
                    leon.OrganisationType = orgType;

                return leon;
            }

            /// <summary>
            /// Get Legal Entity Organisation Structure Node
            /// </summary>
            /// <param name="osDAO"></param>
            /// <returns></returns>
            public ILegalEntityOrganisationNode GetLEOSNode(OrganisationStructure_DAO osDAO)
            {
                if (osDAO == null || osDAO.OrganisationType == null)
                    return null;

                if (osDAO.OrganisationType.Key == (int)OrganisationTypes.Designation)
                {
                    switch (OrganisationStructureNodeType)
                    {
                        case OrganisationStructureNodeTypes.EstateAgent:
                            return (ILegalEntityOrganisationNode)new EstateAgentDesignationOrganisationNode(osDAO);
                        case OrganisationStructureNodeTypes.PaymentDistributionAgency:
                            return (ILegalEntityOrganisationNode)new PaymentDistributionAgentDesignationOrganisationNode(osDAO);
                        case OrganisationStructureNodeTypes.DebtCounsellor:
                            return (ILegalEntityOrganisationNode)new DebtCounsellorDesignationOrganisationNode(osDAO);
                        default:
                            return new LegalEnitityDesignationOrganisationNode(osDAO);
                    }
                }
                else
                {
                    switch (OrganisationStructureNodeType)
                    {
                        case OrganisationStructureNodeTypes.EstateAgent:
                            return (ILegalEntityOrganisationNode)new EstateAgentOrganisationNode(osDAO);
                        case OrganisationStructureNodeTypes.PaymentDistributionAgency:
                            return (ILegalEntityOrganisationNode)new PaymentDistributionAgentOrganisationNode(osDAO);
                        case OrganisationStructureNodeTypes.DebtCounsellor:
                            return (ILegalEntityOrganisationNode)new DebtCounsellorOrganisationNode(osDAO);
                        default:
                            return new LegalEntityOrganisationNode(osDAO);
                    }
                }
            }

            #region IDomainFactory Members

            /// <summary>
            /// Get Business Model Interface
            /// </summary>
            /// <param name="PersistanceObject"></param>
            /// <returns></returns>
            public IBusinessModelObject GetBusinessModelInterface(object PersistanceObject)
            {
                OrganisationStructure_DAO osDAO = PersistanceObject as OrganisationStructure_DAO;
                if (osDAO != null)
                {
                    return (IBusinessModelObject)GetLEOSNode(osDAO);
                }
                else
                    return null;
            }

            #endregion IDomainFactory Members
        }
    }
}