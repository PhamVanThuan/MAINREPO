using System;

namespace SAHL.Core.Data
{
    public interface IDdlDbContext : IDisposable
    {
        int Truncate(string tableName);

        int Create<TDataModel>() where TDataModel : IDataModel;

        int Create<TDataModel>(TDataModel dataModel) where TDataModel : IDataModel;

        int Create<TDataModel>(string tableName, string schema) where TDataModel : IDataModel;
    }
}