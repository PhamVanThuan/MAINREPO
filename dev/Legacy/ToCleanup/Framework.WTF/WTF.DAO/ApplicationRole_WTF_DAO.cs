
using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferRole", Schema = "dbo")]
    public partial class ApplicationRole_WTF_DAO : DB_Test_WTF<ApplicationRole_WTF_DAO>
    {

        private int _applicationRoleTypeKey;

        private System.DateTime? _statusChangeDate;

        private int _key;

        private GeneralStatus_WTF_DAO _generalStatus;

        private int _legalEntityKey;

        private int _applicationKey;

        [Property("OfferRoleTypeKey", ColumnType = "Int32", NotNull = true)]
        public virtual int ApplicationRoleTypeKey
        {
            get
            {
                return this._applicationRoleTypeKey;
            }
            set
            {
                this._applicationRoleTypeKey = value;
            }
        }

        [Property("StatusChangeDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime? StatusChangeDate
        {
            get
            {
                return this._statusChangeDate;
            }
            set
            {
                this._statusChangeDate = value;
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

        [Property("OfferKey", ColumnType = "Int32", NotNull = true)]
        public virtual int ApplicationKey
        {
            get
            {
                return this._applicationKey;
            }
            set
            {
                this._applicationKey = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OfferRoleKey", ColumnType = "Int32")]
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
        public virtual GeneralStatus_WTF_DAO GeneralStatus
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
    }
}

