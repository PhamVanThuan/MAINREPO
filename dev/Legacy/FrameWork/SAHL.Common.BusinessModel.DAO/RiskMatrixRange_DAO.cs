using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("RiskMatrixRange", Schema = "dbo")]
    public partial class RiskMatrixRange_DAO : DB_2AM<RiskMatrixRange_DAO>
    {
        private double? _min;

        private double? _max;

        private string _designation;

        private int _key;

        [Property("Min", ColumnType = "Double", NotNull = true)]
        public virtual double? Min
        {
            get
            {
                return this._min;
            }
            set
            {
                this._min = value;
            }
        }

        [Property("Max", ColumnType = "Double", NotNull = true)]
        public virtual double? Max
        {
            get
            {
                return this._max;
            }
            set
            {
                this._max = value;
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

        [PrimaryKey(PrimaryKeyType.Assigned, "RiskMatrixRangeKey", ColumnType = "Int32")]
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
    }
}