using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// A linking object between margin and OriginationoSourceproduct, with a defined discount.
    /// </summary>
    /// <remarks>
    /// Please note that the columns in the DB are nullable, this is wrong and we're not going to reflect that nonsense in this model.
    /// </remarks>
    [ActiveRecord("MarginProduct", Schema = "dbo")]
    public partial class MarginProduct_DAO : DB_2AM<MarginProduct_DAO>
    {
        private float _discount;

        private int _key;

        private Margin_DAO _margin;

        private OriginationSourceProduct_DAO _originationSourceProduct;

        [Property("Discount", ColumnType = "Single")]
        public virtual float Discount
        {
            get
            {
                return this._discount;
            }
            set
            {
                this._discount = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "MarginProductKey", ColumnType = "Int32")]
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

        [BelongsTo("MarginKey", NotNull = false)]
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

        [BelongsTo("OriginationSourceProductKey", NotNull = true)]
        [ValidateNonEmpty("Origination Source Product is a mandatory field")]
        public virtual OriginationSourceProduct_DAO OriginationSourceProduct
        {
            get
            {
                return this._originationSourceProduct;
            }
            set
            {
                this._originationSourceProduct = value;
            }
        }
    }
}