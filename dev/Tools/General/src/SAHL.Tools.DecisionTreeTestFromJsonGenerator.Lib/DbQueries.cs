using System;

namespace SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib
{
    public class DbQueries
    {
        public string GetDecisionTreeQuery(string exceptionList, bool isPublished)
        {
            if (isPublished)
            {
                return String.Format(publishedTreeQuery, exceptionList);
            }
            else
            {
                return String.Format(unpublishedTreeQuery, exceptionList);
            }
        }

        public static string GetTreeNames()
        {
            return "SELECT DISTINCT [Name] FROM [DecisionTree].[dbo].[DecisionTree]";
        }
        
        private string unpublishedTreeQuery = @"WITH latestTrees AS
                                (
                                    SELECT dt.[Id],dt.[Name],MAX(dtv.[Version]) as [Version],MAX(dtv.Id) as latestVersionId
                                    FROM [DecisionTree].[dbo].[DecisionTreeVersion] dtv (NOLOCK)
                                    JOIN [DecisionTree].[dbo].[DecisionTree] dt (NOLOCK) ON dtv.DecisionTreeId = dt.Id
                                    GROUP BY dt.[Id],dt.[Name]
                                )
                                SELECT DISTINCT 
                                        dt.[Name]
                                        ,dtv.[Version]
                                        ,dtv.[Data] AS Json
                                        ,CASE WHEN lt.latestVersionId IS NOT NULL THEN 1 ELSE 0 END as IsLatestVersion
                                FROM [DecisionTree].[dbo].[DecisionTreeVersion] dtv (NOLOCK)
                                JOIN [DecisionTree].[dbo].[DecisionTree] dt (NOLOCK) ON dtv.DecisionTreeId = dt.Id
                                LEFT JOIN latestTrees lt ON lt.Id = dt.Id AND dtv.Id = lt.latestVersionId
                                WHERE Data IS NOT NULL
                                AND dt.[Name] NOT IN (''{0})
                                ORDER BY dt.Name ASC";
        private string publishedTreeQuery = @"WITH latestTrees AS
                                (
                                    SELECT dt.[Id], dt.[Name], MAX(dtv.[Version]) as [Version], MAX(dtv.Id) as latestVersionId
                                    FROM[DecisionTree].[dbo].[DecisionTreeVersion] dtv(NOLOCK)
                                    JOIN[DecisionTree].[dbo].[DecisionTree] dt(NOLOCK) ON dtv.DecisionTreeId = dt.Id
                                    INNER JOIN[DecisionTree].[dbo].PublishedDecisionTree pdt(NOLOCK) ON dtv.Id = pdt.DecisionTreeVersionId
                                    GROUP BY dt.[Id], dt.[Name]
                                )
                                SELECT DISTINCT
                                        dt.[Name]
                                        ,dtv.[Version]
                                        ,dtv.[Data] AS Json
                                        ,CASE WHEN lt.latestVersionId IS NOT NULL THEN 1 ELSE 0 END as IsLatestVersion
                                FROM[DecisionTree].[dbo].[DecisionTreeVersion] dtv(NOLOCK)
                                JOIN[DecisionTree].[dbo].[DecisionTree] dt(NOLOCK) ON dtv.DecisionTreeId = dt.Id
                                INNER  JOIN[DecisionTree].[dbo].PublishedDecisionTree pdt (NOLOCK) ON dtv.Id = pdt.DecisionTreeVersionId
                                LEFT JOIN latestTrees lt ON lt.Id = dt.Id AND dtv.Id = lt.latestVersionId
                                                                WHERE Data IS NOT NULL
                                AND dt.[Name] NOT IN (''{0})
                                ORDER BY dt.Name ASC";

    }
}
