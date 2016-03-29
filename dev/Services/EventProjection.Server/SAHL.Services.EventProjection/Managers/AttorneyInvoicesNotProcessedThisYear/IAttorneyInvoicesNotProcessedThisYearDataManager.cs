namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisYear
{
    public interface IAttorneyInvoicesNotProcessedThisYearDataManager
    {
        void IncrementCountAndIncreaseYearlyValue(decimal invoiceValue);

        void DecrementCountAndDecreaseYearlyValue(decimal invoiceValue);

        void AdjustYearlyValue(decimal adjustmentValue);
    }
}