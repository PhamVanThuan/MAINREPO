using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("HOCRates", Schema = "dbo")]
    public partial class HOCRates_DAO : DB_2AM<HOCRates_DAO>
    {
        private int _key;
        private HOCInsurer_DAO _hOCInsurer;
        private HOCSubsidence_DAO _hOCSubsidence;
        private double _thatchPremium;
        private double _conventionalPremium;
        private double _shinglePremium;

        [PrimaryKey(PrimaryKeyType.Native, "HOCRatesKey", ColumnType = "Int32")]
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

        [BelongsTo("HOCInsurerKey", NotNull = true)]
        [ValidateNonEmpty("HOC Insurer is a mandatory field")]
        public virtual HOCInsurer_DAO HOCInsurer
        {
            get { return _hOCInsurer; }
            set { _hOCInsurer = value; }
        }

        [BelongsTo("HOCSubsidenceKey", NotNull = true)]
        [ValidateNonEmpty("HOC Subsidence is a mandatory field")]
        public virtual HOCSubsidence_DAO HOCSubsidence
        {
            get
            {
                return _hOCSubsidence;
            }
            set
            {
                _hOCSubsidence = value;
            }
        }

        [Property("ThatchPremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Thatch Premium is a mandatory field")]
        public virtual double ThatchPremium
        {
            get { return _thatchPremium; }
            set { _thatchPremium = value; }
        }

        [Property("ConventionalPremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Conventional Premium is a mandatory field")]
        public virtual double ConventionalPremium
        {
            get { return _conventionalPremium; }
            set { _conventionalPremium = value; }
        }

        [Property("ShinglePremium", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Shingle Premium is a mandatory field")]
        public virtual double ShinglePremium
        {
            get { return _shinglePremium; }
            set { _shinglePremium = value; }
        }
    }
}