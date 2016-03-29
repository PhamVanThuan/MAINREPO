using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("LifeCommissionTargets", Schema = "dbo")]
    public partial class LifeCommissionTargets_DAO : DB_2AM<LifeCommissionTargets_DAO>
    {
        private string _consultant;

        private int? _effectiveYear;

        private int? _effectiveMonth;

        private int? _targetPolicies;

        private int? _minPoliciesToQualify;

        private int _Key;

        [Property("Consultant", ColumnType = "String")]
        public virtual string Consultant
        {
            get
            {
                return this._consultant;
            }
            set
            {
                this._consultant = value;
            }
        }

        [Property("EffectiveYear", ColumnType = "Int32")]
        public virtual int? EffectiveYear
        {
            get
            {
                return this._effectiveYear;
            }
            set
            {
                this._effectiveYear = value;
            }
        }

        [Property("EffectiveMonth", ColumnType = "Int32")]
        public virtual int? EffectiveMonth
        {
            get
            {
                return this._effectiveMonth;
            }
            set
            {
                this._effectiveMonth = value;
            }
        }

        [Property("TargetPolicies", ColumnType = "Int32")]
        public virtual int? TargetPolicies
        {
            get
            {
                return this._targetPolicies;
            }
            set
            {
                this._targetPolicies = value;
            }
        }

        [Property("MinPoliciesToQualify", ColumnType = "Int32")]
        public virtual int? MinPoliciesToQualify
        {
            get
            {
                return this._minPoliciesToQualify;
            }
            set
            {
                this._minPoliciesToQualify = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "TargetKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }
    }
}