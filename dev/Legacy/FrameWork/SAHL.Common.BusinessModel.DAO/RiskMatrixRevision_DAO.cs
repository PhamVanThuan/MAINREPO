using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("RiskMatrixRevision", Schema = "dbo")]
    public partial class RiskMatrixRevision_DAO : DB_2AM<RiskMatrixRevision_DAO>
    {
        private string _description;

        private System.DateTime _revisionDate;

        private int _key;

        private IList<RiskMatrixCell_DAO> _riskMatrixCells;

        private RiskMatrix_DAO _riskMatrix;

        [Property("Description", ColumnType = "String")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [Property("RevisionDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime RevisionDate
        {
            get
            {
                return this._revisionDate;
            }
            set
            {
                this._revisionDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "RiskMatrixRevisionKey", ColumnType = "Int32")]
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

        [HasMany(typeof(RiskMatrixCell_DAO), ColumnKey = "RiskMatrixRevisionKey", Table = "RiskMatrixCell")]
        public virtual IList<RiskMatrixCell_DAO> RiskMatrixCells
        {
            get
            {
                return this._riskMatrixCells;
            }
            set
            {
                this._riskMatrixCells = value;
            }
        }

        [BelongsTo("RiskMatrixKey", NotNull = true)]
        public virtual RiskMatrix_DAO RiskMatrix
        {
            get
            {
                return this._riskMatrix;
            }
            set
            {
                this._riskMatrix = value;
            }
        }
    }
}