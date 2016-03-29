using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ADUser_DAO
    /// </summary>
    public partial interface IADUser : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.LegalEntity
        /// </summary>
        ILegalEntityNaturalPerson LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.ADUserName
        /// </summary>
        System.String ADUserName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.GeneralStatusKey
        /// </summary>
        IGeneralStatus GeneralStatusKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.Password
        /// </summary>
        System.String Password
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.PasswordQuestion
        /// </summary>
        System.String PasswordQuestion
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.PasswordAnswer
        /// </summary>
        System.String PasswordAnswer
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.UserProfileSettings
        /// </summary>
        IEventList<IUserProfileSetting> UserProfileSettings
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.UserOrganisationStructure
        /// </summary>
        IEventList<IUserOrganisationStructure> UserOrganisationStructure
        {
            get;
        }
    }
}