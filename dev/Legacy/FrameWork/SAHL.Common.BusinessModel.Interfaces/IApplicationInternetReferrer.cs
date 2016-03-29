using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationInternetReferrer_DAO
    /// </summary>
    public partial interface IApplicationInternetReferrer : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInternetReferrer_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInternetReferrer_DAO.Application
        /// </summary>
        IApplication Application
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInternetReferrer_DAO.UserURL
        /// </summary>
        System.String UserURL
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInternetReferrer_DAO.ReferringServerURL
        /// </summary>
        System.String ReferringServerURL
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInternetReferrer_DAO.Parameters
        /// </summary>
        System.String Parameters
        {
            get;
            set;
        }
    }
}