using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IManualDebitOrder : IEntityValidation, IBusinessModelObject
    {
        bool Active { get; }

        IManualDebitOrder Original { get; }
    }
}