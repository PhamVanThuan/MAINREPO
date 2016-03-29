using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("DebtCounsellingExternalRole", Schema = "migration")]
    public partial class MigrationDebtCounsellingExternalRole_DAO : DB_2AM<MigrationDebtCounsellingExternalRole_DAO>
    {
        private int _key;

        private int _legalEntityKey;

        private int _externalRoleTypeKey;

        private MigrationDebtCounselling_DAO _debtCounselling;

        [PrimaryKey(PrimaryKeyType.Native, "DebtCounsellingExternalRoleKey", ColumnType = "Int32")]
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

        [Property("LegalEntityKey", ColumnType = "Int32", NotNull = true)]
        public virtual int LegalEntityKey
        {
            get
            {
                return this._legalEntityKey;
            }
            set
            {
                this._legalEntityKey = value;
            }
        }

        [Property("ExternalRoleTypeKey", ColumnType = "Int32", NotNull = true)]
        public virtual int ExternalRoleTypeKey
        {
            get
            {
                return this._externalRoleTypeKey;
            }
            set
            {
                this._externalRoleTypeKey = value;
            }
        }

        [BelongsTo("DebtCounsellingKey", NotNull = true)]
        public virtual MigrationDebtCounselling_DAO DebtCounselling
        {
            get
            {
                return this._debtCounselling;
            }
            set
            {
                this._debtCounselling = value;
            }
        }
    }
}