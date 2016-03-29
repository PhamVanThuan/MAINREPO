
using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("UserOrganisationStructure", Schema = "dbo", Lazy = true)]
    public partial class UserOrganisationStructure_WTF_DAO : DB_Test_WTF<UserOrganisationStructure_WTF_DAO>
    {
        private int _key;

        private ADUser_WTF_DAO _aDUser;

        private OrganisationStructure_WTF_DAO _organisationStructure;

        [PrimaryKey(PrimaryKeyType.Native, "UserOrganisationStructureKey", ColumnType = "Int32")]
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

        [BelongsTo("ADUserKey", NotNull = true)]
        public virtual ADUser_WTF_DAO ADUser
        {
            get
            {
                return this._aDUser;
            }
            set
            {
                this._aDUser = value;
            }
        }

        [BelongsTo("OrganisationStructureKey", NotNull = true)]
        public virtual OrganisationStructure_WTF_DAO OrganisationStructure
        {
            get
            {
                return this._organisationStructure;
            }
            set
            {
                this._organisationStructure = value;
            }
        }
    }
}

