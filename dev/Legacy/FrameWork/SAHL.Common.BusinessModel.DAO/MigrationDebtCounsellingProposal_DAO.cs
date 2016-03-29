using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("DebtCounsellingProposal", Schema = "migration")]
    public partial class MigrationDebtCounsellingProposal_DAO : DB_2AM<MigrationDebtCounsellingProposal_DAO>
    {
        private int _key;

        private int _proposalStatusKey;

        private bool _hOCInclusive;

        private bool _lifeInclusive;

        private bool _acceptedProposal;

        private System.DateTime _proposalDate;

        private IList<MigrationDebtCounsellingProposalItem_DAO> _debtCounsellingProposalItems;

        private MigrationDebtCounselling_DAO _debtCounselling;

        [PrimaryKey(PrimaryKeyType.Native, "DebtCounsellingProposalKey", ColumnType = "Int32")]
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

        [Property("ProposalStatusKey", ColumnType = "Int32", NotNull = true)]
        public virtual int ProposalStatusKey
        {
            get
            {
                return this._proposalStatusKey;
            }
            set
            {
                this._proposalStatusKey = value;
            }
        }

        [Property("HOCInclusive", ColumnType = "Boolean", NotNull = true)]
        public virtual bool HOCInclusive
        {
            get
            {
                return this._hOCInclusive;
            }
            set
            {
                this._hOCInclusive = value;
            }
        }

        [Property("LifeInclusive", ColumnType = "Boolean", NotNull = true)]
        public virtual bool LifeInclusive
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

        [Property("AcceptedProposal", ColumnType = "Boolean", NotNull = true)]
        public virtual bool AcceptedProposal
        {
            get
            {
                return this._acceptedProposal;
            }
            set
            {
                this._acceptedProposal = value;
            }
        }

        [Property("ProposalDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime ProposalDate
        {
            get
            {
                return this._proposalDate;
            }
            set
            {
                this._proposalDate = value;
            }
        }

        [HasMany(typeof(MigrationDebtCounsellingProposalItem_DAO), ColumnKey = "DebtCounsellingProposalKey", Table = "DebtCounsellingProposalItem", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<MigrationDebtCounsellingProposalItem_DAO> DebtCounsellingProposalItems
        {
            get
            {
                return this._debtCounsellingProposalItems;
            }
            set
            {
                this._debtCounsellingProposalItems = value;
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