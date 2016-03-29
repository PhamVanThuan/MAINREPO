using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("RiskMatrixCell", Schema = "dbo")]
    public partial class RiskMatrixCell_DAO : DB_2AM<RiskMatrixCell_DAO>
    {
        private int _key;

        private IList<RiskMatrixDimension_DAO> _riskMatrixDimensions;

        private IList<RiskMatrixRange_DAO> _riskMatrixRanges;

        private CreditScoreDecision_DAO _creditScoreDecision;

        private RiskMatrixRevision_DAO _riskMatrixRevision;

        private RuleItem_DAO _ruleItem;

        private string _designation;

        private GeneralStatus_DAO _GeneralStatus;

        [PrimaryKey(PrimaryKeyType.Assigned, "RiskMatrixCellKey", ColumnType = "Int32")]
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

        [HasAndBelongsToMany(typeof(RiskMatrixDimension_DAO), ColumnKey = "RiskMatrixCellKey", Table = "RiskMatrixCellDimension", ColumnRef = "RiskMatrixDimensionKey")]
        public virtual IList<RiskMatrixDimension_DAO> RiskMatrixDimensions
        {
            get
            {
                return this._riskMatrixDimensions;
            }
            set
            {
                this._riskMatrixDimensions = value;
            }
        }

        [HasAndBelongsToMany(typeof(RiskMatrixRange_DAO), ColumnKey = "RiskMatrixCellKey", Table = "RiskMatrixCellDimension", ColumnRef = "RiskMatrixRangeKey")]
        public virtual IList<RiskMatrixRange_DAO> RiskMatrixRanges
        {
            get
            {
                return this._riskMatrixRanges;
            }
            set
            {
                this._riskMatrixRanges = value;
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

        [BelongsTo("RiskMatrixRevisionKey", NotNull = true)]
        public virtual RiskMatrixRevision_DAO RiskMatrixRevision
        {
            get
            {
                return this._riskMatrixRevision;
            }
            set
            {
                this._riskMatrixRevision = value;
            }
        }

        [BelongsTo("RuleItemKey")]
        public virtual RuleItem_DAO RuleItem
        {
            get
            {
                return this._ruleItem;
            }
            set
            {
                this._ruleItem = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._GeneralStatus;
            }
            set
            {
                this._GeneralStatus = value;
            }
        }

        [Property("Designation", ColumnType = "String")]
        public virtual string Designation
        {
            get
            {
                return this._designation;
            }
            set
            {
                this._designation = value;
            }
        }
    }
}