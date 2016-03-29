using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO
    /// </summary>
    public partial interface IITCCreditScore : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.EmpiricaScore
        /// </summary>
        Double? EmpiricaScore
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.SBCScore
        /// </summary>
        Double? SBCScore
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.ADUserName
        /// </summary>
        System.String ADUserName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.ScoreDate
        /// </summary>
        System.DateTime ScoreDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.ITCDecisionReasons
        /// </summary>
        IEventList<IITCDecisionReason> ITCDecisionReasons
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.CreditScoreDecision
        /// </summary>
        ICreditScoreDecision CreditScoreDecision
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.RiskMatrixRevision
        /// </summary>
        IRiskMatrixRevision RiskMatrixRevision
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.RiskMatrixCell
        /// </summary>
        IRiskMatrixCell RiskMatrixCell
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.ITCKey
        /// </summary>
        System.Int32 ITCKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.ScoreCard
        /// </summary>
        IScoreCard ScoreCard
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }
    }
}