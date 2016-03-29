using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("FutureDatedChange", Schema = "dbo")]
    public partial class FutureDatedChange_DAO : DB_2AM<FutureDatedChange_DAO>
    {
        private int _identifierReferenceKey;

        private System.DateTime _effectiveDate = DateTime.MinValue;     // initialised so we have a reference point for rules

        private bool _notificationRequired;

        private string _userID;

        private System.DateTime _insertDate;

        private System.DateTime _changeDate;

        private int _Key;

        private IList<FutureDatedChangeDetail_DAO> _futureDatedChangeDetails;

        private FutureDatedChangeType_DAO _futureDatedChangeType;

        [Property("IdentifierReferenceKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Identifier Reference Key is a mandatory field")]
        public virtual int IdentifierReferenceKey
        {
            get
            {
                return this._identifierReferenceKey;
            }
            set
            {
                this._identifierReferenceKey = value;
            }
        }

        [Property("EffectiveDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Effective Date is a mandatory field")]
        public virtual System.DateTime EffectiveDate
        {
            get
            {
                return this._effectiveDate;
            }
            set
            {
                this._effectiveDate = value;
            }
        }

        [Property("NotificationRequired", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Notification Required is a mandatory field")]
        public virtual bool NotificationRequired
        {
            get
            {
                return this._notificationRequired;
            }
            set
            {
                this._notificationRequired = value;
            }
        }

        [Property("UserID", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("User ID is a mandatory field")]
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

        [Property("InsertDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Insert Date is a mandatory field")]
        public virtual System.DateTime InsertDate
        {
            get
            {
                return this._insertDate;
            }
            set
            {
                this._insertDate = value;
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

        [PrimaryKey(PrimaryKeyType.Native, "FutureDatedChangeKey", ColumnType = "Int32")]
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

        [HasMany(typeof(FutureDatedChangeDetail_DAO), ColumnKey = "FutureDatedChangeKey", Table = "FutureDatedChangeDetail", Inverse = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan)]
        public virtual IList<FutureDatedChangeDetail_DAO> FutureDatedChangeDetails
        {
            get
            {
                return this._futureDatedChangeDetails;
            }
            set
            {
                this._futureDatedChangeDetails = value;
            }
        }

        [BelongsTo("FutureDatedChangeTypeKey", NotNull = true)]
        [ValidateNonEmpty("Future Dated Change Type is a mandatory field")]
        public virtual FutureDatedChangeType_DAO FutureDatedChangeType
        {
            get
            {
                return this._futureDatedChangeType;
            }
            set
            {
                this._futureDatedChangeType = value;
            }
        }
    }
}