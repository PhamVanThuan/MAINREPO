using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("PropertyDataProviderDataService", Schema = "dbo")]
    public partial class PropertyDataProviderDataService_DAO : DB_2AM<PropertyDataProviderDataService_DAO>
    {
        private int _key;
        private DataProviderDataService_DAO _dataProviderDataService;

        [PrimaryKey(PrimaryKeyType.Assigned, "PropertyDataProviderDataServiceKey", ColumnType = "Int32")]
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

        [BelongsTo("DataProviderDataServiceKey", NotNull = true)]
        [ValidateNonEmpty("Data Provider Data Service is a mandatory field")]
        public virtual DataProviderDataService_DAO DataProviderDataService
        {
            get
            {
                return this._dataProviderDataService;
            }
            set
            {
                this._dataProviderDataService = value;
            }
        }
    }
}