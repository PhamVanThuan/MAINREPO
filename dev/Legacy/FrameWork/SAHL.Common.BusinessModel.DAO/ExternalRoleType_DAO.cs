using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ExternalRoleType", Schema = "dbo")]
    public partial class ExternalRoleType_DAO : DB_2AM<ExternalRoleType_DAO>
    {
        private int _key;

        private string _description;

        private ExternalRoleTypeGroup_DAO _externalRoleTypeGroup;

        [PrimaryKey(PrimaryKeyType.Assigned, "ExternalRoleTypeKey", ColumnType = "Int32")]
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

        [BelongsTo("ExternalRoleTypeGroupKey")]
        public virtual ExternalRoleTypeGroup_DAO ExternalRoleTypeGroup
        {
            get
            {
                return this._externalRoleTypeGroup;
            }
            set
            {
                this._externalRoleTypeGroup = value;
            }
        }
    }
}