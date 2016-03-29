using SAHL.Core.Attributes;

namespace SAHL.Core.Data.Models.DecisionTree.Statements
{
    [NolockConventionExclude]
    public class GetLatestEnumerationSetQuery : ISqlStatement<EnumerationSetDataModel>
    {
        public string GetStatement()
        {
            return @"SELECT TOP 1 es.[Id],es.[Version],es.[Data]
                    FROM [DecisionTree].[dbo].[EnumerationSet] es (NOLOCK)
                    ORDER BY [Version] DESC";
        }
    }
}