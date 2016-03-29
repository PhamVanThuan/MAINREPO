using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ValuationImprovementType_DAO describes the different types of improvements which can be captured against a SAHL Manual
    /// Valuation.
    /// </summary>
    public partial interface IValuationImprovementType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Improvement Description.
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