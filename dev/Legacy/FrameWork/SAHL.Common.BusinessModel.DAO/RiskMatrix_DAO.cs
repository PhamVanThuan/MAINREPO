using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("RiskMatrix", Schema = "dbo")]
    public partial class RiskMatrix_DAO : DB_2AM<RiskMatrix_DAO>
    {
        private int _key;
        private string _description;
        private IList<RiskMatrixRevision_DAO> _riskMatrixRevisions;

        [PrimaryKey(PrimaryKeyType.Assigned, "RiskMatrixKey", ColumnType = "Int32")]
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

        [Property("Description", ColumnType = "String", NotNull = true)]
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

        [HasMany(typeof(RiskMatrixRevision_DAO), ColumnKey = "RiskMatrixKey", Table = "RiskMatrixRevision", OrderBy = "RevisionDate desc")]
        public virtual IList<RiskMatrixRevision_DAO> RiskMatrixRevisions
        {
            get
            {
                return this._riskMatrixRevisions;
            }
            set
            {
                this._riskMatrixRevisions = value;
            }
        }
    }
}