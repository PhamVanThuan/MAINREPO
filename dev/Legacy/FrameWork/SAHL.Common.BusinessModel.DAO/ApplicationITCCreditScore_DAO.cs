using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferITCCreditScore", Schema = "dbo")]
    public partial class ApplicationITCCreditScore_DAO : DB_2AM<ApplicationITCCreditScore_DAO>
    {
        private int _key;
        private ApplicationCreditScore_DAO _applicationCreditScore;
        private ITCCreditScore_DAO _iTCCreditScore;
        private System.DateTime _scoreDate;
        private CreditScoreDecision_DAO _creditScoreDecision;
        private bool _primaryApplicant;

        [PrimaryKey(PrimaryKeyType.Native, "OfferITCCreditScoreKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferCreditScoreKey", NotNull = true)]
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

        [Property("ScoreDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime ScoreDate
        {
            get
            {
                return this._scoreDate;
            }
            set
            {
                this._scoreDate = value;
            }
        }

        [Property("PrimaryApplicant", ColumnType = "Boolean", NotNull = true)]
        public virtual bool PrimaryApplicant
        {
            get
            {
                return this._primaryApplicant;
            }
            set
            {
                this._primaryApplicant = value;
            }
        }
    }
}