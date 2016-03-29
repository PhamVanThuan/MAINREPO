using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("DataProviderDataService", Schema = "dbo")]
    public partial class DataProviderDataService_DAO : DB_2AM<DataProviderDataService_DAO>
    {
        private DataProvider_DAO _dataProvider;
        private DataService_DAO _dataService;
        private int _key;

        [BelongsTo("DataProviderKey", NotNull = true)]
        [ValidateNonEmpty("Data Provider is a mandatory field")]
        public virtual DataProvider_DAO DataProvider
        {
            get
            {
                return this._dataProvider;
            }
            set
            {
                this._dataProvider = value;
            }
        }

        [BelongsTo("DataServiceKey", NotNull = true)]
        [ValidateNonEmpty("Data Service is a mandatory field")]
        public virtual DataService_DAO DataService
        {
            get
            {
                return this._dataService;
            }
            set
            {
                this._dataService = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "DataProviderDataServiceKey", ColumnType = "Int32")]
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