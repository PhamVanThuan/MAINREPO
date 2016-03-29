using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Legal Entity Login DAO
    /// </summary>
    public partial interface ILegalEntityLogin : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityLogin_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityLogin_DAO.Username
        /// </summary>
        System.String Username
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityLogin_DAO.Password
        /// </summary>
        System.String Password
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityLogin_DAO.LastLoginDate
        /// </summary>
        DateTime? LastLoginDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityLogin_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityLogin_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }
    }
}