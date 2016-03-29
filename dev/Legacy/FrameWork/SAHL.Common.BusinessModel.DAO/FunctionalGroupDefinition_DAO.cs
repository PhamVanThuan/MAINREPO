using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("FunctionalGroupDefinition",  Schema = "dbo")]
    public partial class FunctionalGroupDefinition_DAO : DB_2AM<FunctionalGroupDefinition_DAO>
    {

        private string _functionalGroupName;

        private bool _allowMany;

        private int _Key;
                
        private IList<UserGroupMapping_DAO> _userGroupMappings;

        private GenericKeyType_DAO _genericKeyType;

        [Property("FunctionalGroupName", ColumnType = "String", NotNull = true)]
        public virtual string FunctionalGroupName
        {
            get
            {
                return this._functionalGroupName;
            }
            set
            {
                this._functionalGroupName = value;
            }
        }

        [Property("AllowMany", ColumnType = "Boolean", NotNull = true)]
        public virtual bool AllowMany
        {
            get
            {
                return this._allowMany;
            }
            set
            {
                this._allowMany = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "FunctionalGroupDefinitionKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        [HasMany(typeof(UserGroupMapping_DAO), ColumnKey = "FunctionalGroupDefinitionKey", Table = "UserGroupMapping", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<UserGroupMapping_DAO> UserGroupMappings
        {
            get
            {
                return this._userGroupMappings;
            }
            set
            {
                this._userGroupMappings = value;
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
