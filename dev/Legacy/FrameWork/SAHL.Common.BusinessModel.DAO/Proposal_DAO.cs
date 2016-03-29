using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Proposal", Schema = "debtcounselling", Lazy = false)]
    public partial class Proposal_DAO : DB_2AM<Proposal_DAO>
    {
        private int _key;

        private DebtCounselling_DAO _debtCounselling;

        private ADUser_DAO _aDUser;

        private System.DateTime _createDate;

        private IList<ProposalItem_DAO> _proposalItems;

        private ProposalStatus_DAO _proposalStatus;

        private ProposalType_DAO _proposalType;

        private bool? _hocInclusive;

        private bool? _lifeInclusive;

        private bool? _accepted;

        private System.DateTime? _reviewDate;

        private bool _monthlyServiceFee;

        [PrimaryKey(PrimaryKeyType.Native, "ProposalKey", ColumnType = "Int32")]
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

        [BelongsTo("DebtCounsellingKey", NotNull = true)]
        public virtual DebtCounselling_DAO DebtCounselling
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

        [BelongsTo(Column = "ADUserKey", NotNull = true)]
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

        [Property("CreateDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime CreateDate
        {
            get
            {
                return this._createDate;
            }
            set
            {
                this._createDate = value;
            }
        }

        [HasMany(typeof(ProposalItem_DAO), ColumnKey = "ProposalKey", Table = "ProposalItem", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ProposalItem_DAO> ProposalItems
        {
            get
            {
                return this._proposalItems;
            }
            set
            {
                this._proposalItems = value;
            }
        }

        [BelongsTo("ProposalStatusKey", NotNull = true)]
        public virtual ProposalStatus_DAO ProposalStatus
        {
            get
            {
                return this._proposalStatus;
            }
            set
            {
                this._proposalStatus = value;
            }
        }

        [BelongsTo("ProposalTypeKey", NotNull = true)]
        public virtual ProposalType_DAO ProposalType
        {
            get
            {
                return this._proposalType;
            }
            set
            {
                this._proposalType = value;
            }
        }

        [Property("HOCInclusive", ColumnType = "Boolean", NotNull = true)]
        public virtual bool? HOCInclusive
        {
            get
            {
                return this._hocInclusive;
            }
            set
            {
                this._hocInclusive = value;
            }
        }

        [Property("LifeInclusive", ColumnType = "Boolean", NotNull = true)]
        public virtual bool? LifeInclusive
        {
            get
            {
                return this._lifeInclusive;
            }
            set
            {
                this._lifeInclusive = value;
            }
        }

        [Property("Accepted", ColumnType = "Boolean", NotNull = false)]
        public virtual bool? Accepted
        {
            get
            {
                return this._accepted;
            }
            set
            {
                this._accepted = value;
            }
        }

        [Property("ReviewDate", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? ReviewDate
        {
            get
            {
                return this._reviewDate;
            }
            set
            {
                this._reviewDate = value;
            }
        }

        [Property("MonthlyServiceFee", ColumnType = "Boolean", NotNull = true)]
        public virtual bool MonthlyServiceFeeInclusive
        {
            get
            {
                return this._monthlyServiceFee;
            }
            set
            {
                this._monthlyServiceFee = value;
            }
        }
    }
}