using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO
    /// </summary>
    public partial interface IApplicationITCCreditScore : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO.ApplicationCreditScore
        /// </summary>
        IApplicationCreditScore ApplicationCreditScore
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO.ITCCreditScore
        /// </summary>
        IITCCreditScore ITCCreditScore
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO.CreditScoreDecision
        /// </summary>
        ICreditScoreDecision CreditScoreDecision
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO.ScoreDate
        /// </summary>
        System.DateTime ScoreDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO.PrimaryApplicant
        /// </summary>
        System.Boolean PrimaryApplicant
        {
            get;
            set;
        }
    }
}