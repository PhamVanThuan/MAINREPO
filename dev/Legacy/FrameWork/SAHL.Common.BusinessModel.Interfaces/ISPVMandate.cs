using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO
    /// </summary>
    public partial interface ISPVMandate : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.MaxLTV
        /// </summary>
        Double? MaxLTV
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.MaxPTI
        /// </summary>
        Double? MaxPTI
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.MaxLoanAmount
        /// </summary>
        Double? MaxLoanAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.ExceedBondPercent
        /// </summary>
        Double? ExceedBondPercent
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.ExceedLoanAgreementPercent
        /// </summary>
        Double? ExceedLoanAgreementPercent
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.ExceedBondAmount
        /// </summary>
        Double? ExceedBondAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.SPVMaxTerm
        /// </summary>
        Int32? SPVMaxTerm
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.SPV
        /// </summary>
        ISPV SPV
        {
            get;
            set;
        }
    }
}