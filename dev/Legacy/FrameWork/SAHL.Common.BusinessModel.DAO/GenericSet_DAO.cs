using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("GenericSet", Schema = "dbo")]
    public partial class GenericSet_DAO : DB_2AM<GenericSet_DAO>
    {
        private int _genericKey;

        private int _key;

        private GenericSetDefinition_DAO _genericSetDefinition;

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

        [PrimaryKey(PrimaryKeyType.Native, "GenericSetKey", ColumnType = "Int32")]
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

        [BelongsTo("GenericSetDefinitionKey", NotNull = true)]
        public virtual GenericSetDefinition_DAO GenericSetDefinition
        {
            get
            {
                return this._genericSetDefinition;
            }
            set
            {
                this._genericSetDefinition = value;
            }
        }
    }
}