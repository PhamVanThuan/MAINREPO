using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO
    /// </summary>
    public partial interface IOrganisationStructure : IGenericOrganisationNode
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ChildOrganisationStructures
        /// </summary>
        IEventList<IOrganisationStructure> ChildOrganisationStructures
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ADUsers
        /// </summary>
        IEventList<IADUser> ADUsers
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.Parent
        /// </summary>
        IOrganisationStructure Parent
        {
            get;
            set;
        }
    }
}