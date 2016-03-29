using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferCreditScore", Schema = "dbo")]
    public partial class ApplicationCreditScore_DAO : DB_2AM<ApplicationCreditScore_DAO>
    {
        private int _key;
        private Application_DAO _application;
        private ApplicationAggregateDecision_DAO _applicationAggregateDecision;
        private System.DateTime _scoreDate;
        private CallingContext_DAO _callingContext;
        private IList<ITCDecisionReason_DAO> _iTCDecisionReasons;
        private IList<ApplicationITCCreditScore_DAO> _applicationITCCreditScores;

        [PrimaryKey(PrimaryKeyType.Native, "OfferCreditScoreKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferKey", NotNull = true)]
        public virtual Application_DAO Application
        {
            get
            {
                return this._application;
            }
            set
            {
                this._application = value;
            }
        }

        [BelongsTo("OfferAggregateDecisionKey", NotNull = true)]
        public virtual ApplicationAggregateDecision_DAO ApplicationAggregateDecision
        {
            get
            {
                return this._applicationAggregateDecision;
            }
            set
            {
                this._applicationAggregateDecision = value;
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

        [BelongsTo("CallingContextKey", NotNull = true)]
        public virtual CallingContext_DAO CallingContext
        {
            get
            {
                return this._callingContext;
            }
            set
            {
                this._callingContext = value;
            }
        }

        [HasMany(typeof(ITCDecisionReason_DAO), ColumnKey = "OfferCreditScoreKey", Table = "ITCDecisionReason", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ITCDecisionReason_DAO> ITCDecisionReasons
        {
            get
            {
                return this._iTCDecisionReasons;
            }
            set
            {
                this._iTCDecisionReasons = value;
            }
        }

        /// <summary>
        /// An ApplicationCreditScore can have a many ApplicationITCCreditScores associated with it. This relationship is defined in the ApplicationITCCreditScore table where the
        /// Offer.OfferKey = OfferITCCreditScore.OfferKey.
        /// </summary>
        [HasMany(typeof(ApplicationITCCreditScore_DAO), ColumnKey = "OfferCreditScoreKey", Table = "OfferITCCreditScore", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ApplicationITCCreditScore_DAO> ApplicationITCCreditScores
        {
            get
            {
                return this._applicationITCCreditScores;
            }
            set
            {
                this._applicationITCCreditScores = value;
            }
        }
    }
}