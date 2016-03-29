using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Tuple;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("GenericSetDefinition", Schema = "dbo")]
    public partial class GenericSetDefinition_DAO : DB_2AM<GenericSetDefinition_DAO>
    {
        private string _description;

        private string _explanation;

        private int _key;

        private IList<GenericSet_DAO> _genericSets;

        private GenericSetType_DAO _genericSetType;

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

        [Property("Explanation", ColumnType = "String")]
        public virtual string Explanation
        {
            get
            {
                return this._explanation;
            }
            set
            {
                this._explanation = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "GenericSetDefinitionKey", ColumnType = "Int32")]
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

        [HasMany(typeof(GenericSet_DAO), ColumnKey = "GenericSetDefinitionKey", Table = "GenericSet", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<GenericSet_DAO> GenericSets
        {
            get
            {
                return this._genericSets;
            }
            set
            {
                this._genericSets = value;
            }
        }

        [BelongsTo("GenericSetTypeKey", NotNull = true)]
        public virtual GenericSetType_DAO GenericSetType
        {
            get
            {
                return this._genericSetType;
            }
            set
            {
                this._genericSetType = value;
            }
        }
    }
}