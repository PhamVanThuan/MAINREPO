using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ValuationClassification_DAO is the Property Classification captured during a SAHL Manual Valuation.
    /// </summary>
    public partial interface IValuationClassification : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Description of the ValuationClassification_DAO. e.g. Budget Standard.
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