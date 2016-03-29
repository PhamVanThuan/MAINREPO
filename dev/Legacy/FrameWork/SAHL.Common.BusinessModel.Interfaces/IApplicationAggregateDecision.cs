using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO
    /// </summary>
    public partial interface IApplicationAggregateDecision : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO.CreditScoreDecision
        /// </summary>
        ICreditScoreDecision CreditScoreDecision
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO.PrimaryDecisionKey
        /// </summary>
        System.Int32 PrimaryDecisionKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAggregateDecision_DAO.SecondaryDecisionKey
        /// </summary>
        Int32? SecondaryDecisionKey
        {
            get;
            set;
        }
    }
}