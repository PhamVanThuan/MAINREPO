using System.Collections.Generic;
using Castle.ActiveRecord;

using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [ActiveRecord("StateType", Schema = "X2")]
    public partial class StateType_DAO : DB_X2<StateType_DAO>
    {
        private string _name;

        private int _iD;

        private IList<State_DAO> _states;

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

        [HasMany(typeof(State_DAO), ColumnKey = "Type", Table = "State", Lazy = true)]
        public virtual IList<State_DAO> States
        {
            get
            {
                return this._states;
            }
            set
            {
                this._states = value;
            }
        }
    }
}