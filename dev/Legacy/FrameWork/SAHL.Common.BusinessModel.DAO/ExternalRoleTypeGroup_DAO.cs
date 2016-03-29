using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ExternalRoleTypeGroup", Schema = "dbo")]
    public partial class ExternalRoleTypeGroup_DAO : DB_2AM<ExternalRoleTypeGroup_DAO>
    {
        private int _key;

        private string _description;

        private IList<ExternalRoleType_DAO> _externalRoleTypes;

        [PrimaryKey(PrimaryKeyType.Native, "ExternalRoleTypeGroupKey", ColumnType = "Int32")]
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

        [Property("Description", ColumnType = "String")]
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

        [HasMany(typeof(ExternalRoleType_DAO), ColumnKey = "ExternalRoleTypeGroupKey", Table = "ExternalRoleType")]
        public virtual IList<ExternalRoleType_DAO> ExternalRoleTypes
        {
            get
            {
                return this._externalRoleTypes;
            }
            set
            {
                this._externalRoleTypes = value;
            }
        }
    }
}