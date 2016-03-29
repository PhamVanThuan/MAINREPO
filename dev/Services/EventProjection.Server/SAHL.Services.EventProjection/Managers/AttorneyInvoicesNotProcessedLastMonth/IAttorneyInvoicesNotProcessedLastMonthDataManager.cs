using SAHL.Core.Data.Models.EventProjection;
namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedLastMonth
{
    public interface IAttorneyInvoicesNotProcessedLastMonthDataManager
    {
        void UpdateAttorneyInvoicesNotProcessedLastMonth(AttorneyInvoicesNotProcessedThisMonthDataModel model);
    }
}