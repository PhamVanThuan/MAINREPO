using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IAffordabilityAssessmentStressFactor : IEntityValidation, IBusinessModelObject
    {
        System.Int32 Key
        {
            get;
            set;
        }

        System.String StressFactorPercentage
        {
            get;
            set;
        }

        System.Double PercentageIncreaseOnRepayments
        {
            get;
            set;
        }
    }
}