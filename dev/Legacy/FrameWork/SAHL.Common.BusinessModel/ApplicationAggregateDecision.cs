using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO
    /// </summary>
    public partial class ApplicationAggregateDecision : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO>, IApplicationAggregateDecision
    {
        public ApplicationAggregateDecision(SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO ApplicationAggregateDecision)
            : base(ApplicationAggregateDecision)
        {
            this._DAO = ApplicationAggregateDecision;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO.CreditScoreDecision
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
        /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO.PrimaryDecisionKey
        /// </summary>
        public Int32 PrimaryDecisionKey
        {
            get { return _DAO.PrimaryDecisionKey; }
            set { _DAO.PrimaryDecisionKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO.SecondaryDecisionKey
        /// </summary>
        public Int32? SecondaryDecisionKey
        {
            get { return _DAO.SecondaryDecisionKey; }
            set { _DAO.SecondaryDecisionKey = value; }
        }
    }
}