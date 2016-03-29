using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AllocationMandateSet_DAO
    /// </summary>
    public partial interface IAllocationMandateSet : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSet_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSet_DAO.AllocationMandateOperators
        /// </summary>
        IEventList<IAllocationMandateOperator> AllocationMandateOperators
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSet_DAO.UserOrganisationStructures
        /// </summary>
        IEventList<IUserOrganisationStructure> UserOrganisationStructures
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSet_DAO.AllocationMandateSetGroup
        /// </summary>
        IAllocationMandateSetGroup AllocationMandateSetGroup
        {
            get;
            set;
        }
    }
}