using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("OfferAggregateDecision", Schema = "dbo")]
    public partial class ApplicationAggregateDecision_DAO : DB_2AM<ApplicationAggregateDecision_DAO>
    {
        private int _key;
        private CreditScoreDecision_DAO _creditScoreDecision;
        private int _primaryDecisionKey;
        private int? _secondaryDecisionKey;

        [PrimaryKey(PrimaryKeyType.Assigned, "OfferAggregateDecisionKey", ColumnType = "Int32")]
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

        [BelongsTo("AggregateDecision", NotNull = true)]
        public virtual CreditScoreDecision_DAO CreditScoreDecision
        {
            get
            {
                return this._creditScoreDecision;
            }
            set
            {
                this._creditScoreDecision = value;
            }
        }

        [Property("PrimaryDecision", ColumnType = "System.Int32", NotNull = true)]
        public virtual int PrimaryDecisionKey
        {
            get { return _primaryDecisionKey; }
            set { _primaryDecisionKey = value; }
        }

        [Property("SecondaryDecision", ColumnType = "System.Int32", NotNull = false)]
        public virtual int? SecondaryDecisionKey
        {
            get { return _secondaryDecisionKey; }
            set { _secondaryDecisionKey = value; }
        }
    }
}