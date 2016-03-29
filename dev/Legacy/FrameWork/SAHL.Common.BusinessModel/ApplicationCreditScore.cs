using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO
    /// </summary>
    public partial class ApplicationCreditScore : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO>, IApplicationCreditScore
    {
        public ApplicationCreditScore(SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO ApplicationCreditScore)
            : base(ApplicationCreditScore)
        {
            this._DAO = ApplicationCreditScore;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO.Application
        /// </summary>
        public IApplication Application
        {
            get
            {
                if (null == _DAO.Application) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplication, Application_DAO>(_DAO.Application);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Application = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Application = (Application_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO.ApplicationAggregateDecision
        /// </summary>
        public IApplicationAggregateDecision ApplicationAggregateDecision
        {
            get
            {
                if (null == _DAO.ApplicationAggregateDecision) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationAggregateDecision, ApplicationAggregateDecision_DAO>(_DAO.ApplicationAggregateDecision);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationAggregateDecision = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationAggregateDecision = (ApplicationAggregateDecision_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO.ScoreDate
        /// </summary>
        public DateTime ScoreDate
        {
            get { return _DAO.ScoreDate; }
            set { _DAO.ScoreDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO.CallingContext
        /// </summary>
        public ICallingContext CallingContext
        {
            get
            {
                if (null == _DAO.CallingContext) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICallingContext, CallingContext_DAO>(_DAO.CallingContext);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CallingContext = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CallingContext = (CallingContext_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO.ITCDecisionReasons
        /// </summary>
        private DAOEventList<ITCDecisionReason_DAO, IITCDecisionReason, ITCDecisionReason> _ITCDecisionReasons;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCreditScore_DAO.ITCDecisionReasons
        /// </summary>
        public IEventList<IITCDecisionReason> ITCDecisionReasons
        {
            get
            {
                if (null == _ITCDecisionReasons)
                {
                    if (null == _DAO.ITCDecisionReasons)
                        _DAO.ITCDecisionReasons = new List<ITCDecisionReason_DAO>();
                    _ITCDecisionReasons = new DAOEventList<ITCDecisionReason_DAO, IITCDecisionReason, ITCDecisionReason>(_DAO.ITCDecisionReasons);
                    _ITCDecisionReasons.BeforeAdd += new EventListHandler(OnITCDecisionReasons_BeforeAdd);
                    _ITCDecisionReasons.BeforeRemove += new EventListHandler(OnITCDecisionReasons_BeforeRemove);
                    _ITCDecisionReasons.AfterAdd += new EventListHandler(OnITCDecisionReasons_AfterAdd);
                    _ITCDecisionReasons.AfterRemove += new EventListHandler(OnITCDecisionReasons_AfterRemove);
                }
                return _ITCDecisionReasons;
            }
        }

        /// <summary>
        /// An ApplicationCreditScore can have a many ApplicationITCCreditScores associated with it. This relationship is defined in the ApplicationITCCreditScore table where the
        /// Offer.OfferKey = OfferITCCreditScore.OfferKey.
        /// </summary>
        private DAOEventList<ApplicationITCCreditScore_DAO, IApplicationITCCreditScore, ApplicationITCCreditScore> _ApplicationITCCreditScores;

        /// <summary>
        /// An ApplicationCreditScore can have a many ApplicationITCCreditScores associated with it. This relationship is defined in the ApplicationITCCreditScore table where the
        /// Offer.OfferKey = OfferITCCreditScore.OfferKey.
        /// </summary>
        public IEventList<IApplicationITCCreditScore> ApplicationITCCreditScores
        {
            get
            {
                if (null == _ApplicationITCCreditScores)
                {
                    if (null == _DAO.ApplicationITCCreditScores)
                        _DAO.ApplicationITCCreditScores = new List<ApplicationITCCreditScore_DAO>();
                    _ApplicationITCCreditScores = new DAOEventList<ApplicationITCCreditScore_DAO, IApplicationITCCreditScore, ApplicationITCCreditScore>(_DAO.ApplicationITCCreditScores);
                    _ApplicationITCCreditScores.BeforeAdd += new EventListHandler(OnApplicationITCCreditScores_BeforeAdd);
                    _ApplicationITCCreditScores.BeforeRemove += new EventListHandler(OnApplicationITCCreditScores_BeforeRemove);
                    _ApplicationITCCreditScores.AfterAdd += new EventListHandler(OnApplicationITCCreditScores_AfterAdd);
                    _ApplicationITCCreditScores.AfterRemove += new EventListHandler(OnApplicationITCCreditScores_AfterRemove);
                }
                return _ApplicationITCCreditScores;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ITCDecisionReasons = null;
            _ApplicationITCCreditScores = null;
        }
    }
}