using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.DomainMessages;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Globals;
using System.Collections;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.Factories;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.X2.BusinessModel;
using System.Data;
using SAHL.Common.DataAccess;
using System.Security.Principal;
using SAHL.Common;
using SAHL.Common.Exceptions;
using SAHL.Common.CacheData;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IMemoRepository))]
    public class MemoRepository : AbstractRepositoryBase, IMemoRepository
    {
        public IEventList<IMemo> GetMemoByGenericKey(int GenericKey, int GenericKeyTypeKey)
        {
            return Memo.GetByGenericKey(GenericKey, GenericKeyTypeKey);
        }

        public IEventList<IMemo> GetMemoByGenericKey(int GenericKey, int GenericKeyTypeKey, int GeneralStatusKey)
        {
            return Memo.GetByGenericKey(GenericKey, GenericKeyTypeKey, GeneralStatusKey);
        }

        /// <summary>
        /// Get all Memos for a GenericKey created by the ADUser on the date specified
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="dt"></param>
        /// <param name="adUser"></param>
        /// <returns></returns>
        public IEventList<IMemo> GetMemoByGenericKeyADUserAndDate(int genericKey, int genericKeyTypeKey, DateTime dt, IADUser adUser)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            string HQL = "from Memo_DAO memo where memo.GenericKey = ? and memo.GenericKeyType.Key = ? and memo.ADUser.Key = ? order by memo.InsertedDate desc";
            SimpleQuery<Memo_DAO> q = new SimpleQuery<Memo_DAO>(HQL, genericKey, genericKeyTypeKey, adUser.Key);

            Memo_DAO[] daoList = q.Execute();

            IEventList<IMemo> memoList = new EventList<IMemo>();
            if (daoList != null && daoList.Length > 0)
            {
                
                foreach (Memo_DAO memo in daoList)
                {
                    if (memo.InsertedDate.ToShortDateString() == dt.ToShortDateString())
                        memoList.Add(spc.DomainMessages, new Memo(memo));
                }
            }

            if (memoList.Count > 0)
                return memoList;

            return null;
        }

        public IMemo CreateMemo()
        {
			return base.CreateEmpty<IMemo, Memo_DAO>();
			//return new Memo(new Memo_DAO());
        }

        public void SaveMemo( IMemo memo)
        {
			base.Save<IMemo, Memo_DAO>(memo);
			//Memo_DAO m_dao = (Memo_DAO)(memo as IDAOObject).GetDAOObject();
			//    m_dao.SaveAndFlush();
			//    if (ValidationHelper.PrincipalHasValidationErrors())
			//        throw new DomainValidationException();
            
        }

        public IInstance GetInstanceForMemoKeyAndApplicationKeyForOriginationWorkflow( int GenericKey, int MemoKey)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            string WorkflowName = "Followup";
            string query = string.Format("SELECT InstanceID FROM X2.X2DATA.{2} WHERE MemoKey = {0} and ApplicationKey={1}", MemoKey, GenericKey, WorkflowName);
            DataTable DT = new DataTable();
            IDbConnection con = Helper.GetSQLDBConnection("X2");
            
            Helper.FillFromQuery(DT, query, con, null);

            Dictionary<string, object> dict = new Dictionary<string, object>();

            if (DT.Rows.Count > 0)
            {
                Int64 IID = Convert.ToInt64(DT.Rows[0][0]);
                IX2Repository x2 = RepositoryFactory.GetRepository<IX2Repository>();
                IInstance instance = x2.GetInstanceByKey(IID);
                return instance;
            }
            spc.DomainMessages.Add(new Error(string.Format("Unable to find InstanceID for MemoKey:{0}", MemoKey), ""));
            return null;
        }

        public IMemo GetMemoByKey(int Key)
        {
			return base.GetByKey<IMemo, Memo_DAO>(Key);
			//Memo_DAO dao = Memo_DAO.Find(Key);
			//return new Memo(dao);
        }

        /// <summary>
        /// Retrieve all the Memos related an Account or Application.
        /// If Application is recieved, check Account (if it exists as yet) for all the Applications
        /// that is linked to it.
        /// So we retrieve Memos for Applications and Account, 
        /// since Memo is saved in context based on Generic Key.
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="GenericKeyTypeKey"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public IEventList<IMemo> GetMemoRelatedToAccount(int GenericKey, int GenericKeyTypeKey, int Status)
        {
            IAccountRepository _accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            List<int> _accountKeys = new List<int>();
            IAccount _account = null;
            IEventList<IMemo> memoList = new EventList<IMemo>();
            int _applicationKey = -1;
            string hql = string.Empty;

            // This handles account and application types specifically
            if (GenericKeyTypeKey == (int)GenericKeyTypes.Account || GenericKeyTypeKey == (int)GenericKeyTypes.Offer)
            {
                if (GenericKeyTypeKey == (int)GenericKeyTypes.Account)
                    _account = _accRepository.GetAccountByKey(GenericKey);
                else if (GenericKeyTypeKey == (int)GenericKeyTypes.Offer)
                {
                    _account = _accRepository.GetAccountByApplicationKey(GenericKey);
                    _applicationKey = GenericKey;
                }

                // Get all Memo linked to the Application/s
                // Check if the Application is linked to an Account
                // Could be the Application the Account links to, has many Application/s which needs to be retrieved
                if (_account != null)
                {
                    foreach (IApplication application in _account.Applications)
                    {
                        _accountKeys.Add(application.Key);
                    }
                }
                else
                {
                    _accountKeys.Add(_applicationKey);
                }

                //string hql = "select m from Memo_DAO m where m.GenericKey in (:appKeys) and m.GenericKeyType = ? and m.GeneralStatus = ? order by m.InsertedDate desc";
                //SimpleQuery<Memo_DAO> q = new SimpleQuery<Memo_DAO>(hql, (int)GenericKeyTypes.Offer, (int)GeneralStatuses.Active);
                if (_accountKeys.Count > 0)
                {
                    if (Status == 0)
                        hql = string.Format("select m from Memo_DAO m where m.GenericKey in (:appKeys) and m.GenericKeyType = {0} order by m.InsertedDate desc", (int)GenericKeyTypes.Offer);
                    else
                        hql = string.Format("select m from Memo_DAO m where m.GenericKey in (:appKeys) and m.GenericKeyType = {0} and m.GeneralStatus.Key = {1} order by m.InsertedDate desc", (int)GenericKeyTypes.Offer, Status);

                    SimpleQuery<Memo_DAO> q = new SimpleQuery<Memo_DAO>(hql);
                    q.SetParameterList("appKeys", _accountKeys);
                    Memo_DAO[] res = q.Execute();

                    foreach (Memo_DAO memo in res)
                    {
                        memoList.Add(null, new Memo(memo));
                    }
                }

                // Get all Memo linked to the Account
                if (_account != null)
                {
                    SimpleQuery<Memo_DAO> q = new SimpleQuery<Memo_DAO>(hql);
                    if (Status == 0)
                    {
                        hql = "select m from Memo_DAO m where m.GenericKey = ? and m.GenericKeyType = ? order by m.InsertedDate desc";
                        q = new SimpleQuery<Memo_DAO>(hql, _account.Key, (int)GenericKeyTypes.Account);
                    }
                    else
                    {
                        hql = "select m from Memo_DAO m where m.GenericKey = ? and m.GenericKeyType = ? and m.GeneralStatus.Key = ? order by m.InsertedDate desc";
                        q = new SimpleQuery<Memo_DAO>(hql, _account.Key, (int)GenericKeyTypes.Account, Status);
                    }

                    Memo_DAO[] res = q.Execute();
                    foreach (Memo_DAO memo in res)
                    {
                        memoList.Add(null, new Memo(memo));
                    }
                }
            }
            else
            {
                // Then it becomes more of Generic Query
                SimpleQuery<Memo_DAO> q = new SimpleQuery<Memo_DAO>(hql);
                if (Status == 0)
                {
                    hql = "select m from Memo_DAO m where m.GenericKey = ? and m.GenericKeyType = ? order by m.InsertedDate desc";
                    q = new SimpleQuery<Memo_DAO>(hql, GenericKey, GenericKeyTypeKey);
                }
                else
                {
                    hql = "select m from Memo_DAO m where m.GenericKey = ? and m.GenericKeyType = ? and m.GeneralStatus.Key = ? order by m.InsertedDate desc";
                    q = new SimpleQuery<Memo_DAO>(hql, GenericKey, GenericKeyTypeKey, Status);
                }

                Memo_DAO[] res = q.Execute();
                foreach (Memo_DAO memo in res)
                {
                    memoList.Add(null, new Memo(memo));
                }
            }
            return memoList;
        }
    }
}
