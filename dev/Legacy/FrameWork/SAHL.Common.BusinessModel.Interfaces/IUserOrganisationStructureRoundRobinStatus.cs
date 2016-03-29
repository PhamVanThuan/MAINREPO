using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureRoundRobinStatus_DAO
    /// </summary>
    public partial interface IUserOrganisationStructureRoundRobinStatus : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureRoundRobinStatus_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureRoundRobinStatus_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureRoundRobinStatus_DAO.CapitecGeneralStatus
        /// </summary>
        IGeneralStatus CapitecGeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureRoundRobinStatus_DAO.UserOrganisationStructure
        /// </summary>
        IUserOrganisationStructure UserOrganisationStructure
        {
            get;
            set;
        }
    }
}