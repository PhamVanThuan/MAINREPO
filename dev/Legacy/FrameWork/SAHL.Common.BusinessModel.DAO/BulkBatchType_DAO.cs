using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("BulkBatchType", Lazy = true, Schema = "dbo")]
    public partial class BulkBatchType_DAO : DB_2AM<BulkBatchType_DAO>
    {
        private string _description;

        private string _filePath;

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

        [Property("FilePath", ColumnType = "String")]
        public virtual string FilePath
        {
            get
            {
                return this._filePath;
            }
            set
            {
                this._filePath = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "BulkBatchTypeKey", ColumnType = "Int32")]
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