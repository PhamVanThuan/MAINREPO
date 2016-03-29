using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [ActiveRecord("WorkFlowIcon", Schema = "X2")]
    public partial class WorkFlowIcon_DAO : DB_X2<WorkFlowIcon_DAO>
    {
        private string _name;

        //private NullableByte[] _icon;

        private int _iD;

        private IList<WorkFlow_DAO> _workFlows;

        [Property("Name", ColumnType = "String", NotNull = true)]
        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        //[Property("Icon")]
        //public virtual NullableByte[] Icon
        //{
        //    get
        //    {
        //        return this._icon;
        //    }
        //    set
        //    {
        //        this._icon = value;
        //    }
        //}

        [PrimaryKey(PrimaryKeyType.Native, "ID", ColumnType = "Int32")]
        public virtual int ID
        {
            get
            {
                return this._iD;
            }
            set
            {
                this._iD = value;
            }
        }

        [HasMany(typeof(WorkFlow_DAO), ColumnKey = "IconID", Table = "WorkFlow", Lazy = true)]
        public virtual IList<WorkFlow_DAO> WorkFlows
        {
            get
            {
                return this._workFlows;
            }
            set
            {
                this._workFlows = value;
            }
        }
    }
}