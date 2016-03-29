using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ValuationDataProviderDataService_DAO describes the available DataProviderDataServices that allow enumerated discrimination of Valuations.
    /// </summary>
    /// <seealso cref="Valuation_DAO"/>
    /// <seealso cref="DataProviderDataService_DAO"/>
    /// <seealso cref="DataProvider_DAO"/>
    /// <seealso cref="DataService_DAO"/>
    [GenericTest(TestType.Find)]
    [ActiveRecord("ValuationDataProviderDataService", Schema = "dbo")]
    public partial class ValuationDataProviderDataService_DAO : DB_2AM<ValuationDataProviderDataService_DAO>
    {
        private int _key;
        private DataProviderDataService_DAO _dataProviderDataService;

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "ValuationDataProviderDataServiceKey", ColumnType = "Int32")]
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

        /// <summary>
        /// The foreign key reference to the DataProviderDataService table.
        /// </summary>
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