using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("PropertyData", Schema = "dbo")]
    public partial class PropertyData_DAO : DB_2AM<PropertyData_DAO>
    {
        private int _key;
        private Property_DAO _property;
        private PropertyDataProviderDataService_DAO _propertyDataProviderDataService;
        private string _propertyID;
        private string _data;
        private DateTime _insertDate;

        [PrimaryKey(PrimaryKeyType.Native, "PropertyDataKey", ColumnType = "Int32")]
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

        [BelongsTo("PropertyKey", NotNull = true)]
        [ValidateNonEmpty("Property is a mandatory field")]
        public virtual Property_DAO Property
        {
            get
            {
                return this._property;
            }
            set
            {
                this._property = value;
            }
        }

        [BelongsTo("PropertyDataProviderDataServiceKey", NotNull = true)]
        [ValidateNonEmpty("Property Data Provider Data Service is a mandatory field")]
        public virtual PropertyDataProviderDataService_DAO PropertyDataProviderDataService
        {
            get
            {
                return this._propertyDataProviderDataService;
            }
            set
            {
                this._propertyDataProviderDataService = value;
            }
        }

        [Property("PropertyID", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Property ID is a mandatory field")]
        public virtual string PropertyID
        {
            get
            {
                return this._propertyID;
            }
            set
            {
                this._propertyID = value;
            }
        }

        [Property("Data", ColumnType = "String", NotNull = true, Length=int.MaxValue)]
        [ValidateNonEmpty("Data is a mandatory field")]
        public virtual string Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
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