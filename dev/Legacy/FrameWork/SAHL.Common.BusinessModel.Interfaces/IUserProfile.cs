using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.UserProfile_DAO
    /// </summary>
    public partial interface IUserProfile : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserProfile_DAO.ADUserName
        /// </summary>
        System.String ADUserName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserProfile_DAO.Value
        /// </summary>
        System.String Value
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserProfile_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserProfile_DAO.ProfileType
        /// </summary>
        IProfileType ProfileType
        {
            get;
            set;
        }
    }
}