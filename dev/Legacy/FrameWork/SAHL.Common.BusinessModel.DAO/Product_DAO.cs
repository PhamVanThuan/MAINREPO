using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("Product", Schema = "dbo", Lazy = true)]
    public partial class Product_DAO : DB_2AM<Product_DAO>
    {
        private string _description;

        private string _originate;

        private int _key;

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

        [Property("OriginateYN", ColumnType = "String", Access = PropertyAccess.FieldCamelcaseUnderscore)]
        public virtual bool Originate
        {
            get
            {
                return (this._originate == "Y");
            }
            set
            {
                this._originate = (value ? "Y" : "N");
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "ProductKey", ColumnType = "Int32")]
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