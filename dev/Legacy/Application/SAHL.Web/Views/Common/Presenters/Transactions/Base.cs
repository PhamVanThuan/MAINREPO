using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Data.SqlClient;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Web.Controls;
using SAHL.Common.UI;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Common.Presenters.Transactions
{
    public class Base : SAHLCommonBasePresenter<ITransaction>
    {
        #region Locals
        
        protected IAccountRepository _accRepo;
        protected ILoanTransactionRepository _ltRepo;
        protected ICommonRepository _comRepo;

        protected CBOMenuNode _cboNode;
        protected int _accountKey;
        protected DataTable _transactionTypeDescriptions;
        protected DataTable _transactions;

        // private int dtTranNumColumn;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public Base(ITransaction view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node 
            _cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_cboNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
        }

        protected void GetTransactionsAndTypes(bool isArrears)
        {
            int fetchRows = 10000;
            List<SqlParameter> transPrms = new List<SqlParameter>();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);

            transPrms.Add(new SqlParameter("@AccountKey", _accountKey));
            transPrms.Add(new SqlParameter("@Roles", spc.GetCachedRolesAsStringForQuery(true, true, false)));

            if (!isArrears)
                _transactions = LTRepo.GetTransactions("GetFinancialTransactionsByAccountKey", "COMMON", transPrms, fetchRows);
            else
                _transactions = LTRepo.GetTransactions("GetArrearTransactionsByAccountKey", "COMMON", transPrms, fetchRows);
        }

        protected void GetRollbackTransactions(bool isArrears)
        {
            string transStatement; // rollback transaction grid
            List<SqlParameter> transPrms = new List<SqlParameter>();

            if (isArrears)
            {
                transStatement = "GetArrearRollBackTrans";
                SqlParameter prm1 = new SqlParameter("@ArrearTransactionKey", Convert.ToInt32(GlobalCacheData["RollbackTransactionNumber"].ToString()));
                transPrms.Add(prm1);
            }
            else
            {
                transStatement = "GetLoanRollBackTrans";
                SqlParameter prm1 = new SqlParameter("@FinancialTransactionKey", Convert.ToInt32(GlobalCacheData["RollbackTransactionNumber"].ToString()));
                transPrms.Add(prm1);
            }

            _transactions = ComRepo.ExecuteUIStatement(transStatement, "COMMON", transPrms);
        }

        protected IAccountRepository AccRepo
        {
            get
            {
                if (null == _accRepo)
                    _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                return _accRepo;
            }
        }

        protected ILoanTransactionRepository LTRepo
        {
            get
            {
                if (null == _ltRepo)
                    _ltRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();

                return _ltRepo;
            }
        }

        protected ICommonRepository ComRepo
        {
            get
            {
                if (null == _comRepo)
                    _comRepo = RepositoryFactory.GetRepository<ICommonRepository>();

                return _comRepo;
            }
        }

    }
}
