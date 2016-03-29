using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// This class links a OriginationSourceProduct (OSP) for a given Category to a margin.
    /// </summary>
    [ActiveRecord("ProductCategory", Schema = "dbo")]
    public partial class ProductCategory_DAO : DB_2AM<ProductCategory_DAO>
    {
        private int _key;

        private Category_DAO _category;

        private Margin_DAO _margin;

        private OriginationSourceProduct_DAO _originationSourceProduct;

        /// <summary>
        /// This is the primary key, used to identify an instance of ProductCategory.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "ProductCategoryKey", ColumnType = "Int32")]
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

        /// <summary>
        /// The category the OSP is linked to.
        /// </summary>
        [BelongsTo("CategoryKey", NotNull = false)]
        public virtual Category_DAO Category
        {
            get
            {
                return this._category;
            }
            set
            {
                this._category = value;
            }
        }

        /// <summary>
        /// The margin associated with this OSP for given Category.
        /// </summary>
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

        /// <summary>
        /// The OSP.
        /// </summary>
        [BelongsTo("OriginationSourceProductKey", NotNull = false)]
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