using System;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest]
    [ActiveRecord("ExternalRoleDomicilium", Schema = "dbo", Lazy = true)]
    public class ExternalRoleDomicilium_DAO : DB_2AM<ExternalRoleDomicilium_DAO>
    {
        private int _key;
        private LegalEntityDomicilium_DAO _legalEntityDomicilium;
        private ExternalRole_DAO _externalRole;
        private DateTime? _changeDate;
        private ADUser_DAO _adUserDAO;

        [PrimaryKey(PrimaryKeyType.Native, "ExternalRoleDomiciliumKey", ColumnType = "Int32")]
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

        [BelongsTo("LegalEntityDomiciliumKey", NotNull = true)]
        public virtual LegalEntityDomicilium_DAO LegalEntityDomicilium
        {
            get
            {
                return this._legalEntityDomicilium;
            }
            set
            {
                this._legalEntityDomicilium = value;
            }
        }

        [BelongsTo("ExternalRoleKey", NotNull = true)]
        public virtual ExternalRole_DAO ExternalRole
        {
            get
            {
                return this._externalRole;
            }
            set
            {
                this._externalRole = value;
            }
        }

        [BelongsTo("ADUserKey", NotNull = true)]
        public virtual ADUser_DAO ADUser
        {
            get
            {
                return this._adUserDAO;
            }
            set
            {
                this._adUserDAO = value;
            }
        }

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime? ChangeDate
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
    }
}