using System;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.CacheData;
using SAHL.Common.Security;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [ActiveRecord("WorkList", Schema = "X2")]
    public partial class WorkList_DAO : DB_X2<WorkList_DAO>
    {
        private int _iD;

        private string _aDUserName;

        private System.DateTime _listDate;

        private string _message;

        private Instance_DAO _instance;

        [PrimaryKey(PrimaryKeyType.Native, "ID", ColumnType = "Int32")]
        public virtual int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        [Property("ADUserName", ColumnType = "String", NotNull = true)]
        public virtual string ADUserName
        {
            get
            {
                return this._aDUserName;
            }
            set
            {
                this._aDUserName = value;
            }
        }

        [Property("ListDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime ListDate
        {
            get
            {
                return this._listDate;
            }
            set
            {
                this._listDate = value;
            }
        }

        [Property("Message", ColumnType = "String", NotNull = true)]
        public virtual string Message
        {
            get
            {
                return this._message;
            }
            set
            {
                this._message = value;
            }
        }

        [BelongsTo("InstanceID", NotNull = true)]
        public virtual Instance_DAO Instance
        {
            get
            {
                return this._instance;
            }
            set
            {
                this._instance = value;
            }
        }

        public static WorkList_DAO[] FindByADUserName(SAHLPrincipal principal)
        {
            if (principal == null)
                return null;

            string groups = SAHLPrincipalCache.GetPrincipalCache(principal).GetCachedRolesAsStringForQuery(true, true); //principal.GetCachedRolesAsStringForQuery(true, true);
            string HQL = String.Format("SELECT DISTINCT wl.* FROM WorkList_DAO wl WHERE wl.ADUserName IN ({0})", groups);
            SimpleQuery<WorkList_DAO> q = new SimpleQuery<WorkList_DAO>(HQL);
            //q.SetParameter("groups", groups);
            WorkList_DAO[] res = q.Execute();
            return res;
        }
    }
}