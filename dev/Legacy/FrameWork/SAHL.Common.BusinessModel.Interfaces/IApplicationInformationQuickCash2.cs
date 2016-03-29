using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IApplicationInformationQuickCash : IEntityValidation
    {
        System.Double GetMaximumQuickCash();
    }
}