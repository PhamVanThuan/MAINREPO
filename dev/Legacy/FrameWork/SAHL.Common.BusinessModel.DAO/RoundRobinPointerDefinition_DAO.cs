using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("RoundRobinPointerDefinition", Schema = "dbo")]
    public partial class RoundRobinPointerDefinition_DAO : DB_2AM<RoundRobinPointerDefinition_DAO>
    {
        private int _genericKey;

        private string _applicationName;

        private string _statementName;

        private int _key;

        private RoundRobinPointer_DAO _roundRobinPointer;

        private GeneralStatus_DAO _generalStatus;

        private GenericKeyType_DAO _genericKeyType;

        [Property("GenericKey", ColumnType = "Int32", NotNull = true)]
        public virtual int GenericKey
        {
            get
            {
                return this._genericKey;
            }
            set
            {
                this._genericKey = value;
            }
        }

        [Property("ApplicationName", ColumnType = "String", NotNull = true)]
        public virtual string ApplicationName
        {
            get
            {
                return this._applicationName;
            }
            set
            {
                this._applicationName = value;
            }
        }

        [Property("StatementName", ColumnType = "String", NotNull = true)]
        public virtual string StatementName
        {
            get
            {
                return this._statementName;
            }
            set
            {
                this._statementName = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "RoundRobinPointerDefinitionKey", ColumnType = "Int32")]
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

        [BelongsTo("RoundRobinPointerKey", NotNull = true)]
        public virtual RoundRobinPointer_DAO RoundRobinPointer
        {
            get
            {
                return this._roundRobinPointer;
            }
            set
            {
                this._roundRobinPointer = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
        public virtual GenericKeyType_DAO GenericKeyType
        {
            get
            {
                return this._genericKeyType;
            }
            set
            {
                this._genericKeyType = value;
            }
        }
    }
}