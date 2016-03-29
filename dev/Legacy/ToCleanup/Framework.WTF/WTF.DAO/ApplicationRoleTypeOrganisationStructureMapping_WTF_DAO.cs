
using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferRoleTypeOrganisationStructureMapping", Schema = "dbo")]
    public partial class ApplicationRoleTypeOrganisationStructureMapping_WTF_DAO : DB_Test_WTF<ApplicationRoleTypeOrganisationStructureMapping_WTF_DAO>
    {

        private int _key;

        private ApplicationRoleType_WTF_DAO _applicationRoleType;

        private OrganisationStructure_WTF_DAO _organisationStructure;

        [PrimaryKey(PrimaryKeyType.Native, "OfferRoleTypeOrganisationStructureMappingKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferRoleTypeKey", NotNull = true)]
        public virtual ApplicationRoleType_WTF_DAO ApplicationRoleType
        {
            get
            {
                return this._applicationRoleType;
            }
            set
            {
                this._applicationRoleType = value;
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

