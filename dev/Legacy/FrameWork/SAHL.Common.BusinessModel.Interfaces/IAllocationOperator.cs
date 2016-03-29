using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IAllocationOperator : IEntityValidation
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
        IAllocationOperatorGroup AllocationOperatorGroup
        {
            get;
            set;
        }
    }
}