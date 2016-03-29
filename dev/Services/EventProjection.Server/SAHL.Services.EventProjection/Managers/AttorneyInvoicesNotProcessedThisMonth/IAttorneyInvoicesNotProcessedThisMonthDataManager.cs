using SAHL.Core.Data.Models.EventProjection;
namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth
{
    public interface IAttorneyInvoicesNotProcessedThisMonthDataManager
    {
        void IncrementCountAndIncreaseMonthlyValue(decimal invoiceValue);

        void DecrementCountAndDecreaseMonthlyValue(decimal invoiceValue);

        void AdjustMonthlyValue(decimal adjustmentValue);
        AttorneyInvoicesNotProcessedThisMonthDataModel GetAttorneyInvoicesNotProcessedThisMonth();
    }
}