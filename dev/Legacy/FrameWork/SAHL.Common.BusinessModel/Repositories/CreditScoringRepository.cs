using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(ICreditScoringRepository))]
    public class CreditScoringRepository : AbstractRepositoryBase, ICreditScoringRepository
    {
        public CreditScoringRepository()
            : this(RepositoryFactory.GetRepository<IApplicationRepository>(),
            RepositoryFactory.GetRepository<IITCRepository>(),
            RepositoryFactory.GetRepository<ILookupRepository>(),
            RepositoryFactory.GetRepository<IReasonRepository>(),
            new SAHLPrincipalCacheProvider(new SAHLPrincipalProvider()),
              new CastleTransactionsService())
        {
        }

        public CreditScoringRepository(IApplicationRepository applciationRepository, IITCRepository ITCRepository, ILookupRepository lookupRepository, IReasonRepository reasonRepository, ISAHLPrincipalCacheProvider principalCacheProvider, ICastleTransactionsService castleTransactionService)
        {
            appRepo = applciationRepository;
            itcRepo = ITCRepository;
            lookup = lookupRepository;
            rRepo = reasonRepository;
            SPC = principalCacheProvider.GetPrincipalCache();
            this.castleTransactionService = castleTransactionService;
        }

        ICastleTransactionsService castleTransactionService;
        ILookupRepository lookup = null;
        IApplicationRepository appRepo = null;
        IITCRepository itcRepo = null;
        IReasonRepository rRepo = null;
        ISAHLPrincipalCache SPC = null;

        /// <summary>
        /// Calculate the total adjusted score for all a score card's SBC variables from a given ITC xml record
        /// </summary>
        /// <param name="itc">The ITC record containing the SBC variables</param>
        /// <param name="scoreCard">The ScoreCard to use to determine the score</param>
        /// <returns>total SBC score</returns>
        public double CalculateSBCScore(IITC itc, IScoreCard scoreCard)
        {
            // get the SBC's from the XML
            Dictionary<string, string> attributeValues = GetSBCFromXML(itc, scoreCard.Key);
            double totalScore = scoreCard.BasePoints;

            foreach (KeyValuePair<string, string> av in attributeValues)
            {
                IScoreCardAttribute attr = GetScoreCardAttributeByCode(scoreCard.Key, av.Key);
                totalScore += GetPointsForScoreCardAttributeByValue(attr, av.Value);
            }

            totalScore = Math.Exp(((-1) * totalScore) / 10);
            totalScore = (1 / (1 + totalScore));
            totalScore = Math.Round(totalScore * 1000);
            return totalScore;
        }

        /// <summary>
        /// Get a credit decision for a legal entity based on ITC data
        /// </summary>
        /// <param name="application"></param>
        /// <param name="legalEntity"></param>
        /// <param name="scoreCard"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public IITCCreditScore GenerateIndividualDecision(IApplication application, ILegalEntity legalEntity, IScoreCard scoreCard, IRiskMatrixRevision matrix)
        {
            return GenerateIndividualDecision(application, legalEntity, scoreCard, matrix, null);
        }

        /// <summary>
        /// Get a credit decision for a legal entity based on ITC data
        /// </summary>
        /// <param name="application"></param>
        /// <param name="legalEntity"></param>
        /// <param name="scoreCard"></param>
        /// <param name="matrix"></param>
        /// <param name="ADUserName">ADUser that performed the workflow activity which triggered the credit scoring</param>
        /// <returns></returns>
        public IITCCreditScore GenerateIndividualDecision(IApplication application, ILegalEntity legalEntity, IScoreCard scoreCard, IRiskMatrixRevision matrix, string ADUserName)
        {
            //ICreditScoreDecision decision = lookup.CreditScoreDecisions[CreditScoreDecisions.NoScore];
            IList<IITC> ITCs = itcRepo.GetITCByLEAndAccountKey(legalEntity.Key, application.ReservedAccount.Key);

            if (ITCs == null || ITCs.Count == 0)
                return null;

            IITC itc = ITCs[0];
            List<IDomainMessage> messageList = new List<IDomainMessage>();
            IITCCreditScore itcScore = GenerateITCCreditScore(application.Key, matrix, scoreCard, itc, ref messageList, ADUserName, legalEntity.Key);
            ReInsertDomainMessages(messageList);
            return itcScore;
        }

        /// <summary>
        /// Calculates a Credit Scoring Decision for an Offer from the aggregate of the Primary and Secondary applicant's decisions
        /// </summary>
        /// <param name="application">the Offer in question</param>
        /// <param name="matrix">the risk matrix to use</param>
        /// <param name="context">usually the workflow map and action from which this method was called</param>
        /// <returns>a CreditScoreDecision</returns>
        public IApplicationCreditScore GenerateApplicationCreditScore(IApplication application, IRiskMatrixRevision matrix, ICallingContext context)
        {
            return GenerateApplicationCreditScore(application, matrix, matrix, context, null);
        }

        /// <summary>
        /// Calculates a Credit Scoring Decision for an Offer from the aggregate of the Primary and Secondary applicant's decisions
        /// </summary>
        /// <param name="application"></param>
        /// <param name="primaryMatrix"></param>
        /// <param name="secondaryMatrix"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IApplicationCreditScore GenerateApplicationCreditScore(IApplication application, IRiskMatrixRevision primaryMatrix, IRiskMatrixRevision secondaryMatrix, ICallingContext context)
        {
            return GenerateApplicationCreditScore(application, primaryMatrix, secondaryMatrix, context, null);
        }

        /// <summary>
        /// Calculates a Credit Scoring Decision for an Offer from the aggregate of the Primary and Secondary applicant's decisions
        /// </summary>
        /// <param name="application"></param>
        /// <param name="primaryMatrix"></param>
        /// <param name="secondaryMatrix"></param>
        /// <param name="context"></param>
        /// <param name="ADUserName">ADUser that performed the workflow activity which triggered the credit scoring</param>
        /// <returns></returns>
        public IApplicationCreditScore GenerateApplicationCreditScore(IApplication application, IRiskMatrixRevision primaryMatrix, IRiskMatrixRevision secondaryMatrix, ICallingContext context, string ADUserName)
        {
            List<IDomainMessage> messageList = new List<IDomainMessage>();

            if (secondaryMatrix == null)
                secondaryMatrix = primaryMatrix;

            IScoreCard primaryScoreCard = GetScoreCardByRiskMatrixDimension(primaryMatrix, "SBC");
            IScoreCard secondaryScoreCard = GetScoreCardByRiskMatrixDimension(secondaryMatrix, "SBC");
            int primaryApplicant, secondaryApplicant = -1;
            int numApplicants = 0;
            appRepo.GetPrimaryAndSecondaryApplicants(application.Key, out primaryApplicant, out secondaryApplicant, out numApplicants);

            if (numApplicants == 0)
            {
                SPC.DomainMessages.Add(new Warning("The Offer has no Main Applicants or it has a non-natural-person Main Applicant with no Suretors - score cannot be generated", ""));
                return null;

                //throw new Exception("The Offer has no Main Applicants or it has a non-natural-person Main Applicant with no Suretors");
            }

            if (primaryApplicant == -1)
            {
                SPC.DomainMessages.Add(new Warning("The Primary Applicant for the Offer could not be determined - score cannot be generated", ""));
                return null;
            }

            //if (secondaryApplicant > 0 && secondaryMatrix == null)
            //    throw new Exception("The offer has more than one main applicant, but no secondary RiskMatrix was supplied");

            ICreditScoreDecision primaryDecision = null;
            ICreditScoreDecision secondaryDecision = null;// = lookup.CreditScoreDecisions[CreditScoreDecisions.NoScore];
            IITCCreditScore primaryITCreditScore = null;
            IITCCreditScore secondaryITCreditScore = null;
            IList<IITC> ITCs = null;
            IITC itc = null;

            if (primaryApplicant != -1)
            {
                ITCs = itcRepo.GetITCByLEAndAccountKey(primaryApplicant, application.ReservedAccount.Key);

                if (ITCs != null)
                {
                    primaryDecision = lookup.CreditScoreDecisions[CreditScoreDecisions.NoScore];
                    itc = ITCs.Count > 0 ? ITCs[0] : null;
                    primaryITCreditScore = GenerateITCCreditScore(application.Key, primaryMatrix, primaryScoreCard, itc, ref messageList, ADUserName, primaryApplicant);
                    primaryDecision = primaryITCreditScore.CreditScoreDecision;
                }
            }

            if (secondaryApplicant != -1)
            {
                ITCs = itcRepo.GetITCByLEAndAccountKey(secondaryApplicant, application.ReservedAccount.Key);

                if (ITCs != null)
                {
                    secondaryDecision = lookup.CreditScoreDecisions[CreditScoreDecisions.NoScore];
                    itc = ITCs.Count > 0 ? ITCs[0] : null;
                    secondaryITCreditScore = GenerateITCCreditScore(application.Key, secondaryMatrix, secondaryScoreCard, itc, ref messageList, ADUserName, secondaryApplicant);
                    secondaryDecision = secondaryITCreditScore.CreditScoreDecision;
                }
            }

            if (primaryDecision == null && secondaryDecision == null)
            {
                SPC.DomainMessages.Add(new Warning("Neither Primary nor Secondary applicant has an ITC record - score cannot be generated", ""));
                return null;
            }

            // Get the aggregate decision based on the outcome of scoring the primaryApplicant and secondaryApplicant applicants.
            IApplicationAggregateDecision aggregateDecision = GetAggregateDecision(primaryDecision, secondaryDecision);
            IApplicationCreditScore offerScore = CreateEmpty<IApplicationCreditScore, ApplicationCreditScore_DAO>();
            offerScore.Application = application;
            offerScore.ApplicationAggregateDecision = aggregateDecision;

            //offerScore.RiskMatrixRevision = revision;
            offerScore.ScoreDate = DateTime.Now;
            offerScore.CallingContext = context;
            base.Save<IApplicationCreditScore, ApplicationCreditScore_DAO>(offerScore);

            // Save the ApplicationITCCreditScore for each applicant now that we have an OfferCreditScore
            if (null != primaryITCreditScore)
            {
                SaveApplicationITCCreditScore(offerScore, primaryDecision, primaryITCreditScore, true);

                //link the decision reasons to the offercreditscore
                foreach (IITCDecisionReason reason in primaryITCreditScore.ITCDecisionReasons)
                {
                    reason.ApplicationCreditScore = offerScore;
                    base.Save<IITCDecisionReason, ITCDecisionReason_DAO>(reason);
                    offerScore.ITCDecisionReasons.Add(null, reason);
                }
            }

            if (secondaryITCreditScore != null)
            {
                SaveApplicationITCCreditScore(offerScore, secondaryDecision, secondaryITCreditScore, false);

                foreach (IITCDecisionReason reason in secondaryITCreditScore.ITCDecisionReasons)
                {
                    reason.ApplicationCreditScore = offerScore;
                    base.Save<IITCDecisionReason, ITCDecisionReason_DAO>(reason);
                    offerScore.ITCDecisionReasons.Add(null, reason);
                }
            }

            base.Save<IApplicationCreditScore, ApplicationCreditScore_DAO>(offerScore);

            ReInsertDomainMessages(messageList);
            return offerScore;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="itc"></param>
        /// <returns></returns>
        public double GetEmpiricaFromITC(IITC itc)
        {
            double EScore = 0;
            string SQL = UIStatementRepository.GetStatement("CreditScoring", "ValidEmpericaScore");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@ITCKey", itc.Key));
            string Response = castleTransactionService.ExecuteScalarOnCastleTran(SQL, typeof(GeneralStatus_DAO), prms).ToString();

            if (!double.TryParse(Response, out EScore))
                throw new Exception(string.Format("Unable to get Emperica score for ITC:{0}", itc.Key));

            return EScore;
        }

        /// <summary>
        /// Get all the credit scores for an application
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        public IEventList<IApplicationCreditScore> GetApplicationCreditScoreByApplicationKey(int applicationKey)
        {
            string HQL = "select d from ApplicationCreditScore_DAO d where d.Application.Key=?";
            SimpleQuery<ApplicationCreditScore_DAO> q = new SimpleQuery<ApplicationCreditScore_DAO>(HQL, applicationKey);
            ApplicationCreditScore_DAO[] arr = q.Execute();
            return new DAOEventList<ApplicationCreditScore_DAO, IApplicationCreditScore, ApplicationCreditScore>(arr);
        }

        /// <summary>
        /// Get all the credit scores for an application  - with sort option
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="latestFirst"></param>
        /// <returns></returns>
        public IEventList<IApplicationCreditScore> GetApplicationCreditScoreByApplicationKeySorted(int applicationKey, bool latestFirst)
        {
            string HQL = "select d from ApplicationCreditScore_DAO d where d.Application.Key=?";
            if (latestFirst)
                HQL += " order by d.ScoreDate desc";

            SimpleQuery<ApplicationCreditScore_DAO> q = new SimpleQuery<ApplicationCreditScore_DAO>(HQL, applicationKey);
            ApplicationCreditScore_DAO[] arr = q.Execute();
            return new DAOEventList<ApplicationCreditScore_DAO, IApplicationCreditScore, ApplicationCreditScore>(arr);
        }

        /// <summary>
        /// Get all the credit scores for a legal entity
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public IEventList<IITCCreditScore> GetITCCreditScoreByLegalEntity(int legalEntityKey, int accountKey)
        {
            string HQL = "from ITC_DAO d where d.LegalEntity.Key = ? and d.ReservedAccount.Key = ?";
            SimpleQuery<ITC_DAO> qi = new SimpleQuery<ITC_DAO>(HQL, legalEntityKey, accountKey);
            ITC_DAO[] itcs = qi.Execute();

            HQL = "from ITCArchive_DAO d where d.LegalEntityKey = ? and d.AccountKey = ?";
            SimpleQuery<ITCArchive_DAO> qa = new SimpleQuery<ITCArchive_DAO>(HQL, legalEntityKey, accountKey);
            ITCArchive_DAO[] arcs = qa.Execute();

            List<int> itcKeys = new List<int>();
            StringBuilder sb = new StringBuilder();

            foreach (ITC_DAO dao in itcs)
                sb.AppendFormat(",{0}", dao.Key);

            foreach (ITCArchive_DAO dao in arcs)
                sb.AppendFormat(",{0}", dao.Key);

            if (sb.Length == 0)
                return null;

            sb = sb.Remove(0, 1);

            HQL = String.Format("from ITCCreditScore_DAO d where d.ITCKey in ({0})", sb.ToString());
            SimpleQuery<ITCCreditScore_DAO> q = new SimpleQuery<ITCCreditScore_DAO>(HQL);
            ITCCreditScore_DAO[] arr = q.Execute();
            return new DAOEventList<ITCCreditScore_DAO, IITCCreditScore, ITCCreditScore>(arr);
        }

        public IDictionary<string, object> GetCreditScoreInfoForRules(IApplicationMortgageLoan app)
        {
            IDictionary<string, object> creditScoringInformation = new Dictionary<string, object>();

            ICreditScoringRepository csRepo = RepositoryFactory.GetRepository<ICreditScoringRepository>();
            IITCRepository itcRepo = RepositoryFactory.GetRepository<IITCRepository>();
            IApplicationMortgageLoan aml = app as IApplicationMortgageLoan;
            IApplicationProductMortgageLoan _applicationProduct = app.CurrentProduct as IApplicationProductMortgageLoan;
            ISupportsVariableLoanApplicationInformation vlai = _applicationProduct as ISupportsVariableLoanApplicationInformation;
            IRiskMatrixRevision matrixRevision = null;

            if (vlai == null)
                return new Dictionary<string, object>();

            double? LTV = vlai.VariableLoanInformation.LTV;
            int primaryLEKey = -1;
            int secondaryLEKey = -1;
            int numApplicants = 0;
            appRepo.GetPrimaryAndSecondaryApplicants(app.Key, out primaryLEKey, out secondaryLEKey, out numApplicants);

            if (primaryLEKey == -1 && secondaryLEKey == -1)
                return new Dictionary<string, object>();

            string HQL = "select d from RiskMatrix_DAO d where d.Description=? order by d.Key desc";

            if (null != LTV && LTV > 0.80)
            {
                SimpleQuery<RiskMatrix_DAO> q = new SimpleQuery<RiskMatrix_DAO>(HQL, "High LTV");
                RiskMatrix_DAO[] arr = q.Execute();
                IRiskMatrix rm = new RiskMatrix(arr[0]);
                matrixRevision = rm.RiskMatrixRevisions[0];
            }
            else
            {
                SimpleQuery<RiskMatrix_DAO> q = null;

                if (primaryLEKey > 0 && secondaryLEKey > 0)
                    q = new SimpleQuery<RiskMatrix_DAO>(HQL, "Low LTV - Joint");
                else
                    q = new SimpleQuery<RiskMatrix_DAO>(HQL, "Low LTV - Single");

                RiskMatrix_DAO[] arr = q.Execute();
                IRiskMatrix rm = new RiskMatrix(arr[0]);
                matrixRevision = rm.RiskMatrixRevisions[0];
            }

            IList<IITC> ITCs = itcRepo.GetITCByLEAndAccountKey(primaryLEKey, app.ReservedAccount.Key);

            if (ITCs != null && ITCs.Count > 0)
                creditScoringInformation.Add("primaryITC", ITCs[0]);

            ITCs = itcRepo.GetITCByLEAndAccountKey(secondaryLEKey, app.ReservedAccount.Key);

            if (ITCs != null && ITCs.Count > 0)
                creditScoringInformation.Add("secondaryITC", ITCs[0]);

            creditScoringInformation.Add("riskMatrixRevisionKey", matrixRevision.Key);

            HQL = "select d from ScoreCard_DAO d join d.RiskMatrixDimensions rd where rd.Key=2 order by d.RevisionDate desc";
            SimpleQuery<ScoreCard_DAO> sq = new SimpleQuery<ScoreCard_DAO>(HQL);
            ScoreCard_DAO[] cards = sq.Execute();

            if (cards != null && cards.Length > 0)
                creditScoringInformation.Add("scoreCard", new ScoreCard(cards[0]));

            return creditScoringInformation;
        }

        public IApplicationCreditScore GenerateApplicationCreditScore(IApplication application, ICallingContext callingContext, string adusername)
        {
            IRiskMatrixRevision matrixRevision = null;
            IApplicationProductMortgageLoan _applicationProduct = application.CurrentProduct as IApplicationProductMortgageLoan;
            ISupportsVariableLoanApplicationInformation vlai = _applicationProduct as ISupportsVariableLoanApplicationInformation;
            IApplicationCreditScore score = null;

            if (vlai != null)
            {
                double? LTV = vlai.VariableLoanInformation.LTV;
                if (null != LTV && LTV > 0.80)
                {
                    // "HIGH LTV"
                    IRiskMatrix riskMatrix = GetRiskMatrixByKey((int)riskMatrixs.HighLTV);
                    matrixRevision = riskMatrix.RiskMatrixRevisions[0];
                }
                else
                {
                    int primaryLEKey = 0;
                    int secondaryLEKey = 0;
                    int numApplicants = 0;
                    appRepo.GetPrimaryAndSecondaryApplicants(application.Key, out primaryLEKey, out secondaryLEKey, out numApplicants);

                    IRiskMatrix riskMatrix = null;

                    if (primaryLEKey > 0 && secondaryLEKey > 0)
                    {
                        //Low LTV - Joint
                        riskMatrix = GetRiskMatrixByKey((int)riskMatrixs.LowLTVJoint);
                    }
                    else
                    {
                        //Low LTV - Single
                        riskMatrix = GetRiskMatrixByKey((int)riskMatrixs.LowLTVSingle);
                    }

                    matrixRevision = riskMatrix.RiskMatrixRevisions[0];
                }

                score = GenerateApplicationCreditScore(application, matrixRevision, matrixRevision, callingContext, adusername);
            }
            return score;
        }

        #region standard gets

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetITCCreditScoreByKey"></see>.
        /// </summary>
        public IITCCreditScore GetITCCreditScoreByKey(int key)
        {
            return base.GetByKey<IITCCreditScore, ITCCreditScore_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetApplicationITCCreditScoreByKey"></see>.
        /// </summary>
        public IApplicationITCCreditScore GetApplicationITCCreditScoreByKey(int key)
        {
            return base.GetByKey<IApplicationITCCreditScore, ApplicationITCCreditScore_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetApplicationCreditScoreByKey"></see>.
        /// </summary>
        public IApplicationCreditScore GetApplicationCreditScoreByKey(int key)
        {
            return base.GetByKey<IApplicationCreditScore, ApplicationCreditScore_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetCreditScoreDecisionByKey"></see>.
        /// </summary>
        public ICreditScoreDecision GetCreditScoreDecisionByKey(int key)
        {
            return base.GetByKey<ICreditScoreDecision, CreditScoreDecision_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetApplicationAggregateDecisionByKey"></see>.
        /// </summary>
        public IApplicationAggregateDecision GetApplicationAggregateDecisionByKey(int key)
        {
            return base.GetByKey<IApplicationAggregateDecision, ApplicationAggregateDecision_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetITCDecisionReasonByKey"></see>.
        /// </summary>
        public IITCDecisionReason GetITCDecisionReasonByKey(int key)
        {
            return base.GetByKey<IITCDecisionReason, ITCDecisionReason_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetScoreCardByKey"></see>.
        /// </summary>
        public IScoreCard GetScoreCardByKey(int key)
        {
            return base.GetByKey<IScoreCard, ScoreCard_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetScoreCardAttributeByKey"></see>.
        /// </summary>
        public IScoreCardAttribute GetScoreCardAttributeByKey(int key)
        {
            return base.GetByKey<IScoreCardAttribute, ScoreCardAttribute_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetScoreCardAttributeRangeByKey"></see>.
        /// </summary>
        public IScoreCardAttributeRange GetScoreCardAttributeRangeByKey(int key)
        {
            return base.GetByKey<IScoreCardAttributeRange, ScoreCardAttributeRange_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetRiskMatrixByKey"></see>.
        /// </summary>
        public IRiskMatrix GetRiskMatrixByKey(int key)
        {
            return base.GetByKey<IRiskMatrix, RiskMatrix_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetRiskMatrixRevisionByKey"></see>.
        /// </summary>
        public IRiskMatrixRevision GetRiskMatrixRevisionByKey(int key)
        {
            return base.GetByKey<IRiskMatrixRevision, RiskMatrixRevision_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetRiskMatrixDimensionByKey"></see>.
        /// </summary>
        public IRiskMatrixDimension GetRiskMatrixDimensionByKey(int key)
        {
            return base.GetByKey<IRiskMatrixDimension, RiskMatrixDimension_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetRiskMatrixCellByKey"></see>.
        /// </summary>
        public IRiskMatrixCell GetRiskMatrixCellByKey(int key)
        {
            return base.GetByKey<IRiskMatrixCell, RiskMatrixCell_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetRiskMatrixRangeByKey"></see>.
        /// </summary>
        public IRiskMatrixRange GetRiskMatrixRangeByKey(int key)
        {
            return base.GetByKey<IRiskMatrixRange, RiskMatrixRange_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetCallingContextByKey"></see>.
        /// </summary>
        public ICallingContext GetCallingContextByKey(int key)
        {
            return base.GetByKey<ICallingContext, CallingContext_DAO>(key);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.GetCallingContextTypeByKey"></see>.
        /// </summary>
        public ICallingContextType GetCallingContextTypeByKey(int key)
        {
            return base.GetByKey<ICallingContextType, CallingContextType_DAO>(key);
        }

        #endregion standard gets

        #region standard creates

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IITCCreditScore CreateEmptyITCCreditScore()
        {
            return base.CreateEmpty<IITCCreditScore, ITCCreditScore_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IApplicationITCCreditScore CreateEmptyApplicationITCCreditScore()
        {
            return base.CreateEmpty<IApplicationITCCreditScore, ApplicationITCCreditScore_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IApplicationCreditScore CreateEmptyApplicationCreditScore()
        {
            return base.CreateEmpty<IApplicationCreditScore, ApplicationCreditScore_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ICallingContext CreateEmptyCallingContext()
        {
            return base.CreateEmpty<ICallingContext, CallingContext_DAO>();
        }

        #endregion standard creates

        #region standard saves

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.SaveITCCreditScore"></see>.
        /// </summary>
        public void SaveITCCreditScore(IITCCreditScore itcCreditScore)
        {
            base.Save<IITCCreditScore, ITCCreditScore_DAO>(itcCreditScore);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.SaveApplicationITCCreditScore"></see>.
        /// </summary>
        public void SaveApplicationITCCreditScore(IApplicationITCCreditScore applicationITCCreditScore)
        {
            base.Save<IApplicationITCCreditScore, ApplicationITCCreditScore_DAO>(applicationITCCreditScore);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.SaveApplicationCreditScore"></see>.
        /// </summary>
        public void SaveApplicationCreditScore(IApplicationCreditScore applicationCreditScore)
        {
            base.Save<IApplicationCreditScore, ApplicationCreditScore_DAO>(applicationCreditScore);
        }

        /// <summary>
        /// Implements <see cref="ICreditScoringRepository.SaveCallingContext"></see>.
        /// </summary>
        public void SaveCallingContext(ICallingContext callingContext)
        {
            base.Save<ICallingContext, CallingContext_DAO>(callingContext);
        }

        #endregion standard saves

        #region private methods

        private IITCCreditScore GenerateITCCreditScore(int applicationKey, IRiskMatrixRevision matrix, IScoreCard scoreCard, IITC itc, ref List<IDomainMessage> messageList, string ADUserName, int iLegalEntityKey)
        {
            IITCCreditScore itcCreditScore = CreateEmpty<IITCCreditScore, ITCCreditScore_DAO>();

            itcCreditScore.CreditScoreDecision = lookup.CreditScoreDecisions[CreditScoreDecisions.NoScore];
            itcCreditScore.EmpiricaScore = null;
            itcCreditScore.GeneralStatus = lookup.GeneralStatuses[GeneralStatuses.Active];
            if (itc != null)
                itcCreditScore.ITCKey = itc.Key;

            itcCreditScore.RiskMatrixCell = null;
            itcCreditScore.RiskMatrixRevision = matrix;
            itcCreditScore.SBCScore = null;
            itcCreditScore.ScoreCard = scoreCard;
            itcCreditScore.ScoreDate = DateTime.Now;
            itcCreditScore.ADUserName = ADUserName;
            itcCreditScore.LegalEntity = RepositoryFactory.GetRepository<ILegalEntityRepository>().GetLegalEntityByKey(iLegalEntityKey);

            base.Save<IITCCreditScore, ITCCreditScore_DAO>(itcCreditScore);

            if (itc == null)
            {
                // write the reject reason
                IReason reason = SaveReason(applicationKey, ReasonTypes.CreditScoringQuery, (int)ReasonDescriptions.NocreditscoreNoITCdata, "No credit score could be determined for this Legal Entity due to no ITC credit record being available.");
                IITCDecisionReason dr = SaveITCDecisionReason(itcCreditScore, reason, null);
                itcCreditScore.ITCDecisionReasons.Add(null, dr);

                base.Save<IITCCreditScore, ITCCreditScore_DAO>(itcCreditScore);

                return itcCreditScore;
            }

            bool validITC = ValidateITCXml(itc, itcCreditScore, applicationKey, ref messageList);

            RunRules(itc, itcCreditScore, applicationKey, ref messageList);

            //if ITC is invalid or we got a knockout rule 'Decline' or 'Refer', save and exit, no need to score
            if (!validITC || (itcCreditScore.CreditScoreDecision.Key == lookup.CreditScoreDecisions[CreditScoreDecisions.Decline].Key
                          || itcCreditScore.CreditScoreDecision.Key == lookup.CreditScoreDecisions[CreditScoreDecisions.Refer].Key))
            {
                base.Save<IITCCreditScore, ITCCreditScore_DAO>(itcCreditScore);
                return itcCreditScore;
            }

            double empiricaScore = GetEmpiricaFromITC(itc);
            double SBCScore = CalculateSBCScore(itc, scoreCard);

            //get the matrix decision
            IRiskMatrixCell cell = GetRiskMatrixCellByScoreIntersection(empiricaScore, SBCScore, matrix);

            itcCreditScore.EmpiricaScore = empiricaScore;
            itcCreditScore.SBCScore = SBCScore;
            itcCreditScore.RiskMatrixCell = cell;

            // use the cell's decision if there is one - otherwise use 'NoScore'
            itcCreditScore.CreditScoreDecision = cell.CreditScoreDecision != null ? cell.CreditScoreDecision : lookup.CreditScoreDecisions[CreditScoreDecisions.NoScore];

            //a decline at this point should only be due to a score decline.
            if (itcCreditScore.CreditScoreDecision.Key == lookup.CreditScoreDecisions[CreditScoreDecisions.Decline].Key)
            {
                IReason reason = SaveReason(applicationKey, ReasonTypes.CreditScoringDecline, (int)ReasonDescriptions.CreditScoreDecline, "Your ITC credit record failed to meet with our credit and affordability policy requirements.");
                IITCDecisionReason dr = SaveITCDecisionReason(itcCreditScore, reason, null);
                itcCreditScore.ITCDecisionReasons.Add(null, dr);
            }

            //Added by Terence to Add Risk Matrix Refer Reason
            //---------------------------
            if (itcCreditScore.CreditScoreDecision.Key == lookup.CreditScoreDecisions[CreditScoreDecisions.Refer].Key)
            {
                IReason reason = SaveReason(applicationKey, ReasonTypes.CreditScoringQuery, (int)ReasonDescriptions.RiskMatrixRefer, "A manual credit assessment is required on this Legal Entity.");
                IITCDecisionReason dr = SaveITCDecisionReason(itcCreditScore, reason, null);
                itcCreditScore.ITCDecisionReasons.Add(null, dr);
            }

            //---------------------------

            //policy knockout rules may alter the decision
            base.Save<IITCCreditScore, ITCCreditScore_DAO>(itcCreditScore);
            return itcCreditScore;
        }

        /// <summary>
        /// Gets a RiskMatrixCell by cross-referencing empirica and sbc scores
        /// </summary>
        /// <param name="empiricaScore"></param>
        /// <param name="sbcScore"></param>
        /// <param name="matrixRevision"></param>
        /// /// <returns>a RiskMatrixCell</returns>
        private IRiskMatrixCell GetRiskMatrixCellByScoreIntersection(double empiricaScore, double sbcScore, IRiskMatrixRevision matrixRevision)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("declare @empiricaScore float; set @empiricaScore = {0}; ", empiricaScore);
            sb.AppendFormat("declare @sbcScore float; set @sbcScore = {0}; ", sbcScore);
            sb.AppendFormat("declare @revisionKey int; set @revisionKey = {0}; ", matrixRevision.Key);
            sb.AppendLine(@"select cell.* from [2AM].dbo.RiskMatrixCell cell
                        join [2AM].dbo.RiskMatrixCellDimension cd on cd.RiskMatrixCellKey = cell.RiskMatrixCellKey
                        join [2AM].dbo.RiskMatrixDimension dim on dim.RiskMatrixDimensionKey = cd.RiskMatrixDimensionKey
                        join [2AM].dbo.RiskMatrixRange rmr on rmr.RiskMatrixRangeKey = cd.RiskMatrixRangeKey
                        where cell.RiskMatrixRevisionKey = @revisionKey
                        and cell.GeneralStatusKey = 1
                        and dim.[Description] = 'Empirica'
                        and (@empiricaScore >= isnull(rmr.[Min], - 1.79E+308) and @empiricaScore <= isnull(rmr.[max], 1.79E+308) )
                        intersect
                        select cell.* from [2AM].dbo.RiskMatrixCell cell
                        join [2AM].dbo.RiskMatrixCellDimension cd on cd.RiskMatrixCellKey = cell.RiskMatrixCellKey
                        join [2AM].dbo.RiskMatrixDimension dim on dim.RiskMatrixDimensionKey = cd.RiskMatrixDimensionKey
                        join [2AM].dbo.RiskMatrixRange rmr on rmr.RiskMatrixRangeKey = cd.RiskMatrixRangeKey
                        where cell.RiskMatrixRevisionKey = @revisionKey
                        and cell.GeneralStatusKey = 1
                        and dim.[Description] = 'SBC'
                        and (@sbcScore >= isnull(rmr.[Min], - 1.79E+308) and @sbcScore <= isnull(rmr.[max], 1.79E+308) )");

            SimpleQuery<RiskMatrixCell_DAO> q = new SimpleQuery<RiskMatrixCell_DAO>(QueryLanguage.Sql, sb.ToString());
            q.AddSqlReturnDefinition(typeof(RiskMatrixCell_DAO), "cell");
            RiskMatrixCell_DAO[] arr = q.Execute();

            if (arr == null || arr.Length == 0)
                return null;

            if (arr.Length > 1)
                throw new Exception("More than 1 cell was found at the intersection of the given scores - bad data.");

            return new RiskMatrixCell(arr[0]);
        }

        #region fxcop

        #endregion fxcop

        /// <summary>
        /// Gets the ScoreCard associated with a RiskMatrixDimension
        /// </summary>
        /// <param name="revision">a RiskMatrixRevision</param>
        /// <param name="dimensionDescription">the description of the dimension ie. Empirica or SBC</param>
        /// <returns>a ScoreCard</returns>
        private IScoreCard GetScoreCardByRiskMatrixDimension(IRiskMatrixRevision revision, string dimensionDescription)
        {
            foreach (IRiskMatrixDimension dim in revision.RiskMatrixCells[0].RiskMatrixDimensions)
            {
                if (dim.Description == dimensionDescription)
                    return GetLatestScoreCardByRiskMatrixDimensionKey(dim.Key);
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dimensionKey"></param>
        /// <returns></returns>
        private IScoreCard GetLatestScoreCardByRiskMatrixDimensionKey(int dimensionKey)
        {
            string HQL = "select d from ScoreCard_DAO d join d.RiskMatrixDimensions rd where rd.Key=? order by d.RevisionDate desc";
            SimpleQuery<ScoreCard_DAO> q = new SimpleQuery<ScoreCard_DAO>(HQL, dimensionKey);
            ScoreCard_DAO[] arr = q.Execute();

            if (arr != null && arr.Length > 0)
                return new ScoreCard(arr[0]);

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="itc"></param>
        /// <param name="scoreCardKey"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetSBCFromXML(IITC itc, int scoreCardKey)
        {
            StringBuilder sb = new StringBuilder();
            ScoreCardAttribute_DAO[] attribs = ScoreCardAttribute_DAO.FindAllByProperty("ScoreCardKey.Key", scoreCardKey);

            foreach (ScoreCardAttribute_DAO attr in attribs)
            {
                sb.AppendFormat(", convert(varchar(max),ResponseXML.query('BureauResponse/TUBureau:StandardBatchCharsSB33/TUBureau:{0}/text()')) {1}", attr.Code, attr.Code);
                sb.AppendLine();
            }

            if (sb.Length > 0)
                sb = sb.Replace(",", "WITH XMLNAMESPACES( 'https://secure.transunion.co.za/TUBureau' AS  \"TUBureau\") SELECT", 0, 1);

            sb.AppendFormat("from [2AM].dbo.ITC (nolock) where ITCKey = {0};", itc.Key);

            //string SQL = UIStatementRepository.GetStatement("CreditScoring", "GetSBCValuesFromITC");
            //ParameterCollection prms = new ParameterCollection();
            //prms.Add(new SqlParameter("@ITCKey", itc.Key));
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), typeof(GeneralStatus_DAO), null);
            DataRow dr = ds.Tables[0].Rows[0];
            Dictionary<string, string> atts = new Dictionary<string, string>();
            foreach (DataColumn dc in dr.Table.Columns)
            {
                atts.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
            }
            return atts;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scoreCardKey"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private IScoreCardAttribute GetScoreCardAttributeByCode(int scoreCardKey, string code)
        {
            string HQL = "select d from ScoreCardAttribute_DAO d where d.ScoreCardKey=? and d.Code=?";
            SimpleQuery<ScoreCardAttribute_DAO> q = new SimpleQuery<ScoreCardAttribute_DAO>(HQL, scoreCardKey, code);
            ScoreCardAttribute_DAO[] arr = q.Execute();

            if (null != arr && arr.Length > 0)
                return new ScoreCardAttribute(arr[0]);

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="primaryScore"></param>
        /// <param name="secondaryScore"></param>
        /// <returns></returns>
        private IApplicationAggregateDecision GetAggregateDecision(ICreditScoreDecision primaryScore, ICreditScoreDecision secondaryScore)
        {
            string HQL = String.Format("select d from ApplicationAggregateDecision_DAO d where d.PrimaryDecisionKey{0} and d.SecondaryDecisionKey{1}", primaryScore == null ? " is null" : "=" + Convert.ToString(primaryScore.Key), secondaryScore == null ? " is null" : "=" + Convert.ToString(secondaryScore.Key));
            SimpleQuery<ApplicationAggregateDecision_DAO> q = new SimpleQuery<ApplicationAggregateDecision_DAO>(HQL);

            ApplicationAggregateDecision_DAO[] arr = q.Execute();

            if (null != arr && arr.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IApplicationAggregateDecision, ApplicationAggregateDecision_DAO>(arr[0]);
            }
            else
            {
                throw new Exception(string.Format("No ApplicationAggregateDecision exists with PrimaryDecision '{0}' and SecondaryDecision '{1}'", primaryScore.Description, secondaryScore != null ? secondaryScore.Description : "null"));
            }
        }

        /// <summary>
        /// Given an SBC variable and it's value, this method will determine the range that the value falls in and return the associated points
        /// </summary>
        /// <param name="attribute">a ScoreCardAttribute (SBC variable)</param>
        /// <param name="attributeValue">the value of the SBC variable (from ITC xml)</param>
        /// <returns>the points for the appropriate ScoreCardAttributRange</returns>
        private double GetPointsForScoreCardAttributeByValue(IScoreCardAttribute attribute, string attributeValue)
        {
            double value = 0;

            if (!double.TryParse(attributeValue, out value))
                return 0;

            foreach (IScoreCardAttributeRange range in attribute.ScoreCardAttributeRanges)
            {
                if (!range.Min.HasValue && !range.Max.HasValue)
                    return range.Points;

                if (!range.Min.HasValue)
                    if (value <= range.Max.Value)
                        return range.Points;
                    else
                        continue;

                if (!range.Max.HasValue)
                    if (value >= range.Min.Value)
                        return range.Points;
                    else
                        continue;

                if (value >= range.Min.Value && value <= range.Max.Value)
                    return range.Points;
            }

            //SPC.DomainMessages.Add(new Warning("The supplied ScoreCardAttribute value does not fall in to any of the ScoreCardAttributeRanges", ""));
            throw new Exception("The supplied ScoreCardAttribute value does not fall in to any of the ScoreCardAttributeRanges");
        }

        //IITCCreditScore SaveITCCreditScore(ICreditScoreDecision decision, double empiricaScore, double sbcScore, IITC itc, IRiskMatrixCell cell, IRiskMatrixRevision revision, IScoreCard scoreCard)
        //{
        //    IITCCreditScore score = CreateEmpty<IITCCreditScore, ITCCreditScore_DAO>();

        //    score.CreditScoreDecision = decision;
        //    score.EmpiricaScore = empiricaScore;
        //    score.GeneralStatus = lookup.GeneralStatuses[GeneralStatuses.Active];
        //    score.ITCKey = itc.Key;
        //    score.RiskMatrixCell = cell;
        //    score.RiskMatrixRevision = revision;
        //    score.SBCScore = sbcScore;
        //    score.ScoreCard = scoreCard;
        //    score.ScoreDate = DateTime.Now;

        //    base.Save<IITCCreditScore, ITCCreditScore_DAO>(score);
        //    return score;
        //}

        private IApplicationITCCreditScore SaveApplicationITCCreditScore(IApplicationCreditScore offerCreditScore, ICreditScoreDecision decision, IITCCreditScore itcCreditScore, bool isPrimary)
        {
            IApplicationITCCreditScore appITCCreditScore = CreateEmpty<IApplicationITCCreditScore, ApplicationITCCreditScore_DAO>();

            appITCCreditScore.ApplicationCreditScore = offerCreditScore;
            appITCCreditScore.CreditScoreDecision = decision;
            appITCCreditScore.ITCCreditScore = itcCreditScore;
            appITCCreditScore.PrimaryApplicant = isPrimary;
            appITCCreditScore.ScoreDate = DateTime.Now;

            Save<IApplicationITCCreditScore, ApplicationITCCreditScore_DAO>(appITCCreditScore);
            return appITCCreditScore;
        }

        private void ExtractDomainMessages(List<IDomainMessage> messageList)
        {
            foreach (IDomainMessage dm in SPC.DomainMessages)
            {
                messageList.Add(dm);
            }
            SPC.DomainMessages.Clear();
        }

        private void ReInsertDomainMessages(List<IDomainMessage> messageList)
        {
            foreach (IDomainMessage dm in messageList)
            {
                SPC.DomainMessages.Add(dm);
            }
        }

        private bool ValidateITCXml(IITC itc, IITCCreditScore itcScore, int applicationKey, ref List<IDomainMessage> messageList)
        {
            IRuleService ruleService = ServiceFactory.GetService<IRuleService>();

            bool getOnMyHorse = true;
            int result = 0;
            result = ruleService.ExecuteRule(SPC.DomainMessages, "ITCValidEmpericaScore", new object[] { applicationKey, itc, itcScore.RiskMatrixRevision.Key });
            if (result != 0)
            {
                ExtractDomainMessages(messageList);
                IReason reason = SaveReason(applicationKey, ReasonTypes.CreditScoringQuery, (int)ReasonDescriptions.NocreditscoreEmpiricaScorenotavailable, "No credit score could be determined for this Legal Entity due to no  Empirica Score available in the ITC credit record.");
                IITCDecisionReason dr = SaveITCDecisionReason(itcScore, reason, null);
                itcScore.ITCDecisionReasons.Add(null, dr);

                getOnMyHorse = false;
            }
            result = ruleService.ExecuteRule(SPC.DomainMessages, "ITCValidSBC", new object[] { applicationKey, itc, itcScore.RiskMatrixRevision.Key, itcScore.ScoreCard });
            if (result != 0)
            {
                ExtractDomainMessages(messageList);
                IReason reason = SaveReason(applicationKey, ReasonTypes.CreditScoringQuery, (int)ReasonDescriptions.NocreditscoreMissingIncompleteSBCinformation, "No credit score could be determined for this Legal Entity due to  missing/incomplete SBC information in ITC credit record.");
                IITCDecisionReason dr = SaveITCDecisionReason(itcScore, reason, null);
                itcScore.ITCDecisionReasons.Add(null, dr);

                getOnMyHorse = false;
            }

            return getOnMyHorse;
        }

        private void RunRules(IITC itc, IITCCreditScore itcScore, int applicationKey, ref List<IDomainMessage> messageList)
        {
            IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
            int result = 0;

            result = ruleService.ExecuteRule(SPC.DomainMessages, "ITCDisputeIndicated", new object[] { applicationKey, itc });
            if (result != 0)
            {
                ExtractDomainMessages(messageList);

                IReason reason = SaveReason(applicationKey, ReasonTypes.CreditScoringQuery, (int)ReasonDescriptions.ITCKnockoutRuleDisputes, "SA Home Loan’s cannot enter into a credit agreement with you in light of the fact you are indicated as being involved with a dispute.");

                IITCDecisionReason dr = SaveITCDecisionReason(itcScore, reason, null);
                itcScore.ITCDecisionReasons.Add(null, dr);
                itcScore.CreditScoreDecision = lookup.CreditScoreDecisions[CreditScoreDecisions.Refer];
            }

            result = ruleService.ExecuteRule(SPC.DomainMessages, "ITCDebtReviewIndicated", new object[] { applicationKey, itc });
            if (result != 0)
            {
                ExtractDomainMessages(messageList);

                // store the reasons
                //reasonDefinitions.Add("ITCDebtReviewIndicated", "SA Home Loan’s cannot enter into a credit agreement with you in light of your debt review/insolvency / sequestration/ administration order.");
                IReason reason = SaveReason(applicationKey, ReasonTypes.CreditScoringDecline, (int)ReasonDescriptions.ITCKnockoutRuleDebtReview, "SA Home Loan’s cannot enter into a credit agreement with you in light of the fact you are indicated as being under debt review on your ITC credit record.");
                IITCDecisionReason dr = SaveITCDecisionReason(itcScore, reason, null);
                itcScore.ITCDecisionReasons.Add(null, dr);
                itcScore.CreditScoreDecision = lookup.CreditScoreDecisions[CreditScoreDecisions.Decline];
            }

            result = ruleService.ExecuteRule(SPC.DomainMessages, "ITCAccountLegalNoticesIndicated", new object[] { applicationKey, itc });
            if (result != 0)
            {
                ExtractDomainMessages(messageList);

                // store the reasons
                //reasonDefinitions.Add("ITCAccountLegalNoticesIndicated", "SA Home Loan’s cannot enter into a credit agreement with you in light of fact that you have [G044] notices");
                IReason reason = SaveReason(applicationKey, ReasonTypes.CreditScoringDecline, (int)ReasonDescriptions.ITCKnockoutRuleNotices, "SA Home Loan’s cannot enter into a credit agreement with you in light of fact that you have legal notices on your credit record");
                IITCDecisionReason dr = SaveITCDecisionReason(itcScore, reason, null);
                itcScore.ITCDecisionReasons.Add(null, dr);
                itcScore.CreditScoreDecision = lookup.CreditScoreDecisions[CreditScoreDecisions.Decline];
            }

            result = ruleService.ExecuteRule(SPC.DomainMessages, "ITCAccountDefaultsIndicated", new object[] { applicationKey, itc });
            if (result != 0)
            {
                ExtractDomainMessages(messageList);

                // store the reasons
                //reasonDefinitions.Add("ITCAccountDefaultsIndicated", "SA Home Loan’s cannot enter into a credit agreement with you in light of fact that you have defaults in the last [G012] months");
                IReason reason = SaveReason(applicationKey, ReasonTypes.CreditScoringDecline, (int)ReasonDescriptions.ITCKnockoutRuleDefaults, "SA Home Loan’s cannot enter into a credit agreement with you in light of fact that you have high number of defaults on your ITC credit record");
                IITCDecisionReason dr = SaveITCDecisionReason(itcScore, reason, null);
                itcScore.ITCDecisionReasons.Add(null, dr);
                itcScore.CreditScoreDecision = lookup.CreditScoreDecisions[CreditScoreDecisions.Decline];
            }

            result = ruleService.ExecuteRule(SPC.DomainMessages, "ITCAccountCustomerWorstEverPaymentProfileStatus", new object[] { applicationKey, itc });
            if (result != 0)
            {
                ExtractDomainMessages(messageList);

                // store the reasons
                //reasonDefinitions.Add("ITCAccountCustomerWorstEverPaymentProfileStatus", "");
                IReason reason = SaveReason(applicationKey, ReasonTypes.CreditScoringDecline, (int)ReasonDescriptions.ITCKnockoutRuleWorstEverPaymentProfile, "SA Home Loan’s cannot enter into a credit agreement with you in light of fact that you have overdue payments on your ITC credit record");
                IITCDecisionReason dr = SaveITCDecisionReason(itcScore, reason, null);
                itcScore.ITCDecisionReasons.Add(null, dr);
                if (result != 2) // Refer
                    itcScore.CreditScoreDecision = lookup.CreditScoreDecisions[CreditScoreDecisions.Decline];
                else
                    itcScore.CreditScoreDecision = lookup.CreditScoreDecisions[CreditScoreDecisions.Refer];
            }

            result = ruleService.ExecuteRule(SPC.DomainMessages, "ITCAccountJudgementsIndicated", new object[] { applicationKey, itc });
            if (result != 0)
            {
                ExtractDomainMessages(messageList);

                //store the reasons
                //reasonDefinitions.Add("ITCAccountJudgementsIndicated", "SA Home Loan’s cannot enter into a credit agreement with you in light of fact that you have judgements in the last [G032] months");
                IReason reason = SaveReason(applicationKey, ReasonTypes.CreditScoringDecline, (int)ReasonDescriptions.ITCKnockoutRuleJudgments, "SA Home Loan’s cannot enter into a credit agreement with you in light of fact that you have recent judgements on your credit record");
                IITCDecisionReason dr = SaveITCDecisionReason(itcScore, reason, null);
                itcScore.ITCDecisionReasons.Add(null, dr);
                itcScore.CreditScoreDecision = lookup.CreditScoreDecisions[CreditScoreDecisions.Decline];
            }

            //SaveITCCreditScore(itcScore);
        }

        private IReason SaveReason(int applicationKey, ReasonTypes reasonType, int reasonDescriptionKey, string comment)
        {
            IReadOnlyEventList<IReasonDefinition> rd = rRepo.GetReasonDefinitionsByReasonDescriptionKey(reasonType, reasonDescriptionKey);

            if (rd == null || rd.Count == 0)
                throw new Exception(string.Format("No ReasonDefinition found matching the ReasonDescriptionKey '{0}'", reasonDescriptionKey));

            IReasonDefinition def = rd[0];
            IReason reason = rRepo.CreateEmptyReason();
            reason.Comment = comment;
            reason.GenericKey = applicationKey;
            reason.ReasonDefinition = def;
            rRepo.SaveReason(reason);
            return reason;
        }

        private IITCDecisionReason SaveITCDecisionReason(IITCCreditScore score, IReason reason, IApplicationCreditScore offerScore)
        {
            IITCDecisionReason dr = CreateEmpty<IITCDecisionReason, ITCDecisionReason_DAO>();
            dr.ApplicationCreditScore = offerScore;
            dr.CreditScoreDecision = score.CreditScoreDecision;
            dr.ITCCreditScore = score;
            dr.Reason = reason;
            Save<IITCDecisionReason, ITCDecisionReason_DAO>(dr);
            return dr;
        }

        #endregion private methods
    }
}