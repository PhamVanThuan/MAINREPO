using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("UserOrganisationStructureHistory", Schema = "dbo")]
    public partial class UserOrganisationStructureHistory_DAO : DB_2AM<UserOrganisationStructureHistory_DAO>
    {
        private int _userOrganisationStructureKey;

        private OrganisationStructure_DAO _organisationStructure;

        private int _genericKey;

        private GenericKeyType_DAO _genericKeyType;

        private GeneralStatus_DAO _generalStatus;

        private System.DateTime _changeDate;

        private char _action;

        private int _Key;

        private ADUser_DAO _aDUser;

        [Property("UserOrganisationStructureKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("User Organisation Structure Key is a mandatory field")]
        public virtual int UserOrganisationStructureKey
        {
            get
            {
                return this._userOrganisationStructureKey;
            }
            set
            {
                this._userOrganisationStructureKey = value;
            }
        }

        [BelongsTo("OrganisationStructureKey", NotNull = true)]
        [ValidateNonEmpty("Organisation Structure is a mandatory field")]
        public virtual OrganisationStructure_DAO OrganisationStructureKey
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

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Change Date is a mandatory field")]
        public virtual System.DateTime ChangeDate
        {
            get
            {
                return this._changeDate;
            }
            set
            {
                this._changeDate = value;
            }
        }

        [Property("Action", ColumnType = "AnsiChar", NotNull = true)]
        [ValidateNonEmpty("Action is a mandatory field")]
        public virtual char Action
        {
            get
            {
                return this._action;
            }
            set
            {
                this._action = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "UserOrganisationStructureHistoryKey", ColumnType = "Int32")]
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

        [BelongsTo("ADUserKey", NotNull = true)]
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
    }
}