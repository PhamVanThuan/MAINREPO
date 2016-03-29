using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    ///
    /// </summary>
    [FactoryType(typeof(IFutureDatedChangeRepository))]
    public class FutureDatedChangeRepository : AbstractRepositoryBase, IFutureDatedChangeRepository
    {
        public FutureDatedChangeRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public FutureDatedChangeRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        /// <summary>
        /// Returns all Future Dated transaction per Generic Key
        /// </summary>
        /// <param name="genericKey">Generic Key </param>
        /// <param name="futureDatedChangeTypeKey">Either Fixed D/) or Normal D/O</param>
        /// <returns></returns>
        public IList<IFutureDatedChange> GetFutureDatedChangesByGenericKey(int genericKey, int futureDatedChangeTypeKey)
        {
            string HQL = "from FutureDatedChange_DAO t where t.IdentifierReferenceKey = ? and t.FutureDatedChangeType.Key = ? order by t.EffectiveDate";
            SimpleQuery<FutureDatedChange_DAO> q = new SimpleQuery<FutureDatedChange_DAO>(HQL, genericKey, futureDatedChangeTypeKey);
            FutureDatedChange_DAO[] res = q.Execute();

            IList<IFutureDatedChange> retval = new List<IFutureDatedChange>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new FutureDatedChange(res[i]));
            }
            return retval;
        }

        /// <summary>
        /// returns a futureDatedChange object given its key
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IFutureDatedChange GetFutureDatedChangeByKey(int Key)
        {
            return base.GetByKey<IFutureDatedChange, FutureDatedChange_DAO>(Key);
        }

        public IFutureDatedChangeDetail GetFutureDatedChangeDetailByKey(int Key)
        {
            return base.GetByKey<IFutureDatedChangeDetail, FutureDatedChangeDetail_DAO>(Key);
        }

        /// <summary>
        /// Deletes any future dated changes
        /// </summary>
        /// <param name="futureDatedChangeKey"></param>
        /// <param name="includeSameDayTransactions"></param>
        public void DeleteFutureDateChangeByKey(int futureDatedChangeKey, bool includeSameDayTransactions)
        {
            if (!includeSameDayTransactions)
            {
                string q = string.Format("FutureDatedChangeKey={0}", futureDatedChangeKey);
                FutureDatedChange_DAO.DeleteAll(q);
            }
            else
            {
                IFutureDatedChangeRepository FDCR = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();
                IFutureDatedChange futureDatedChange = FDCR.GetFutureDatedChangeByKey(futureDatedChangeKey);
                if (futureDatedChange != null)
                {
                    string query = "select fdc from FutureDatedChange_DAO fdc where fdc.IdentifierReferenceKey = ? and fdc.EffectiveDate = ?";
                    SimpleQuery<FutureDatedChange_DAO> SQ = new SimpleQuery<FutureDatedChange_DAO>(query, futureDatedChange.IdentifierReferenceKey, futureDatedChange.EffectiveDate);
                    object o = FutureDatedChange_DAO.ExecuteQuery(SQ);
                    FutureDatedChange_DAO[] fdc = o as FutureDatedChange_DAO[];
                    List<int> keys = new List<int>();
                    if (fdc != null && fdc.Length >= 1)
                    {
                        foreach (FutureDatedChange_DAO obj in fdc)
                        {
                            keys.Add(obj.Key);
                        }
                    }
                    FutureDatedChange_DAO.DeleteAll(keys);
                }
            }
        }

        /// <summary>
        /// Save FutureDatedChange Records
        /// </summary>
        /// <param name="futureDatedChange"></param>
        public void SaveFutureDatedChange(IFutureDatedChange futureDatedChange)
        {
            base.Save<IFutureDatedChange, FutureDatedChange_DAO>(futureDatedChange);
        }

        /// <summary>
        /// Save FutureDatedChange Records
        /// </summary>
        /// <param name="futureDatedChangeDetail"></param>
        public void SaveFutureDatedChangeDetail(IFutureDatedChangeDetail futureDatedChangeDetail)
        {
            base.Save<IFutureDatedChangeDetail, FutureDatedChangeDetail_DAO>(futureDatedChangeDetail);
        }

        /// <summary>
        /// Creates an Empty FutureDatedChange object
        /// </summary>
        /// <returns>IRole</returns>
        public IFutureDatedChange CreateEmptyFutureDatedChange()
        {
            return base.CreateEmpty<IFutureDatedChange, FutureDatedChange_DAO>();
        }

        /// <summary>
        /// Creates an Empty FutureDatedChange object
        /// </summary>
        /// <returns>IRole</returns>
        public IFutureDatedChangeDetail CreateEmptyFutureDatedChangeDetail()
        {
            return base.CreateEmpty<IFutureDatedChangeDetail, FutureDatedChangeDetail_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="FinancialServiceKey"></param>
        /// <returns></returns>
        public Hashtable FutureDatedChangeMap(int FinancialServiceKey)
        {
            Hashtable fdcMap = new Hashtable();
            string sqlQuery = UIStatementRepository.GetStatement("COMMON", "FutureDatedChangeMap");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@FinancialServiceKey", Convert.ToInt32(FinancialServiceKey)));
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sqlQuery, typeof(FinancialService_DAO), prms);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int FDCVariableKey = Convert.ToInt32(dr[0]);
                    int FDCFixedKey = Convert.ToInt32(dr[1]);
                    if (!fdcMap.ContainsKey(FDCVariableKey)) // only add the values if key doesnt exist
                        fdcMap.Add(FDCVariableKey, FDCFixedKey);
                }
            }

            return fdcMap;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="FinancialServiceKey"></param>
        /// <returns></returns>
        public Hashtable FutureDatedChangeDetailMap(int FinancialServiceKey)
        {
            Hashtable fdcdMap = new Hashtable();
            string sqlQuery = UIStatementRepository.GetStatement("COMMON", "FutureDatedChangeDetailMap");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@FinancialServiceKey", Convert.ToInt32(FinancialServiceKey)));
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sqlQuery, typeof(FinancialService_DAO), prms);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int FDCDVariableKey = Convert.ToInt32(dr[0]);
                    int FDCDFixedKey = Convert.ToInt32(dr[1]);
                    if (!fdcdMap.ContainsKey(FDCDVariableKey)) // only add the values if key doesnt exist
                        fdcdMap.Add(FDCDVariableKey, FDCDFixedKey);
                }
            }

            return fdcdMap;
        }
    }
}