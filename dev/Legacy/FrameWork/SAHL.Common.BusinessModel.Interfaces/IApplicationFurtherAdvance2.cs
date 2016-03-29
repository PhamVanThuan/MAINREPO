using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IApplicationFurtherAdvance : IEntityValidation, IApplicationFurtherLending, IApplicationMortgageLoan
    {
        void SetProduct(IProduct product);
    }
}