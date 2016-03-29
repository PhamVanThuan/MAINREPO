using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IAllocationOperatorGroup : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IEventList<IAllocationOperator> AllocationOperators
        {
            get;
        }
    }
}