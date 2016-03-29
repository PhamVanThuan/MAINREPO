using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ProductConditionType", Schema = "dbo")]
    public partial class ProductConditionType_DAO : DB_2AM<ProductConditionType_DAO>
    {
        private string _description;

        private int _Key;

        private IList<ProductCondition_DAO> _productConditions;

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

        [PrimaryKey(PrimaryKeyType.Native, "ProductConditionTypeKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ProductCondition_DAO), ColumnKey = "ProductConditionTypeKey", Table = "ProductCondition", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ProductCondition_DAO> ProductConditions
        {
            get
            {
                return this._productConditions;
            }
            set
            {
                this._productConditions = value;
            }
        }
    }
}