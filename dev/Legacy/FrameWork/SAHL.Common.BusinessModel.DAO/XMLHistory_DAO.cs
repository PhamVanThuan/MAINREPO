using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("XMLHistory", Schema = "dbo")]
    public partial class XMLHistory_DAO : DB_2AM<XMLHistory_DAO>
    {
        private int _Key;
        private GenericKeyType_DAO _genericKeyType;
        private int _genericKey;
        private string _xmlData;
        private DateTime _insertDate;

        [PrimaryKey(PrimaryKeyType.Native, "XMLHistoryKey", ColumnType = "Int32")]
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

        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
        [ValidateNonEmpty("Generic Key Type is a mandatory field")]
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

        [Property("GenericKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Generic Key is a mandatory field")]
        public virtual int GenericKey
        {
            get
            {
                return this._genericKey;
            }
            set
            {
                this._genericKey = value;
            }
        }

        [Property("XMLData", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("XML Data is a mandatory field")]
        public virtual string XmlData
        {
            get
            {
                return this._xmlData;
            }
            set
            {
                this._xmlData = value;
            }
        }

        [Property("InsertDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Insert Date is a mandatory field")]
        public virtual DateTime InsertDate
        {
            get
            {
                return this._insertDate;
            }
            set
            {
                this._insertDate = value;
            }
        }
    }
}