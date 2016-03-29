using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO
    /// </summary>
    public partial interface IInternetLeadUsers : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO.Flag
        /// </summary>
        System.Boolean Flag
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO.CaseCount
        /// </summary>
        System.Int32 CaseCount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO.LastCaseKey
        /// </summary>
        System.Int32 LastCaseKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO.ADUser
        /// </summary>
        IADUser ADUser
        {
            get;
            set;
        }
    }
}