using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("InternetLeadUsers", Schema = "dbo")]
    public partial class InternetLeadUsers_DAO : DB_2AM<InternetLeadUsers_DAO>
    {
        //private int _aDUserKey;
        private ADUser_DAO _aDUser;

        private bool _flag;

        private int _caseCount;

        private int _lastCaseKey;

        private int _key;

        private GeneralStatus_DAO _generalStatus;

        [Property("Flag", ColumnType = "Boolean")]
        public virtual bool Flag
        {
            get
            {
                return this._flag;
            }
            set
            {
                this._flag = value;
            }
        }

        [Property("CaseCount", ColumnType = "Int32")]
        public virtual int CaseCount
        {
            get
            {
                return this._caseCount;
            }
            set
            {
                this._caseCount = value;
            }
        }

        [Property("LastCaseKey", ColumnType = "Int32")]
        public virtual int LastCaseKey
        {
            get
            {
                return this._lastCaseKey;
            }
            set
            {
                this._lastCaseKey = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "InternetLeadUsersKey", ColumnType = "Int32")]
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
        [ValidateNonEmpty("GeneralStatusKey is a mandatory field")]
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

        [BelongsTo("ADUserKey", NotNull = true)]
        [ValidateNonEmpty("ADUserKey is a mandatory field")]
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

        //[Property("ADUserKey", ColumnType = "Int32", NotNull = true)]
        ////public virtual int ADUserKey
        //{
        //    get
        //    {
        //        return this._aDUserKey;
        //    }
        //    set
        //    {
        //        this._aDUser = _aDUserKey;
        //    }
        //}
    }
}