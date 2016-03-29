using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("UserOrganisationStructureRoundRobinStatus", Schema = "dbo")]
    public partial class UserOrganisationStructureRoundRobinStatus_DAO : DB_2AM<UserOrganisationStructureRoundRobinStatus_DAO>
    {
        private int _key;

        private GeneralStatus_DAO _generalStatus;

        private GeneralStatus_DAO _capitecGeneralStatus;

        private UserOrganisationStructure_DAO _userOrganisationStructure;

        [PrimaryKey(PrimaryKeyType.Native, "UserOrganisationStructureRoundRobinStatusKey", ColumnType = "Int32")]
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

        [BelongsTo("GeneralStatusKey", NotNull = true)]
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

        [BelongsTo("CapitecGeneralStatusKey", NotNull = true)]
        public virtual GeneralStatus_DAO CapitecGeneralStatus
        {
            get
            {
                return this._capitecGeneralStatus;
            }
            set
            {
                this._capitecGeneralStatus = value;
            }
        }

        [BelongsTo("UserOrganisationStructureKey", NotNull = true)]
        public virtual UserOrganisationStructure_DAO UserOrganisationStructure
        {
            get
            {
                return this._userOrganisationStructure;
            }
            set
            {
                this._userOrganisationStructure = value;
            }
        }
    }
}