using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Account_ArrearSnapshot", Lazy = true, Schema = "dbo")]
    public partial class Account_ArrearSnapshot_DAO : DB_2AM<Account_ArrearSnapshot_DAO>
    {

        private double _fixedPayment;

        private int _accountStatusKey;

        private System.DateTime _insertedDate;

        private int _originationSourceProductKey;

        private System.DateTime? _openDate;

        private System.DateTime? _closeDate;

        private int? _rRR_ProductKey;

        private int? _rRR_OriginationSourceKey;

        private string _userID;

        private System.DateTime _changeDate;

        private int _Key;

        [Property("FixedPayment", ColumnType = "Double", NotNull = true)]
        public virtual double FixedPayment
        {
            get
            {
                return this._fixedPayment;
            }
            set
            {
                this._fixedPayment = value;
            }
        }

        [Property("AccountStatusKey", ColumnType = "Int32", NotNull = true)]
        public virtual int AccountStatusKey
        {
            get
            {
                return this._accountStatusKey;
            }
            set
            {
                this._accountStatusKey = value;
            }
        }

        [Property("InsertedDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime InsertedDate
        {
            get
            {
                return this._insertedDate;
            }
            set
            {
                this._insertedDate = value;
            }
        }

        [Property("OriginationSourceProductKey", ColumnType = "Int32")]
        public virtual int OriginationSourceProductKey
        {
            get
            {
                return this._originationSourceProductKey;
            }
            set
            {
                this._originationSourceProductKey = value;
            }
        }

        [Property("OpenDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? OpenDate
        {
            get
            {
                return this._openDate;
            }
            set
            {
                this._openDate = value;
            }
        }

        [Property("CloseDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? CloseDate
        {
            get
            {
                return this._closeDate;
            }
            set
            {
                this._closeDate = value;
            }
        }

        [Property("RRR_ProductKey", ColumnType = "Int32")]
        public virtual int? RRR_ProductKey
        {
            get
            {
                return this._rRR_ProductKey;
            }
            set
            {
                this._rRR_ProductKey = value;
            }
        }

        [Property("RRR_OriginationSourceKey", ColumnType = "Int32")]
        public virtual int? RRR_OriginationSourceKey
        {
            get
            {
                return this._rRR_OriginationSourceKey;
            }
            set
            {
                this._rRR_OriginationSourceKey = value;
            }
        }

        [Property("UserID", ColumnType = "String", NotNull = true)]
        public virtual string UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
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

        [PrimaryKey(PrimaryKeyType.Native, "AccountKey", ColumnType = "Int32")]
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
    }
}
