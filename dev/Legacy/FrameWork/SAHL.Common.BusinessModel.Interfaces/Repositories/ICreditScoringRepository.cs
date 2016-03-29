using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface ICreditScoringRepository
    {
        IApplicationCreditScore GenerateApplicationCreditScore(IApplication application, ICallingContext callingContext, string adusername);

        /// <summary>
        /// Calculate the total adjusted score for all a score card's SBC variables from a given ITC xml record
        /// </summary>
        /// <param name="itc">The ITC record containing the SBC variables</param>
        /// <param name="scoreCard">The ScoreCard to use to determine the score</param>
        /// <returns>total SBC score</returns>
        double CalculateSBCScore(IITC itc, IScoreCard scoreCard);

        /// <summary>
        /// Gets the emperica score from the ITC XML
        /// </summary>
        /// <param name="itc">The ITC record containing the emperica score</param>
        /// <returns></returns>
        double GetEmpiricaFromITC(IITC itc);

        /// <summary>
        /// Get a credit decision for a legal entity based on ITC data
        /// </summary>
        /// <param name="application"></param>
        /// <param name="legalEntity"></param>
        /// <param name="scoreCard"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        IITCCreditScore GenerateIndividualDecision(IApplication application, ILegalEntity legalEntity, IScoreCard scoreCard, IRiskMatrixRevision matrix);

        /// <summary>
        /// Get a credit decision for a legal entity based on ITC data
        /// </summary>
        /// <param name="application"></param>
        /// <param name="legalEntity"></param>
        /// <param name="scoreCard"></param>
        /// <param name="matrix"></param>
        /// <param name="username">ADUser that performed the workflow activity which triggered the credit scoring</param>
        /// <returns></returns>
        IITCCreditScore GenerateIndividualDecision(IApplication application, ILegalEntity legalEntity, IScoreCard scoreCard, IRiskMatrixRevision matrix, string username);

        /// <summary>
        /// Calculates a Credit Scoring Decision for an Offer using a single risk matrix
        /// If this is a joint application, both primary and secondary applicants will use the same risk matrix
        /// </summary>
        /// <param name="application">the Offer in question</param>
        /// <param name="matrix">the risk matrix to use</param>
        /// <param name="context">usually the workflow map and action from which this method was called</param>
        /// <returns>a CreditScoreDecision</returns>
        IApplicationCreditScore GenerateApplicationCreditScore(IApplication application, IRiskMatrixRevision matrix, ICallingContext context);

        /// <summary>
        /// Calculates a credit scoring decision for an offer using seperate risk matrices for Primary and Secondary applicants
        /// </summary>
        /// <param name="application"></param>
        /// <param name="primaryMatrix"></param>
        /// <param name="secondaryMatrix"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        IApplicationCreditScore GenerateApplicationCreditScore(IApplication application, IRiskMatrixRevision primaryMatrix, IRiskMatrixRevision secondaryMatrix, ICallingContext context);

        /// <summary>
        /// Calculates a Credit Scoring Decision for an Offer from the aggregate of the Primary and Secondary applicant's decisions
        /// </summary>
        /// <param name="application"></param>
        /// <param name="primaryMatrix"></param>
        /// <param name="secondaryMatrix"></param>
        /// <param name="context"></param>
        /// <param name="username">ADUser that performed the workflow activity which triggered the credit scoring</param>
        /// <returns></returns>
        IApplicationCreditScore GenerateApplicationCreditScore(IApplication application, IRiskMatrixRevision primaryMatrix, IRiskMatrixRevision secondaryMatrix, ICallingContext context, string username);

        /// <summary>
        /// Get all the credit scores for an application
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        IEventList<IApplicationCreditScore> GetApplicationCreditScoreByApplicationKey(int applicationKey);

        /// <summary>
        /// Get all the credit scores for an application - with sort option
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="latestFirst"></param>
        /// <returns></returns>
        IEventList<IApplicationCreditScore> GetApplicationCreditScoreByApplicationKeySorted(int applicationKey, bool latestFirst);

        /// <summary>
        /// Get all the credit scores for a legal entity
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        IEventList<IITCCreditScore> GetITCCreditScoreByLegalEntity(int legalEntityKey, int accountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IITCCreditScore GetITCCreditScoreByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IApplicationITCCreditScore GetApplicationITCCreditScoreByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IApplicationCreditScore GetApplicationCreditScoreByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ICreditScoreDecision GetCreditScoreDecisionByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IApplicationAggregateDecision GetApplicationAggregateDecisionByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IITCDecisionReason GetITCDecisionReasonByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IScoreCard GetScoreCardByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IScoreCardAttribute GetScoreCardAttributeByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IScoreCardAttributeRange GetScoreCardAttributeRangeByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IRiskMatrix GetRiskMatrixByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IRiskMatrixRevision GetRiskMatrixRevisionByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IRiskMatrixDimension GetRiskMatrixDimensionByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IRiskMatrixCell GetRiskMatrixCellByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IRiskMatrixRange GetRiskMatrixRangeByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ICallingContext GetCallingContextByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ICallingContextType GetCallingContextTypeByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IITCCreditScore CreateEmptyITCCreditScore();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IApplicationITCCreditScore CreateEmptyApplicationITCCreditScore();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IApplicationCreditScore CreateEmptyApplicationCreditScore();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ICallingContext CreateEmptyCallingContext();

        /// <summary>
        /// Get Credit Score Info for Rules
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        IDictionary<string, object> GetCreditScoreInfoForRules(IApplicationMortgageLoan app);

        /// <summary>
        ///
        /// </summary>
        /// <param name="itcCreditScore"></param>
        void SaveITCCreditScore(IITCCreditScore itcCreditScore);

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationITCCreditScore"></param>
        void SaveApplicationITCCreditScore(IApplicationITCCreditScore applicationITCCreditScore);

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationCreditScore"></param>
        void SaveApplicationCreditScore(IApplicationCreditScore applicationCreditScore);

        /// <summary>
        ///
        /// </summary>
        /// <param name="callingContext"></param>
        void SaveCallingContext(ICallingContext callingContext);
    }
}