using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Fee_DAO
    /// </summary>
    public partial interface IFee : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fee_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fee_DAO.Amount
        /// </summary>
        System.Double Amount
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fee_DAO.LastPostedDate
        /// </summary>
        DateTime? LastPostedDate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fee_DAO.FinancialService
        /// </summary>
        IFinancialService FinancialService
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fee_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fee_DAO.FeeType
        /// </summary>
        IFeeType FeeType
        {
            get;
        }
    }
}