using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.SPVFee_DAO
    /// </summary>
    public partial interface ISPVFee : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.SPV
        /// </summary>
        ISPV SPV
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.SPVFeeType
        /// </summary>
        ISPVFeeType SPVFeeType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.Value
        /// </summary>
        System.Double Value
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.MaxFeeAmount
        /// </summary>
        Double? MaxFeeAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.MinFeeAmount
        /// </summary>
        Double? MinFeeAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.RoundingYield
        /// </summary>
        Double? RoundingYield
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.AdditionalYield
        /// </summary>
        Double? AdditionalYield
        {
            get;
            set;
        }
    }
}