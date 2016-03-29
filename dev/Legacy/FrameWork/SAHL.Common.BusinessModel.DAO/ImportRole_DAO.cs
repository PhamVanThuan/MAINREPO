using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ImportRole", Schema = "dbo")]
    public partial class ImportRole_DAO : DB_2AM<ImportRole_DAO>
    {

        private string _roleTypeKey;

        private int _key;

        private ImportLegalEntity_DAO _importLegalEntity;

        [Property("RoleTypeKey", ColumnType = "String")]
        public virtual string RoleTypeKey
        {
            get
            {
                return this._roleTypeKey;
            }
            set
            {
                this._roleTypeKey = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "RoleKey", ColumnType = "Int32")]
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

        [BelongsTo("LegalEntityKey", NotNull = true)]
        public virtual ImportLegalEntity_DAO ImportLegalEntity
        {
            get
            {
                return this._importLegalEntity;
            }
            set
            {
                this._importLegalEntity = value;
            }
        }
    }
}
