using SAHL.Core.Attributes;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;

namespace SAHL.Services.DecisionTreeDesign.Managers.Variable.Statements
{
    [NolockConventionExclude]
    public class GetLatestVariableSetQuery : ISqlStatement<VariableSetDataModel>
    {
        public string GetStatement()
        {
            return @"SELECT TOP 1 es.[Id],es.[Version],es.[Data]
                    FROM [DecisionTree].[dbo].[VariableSet] es (NOLOCK)
                    ORDER BY [Version] DESC";
        }
    }
}