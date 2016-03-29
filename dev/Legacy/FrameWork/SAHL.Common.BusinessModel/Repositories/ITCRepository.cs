using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
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
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IITCRepository))]
    public class ITCRepository : AbstractRepositoryBase, IITCRepository
    {
        public ITCRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public ITCRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        //private void AddDomainErrorMessage(IDomainMessage item)
        //{
        //    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
        //    spc.DomainMessages.Add(item);
        //}

        public IList<IAddressStreet> GetITCAddressListByLegalEntityKey(int LegalEntityKey)
        {
            //LegalEntityAddress_DAO lea;
            //lea.AddressType.Key;
            //lea.Address.ChangeDate;

            string HQL = "select lea from LegalEntityAddress_DAO lea join lea.Address a WHERE lea.LegalEntity.Key = ? "
                + "AND lea.AddressType.Key in (1,2) AND a.AddressFormat = 1 order by lea.AddressType.Key, lea.Address.ChangeDate desc";
            SimpleQuery<LegalEntityAddress_DAO> q = new SimpleQuery<LegalEntityAddress_DAO>(HQL, LegalEntityKey);
            LegalEntityAddress_DAO[] res = q.Execute();

            List<AddressStreet_DAO> list = new List<AddressStreet_DAO>();

            for (int i = 0; i < res.Length; i++)
            {
                list.Add(res[i].Address.As<AddressStreet_DAO>());

                if (list.Count == 2)
                    break;
            }

            IEventList<IAddressStreet> evList = new DAOEventList<AddressStreet_DAO, IAddressStreet, AddressStreet>(list);
            return new List<IAddressStreet>(evList);
        }

        public IList<IITC> GetITCByAccountKey(int AccountKey)
        {
            //ITC itc;
            //itc.LegalEntity.LegalEntityStatus.Key;

            string HQL = "select itc from ITC_DAO itc where itc.ReservedAccount.Key = ? ";
            SimpleQuery<ITC_DAO> q = new SimpleQuery<ITC_DAO>(HQL, AccountKey);
            ITC_DAO[] res = q.Execute();

            IEventList<IITC> list = new DAOEventList<ITC_DAO, IITC, ITC>(res);
            return new List<IITC>(list);
        }

        //public IList<IITC> GetITCForHistoryByLeExcludingAccount(int legalEntity, int accountKey)
        //{
        //    //ITC itc;
        //    //itc.LegalEntity.LegalEntityStatus.Key;

        //    string HQL = "select itc from ITC_DAO itc inner join itc.LegalEntity le "
        //                  + "where le.Key = ? and itc.ReservedAccount.Key != ? "
        //                  + "order by itc.ChangeDate desc";

        //    SimpleQuery<ITC_DAO> q = new SimpleQuery<ITC_DAO>(HQL, legalEntity, accountKey);
        //    ITC_DAO[] res = q.Execute();

        //    IEventList<IITC> list = new DAOEventList<ITC_DAO, IITC, ITC>(res);
        //    return new List<IITC>(list);
        //}

        public DataTable GetArchivedITCByLegalEntityKey(int legalEntityKey, int accountKey)
        {
            string sql = UIStatementRepository.GetStatement("COMMON", "ITCGetHistoryByLegalEntityKey");

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@LEKey", legalEntityKey));
            prms.Add(new SqlParameter("@AccountKey", accountKey));

            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(ITC_DAO), prms);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return new DataTable();
        }

        public IITCArchive GetArchivedITCByITCKey(int Key)
        {
            return base.GetByKey<IITCArchive, ITCArchive_DAO>(Key);
        }

        public IITC GetITCByKey(int Key)
        {
            return base.GetByKey<IITC, ITC_DAO>(Key);
        }

        public IITC GetEmptyITC()
        {
            return base.CreateEmpty<IITC, ITC_DAO>();

            //return new ITC(new ITC_DAO());
        }

        public string GetITCXslByDate(DateTime date)
        {
            //string HQL = "select itcxsl from ITCXSL_DAO itcxsl where itcxsl.EffectiveDate < ? order by itcxsl.EffectiveDate desc";
            //SimpleQuery<ITCXSL_DAO> q = new SimpleQuery<ITCXSL_DAO>(HQL, date);
            //q.SetQueryRange(1);
            //ITCXSL_DAO[] res = q.Execute();

            //return new ITCXSL(res[0]);

            // use the uiStatement to inject dates, bit of a hack, but useful
            ICommonRepository cRepo = RepositoryFactory.GetRepository<ICommonRepository>();

            List<SqlParameter> listprms = new List<SqlParameter>();
            listprms.Add(new SqlParameter("@EffectiveDate", date));
            DataTable dt = cRepo.ExecuteUIStatement("ITCXslGetByDate", "COMMON", listprms);

            if (dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();

            return null;
        }

        //public DataTable GetITCWarningsForLegalEntity(Int32 accountKey, Int32 legalEntityKey)
        //{
        //    throw new NotImplementedException("not done yet");

        //    //ICommonRepository cRepo = RepositoryFactory.GetRepository<ICommonRepository>();

        //    //List<SqlParameter> listprms = new List<SqlParameter>();
        //    //listprms.Add(new SqlParameter("@AccountKey", accountKey));
        //    //listprms.Add(new SqlParameter("@LegalEntityKey", legalEntityKey));

        //    //return cRepo.ExecuteUIStatement("ITCGetWarnings", "COMMON", listprms);
        //}

        //public DataTable GetITCWarningsForAccount(IAccount account)
        //{
        //    throw new NotImplementedException("not done yet");

        //    //foreach (IRole r in account.Roles)
        //    //{
        //    //    if (r.RoleType.Key == (int)RoleTypes.MainApplicant || r.RoleType.Key == (int)RoleTypes.Suretor)
        //    //    {
        //    //        GetITCWarningsForLegalEntity(r.Account.Key, r.LegalEntity.Key);
        //    //    }
        //    //}

        //    //return null;
        //}

        //public DataTable GetITCWarningsForApplication(IApplication application)
        //{
        //    throw new NotImplementedException("not done yet");

        //    //foreach (IRole r in application.ApplicationRoles)
        //    //{
        //    //    if (r.RoleType.Key == (int)RoleTypes.MainApplicant || r.RoleType.Key == (int)RoleTypes.Suretor)
        //    //    {
        //    //        GetITCWarningsForLegalEntity(application.ReservedAccount.Key, r.LegalEntity.Key);
        //    //    }
        //    //}

        //    //return null;
        //}

        public IList<IITC> GetITCByLEAndAccountKey(int LEKey, int AccountKey)
        {
            string HQL = "select itc from ITC_DAO itc where itc.ReservedAccount.Key = ? and itc.LegalEntity.Key=? order by itc.Key desc";
            SimpleQuery<ITC_DAO> q = new SimpleQuery<ITC_DAO>(HQL, AccountKey, LEKey);
            ITC_DAO[] res = q.Execute();

            IEventList<IITC> list = new DAOEventList<ITC_DAO, IITC, ITC>(res);
            return new List<IITC>(list);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="LEKeys"></param>
        /// <returns></returns>
        public IList<IITC> GetITCByLegalEntityKeys(int[] LEKeys)
        {
            String keys = String.Empty;

            foreach (int i in LEKeys)
                keys += String.Format("{0}, ", i.ToString());

            keys = keys.Substring(0, keys.Length - 2);

            string sql = String.Format("select itc.* from ITC itc where itc.LegalEntityKey IN ({0}) order by itc.ITCKey desc", keys);
            SimpleQuery<ITC_DAO> q = new SimpleQuery<ITC_DAO>(QueryLanguage.Sql, sql);
            q.AddSqlReturnDefinition(typeof(ITC_DAO), "itc");
            ITC_DAO[] res = q.Execute();

            IEventList<IITC> list = new DAOEventList<ITC_DAO, IITC, ITC>(res);
            return new List<IITC>(list);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="itc"></param>
        /// <param name="itca"></param>
        public void GetITCOrArchiveITCByITCKey(int Key, out IITC itc, out IITCArchive itca)
        {
            ITC_DAO dao = ITC_DAO.TryFind(Key);
            if (null != dao)
                itc = new ITC(dao);
            else
                itc = null;
            ITCArchive_DAO daoa = ITCArchive_DAO.TryFind(Key);
            if (null != daoa)
                itca = new ITCArchive(daoa);
            else
                itca = null;
        }

        /// <summary>
        /// Saves an IITC object and its properties to the database (SQLUpdate)
        /// </summary>
        /// <param name="itc"></param>
        public void SaveITC(IITC itc)
        {
            base.Save<IITC, ITC_DAO>(itc);
        }
    }
}