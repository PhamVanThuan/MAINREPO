using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AllocationMandateOperator_DAO
    /// </summary>
    public partial interface IAllocationMandateOperator : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateOperator_DAO.Order
        /// </summary>
        System.Int32 Order
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateOperator_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateOperator_DAO.AllocationOperator
        /// </summary>
        IOperator AllocationOperator
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateOperator_DAO.AllocationMandateSet
        /// </summary>
        IAllocationMandateSet AllocationMandateSet
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateOperator_DAO.AllocationMandate
        /// </summary>
        IAllocationMandate AllocationMandate
        {
            get;
            set;
        }
    }
}