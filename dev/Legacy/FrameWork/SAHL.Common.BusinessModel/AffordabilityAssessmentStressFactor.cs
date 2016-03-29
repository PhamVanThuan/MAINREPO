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
    /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessmentStressFactor_DAO
    /// </summary>
    public partial class AffordabilityAssessmentStressFactor : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AffordabilityAssessmentStressFactor_DAO>, IAffordabilityAssessmentStressFactor
    {
        public AffordabilityAssessmentStressFactor(SAHL.Common.BusinessModel.DAO.AffordabilityAssessmentStressFactor_DAO AffordabilityAssessmentStressFactor)
            : base(AffordabilityAssessmentStressFactor)
        {
            this._DAO = AffordabilityAssessmentStressFactor;
        }

        public int Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        public string StressFactorPercentage
        {
            get { return _DAO.StressFactorPercentage; }
            set { _DAO.StressFactorPercentage = value; }
        }

        public double PercentageIncreaseOnRepayments
        {
            get { return _DAO.PercentageIncreaseOnRepayments; }
            set { _DAO.PercentageIncreaseOnRepayments = value; }
        }
    }
}