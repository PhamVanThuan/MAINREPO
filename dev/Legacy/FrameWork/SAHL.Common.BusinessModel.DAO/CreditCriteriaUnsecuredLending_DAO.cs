using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("CreditCriteriaUnsecuredLending", Schema = "dbo")]
    public partial class CreditCriteriaUnsecuredLending_DAO : DB_2AM<CreditCriteriaUnsecuredLending_DAO>
    {
        private double _minLoanAmount;

        private double _maxLoanAmount;

        private int _term;

        private int _key;

        private CreditMatrixUnsecuredLending_DAO _creditMatrixUnsecuredLending;

        private Margin_DAO _margin;

        [PrimaryKey(PrimaryKeyType.Native, "CreditCriteriaUnsecuredLendingKey", ColumnType = "Int32")]
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

        [Property("MinLoanAmount", ColumnType = "Double", NotNull = true)]
        public virtual double MinLoanAmount
        {
            get
            {
                return this._minLoanAmount;
            }
            set
            {
                this._minLoanAmount = value;
            }
        }

        [Property("MaxLoanAmount", ColumnType = "Double", NotNull = true)]
        public virtual double MaxLoanAmount
        {
            get
            {
                return this._maxLoanAmount;
            }
            set
            {
                this._maxLoanAmount = value;
            }
        }

        [Property("Term", ColumnType = "Int32", NotNull = true)]
        public virtual int Term
        {
            get
            {
                return this._term;
            }
            set
            {
                this._term = value;
            }
        }

        [BelongsTo("CreditMatrixUnsecuredLendingKey", NotNull = true)]
        public virtual CreditMatrixUnsecuredLending_DAO CreditMatrixUnsecuredLending
        {
            get
            {
                return this._creditMatrixUnsecuredLending;
            }
            set
            {
                this._creditMatrixUnsecuredLending = value;
            }
        }

        [BelongsTo("MarginKey", NotNull = true)]
        public virtual Margin_DAO Margin
        {
            get
            {
                return this._margin;
            }
            set
            {
                this._margin = value;
            }
        }
    }
}