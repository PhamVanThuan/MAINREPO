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
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    ///
    /// </summary>
    [FactoryType(typeof(ICommonRepository))]
    public class CommonRepository : AbstractRepositoryBase, ICommonRepository
    {
        private ICastleTransactionsService castleTransactionService;

        public CommonRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public CommonRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        #region ICommonRepository Members

        public DateTime GetTodaysDate()
        {
            return DateTime.Now;
        }

        // TODO: This shouldn't be here!
        [Obsolete("Use method AbstractRepositoryBase.ExecuteQueryOnCastleTran")]
        public DataTable ExecuteUIStatement(string StatementName, string ApplicationName, List<SqlParameter> Parameters)
        {
            ParameterCollection prms = new ParameterCollection();
            if (Parameters != null)
            {
                foreach (SqlParameter prm in Parameters)
                {
                    prms.Add(prm);
                }
            }

            IDbConnection con = Helper.GetSQLDBConnection();
            DataTable DT = new DataTable();
            Helper.Fill(DT, ApplicationName, StatementName, con, prms);

            return DT;
        }

        // TODO: This shouldn't be here!
        [Obsolete("Use method AbstractRepositoryBase.ExecuteNonQueryOnCastleTran")]
        public void ExecuteNonQuery(string statementName, string application, List<SqlParameter> Parameters)
        {
            string query = UIStatementRepository.GetStatement(application, statementName);

            ParameterCollection prms = new ParameterCollection();
            if (Parameters != null)
            {
                foreach (SqlParameter prm in Parameters)
                {
                    prms.Add(prm);
                }
            }

            using (IDbConnection con = Helper.GetSQLDBConnection())
            {
                Helper.ExecuteNonQuery(con, query, prms);
            }
        }

        // TODO: This shouldn't be here!
        [Obsolete("Use method AbstractRepositoryBase.ExecuteQueryOnCastleTran")]
        public DataSet ExecuteUIStatement(string StatementName, string ApplicationName, List<SqlParameter> Parameters, StringCollection TableMappings)
        {
            ParameterCollection prms = new ParameterCollection();
            if (Parameters != null)
            {
                foreach (SqlParameter prm in Parameters)
                {
                    prms.Add(prm);
                }
            }

            IDbConnection con = Helper.GetSQLDBConnection();
            DataSet ds = new DataSet();
            Helper.Fill(ds, TableMappings, ApplicationName, StatementName, con, prms);

            return ds;
        }

        // TODO: This shouldn't be here!
        [Obsolete("Use method AbstractRepositoryBase.ExecuteNonQueryOnCastleTran")]
        public void ExecuteUIStatementUpdate(DataTable DT, string statementName, string applicationName, List<SqlParameter> Parameters)
        {
            string query = UIStatementRepository.GetStatement("COMMON", statementName);

            ParameterCollection prms = new ParameterCollection();
            if (Parameters != null)
            {
                foreach (SqlParameter prm in Parameters)
                {
                    prms.Add(prm);
                }
            }

            using (IDbConnection con = Helper.GetSQLDBConnection())
            {
                Helper.Update(DT, con, "update", query, prms);
            }
        }

        /// <summary>
        /// Gets an entity by its primary key. This is a useful helper function to use inside any repository to
        /// get any business object by its primary key.
        /// </summary>
        /// <typeparam name="TInterface">The Interface to map to.</typeparam>
        /// <param name="Key">The primary key of the business object to find.</param>
        /// <returns>Returns the business object for the primary key value.</returns>
        public TInterface GetByKey<TInterface>(int Key)
        {
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            Type DAOType = BMTM.GetDAOFromInterface<TInterface>();
            object DAO = ActiveRecordMediator.FindByPrimaryKey(DAOType, Key);

            return BMTM.GetMappedType<TInterface>(DAO);

            //return BMTM.GetMappedType<TInterface, DAOType>(DAO);
            //return default(TInterface);//base.GetByKey<TInterface, TDAO>(Key);
        }

        /// <summary>
        /// Creates and empty business object. This is a useful helper function to use inside any repository to
        /// create an empty business object.
        /// </summary>
        /// <typeparam name="TInterface">The Interface to map to.</typeparam>
        /// <typeparam name="TDAO">The DAO object to map from.</typeparam>
        /// <returns>A new instance of the businessobject wrapped in an interface.</returns>
        public new TInterface CreateEmpty<TInterface, TDAO>() where TDAO : class, new()
        {
            return base.CreateEmpty<TInterface, TDAO>();
        }

        /// <summary>
        /// Saves a business object. This is a useful helper function to use inside any repository to
        /// save any business object.
        /// </summary>
        /// <typeparam name="TInterface">The Interface to map to.</typeparam>
        /// <typeparam name="TDAO">The DAO object to map from.</typeparam>
        /// <param name="ObjectToSave">The object to save to the database.</param>
        public new void Save<TInterface, TDAO>(TInterface ObjectToSave)
            where TInterface : IDAOObject
            where TDAO : class
        {
            base.Save<TInterface, TDAO>(ObjectToSave);

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="StatementDefinitionKey"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public object ExecuteConditionTokenStatement(int StatementDefinitionKey, ParameterCollection Parameters)
        {
            StatementDefinition_DAO def = StatementDefinition_DAO.Find(StatementDefinitionKey);
            string query = UIStatementRepository.GetStatement(def.ApplicationName, def.StatementName);
            return castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(StatementDefinition_DAO), Parameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetLinkRatesByAccountKey(int AccountKey)
        {
            //Account_DAO a;
            //a.OriginationSourceProduct.CreditMatrices;
            //CreditMatrix_DAO cm;
            //cm.CreditCriterias;
            //CreditCriteria_DAO cc;
            //cc.Margin;

            //Margin_DAO m;

            string HQL = "select m from Account_DAO a join a.OriginationSourceProduct.CreditMatrices cm join cm.CreditCriterias cc join cc.Margin m where a.Key = ? group by m.Key, m.Description, m.Value order by m.Description";
            SimpleQuery<Margin_DAO> query = new SimpleQuery<Margin_DAO>(HQL, AccountKey);
            query.SetResultTransformer(new NHibernate.Transform.DistinctRootEntityResultTransformer());
            Margin_DAO[] res = query.Execute();

            Dictionary<int, string> lstRates = new Dictionary<int, string>();
            for (int x = 0; x < res.GetLength(0); x++)
            {
                lstRates.Add(res[x].Key, res[x].Description);
            }
            return lstRates;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="OriginationSourceKey"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetLinkRatesByOriginationSource(int OriginationSourceKey)
        {
            //Account_DAO a;
            //a.OriginationSourceProduct.CreditMatrices;
            //CreditMatrix_DAO cm;
            //cm.CreditCriterias;
            //CreditCriteria_DAO cc;
            //cc.Margin;
            //Margin_DAO m;

            //OriginationSourceProduct_DAO osp;
            //osp.OriginationSource.Key;

            string HQL = "select m from OriginationSourceProduct_DAO osp join osp.CreditMatrices cm join cm.CreditCriterias cc join cc.Margin m where osp.OriginationSource.Key = ? group by m.Key, m.Description, m.Value order by m.Description";
            SimpleQuery<Margin_DAO> query = new SimpleQuery<Margin_DAO>(HQL, OriginationSourceKey);
            query.SetResultTransformer(new NHibernate.Transform.DistinctRootEntityResultTransformer());
            Margin_DAO[] res = query.Execute();

            Dictionary<int, string> lstRates = new Dictionary<int, string>();
            for (int x = 0; x < res.GetLength(0); x++)
            {
                lstRates.Add(res[x].Key, res[x].Description);
            }
            return lstRates;
        }

        public bool IsBusinessDay(DateTime calendarDate)
        {
            bool retVal = true;

            ICalendar cal = Calendar.GetCalendarItemsByDate(calendarDate);

            if (cal != null)
            {
                if (cal.IsHoliday || cal.IsSaturday || cal.IsSunday)
                    retVal = false;
            }
            else
                throw new Exception("Effective Date can not be found on Calendar ");

            return retVal;
        }

        public DateTime GetNextBusinessDay(DateTime date)
        {
            while (!IsBusinessDay(date))
            {
                date = date.AddDays(1);
            }
            return date;
        }

        public DateTime GetnWorkingDaysFromToday(int nDays)
        {
            DateTime dt = DateTime.Now.AddDays(nDays);
            using (new TransactionScope())
            {
                try
                {
                    string sql = string.Format("select top {0} * from [2am]..calendar where ISSaturday<>1 and ISSunday <> 1 and ISHOliday <> 1 and CalendarDate > getdate()", nDays);
                    DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                    dt = Convert.ToDateTime(ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["CalendarDate"]);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Unable to GetnWorkingDaysFromToday() {0}", ex.ToString()));
                }
            }
            return dt;
        }

        public DateTime GetnWorkingDaysFromDate(int nDays, DateTime fromDate)
        {
            DateTime dt = DateTime.Now.AddDays(nDays);
            using (new TransactionScope())
            {
                try
                {
                    string sql = string.Format("select top {0} * from [2am]..calendar where ISSaturday<>1 and ISSunday <> 1 and ISHOliday <> 1 and CalendarDate > '{1}'", nDays, getSQLDate(fromDate));
                    DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                    dt = Convert.ToDateTime(ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["CalendarDate"]);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Unable to GetnWorkingDaysFromDate() {0}", ex.ToString()));
                }
            }
            return dt;
        }

        private string getSQLDate(DateTime dt)
        {
            if (dt != null)
                return String.Format("{0}/{1}/{2}", dt.Year, dt.Month, dt.Day);

            return getSQLDate(DateTime.Now);
        }

        /// <summary>
        /// Remove the object from memory so that we can re-load it as a different type.
        /// </summary>
        /// <param name="businessObject"></param>
        public void ClearFromNHibernateSession(object businessObject)
        {
            IDAOObject daoObject = businessObject as IDAOObject;
            if (daoObject == null)
                return;

            ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = sessionHolder.CreateSession(daoObject.GetDAOObject().GetType());

            session.Clear();
        }

        /// <summary>
        /// Gets a handle on an NHibernate session for the supplied <c>businessObject</c>.
        /// </summary>
        /// <param name="businessObject">The business object you want a session for.</param>
        /// <returns>The NHibernate session.</returns>
        public ISession GetNHibernateSession(object businessObject)
        {
            IDAOObject daoObject = businessObject as IDAOObject;
            return GetNHibernateSession(daoObject.GetDAOObject().GetType());
        }

        public void RefreshDAOObject<TInterface>(int key)
        {
            IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
            Type TDAO = BMTM.GetDAOFromInterface<TInterface>();
            var genericActiveRecordMediator = typeof(ActiveRecordMediator<>).MakeGenericType(TDAO);
            var genericFindByPrimaryKey = genericActiveRecordMediator.GetMethods().Where(x => x.GetParameters().Count() == 1 && x.Name == "FindByPrimaryKey").SingleOrDefault();
            var genericDAOObject = genericFindByPrimaryKey.Invoke(null, new object[] { key });
            ActiveRecordMediator.Refresh(genericDAOObject);
        }

        /// <summary>
        /// Gets a handle on an NHibernate session for the supplied type.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <returns>The NHibernate session.</returns>
        public ISession GetNHibernateSession(Type type)
        {
            ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();

            ISession session = sessionHolder.CreateSession(type);
            return session;
        }

        /// <summary>
        /// Remove the application from memory so that we can re-load it as a different type.
        /// </summary>
        /// <param name="businessObject"></param>
        [Obsolete("Use AttachUnModifiedToCurrentNHibernateSession instead.")]
        public void UpdateInCurrentNHibernateSession(object businessObject)
        {
            IDAOObject daoObject = businessObject as IDAOObject;
            if (daoObject == null)
                return;

            ISession session = GetNHibernateSession(businessObject);

            object dao = daoObject.GetDAOObject();
            if (!session.Contains(dao))
                session.Update(dao);

            //IDAOObject dao = App as IDAOObject;
            //ApplicationUnknown_DAO adao = (ApplicationUnknown_DAO)dao.GetDAOObject();

            //ISession Sess = null;
            //ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();
            //Sess = sessionHolder.CreateSession(typeof(ApplicationUnknown_DAO));

            //Sess.Evict(adao);
        }

        /// <summary>
        /// Remove the application from memory so that we can re-load it as a different type.
        /// </summary>
        /// <param name="businessObject"></param>
        public void AttachUnModifiedToCurrentNHibernateSession(object businessObject)
        {
            IDAOObject daoObject = businessObject as IDAOObject;
            if (daoObject == null)
                return;

            ISession session = GetNHibernateSession(businessObject);

            object dao = daoObject.GetDAOObject();
            if (!session.Contains(dao))
                session.Lock(dao, LockMode.None);
        }

        public void AttachUnModifiedToCurrentNHibernateSession(object businessObject, string Identifier)
        {
            IDAOObject daoObject = businessObject as IDAOObject;
            if (daoObject == null)
                return;

            ISession session = GetNHibernateSession(businessObject);
            object dao = daoObject.GetDAOObject();
            object obj = null;

            foreach (NHibernate.Engine.EntityKey ek in session.Statistics.EntityKeys)
            {
                if (ek.EntityName == dao.GetType().BaseType.FullName && ek.Identifier.ToString() == Identifier)
                {
                    obj = session.Get(ek.EntityName, ek.Identifier);
                    break;
                }
            }

            if (obj != null)
                session.Evict(obj);

            session.Lock(dao, LockMode.None);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public ILanguage GetLanguageByKey(int Key)
        {
            Language_DAO language = Language_DAO.TryFind(Key);
            if (language != null)
            {
                IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<ILanguage, Language_DAO>(language);
            }
            return null;
        }

        /// <summary>
        /// Get Correspondence By Key
        /// </summary>
        /// <param name="correspondenceTemplates"></param>
        /// <returns></returns>
        public ICorrespondenceTemplate GetCorrespondenceTemplateByKey(CorrespondenceTemplates correspondenceTemplates)
        {
            CorrespondenceTemplate_DAO correspondenceTemplate = CorrespondenceTemplate_DAO.TryFind((int)correspondenceTemplates);
            if (correspondenceTemplate != null)
            {
                IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<ICorrespondenceTemplate, CorrespondenceTemplate_DAO>(correspondenceTemplate);
            }
            return null;
        }

        /// <summary>
        /// Get Correspondence By Key
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ICorrespondenceTemplate GetCorrespondenceTemplateByName(string name)
        {
            CorrespondenceTemplate_DAO correspondenceTemplate = CorrespondenceTemplate_DAO.FindAllByProperty("Name", name).FirstOrDefault();
            if (correspondenceTemplate != null)
            {
                IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<ICorrespondenceTemplate, CorrespondenceTemplate_DAO>(correspondenceTemplate);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetDefaultDebitOrderProviderKey()
        {
            using (IDbConnection con = Helper.GetSQLDBConnection())
            {
                string query = UIStatementRepository.GetStatement("Common", "GetDefaultDebitOrderProviderKey");
                ParameterCollection parameters = new ParameterCollection();
                object o = Helper.ExecuteScalar(con, query, parameters);

                if (o != null)
                    return Convert.ToInt32(o);
                else
                    return 0;
            }
        }

        #endregion ICommonRepository Members
    }
}