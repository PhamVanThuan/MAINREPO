using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ITCDecisionReason_DAO
    /// </summary>
    public partial interface IITCDecisionReason : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCDecisionReason_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCDecisionReason_DAO.CreditScoreDecision
        /// </summary>
        ICreditScoreDecision CreditScoreDecision
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCDecisionReason_DAO.ITCCreditScore
        /// </summary>
        IITCCreditScore ITCCreditScore
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCDecisionReason_DAO.ApplicationCreditScore
        /// </summary>
        IApplicationCreditScore ApplicationCreditScore
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCDecisionReason_DAO.Reason
        /// </summary>
        IReason Reason
        {
            get;
            set;
        }
    }
}