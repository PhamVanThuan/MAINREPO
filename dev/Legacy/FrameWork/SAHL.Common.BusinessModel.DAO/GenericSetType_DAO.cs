using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("GenericSetType", Schema = "dbo")]
    public partial class GenericSetType_DAO : DB_2AM<GenericSetType_DAO>
    {
        private string _description;

        private GenericKeyType_DAO _genericKeyType;

        private int _key;

        private IList<GenericSetDefinition_DAO> _genericSetDefinitions;

        [Property("Description", ColumnType = "String", NotNull = true)]
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

        [PrimaryKey(PrimaryKeyType.Native, "GenericSetTypeKey", ColumnType = "Int32")]
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

        [HasMany(typeof(GenericSetDefinition_DAO), ColumnKey = "GenericSetTypeKey", Table = "GenericSetDefinition")]
        public virtual IList<GenericSetDefinition_DAO> GenericSetDefinitions
        {
            get
            {
                return this._genericSetDefinitions;
            }
            set
            {
                this._genericSetDefinitions = value;
            }
        }
    }
}