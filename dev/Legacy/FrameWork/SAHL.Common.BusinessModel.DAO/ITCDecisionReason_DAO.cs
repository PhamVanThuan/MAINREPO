using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ITCDecisionReason", Schema = "dbo")]
    public partial class ITCDecisionReason_DAO : DB_2AM<ITCDecisionReason_DAO>
    {
        private int _key;
        private CreditScoreDecision_DAO _creditScoreDecision;
        private ITCCreditScore_DAO _iTCCreditScore;
        private ApplicationCreditScore_DAO _applicationCreditScore;
        private Reason_DAO _reason;

        [PrimaryKey(PrimaryKeyType.Native, "ITCDecisionReasonKey", ColumnType = "Int32")]
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

        [BelongsTo("CreditScoreDecisionKey", NotNull = true)]
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

        [BelongsTo("ITCCreditScoreKey", NotNull = true)]
        public virtual ITCCreditScore_DAO ITCCreditScore
        {
            get
            {
                return this._iTCCreditScore;
            }
            set
            {
                this._iTCCreditScore = value;
            }
        }

        [BelongsTo("OfferCreditScoreKey", NotNull = false)]
        public virtual ApplicationCreditScore_DAO ApplicationCreditScore
        {
            get
            {
                return this._applicationCreditScore;
            }
            set
            {
                this._applicationCreditScore = value;
            }
        }

        [BelongsTo("ReasonKey", NotNull = false)]
        public virtual Reason_DAO Reason
        {
            get
            {
                return this._reason;
            }
            set
            {
                this._reason = value;
            }
        }
    }
}