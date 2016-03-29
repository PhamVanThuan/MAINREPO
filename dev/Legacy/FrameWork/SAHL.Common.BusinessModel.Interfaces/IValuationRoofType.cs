using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ValuationRoofType_DAO describes the different roof types available for SAHL Manual Valuations.
    /// </summary>
    public partial interface IValuationRoofType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// ValuationRoofType Description
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