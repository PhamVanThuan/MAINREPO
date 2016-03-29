using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IApplicationReAdvance : IEntityValidation, IApplicationFurtherLending, IApplicationMortgageLoan
    {
        void SetProduct(IProduct product);
    }
}