namespace SAHL.Core.TextSearch
{
    public interface IQueryResultModel
    {
        int DocumentId { get; set; }

        float Score { get; set; }
    }
}