using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("XSLTransformation", Schema = "dbo")]
    public partial class XSLTransformation_DAO : DB_2AM<XSLTransformation_DAO>
    {
        private int _key;

        private string _styleSheet;

        private int _version;

        private GenericKeyType_DAO _genericKeyType;

        [PrimaryKey(PrimaryKeyType.Native, "XSLTransformationKey", ColumnType = "Int32")]
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

        [Property("StyleSheet", ColumnType = "String", NotNull = true)]
        public virtual string StyleSheet
        {
            get
            {
                return this._styleSheet;
            }
            set
            {
                this._styleSheet = value;
            }
        }

        [Property("Version", ColumnType = "Int32", NotNull = true)]
        public virtual int Version
        {
            get
            {
                return this._version;
            }
            set
            {
                this._version = value;
            }
        }

        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
        public virtual GenericKeyType_DAO GenericKeyType
        {
            get
            {
                return this._genericKeyType;
            }
            set
            {
                this._genericKeyType = value;
            }
        }
    }
}