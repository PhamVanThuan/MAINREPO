using SAHL.Core.Attributes;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree.Statements
{
    [NolockConventionExclude]
    public class GetTreeJsonQuery : ISqlStatement<DecisionTreeVersionDataModel>
    {
        public string Name { get; protected set; }

        public int Version { get; protected set; }

        public GetTreeJsonQuery(string name, int version)
        {
            this.Name = name;
            this.Version = version;
        }

        public string GetStatement()
        {
            return @"SELECT dtv.[Id],dtv.[DecisionTreeId],dtv.[Version],dtv.[Data]
                      FROM [DecisionTree].[dbo].[DecisionTreeVersion] dtv (NOLOCK)
                      JOIN [DecisionTree].[dbo].[DecisionTree] dt (NOLOCK) ON dt.Id = dtv.DecisionTreeId
                    WHERE dt.Name = @Name AND dtv.[Version]= @Version";
        }
    }
}