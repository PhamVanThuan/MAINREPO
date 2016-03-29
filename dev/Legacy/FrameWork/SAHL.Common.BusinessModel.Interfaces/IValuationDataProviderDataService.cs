using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ValuationDataProviderDataService_DAO describes the available DataProviderDataServices that allow enumerated discrimination of Valuations.
    /// </summary>
    public partial interface IValuationDataProviderDataService : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the DataProviderDataService table.
        /// </summary>
        IDataProviderDataService DataProviderDataService
        {
            get;
            set;
        }
    }
}