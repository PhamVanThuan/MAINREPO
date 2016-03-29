using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ValuationStatus_DAO describes the status of a Valuation. A valuation can currently be pending or complete.
    /// </summary>
    public partial interface IValuationStatus : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Valuation status description.
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}