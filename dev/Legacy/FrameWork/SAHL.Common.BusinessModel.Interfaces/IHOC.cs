using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.HOC_DAO
    /// </summary>
    public partial interface IHOC : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Used for Activerecord exclusively, please use Key.
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCPolicyNumber
        /// </summary>
        System.String HOCPolicyNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCProrataPremium
        /// </summary>
        System.Double HOCProrataPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCMonthlyPremium
        /// </summary>
        Double? HOCMonthlyPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCThatchAmount
        /// </summary>
        Double? HOCThatchAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCConventionalAmount
        /// </summary>
        Double? HOCConventionalAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCShingleAmount
        /// </summary>
        Double? HOCShingleAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCStatusID
        /// </summary>
        Int32? HOCStatusID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCSBICFlag
        /// </summary>
        Boolean? HOCSBICFlag
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.CapitalizedMonthlyBalance
        /// </summary>
        Double? CapitalizedMonthlyBalance
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.CommencementDate
        /// </summary>
        DateTime? CommencementDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.ChangeDate
        /// </summary>
        DateTime? ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.Ceded
        /// </summary>
        System.Boolean Ceded
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.SAHLPolicyNumber
        /// </summary>
        System.String SAHLPolicyNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.CancellationDate
        /// </summary>
        DateTime? CancellationDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCHistory
        /// </summary>
        IHOCHistory HOCHistory
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCHistories
        /// </summary>
        IEventList<IHOCHistory> HOCHistories
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCInsurer
        /// </summary>
        IHOCInsurer HOCInsurer
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCConstruction
        /// </summary>
        IHOCConstruction HOCConstruction
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCRoof
        /// </summary>
        IHOCRoof HOCRoof
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCStatus
        /// </summary>
        IHOCStatus HOCStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCSubsidence
        /// </summary>
        IHOCSubsidence HOCSubsidence
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCAdministrationFee
        /// </summary>
        System.Double HOCAdministrationFee
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCBasePremium
        /// </summary>
        System.Double HOCBasePremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.SASRIAAmount
        /// </summary>
        System.Double SASRIAAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.FinancialService
        /// </summary>
        IFinancialService FinancialService
        {
            get;
            set;
        }
    }
}