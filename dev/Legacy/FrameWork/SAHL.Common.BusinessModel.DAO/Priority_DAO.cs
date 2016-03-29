using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Priority", Schema = "dbo", Lazy = true)]
    public partial class Priority_DAO : DB_2AM<Priority_DAO>
    {
        private string _description;

        private int _key;

        // commented, this is a lookup.
        //private IList<LifePolicy_DAO> _lifePolicies;

        private OriginationSourceProduct_DAO _originationSourceProduct;

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
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

        [PrimaryKey(PrimaryKeyType.Native, "PriorityKey", ColumnType = "Int32")]
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

        // commented, this is a lookup.
        //[HasMany(typeof(LifePolicy_DAO), ColumnKey = "PriorityKey", Table = "LifePolicy", Lazy = true)]
        //public virtual IList<LifePolicy_DAO> LifePolicies
        //{
        //    get
        //    {
        //        return this._lifePolicies;
        //    }
        //    set
        //    {
        //        this._lifePolicies = value;
        //    }
        //}

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