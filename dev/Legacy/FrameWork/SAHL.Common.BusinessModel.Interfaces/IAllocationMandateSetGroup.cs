using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AllocationMandateSetGroup_DAO
    /// </summary>
    public partial interface IAllocationMandateSetGroup : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSetGroup_DAO.AllocationGroupName
        /// </summary>
        System.String AllocationGroupName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSetGroup_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSetGroup_DAO.AllocationMandateSets
        /// </summary>
        IEventList<IAllocationMandateSet> AllocationMandateSets
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSetGroup_DAO.OrganisationStructure
        /// </summary>
        IOrganisationStructure OrganisationStructure
        {
            get;
            set;
        }
    }
}