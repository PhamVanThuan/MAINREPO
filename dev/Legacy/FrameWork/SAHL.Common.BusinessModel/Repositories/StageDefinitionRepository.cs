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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IStageDefinitionRepository))]
    public class StageDefinitionRepository : AbstractRepositoryBase, IStageDefinitionRepository
    {
        public StageDefinitionRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public StageDefinitionRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        /// <summary>
        /// Returns all the recorded stage transitions for the specified generic key
        /// </summary>
        /// <param name="genericKeyValue"></param>
        /// <returns></returns>
        public IList<IStageTransition> GetStageTransitionsByGenericKey(int genericKeyValue)
        {
            IList<int> genericKeys = new List<int>();
            genericKeys.Add(genericKeyValue);

            return GetStageTransitionsByGenericKeys(genericKeys);
        }

        /// <summary>
        /// Returns all the recorded stage transitions for the specified generic keys
        /// </summary>
        /// <param name="genericKeyValues"></param>
        /// <returns>IList&lt;IStageTransition&gt;</returns>
        public IList<IStageTransition> GetStageTransitionsByGenericKeys(IList<int> genericKeyValues)
        {
            IList<IStageTransition> _stageTransitions = new List<IStageTransition>();

            if (genericKeyValues.Count <= 0)
                return null;

            // build delimited list for selection
            string delimitedList = "(";
            for (int i = 0; i < genericKeyValues.Count; i++)
            {
                if (i == 0)
                    delimitedList += genericKeyValues[i].ToString();
                else
                    delimitedList += "," + genericKeyValues[i].ToString();
            }
            delimitedList += ")";

            // get the data from the db
            string HQL = "from StageTransition_DAO t where t.GenericKey in " + delimitedList + " order by t.TransitionDate";
            SimpleQuery<StageTransition_DAO> q = new SimpleQuery<StageTransition_DAO>(HQL);
            StageTransition_DAO[] res = q.Execute();
            if (res != null && res.Length > 0)
            {
                for (int j = 0; j < res.Length; j++)
                {
                    _stageTransitions.Add(new StageTransition(res[j]));
                }
            }
            return _stageTransitions;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dicGenricKeyTypeAndKeys"></param>
        /// <returns></returns>
        public DataTable GetStageTransitionDTByGenericKeyTypeAndKeys(IDictionary<GenericKeyTypes, List<int>> dicGenricKeyTypeAndKeys)
        {
            DataTable _dtStageTransitions = new DataTable();

            if (dicGenricKeyTypeAndKeys.Count <= 0)
                return _dtStageTransitions;

            int loopCount = 0;
            foreach (var item in dicGenricKeyTypeAndKeys)
            {
                loopCount++;
                int genericKeyTypeKey = (int)item.Key;
                string genericKeys = BuildDelimitedList(item.Value);

                string query = UIStatementRepository.GetStatement("Repositories.StageDefinitionRepository", "GetStageTransitionDTByGenricKeyTypeAndKeys");
                query = string.Format(query, genericKeyTypeKey, genericKeys);

                ParameterCollection param = new ParameterCollection();
                DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(StageTransition_DAO), param);

                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    if (loopCount == 1)
                        _dtStageTransitions = ds.Tables[0].Clone();

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        _dtStageTransitions.ImportRow(row);
                    }
                }
            }

            return _dtStageTransitions;
        }

        private static string BuildDelimitedList(IList<int> KeyList)
        {
            string delimitedList = "(";
            for (int i = 0; i < KeyList.Count; i++)
            {
                if (i == 0)
                    delimitedList += KeyList[i].ToString();
                else
                    delimitedList += "," + KeyList[i].ToString();
            }
            delimitedList += ")";
            return delimitedList;
        }

        /// <summary>
        /// Returns the stage definition group of the given key value.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IStageDefinitionGroup GetStageDefinitionGroupByKey(int Key)
        {
            return base.GetByKey<IStageDefinitionGroup, StageDefinitionGroup_DAO>(Key);
        }

        /// <summary>
        /// Returns the IStageDefinition for the given StageDefinition Description
        /// </summary>
        /// <param name="stageDefinitionDescription"></param>
        /// <returns></returns>
        public IStageDefinition GetStageDefinitionByDescription(string stageDefinitionDescription)
        {
            string HQL = "from StageDefinition_DAO sd where sd.Description = ?";
            SimpleQuery<StageDefinition_DAO> q = new SimpleQuery<StageDefinition_DAO>(HQL, stageDefinitionDescription);
            StageDefinition_DAO[] stageDefinitions = q.Execute();

            if (stageDefinitions != null && stageDefinitions.Length >= 1)
            {
                return new StageDefinition(stageDefinitions[0]);
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stageDefinitionGroupKey"></param>
        /// <param name="stageDefinitionKey"></param>
        /// <returns></returns>
        public IStageDefinitionStageDefinitionGroup GetStageDefinitionStageDefinitionGroup(int stageDefinitionGroupKey, int stageDefinitionKey)
        {
            string HQL = "from StageDefinitionStageDefinitionGroup_DAO sdsdg where sdsdg.StageDefinitionGroup.Key = ? and sdsdg.StageDefinition.Key = ?";
            SimpleQuery<StageDefinitionStageDefinitionGroup_DAO> q = new SimpleQuery<StageDefinitionStageDefinitionGroup_DAO>(HQL, stageDefinitionGroupKey, stageDefinitionKey);
            StageDefinitionStageDefinitionGroup_DAO[] stageDefinitionStageDefinitionGroups = q.Execute();

            if (stageDefinitionStageDefinitionGroups != null && stageDefinitionStageDefinitionGroups.Length >= 1)
            {
                return new StageDefinitionStageDefinitionGroup(stageDefinitionStageDefinitionGroups[0]);
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stageDefinitionGroupKey"></param>
        /// <param name="stageDefinitionKey"></param>
        /// <returns></returns>
        public int GetStageDefinitionStageDefinitionGroupKey(int stageDefinitionGroupKey, int stageDefinitionKey)
        {
            IDbConnection con = Helper.GetSQLDBConnection();
            try
            {
                string sqlQuery = UIStatementRepository.GetStatement("COMMON", "SDSDGKeyGetBySDGKeyandSDKey");

                ParameterCollection parameters = new ParameterCollection();
                parameters.Add(new SqlParameter("@sdgKey", stageDefinitionGroupKey));
                parameters.Add(new SqlParameter("@sdKey", stageDefinitionKey));

                Object o = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (o != null)
                    return (int)o;

                return 0;
            }
            finally
            {
                if (con != null)
                    con.Dispose();
            }
        }

        /// <summary>
        /// Get the last Composite occurrence of a list of StageDefinitionStageDefinitionGroups
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="stageDefinitionStageDefinitionGroups"></param>
        /// <returns></returns>
        public IStageTransitionComposite GetLastStageTransitionComposite(int genericKey, IList<int> stageDefinitionStageDefinitionGroups)
        {
            if (stageDefinitionStageDefinitionGroups.Count <= 0)
                return null;

            // build delimited list for selection
            string delimitedList = "";
            for (int i = 0; i < stageDefinitionStageDefinitionGroups.Count; i++)
            {
                if (i == 0)
                    delimitedList += stageDefinitionStageDefinitionGroups[i].ToString();
                else
                    delimitedList += "," + stageDefinitionStageDefinitionGroups[i].ToString();
            }

            string HQL = "from StageTransitionComposite_DAO stc where stc.GenericKey = ? and stc.StageDefinitionStageDefinitionGroup.Key in (" + delimitedList + ") order by stc.TransitionDate desc";
            SimpleQuery<StageTransitionComposite_DAO> q = new SimpleQuery<StageTransitionComposite_DAO>(HQL, genericKey);

            StageTransitionComposite_DAO[] stageTransitionComposites = q.Execute();

            if (stageTransitionComposites != null && stageTransitionComposites.Length > 0)
            {
                return new StageTransitionComposite(stageTransitionComposites[0]);
            }
            return null;
        }

        /// <summary>
        /// Get the Stage Transition prior to the Transition passed in with the matching
        /// StageDefinitionStageDefinitionGroupKey passed in.
        /// This is a HACK because x2 is writing the Transitions and Composites and we
        /// need to get the ADUser that made the decision.
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="stageDefinitionStageDefinitionGroupKey"></param>
        /// <param name="stageTransitionKey"></param>
        /// <returns></returns>
        public IStageTransition GetPreviousTransition(int genericKey, int stageDefinitionStageDefinitionGroupKey, int stageTransitionKey)
        {
            //StageTransition_DAO st;
            //st.StageDefinitionStageDefinitionGroup.Key;
            //st.Key;
            //st.TransitionDate;
            string HQL = "from StageTransition_DAO st where st.GenericKey = ? and st.StageDefinitionStageDefinitionGroup.Key = ? and st.Key <= ? order by st.TransitionDate desc";
            SimpleQuery<StageTransition_DAO> q = new SimpleQuery<StageTransition_DAO>(HQL, genericKey, stageDefinitionStageDefinitionGroupKey, stageTransitionKey);
            q.SetQueryRange(1);

            StageTransition_DAO[] stageTransitions = q.Execute();

            if (stageTransitions != null && stageTransitions.Length > 0)
            {
                return new StageTransition(stageTransitions[0]);
            }
            return null;
        }

        /// <summary>
        /// Creates an empty StageTransition object
        /// </summary>
        /// <returns>IStageTransition</returns>
        public IStageTransition CreateEmptyStageTransition()
        {
            return base.CreateEmpty<IStageTransition, StageTransition_DAO>();

            //return new StageTransition(new StageTransition_DAO());
        }

        /// <summary>
        /// Saves a StageTransition record
        /// </summary>
        /// <param name="stageTransition"></param>
        public void SaveStageTransition(IStageTransition stageTransition)
        {
            base.Save<IStageTransition, StageTransition_DAO>(stageTransition);

            //IDAOObject dao = stageTransition as IDAOObject;
            //StageTransition_DAO st = (StageTransition_DAO)dao.GetDAOObject();
            //st.SaveAndFlush();

            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        /// <summary>
        /// Saves a StageTransition record
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="StageDefinitionGroupKey"></param>
        /// <param name="StageDefinitionDescription"></param>
        /// <param name="Comments"></param>
        /// <param name="ADUser"></param>
        public IStageTransition SaveStageTransition(int GenericKey, int StageDefinitionGroupKey, string StageDefinitionDescription, string Comments, IADUser ADUser)
        {
            IStageDefinition stageDefinition = GetStageDefinitionByDescription(StageDefinitionDescription);
            IStageDefinitionStageDefinitionGroup stageDefinitionStageDefinitionGroup = GetStageDefinitionStageDefinitionGroup(StageDefinitionGroupKey, stageDefinition.Key);

            IStageTransition stageTransition = CreateEmptyStageTransition();

            stageTransition.ADUser = ADUser;
            stageTransition.Comments = Comments;
            stageTransition.GenericKey = GenericKey;
            stageTransition.StageDefinitionStageDefinitionGroup = stageDefinitionStageDefinitionGroup;
            stageTransition.TransitionDate = System.DateTime.Now;

            SaveStageTransition(stageTransition);

            return stageTransition;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="StageDefinitionStageDefinitionGroup"></param>
        /// <param name="Comments"></param>
        /// <param name="ADUserName"></param>
        /// <returns></returns>
        public IStageTransition SaveStageTransition(int GenericKey, StageDefinitionStageDefinitionGroups StageDefinitionStageDefinitionGroup, string Comments, string ADUserName)
        {
            return SaveStageTransition(GenericKey, StageDefinitionStageDefinitionGroup, DateTime.Now, Comments, ADUserName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="StageDefinitionStageDefinitionGroup"></param>
        /// <param name="transitionDate"></param>
        /// <param name="Comments"></param>
        /// <param name="ADUserName"></param>
        /// <returns></returns>
        public IStageTransition SaveStageTransition(int GenericKey, StageDefinitionStageDefinitionGroups StageDefinitionStageDefinitionGroup, DateTime transitionDate, string Comments, string ADUserName)
        {
            IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IStageDefinitionStageDefinitionGroup stageDefinitionStageDefinitionGroup = base.GetByKey<IStageDefinitionStageDefinitionGroup, StageDefinitionStageDefinitionGroup_DAO>((int)StageDefinitionStageDefinitionGroup);
            IStageTransition stageTransition = CreateEmptyStageTransition();

            stageTransition.ADUser = osRepo.GetAdUserForAdUserName(ADUserName);
            stageTransition.Comments = Comments;
            stageTransition.GenericKey = GenericKey;
            stageTransition.StageDefinitionStageDefinitionGroup = stageDefinitionStageDefinitionGroup;
            stageTransition.TransitionDate = transitionDate;
            stageTransition.EndTransitionDate = transitionDate;
            SaveStageTransition(stageTransition);

            return stageTransition;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IStageTransition GetStageTransitionByKey(int Key)
        {
            return base.GetByKey<IStageTransition, StageTransition_DAO>(Key);

            //StageTransition_DAO stageTransition = StageTransition_DAO.Find(stageTransitionKey);
            //if (stageTransition != null)
            //{
            //    return new StageTransition(stageTransition);
            //}
            //return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="sdsdgKey"></param>
        /// <returns></returns>
        public IStageTransition GetLastStageTransitionByGenericKeyAndSDSDGKey(int genericKey, int genericKeyTypeKey, int sdsdgKey)
        {
            string HQL = @"from StageTransition_DAO st 
                        where st.GenericKey = ? 
                        and st.StageDefinitionStageDefinitionGroup.Key = ? 
                        and st.StageDefinitionStageDefinitionGroup.StageDefinitionGroup.GenericKeyType.Key = ?
                        order by st.Key desc";

            SimpleQuery<StageTransition_DAO> q = new SimpleQuery<StageTransition_DAO>(HQL, genericKey, sdsdgKey, genericKeyTypeKey);
            q.SetQueryRange(1);

            StageTransition_DAO[] stageTransition = q.Execute();

            if (stageTransition != null && stageTransition.Length > 0)
            {
                return new StageTransition(stageTransition[0]);
            }
            return null;
        }

        /// <summary>
        /// Implements <see cref="IStageDefinitionRepository.CheckCompositeStageDefinition"/>
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="stageDefinitionstageDefinitionGroupKey"></param>
        /// <returns></returns>
        public bool CheckCompositeStageDefinition(int genericKey, int stageDefinitionstageDefinitionGroupKey)
        {
            if (genericKey <= 0)
                return false;

            string sqlQuery = UIStatementRepository.GetStatement("COMMON", "CompositeStageDefinitionGetByGenericKeyandSDSDGKey");

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@genericKey", genericKey));
            parameters.Add(new SqlParameter("@sdsdgKey", stageDefinitionstageDefinitionGroupKey));

            Object o = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
            if (o != null)
                return true;

            return false;
        }

        /// <summary>
        /// Implements <see cref="IStageDefinitionRepository.CountCompositeStageOccurance(int,int)"/>
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="stageDefinitionstageDefinitionGroupKey"></param>
        /// <returns></returns>
        public int CountCompositeStageOccurance(int genericKey, int stageDefinitionstageDefinitionGroupKey)
        {
            int results = 0;

            if (genericKey > 0)
            {
                ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();
                ISession session = sessionHolder.CreateSession(typeof(StageTransitionComposite_DAO));

                string hql = @"select count(*) countRows from
                    (
                    select stc.GenericKey, stc.TransitionDate, stc.StageDefinitionStageDefinitionGroupKey from
	                    dbo.StageTransitionComposite stc
                    join
	                    dbo.StageDefinitionStageDefinitionGroup sdsdg
                    on
	                    stc.StageDefinitionStageDefinitionGroupKey = sdsdg.StageDefinitionStageDefinitionGroupKey
                    where
	                    stc.GenericKey = " + genericKey + @"
                    and
	                    STC.StageDefinitionStageDefinitionGroupKey = " + stageDefinitionstageDefinitionGroupKey + @"
                    group by
	                    stc.GenericKey, stc.TransitionDate, stc.StageDefinitionStageDefinitionGroupKey
                    )
                    njaf";

                IQuery sqlQuery = session.CreateSQLQuery(hql).AddScalar("countRows", NHibernateUtil.Int32);

                results = sqlQuery.UniqueResult<int>();
            }

            return results;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="stageDefinitionstageDefinitionGroupKey"></param>
        /// <param name="doGroup"></param>
        /// <returns></returns>
        public int CountCompositeStageOccurance(int genericKey, int stageDefinitionstageDefinitionGroupKey, bool doGroup)
        {
            if (!doGroup)
            {
                int results = 0;

                if (genericKey > 0)
                {
                    ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();
                    ISession session = sessionHolder.CreateSession(typeof(StageTransitionComposite_DAO));

                    string hql = @"select count(*) countRows
                    from dbo.StageTransitionComposite stc
                    join
	                    dbo.StageDefinitionStageDefinitionGroup sdsdg
                    on
	                    stc.StageDefinitionStageDefinitionGroupKey = sdsdg.StageDefinitionStageDefinitionGroupKey
                    where
	                    stc.GenericKey = " + genericKey + @"
                    and
	                    STC.StageDefinitionStageDefinitionGroupKey = " + stageDefinitionstageDefinitionGroupKey;

                    IQuery sqlQuery = session.CreateSQLQuery(hql).AddScalar("countRows", NHibernateUtil.Int32);

                    results = sqlQuery.UniqueResult<int>();
                }

                return results;
            }
            else
            {
                return CountCompositeStageOccurance(genericKey, stageDefinitionstageDefinitionGroupKey);
            }
        }

        public IList<IStageTransition> GetStageTransitionList(int genericKey, int genericKeyTypeKey, List<int> stageDefinitionstageDefinitionGroupKey)
        {
            IList<IStageTransition> stageTransList = new List<IStageTransition>();
            IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();

            string sql = string.Format(@"select st.*
            from stagetransition st (nolock)
            join stagedefinitionstagedefinitiongroup sdsdg (nolock) on
            st.StageDefinitionStageDefinitionGroupKey = sdsdg.StageDefinitionStageDefinitionGroupKey
            join stagedefinition sd (nolock) on sdsdg.stagedefinitionkey = sd.stagedefinitionkey
            join stagedefinitiongroup sdg (nolock) on sdsdg.stagedefinitiongroupkey = sdg.stagedefinitiongroupkey
            where st.genericKey = ? and sdg.GenericKeyTypeKey = ? and sdsdg.stagedefinitionstagedefinitiongroupKey in (:keys)
            order by sd.stagedefinitionKey");

            SimpleQuery<StageTransition_DAO> qSt = new SimpleQuery<StageTransition_DAO>(QueryLanguage.Sql, sql, genericKey, genericKeyTypeKey);
            qSt.AddSqlReturnDefinition(typeof(StageTransition_DAO), "st");
            qSt.SetParameterList("keys", stageDefinitionstageDefinitionGroupKey);

            StageTransition_DAO[] res = qSt.Execute();

            if (res == null || res.Length == 0)
                return stageTransList;

            foreach (StageTransition_DAO st in res)
            {
                stageTransList.Add(BMTM.GetMappedType<IStageTransition, StageTransition_DAO>(st));
            }

            return stageTransList;
        }

        public IEnumerable<IStageTransition> GetStageTransitionsByGenericKey(int genericKey, int stageDefinitionGroupKey)
        {
            IList<IStageTransition> stageTransList = new List<IStageTransition>();
            IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();

            string sql = string.Format(@"select st.*
            from stagetransition st (nolock)
            join stagedefinitionstagedefinitiongroup sdsdg (nolock) on
            st.StageDefinitionStageDefinitionGroupKey = sdsdg.StageDefinitionStageDefinitionGroupKey
            join stagedefinition sd (nolock) on sdsdg.stagedefinitionkey = sd.stagedefinitionkey
            join stagedefinitiongroup sdg (nolock) on sdsdg.stagedefinitiongroupkey = sdg.stagedefinitiongroupkey
            where st.genericKey = ? and sdg.stagedefinitiongroupkey = ? 
            order by sd.stagedefinitionKey");

            SimpleQuery<StageTransition_DAO> simpleQuery = new SimpleQuery<StageTransition_DAO>(QueryLanguage.Sql, sql, genericKey, stageDefinitionGroupKey);
            simpleQuery.AddSqlReturnDefinition(typeof(StageTransition_DAO), "st");

            StageTransition_DAO[] resultSet = simpleQuery.Execute();

            if (resultSet == null || resultSet.Length == 0)
                return stageTransList;

            foreach (StageTransition_DAO stageTransition in resultSet)
            {
                stageTransList.Add(BMTM.GetMappedType<IStageTransition, StageTransition_DAO>(stageTransition));
            }

            return stageTransList;
        }

    }
}