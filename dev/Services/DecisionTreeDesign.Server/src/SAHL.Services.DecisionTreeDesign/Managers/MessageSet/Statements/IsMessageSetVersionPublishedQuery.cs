using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;

namespace SAHL.Services.DecisionTreeDesign.Managers.MessageSet.Statements
{
    public class IsMessageSetVersionPublishedQuery : ISqlStatement<PublishedMessageSetDataModel>
    {
        public int Version { get; protected set; }

        public IsMessageSetVersionPublishedQuery(int version)
        {
            this.Version = version;
        }

        public string GetStatement()
        {
            return @"SELECT pms.Id, pms.MessageSetId, pms.PublishStatusId, pms.PublishDate, pms.Publisher
FROM [DecisionTree].[dbo].[MessageSet] ms
JOIN [DecisionTree].[dbo].[PublishedMessageSet] pms ON pms.MessageSetId = ms.Id
WHERE ms.[Version] = @Version";
        }
    }
}