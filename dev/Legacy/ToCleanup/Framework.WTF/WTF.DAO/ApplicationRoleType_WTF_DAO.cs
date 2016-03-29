
using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferRoleType", Schema = "dbo")]
    public partial class ApplicationRoleType_WTF_DAO : DB_Test_WTF<ApplicationRoleType_WTF_DAO>
    {

        private string _description;

        private int _applicationRoleTypeGroupKey;

        private int _key;

        private IList<ApplicationRoleTypeOrganisationStructureMapping_WTF_DAO> _applicationRoleTypeOrganisationStructureMappings;

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

        [Property("OfferRoleTypeGroupKey", ColumnType = "Int32", NotNull = true)]
        public virtual int ApplicationRoleTypeGroupKey
        {
            get
            {
                return this._applicationRoleTypeGroupKey;
            }
            set
            {
                this._applicationRoleTypeGroupKey = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OfferRoleTypeKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ApplicationRoleTypeOrganisationStructureMapping_WTF_DAO), ColumnKey = "OfferRoleTypeKey", Table = "OfferRoleTypeOrganisationStructureMapping", Lazy=true)]
        public virtual IList<ApplicationRoleTypeOrganisationStructureMapping_WTF_DAO> ApplicationRoleTypeOrganisationStructureMappings
        {
            get
            {
                return this._applicationRoleTypeOrganisationStructureMappings;
            }
            set
            {
                this._applicationRoleTypeOrganisationStructureMappings = value;
            }
        }
    }
}

