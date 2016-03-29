using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IAffordabilityAssessmentRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        IList<IAffordabilityAssessment> GetActiveApplicationAffordabilityAssessments(int applicationKey);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="affordabilityAssessmentKey"></param>
        /// <returns></returns>
        IAffordabilityAssessment GetAffordabilityAssessmentByKey(int affordabilityAssessmentKey);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="affordabilityAssessment"></param>
        void SaveAffordabilityAssessment(IAffordabilityAssessment affordabilityAssessment);
    }
}