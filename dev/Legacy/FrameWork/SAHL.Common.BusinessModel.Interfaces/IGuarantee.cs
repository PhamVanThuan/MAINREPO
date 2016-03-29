using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Guarantee_DAO
    /// </summary>
    public partial interface IGuarantee : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Guarantee_DAO.LimitedAmount
        /// </summary>
        System.Double LimitedAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Guarantee_DAO.IssueDate
        /// </summary>
        System.DateTime IssueDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Guarantee_DAO.StatusNumber
        /// </summary>
        System.Byte StatusNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Guarantee_DAO.CancelledDate
        /// </summary>
        DateTime? CancelledDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Guarantee_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Guarantee_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }
    }
}