using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO
    /// </summary>
    public partial interface IApplicationCreditScore : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO.Application
        /// </summary>
        IApplication Application
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO.ApplicationAggregateDecision
        /// </summary>
        IApplicationAggregateDecision ApplicationAggregateDecision
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO.ScoreDate
        /// </summary>
        System.DateTime ScoreDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO.CallingContext
        /// </summary>
        ICallingContext CallingContext
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO.ITCDecisionReasons
        /// </summary>
        IEventList<IITCDecisionReason> ITCDecisionReasons
        {
            get;
        }

        /// <summary>
        /// An ApplicationCreditScore can have a many ApplicationITCCreditScores associated with it. This relationship is defined in the ApplicationITCCreditScore table where the
        /// Offer.OfferKey = OfferITCCreditScore.OfferKey.
        /// </summary>
        IEventList<IApplicationITCCreditScore> ApplicationITCCreditScores
        {
            get;
        }
    }
}