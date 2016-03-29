using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// The Control DAO Object
    /// </summary>
    [ActiveRecord("Control", Schema = "dbo", Lazy = false)]
    public partial class Control_DAO : DB_2AM<Control_DAO>
    {
        private string _controlDescription;

        private double? _controlNumeric;

        private string _controlText;

        private ControlGroup_DAO _controlGroup;

        private int _key;

        [Property("ControlDescription", ColumnType = "String")]
        public virtual string ControlDescription
        {
            get
            {
                return this._controlDescription;
            }
            set
            {
                this._controlDescription = value;
            }
        }

        [Property("ControlNumeric", ColumnType = "Double")]
        public virtual double? ControlNumeric
        {
            get
            {
                return this._controlNumeric;
            }
            set
            {
                this._controlNumeric = value;
            }
        }

        [Property("ControlText", ColumnType = "String")]
        public virtual string ControlText
        {
            get
            {
                return this._controlText;
            }
            set
            {
                this._controlText = value;
            }
        }

        [BelongsTo("ControlGroupKey", NotNull = false)]
        public virtual ControlGroup_DAO ControlGroup
        {
            get
            {
                return this._controlGroup;
            }
            set
            {
                this._controlGroup = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ControlNumber", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }
    }
}