using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO
    /// </summary>
    public partial interface IUserOrganisationStructureHistory : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.UserOrganisationStructureKey
        /// </summary>
        System.Int32 UserOrganisationStructureKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.OrganisationStructureKey
        /// </summary>
        IOrganisationStructure OrganisationStructureKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.GenericKey
        /// </summary>
        System.Int32 GenericKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.GenericKeyType
        /// </summary>
        IGenericKeyType GenericKeyType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.Action
        /// </summary>
        System.Char Action
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.ADUser
        /// </summary>
        IADUser ADUser
        {
            get;
            set;
        }
    }
}