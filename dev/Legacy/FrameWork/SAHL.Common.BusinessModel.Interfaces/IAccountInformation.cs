using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountInformation_DAO
    /// </summary>
    public partial interface IAccountInformation : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountInformation_DAO.EntryDate
        /// </summary>
        DateTime? EntryDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountInformation_DAO.Amount
        /// </summary>
        Double? Amount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountInformation_DAO.Information
        /// </summary>
        System.String Information
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountInformation_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountInformation_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountInformation_DAO.AccountInformationType
        /// </summary>
        IAccountInformationType AccountInformationType
        {
            get;
            set;
        }
    }
}