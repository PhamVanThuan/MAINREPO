using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ITCCreditScore", Schema = "dbo")]
    [GenericTest(Globals.TestType.Find)]
    public partial class ITCCreditScore_DAO : DB_2AM<ITCCreditScore_DAO>
    {
        private double? _empiricaScore;

        private double? _sBCScore;

        private System.DateTime _scoreDate;

        private string _ADUserName;

        private int _key;

        private IList<ITCDecisionReason_DAO> _iTCDecisionReasons;

        private CreditScoreDecision_DAO _creditScoreDecision;

        private GeneralStatus_DAO _generalStatus;

        private int _iTCKey;

        private ScoreCard_DAO _scoreCard;

        private RiskMatrixRevision_DAO _riskMatrixRevision;

        private RiskMatrixCell_DAO _cell;

        private LegalEntity_DAO _legalEntity;

        [Property("EmpiricaScore", ColumnType = "Double")]
        public virtual double? EmpiricaScore
        {
            get
            {
                return this._empiricaScore;
            }
            set
            {
                this._empiricaScore = value;
            }
        }

        [Property("SBCScore", ColumnType = "Double")]
        public virtual double? SBCScore
        {
            get
            {
                return this._sBCScore;
            }
            set
            {
                this._sBCScore = value;
            }
        }

        [Property("ADUserName", ColumnType = "String")]
        public virtual string ADUserName
        {
            get
            {
                return this._ADUserName;
            }
            set
            {
                this._ADUserName = value;
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

        [PrimaryKey(PrimaryKeyType.Native, "ITCCreditScoreKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ITCDecisionReason_DAO), ColumnKey = "ITCCreditScoreKey", Table = "ITCDecisionReason")]
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

        [BelongsTo("GeneralStatusKey", NotNull = true)]
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

        [BelongsTo("RiskMatrixRevisionKey", NotNull = true)]
        public virtual RiskMatrixRevision_DAO RiskMatrixRevision
        {
            get
            {
                return _riskMatrixRevision;
            }
            set
            {
                _riskMatrixRevision = value;
            }
        }

        [BelongsTo("RiskMatrixCellKey", NotNull = false)]
        public virtual RiskMatrixCell_DAO RiskMatrixCell
        {
            get
            {
                return _cell;
            }
            set
            {
                _cell = value;
            }
        }

        [Property("ITCKey", ColumnType = "Int32", NotNull = false)]
        public virtual int ITCKey
        {
            get
            {
                return this._iTCKey;
            }
            set
            {
                this._iTCKey = value;
            }
        }

        [BelongsTo("ScoreCardKey", NotNull = false)]
        public virtual ScoreCard_DAO ScoreCard
        {
            get
            {
                return this._scoreCard;
            }
            set
            {
                this._scoreCard = value;
            }
        }

        [BelongsTo("LegalEntityKey", NotNull = true)]
        public virtual LegalEntity_DAO LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }
    }
}