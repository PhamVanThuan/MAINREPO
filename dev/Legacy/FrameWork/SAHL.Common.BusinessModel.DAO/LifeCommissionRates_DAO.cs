using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("LifeCommissionRates", Schema = "dbo")]
    public partial class LifeCommissionRates_DAO : DB_2AM<LifeCommissionRates_DAO>
    {
        private string _entity;

        private double _percentage;

        private int _Key;

        private System.DateTime _effectiveDate;

        [Property("Entity", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Entity is a mandatory field")]
        public virtual string Entity
        {
            get
            {
                return this._entity;
            }
            set
            {
                this._entity = value;
            }
        }

        [Property("Percentage", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Percentage is a mandatory field")]
        public virtual double Percentage
        {
            get
            {
                return this._percentage;
            }
            set
            {
                this._percentage = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "RatesKey", ColumnType = "Int32")]
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

        [Property("EffectiveDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Effective Date is a mandatory field")]
        public virtual System.DateTime EffectiveDate
        {
            get
            {
                return this._effectiveDate;
            }
            set
            {
                this._effectiveDate = value;
            }
        }
    }
}