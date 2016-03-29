using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OriginationSourceProduct", Schema = "dbo", Lazy = true)]
    public partial class OriginationSourceProduct_DAO : DB_2AM<OriginationSourceProduct_DAO>
    {
        private OriginationSource_DAO _originationSource;

        private Product_DAO _product;

        private int _key;

        private IList<CreditMatrix_DAO> _creditMatrices;

        private IList<Priority_DAO> _priorities;

        [BelongsTo("OriginationSourceKey", NotNull = true)]
        [ValidateNonEmpty("Origination Source is a mandatory field")]
        public virtual OriginationSource_DAO OriginationSource
        {
            get
            {
                return this._originationSource;
            }
            set
            {
                this._originationSource = value;
            }
        }

        [BelongsTo("ProductKey", NotNull = true)]
        [ValidateNonEmpty("Product is a mandatory field")]
        public virtual Product_DAO Product
        {
            get
            {
                return this._product;
            }
            set
            {
                this._product = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OriginationSourceProductKey", ColumnType = "Int32")]
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

        [HasAndBelongsToMany(typeof(CreditMatrix_DAO), ColumnRef = "CreditMatrixKey", ColumnKey = "OriginationSourceProductKey", Lazy = true, Schema = "dbo", Table = "OriginationSourceProductCreditMatrix")]
        public virtual IList<CreditMatrix_DAO> CreditMatrices
        {
            get
            {
                return this._creditMatrices;
            }
            set
            {
                this._creditMatrices = value;
            }
        }

        [HasMany(typeof(Priority_DAO), ColumnKey = "OriginationSourceProductKey", Table = "Priority", Lazy = true)]
        public virtual IList<Priority_DAO> Priorities
        {
            get
            {
                return this._priorities;
            }
            set
            {
                this._priorities = value;
            }
        }
    }
}