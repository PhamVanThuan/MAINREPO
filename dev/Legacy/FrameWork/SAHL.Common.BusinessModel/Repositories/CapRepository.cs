using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;
using NHibernate;
using SAHL.Common;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(ICapRepository))]
    public class CapRepository : AbstractRepositoryBase, ICapRepository
    {
        public CapRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public CapRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        #region ICapRepository Members

        public List<IAccount> CapAccountSearch(int accountKey)
        {
            List<IAccount> accLst = new List<IAccount>();
            string sql = UIStatementRepository.GetStatement("Repositories.CapRepository", "CapAccountSearch");
            SimpleQuery<Account_DAO> q = new SimpleQuery<Account_DAO>(QueryLanguage.Sql, sql, accountKey);
            q.AddSqlReturnDefinition(typeof(Account_DAO), "a");
            Account_DAO[] res = q.Execute();

            if (res != null && res.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                foreach (Account_DAO acc in res)
                {
                    accLst.Add(BMTM.GetMappedType<IAccount, Account_DAO>(acc));
                }
            }
            return accLst;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IList<ICapPaymentOption> GetCapPaymentOptions()
        {
            IList<ICapPaymentOption> list = new List<ICapPaymentOption>();
            CapPaymentOption_DAO[] res = CapPaymentOption_DAO.FindAll();

            if (res != null && res.Length > 0)
            {
                for (int i = 0; i < res.Length; i++)
                {
                    list.Add(new CapPaymentOption(res[i]));
                }
            }
            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public ICapApplication GetCapOfferByKey(int Key)
        {
            return base.GetByKey<ICapApplication, CapApplication_DAO>(Key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public IList<ICapApplication> GetCapOfferByAccountKey(int accountKey)
        {
            string HQL = "from CapApplication_DAO capp where capp.Account.Key = ? ";
            SimpleQuery<CapApplication_DAO> q = new SimpleQuery<CapApplication_DAO>(HQL, accountKey);
            CapApplication_DAO[] res = q.Execute();

            IList<ICapApplication> retval = new List<ICapApplication>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new CapApplication(res[i]));
            }
            return retval;
        }

        public ICapApplication GetCapApplicationFromInstance(IInstance instance)
        {
            ICapApplication capApplication = null;

            string sql = "SELECT o.* FROM [x2].[X2DATA]." + instance.WorkFlow.StorageTable + " xd (nolock) ";
            sql += " JOIN [2am].[dbo].CapOffer o (nolock) ON o.CapOfferKey = xd." + instance.WorkFlow.StorageKey;
            sql += " WHERE xd.InstanceID = " + instance.ID;

            ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = sessionHolder.CreateSession(typeof(CapApplication_DAO));
            IQuery sqlQuery = session.CreateSQLQuery(sql).AddEntity(typeof(CapApplication_DAO));
            sqlQuery.SetMaxResults(1);

            IList<CapApplication_DAO> capApplications = sqlQuery.List<CapApplication_DAO>();

            if (capApplications != null && capApplications.Count > 0)
            {
                IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                capApplication = bmtm.GetMappedType<ICapApplication>(capApplications[0]);
            }
            return capApplication;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="statusKey"></param>
        /// <returns></returns>
        public IList<ICapApplication> GetCapOfferByAccountKeyAndStatus(int accountKey, int statusKey)
        {
            string HQL = "from CapApplication_DAO capp where capp.Account.Key = ? and capp.CapStatus.Key = ?";
            SimpleQuery<CapApplication_DAO> q = new SimpleQuery<CapApplication_DAO>(HQL, accountKey, statusKey);
            CapApplication_DAO[] res = q.Execute();

            IList<ICapApplication> retval = new List<ICapApplication>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new CapApplication(res[i]));
            }
            return retval;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IList<ICapNTUReason> GetCapNTUReasons()
        {
            string HQL = "from CapNTUReason_DAO c";
            SimpleQuery<CapNTUReason_DAO> q = new SimpleQuery<CapNTUReason_DAO>(HQL);
            CapNTUReason_DAO[] res = q.Execute();

            IList<ICapNTUReason> retval = new List<ICapNTUReason>();

            for (int i = 0; i < res.Length; i++)
            {
                if (!res[i].Description.StartsWith("Decline"))
                    retval.Add(new CapNTUReason(res[i]));
            }
            return retval;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IList<ICapNTUReason> GetCapDeclineReasons()
        {
            string HQL = "from CapNTUReason_DAO c";
            SimpleQuery<CapNTUReason_DAO> q = new SimpleQuery<CapNTUReason_DAO>(HQL);
            CapNTUReason_DAO[] res = q.Execute();

            IList<ICapNTUReason> retval = new List<ICapNTUReason>();

            for (int i = 0; i < res.Length; i++)
            {
                if (res[i].Description.StartsWith("Decline"))
                    retval.Add(new CapNTUReason(res[i]));
            }
            return retval;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IList<ICancellationReason> GetCapCancellationReasons()
        {
            string HQL = "from CancellationReason_DAO c";
            SimpleQuery<CancellationReason_DAO> q = new SimpleQuery<CancellationReason_DAO>(HQL);
            CancellationReason_DAO[] res = q.Execute();

            IList<ICancellationReason> retval = new List<ICancellationReason>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new CancellationReason(res[i]));
            }
            return retval;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DataTable GetCurrentCAPResetConfigDates()
        {
            string query = UIStatementRepository.GetStatement("COMMON", "GetCurrentCAPResetConfigDates");
            ParameterCollection parameters = new ParameterCollection();
            DataTable DT = new DataTable();
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(CapApplication_DAO), parameters);

            if (ds.Tables.Count > 0)
                DT = ds.Tables[0];

            return DT;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resetDate"></param>
        /// <returns></returns>
        public IList<ICapTypeConfiguration> GetCapTypeConfigByResetDate(DateTime resetDate)
        {
            string HQL = "from CapTypeConfiguration_DAO ctc where ctc.ResetDate = ?";
            SimpleQuery<CapTypeConfiguration_DAO> q = new SimpleQuery<CapTypeConfiguration_DAO>(HQL, resetDate);
            CapTypeConfiguration_DAO[] res = q.Execute();

            IList<ICapTypeConfiguration> retval = new List<ICapTypeConfiguration>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new CapTypeConfiguration(res[i]));
            }
            return retval;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="capConfigKey"></param>
        /// <returns></returns>
        public ICapTypeConfiguration GetCapTypeConfigByKey(int capConfigKey)
        {
            return base.GetByKey<ICapTypeConfiguration, CapTypeConfiguration_DAO>(capConfigKey);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="capTypeConfiguration"></param>
        public void SaveCapTypeConfiguration(ICapTypeConfiguration capTypeConfiguration)
        {
            base.Save<ICapTypeConfiguration, CapTypeConfiguration_DAO>(capTypeConfiguration);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="capTypeConfigurationDetail"></param>
        public void SaveCapTypeConfigurationDetail(ICapTypeConfigurationDetail capTypeConfigurationDetail)
        {
            base.Save<ICapTypeConfigurationDetail, CapTypeConfigurationDetail_DAO>(capTypeConfigurationDetail);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="capApplication"></param>
        public void SaveCapApplication(ICapApplication capApplication)
        {
            base.Save<ICapApplication, CapApplication_DAO>(capApplication);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IList<ICapType> GetCapTypes()
        {
            string HQL = "from CapType_DAO c";
            SimpleQuery<CapType_DAO> q = new SimpleQuery<CapType_DAO>(HQL);
            CapType_DAO[] res = q.Execute();

            IList<ICapType> retval = new List<ICapType>();

            for (int i = 0; i < res.Length; i++)
            {
                if (res[i].Key <= 3)
                    retval.Add(new CapType(res[i]));
            }
            return retval;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resetDate"></param>
        /// <param name="resetConfigKey"></param>
        /// <returns></returns>
        public IReset GetPreviousResetByResetDateAndRCKey(DateTime resetDate, int resetConfigKey)
        {
            string HQL = "select r from Reset_DAO r where r.ResetDate < ? and r.ResetConfiguration.Key = ? order by r.ResetDate desc";
            SimpleQuery<Reset_DAO> q = new SimpleQuery<Reset_DAO>(HQL, resetDate, resetConfigKey);
            object o = Reset_DAO.ExecuteQuery(q);
            Reset_DAO[] reset = o as Reset_DAO[];
            if (reset != null && reset.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IReset, Reset_DAO>(reset[0]);
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ICapApplication CreateCapApplication()
        {
            return base.CreateEmpty<ICapApplication, CapApplication_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ICapApplicationDetail CreateCapApplicationDetail()
        {
            return base.CreateEmpty<ICapApplicationDetail, CapApplicationDetail_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ICapTypeConfiguration CreateCapTypeConfiguration()
        {
            return base.CreateEmpty<ICapTypeConfiguration, CapTypeConfiguration_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ICapTypeConfigurationDetail CreateCapTypeConfigurationDetail()
        {
            return base.CreateEmpty<ICapTypeConfigurationDetail, CapTypeConfigurationDetail_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="cancellationReasonKey"></param>
        /// <param name="userid"></param>
        public void OptOutCAP(int accountkey, int cancellationReasonKey, string userid)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountkey));
            prms.Add(new SqlParameter("@CancellationReasonKey", cancellationReasonKey));
            prms.Add(new SqlParameter("@UserID", userid));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.CAPRepository", "OptOutCAP", prms);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IList<IBroker> GetCapBrokers()
        {
            IList<IBroker> brokerList = new List<IBroker>();

            // In the event of more role type being added
            List<int> ortKeyList = new List<int>();
            ortKeyList.Add((int)OfferRoleTypes.CAPConsultant);

            string sql = @"select distinct [br].*
			from [2am].[dbo].[OfferRoleTypeOrganisationStructureMapping] ortosm
			inner join [2am].[dbo].[UserOrganisationStructure] uos
			on uos.OrganisationStructureKey = ortosm.OrganisationStructureKey
			inner join [2am].[dbo].[aduser] ad
			on ad.ADUserKey = uos.ADUserKey
			inner join [2am].[dbo].[broker] br
			on br.ADUserKey = ad.ADUserKey
			left join [2am].[dbo].[CapCreditBrokerToken] ccbr
			on br.BrokerKey = ccbr.BrokerKey
			where ortosm.OfferRoleTypeKey in (:ortKeys) and ad.generalstatuskey = 1 and ccbr.BrokerKey is null";

            SimpleQuery<Broker_DAO> brQ = new SimpleQuery<Broker_DAO>(QueryLanguage.Sql, sql);
            brQ.SetParameterList("ortKeys", ortKeyList);
            brQ.AddSqlReturnDefinition(typeof(Broker_DAO), " r");
            Broker_DAO[] res = brQ.Execute();

            foreach (Broker_DAO broker in res)
            {
                brokerList.Add(new Broker(broker));
            }

            return brokerList;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        public IList<IBroker> GetCapCreditBrokers()
        {
            IList<IBroker> brokerList = new List<IBroker>();

            // Filter these out by brokers listed in the
            // CapCreditBrokerToken table
            string sql = @"select distinct [br].*
			from [2am].[dbo].[UserOrganisationStructure] uos
			inner join [2am].[dbo].[aduser] ad
			on ad.ADUserKey = uos.ADUserKey
			inner join [2am].[dbo].[broker] br
			on br.ADUserKey = ad.ADUserKey
			inner join [2am].[dbo].[CapCreditBrokerToken] ccbr
			on br.BrokerKey = ccbr.BrokerKey
			where ad.generalstatuskey = 1 and uos.OrganisationStructureKey in (2017)";

            SimpleQuery<Broker_DAO> brQ = new SimpleQuery<Broker_DAO>(QueryLanguage.Sql, sql);
            brQ.AddSqlReturnDefinition(typeof(Broker_DAO), "br");
            Broker_DAO[] res = brQ.Execute();

            foreach (Broker_DAO broker in res)
            {
                brokerList.Add(new Broker(broker));
            }

            return brokerList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="brokerKey"></param>
        /// <returns></returns>
        public DataTable GetCapOffersByBrokerKey(int brokerKey)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "GetCapOfferBrokerView");
            ParameterCollection parameters = new ParameterCollection();

            //SAHL.Common.DataAccess.Helper.AddIntParameter(parameters, "@BrokerKey", brokerKey);
            parameters.Add(new SqlParameter("@BrokerKey", brokerKey));
            DataTable DT = new DataTable();
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(CapApplication_DAO), parameters);

            if (ds.Tables.Count > 0)
                DT = ds.Tables[0];

            return DT;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="brokerKey"></param>
        /// <returns></returns>
        public DataTable GetCapCreditOffersByBrokerKey(int brokerKey)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "GetCapOfferCreditBrokerView");
            ParameterCollection parameters = new ParameterCollection();

            //SAHL.Common.DataAccess.Helper.AddIntParameter(parameters, "@BrokerKey", brokerKey);
            parameters.Add(new SqlParameter("@BrokerKey", brokerKey));
            DataTable DT = new DataTable();
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(CapApplication_DAO), parameters);

            if (ds.Tables.Count > 0)
                DT = ds.Tables[0];

            return DT;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="brokerKey"></param>
        /// <returns></returns>
        public IBroker GetBrokerByBrokerKey(int brokerKey)
        {
            string HQL = "select b from Broker_DAO b where b.Key = ?";
            SimpleQuery<Broker_DAO> q = new SimpleQuery<Broker_DAO>(HQL, brokerKey);
            object o = Broker_DAO.ExecuteQuery(q);
            Broker_DAO[] broker = o as Broker_DAO[];
            if (broker != null && broker.Length == 1)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IBroker, Broker_DAO>(broker[0]);
            }
            return null;
        }

        /// <summary>
        /// Gets a broker by the full name.  This is used for the Cap Import report, and unfortunately doesn't take
        /// duplicate names into account - it just returns the first broker found matching the name.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        /// <remarks>This is not case-sensitive.</remarks>
        public IBroker GetBrokerByFullName(string fullName)
        {
            Broker_DAO[] brokers = Broker_DAO.FindAllByProperty("FullName", fullName);
            if (brokers.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IBroker, Broker_DAO>(brokers[0]);
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="adUserKey"></param>
        /// <returns></returns>
        public IBroker GetBrokerByADUserKey(int adUserKey)
        {
            string HQL = "select b from Broker_DAO b where b.ADUser.Key = ?";
            SimpleQuery<Broker_DAO> q = new SimpleQuery<Broker_DAO>(HQL, adUserKey);
            object o = Broker_DAO.ExecuteQuery(q);
            Broker_DAO[] broker = o as Broker_DAO[];
            if (broker != null && broker.Length == 1)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IBroker, Broker_DAO>(broker[0]);
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="capOfferKey"></param>
        /// <param name="brokerKey"></param>
        // TODO: This transaction needs to be removed
        public void UpdateX2CapCaseData(int capOfferKey, int brokerKey)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "UpdateX2CapCaseData");
            ParameterCollection parameters = new ParameterCollection();

            //SAHL.Common.DataAccess.Helper.AddIntParameter(parameters, "@CapOfferKey", capOfferKey);
            //SAHL.Common.DataAccess.Helper.AddIntParameter(parameters, "@BrokerKey", brokerKey);
            parameters.Add(new SqlParameter("@CapOfferKey", capOfferKey));
            parameters.Add(new SqlParameter("@BrokerKey", brokerKey));
            castleTransactionService.ExecuteNonQueryOnCastleTran(query, typeof(CapApplication_DAO), parameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="capOfferKey"></param>
        /// <param name="brokerKey"></param>
        // TODO: This transaction needs to be removed
        public void UpdateX2CapCreditCaseData(int capOfferKey, int brokerKey)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "UpdateX2CapCreditCaseData");
            ParameterCollection parameters = new ParameterCollection();

            //SAHL.Common.DataAccess.Helper.AddIntParameter(parameters, "@CapOfferKey", capOfferKey);
            //SAHL.Common.DataAccess.Helper.AddIntParameter(parameters, "@BrokerKey", brokerKey);
            parameters.Add(new SqlParameter("@CapOfferKey", capOfferKey));
            parameters.Add(new SqlParameter("@BrokerKey", brokerKey));
            castleTransactionService.ExecuteNonQueryOnCastleTran(query, typeof(CapApplication_DAO), parameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public IList<ICapApplication> GetAcceptedHistoryForCancel(int accountKey)
        {
            string HQL = "from CapApplication_DAO capp where capp.Account.Key = ? and capp.CapStatus.Key = 2 and capp.CapTypeConfiguration.CapEffectiveDate < ? and capp.CapTypeConfiguration.CapClosureDate > ?";
            SimpleQuery<CapApplication_DAO> q = new SimpleQuery<CapApplication_DAO>(HQL, accountKey, DateTime.Now, DateTime.Now);
            CapApplication_DAO[] res = q.Execute();

            IList<ICapApplication> retval = new List<ICapApplication>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new CapApplication(res[i]));
            }
            return retval;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resetConfigKey"></param>
        /// <returns></returns>
        public ICapTypeConfiguration GetCurrentCapTypeConfigByResetConfigKey(int resetConfigKey)
        {
            DateTime today = DateTime.Now;
            string HQL = "select ct from CapTypeConfiguration_DAO ct where ct.ApplicationStartDate <= ? and ct.ApplicationEndDate >= ? and ct.ResetConfiguration.Key = ? and ct.GeneralStatus.Key = 1 order by ct.ApplicationStartDate";
            SimpleQuery<CapTypeConfiguration_DAO> q = new SimpleQuery<CapTypeConfiguration_DAO>(HQL, today, today, resetConfigKey);
            object o = CapTypeConfiguration_DAO.ExecuteQuery(q);
            CapTypeConfiguration_DAO[] captypeConfig = o as CapTypeConfiguration_DAO[];
            if (captypeConfig != null && captypeConfig.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<ICapTypeConfiguration, CapTypeConfiguration_DAO>(captypeConfig[0]);
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string GetNextCapCreditBroker()
        {
            string sql = @"select distinct [ccbr].*
			from [2am].[dbo].[UserOrganisationStructure] uos
			inner join [2am].[dbo].[aduser] ad
			on ad.ADUserKey = uos.ADUserKey
			inner join [2am].[dbo].[broker] br
			on br.ADUserKey = ad.ADUserKey
			inner join [2am].[dbo].[CapCreditBrokerToken] ccbr
			on br.BrokerKey = ccbr.BrokerKey
			where ad.generalstatuskey = 1 and uos.OrganisationStructureKey in (2017)";

            SimpleQuery<CapCreditBrokerToken_DAO> brQ = new SimpleQuery<CapCreditBrokerToken_DAO>(QueryLanguage.Sql, sql);
            brQ.AddSqlReturnDefinition(typeof(CapCreditBrokerToken_DAO), "ccbr");
            CapCreditBrokerToken_DAO[] res = brQ.Execute();

            string retval = "";
            bool foundBroker = false;
            if (res.Length > 0)
            {
                for (int i = 0; i < res.Length; i++)
                {
                    if (res[i].LastAssigned == true)
                    {
                        foundBroker = true;
                        res[i].LastAssigned = false;
                        res[i].SaveAndFlush();
                        if (res.Length > i + 1)
                        {
                            res[i + 1].LastAssigned = true;
                            retval = res[i + 1].Broker.ADUser.ADUserName;
                            res[i + 1].SaveAndFlush();
                        }
                        else
                        {
                            res[0].LastAssigned = true;
                            retval = res[0].Broker.ADUser.ADUserName;
                            res[0].SaveAndFlush();
                        }
                        break;
                    }
                }
                if (foundBroker == false)
                {
                    res[0].LastAssigned = true;
                    retval = res[0].Broker.ADUser.ADUserName;
                    res[0].SaveAndFlush();
                }
            }
            return retval;
        }

        #region Trade screen methods

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DataTable GetResetDatesByTradeType(string tradeType)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "GetResetDatesByTradeType");
            ParameterCollection parameters = new ParameterCollection();

            //SAHL.Common.DataAccess.Helper.AddVarcharParameter(parameters, "@TradeType", tradeType);
            parameters.Add(new SqlParameter("@TradeType", tradeType));
            DataTable dt = new DataTable();
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(CapApplication_DAO), parameters);
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tradeType"></param>
        /// <returns></returns>
        public DataTable GetResetDatesForAddingByTradeType(string tradeType)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "GetResetDatesForAddingByTradeType");
            ParameterCollection parameters = new ParameterCollection();

            //SAHL.Common.DataAccess.Helper.AddVarcharParameter(parameters, "@TradeType", tradeType);
            parameters.Add(new SqlParameter("@TradeType", tradeType));
            DataTable dt = new DataTable();
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(CapApplication_DAO), parameters);
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resetConfigurationKey"></param>
        /// <param name="tradeType"></param>
        /// <returns></returns>
        public DataTable GetTradeGroupingsByResetConfigurationKey(int resetConfigurationKey, string tradeType)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "GetTradeGroupingsByResetConfigurationKey");
            ParameterCollection parameters = new ParameterCollection();

            //SAHL.Common.DataAccess.Helper.AddIntParameter(parameters, "@ResetConfigurationKey", resetConfigurationKey);
            //SAHL.Common.DataAccess.Helper.AddVarcharParameter(parameters, "@TradeType", tradeType);
            parameters.Add(new SqlParameter("@ResetConfigurationKey", resetConfigurationKey));
            parameters.Add(new SqlParameter("@TradeType", tradeType));
            DataTable dt = new DataTable();
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(CapApplication_DAO), parameters);
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }

        public DataTable GetTradeGroupsByResetConfigKeyForDelete(int resetConfigurationKey, string tradeType)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "GetTradeGroupsByResetConfigKeyForDelete");
            ParameterCollection parameters = new ParameterCollection();

            //SAHL.Common.DataAccess.Helper.AddIntParameter(parameters, "@ResetConfigurationKey", resetConfigurationKey);
            //SAHL.Common.DataAccess.Helper.AddVarcharParameter(parameters, "@TradeType", tradeType);
            parameters.Add(new SqlParameter("@ResetConfigurationKey", resetConfigurationKey));
            parameters.Add(new SqlParameter("@TradeType", tradeType));
            DataTable dt = new DataTable();
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(CapApplication_DAO), parameters);
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resetConfigurationKey"></param>
        /// <param name="tradeType"></param>
        /// <returns></returns>
        public DataTable GetActiveTradeGroupingsByResetConfigurationKey(int resetConfigurationKey, string tradeType)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "GetTradeGroupingsByResetConfigurationKey");
            ParameterCollection parameters = new ParameterCollection();

            //SAHL.Common.DataAccess.Helper.AddIntParameter(parameters, "@ResetConfigurationKey", resetConfigurationKey);
            //SAHL.Common.DataAccess.Helper.AddVarcharParameter(parameters, "@TradeType", tradeType);
            parameters.Add(new SqlParameter("@ResetConfigurationKey", resetConfigurationKey));
            parameters.Add(new SqlParameter("@TradeType", tradeType));
            DataTable dt = new DataTable();
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(CapApplication_DAO), parameters);
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow newRow = dt.Rows[i];
                if (newRow["Status"].ToString() == "Inactive")
                {
                    newRow.Delete();
                }
            }

            return dt;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="capTypeKey"></param>
        /// <param name="resetConfigurationKey"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="closureDate"></param>
        /// <returns></returns>
        public IList<ITrade> GetTradeByGrouping(int capTypeKey, int resetConfigurationKey, DateTime effectiveDate, DateTime closureDate)
        {
            Trade_DAO[] res = null;
            string HQL = "";
            if (capTypeKey != -1)
            {
                HQL = "from Trade_DAO trade where trade.ResetConfiguration.Key = ? and trade.StartDate = ? and trade.EndDate = ? and trade.CapType.Key = ?";
                SimpleQuery<Trade_DAO> q = new SimpleQuery<Trade_DAO>(HQL, resetConfigurationKey, effectiveDate, closureDate, capTypeKey);
                res = q.Execute();
            }
            else
            {
                HQL = "from Trade_DAO trade where trade.ResetConfiguration.Key = ? and trade.StartDate = ? and trade.EndDate = ?";
                SimpleQuery<Trade_DAO> q = new SimpleQuery<Trade_DAO>(HQL, resetConfigurationKey, effectiveDate, closureDate);
                res = q.Execute();
            }

            IList<ITrade> retval = new List<ITrade>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new Trade(res[i]));
            }
            return retval;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IList<IResetConfiguration> GetResetConfigurations()
        {
            string HQL = "from ResetConfiguration_DAO r";
            SimpleQuery<ResetConfiguration_DAO> q = new SimpleQuery<ResetConfiguration_DAO>(HQL);
            ResetConfiguration_DAO[] res = q.Execute();

            IList<IResetConfiguration> retval = new List<IResetConfiguration>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new ResetConfiguration(res[i]));
            }
            return retval;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IResetConfiguration GetResetConfigurationByKey(int Key)
        {
            return base.GetByKey<IResetConfiguration, ResetConfiguration_DAO>(Key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IList<ICapType> GetCapTypesForTrade()
        {
            string HQL = "from CapType_DAO c";
            SimpleQuery<CapType_DAO> q = new SimpleQuery<CapType_DAO>(HQL);
            CapType_DAO[] res = q.Execute();

            IList<ICapType> retval = new List<ICapType>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new CapType(res[i]));
            }
            return retval;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ITrade CreateTrade()
        {
            return base.CreateEmpty<ITrade, Trade_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="trade"></param>
        public void SaveTrade(ITrade trade)
        {
            base.Save<ITrade, Trade_DAO>(trade);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="trade"></param>
        public void RemoveTrade(ITrade trade)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            List<string> rulesToRun = new List<string>();
            rulesToRun.Add("TradeCapBalanceValidation");
            svc.ExecuteRuleSet(spc.DomainMessages, rulesToRun, trade);

            Trade_DAO dao = (Trade_DAO)(trade as IDAOObject).GetDAOObject();
            dao.DeleteAndFlush();
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        #endregion Trade screen methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="productKey"></param>
        /// <returns></returns>
        public bool ProductQualifyForCap(int productKey)
        {
            string HQL = "select ospFATS from OSPFinancialAdjustmentTypeSource_DAO ospFATS join ospFATS.OriginationSourceProduct osp where osp.Product.Key = ?";
            SimpleQuery<OSPFinancialAdjustmentTypeSource_DAO> q = new SimpleQuery<OSPFinancialAdjustmentTypeSource_DAO>(HQL, productKey);
            OSPFinancialAdjustmentTypeSource_DAO[] res = q.Execute();

            bool retval = false;
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i].FinancialAdjustmentTypeSource.Key == Convert.ToInt32(FinancialAdjustmentTypeSources.CAP) ||
                    res[i].FinancialAdjustmentTypeSource.Key == Convert.ToInt32(FinancialAdjustmentTypeSources.CAP2))
                {
                    retval = true;
                    break;
                }
            }
            return retval;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="capApplication"></param>
        /// <returns></returns>
        public bool IsReAdvance(ICapApplication capApplication)
        {
            for (int i = 0; i < capApplication.CapApplicationDetails.Count; i++)
            {
                if (capApplication.CapApplicationDetails[i].CapStatus.Key == (int)CapStatuses.ReadvanceRequired ||
                    capApplication.CapApplicationDetails[i].CapStatus.Key == (int)CapStatuses.PrepareForCredit ||
                    capApplication.CapApplicationDetails[i].CapStatus.Key == (int)CapStatuses.GrantedCap2Offer)
                {
                    double currentBalance = 0;
                    double incInCLV = 0.0;

                    IMortgageLoanAccount mortgageLoanAccount = capApplication.Account as IMortgageLoanAccount;

                    // The CurrentBalance should not be calculated but rather read as at the point the application
                    // was created. If it needs to reflect the latest value the Cap Application should be reworked.
                    currentBalance = capApplication.CurrentBalance;

                    incInCLV = (Math.Round(Convert.ToDouble(capApplication.CapApplicationDetails[i].Fee + currentBalance), 2) - Math.Round(capApplication.Account.CommittedLoanValue, 2));
                    if (incInCLV > 0D)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Same as the IsReAdvance Method but checking the Loan Agreement Amount instead.
        /// </summary>
        /// <param name="capApplication"></param>
        /// <returns></returns>
        public bool IsReAdvanceLAA(ICapApplication capApplication)
        {
            bool isReAdvanceLAA = true;
            for (int i = 0; i < capApplication.CapApplicationDetails.Count; i++)
            {
                if (capApplication.CapApplicationDetails[i].CapStatus.Key == (int)CapStatuses.ReadvanceRequired ||
                  capApplication.CapApplicationDetails[i].CapStatus.Key == (int)CapStatuses.PrepareForCredit ||
                  capApplication.CapApplicationDetails[i].CapStatus.Key == (int)CapStatuses.GrantedCap2Offer)
                {
                    var ml = (IMortgageLoanAccount)capApplication.Account;
                    var loanAgreementAmount = ml.SecuredMortgageLoan.SumBondLoanAgreementAmounts();

                    if (capApplication.CurrentBalance + capApplication.CapApplicationDetails[i].Fee > loanAgreementAmount)
                    {
                        isReAdvanceLAA = false;
                        break;
                    }
                }
            }
            return isReAdvanceLAA;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="capApplication"></param>
        /// <returns></returns>
        public bool CheckLTVThreshold(ICapApplication capApplication)
        {
            IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
            double LTVThreshold = Convert.ToDouble(ctrlRepo.GetControlByDescription("CapLTVThreshold").ControlNumeric);

            for (int i = 0; i < capApplication.CapApplicationDetails.Count; i++)
            {
                if (capApplication.CapApplicationDetails[i].CapStatus.Key == (int)CapStatuses.ReadvanceRequired ||
                    capApplication.CapApplicationDetails[i].CapStatus.Key == (int)CapStatuses.PrepareForCredit)
                {
                    IMortgageLoanAccount mortgageLoanAccount = capApplication.Account as IMortgageLoanAccount;
                    double latestValuation = mortgageLoanAccount.SecuredMortgageLoan.GetActiveValuationAmount();
                    double currentBalance = 0;

                    // The CurrentBalance should not be calculated but rather read as at the point the application
                    // was created. If it needs to reflect the latest value the Cap Application should be reworked.
                    currentBalance = capApplication.CurrentBalance;
                    currentBalance += capApplication.CapApplicationDetails[i].Fee;
                    currentBalance += (mortgageLoanAccount.SecuredMortgageLoan.AccruedInterestMTD.HasValue ? mortgageLoanAccount.SecuredMortgageLoan.AccruedInterestMTD.Value : 0D);

                    double LTVCalculated = (currentBalance / latestValuation);

                    if (LTVCalculated > LTVThreshold)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns the CLV and Application Type for each CAP Type
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="capTypeConfigurationKey"></param>
        /// <param name="capOfferKey"></param>
        /// <param name="committedLoanValue"></param>
        /// <param name="appType1"></param>
        /// <param name="appType2"></param>
        /// <param name="appType3"></param>
        public void CapTypeDetermineAppType(int accountKey, int capTypeConfigurationKey, int capOfferKey, out double committedLoanValue, out string appType1, out string appType2, out string appType3)
        {
            committedLoanValue = 0;
            appType1 = "";
            appType2 = "";
            appType3 = "";

            string query = UIStatementRepository.GetStatement("Repositories.CapRepository", "CapTypeDetermineAppType");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));
            parameters.Add(new SqlParameter("@CapTypeConfigurationKey", capTypeConfigurationKey));
            parameters.Add(new SqlParameter("@CapOfferKey", capOfferKey));
            DataTable DT = new DataTable();
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(CapApplication_DAO), parameters);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CLV"] != null && !String.IsNullOrEmpty(ds.Tables[0].Rows[0]["CLV"].ToString()))
                    committedLoanValue = Convert.ToDouble(ds.Tables[0].Rows[0]["CLV"].ToString());
                if (ds.Tables[0].Rows[0]["AppType1"] != null && !String.IsNullOrEmpty(ds.Tables[0].Rows[0]["AppType1"].ToString()))
                    appType1 = ds.Tables[0].Rows[0]["AppType1"].ToString();
                if (ds.Tables[0].Rows[0]["AppType2"] != null && !String.IsNullOrEmpty(ds.Tables[0].Rows[0]["AppType2"].ToString()))
                    appType2 = ds.Tables[0].Rows[0]["AppType2"].ToString();
                if (ds.Tables[0].Rows[0]["AppType3"] != null && !String.IsNullOrEmpty(ds.Tables[0].Rows[0]["AppType3"].ToString()))
                    appType3 = ds.Tables[0].Rows[0]["AppType3"].ToString();
            }
        }

        #endregion ICapRepository Members
    }
}