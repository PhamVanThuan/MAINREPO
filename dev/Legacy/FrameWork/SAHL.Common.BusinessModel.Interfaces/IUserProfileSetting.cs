using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.UserProfileSetting_DAO
    /// </summary>
    public partial interface IUserProfileSetting : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserProfileSetting_DAO.SettingName
        /// </summary>
        System.String SettingName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserProfileSetting_DAO.SettingValue
        /// </summary>
        System.String SettingValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserProfileSetting_DAO.SettingType
        /// </summary>
        System.String SettingType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserProfileSetting_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserProfileSetting_DAO.ADUser
        /// </summary>
        IADUser ADUser
        {
            get;
            set;
        }
    }
}