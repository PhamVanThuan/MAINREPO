using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models._2AM
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string itcrequestdatamodel_selectwhere = "SELECT Id, ITCDate, ITCData FROM [2am].[capitec].[ITCRequest] WHERE";
        public const string itcrequestdatamodel_selectbykey = "SELECT Id, ITCDate, ITCData FROM [2am].[capitec].[ITCRequest] WHERE Id = @PrimaryKey";
        public const string itcrequestdatamodel_delete = "DELETE FROM [2am].[capitec].[ITCRequest] WHERE Id = @PrimaryKey";
        public const string itcrequestdatamodel_deletewhere = "DELETE FROM [2am].[capitec].[ITCRequest] WHERE";
        public const string itcrequestdatamodel_insert = "INSERT INTO [2am].[capitec].[ITCRequest] (Id, ITCDate, ITCData) VALUES(@Id, @ITCDate, @ITCData); ";
        public const string itcrequestdatamodel_update = "UPDATE [2am].[capitec].[ITCRequest] SET Id = @Id, ITCDate = @ITCDate, ITCData = @ITCData WHERE Id = @Id";



    }
}