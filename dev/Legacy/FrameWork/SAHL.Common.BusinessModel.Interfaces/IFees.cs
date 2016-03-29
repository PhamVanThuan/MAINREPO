using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Fees_DAO
    /// </summary>
    public partial interface IFees : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeNaturalTransferDuty
        /// </summary>
        Double? FeeNaturalTransferDuty
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeNaturalConveyancing
        /// </summary>
        Double? FeeNaturalConveyancing
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeNaturalVAT
        /// </summary>
        Double? FeeNaturalVAT
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeLegalTransferDuty
        /// </summary>
        Double? FeeLegalTransferDuty
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeLegalConveyancing
        /// </summary>
        Double? FeeLegalConveyancing
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeLegalVAT
        /// </summary>
        Double? FeeLegalVAT
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeBondStamps
        /// </summary>
        Double? FeeBondStamps
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeBondConveyancing
        /// </summary>
        Double? FeeBondConveyancing
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeBondVAT
        /// </summary>
        Double? FeeBondVAT
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeAdmin
        /// </summary>
        Double? FeeAdmin
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeValuation
        /// </summary>
        Double? FeeValuation
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeCancelDuty
        /// </summary>
        Double? FeeCancelDuty
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeCancelConveyancing
        /// </summary>
        Double? FeeCancelConveyancing
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeCancelVAT
        /// </summary>
        Double? FeeCancelVAT
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeFlexiSwitch
        /// </summary>
        Double? FeeFlexiSwitch
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeRCSBondConveyancing
        /// </summary>
        Double? FeeRCSBondConveyancing
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeRCSBondVAT
        /// </summary>
        Double? FeeRCSBondVAT
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeDeedsOffice
        /// </summary>
        Double? FeeDeedsOffice
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeRCSBondPreparation
        /// </summary>
        Double? FeeRCSBondPreparation
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeBondConveyancing80Pct
        /// </summary>
        Double? FeeBondConveyancing80Pct
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeBondVAT80Pct
        /// </summary>
        Double? FeeBondVAT80Pct
        {
            get;
            set;
        }
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeBondConveyancing80Pct
        /// </summary>
        Double? FeeBondConveyancingNoFICA
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.FeeBondVAT80Pct
        /// </summary>
        Double? FeeBondNoFICAVAT
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Fees_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}