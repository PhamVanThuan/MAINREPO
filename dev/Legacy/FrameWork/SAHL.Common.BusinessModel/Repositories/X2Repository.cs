using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NHibernate;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.X2.BusinessModel;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IX2Repository))]
    public class X2Repository : AbstractRepositoryBase, IX2Repository
    {
        private ICastleTransactionsService castleTransactionsService;

        public X2Repository(ICastleTransactionsService castleTransactionsService)
        {
            this.castleTransactionsService = castleTransactionsService;
        }

        public X2Repository()
        {
            if (castleTransactionsService == null)
            {
                castleTransactionsService = new CastleTransactionsService();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="process"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public IState GetStateByName(string workflow, string process, string state)
        {
            string query = string.Format(@"SELECT TOP 1 xs.*
                        FROM x2.x2.state xs (nolock)
                        JOIN x2.x2.workflow xwf (nolock)
	                        ON xs.WorkFlowID = xwf.ID
                        JOIN x2.x2.process xp (nolock)
	                        ON xwf.ProcessID = xp.ID
                        WHERE
	                        xp.name = '{0}'
		                        AND
	                        xwf.name = '{1}'
		                        AND
	                        xs.name = '{2}'
                        ORDER BY XS.ID DESC", process, workflow, state);
            SimpleQuery<State_DAO> sq = new SimpleQuery<State_DAO>(QueryLanguage.Sql, query);
            sq.AddSqlReturnDefinition(typeof(State_DAO), "xs");
            State_DAO[] res = sq.Execute();

            if (res != null && res.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IState, State_DAO>(res[0]);
            }

            return null;
        }

        public IState GetStateByKey(int StateID)
        {
            IState state = new State(State_DAO.Find(StateID));
            return state;
        }

        /// <summary>
        /// Implements <see cref="ILegalEntityRepository.GetLegalEntityByKey"></see>.
        /// </summary>
        /// <param name="InstanceID"></param>
        /// <returns></returns>
        public IInstance GetInstanceByKey(long InstanceID)
        {
            IInstance I = new Instance(Instance_DAO.Find(InstanceID));
            return I;
        }

        public IEventList<IInstance> GetInstanceByPrincipal(SAHLPrincipal principal)
        {
            IEventList<IInstance> list = Instance.FindByPrincipal(principal);

            return list;
        }

        public IWorkFlow GetWorkFlowByKey(int WorkFlowID)
        {
            IWorkFlow WF = new WorkFlow(WorkFlow_DAO.Find(WorkFlowID));
            return WF;
        }

        public IWorkFlow GetWorkFlowByName(string WorkFlowName, string ProcessName)
        {
            string query = string.Format("select w from WorkFlow_DAO w where w.Process.Name = ? and w.Name = ? order by w.ID desc");
            SimpleQuery<WorkFlow_DAO> SQ = new SimpleQuery<WorkFlow_DAO>(query, ProcessName, WorkFlowName);
            SQ.SetQueryRange(1);
            WorkFlow_DAO[] arr = SQ.Execute();
            IWorkFlow iwf = null;

            if (arr != null && arr.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                iwf = bmtm.GetMappedType<IWorkFlow, WorkFlow_DAO>(arr[0]);
            }
            return iwf;
        }

        public IEventList<IInstance> GetInstanceForSourceInstanceID(Int64 SourceInstanceID)
        {
            string HQL = "from Instance_DAO d where d.SourceInstanceID = ?";
            SimpleQuery<Instance_DAO> q = new SimpleQuery<Instance_DAO>(HQL, SourceInstanceID);
            Instance_DAO[] arr = q.Execute();
            return new DAOEventList<Instance_DAO, IInstance, Instance>(arr);
        }

        /// <summary>
        /// Implements <see cref="IX2Repository.GetWorkListByState"></see>.
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="StateID"></param>
        /// <returns></returns>
        public IEventList<IWorkList> GetWorkListByState(SAHLPrincipal principal, long StateID)
        {
            return WorkList.FindByUserAndState(principal, StateID);
        }

        public IEventList<IWatchListConfiguration> GetWatchListConfigurationByWorkFlowName(string WorkFlowName)
        {
            WatchListConfiguration_DAO[] arr = WatchListConfiguration_DAO.FindAllByProperty("WorkFlowName", WorkFlowName);
            IList<WatchListConfiguration_DAO> list = new List<WatchListConfiguration_DAO>(arr);
            return new DAOEventList<WatchListConfiguration_DAO, IWatchListConfiguration, WatchListConfiguration>(list);
        }

        public IWatchListConfiguration GetWatchListConfiguration(string ProcessName, string WorkFlowName)
        {
            WatchListConfiguration_DAO[] arr = WatchListConfiguration_DAO.FindAllByProperty("WorkFlowName", WorkFlowName);
            IWatchListConfiguration iwlc = null;

            for (int i = 0; i < arr.Length; i++)
            {
                if (String.Compare(arr[i].ProcessName, ProcessName, true) == 0)
                {
                    iwlc = new WatchListConfiguration(arr[i]);
                    break;
                }
            }

            return iwlc;
        }

        public IEventList<IDataGridConfiguration> GetDataGridConfigurationByStatementName(string StatementName)
        {
            DataGridConfiguration_DAO[] arr = DataGridConfiguration_DAO.FindAllByProperty("Sequence", "StatementName", StatementName);

            IList<DataGridConfiguration_DAO> list = new List<DataGridConfiguration_DAO>(arr);
            return new DAOEventList<DataGridConfiguration_DAO, IDataGridConfiguration, DataGridConfiguration>(list);
        }

        public IEventList<IDataGridConfiguration> GetDataGridConfigurationByWorkFlowName(string WorkFlowName, string ProcessName)
        {
            IWorkFlow iwf = GetWorkFlowByName(WorkFlowName, ProcessName);
            IWatchListConfiguration iwlc = GetWatchListConfiguration(iwf.Process.Name, iwf.Name);
            IEventList<IDataGridConfiguration> configList = GetDataGridConfigurationByStatementName(iwlc.StatementName);

            return configList;
        }

        public int GetLatestExternalActivityIDFromWorkFlow(string WorkFlowName, string ExternalName)
        {
            int id = -1;

            string query = UIStatementRepository.GetStatement("X2", "ExternalActivityIDLatestGetGivenWorkFlow");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            //Add the required parameters
            parameters.Add(new SqlParameter("@WorkFlowname", WorkFlowName));
            parameters.Add(new SqlParameter("@ExternalName", ExternalName));

            // execute
            object o = castleTransactionsService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            id = o == null ? -1 : Convert.ToInt32(o);

            return id;
        }

        public IActiveExternalActivity GetEmptyActiveExternalActivity()
        {
            return base.CreateEmpty<IActiveExternalActivity, ActiveExternalActivity_DAO>();
        }

        public IExternalActivity GetExternalActivityByKey(int key)
        {
            return base.GetByKey<IExternalActivity, ExternalActivity_DAO>(key);
        }

        public IUIStatement GetUIStatement(string StatementName, string ApplicationName)
        {
            string query = String.Format("SELECT ui FROM UIStatement_DAO ui WHERE ui.StatementName = '{0}' AND ui.ApplicationName = '{1}' ORDER BY ui.Version desc", StatementName, ApplicationName);

            SimpleQuery q = new SimpleQuery(typeof(UIStatement_DAO), query);
            UIStatement_DAO[] result = UIStatement_DAO.ExecuteQuery(q) as UIStatement_DAO[];

            if (result != null && result.Length > 0)
                return new UIStatement(result[0]);

            return null;
        }

        public IEventList<IUIStatement> GetAllUIStatement()
        {
            SimpleQuery<UIStatement_DAO> query = new SimpleQuery<UIStatement_DAO>(QueryLanguage.Sql,
            @"select ui.* from uistatement ui (nolock) inner join (select max(version) as MaxVer, statementkey from uistatement (nolock) group by statementkey)  a on ui.statementkey = a.statementkey");
            query.AddSqlReturnDefinition(typeof(UIStatement_DAO), "ui");
            UIStatement_DAO[] dao = query.Execute();
            List<UIStatement_DAO> arr = new List<UIStatement_DAO>();
            for (int i = 0; i < dao.Length; i++)
            {
                arr.Add(dao[i]);
            }
            return new DAOEventList<UIStatement_DAO, IUIStatement, UIStatement>(arr);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IUIStatement GetUIStatment(int Key)
        {
            UIStatement_DAO dao = UIStatement_DAO.Find(Key);
            return new UIStatement(dao);
        }

        /// <summary>
        /// Creates an empty IActiveExternalActivity
        /// </summary>
        /// <returns></returns>
        public IActiveExternalActivity CreateActiveExternalActivity()
        {
            return base.CreateEmpty<IActiveExternalActivity, ActiveExternalActivity_DAO>();

            //return new ActiveExternalActivity(new ActiveExternalActivity_DAO());
        }

        /// <summary>
        /// saves an IActiveExternalActivity that will be picked up and executed by the engine.
        /// </summary>
        /// <param name="activity"></param>
        public void SaveActiveExternalActivity(IActiveExternalActivity activity)
        {
            base.Save<IActiveExternalActivity, ActiveExternalActivity_DAO>(activity);

            //IDAOObject dao = activity as IDAOObject;
            //ActiveExternalActivity_DAO o = (ActiveExternalActivity_DAO)dao.GetDAOObject();
            //o.SaveAndFlush();
            // - Paul C Commented on purpose. NO rules should run when you save an active EXT activity.
            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ExternalActivityName"></param>
        /// <param name="WorkflowID"></param>
        /// <returns></returns>
        public IExternalActivity GetExternalActivityByName(string ExternalActivityName, int WorkflowID)
        {
            string HQL = string.Format("from ExternalActivity_DAO o where o.Name = ? and WorkflowID = ?");
            SimpleQuery<ExternalActivity_DAO> q = new SimpleQuery<ExternalActivity_DAO>(HQL, ExternalActivityName, WorkflowID);
            ExternalActivity_DAO[] res = q.Execute();
            if (null != res)
                return new ExternalActivity(res[0]);
            return null;
        }

        public IInstance GetInstanceForGenericKey(int GenericKey, string WorkflowName, string ProcessName)
        {
            IWorkFlow workFlow = GetWorkFlowByName(WorkflowName, ProcessName);

            if (workFlow == null)
                return null;

            string query = string.Format("SELECT top 10 InstanceID FROM X2.X2DATA.{0} WHERE {1}={2} order by InstanceID", workFlow.StorageTable, workFlow.StorageKey, GenericKey);
            DataSet dsResults = this.castleTransactionsService.ExecuteQueryOnCastleTran(query, typeof(Instance_DAO), null);
            if (dsResults != null && dsResults.Tables.Count > 0)
            {
                DataTable dtResults = dsResults.Tables[0];

                if (dtResults.Rows.Count > 0)
                {
                    Int64 IID = Convert.ToInt64(dtResults.Rows[0][0]);
                    return this.GetInstanceByKey(IID);
                }
            }
            return null;
        }

        public IInstance GetLatestInstanceForGenericKey(int GenericKey, string WorkflowName, string ProcessName)
        {
            IWorkFlow workFlow = GetWorkFlowByName(WorkflowName, ProcessName);

            if (workFlow == null)
                return null;

            string query = string.Format("SELECT top 1 i.ID FROM X2.X2DATA.{0} x join X2.X2.Instance i on i.ID = x.InstanceID and i.ParentInstanceID is null WHERE {1}={2} order by i.ID desc", workFlow.StorageTable, workFlow.StorageKey, GenericKey);
            DataSet dsResults = this.castleTransactionsService.ExecuteQueryOnCastleTran(query, typeof(Instance_DAO), null);
            if (dsResults != null && dsResults.Tables.Count > 0)
            {
                DataTable dtResults = dsResults.Tables[0];

                if (dtResults.Rows.Count > 0)
                {
                    Int64 IID = Convert.ToInt64(dtResults.Rows[0][0]);
                    return this.GetInstanceByKey(IID);
                }
            }
            return null;
        }

        public IList<IInstance> GetInstancesForGenericKey(int GenericKey, string ProcessName)
        {
            // we need to find all the instances that the application has
            string query = UIStatementRepository.GetStatement("Repositories.X2Repository", "GetInstancesForGenericKey");
            SimpleQuery<Instance_DAO> IQ = new SimpleQuery<Instance_DAO>(QueryLanguage.Sql, query.ToString(), ProcessName, GenericKey);
            IQ.AddSqlReturnDefinition(typeof(Instance_DAO), "I");
            Instance_DAO[] Is = IQ.Execute();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IInstance>(Is);
        }

        public IActivity GetActivityForName(string Name)
        {
            string HQL = "from Activity_DAO o where o.Name = ? order by o.id desc";
            SimpleQuery<Activity_DAO> aquery = new SimpleQuery<Activity_DAO>(HQL, Name);
            Activity_DAO[] activities = aquery.Execute();

            // want the newest one
            if (activities.Length > 0)
            {
                return new Activity(activities[0]);
            }
            else
            {
                return null;
            }
        }

        public IEventList<IActivity> GetActivitiesForName(string Name)
        {
            Activity_DAO[] dao = Activity_DAO.FindAllByProperty("Name", Name);
            return new DAOEventList<Activity_DAO, IActivity, Activity>(dao);
        }

        public IWorkFlowHistory GetHistoryForInstanceAndActivity(IInstance ID, IActivity activity)
        {
            // get activity name ... get all the id's that this activity has had in the past (each time we republish the id's get updated)
            // however the workflowhistory table ISNT updated with new ID's .. this is cause id's can be removed, renamed etc

            string HQL = "from Activity_DAO o where o.Name = ? order by o.id desc";
            SimpleQuery<Activity_DAO> aquery = new SimpleQuery<Activity_DAO>(HQL, activity.Name);
            Activity_DAO[] activities = aquery.Execute();

            // loop throgh the workflowhistory table looking for a match
            foreach (Activity_DAO a in activities)
            {
                HQL = "from WorkFlowHistory_DAO o where o.Activity.ID=? and o.InstanceID=? order by o.id desc";
                SimpleQuery<WorkFlowHistory_DAO> query = new SimpleQuery<WorkFlowHistory_DAO>(HQL, a.ID, ID.ID);
                query.SetQueryRange(10);
                WorkFlowHistory_DAO[] arr = query.Execute();
                if (arr.Length > 0)
                    return new WorkFlowHistory(arr[0]);
            }
            return null;
        }

        public IList<IInstance> SuperSearchWorkflow(IWorkflowSearchCriteria searchCriteria)
        {
			var searchSetups = GetSearchSetups();
            var searchCleanups = GetSearchCleanups();
            var searchSelects = GetSearchSelects();
            var searchContexts = GetSearchContexts();
            var preContextFilters = GetPreContextFilters();
            var preWorkflowDataFilters = GetPreWorkflowDataFilter();
            var searchFilters = GetSearchFilters();
            var searchWorkflowContexts = GetSearchWorkflowContexts();
            var searchWorkflowDatas = GetSearchWorkflowDatas();
            var searchInternalRoles = GetSearchInternalRoles();

            var contextFilters = new List<string>();
            var workflowContextFilters = new List<string>();

            var sqlBuilder = new StringBuilder();

            //Setup Scripts
            foreach (var setupScript in searchSetups)
            {
                sqlBuilder.AppendLine(setupScript.Query);
            }

            //Let's insert the filter parameters
            if (searchCriteria.ApplicationTypes != null && searchCriteria.ApplicationTypes.Count > 0)
            {
                contextFilters.Add("OfferTypes");
                foreach (var offerTypeKey in searchCriteria.ApplicationTypes)
                {
                    sqlBuilder.AppendFormat("insert into #offertypes values({0});", (int)offerTypeKey);
                }

                //Context Filters
                var filters = from preContextFilter in preContextFilters
                              where searchCriteria.ApplicationTypes.Contains((OfferTypes)preContextFilter.OfferTypeKey)
                              select preContextFilter;

                searchContexts = (IList<IContext>)(from searchContext in searchContexts
                                                   where filters.Where(x => x.ContextKey == searchContext.Key).Count() > 0
                                                   select searchContext).ToList();

                //x2
                var preWorkflowFilters = from workflowDataFilter in preWorkflowDataFilters
                                         where searchCriteria.ApplicationTypes.Contains((OfferTypes)workflowDataFilter.OfferTypeKey)
                                         select workflowDataFilter;

                searchWorkflowDatas = (IList<IWorkflowData>)(from workflowData in searchWorkflowDatas
                                                             where preWorkflowFilters.Where(x => x.WorkflowDataKey == workflowData.Key).Count() > 0
                                                             select workflowData).ToList();
            }

            if (!String.IsNullOrEmpty(searchCriteria.ApplicationNumber))
            {
                contextFilters.Add("ReferenceKey");
                sqlBuilder.AppendLine("declare @ReferenceKey varchar(64)");
                sqlBuilder.AppendFormat("set @ReferenceKey = {0} \n", searchCriteria.ApplicationNumber);
                sqlBuilder.AppendFormat("insert into #offers values({0});", searchCriteria.ApplicationNumber);
            }

            if (!String.IsNullOrEmpty(searchCriteria.Firstname) || !String.IsNullOrEmpty(searchCriteria.Surname) || !String.IsNullOrEmpty(searchCriteria.IDNumber))
            {
                contextFilters.Add("LegalEntityCredentials");
            }

            if (!String.IsNullOrEmpty(searchCriteria.Firstname))
            {
                contextFilters.Add("FirstName");
                sqlBuilder.AppendLine("declare @FirstName varchar(64)");
                sqlBuilder.AppendFormat("set @FirstName = '{0}' \n", searchCriteria.Firstname);
            }

            if (!String.IsNullOrEmpty(searchCriteria.Surname))
            {
                contextFilters.Add("Surname");
                sqlBuilder.AppendLine("declare @Surname varchar(64)");
                sqlBuilder.AppendFormat("set @Surname = '{0}' \n", searchCriteria.Surname);
            }

            if (!String.IsNullOrEmpty(searchCriteria.IDNumber))
            {
                contextFilters.Add("IDNumber");
                sqlBuilder.AppendLine("declare @IDNumber varchar(64)");
                sqlBuilder.AppendFormat("set @IDNumber = '{0}' \n", searchCriteria.IDNumber);
            }

            foreach (var userFilter in searchCriteria.UserFilter)
            {
                contextFilters.Add("Users");
                sqlBuilder.AppendFormat("insert into #users values('{0}')\n", userFilter);
            }

            if (searchCriteria.WorkflowFilter != null && searchCriteria.WorkflowFilter.Count > 0)
            {
                workflowContextFilters.Add("WorkflowStates");
                foreach (var workflowFilter in searchCriteria.WorkflowFilter)
                {
                    foreach (var workflowFilterState in workflowFilter.States)
                    {
                        sqlBuilder.AppendFormat("insert into #workflowStates values({0}, {1})\n", workflowFilter.WorkflowID, workflowFilterState);
                    }
                }
            }

            //Get the Offers
            foreach (var contextTable in searchContexts)
            {
                sqlBuilder.AppendLine(Context.BuildQuery(searchFilters, searchInternalRoles, contextTable, contextFilters.ToArray()));
            }

            //Get the Workflow Data
            if (!searchCriteria.IncludeHistoricUsers)
            {
                workflowContextFilters.Add("Worklist");
            }
            foreach (var workflowContext in searchWorkflowContexts)
            {
                sqlBuilder.AppendLine(WorkflowContext.BuildQuery(searchFilters, searchWorkflowDatas, workflowContext, workflowContextFilters.ToArray()));
            }

            //Get the Selects
            foreach (var selectScript in searchSelects)
            {
                sqlBuilder.AppendFormat(selectScript.Query, searchCriteria.MaxResults);
            }

            //Cleanup Scripts
            foreach (var cleanupScript in searchCleanups)
            {
                sqlBuilder.AppendLine(cleanupScript.Query);
            }

            SimpleQuery<Instance_DAO> queryResults2 = new SimpleQuery<Instance_DAO>(QueryLanguage.Sql, sqlBuilder.ToString());
            queryResults2.AddSqlReturnDefinition(typeof(Instance_DAO), "I");
            Instance_DAO[] instances2 = queryResults2.Execute();
            Instance_DAO[] iinstances2 = new Instance_DAO[instances2.Length];
            for (int j = instances2.Length - 1, p = 0; j >= 0; p++, j--)
                iinstances2[p] = instances2[j];

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IInstance>(iinstances2);
        }

        public Dictionary<IInstance, IApplication> WorkflowSearch(IWorkflowSearchCriteria SearchCriteria, Hashtable AdditionalCriteria)
        {
            // if the is a CAP2 search we need to a different search
            SearchCriteria.Cap2Search = false;
            if (SearchCriteria.ApplicationTypes != null && SearchCriteria.ApplicationTypes.Count == 1 && String.Compare(SearchCriteria.ApplicationTypes[0].ToString(), "cap2", true) == 0)
                SearchCriteria.Cap2Search = true;

            IList<IInstance> instanceList = SuperSearchWorkflow(SearchCriteria);
            Dictionary<IInstance, IApplication> filteredList = new Dictionary<IInstance, IApplication>();
            List<SortedInstance> instList = new List<SortedInstance>();
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            foreach (IInstance instance in instanceList)
            {
                IApplication _app = appRepo.GetApplicationByInstanceAndAddCriteria(instance, AdditionalCriteria);

                if (_app != null)
                    instList.Add(new SortedInstance(instance, _app));
            }

            instList.Sort(
            delegate(SortedInstance s1, SortedInstance s2)
            {
                return s1.Application.Key.CompareTo(s2.Application.Key);
            });

            foreach (SortedInstance _sortedInst in instList)
            {
                filteredList.Add(_sortedInst.Instance, _sortedInst.Application);
            }

            return filteredList;
        }

        public IEventList<IWorkFlow> GetSearchableWorkflowsForUser(SAHLPrincipal principal)
        {
            // get a list of worflows this user has access to via a recursive query
            SimpleQuery<WorkFlow_DAO> WorkflowDAOQuery = new SimpleQuery<WorkFlow_DAO>(QueryLanguage.Sql,
                @"with OSTopLevels (OrganisationStructureKey, ParentKey)
                    as
                    (
	                    select OS.OrganisationStructureKey, OS.ParentKey
	                    from
		                    [2am].dbo.OrganisationStructure OS (nolock)
	                    inner join
		                    [2am].dbo.UserOrganisationStructure UOS (nolock)
	                    on
		                    OS.OrganisationStructureKey = UOS.OrganisationStructureKey
	                    inner join
		                    [2am].dbo.ADUser A (nolock)
	                    on
		                    A.ADUserKey = UOS.ADUserKey
	                    where
		                    A.ADUserName = ?
                    UNION ALL
	                    select OS.OrganisationStructureKey, OS.ParentKey
	                    from
		                    [2am].dbo.OrganisationStructure OS (nolock)
	                    join
		                    OSTopLevels
	                    on
		                    OS.OrganisationStructureKey = OSTopLevels.ParentKey
                    )
                    select w.* from OSTopLevels OTL inner join [2am].dbo.workfloworganisationstructure wos (nolock) on OTL.organisationstructurekey = wos.organisationstructurekey
                    inner join x2.x2.process p (nolock)
                    on p.name = wos.processname
                    inner join
                    (
	                    select max(id) as ID from x2.x2.process p (nolock) group by name
                    )
                    p2
                    on
	                    p2.id = p.id
                    inner join
	                    x2.x2.workflow w (nolock)
                    on
	                    w.processid = p.id
                    inner join
                    (
	                    select max(id) as ID from x2.x2.workflow (nolock) group by name
                    )
                    w2
                    on w2.id = w.id

                    where w.name = wos.workflowname",
                    principal.Identity.Name);
            WorkflowDAOQuery.AddSqlReturnDefinition(typeof(WorkFlow_DAO), "w");

            WorkFlow_DAO[] WorkflowDAOs = WorkflowDAOQuery.Execute();
            return new DAOEventList<WorkFlow_DAO, IWorkFlow, WorkFlow>(new List<WorkFlow_DAO>(WorkflowDAOs));
        }

        public void UpdateInstanceSubject(int applicationKey, string instanceSubject)
        {
            // we need to find all the instances that the application has
            IList<IInstance> instances = new List<IInstance>();

            // Origination
            IList<IInstance> instancesOrig = GetInstancesForGenericKey(applicationKey, SAHL.Common.Constants.WorkFlowProcessName.Origination);
            foreach (IInstance i in instancesOrig)
            {
                UpdateInstanceSubject(i, instanceSubject);
            }

            // Life
            IList<IInstance> instancesLife = GetInstancesForGenericKey(applicationKey, SAHL.Common.Constants.WorkFlowProcessName.LifeOrigination);
            foreach (IInstance i in instancesLife)
            {
                UpdateInstanceSubject(i, instanceSubject);
            }

            // CAP2
            IList<IInstance> instancesCap = GetInstancesForGenericKey(applicationKey, SAHL.Common.Constants.WorkFlowProcessName.Cap2Offers);
            foreach (IInstance i in instancesCap)
            {
                UpdateInstanceSubject(i, instanceSubject);
            }
        }

        public void UpdateInstanceSubject(IInstance instance, string instanceSubject)
        {
            // check the length of the subject - if its longer than the column max then truncate
            int maxLength = 128;
            if (instanceSubject.Length > maxLength)
                instanceSubject = instanceSubject.Substring(0, maxLength);

            instance.Subject = instanceSubject;

            IDAOObject dao = instance as IDAOObject;
            Instance_DAO instance_DAO = (Instance_DAO)dao.GetDAOObject();
            instance_DAO.SaveAndFlush();
        }

        public IEventList<IWorkFlowHistory> GetWorkflowHistoryForInstance(IInstance instance)
        {
            return GetWorkflowHistoryForInstanceAndActivity(instance, null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="activityName"></param>
        /// <returns></returns>
        public IEventList<IWorkFlowHistory> GetWorkflowHistoryForInstanceAndActivity(long instanceID, string activityName)
        {
            string HQL = "";
            SimpleQuery<WorkFlowHistory_DAO> aquery = null;
            if (!string.IsNullOrEmpty(activityName))
            {
                HQL = "from WorkFlowHistory_DAO o where o.InstanceID = ? and o.Activity.Name = ? order by o.id desc";
                aquery = new SimpleQuery<WorkFlowHistory_DAO>(HQL, instanceID, activityName);
            }
            else
            {
                HQL = "from WorkFlowHistory_DAO o where o.InstanceID = ? order by o.id desc";
                aquery = new SimpleQuery<WorkFlowHistory_DAO>(HQL, instanceID);
            }

            WorkFlowHistory_DAO[] hist = aquery.Execute();
            return new DAOEventList<WorkFlowHistory_DAO, IWorkFlowHistory, WorkFlowHistory>(hist);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        public IEventList<IWorkFlowHistory> GetWorkflowHistoryForInstanceAndActivity(IInstance instance, IActivity activity)
        {
            string HQL = "";
            SimpleQuery<WorkFlowHistory_DAO> aquery = null;
            if (null != activity)
            {
                HQL = "from WorkFlowHistory_DAO o where o.InstanceID = ? and o.Activity.ID = ? order by o.id desc";
                aquery = new SimpleQuery<WorkFlowHistory_DAO>(HQL, instance.ID, activity.ID);
            }
            else
            {
                HQL = "from WorkFlowHistory_DAO o where o.InstanceID = ? order by o.id desc";
                aquery = new SimpleQuery<WorkFlowHistory_DAO>(HQL, instance.ID);
            }

            WorkFlowHistory_DAO[] hist = aquery.Execute();
            return new DAOEventList<WorkFlowHistory_DAO, IWorkFlowHistory, WorkFlowHistory>(hist);
        }

        public string GetUserWhoPerformedActivity(Int64 InstanceID, string ActivityName)
        {
            string HQL = "from WorkFlowHistory_DAO wfh where wfh.Activity.Name=? and wfh.InstanceID=? order by wfh.ID desc";
            SimpleQuery<WorkFlowHistory_DAO> query = new SimpleQuery<WorkFlowHistory_DAO>(HQL, ActivityName, InstanceID);
            WorkFlowHistory_DAO[] arr = query.Execute();
            if (null != arr && arr.Length > 0)
                return new WorkFlowHistory(arr[0]).ADUserName;
            return "";
        }

        public IEventList<IInstance> GetChildInstances(Int64 ParentInstanceID)
        {
            string HQL = "";
            SimpleQuery<Instance_DAO> aquery = null;
            HQL = "from Instance_DAO o where o.ParentInstance.ID = ?";
            aquery = new SimpleQuery<Instance_DAO>(HQL, ParentInstanceID);
            Instance_DAO[] arr = aquery.Execute();
            return new DAOEventList<Instance_DAO, IInstance, Instance>(arr);
        }

        //public void NavigateToWorkFlowTaskListNode(long InstanceID, SAHL.Common.Service.Interfaces.ICBOService CBOService)
        //{
        //SAHLPrincipalCache spc = SAHLPrincipal.GetSAHLPrincipalCache();
        //IDomainMessageCollection Messages = spc.DomainMessages;

        //IX2Repository _repo = RepositoryFactory.GetRepository<IX2Repository>();
        //SAHL.Common.X2.BusinessModel.Interfaces.IInstance instance = _repo.GetInstanceByKey(InstanceID);
        //SAHL.Common.Collections.Interfaces.List<CBONode> nodes = CBOService.GetMenuNodes(Messages, _view.CurrentPrincipal, CBONodeSet.X2NODESET);

        //TaskListNode taskListNode = null;

        //for (int i = 0; i < nodes.Count; i++)
        //{
        //    if (nodes[i] is TaskListNode)
        //    {
        //        taskListNode = nodes[i] as TaskListNode;
        //        break;
        //    }
        //}

        //if (taskListNode == null)
        //{
        //    taskListNode = new TaskListNode(null);
        //    CBOService.AddCBOMenuNode(_view.Messages, _view.CurrentPrincipal, null, taskListNode, CBONodeSet.X2NODESET);
        //}

        ////the name of the relevant x2data table will be the same as the storagetable field from the workflow table
        ////the storagekey field in the workflow table is the name of the column in the x2data table that has the actual key
        ////forms for a stage are ordered by the formorder column in the stateform table
        //IWorkFlow wf = _repo.GetWorkFlowByKey(instance.WorkFlow.ID);
        //IDictionary<string, object> dict = X2Service.GetX2DataRow(instanceID);
        //int businessKey = Convert.ToInt32(dict[wf.StorageKey]);

        //// setup the instance node description
        //string nodeDesc = "", longDesc = "";
        //switch (wf.Name)
        //{
        //    case SAHL.Common.Constants.WorkFlowName.HelpDesk:
        //        nodeDesc = instance.Name;
        //        longDesc = String.Format("{0} ({1}: {2})", instance.Subject, wf.StorageKey, businessKey);
        //        break;
        //    case SAHL.Common.Constants.WorkFlowName.LifeOrigination:
        //        //nodeDesc = "Life Origination: " + instance.Name;
        //        nodeDesc = "Policy: " + instance.Name + " (" + wf.Name + ")";
        //        longDesc = String.Format("{0} ({1}: {2})", instance.Subject, wf.StorageKey, businessKey);
        //        break;
        //    default:
        //        IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
        //        IApplication application = appRepo.GetApplicationFromInstance(instance);
        //        nodeDesc = "Loan: " + application.ReservedAccount.Key + " (" + wf.Name + ")";
        //        longDesc = String.Format("{0} ({1}: {2})", instance.Subject, "Stage", instance.State.Name);
        //        break;
        //}

        //InstanceNode iNode = new InstanceNode(businessKey, taskListNode, nodeDesc, longDesc, instance.ID, instance.State.Forms[0].Name);

        //CBOService.AddCBOMenuNode(_view.Messages, _view.CurrentPrincipal, taskListNode, iNode, CBONodeSet.X2NODESET);
        //CBOService.SetCurrentNodeSet(_view.CurrentPrincipal, CBONodeSet.X2NODESET);
        //CBOService.SetCurrentCBONode(_view.CurrentPrincipal, iNode, CBONodeSet.X2NODESET);

        //return iNode.URL; //_view.Navigator.Navigate(iNode.URL);
        //}

        /// <summary>
        /// This method checks if an instance can be assigned to a user.
        /// </summary>
        /// <param name="Instance">The instance that might be assigned</param>
        /// <param name="ADUser">The ADUser that might get the instance assigned.</param>
        /// <returns>True if the Instance can be assigned.</returns>
        public bool CanInstanceBeAssignedToUser(IInstance Instance, IADUser ADUser)
        {
            string Query = @"
                        select
	                        count(*) cnt
                        from
	                        x2.Instance i (nolock)
                        join
	                        x2.state st (nolock)
                        on
	                        st.id = i.stateId
                        join
	                        x2.stateworklist swl (nolock)
                        on
	                        swl.stateid = st.id
                        join
	                        x2.securitygroup sg  (nolock)
                        on
	                        sg.id = swl.securitygroupid
                        and
	                        sg.IsDynamic = 1
                        join
	                        [2AM]..OfferRoleType ort (nolock)
                        on
	                        ort.Description = sg.Name
                        join
	                        [2AM].dbo.OfferRoleTypeOrganisationStructureMapping ortosm (nolock)
                        on
	                        ortosm.OfferRoletypeKey = ort.OfferRoleTypekey
                        join
	                        [2AM]..OrganisationStructure os (nolock)
                        on
	                        os.OrganisationStructurekey = ortosm.OrganisationStructurekey
                        join
	                        [2AM]..UserOrganisationStructure uos (nolock)
                        on
	                        uos.OrganisationStructurekey = os.OrganisationStructurekey
                        where
	                        i.id = {0}
                        and
	                        uos.ADUserKey = {1};
                        ";

            ICommonRepository CommonRep = RepositoryFactory.GetRepository<ICommonRepository>();

            ISession Session = CommonRep.GetNHibernateSession(Instance);

            ISQLQuery Q = Session.CreateSQLQuery(String.Format(Query, Instance.ID, ADUser.Key));
            Q.AddScalar("cnt", NHibernateUtil.Int32);
            IList result = Q.List();

            //ScalarQuery<int> SQ = new ScalarQuery<int>(typeof(Instance_DAO), QueryLanguage.Sql, Query); //, new object[]());
            //SQ.AddSqlReturnDefinition(typeof(Int32), "cnt");
            //int myres = SQ.Execute();

            if ((int)result[0] > 0)
                return true;
            else
                return false;
        }

        public IList<IInstance> WorkflowArchiveSuperSearch(IWorkflowSearchCriteria SearchCriteria)
        {
            List<string> ApplicationDataTables = new List<string>(new string[] {
                    SAHL.Common.Constants.WorkFlowDataTables.Origination,
                    SAHL.Common.Constants.WorkFlowDataTables.ApplicationCapture,
                    SAHL.Common.Constants.WorkFlowDataTables.ReadvancePayments});

            StringBuilder SQL = new StringBuilder();

            SQL.AppendLine("create table #WORKFLOWSTATE");
            SQL.AppendLine("(");
            SQL.AppendLine("[WorkflowId] [int] NOT NULL,");
            SQL.AppendLine("[StateId] [int] NOT NULL");
            SQL.AppendLine(")");

            // do the inserts
            // insert into #WORKFLOWSTATE values(28,417)
            for (int i = 0; i < SearchCriteria.WorkflowFilter.Count; i++)
            {
                for (int k = 0; k < SearchCriteria.WorkflowFilter[i].States.Count; k++)
                {
                    SQL.AppendFormat("insert into #WORKFLOWSTATE values({0}, {1})\n", SearchCriteria.WorkflowFilter[i].WorkflowID, SearchCriteria.WorkflowFilter[i].States[k]);
                }
            }

            SQL.AppendLine("CREATE TABLE #INSTANCE");
            SQL.AppendLine("(");
            SQL.AppendLine("[ID] [bigint] NOT NULL,");
            SQL.AppendLine("[WorkFlowID] [int] NOT NULL,");
            SQL.AppendLine("[ParentInstanceID] [bigint] NULL,");
            SQL.AppendLine("[Name] [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,");
            SQL.AppendLine("[Subject] [varchar](800) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,");
            SQL.AppendLine("[WorkFlowProvider] [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,");
            SQL.AppendLine("[StateID] [int] NULL,");
            SQL.AppendLine("[CreatorADUserName] [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,");
            SQL.AppendLine("[CreationDate] [datetime] NOT NULL,");
            SQL.AppendLine("[StateChangeDate] [datetime] NULL,");
            SQL.AppendLine("[DeadlineDate] [datetime] NULL,");
            SQL.AppendLine("[ActivityDate] [datetime] NULL,");
            SQL.AppendLine("[ActivityADUserName] [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,");
            SQL.AppendLine("[ActivityID] [int] NULL,");
            SQL.AppendLine("[Priority] [int] NULL,");
            SQL.AppendLine("[SourceInstanceID] [bigint] NULL,");
            SQL.AppendLine("[ReturnActivityID] [int] NULL,");
            SQL.AppendLine("[RANKING] [int] NULL");
            SQL.AppendLine(") \n");

            SQL.Append(@"
                create table #OFFERS
                (
                    [OfferKey] int not null
                );");

            // Populate the #Offers table, given Search Criteria.
            if (!String.IsNullOrEmpty(SearchCriteria.ApplicationNumber))
            {
                SQL.AppendFormat("insert into #OFFERS values ({0});", SearchCriteria.ApplicationNumber);
            }

            if ((!String.IsNullOrEmpty(SearchCriteria.IDNumber)) || (!String.IsNullOrEmpty(SearchCriteria.Firstname)) || (!String.IsNullOrEmpty(SearchCriteria.Surname)))
            {
                SQL.Append(@"
                    insert into #OFFERS
                    select
	                    distinct O.OfferKey
                    from
	                    [2AM]..Offer O (nolock)
                    join
                        [2AM]..OfferRole ClientRole (nolock)
	                    join
		                    [2AM]..OfferRoleType ORTClient (nolock)
		                    on
			                    ClientRole.OfferRoleTypeKey = ORTClient.OfferRoleTypeKey
		                    and
			                    ORTClient.OfferRoleTypeGroupKey = 3
                    on
	                    ClientRole.OfferKey = O.OfferKey
                    join
	                    [2AM]..LegalEntity le (nolock)
                    on
	                    le.LegalEntityKey = ClientRole.LegalEntityKey
                    ");
                if (!String.IsNullOrEmpty(SearchCriteria.IDNumber))
                {
                    SQL.AppendFormat(@"
                            and le.IDNumber like ('%{0}%') ", SearchCriteria.IDNumber);
                }
                if (!String.IsNullOrEmpty(SearchCriteria.Firstname))
                {
                    SQL.AppendFormat(@"
                            and le.FirstNames like ('%{0}%') ", SearchCriteria.Firstname);
                }
                if (!String.IsNullOrEmpty(SearchCriteria.Surname))
                {
                    SQL.AppendFormat(@"
                            and le.Surname like ('%{0}%') ", SearchCriteria.Surname);
                }

                SQL.AppendLine(@"
						where O.OfferStatusKey <> 1 ");

                if (!String.IsNullOrEmpty(SearchCriteria.ApplicationNumber))
                {
                    SQL.AppendFormat(@"
							and O.OfferKey = {0} ", SearchCriteria.ApplicationNumber);
                }
            }

            // add data to #Instance table for every applicable data table.
            for (int i = 0; i < ApplicationDataTables.Count; i++)
            {
                // Insert Instances that occur in both the x2data tables and the #OFFERS table.
                SQL.Append(@"Insert into #INSTANCE
                        select I.*, 2 as RANKING  from
                            [x2].[x2].Instance I (nolock)
                        join ");
                SQL.AppendFormat("X2.X2DATA.{0} O{1} (nolock) \n", ApplicationDataTables[i], i);
                SQL.AppendFormat("on O{0}.InstanceID = I.ID \n", i);
                SQL.Append(@"join #OFFERS Offers on ");

                SQL.AppendFormat(" Offers.OfferKey = O{0}.ApplicationKey ", i);

                if (SearchCriteria.WorkflowFilter.Count > 0)
                {
                    SQL.AppendLine("inner join ");
                    SQL.AppendLine("#WORKFLOWSTATE WF ");
                    SQL.AppendLine("on ");
                    SQL.AppendLine("I.WorkflowID = WF.WorkflowId ");
                    SQL.AppendLine("and ");
                    SQL.AppendLine("I.StateID = WF.StateID ");
                }

                SQL.Append(@"
                    left outer JOIN #INSTANCE IAlready
                    on
                        IAlready.ID = I.ID
                    where
                        IAlready.ID is null
						and I.subject is not null
						and I.ParentInstanceID is null
                ");
            }

            SQL.AppendFormat("select DISTINCT TOP {0} I.* from \n", SearchCriteria.MaxResults);
            SQL.Append(@"
                #INSTANCE I
            join
                [x2].[x2].State St (nolock)
            on
                St.ID = I.StateID
            where
                 St.Type = 5
            ORDER BY RANKING DESC
            ");

            SQL.AppendLine("drop table #WORKFLOWSTATE ");
            SQL.AppendLine("drop table #INSTANCE ");
            SQL.AppendLine("drop table #OFFERS ");
            SimpleQuery<Instance_DAO> IQ = new SimpleQuery<Instance_DAO>(QueryLanguage.Sql, SQL.ToString());
            IQ.AddSqlReturnDefinition(typeof(Instance_DAO), "I");
            Instance_DAO[] Is = IQ.Execute();
            Instance_DAO[] IIs = new Instance_DAO[Is.Length];
            for (int j = Is.Length - 1, p = 0; j >= 0; p++, j--)
                IIs[p] = Is[j];

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IInstance>(IIs);
        }

        public IEventList<IWorkFlow> GetArchiveSearchWorkflows(string archiveSuperSearchWorkflows)
        {
            string SQL = @"select wf.*
					from x2.X2.WorkFlow wf (nolock)
					join x2.X2.State st (nolock) on wf.ID = st.WorkFlowID and st.[Type] = " + (int)X2StateTypes.Archive;
            SQL = SQL + @" where wf.ID in (
						select max(id) as ID
						from x2.x2.workflow (nolock)
						group by [name])
					and wf.[name] in (" + archiveSuperSearchWorkflows + ")";

            SimpleQuery<WorkFlow_DAO> q = new SimpleQuery<WorkFlow_DAO>(QueryLanguage.Sql, SQL);
            q.AddSqlReturnDefinition(typeof(WorkFlow_DAO), "w");
            WorkFlow_DAO[] archiveWorkflowItems = q.Execute();
            if (archiveWorkflowItems.Length == 0)
                return null;

            return new DAOEventList<WorkFlow_DAO, IWorkFlow, WorkFlow>(new List<WorkFlow_DAO>(archiveWorkflowItems));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="instance"></param>
        /// <param name="businessKey"></param>
        /// <param name="nodeDesc"></param>
        /// <param name="longDesc"></param>
        public void SetInstanceNodeDescription(IWorkFlow workflow, IInstance instance, int businessKey, out string nodeDesc, out string longDesc)
        {
            nodeDesc = "";
            longDesc = "";

            switch (workflow.Name)
            {
                case SAHL.Common.Constants.WorkFlowName.HelpDesk:
                case SAHL.Common.Constants.WorkFlowName.DeleteDebitOrder:
                    nodeDesc = instance.Name;
                    longDesc = String.Format("{0} ({1}: {2})", instance.Subject, workflow.StorageKey, businessKey);
                    break;
                case SAHL.Common.Constants.WorkFlowName.LifeOrigination:
                    nodeDesc = "Policy: " + instance.Name + " (" + workflow.Name + ")";
                    longDesc = String.Format("{0} ({1}: {2})", instance.Subject, workflow.StorageKey, businessKey);
                    break;
                case SAHL.Common.Constants.WorkFlowName.Cap2Offers:
                    IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                    IAccount account = accRepo.GetAccountByCap2InstanceID(instance.ID);
                    nodeDesc = "Account: " + account.Key + " (" + workflow.Name + ")";
                    longDesc = String.Format("{0} ({1}: {2})", instance.Subject, "Stage", instance.State.Name);
                    break;

                case SAHL.Common.Constants.WorkFlowName.LoanAdjustments:
                    nodeDesc = instance.Name;
                    longDesc = String.Format("{0} ({1}: {2})", instance.Subject, workflow.StorageKey, businessKey);
                    break;

                case SAHL.Common.Constants.WorkFlowName.DebtCounselling:
                    nodeDesc = "Debt Counselling : " + instance.Name + " (" + workflow.Name + ")";
                    longDesc = String.Format("{0} ({1}: {2})", instance.Subject, "Stage", instance.State.Name);
                    break;

                case SAHL.Common.Constants.WorkFlowName.PersonalLoans:
                    nodeDesc = "Personal Loan : " + instance.Name + " (" + workflow.Name + ")";
                    longDesc = String.Format("{0} ({1}: {2})", instance.Subject, "Stage", instance.State.Name);
                    break;

                case SAHL.Common.Constants.WorkFlowName.DisabilityClaim:
                    ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
                    int loanAccountKey = lifeRepo.GetLoanAccountKeyByDisabilityClaimInstanceID(instance.ID);
                    nodeDesc = "Loan: " + loanAccountKey + " (" + workflow.Name + ")";
                    longDesc = String.Format("{0} ({1}: {2})", instance.Subject, "Stage", instance.State.Name);
                    break;

                default:
                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IApplication application = appRepo.GetApplicationFromInstance(instance);
                    nodeDesc = "Loan: " + application.ReservedAccount.Key + " (" + workflow.Name + ")";
                    longDesc = String.Format("{0} ({1}: {2})", instance.Subject, "Stage", instance.State.Name);
                    break;
            }
        }

        /// <summary>
        /// Gets an <see cref="IWorkflowRoleType"/> according to the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IWorkflowRoleType GetWorkflowRoleTypeByKey(int key)
        {
            return base.GetByKey<IWorkflowRoleType, WorkflowRoleType_DAO>(key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflowRoleTypeKey"></param>
        /// <param name="ADUserName"></param>
        /// <returns></returns>
        public IList<IInstance> GetInstancesForWorkflowRoleTypeAndUser(int workflowRoleTypeKey, string ADUserName)
        {
            string query = UIStatementRepository.GetStatement("Repositories.X2Repository", "GetInstancesForWorkflowRoleTypeAndUser");
            SimpleQuery<Instance_DAO> IQ = new SimpleQuery<Instance_DAO>(QueryLanguage.Sql, query.ToString(), workflowRoleTypeKey, workflowRoleTypeKey, ADUserName);
            IQ.AddSqlReturnDefinition(typeof(Instance_DAO), "I");
            Instance_DAO[] Is = IQ.Execute();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IInstance>(Is);
        }

        public IList<IInstance> GetInstancesOnWorkListForOfferRoleTypeAndUser(int offerRoleType, string ADUserName)
        {
            string query = UIStatementRepository.GetStatement("Repositories.X2Repository", "GetInstancesOnWorkListForADUser");
            SimpleQuery<Instance_DAO> IQ = new SimpleQuery<Instance_DAO>(QueryLanguage.Sql, query.ToString(), ADUserName, offerRoleType);
            IQ.AddSqlReturnDefinition(typeof(Instance_DAO), "I");
            Instance_DAO[] Is = IQ.Execute();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IInstance>(Is);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflowRoleTypeKey"></param>
        /// <returns></returns>
        public IDictionary<int, int> GetInstanceCountForWorkflowRoleTypeAndUser(int workflowRoleTypeKey)
        {
            return GetInstanceCountForWorkflowRoleTypeAndUser(workflowRoleTypeKey, null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflowRoleTypeKey"></param>
        /// <param name="ADUserName"></param>
        /// <returns></returns>
        public IDictionary<int, int> GetInstanceCountForWorkflowRoleTypeAndUser(int workflowRoleTypeKey, string ADUserName)
        {
            IDictionary<int, int> dicInstanceCount = new Dictionary<int, int>();

            string query = UIStatementRepository.GetStatement("Repositories.X2Repository", "GetInstanceCountForWorkflowRoleTypeAndUser");

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@WorkflowRoleTypeKey", workflowRoleTypeKey));
            prms.Add(new SqlParameter("@ADUserName", String.IsNullOrEmpty(ADUserName) ? "" : ADUserName));
            DataSet ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    dicInstanceCount.Add(Convert.ToInt32(row[0]), Convert.ToInt32(row[1]));
                }
            }

            return dicInstanceCount;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="adUserName"></param>
        /// <param name="workflowRoleTypeKey"></param>
        /// <param name="genericKey"></param>
        /// <param name="message"></param>
        public void AssignWorkflowRoleForADUser(Int64 instanceID, string adUserName, int workflowRoleTypeKey, int genericKey, string message)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            // Get the ADUser to assign to
            IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IADUser usr = osRepo.GetAdUserForAdUserName(adUserName);

            if (usr == null)
            {
                spc.DomainMessages.Add(new Error(String.Format("ADUser record not does not exist for '{0}'.", adUserName), ""));
                return;
            }

            #region WorkflowRole

            // Deactivate Active WorkflowRole records for WorkflowRoleType
            string query = UIStatementRepository.GetStatement("Repositories.X2Repository", "UpdateWorkflowRoleForWorkflowRoleTypeKey");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@GenericKey", genericKey));
            parameters.Add(new SqlParameter("@WorkflowRoleTypeKey", workflowRoleTypeKey));
            parameters.Add(new SqlParameter("@GeneralStatusKey", (int)GeneralStatuses.Active));
            parameters.Add(new SqlParameter("@UpdateGeneralStatusKey", (int)GeneralStatuses.Inactive));
            castleTransactionsService.ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

            // Check if WorkflowRole rec exists
            query = UIStatementRepository.GetStatement("Repositories.X2Repository", "GetWorkflowRole");
            parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@GenericKey", genericKey));
            parameters.Add(new SqlParameter("@LegalEntityKey", usr.LegalEntity.Key));
            parameters.Add(new SqlParameter("@WorkflowRoleTypeKey", workflowRoleTypeKey));

            int workflowRoleKey = 0;
            DataSet ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                workflowRoleKey = Convert.ToInt32(ds.Tables[0].Rows[0]["WorkflowRoleKey"]);

            if (workflowRoleKey > 0)
            {
                // Reactivate existing WorkflowRole record
                query = UIStatementRepository.GetStatement("Repositories.X2Repository", "UpdateWorkflowRole");
                parameters = new ParameterCollection();
                parameters.Add(new SqlParameter("@WorkflowRoleKey", workflowRoleKey));
                parameters.Add(new SqlParameter("@GeneralStatusKey", (int)GeneralStatuses.Active));
                castleTransactionsService.ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            }
            else
            {
                // Insert new WorkflowRole record
                query = UIStatementRepository.GetStatement("Repositories.X2Repository", "InsertWorkflowRole");
                parameters = new ParameterCollection();
                parameters.Add(new SqlParameter("@LegalEntityKey", usr.LegalEntity.Key));
                parameters.Add(new SqlParameter("@GenericKey", genericKey));
                parameters.Add(new SqlParameter("@WorkflowRoleTypeKey", workflowRoleTypeKey));
                parameters.Add(new SqlParameter("@GeneralStatusKey", (int)GeneralStatuses.Active));
                castleTransactionsService.ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            }

            #endregion WorkflowRole

            #region WorkflowRoleAssignment

            // Deactivate all WorkflowRoleAssigment recs for Instance
            query = UIStatementRepository.GetStatement("Repositories.X2Repository", "UpdateWorkflowRoleAssignment");
            parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@InstanceID", instanceID));
            parameters.Add(new SqlParameter("@GeneralStatusKey", (int)GeneralStatuses.Inactive));
            castleTransactionsService.ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

            // Get WorkflowRoleTypeOrganisationStructureMappingKey
            query = UIStatementRepository.GetStatement("Repositories.X2Repository", "GetWorkflowRoleTypeOrganisationStructureMapping");
            parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@ADUserKey", usr.Key));
            parameters.Add(new SqlParameter("@WorkflowRoleTypeKey", workflowRoleTypeKey));

            int workflowRoleTypeOrganisationStructureMappingKey = 0;
            ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                workflowRoleTypeOrganisationStructureMappingKey = Convert.ToInt32(ds.Tables[0].Rows[0]["WorkflowRoleTypeOrganisationStructureMappingKey"]);

            // Insert WorkflowRoleAssignment
            if (workflowRoleTypeOrganisationStructureMappingKey > 0)
            {
                query = UIStatementRepository.GetStatement("Repositories.X2Repository", "InsertWorkflowRoleAssignment");
                parameters = new ParameterCollection();
                parameters.Add(new SqlParameter("@InstanceID", instanceID));
                parameters.Add(new SqlParameter("@WorkflowRoleTypeOrganisationStructureMappingKey", workflowRoleTypeOrganisationStructureMappingKey));
                parameters.Add(new SqlParameter("@ADUserKey", usr.Key));
                parameters.Add(new SqlParameter("@GeneralStatusKey", (int)GeneralStatuses.Active));
                parameters.Add(new SqlParameter("@Message", message));
                castleTransactionsService.ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            }
            else
            {
                spc.DomainMessages.Add(new Error(String.Format("WorkflowRoleTypeOrganisationStructureMapping does not exist for '{0}' and WorkflowRoleTypeKey '{1}'.", adUserName, workflowRoleTypeKey), ""));
            }

            #endregion WorkflowRoleAssignment
        }

        public X2Data GetX2DataForInstance(IInstance instance)
        {
            X2Data data = new X2Data();

            data.GenericKeyTypeKey = instance.WorkFlow.GenericKeyTypeKey;
            data.GenericKey = -1;

            string sql = "select * from [x2].[X2DATA]." + instance.WorkFlow.StorageTable + " (nolock) where InstanceID = " + instance.ID;

            DataSet ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                data.GenericKey = Convert.ToInt32(ds.Tables[0].Rows[0][instance.WorkFlow.StorageKey]);
                data.Data = ds.Tables[0];
            }

            return data;
        }

        public IList<IWorkflowRole> GetWorkflowRoleForGenericKey(int genericKey)
        {
            string query = UIStatementRepository.GetStatement("Repositories.X2Repository", "GetWorkflowRoleForGenericKey");
            SimpleQuery<WorkflowRole_DAO> wr = new SimpleQuery<WorkflowRole_DAO>(QueryLanguage.Sql, query.ToString(), genericKey);
            wr.AddSqlReturnDefinition(typeof(WorkflowRole_DAO), "W");
            WorkflowRole_DAO[] wrs = wr.Execute();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IWorkflowRole>(wrs);
        }

        public IList<IWorkflowRole> GetWorkflowRoleForGenericKey(int genericKey, int workflowRoleTypeKey)
        {
            string query = UIStatementRepository.GetStatement("Repositories.X2Repository", "GetWorkflowRoleForGenericKeyAndWorkflowRoleType");
            SimpleQuery<WorkflowRole_DAO> wr = new SimpleQuery<WorkflowRole_DAO>(QueryLanguage.Sql, query.ToString(), genericKey, workflowRoleTypeKey);
            wr.AddSqlReturnDefinition(typeof(WorkflowRole_DAO), "W");
            WorkflowRole_DAO[] wrs = wr.Execute();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IWorkflowRole>(wrs);
        }

        public IList<IWorkflowRole> GetWorkflowRoleForGenericKey(int genericKey, int workflowRoleTypeKey, int generalStatusKey)
        {
            string query = UIStatementRepository.GetStatement("Repositories.X2Repository", "GetWorkflowRoleForGenericKeyAndTypeAndStatus");
            SimpleQuery<WorkflowRole_DAO> wr = new SimpleQuery<WorkflowRole_DAO>(QueryLanguage.Sql, query.ToString(), genericKey, workflowRoleTypeKey, generalStatusKey);
            wr.AddSqlReturnDefinition(typeof(WorkflowRole_DAO), "W");
            WorkflowRole_DAO[] wrs = wr.Execute();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IWorkflowRole>(wrs);
        }

        public IList<IWorkflowRole> GetWorkflowRoleForLegalEntityKey(int legalEntityKey, int workflowRoleTypeKey, int generalStatusKey)
        {
            string query = UIStatementRepository.GetStatement("Repositories.X2Repository", "GetWorkflowRoleForLegalEntityKey");
            SimpleQuery<WorkflowRole_DAO> wr = new SimpleQuery<WorkflowRole_DAO>(QueryLanguage.Sql, query.ToString(), legalEntityKey, workflowRoleTypeKey, generalStatusKey);
            wr.AddSqlReturnDefinition(typeof(WorkflowRole_DAO), "W");
            WorkflowRole_DAO[] wrs = wr.Execute();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IWorkflowRole>(wrs);
        }

        public IList<IWorkflowRole> GetWorkflowRoleForGenericKey(string adUserName, int workflowRoleTypeGroupKey, int generalStatusKey)
        {
            string query = UIStatementRepository.GetStatement("Repositories.X2Repository", "GetWorkflowRoleForWorkflowRoleTypeGroupAndADUser");
            SimpleQuery<WorkflowRole_DAO> wr = new SimpleQuery<WorkflowRole_DAO>(QueryLanguage.Sql, query.ToString(), adUserName, workflowRoleTypeGroupKey, generalStatusKey);
            wr.AddSqlReturnDefinition(typeof(WorkflowRole_DAO), "W");
            WorkflowRole_DAO[] wrs = wr.Execute();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IWorkflowRole>(wrs);
        }

        /// <summary>
        /// Intentionally not returning business model objects so there is no risk of consumers
        /// walking through the domain
        /// This is X2 data
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable GetScheduledActivities(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0)
                    sb.Append(",");

                sb.Append(dt.Rows[i][0].ToString());
            }

            string sql = String.Format(@"select
                    *
                    from x2.X2.ScheduledActivity sa (nolock)
                    join x2.x2.Activity a (nolock) on sa.ActivityID = a.ID
                    where sa.InstanceID in ({0})", sb.ToString());

            DataSet ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return new DataTable();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetRelatedInstances(Int64 id)
        {
            string sql = String.Format(@"
                        --Get the parent/root ID
                        ; with parent as
                        (
	                        select * from [x2].[x2].Instance i (nolock)
	                        where i.ID = @id
	                        union all
	                        select i.* from [x2].[x2].Instance i (nolock)
	                        join parent on i.ID = parent.ParentInstanceID
                        )

                        select @id = ID from parent where ParentInstanceID is null

                        --Get parent and all the child ID's
                        ; with cte as
                        (
	                        select * from [x2].[x2].Instance i (nolock)
	                        where i.ID = @id --or i.ParentInstanceID = @id
	                        union all
	                        select i.* from [x2].[x2].Instance i (nolock)
	                        join cte on i.ParentInstanceID = cte.ID
                        )

                        select distinct ID from cte");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@id", id));

            DataSet ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), prms);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return new DataTable();
        }

        /// <summary>
        /// Inserts a ercord into the X2.ActiveExternalActivity Table. This will be picked up by the engine and executed. Used to
        /// create cases in remote workflows (Submit applicaiton in App Man an example) or to pickup a case and move it within
        /// a workflow (App Man. Clone case sits with a 15 day timer. The parent raises an EXT activity to archive the child)
        /// </summary>
        /// <param name="ExtActivityName"></param>
        /// <param name="ActivatingInstanceID"></param>
        /// <param name="workflowName"></param>
        /// <param name="processName"></param>
        /// <param name="XMLFieldInputs"></param>
        public void CreateAndSaveActiveExternalActivity(string ExtActivityName, Int64 ActivatingInstanceID, string workflowName, string processName, string XMLFieldInputs)
        {
            IWorkFlow workflow = GetWorkFlowByName(workflowName, processName);
            IActiveExternalActivity activeExternalActivity = base.CreateEmpty<IActiveExternalActivity, ActiveExternalActivity_DAO>();
            activeExternalActivity.ActivatingInstanceID = ActivatingInstanceID;
            activeExternalActivity.ActivationTime = DateTime.Now;
            activeExternalActivity.ActivityXMLData = XMLFieldInputs;
            activeExternalActivity.ExternalActivity = GetExternalActivityByName(ExtActivityName, workflow.ID);
            activeExternalActivity.WorkFlowID = workflow.ID;
            activeExternalActivity.WorkFlowProviderName = "";
            base.Save<IActiveExternalActivity, ActiveExternalActivity_DAO>(activeExternalActivity);
        }

        /// <summary>
        /// We need to find the parent instance / main instance for the open status Debt Counselling case
        /// of the related the account
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public IInstance GetDebtCousellingInstanceByAccountKey(int accountKey)
        {
            IInstance instance = null;

            string query = string.Format(UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetDebtCousellingInstanceByAccountKey"), accountKey);

            SimpleQuery<Instance_DAO> p = new SimpleQuery<Instance_DAO>(QueryLanguage.Sql, query);
            p.AddSqlReturnDefinition(typeof(Instance_DAO), "xi");
            p.SetQueryRange(1);
            Instance_DAO[] instances = p.Execute();

            if (instances != null && instances.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                instance = bmtm.GetMappedType<IInstance, Instance_DAO>(instances[0]);
            }

            return instance;
        }

        /// <summary>
        /// Get Search Setup
        /// </summary>
        /// <returns></returns>
        public IList<ISetup> GetSearchSetups()
        {
            var setups = Setup_DAO.FindAll();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<ISetup>(setups);
        }

        /// <summary>
        /// Get Search Contexts
        /// </summary>
        /// <returns></returns>
        public IList<IContext> GetSearchContexts()
        {
            var contexts = Context_DAO.FindAll();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IContext>(contexts);
        }

        /// <summary>
        /// Get Search Internal Role
        /// </summary>
        /// <returns></returns>
        public IList<IInternalRole> GetSearchInternalRoles()
        {
            var internalRoles = InternalRole_DAO.FindAll();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IInternalRole>(internalRoles);
        }

        /// <summary>
        /// Get Search Workflow Contexts
        /// </summary>
        /// <returns></returns>
        public IList<IWorkflowContext> GetSearchWorkflowContexts()
        {
            var workflowContexts = WorkflowContext_DAO.FindAll();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IWorkflowContext>(workflowContexts);
        }

        /// <summary>
        /// Get Search Workflow Datas
        /// </summary>
        /// <returns></returns>
        public IList<IWorkflowData> GetSearchWorkflowDatas()
        {
            var workflowDatas = WorkflowData_DAO.FindAll();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IWorkflowData>(workflowDatas);
        }

        /// <summary>
        /// Get Search Filters
        /// </summary>
        /// <returns></returns>
        public IList<BusinessModel.Interfaces.IFilter> GetSearchFilters()
        {
            var filters = Filter_DAO.FindAll();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<BusinessModel.Interfaces.IFilter>(filters);
        }

        /// <summary>
        /// Get Context Filters
        /// </summary>
        /// <returns></returns>
        public IList<IPreContextFilter> GetPreContextFilters()
        {
            var selects = PreContextFilter_DAO.FindAll();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IPreContextFilter>(selects);
        }

        /// <summary>
        /// Get Workflow Data Filter
        /// </summary>
        /// <returns></returns>
        public IList<IPreWorkflowDataFilter> GetPreWorkflowDataFilter()
        {
            var selects = PreWorkflowDataFilter_DAO.FindAll();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IPreWorkflowDataFilter>(selects);
        }

        /// <summary>
        /// Get Search Select
        /// </summary>
        /// <returns></returns>
        public IList<ISearchSelect> GetSearchSelects()
        {
            var selects = SearchSelect_DAO.FindAll();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<ISearchSelect>(selects);
        }

        /// <summary>
        /// Get Search Filters
        /// </summary>
        /// <returns></returns>
        public IList<ICleanup> GetSearchCleanups()
        {
            var cleanups = Cleanup_DAO.FindAll();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<ICleanup>(cleanups);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="InstanceID"></param>
        /// <returns></returns>
        public DataTable GetCurrentConsultantAndAdmin(Int64 InstanceID)
        {
            using (new TransactionScope())
            {
                // Go look at the x2data.app_man for this offerkey
                // Get the instnace
                // look for the latest Branch consultant D, Branch Admin D and FL Processor D
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(" select top 1 LE.EMailAddress ");
                sb.AppendFormat("from vw_WFAssignment wfa ");
                sb.AppendFormat("join [2am]..aduser a on wfa.aduserkey=a.aduserkey ");
                sb.AppendFormat("join [2am]..LegalEntity LE on a.legalentitykey=le.legalentitykey ");
                sb.AppendFormat("where IID={0} and OfferRoleTypeKey =101 ", InstanceID);
                sb.AppendFormat("union ");
                sb.AppendFormat("select top 1 LE.EMailAddress ");
                sb.AppendFormat("from vw_WFAssignment wfa ");
                sb.AppendFormat("join [2am]..aduser a on wfa.aduserkey=a.aduserkey ");
                sb.AppendFormat("join [2am]..LegalEntity LE on a.legalentitykey=le.legalentitykey ");
                sb.AppendFormat("where IID={0} and OfferRoleTypeKey =102 ", InstanceID);
                sb.AppendFormat("union ");
                sb.AppendFormat("select top 1 LE.EMailAddress ");
                sb.AppendFormat("from vw_WFAssignment wfa ");
                sb.AppendFormat("join [2am]..aduser a on wfa.aduserkey=a.aduserkey ");
                sb.AppendFormat("join [2am]..LegalEntity LE on a.legalentitykey=le.legalentitykey ");
                sb.AppendFormat("where IID={0} and OfferRoleTypeKey =857 ", InstanceID);

                DataSet ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(sb.ToString(), typeof(Instance_DAO), null);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        public string GetCurrentConsultantEmailAddress(Int64 InstanceID)
        {
            using (new TransactionScope())
            {
                // Get the instnace
                // look for the latest Branch consultant D
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(" select top 1 LE.EMailAddress ");
                sb.AppendFormat("from vw_WFAssignment wfa ");
                sb.AppendFormat("join [2am]..aduser a on wfa.aduserkey=a.aduserkey ");
                sb.AppendFormat("join [2am]..LegalEntity LE on a.legalentitykey=le.legalentitykey ");
                sb.AppendFormat("where IID={0} and OfferRoleTypeKey =101 ", InstanceID);

                object o = castleTransactionsService.ExecuteScalarOnCastleTran(sb.ToString(), typeof(Instance_DAO), null);
                if (o != null)
                {
                    return o.ToString();
                }
            }
            return string.Empty;
        }

        public string GetEmailAddressForCaseOwner(long instanceID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select le.emailaddress ");
            sb.AppendLine("from x2.instance i (nolock) join x2.instance i1 (nolock) on i.parentinstanceid=i1.id ");
            sb.AppendLine("join [2am]..aduser a on i1.CreatorADUserName=a.adusername ");
            sb.AppendLine("join [2am]..legalentity le on a.legalentitykey=le.legalentitykey ");
            sb.AppendFormat("where i.id={0}", instanceID);

            DataSet ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(sb.ToString(), typeof(Instance_DAO), null);

            string emailAddress = ds.Tables[0].Rows[0][0].ToString();
            return emailAddress;
        }

        public bool HasRelatedSourceInstancesInWorkflow(Int64 instanceID, string workflow)
        {
            bool is2ndPass = false;

            DataTable dt = new DataTable();
            StringBuilder sql = new StringBuilder();
            sql.Append("select si.* from x2.x2.Instance i (nolock) ");
            sql.Append("join x2.x2.Instance si (nolock) on i.SourceInstanceID=si.SourceInstanceID ");
            sql.Append("join x2.x2.Workflow w (nolock) on si.WorkflowID=w.ID ");
            sql.AppendFormat("where i.ID = {0} and w.Name='{1}'", instanceID, workflow);

            DataSet ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(sql.ToString(), typeof(Instance_DAO), new ParameterCollection());

            if (ds.Tables != null && ds.Tables[0].Rows.Count > 1)
                is2ndPass = true;

            return is2ndPass;
        }

        public bool IsValuationApprovalRequired(long instanceID)
        {
            bool approvalRequired = false;
            string applicationName = "DomainService.Client";
            string statementName = "IsValuationApprovalRequired";

            ParameterCollection param = new ParameterCollection();
            param.Add(new SqlParameter("@InstanceID", instanceID));

            string uistatementSQL = UIStatementRepository.GetStatement(applicationName, statementName);
            object result = castleTransactionsService.ExecuteScalarOnCastleTran(uistatementSQL, typeof(Instance_DAO), param);

            if (result != null)
            {
                if (Convert.ToBoolean(result) == false)
                    approvalRequired = true;
                else
                    approvalRequired = false;
            }

            return approvalRequired;
        }

        public string GetPersonalLoansInstanceSubject(int applicationKey)
        {
            string subject = String.Empty;
            string query = string.Format(@"SELECT
                                                [2AM].[dbo].[LegalEntityLegalName] (ER.LegalEntityKey, 1) AS Name
                                            FROM
                                                [2am].dbo.ExternalRole (nolock) AS ER
                                            INNER JOIN
                                                [2am].dbo.Offer (nolock) AS O ON ER.GenericKey = O.OfferKey
                                            WHERE
                                                (ER.ExternalRoleTypeKey = 1) AND (O.OfferKey = {0})", applicationKey);
            var dataSet = castleTransactionsService.ExecuteQueryOnCastleTran(query, Databases.TwoAM, null);
            if (dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    subject = subject + dr[0].ToString() + " & ";
                }

                subject = subject.Substring(0, subject.Length - 2);
            }
            return subject;
        }

        public string GetDebtCounsellingInstanceSubject(int debtCounsellingKey)
        {
            string subject = String.Empty;

            string query = string.Format(@"SELECT   ISNULL(ST.Description + ' ', '') + COALESCE (LE.Initials + ' ', LEFT(LE.FirstNames, 1) + ' ', '')
												+ ISNULL(LE.Surname, '') AS Name
												FROM         [2am].dbo.ExternalRole (nolock) AS ER INNER JOIN
												[2am].dbo.LegalEntity (nolock) AS LE ON ER.LegalEntityKey = LE.LegalEntityKey INNER JOIN
												[2am].debtcounselling.DebtCounselling (nolock) AS DC ON ER.GenericKey = DC.DebtCounsellingKey LEFT OUTER JOIN
												[2am].dbo.SalutationType (nolock) AS ST ON LE.Salutationkey = ST.SalutationKey
												WHERE     (ER.ExternalRoleTypeKey = 1) AND (DC.DebtCounsellingKey = {0})
												GROUP BY LE.FirstNames, LE.Initials, LE.Surname, LE.PreferredName, ST.Description", debtCounsellingKey);

            DataSet dsLegalEntity = castleTransactionsService.ExecuteQueryOnCastleTran(query, SAHL.Common.Globals.Databases.TwoAM, null);
            if (dsLegalEntity.Tables.Count > 0)
            {
                if (dsLegalEntity.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsLegalEntity.Tables[0].Rows)
                    {
                        subject = subject + dr[0].ToString() + " & ";
                    }

                    subject = subject.Substring(0, subject.Length - 2);
                }
            }

            return subject;
        }

        public void SetX2DataRow(long InstanceID, IDictionary<string, object> X2Data)
        {
            IInstance instance = GetInstanceByKey(InstanceID);

            string Query = "UPDATE X2.X2DATA.{0} SET ";
            string Where = " WHERE InstanceID = {1} ";

            foreach (KeyValuePair<string, object> KP in X2Data)
            {
                Query += (KP.Key + " = " + KP.Value + ", ");
            }
            Query = Query.Substring(0, Query.Length - 2);
            Query += Where;
            Query = String.Format(Query, instance.WorkFlow.StorageTable, InstanceID);

            this.castleTransactionsService.ExecuteNonQueryOnCastleTran(Query, SAHL.Common.Globals.Databases.X2, null);
        }

        public IDictionary<string, object> GetX2DataRow(long instanceID)
        {
            IInstance instance = GetInstanceByKey(instanceID);

            string query = string.Format("SELECT * FROM X2DATA.{0} (nolock) WHERE InstanceID = {1}", instance.WorkFlow.StorageTable, instanceID);
            DataTable DT = new DataTable();

            DataSet ds = this.castleTransactionsService.ExecuteQueryOnCastleTran(query, SAHL.Common.Globals.Databases.X2, null);
            if (ds.Tables.Count > 0)
            {
                DT = ds.Tables[0];
            }

            Dictionary<string, object> dict = new Dictionary<string, object>();

            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Columns.Count; i++)
                {
                    dict.Add(DT.Columns[i].ColumnName, DT.Rows[0].ItemArray[i]);
                }
            }

            return dict;
        }

        public bool HasInstancePerformedActivity(long instanceID, string activityName)
        {
            return (GetWorkflowHistoryForInstanceAndActivity(instanceID, activityName).Count > 0);
        }

        public string GetPreviousStateName(long instanceID)
        {
            var instance = GetInstanceByKey(instanceID);
            return instance.State.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        public DataRow GetApplicationManagementDataForApplicationKey(int applicationKey)
        {
            string sql = UIStatementRepository.GetStatement("Repositories.X2Repository", "GetApplicationManagementDataForApplicationKey");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            // Add the required parameters
            parameters.Add(new SqlParameter("@ApplicationKey", applicationKey));

            // Execute Query
            DataSet dsResults = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(Instance_DAO), parameters);
            if (dsResults != null && dsResults.Tables.Count > 0 && dsResults.Tables[0].Rows.Count > 0)
                return dsResults.Tables[0].Rows[0];

            return null;
        }

        public IList<IProcess> GetProcessesByUserActivity(string userActivityName)
        {
            // we need to find all the instances that the application has
            string query = UIStatementRepository.GetStatement("Repositories.X2Repository", "GetProcessesByUserActivity");
            SimpleQuery<Process_DAO> PQ = new SimpleQuery<Process_DAO>(QueryLanguage.Sql, query.ToString(), userActivityName);
            PQ.AddSqlReturnDefinition(typeof(Process_DAO), "P");
            Process_DAO[] process = PQ.Execute();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IProcess>(process);
        }


        public IList<IInstance> GetFLInstancesForAccountAtState(int accountKey, string stateName)
        {
            string query = String.Format(UIStatementRepository.GetStatement("Repositories.X2Repository", "GetFLInstancesForAccountAtState"), accountKey, stateName);
            SimpleQuery<Instance_DAO> IQ = new SimpleQuery<Instance_DAO>(QueryLanguage.Sql, query);
            IQ.AddSqlReturnDefinition(typeof(Instance_DAO), "I");
            Instance_DAO[] Is = IQ.Execute();

            IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return typeMapper.GetMappedTypeList<IInstance>(Is);
        }
    }

    // There is not direct link between instance and offer, hence
    public class SortedInstance
    {
        IInstance _instance;
        IApplication _application;

        public SortedInstance(IInstance inst, IApplication app)
        {
            _instance = inst;
            _application = app;
        }

        public IInstance Instance
        {
            get { return _instance; }
        }

        public IApplication Application
        {
            get { return _application; }
        }
    }
}