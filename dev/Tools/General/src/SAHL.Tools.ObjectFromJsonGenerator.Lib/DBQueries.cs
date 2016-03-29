using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.ObjectFromJsonGenerator.Lib
{
    public class DBQueries
    {
        public const string All = @"SELECT es.Data,es.[Version],CASE WHEN pes.Id IS NULL THEN 0 ELSE 1 END as IsPublished,pes.PublishDate FROM [DecisionTree].[dbo].[{0}] es (NOLOCK)
LEFT OUTER JOIN [DecisionTree].[dbo].Published{0} pes (NOLOCK) ON es.Id = pes.{0}Id
ORDER BY Version ASC";

        public const string SpecificVersion = @"SELECT es.Data,es.[Version],CASE WHEN pes.Id IS NULL THEN 0 ELSE 1 END as IsPublished,pes.PublishDate FROM [DecisionTree].[dbo].[{0}] es (NOLOCK)
LEFT OUTER JOIN [DecisionTree].[dbo].Published{0} pes (NOLOCK) ON es.Id = pes.{0}Id
WHERE es.[Version] = @ver
ORDER BY Version ASC";

        public const string LatestVersion = @"SELECT top 1 es.Data,es.[Version],CASE WHEN pes.Id IS NULL THEN 0 ELSE 1 END as IsPublished,pes.PublishDate 
FROM [DecisionTree].[dbo].[{0}] es (NOLOCK)
LEFT OUTER JOIN [DecisionTree].[dbo].Published{0} pes (NOLOCK) ON es.Id = pes.{0}Id
ORDER BY Version DESC";

        public const string AllPublished = @"SELECT es.Data,es.[Version], 1 as IsPublished,pes.PublishDate FROM [DecisionTree].[dbo].[{0}] es (NOLOCK)
INNER  JOIN [DecisionTree].[dbo].Published{0} pes (NOLOCK) ON es.Id = pes.{0}Id
ORDER BY Version ASC";

        public const string SpecificPublishedVersion = @"SELECT es.Data,es.[Version], 1 as IsPublished,pes.PublishDate FROM [DecisionTree].[dbo].[{0}] es (NOLOCK)
INNER  JOIN [DecisionTree].[dbo].Published{0} pes (NOLOCK) ON es.Id = pes.{0}Id
WHERE es.[Version] = @ver
ORDER BY Version ASC";
    }
}
