using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [Lurker(Lurkee = "Define a non-standard interface")]
    [ActiveRecord("OrganisationStructure", Schema = "dbo", Lazy = true)]
    public partial class OrganisationStructure_DAO : DB_2AM<OrganisationStructure_DAO>
    {
        private string _description;

        private OrganisationType_DAO _organisationType;

        private GeneralStatus_DAO _generalStatus;

        private int _key;

        private IList<OrganisationStructure_DAO> _childOrganisationStructures;

        private OrganisationStructure_DAO _parent;
        private IList<ADUser_DAO> _adUsers;
        private IList<ApplicationRoleType_DAO> _RoleTypes;
        private IList<LegalEntity_DAO> _LegalEntities;

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
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

        [BelongsTo(Column = "OrganisationTypeKey", NotNull = true)]
        [ValidateNonEmpty("Organisation Type is a mandatory field")]
        public virtual OrganisationType_DAO OrganisationType
        {
            get
            {
                return this._organisationType;
            }
            set
            {
                this._organisationType = value;
            }
        }

        [BelongsTo(Column = "GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
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

        [HasMany(typeof(OrganisationStructure_DAO), ColumnKey = "ParentKey", Table = "OrganisationStructure", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<OrganisationStructure_DAO> ChildOrganisationStructures
        {
            get
            {
                return this._childOrganisationStructures;
            }
            set
            {
                this._childOrganisationStructures = value;
            }
        }

        [BelongsTo("ParentKey", NotNull = false)]
        public virtual OrganisationStructure_DAO Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                this._parent = value;
            }
        }

        [HasAndBelongsToMany(typeof(ADUser_DAO), Table = "UserOrganisationStructure", ColumnKey = "OrganisationStructureKey", ColumnRef = "ADUserKey")]
        public virtual IList<ADUser_DAO> ADUsers
        {
            get
            {
                return _adUsers;
            }
            set
            {
                _adUsers = value;
            }
        }

        [HasAndBelongsToMany(typeof(ApplicationRoleType_DAO), Table = "OfferRoleTypeOrganisationStructureMapping", ColumnKey = "OrganisationStructureKey", ColumnRef = "OfferRoleTypeKey", Lazy = true)]
        public virtual IList<ApplicationRoleType_DAO> ApplicationRoleTypes
        {
            get { return this._RoleTypes; }
            set { _RoleTypes = value; }
        }

        [HasAndBelongsToMany(typeof(LegalEntity_DAO), Table = "LegalEntityOrganisationStructure", ColumnKey = "OrganisationStructureKey", ColumnRef = "LegalEntityKey")]
        public virtual IList<LegalEntity_DAO> LegalEntities
        {
            get
            {
                return _LegalEntities;
            }
            set
            {
                _LegalEntities = value;
            }
        }
    }
}