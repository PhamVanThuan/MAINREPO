using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IAffordabilityAssessmentStatus : IEntityValidation, IBusinessModelObject
    {
        System.String Description
        {
            get;
            set;
        }

        System.Int32 Key
        {
            get;
            set;
        }
    }
}