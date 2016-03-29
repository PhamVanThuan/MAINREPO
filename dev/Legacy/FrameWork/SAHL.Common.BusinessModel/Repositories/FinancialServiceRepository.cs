using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.TypeMapper;
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

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IFinancialServiceRepository))]
    public class FinancialServiceRepository : AbstractRepositoryBase, IFinancialServiceRepository
    {
        public FinancialServiceRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public FinancialServiceRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        #region IFinancialServiceRepository Members

        public SAHL.Common.BusinessModel.Interfaces.IFinancialService GetFinancialServiceByKey(int Key)
        {
            return base.GetByKey<IFinancialService, FinancialService_DAO>(Key);

            //FinancialService_DAO fs = FinancialService_DAO.Find(Key);
            //if (fs != null)
            //{
            //    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            //    return BMTM.GetMappedType<IFinancialService, FinancialService_DAO>(fs);

            //}
            //return null;
        }

        /// <summary>
        /// Gets a <see cref="IFinancialServiceBankAccount"></see> by key.
        /// </summary>
        /// <param name="key">The primary key.</param>
        /// <returns>The <see cref="IFinancialServiceBankAccount"> object. This will return null if the object doesn't exist.</see></returns>
        public IFinancialServiceBankAccount GetFinancialServiceBankAccountByKey(int key)
        {
            FinancialServiceBankAccount_DAO dao = FinancialServiceBankAccount_DAO.TryFind(key);
            if (dao == null)
                return null;

            IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return bmtm.GetMappedType<IFinancialServiceBankAccount>(dao);
        }

        /// <summary>
        /// Implements <see cref="IFinancialServiceRepository.SaveFinancialService"></see>.
        /// </summary>
        public void SaveFinancialService(IFinancialService FinancialServiceToSave)
        {
            base.Save<IFinancialService, FinancialService_DAO>(FinancialServiceToSave);

            //IDAOObject dao = FinancialServiceToSave as IDAOObject;
            //FinancialService_DAO o = (FinancialService_DAO)dao.GetDAOObject();
            //o.SaveAndFlush();
            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IFinancialServiceBankAccount GetEmptyFinancialServiceBankAccount()
        {
            return base.CreateEmpty<IFinancialServiceBankAccount, FinancialServiceBankAccount_DAO>();

            //return new FinancialServiceBankAccount(new FinancialServiceBankAccount_DAO());
        }

        public void SaveFinancialServiceBankAccount(IFinancialServiceBankAccount bankAccount)
        {
            base.Save<IFinancialServiceBankAccount, FinancialServiceBankAccount_DAO>(bankAccount);

            //IDAOObject dao = bankAccount as IDAOObject;
            //FinancialServiceBankAccount_DAO o = (FinancialServiceBankAccount_DAO)dao.GetDAOObject();
            //o.SaveAndFlush();
            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        #endregion IFinancialServiceRepository Members

        // All Methods used when converting a loan that was marked/unmarked as Non Performing
        // Some of methods could not be ported over and have been re-used through UI statements

        #region NonPerformingLoan

        /// <summary>
        /// Get the suspended interest amount and
        /// date that interest was suspended from
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="suspendedDate"></param>
        /// <returns></returns>
        public Decimal GetSuspendedInterest(int accountKey, out DateTime? suspendedDate)
        {
            suspendedDate = null;

            string query = UIStatementRepository.GetStatement("MortgageLoan", "NonPerformingLoanInfoGet");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountKey));

            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(MortgageLoan_DAO), prms);
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    suspendedDate = Convert.ToDateTime(dr["FromDate"]);
                    return Convert.ToDecimal(dr["SuspendedInterestAmount"]);
                }
            }

            return 0M;
        }

        public bool IsLoanNonPerforming(int accountKey)
        {
            string query = UIStatementRepository.GetStatement("MortgageLoan", "IsLoanNonPerforming");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountKey));

            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(MortgageLoan_DAO), prms);

            if ((int)o > 0)
                return true;
            else
                return false;
        }

        #endregion NonPerformingLoan

        /// <summary>
        ///
        /// </summary>
        /// <param name="FinancialServiceGroupKey"></param>
        /// <returns></returns>
        public IFinancialServiceGroup GetFinancialServiceGroup(int FinancialServiceGroupKey)
        {
            return base.GetByKey<IFinancialServiceGroup, FinancialServiceGroup_DAO>(FinancialServiceGroupKey);
        }
    }
}