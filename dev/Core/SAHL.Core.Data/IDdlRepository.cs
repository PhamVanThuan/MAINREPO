namespace SAHL.Core.Data
{
    public interface IDdlRepository : ISqlRepository
    {
        int Truncate(string tableName);

        int Create<TDataModel>(string tableName, string schema) where TDataModel : IDataModel;
    }
}