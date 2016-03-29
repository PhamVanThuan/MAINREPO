using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RegMail_DAO
    /// </summary>
    public partial interface IRegMail : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.LoanNumber
        /// </summary>
        System.Int32 LoanNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.PurposeNumber
        /// </summary>
        System.Decimal PurposeNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.DetailTypeNumber
        /// </summary>
        System.Decimal DetailTypeNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.AttorneyNumber
        /// </summary>
        System.Decimal AttorneyNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailLoanStatus
        /// </summary>
        Int16? RegMailLoanStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailDateTime
        /// </summary>
        DateTime? RegMailDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailBondAmount
        /// </summary>
        Double? RegMailBondAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailBondDate
        /// </summary>
        DateTime? RegMailBondDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailGuaranteeAmount
        /// </summary>
        Double? RegMailGuaranteeAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailCashRequired
        /// </summary>
        Double? RegMailCashRequired
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailCashDeposit
        /// </summary>
        Double? RegMailCashDeposit
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailConveyancingFee
        /// </summary>
        Double? RegMailConveyancingFee
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailVAT
        /// </summary>
        Double? RegMailVAT
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailTransferDuty
        /// </summary>
        Double? RegMailTransferDuty
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailDeedsFee
        /// </summary>
        Double? RegMailDeedsFee
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailInstructions1
        /// </summary>
        System.String RegMailInstructions1
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailInstructions2
        /// </summary>
        System.String RegMailInstructions2
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailInstructions3
        /// </summary>
        System.String RegMailInstructions3
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailStampDuty
        /// </summary>
        Double? RegMailStampDuty
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailCancelFee
        /// </summary>
        Double? RegMailCancelFee
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailCATSFlag
        /// </summary>
        Int16? RegMailCATSFlag
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailLoanAgreementAmount
        /// </summary>
        Double? RegMailLoanAgreementAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailValuationFee
        /// </summary>
        Double? RegMailValuationFee
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.RegMailAdminFee
        /// </summary>
        Double? RegMailAdminFee
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.QuickCashInterest
        /// </summary>
        Double? QuickCashInterest
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.QCAdminFee
        /// </summary>
        Double? QCAdminFee
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegMail_DAO.Key
        /// </summary>
        System.Decimal Key
        {
            get;
            set;
        }
    }
}