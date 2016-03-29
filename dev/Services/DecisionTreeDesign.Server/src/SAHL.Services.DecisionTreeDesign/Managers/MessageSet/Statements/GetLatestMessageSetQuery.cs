using SAHL.Core.Attributes;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;

namespace SAHL.Services.DecisionTreeDesign.Managers.MessageSet.Statements
{
    [NolockConventionExclude]
    public class GetLatestMessageSetQuery : ISqlStatement<MessageSetDataModel>
    {
        public string GetStatement()
        {
            return @"SELECT TOP 1 es.[Id],es.[Version],es.[Data]
                    FROM [DecisionTree].[dbo].[MessageSet] es (NOLOCK)
                    ORDER BY [Version] DESC";
        }
    }
}