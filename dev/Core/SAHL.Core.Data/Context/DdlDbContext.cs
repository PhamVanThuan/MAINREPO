using System;

namespace SAHL.Core.Data.Context
{
    public class DdlDbContext : DbContextBase, IDdlDbContext
    {
        private const string defaultSchema = "dbo";
        private readonly IDdlRepository ddlRepository;

        public DdlDbContext(IDdlRepository ddlRepository, IDbConnectionProviderStorage connectionProviderStorage,
            IDbConnectionProviderFactory connectionProviderFactory, string connectionContextName,
            TransactionScopeTypeEnum scopeType = TransactionScopeTypeEnum.None)
            : base(connectionProviderStorage, connectionProviderFactory, ddlRepository, connectionContextName, scopeType
                )
        {
            if (ddlRepository == null)
            {
                throw new ArgumentNullException("ddlRepository");
            }
            this.ddlRepository = ddlRepository;
        }

        public int Truncate(string tableName)
        {
            return ddlRepository.Truncate(tableName);
        }

        public int Create<TDataModel>() where TDataModel : IDataModel
        {
            return Create<TDataModel>(typeof (TDataModel).Name, defaultSchema);
        }

        public int Create<TDataModel>(TDataModel dataModel) where TDataModel : IDataModel
        {
            return Create<TDataModel>(dataModel.GetType().Name, defaultSchema);
        }

        public int Create<TDataModel>(string tableName, string schema) where TDataModel : IDataModel
        {
            return ddlRepository.Create<TDataModel>(GetTableName<TDataModel>(tableName), GetSchema(schema));
        }

        private static string GetSchema(string schema)
        {
            return string.IsNullOrEmpty(schema) ? defaultSchema : schema;
        }

        private static string GetTableName<TDataModel>(string tableName) where TDataModel : IDataModel
        {
            return string.IsNullOrEmpty(tableName) ? typeof (TDataModel).Name : tableName;
        }
    }
}