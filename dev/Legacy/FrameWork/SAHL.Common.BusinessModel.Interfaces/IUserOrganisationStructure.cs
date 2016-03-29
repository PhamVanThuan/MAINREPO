using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO
    /// </summary>
    public partial interface IUserOrganisationStructure : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.ADUser
        /// </summary>
        IADUser ADUser
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.OrganisationStructure
        /// </summary>
        IOrganisationStructure OrganisationStructure
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.GenericKey
        /// </summary>
        System.Int32 GenericKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.GenericKeyType
        /// </summary>
        IGenericKeyType GenericKeyType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.UserOrganisationStructureRoundRobinStatus
        /// </summary>
        IEventList<IUserOrganisationStructureRoundRobinStatus> UserOrganisationStructureRoundRobinStatus
        {
            get;
        }
    }
}