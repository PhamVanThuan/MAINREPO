using SAHL.Core.Attributes;

namespace SAHL.Core.Data.Models.DecisionTree.Statements
{
    [NolockConventionExclude]
    public class IsVariableSetVersionPublishedQuery : ISqlStatement<PublishedVariableSetDataModel>
    {
        public int Version { get; protected set; }

        public IsVariableSetVersionPublishedQuery(int version)
        {
            this.Version = version;
        }

        public string GetStatement()
        {
            return @"SELECT pvs.[Id],pvs.[VariableSetId],pvs.[PublishStatusId],pvs.[PublishDate],pvs.[Publisher]
                    FROM [DecisionTree].[dbo].[PublishedVariableSet] pvs (NOLOCK)
                    JOIN [DecisionTree].[dbo].[VariableSet] vs (NOLOCK) ON vs.Id = pvs.VariableSetId
                    WHERE vs.Version = @Version";
        }
    }
}