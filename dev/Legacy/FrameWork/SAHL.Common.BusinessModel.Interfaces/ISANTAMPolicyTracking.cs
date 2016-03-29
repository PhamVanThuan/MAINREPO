using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO
    /// </summary>
    public partial interface ISANTAMPolicyTracking : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.SANTAMPolicyTrackingKey
        /// </summary>
        System.Int32 SANTAMPolicyTrackingKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.PolicyNumber
        /// </summary>
        System.String PolicyNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.QuoteNumber
        /// </summary>
        System.String QuoteNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.CampaignTargetContactKey
        /// </summary>
        System.Int32 CampaignTargetContactKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.LegalEntityKey
        /// </summary>
        System.Int32 LegalEntityKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.AccountKey
        /// </summary>
        System.Int32 AccountKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.ActiveDate
        /// </summary>
        System.DateTime ActiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.Canceldate
        /// </summary>
        System.DateTime Canceldate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.MonthlyPremium
        /// </summary>
        System.Double MonthlyPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.CollectionDay
        /// </summary>
        System.Int32 CollectionDay
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.SANTAMPolicyStatus
        /// </summary>
        ISANTAMPolicyStatus SANTAMPolicyStatus
        {
            get;
            set;
        }
    }
}