using SAHL.Core.Attributes;

namespace SAHL.Core.Data.Models.DecisionTree.Statements
{
    [NolockConventionExclude]
    public class IsEnumerationSetVersionPublishedQuery : ISqlStatement<PublishedEnumerationSetDataModel>
    {
        public int Version { get; protected set; }

        public IsEnumerationSetVersionPublishedQuery(int version)
        {
            this.Version = version;
        }

        public string GetStatement()
        {
            return @"SELECT pes.[Id],pes.[EnumerationSetId],pes.[PublishStatusId],pes.[PublishDate],pes.[Publisher]
                    FROM [DecisionTree].[dbo].[PublishedEnumerationSet] pes (NOLOCK)
                    JOIN [DecisionTree].[dbo].[EnumerationSet] es (NOLOCK) ON es.Id = pes.EnumerationSetId
                    WHERE es.Version = @Version";
        }
    }
}