using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("UserOrganisationStructure", Schema = "dbo", Lazy = true)]
    public partial class UserOrganisationStructure_DAO : DB_2AM<UserOrganisationStructure_DAO>
    {
        private ADUser_DAO _aDUser;

        private OrganisationStructure_DAO _organisationStructure;

        private int _key;

        private int _genericKey;

        private GenericKeyType_DAO _genericKeyType;

        private GeneralStatus_DAO _generalStatus;

        private IList<UserOrganisationStructureRoundRobinStatus_DAO> _userOrganisationStructureRoundRobinStatus;

        [BelongsTo(Column = "ADUserKey", NotNull = true)]
        [ValidateNonEmpty("AD User is a mandatory field")]
        public virtual ADUser_DAO ADUser
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

        [BelongsTo(Column = "OrganisationStructureKey", NotNull = true)]
        [ValidateNonEmpty("Organisation Structure is a mandatory field")]
        public virtual OrganisationStructure_DAO OrganisationStructure
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

        [Property("GenericKey", ColumnType = "Int32", NotNull = false)]
        public virtual int GenericKey
        {
            get
            {
                return this._genericKey;
            }
            set
            {
                this._genericKey = value;
            }
        }

        [BelongsTo(Column = "GenericKeyTypeKey", NotNull = false)]
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

        [BelongsTo(Column = "GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status Key is a mandatory field")]
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

        [HasMany(typeof(UserOrganisationStructureRoundRobinStatus_DAO), ColumnKey = "UserOrganisationStructureKey", Table = "UserOrganisationStructureRoundRobinStatus", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<UserOrganisationStructureRoundRobinStatus_DAO> UserOrganisationStructureRoundRobinStatus
        {
            get
            {
                return this._userOrganisationStructureRoundRobinStatus;
            }
            set
            {
                this._userOrganisationStructureRoundRobinStatus = value;
            }
        }
    }
}