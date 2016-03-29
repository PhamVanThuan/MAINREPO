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
using SAHL.Common.Factories;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data.SqlClient;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Common.Presenters.TransactionAccessMaintenance
{
    public class Base : SAHLCommonBasePresenter<ITransactionAccessMaintenance>
    {
        #region Locals

        protected ICommonRepository _comRepo;
        private DataTable _groups;
        protected DataTable _transactionsAD;
        private DataSet _orgStructure;
        private DataTable _users;

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public Base(ITransactionAccessMaintenance view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected void BindGroups()
        {
            //Get the data
            _groups = ComRepo.ExecuteUIStatement("GetTransactionAccessGroups", "COMMON", null);
            //Bind to the list
            _view.BindGroupDropDown(_groups);
        }

        protected void BindUsers()
        {
            StringCollection table = new StringCollection();
            table.Add("ADUser");
            table.Add("UserOrganisationStructure");
            //Get the data
            _orgStructure = ComRepo.ExecuteUIStatement("ADUserListGet", "COMMON", null, table);
            _users = _orgStructure.Tables["ADUser"];
            //Bind to the list
            _view.BindUserDropDown(_users);
        }

        protected void GetTransactionTypesByADCredentials(string ADCredentials, bool BindTree)
        {
            List<SqlParameter> prmList = new List<SqlParameter>();

            prmList.Add(new SqlParameter("@ADCredentials", ADCredentials));
            //Get the data
            _transactionsAD = ComRepo.ExecuteUIStatement("GetTransactionTypesByADCredentials", "COMMON", prmList);
            //Bind to the list
            if (BindTree)
                _view.BindTransactionAccessTree(_transactionsAD);
        }

        private void PopulateDataTableForUpdate()
        {
            int i;
            string checkedTransactions = _view.CheckedTransactions;

            for (i = 0; i < _transactionsAD.Rows.Count; i++)
            {
                DataRow row = (DataRow)_transactionsAD.Rows[i];

                if (checkedTransactions.IndexOf("[" + i + "];") > -1)
                    row["HasAccess"] = true;
                else
                    row["HasAccess"] = false;
            }
        }

        protected void CommitToDB(string ADCredentials)
        {
            GetTransactionTypesByADCredentials(ADCredentials, false);

            PopulateDataTableForUpdate();

            ComRepo.ExecuteUIStatementUpdate(_transactionsAD, "TransactionTypeDataAccessBulkUpdate", "COMMON", UpdateParameters(ADCredentials));
        }

        protected bool GroupExists(string groupName)
        {
            foreach (DataRow dr in _groups.Rows)
            {
                if (groupName == dr["ADCredentials"].ToString())
                    return true;
            }

            return false;
        }

        protected static List<SqlParameter> UpdateParameters(string ADCredentials)
        {
            List<SqlParameter> prms = new List<SqlParameter>();
            // Update Parameters
            prms.Add(new SqlParameter("@ADCredentials", ADCredentials));
            
            SqlParameter param1 = new SqlParameter("@HasAccess", SqlDbType.Bit);
            param1.Direction = ParameterDirection.Input;
            param1.SourceColumn = "HasAccess";
            prms.Add(param1);

            SqlParameter param2 = new SqlParameter("@TransactionTypeNumber", SqlDbType.SmallInt);
            param2.Direction = ParameterDirection.Input;
            param2.SourceColumn = "TransactionTypeNumber";
            prms.Add(param2);

            return prms;
        }

        protected void NavigateCancel()
        {
            Navigator.Navigate("TxnAccess");
        }

        protected void NavigateAdd()
        {
            if (GlobalCacheData.ContainsKey("NewTransactionAccessGroup"))
                GlobalCacheData.Remove("NewTransactionAccessGroup");

            IList<ICacheObjectLifeTime> lifeTimes = new List<ICacheObjectLifeTime>();
            GlobalCacheData.Add("NewTransactionAccessGroup", _view.NewGroupDescription, lifeTimes);

            NavigateCancel();
        }

        protected void CheckCache()
        {
            if (GlobalCacheData.ContainsKey("NewTransactionAccessGroup"))
            {
                _view.GroupSelectedValue = GlobalCacheData["NewTransactionAccessGroup"].ToString();

                GlobalCacheData.Remove("NewTransactionAccessGroup");
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
