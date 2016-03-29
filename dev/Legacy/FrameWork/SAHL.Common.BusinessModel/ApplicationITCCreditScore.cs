using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO
    /// </summary>
    public partial class ApplicationITCCreditScore : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO>, IApplicationITCCreditScore
    {
        public ApplicationITCCreditScore(SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO ApplicationITCCreditScore)
            : base(ApplicationITCCreditScore)
        {
            this._DAO = ApplicationITCCreditScore;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO.ApplicationCreditScore
        /// </summary>
        public IApplicationCreditScore ApplicationCreditScore
        {
            get
            {
                if (null == _DAO.ApplicationCreditScore) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationCreditScore, ApplicationCreditScore_DAO>(_DAO.ApplicationCreditScore);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationCreditScore = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationCreditScore = (ApplicationCreditScore_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO.ITCCreditScore
        /// </summary>
        public IITCCreditScore ITCCreditScore
        {
            get
            {
                if (null == _DAO.ITCCreditScore) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IITCCreditScore, ITCCreditScore_DAO>(_DAO.ITCCreditScore);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ITCCreditScore = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ITCCreditScore = (ITCCreditScore_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO.CreditScoreDecision
        /// </summary>
        public ICreditScoreDecision CreditScoreDecision
        {
            get
            {
                if (null == _DAO.CreditScoreDecision) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICreditScoreDecision, CreditScoreDecision_DAO>(_DAO.CreditScoreDecision);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CreditScoreDecision = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CreditScoreDecision = (CreditScoreDecision_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO.ScoreDate
        /// </summary>
        public DateTime ScoreDate
        {
            get { return _DAO.ScoreDate; }
            set { _DAO.ScoreDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationITCCreditScore_DAO.PrimaryApplicant
        /// </summary>
        public Boolean PrimaryApplicant
        {
            get { return _DAO.PrimaryApplicant; }
            set { _DAO.PrimaryApplicant = value; }
        }
    }
}