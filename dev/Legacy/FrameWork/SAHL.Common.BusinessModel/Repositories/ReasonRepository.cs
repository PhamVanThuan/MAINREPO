using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    ///
    /// </summary>
    [FactoryType(typeof(IReasonRepository))]
    public class ReasonRepository : AbstractRepositoryBase, IReasonRepository
    {
        public ReasonRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
            if (lookupRepository == null)
            {
                lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            }
        }

        public ReasonRepository(ICastleTransactionsService castleTransactionService, ILookupRepository lookupRepository)
        {
            this.castleTransactionService = castleTransactionService;
            this.lookupRepository = lookupRepository;
        }

        private ICastleTransactionsService castleTransactionService;
        private ILookupRepository lookupRepository;

        #region IReasonRepository Members

        public IReadOnlyEventList<IReason> GetReasonByGenericKeyAndReasonDefinitionKey(int genericKey, ReasonDefinitions reasonDefinition)
        {
            const string HQL = "SELECT r from Reason_DAO r WHERE r.GenericKey = ? AND r.ReasonDefinition.Key = ?";
            SimpleQuery<Reason_DAO> q = new SimpleQuery<Reason_DAO>(HQL, genericKey, (int)reasonDefinition);
            Reason_DAO[] res = q.Execute();
            IEventList<IReason> list = new DAOEventList<Reason_DAO, IReason, Reason>(res);
            return new ReadOnlyEventList<IReason>(list);
        }

        public IReason GetReasonByKey(int Key)
        {
            return GetByKey<IReason, Reason_DAO>(Key);
        }

        public IReasonType GetReasonTypeByKey(int Key)
        {
            return GetByKey<IReasonType, ReasonType_DAO>(Key);
        }

        public int GetNextReasonDescriptionKey()
        {
            const string HQL = "SELECT r from ReasonDescription_DAO r order by 1 DESC";
            SimpleQuery<ReasonDescription_DAO> q = new SimpleQuery<ReasonDescription_DAO>(HQL);
            q.SetQueryRange(1);
            ReasonDescription_DAO[] res = q.Execute();
            return res[0].Key + 1;
        }

        public IReadOnlyEventList<IReason> GetReasonByGenericTypeAndKey(int GenericKeyTypeKey, int GenericKey)
        {
            const string HQL = "SELECT r from Reason_DAO r WHERE r.GenericKey = ? AND r.ReasonDefinition.ReasonType.GenericKeyType.Key = ?";
            SimpleQuery<Reason_DAO> q = new SimpleQuery<Reason_DAO>(HQL, GenericKey, GenericKeyTypeKey);
            Reason_DAO[] res = q.Execute();
            IEventList<IReason> list = new DAOEventList<Reason_DAO, IReason, Reason>(res);
            return new ReadOnlyEventList<IReason>(list);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="ReasonGroupTypeKey"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IReason> GetReasonByGenericKeyAndReasonGroupTypeKey(int GenericKey, int ReasonGroupTypeKey)
        {
            const string HQL = "from Reason_DAO r WHERE r.GenericKey = ? AND r.ReasonDefinition.ReasonType.ReasonTypeGroup.Key = ?";
            SimpleQuery<Reason_DAO> q = new SimpleQuery<Reason_DAO>(HQL, GenericKey, ReasonGroupTypeKey);
            Reason_DAO[] res = q.Execute();
            IEventList<IReason> list = new DAOEventList<Reason_DAO, IReason, Reason>(res);
            return new ReadOnlyEventList<IReason>(list);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKeyList"></param>
        /// <param name="ReasonTypeGroupKey"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IReason> GetReasonByGenericKeyListAndReasonTypeGroupKey(List<int> GenericKeyList, int ReasonTypeGroupKey)
        {
            const string HQL = "from Reason_DAO r WHERE r.GenericKey in (:genericKeys) AND r.ReasonDefinition.ReasonType.ReasonTypeGroup.Key = :reasonTypeGroupKey";
            SimpleQuery<Reason_DAO> q = new SimpleQuery<Reason_DAO>(HQL);
            q.SetParameterList("genericKeys", GenericKeyList);
            q.SetParameter("reasonTypeGroupKey", ReasonTypeGroupKey);
            Reason_DAO[] res = q.Execute();
            IEventList<IReason> list = new DAOEventList<Reason_DAO, IReason, Reason>(res);
            return new ReadOnlyEventList<IReason>(list);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKeyList"></param>
        /// <param name="ReasonTypeKey"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IReason> GetReasonByGenericKeyListAndReasonTypeKey(List<int> GenericKeyList, int ReasonTypeKey)
        {
            const string HQL = "from Reason_DAO r WHERE r.GenericKey in (:genericKeys) AND r.ReasonDefinition.ReasonType.Key = :reasonTypeKey";
            SimpleQuery<Reason_DAO> q = new SimpleQuery<Reason_DAO>(HQL);
            q.SetParameterList("genericKeys", GenericKeyList);
            q.SetParameter("reasonTypeKey", ReasonTypeKey);
            Reason_DAO[] res = q.Execute();
            IEventList<IReason> list = new DAOEventList<Reason_DAO, IReason, Reason>(res);
            return new ReadOnlyEventList<IReason>(list);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IReason CreateEmptyReason()
        {
            return CreateEmpty<IReason, Reason_DAO>();
        }

        public IReasonDescription CreateEmptyReasonDescription()
        {
            return CreateEmpty<IReasonDescription, ReasonDescription_DAO>();
        }

        public IReasonDefinition CreateEmptyReasonDefinition()
        {
            return CreateEmpty<IReasonDefinition, ReasonDefinition_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reason"></param>
        ///
        public void SaveReason(IReason reason)
        {
            Save<IReason, Reason_DAO>(reason);
        }

        public void SaveReasonDescription(IReasonDescription reasondescription)
        {
            Save<IReasonDescription, ReasonDescription_DAO>(reasondescription);
        }

        public void SaveReasonDefinition(IReasonDefinition reasondefinition)
        {
            Save<IReasonDefinition, ReasonDefinition_DAO>(reasondefinition);
        }

        public IReasonDefinition AddNewReasonDefinition(string description, ReasonTypes reasonType)
        {
            //Create Description
            var reasonDescription = CreateEmptyReasonDescription();
            reasonDescription.Description = description;
            reasonDescription.TranslatableItem = null;
            reasonDescription.Key = GetNextReasonDescriptionKey();
            SaveReasonDescription(reasonDescription);

            //Create Definition
            var reasonDefinition = CreateEmptyReasonDefinition();
            reasonDefinition.AllowComment = false;
            reasonDefinition.EnforceComment = false;
            reasonDefinition.GeneralStatus = lookupRepository.GeneralStatuses[GeneralStatuses.Active];
            //reasonDefinition.OriginationSourceProducts = ;
            reasonDefinition.ReasonDescription = reasonDescription;
            reasonDefinition.ReasonType = lookupRepository.ReasonTypes.ObjectDictionary[Convert.ToString((int)reasonType)];
            SaveReasonDefinition(reasonDefinition);

            return reasonDefinition;
        }

        /// <summary>
        /// Gets an IReasonDefinition for a given ReasonDefinitionKey
        /// </summary>
        /// <param name="Key"></param>
        /// <returns>IReasonDefinition</returns>
        public IReasonDefinition GetReasonDefinitionByKey(int Key)
        {
            return GetByKey<IReasonDefinition, ReasonDefinition_DAO>(Key);
        }

        /// <summary>
        /// Gets a collection of ReasonDefinitions for a given ReasonType and ReasonDescription
        /// </summary>s
        /// <param name="reasonType">SAHL.Common.Globals.ReasonTypes</param>
        /// <param name="reasonDescription">string</param>
        /// <returns>IReasonDefinition</returns>
        public IReadOnlyEventList<IReasonDefinition> GetReasonDefinitionsByReasonDescription(ReasonTypes reasonType, string reasonDescription)
        {
            const string HQL = "from ReasonDefinition_DAO rd where rd.ReasonType.Key = ? and rd.ReasonDescription.Description = ?";
            int reasonTypeKey = Convert.ToInt32(reasonType);
            SimpleQuery<ReasonDefinition_DAO> q = new SimpleQuery<ReasonDefinition_DAO>(HQL, reasonTypeKey, reasonDescription);
            ReasonDefinition_DAO[] res = q.Execute();

            IEventList<IReasonDefinition> list = new DAOEventList<ReasonDefinition_DAO, IReasonDefinition, ReasonDefinition>(res);
            return new ReadOnlyEventList<IReasonDefinition>(list);
        }

        /// <summary>
        /// Gets a collection of ReasonDefinitions for a given ReasonType and ReasonDescription
        /// </summary>
        /// <param name="reasonType">SAHL.Common.Globals.ReasonTypes</param>
        /// <param name="reasonDescriptionKey">int</param>
        /// <returns>IReasonDefinition</returns>
        public IReadOnlyEventList<IReasonDefinition> GetReasonDefinitionsByReasonDescriptionKey(ReasonTypes reasonType, int reasonDescriptionKey)
        {
            const string HQL = "from ReasonDefinition_DAO rd where rd.ReasonType.Key = ? and rd.ReasonDescription.Key = ?";
            int reasonTypeKey = Convert.ToInt32(reasonType);
            SimpleQuery<ReasonDefinition_DAO> q = new SimpleQuery<ReasonDefinition_DAO>(HQL, reasonTypeKey, reasonDescriptionKey);
            ReasonDefinition_DAO[] res = q.Execute();

            IEventList<IReasonDefinition> list = new DAOEventList<ReasonDefinition_DAO, IReasonDefinition, ReasonDefinition>(res);
            return new ReadOnlyEventList<IReasonDefinition>(list);
        }

        /// <summary>
        /// Gets a collection of ReasonDefinitions for a given ReasonType
        /// </summary>
        /// <param name="reasonType">SAHL.Common.Globals.ReasonTypes</param>
        /// <returns>IReadOnlyEventList&lt;IReasonDefinition&gt;</returns>
        public IReadOnlyEventList<IReasonDefinition> GetReasonDefinitionsByReasonType(ReasonTypes reasonType)
        {
            const string HQL = "from ReasonDefinition_DAO rd where rd.ReasonType.Key = ?";
            int reasonTypeKey = Convert.ToInt32(reasonType);
            SimpleQuery<ReasonDefinition_DAO> q = new SimpleQuery<ReasonDefinition_DAO>(HQL, reasonTypeKey);

            ReasonDefinition_DAO[] res = q.Execute();

            IEventList<IReasonDefinition> list = new DAOEventList<ReasonDefinition_DAO, IReasonDefinition, ReasonDefinition>(res);
            return new ReadOnlyEventList<IReasonDefinition>(list);
        }

        /// <summary>
        /// Gets a collection of ReasonDefinitions for a given ReasonTypeKey
        /// </summary>
        /// <param name="reasonTypeKey"></param>
        /// <param name="sortByDescription">Order the reasons by the ReasonDescription ASC</param>
        /// <returns>IReadOnlyEventList&lt;IReasonDefinition&gt;</returns>
        public IReadOnlyEventList<IReasonDefinition> GetReasonDefinitionsByReasonTypeKey(int reasonTypeKey, bool sortByDescription)
        {
            string HQL = "from ReasonDefinition_DAO rd where rd.ReasonType.Key = ? and rd.GeneralStatus.Key = 1";
            if (sortByDescription)
                HQL += " order by rd.ReasonDescription.Description ";

            SimpleQuery<ReasonDefinition_DAO> q = new SimpleQuery<ReasonDefinition_DAO>(HQL, reasonTypeKey);

            ReasonDefinition_DAO[] res = q.Execute();

            IEventList<IReasonDefinition> list = new DAOEventList<ReasonDefinition_DAO, IReasonDefinition, ReasonDefinition>(res);
            return new ReadOnlyEventList<IReasonDefinition>(list);
        }

        /// <summary>
        /// Gets a collection of ReasonDefinitions for a given ReasonTypeKey
        /// </summary>
        /// <param name="reasonTypeKey"></param>
        /// <returns>IReadOnlyEventList&lt;IReasonDefinition&gt;</returns>
        public IReadOnlyEventList<IReasonDefinition> GetReasonDefinitionsByReasonTypeKey(int reasonTypeKey)
        {
            return GetReasonDefinitionsByReasonTypeKey(reasonTypeKey, false);
        }

        /// <summary>
        /// Gets a collection of ReasonDefinitions for a given ReasonTypeGroup
        /// </summary>
        /// <param name="reasonTypeGroup">SAHL.Common.Globals.ReasonTypeGroups</param>
        /// <returns>IReadOnlyEventList&lt;IReasonDefinition&gt;</returns>
        public IReadOnlyEventList<IReasonDefinition> GetReasonDefinitionsByReasonTypeGroup(ReasonTypeGroups reasonTypeGroup)
        {
            const string HQL = "from ReasonDefinition_DAO rd where rd.ReasonType.ReasonTypeGroup.Key = ?";
            int reasonTypeGroupKey = Convert.ToInt32(reasonTypeGroup);
            SimpleQuery<ReasonDefinition_DAO> q = new SimpleQuery<ReasonDefinition_DAO>(HQL, reasonTypeGroupKey);

            ReasonDefinition_DAO[] res = q.Execute();

            IEventList<IReasonDefinition> list = new DAOEventList<ReasonDefinition_DAO, IReasonDefinition, ReasonDefinition>(res);
            return new ReadOnlyEventList<IReasonDefinition>(list);
        }

        public IReadOnlyEventList<IReasonType> GetReasonTypeByReasonTypeGroup(int[] ReasonTypeGroupKeys)
        {
            if (ReasonTypeGroupKeys.Length == 0)
                throw new Exception("At least one ReasonTypeGroupKey must be provided.");

            const string HQL = "Select rt from ReasonType_DAO rt join rt.ReasonTypeGroup as rtg where rtg.Key in (:keys)";
            SimpleQuery<ReasonType_DAO> q = new SimpleQuery<ReasonType_DAO>(HQL);
            q.SetParameterList("keys", ReasonTypeGroupKeys);
            ReasonType_DAO[] res = q.Execute();

            IEventList<IReasonType> list = new DAOEventList<ReasonType_DAO, IReasonType, ReasonType>(res);
            return new ReadOnlyEventList<IReasonType>(list);
        }

        public IReadOnlyEventList<IReason> GetReasonByGenericKeyAndReasonTypeKey(int GenericKey, int ReasonTypeKey)
        {
            const string HQL = "from Reason_DAO r WHERE r.GenericKey = ? AND r.ReasonDefinition.ReasonType.Key = ?";
            SimpleQuery<Reason_DAO> q = new SimpleQuery<Reason_DAO>(HQL, GenericKey, ReasonTypeKey);
            Reason_DAO[] res = q.Execute();
            IEventList<IReason> list = new DAOEventList<Reason_DAO, IReason, Reason>(res);
            return new ReadOnlyEventList<IReason>(list);
        }

        public IReason GetLatestReasonByGenericKeyAndReasonTypeKey(int GenericKey, int ReasonTypeKey)
        {
            const string HQL = "from Reason_DAO r WHERE r.GenericKey = ? AND r.ReasonDefinition.ReasonType.Key = ? ORDER BY r.Key desc";
            SimpleQuery<Reason_DAO> q = new SimpleQuery<Reason_DAO>(HQL, GenericKey, ReasonTypeKey);
            q.SetQueryRange(1);
            Reason_DAO[] res = q.Execute();
            if (res.Length > 0)
                return new Reason(res[0]);
            else
                return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <returns></returns>
        public int GetLatestReasonDescriptionKeyForGenericKey(int genericKey, int genericKeyTypeKey)
        {
            string query = UIStatementRepository.GetStatement("Repositories.ReasonRepository", "GetLatestReasonDescriptionKeyForGenericKey");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@GenericKey", genericKey));
            prms.Add(new SqlParameter("@GenericKeyTypeKey", genericKeyTypeKey));

            object obj = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), prms);

            if (obj != DBNull.Value && obj != null)
                return Convert.ToInt32(obj);
            else
                return -1;
        }

        /// <summary>
        /// Get a complete list of reasons for display
        /// </summary>
        /// <param name="GenericKeys"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IReason> GeReasonsByGenericKeys(int[] GenericKeys)
        {
            const string HQL = "from Reason_DAO r WHERE r.GenericKey in (:keys)";
            SimpleQuery<Reason_DAO> q = new SimpleQuery<Reason_DAO>(HQL);
            q.SetParameterList("keys", GenericKeys);
            Reason_DAO[] res = q.Execute();
            IEventList<IReason> list = new DAOEventList<Reason_DAO, IReason, Reason>(res);
            return new ReadOnlyEventList<IReason>(list);
        }

        /// <summary>
        /// Get a complete list of reasons per GenericKeyType and GenericKeys for display
        /// </summary>
        /// <param name="dtGenericKeysAndTypes"></param>
        /// <returns></returns>
        public IEventList<IReason> GetReasonsByGenericKeyTypeAndKeys(DataTable dtGenericKeysAndTypes)
        {
            IEventList<IReason> lstCombinedReasons = new EventList<IReason>();

            DataTable distinctTable = dtGenericKeysAndTypes.DefaultView.ToTable(true);

            foreach (DataRow dr in distinctTable.Rows)
            {
                Int32 genericKey = Convert.ToInt32(dr[0]);
                Int32 genericKeyTypeKey = Convert.ToInt32(dr[1]); ;
                const string HQL = "from Reason_DAO r WHERE r.GenericKey = :GenericKey AND r.ReasonDefinition.ReasonType.GenericKeyType.Key = :GenericKeyTypeKey";
                SimpleQuery<Reason_DAO> q = new SimpleQuery<Reason_DAO>(HQL);
                q.SetParameter("GenericKey", genericKey);
                q.SetParameter("GenericKeyTypeKey", genericKeyTypeKey);
                Reason_DAO[] res = q.Execute();
                IEventList<IReason> list1 = new DAOEventList<Reason_DAO, IReason, Reason>(res);

                foreach (IReason reason in list1)
                {
                    lstCombinedReasons.Add(new DomainMessageCollection(), reason);
                }
            }

            return lstCombinedReasons;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reason"></param>
        public void DeleteReason(IReason reason)
        {
            Reason_DAO dao = (Reason_DAO)((IDAOObject)reason).GetDAOObject();
            dao.DeleteAndFlush();
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// Get a List of Reasons by Stage Transition Keys
        /// </summary>
        /// <param name="stageTransitionKeys"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IReason> GetReasonsByStageTransitionKeys(int[] stageTransitionKeys)
        {
            const string query = "from Reason_DAO r WHERE r.StageTransition.Key in (:keys)";
            SimpleQuery<Reason_DAO> q = new SimpleQuery<Reason_DAO>(query);
            q.SetParameterList("keys", stageTransitionKeys);
            Reason_DAO[] res = q.Execute();
            IEventList<IReason> list = new DAOEventList<Reason_DAO, IReason, Reason>(res);
            return new ReadOnlyEventList<IReason>(list);
        }

        #endregion IReasonRepository Members
    }
}