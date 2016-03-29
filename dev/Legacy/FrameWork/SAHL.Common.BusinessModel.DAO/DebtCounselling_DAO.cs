using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("DebtCounselling", Schema = "DebtCounselling")]
    public class DebtCounselling_DAO : DB_2AM<DebtCounselling_DAO>
    {
        private int _key;

        private DateTime? _paymentReceivedDate;

        private double? _paymentReceivedAmount;

        private Account_DAO _account;

        private DebtCounsellingStatus_DAO _debtCounsellingStatus;

        private DebtCounsellingGroup_DAO _debtCounsellingGroup;

        private DateTime? _diaryDate;

        private string _referenceNumber;

        private IList<HearingDetail_DAO> _hearingDetails;

        private IList<Proposal_DAO> _proposals;

        [PrimaryKey(PrimaryKeyType.Native, "DebtCounsellingKey", ColumnType = "Int32")]
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

        [Property("PaymentReceivedDate", ColumnType = "DateTime", NotNull = false)]
        public virtual DateTime? PaymentReceivedDate
        {
            get
            {
                return this._paymentReceivedDate;
            }
            set
            {
                this._paymentReceivedDate = value;
            }
        }

        [Property("PaymentReceivedAmount", ColumnType = "Double", NotNull = false)]
        public virtual double? PaymentReceivedAmount
        {
            get
            {
                return this._paymentReceivedAmount;
            }
            set
            {
                this._paymentReceivedAmount = value;
            }
        }

        [BelongsTo("AccountKey", NotNull = true)]
        public virtual Account_DAO Account
        {
            get
            {
                return this._account;
            }
            set
            {
                this._account = value;
            }
        }

        [BelongsTo("DebtCounsellingStatusKey", NotNull = true)]
        public virtual DebtCounsellingStatus_DAO DebtCounsellingStatus
        {
            get
            {
                return this._debtCounsellingStatus;
            }
            set
            {
                this._debtCounsellingStatus = value;
            }
        }

        [BelongsTo("DebtCounsellingGroupKey", NotNull = true)]
        public virtual DebtCounsellingGroup_DAO DebtCounsellingGroup
        {
            get
            {
                return this._debtCounsellingGroup;
            }
            set
            {
                this._debtCounsellingGroup = value;
            }
        }

        [Property("DiaryDate", ColumnType = "DateTime", NotNull = false)]
        public virtual DateTime? DiaryDate
        {
            get
            {
                return this._diaryDate;
            }
            set
            {
                this._diaryDate = value;
            }
        }

        [Property("ReferenceNumber", ColumnType = "String", NotNull = false)]
        public virtual string ReferenceNumber
        {
            get
            {
                return this._referenceNumber;
            }
            set
            {
                this._referenceNumber = value;
            }
        }

        [HasMany(typeof(HearingDetail_DAO), ColumnKey = "DebtCounsellingKey", Lazy = true, Table = "HearingDetail", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<HearingDetail_DAO> HearingDetails
        {
            get
            {
                return this._hearingDetails;
            }
            set
            {
                this._hearingDetails = value;
            }
        }

        [HasMany(typeof(Proposal_DAO), ColumnKey = "DebtCounsellingKey", Lazy = true, Table = "Proposal", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<Proposal_DAO> Proposals
        {
            get
            {
                return this._proposals;
            }
            set
            {
                this._proposals = value;
            }
        }
    }
}