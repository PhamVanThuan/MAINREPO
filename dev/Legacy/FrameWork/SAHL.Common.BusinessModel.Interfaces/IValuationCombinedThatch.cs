using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ValuationCombinedThatch_DAO stores the Combined Total Thatch Value of the SAHL Manual Valuation where the roof type
    /// is Thatch.
    /// </summary>
    public partial interface IValuationCombinedThatch : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Combined Total Thatch Value
        /// </summary>
        System.Double Value
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

        /// <summary>
        /// The Combined Total Thatch Value is related to a single Valuation.
        /// </summary>
        IValuation Valuation
        {
            get;
            set;
        }
    }
}