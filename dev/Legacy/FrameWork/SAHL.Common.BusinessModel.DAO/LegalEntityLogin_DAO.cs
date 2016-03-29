using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Legal Entity Login DAO
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("LegalEntityLogin", Schema = "dbo", Lazy = true)]
    public class LegalEntityLogin_DAO : DB_2AM<LegalEntityLogin_DAO>
    {
        private int _key;

        private string _username;

        private string _password;

        private System.DateTime? _lastLoginDate;

        private GeneralStatus_DAO _generalStatus;

        private LegalEntity_DAO _legalEntity;

        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityLoginKey", ColumnType = "Int32")]
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

        [Property("Username", ColumnType = "String", NotNull = true)]
        public virtual string Username
        {
            get
            {
                return this._username;
            }
            set
            {
                this._username = value;
            }
        }

        [Property("Password", ColumnType = "String", NotNull = true)]
        public virtual string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        [Property("LastLoginDate", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? LastLoginDate
        {
            get
            {
                return this._lastLoginDate;
            }
            set
            {
                this._lastLoginDate = value;
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

        [BelongsTo(Column = "LegalEntityKey", NotNull = true)]
        public virtual LegalEntity_DAO LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }
    }
}