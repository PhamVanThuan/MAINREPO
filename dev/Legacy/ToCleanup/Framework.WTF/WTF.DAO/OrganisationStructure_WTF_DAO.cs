
using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OrganisationStructure", Schema = "dbo")]
    public partial class OrganisationStructure_WTF_DAO : DB_Test_WTF<OrganisationStructure_WTF_DAO>
    {

        private int _parentKey;

        private string _description;

        private int _organisationTypeKey;

        private int _generalStatusKey;

        private int _key;

        private IList<UserOrganisationStructure_WTF_DAO> _userOrganisationStructures;

        private IList<ApplicationRoleTypeOrganisationStructureMapping_WTF_DAO> _applicationRoleTypeOrganisationStructureMappings;

        [Property("ParentKey", ColumnType = "Int32")]
        public virtual int ParentKey
        {
            get
            {
                return this._parentKey;
            }
            set
            {
                this._parentKey = value;
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

        [Property("OrganisationTypeKey", ColumnType = "Int32", NotNull = true)]
        public virtual int OrganisationTypeKey
        {
            get
            {
                return this._organisationTypeKey;
            }
            set
            {
                this._organisationTypeKey = value;
            }
        }

        [Property("GeneralStatusKey", ColumnType = "Int32", NotNull = true)]
        public virtual int GeneralStatusKey
        {
            get
            {
                return this._generalStatusKey;
            }
            set
            {
                this._generalStatusKey = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OrganisationStructureKey", ColumnType = "Int32")]
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

        [HasMany(typeof(UserOrganisationStructure_WTF_DAO), ColumnKey = "OrganisationStructureKey", Table = "UserOrganisationStructure")]
        public virtual IList<UserOrganisationStructure_WTF_DAO> UserOrganisationStructures
        {
            get
            {
                return this._userOrganisationStructures;
            }
            set
            {
                this._userOrganisationStructures = value;
            }
        }

        [HasMany(typeof(ApplicationRoleTypeOrganisationStructureMapping_WTF_DAO), ColumnKey = "OrganisationStructureKey", Table = "OfferRoleTypeOrganisationStructureMapping")]
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

