using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Operator", Schema = "dbo")]
    public partial class Operator_DAO : DB_2AM<Operator_DAO>
    {
        private string _description;

        private int _key;

        private OperatorGroup_DAO _operatorGroup;

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OperatorKey", ColumnType = "Int32")]
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

        [BelongsTo("OperatorGroupKey", NotNull = true)]
        [ValidateNonEmpty("Operator Group is a mandatory field")]
        public virtual OperatorGroup_DAO OperatorGroup
        {
            get
            {
                return this._operatorGroup;
            }
            set
            {
                this._operatorGroup = value;
            }
        }
    }
}