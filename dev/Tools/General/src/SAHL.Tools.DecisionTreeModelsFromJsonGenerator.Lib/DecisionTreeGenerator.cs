using Dapper;
using Newtonsoft.Json.Linq;
using SAHL.Tools.DecisionTreeModelsFromJsonGenerator.Lib.Templates;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace SAHL.Tools.DecisionTreeModelsFromJsonGenerator.Lib
{
    public class MaxVersionMap
    {
        public int MaxEnumerationVersion { get; set; }

        public int MaxMessageVersion { get; set; }

        public int MaxVariableVersion { get; set; }
    }

    public static class MaxVersion
    {
        public static MaxVersionMap Mappings
        {
            get;
            set;
        }
    }

    public class DecisionTreeGenerator
    {
        private class VersionedTreeMap
        {
            public Guid DecisionTreeId { get; set; }

            public string Name { get; set; }

            public string Version { get; set; }

            public string Data { get; set; }

            public string Description { get; set; }

            public bool IsLatestVersion { get; set; }
        };

        protected string decisionTreeName;

        protected Guid decisionTreeId;

        protected string decisionTreeVersion;

        protected JArray treeVariablesJson;

        protected int[] treeInputVariablesIds;

        protected string defaultTreeNamespace;

        protected string defaultTreeQueryNamespace;

        protected string defaultTreeQueryResultNamespace;

        private string defaultTreeVariablesNamespace;

        public DecisionTreeGenerator()
        {
        }

        public void GenerateTreeObject(string connectionString, string treeOutputPath, string treeQueryOutputPath, string decisionTreeName, string decisionTreeVersion, string buildMode = "Debug")
        {
            this.decisionTreeName = decisionTreeName;
            //this.decisionTreeVersion = decisionTreeVersion;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                MaxVersion.Mappings = GetMaxGlobalVersions(connection, buildMode);

                string query = "";
                IEnumerable<VersionedTreeMap> msgRaw = null;

                string requiredVersion = decisionTreeVersion;

                if (requiredVersion == "*")
                {
                    //get latest
                    query = @"SELECT TOP 1 dt.[Id] TreeID
                                ,[Name]
	                            ,dtv.[Version]
	                            ,dtv.[Data]
                                ,[Description]
                            FROM
	                            [DecisionTree].[dbo].[DecisionTree] dt
                            INNER JOIN
	                            [DecisionTree].[dbo].[DecisionTreeVersion] dtv
                            ON
	                            dt.id = dtv.DecisionTreeId
                            WHERE
	                            dt.[Name] = @treeName
                            --AND
                            --    dt.[IsActive] = 1
                            ORDER BY
                                dtv.[Version] desc";

                    msgRaw = connection.Query<VersionedTreeMap>(query, new { treeName = decisionTreeName });
                }
                else
                {
                    query = @"SELECT dt.[Id] TreeID
                                ,[Name]
	                            ,dtv.[Version]
	                            ,dtv.[Data]
                                ,[Description]
                            FROM
	                            [DecisionTree].[dbo].[DecisionTree] dt
                            INNER JOIN
	                            [DecisionTree].[dbo].[DecisionTreeVersion] dtv
                            ON
	                            dt.id = dtv.DecisionTreeId
                            WHERE
	                            dt.[Name] = @treeName
                            AND
                                dtv.[Version] = @treeVersion
                            --AND
                            --    dt.[IsActive] = 1";

                    msgRaw = connection.Query<VersionedTreeMap>(query, new { treeName = decisionTreeName, treeVersion = Int32.Parse(requiredVersion) });
                }

                if (msgRaw.First() != null)
                {
                    this.decisionTreeName = msgRaw.First().Name;
                    this.decisionTreeVersion = msgRaw.First().Version.ToString();

                    GenerateTreeAndQueryClasses(treeOutputPath, treeQueryOutputPath, msgRaw.First().Data.ToString(), msgRaw.First().Version, false);
                }
            }
        }

        internal MaxVersionMap GetMaxGlobalVersions(IDbConnection connection, string buildMode)
        {
            string query = string.Empty;
            if (buildMode.Equals("Debug", StringComparison.InvariantCultureIgnoreCase))
            {
                query = @"DECLARE @debugMode bit
SET @debugMode = 1
";
            }
            else if (buildMode.Equals("Release", StringComparison.InvariantCultureIgnoreCase))
            {
                query = @"DECLARE @debugMode bit
SET @debugMode = 0
";
            }

            query += @"
SELECT
	MaxEnumerationVersion =
		(SELECT
			CASE WHEN @debugMode = 0 THEN
				(SELECT
					MAX([Version])
				FROM
					[DecisionTree].[dbo].EnumerationSet es
				INNER JOIN
					[DecisionTree].[dbo].PublishedEnumerationSet pes
				ON
					es.Id = pes.EnumerationSetId)
			ELSE
				(SELECT MAX([Version]) FROM [DecisionTree].[dbo].EnumerationSet)
			END),
	MaxMessageVersion =
		(SELECT
			CASE WHEN @debugMode = 0 THEN
				(SELECT
					MAX(ms.[Version])
				FROM
					[DecisionTree].[dbo].MessageSet ms
				INNER JOIN
					[DecisionTree].[dbo].PublishedMessageSet pms
				ON
					ms.Id = pms.MessageSetId)
			ELSE
				(SELECT MAX([Version]) FROM [DecisionTree].[dbo].MessageSet)
			END),
	MaxVariableVersion =
			(SELECT
			(CASE WHEN @debugMode = 0 THEN
				(SELECT
					MAX(vs.[Version])
				FROM
					[DecisionTree].[dbo].VariableSet vs
				INNER JOIN
					[DecisionTree].[dbo].PublishedVariableSet pvs
				ON
					vs.Id = pvs.VariableSetId)
			ELSE
				(SELECT MAX([Version]) FROM [DecisionTree].[dbo].VariableSet)
			END)
			)";

            MaxVersionMap mapping = null;
            var results = connection.Query<MaxVersionMap>(query);
            if (results.Count() == 1)
            {
                mapping = results.First();
            }
            else
            {
                mapping = new MaxVersionMap() { MaxEnumerationVersion = -1, MaxMessageVersion = -1, MaxVariableVersion = -1 };
            }
            return mapping;
        }

        public void GenerateTreeObjects(string connectionString, string treeOutputPath, string treeQueryOutputPath, string buildMode = "Debug",string packagesJsonFile = "")
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "";
                IEnumerable<VersionedTreeMap> msgRaw = null;

                //get latest of every named tree in db
                if (buildMode.Equals("Debug", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = @";WITH latestTrees AS
(
	SELECT dt.[Id],dt.[Name],MAX(dtv.[Version]) as [Version],MAX(dtv.Id) as latestVersionId
	FROM [DecisionTree].[dbo].[DecisionTreeVersion] dtv (NOLOCK)
	JOIN [DecisionTree].[dbo].[DecisionTree] dt (NOLOCK) ON dtv.DecisionTreeId = dt.Id
	GROUP BY dt.[Id],dt.[Name]
)
SELECT DISTINCT dt.[Id] TreeID
        ,dt.[Name]
	    ,dtv.[Version]
	    ,dtv.[Data]
        ,dt.[Description]
		,CASE WHEN lt.latestVersionId IS NOT NULL THEN 1 ELSE 0 END as IsLatestVersion
FROM [DecisionTree].[dbo].[DecisionTreeVersion] dtv (NOLOCK)
JOIN [DecisionTree].[dbo].[DecisionTree] dt (NOLOCK) ON dtv.DecisionTreeId = dt.Id
LEFT JOIN latestTrees lt ON lt.Id = dt.Id AND dtv.Id = lt.latestVersionId
WHERE Data IS NOT NULL
ORDER BY dt.Name ASC";
                }
                else if (buildMode.Equals("Release", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = @";WITH latestTrees AS
(
	SELECT dt.[Id],dt.[Name],MAX(dtv.[Version]) as [Version],MAX(dtv.Id) as latestVersionId
	FROM [DecisionTree].[dbo].[DecisionTreeVersion] dtv (NOLOCK)
	JOIN [DecisionTree].[dbo].[DecisionTree] dt (NOLOCK) ON dtv.DecisionTreeId = dt.Id
    INNER  JOIN [DecisionTree].[dbo].PublishedDecisionTree pdt (NOLOCK) ON dtv.Id = pdt.DecisionTreeVersionId
	GROUP BY dt.[Id],dt.[Name]
)
SELECT DISTINCT dt.[Id] TreeID
        ,dt.[Name]
	    ,dtv.[Version]
	    ,dtv.[Data]
        ,dt.[Description]
		,CASE WHEN lt.latestVersionId IS NOT NULL THEN 1 ELSE 0 END as IsLatestVersion
FROM [DecisionTree].[dbo].[DecisionTreeVersion] dtv (NOLOCK)
JOIN [DecisionTree].[dbo].[DecisionTree] dt (NOLOCK) ON dtv.DecisionTreeId = dt.Id
INNER  JOIN [DecisionTree].[dbo].PublishedDecisionTree pdt (NOLOCK) ON dtv.Id = pdt.DecisionTreeVersionId
LEFT JOIN latestTrees lt ON lt.Id = dt.Id AND dtv.Id = lt.latestVersionId
WHERE Data IS NOT NULL
ORDER BY dt.Name ASC";
                }

                msgRaw = connection.Query<VersionedTreeMap>(query);
                MaxVersion.Mappings = GetMaxGlobalVersions(connection, buildMode);

                if (msgRaw.Count() > 0)
                {
                    PackageHelper packageHelper = new PackageHelper();
                    IEnumerable<string> inclusionList = packageHelper.GetPackageManifest(packagesJsonFile);
                    msgRaw = inclusionList!=null ? msgRaw.Where(x=> inclusionList.Contains(x.Name)) : msgRaw;
                    foreach (VersionedTreeMap treeMap in msgRaw)
                    {
                        this.decisionTreeName = Utilities.StripBadChars(treeMap.Name);
                        this.decisionTreeVersion = treeMap.Version.ToString();
                        this.decisionTreeId = treeMap.DecisionTreeId;

                        GenerateTreeAndQueryClasses(treeOutputPath, treeQueryOutputPath, treeMap.Data.ToString(), treeMap.Version, false);
                    }

                    foreach (VersionedTreeMap treeMap in msgRaw)
                    {
                        this.decisionTreeName = Utilities.StripBadChars(treeMap.Name);
                        this.decisionTreeVersion = treeMap.Version.ToString();
                        this.decisionTreeId = treeMap.DecisionTreeId;
                        GenerateTreeAndQueryClasses(treeOutputPath, treeQueryOutputPath, treeMap.Data.ToString(), treeMap.Version, true);
                    }
                }
            }
        }

        private void SetDefaultNamespaces(string treeOutputPath, string treeQueryOutputPath)
        {
            this.defaultTreeVariablesNamespace = treeOutputPath.Split(new char[] { '\\' }).Last();
            this.defaultTreeNamespace = string.Format("{0}.Trees", treeOutputPath.Split(new char[] { '\\' }).Last());
            var enclosingNamespace = treeQueryOutputPath.Split(new char[] { '\\' }).Last();
            this.defaultTreeQueryNamespace = string.Format("{0}.Queries", enclosingNamespace);
            this.defaultTreeQueryResultNamespace = string.Format("{0}.Models", enclosingNamespace);
        }

        private void GenerateTreeAndQueryClasses(string treeOutputPath, string treeQueryOutputPath, string rawJson, string treeVersion, bool isLatestVersion)
        {
            SetDefaultNamespaces(treeOutputPath, treeQueryOutputPath);

            var decisionTreeObject = new DecisionTreeObject(rawJson, defaultTreeNamespace, treeVersion);

            if (decisionTreeObject != null)
            {
                if (!isLatestVersion && !string.IsNullOrWhiteSpace(treeOutputPath))
                    GenerateAndSaveTreeClass(treeOutputPath, decisionTreeObject);

                if (decisionTreeObject.VariablesJson != null && decisionTreeObject.VariablesJson.Count() > 0)
                {
                    this.treeVariablesJson = decisionTreeObject.VariablesJson;
                    if (!isLatestVersion && !string.IsNullOrWhiteSpace(treeOutputPath))
                        GenerateAndSaveTreeVariablesClass(treeOutputPath, defaultTreeVariablesNamespace);

                    if (!string.IsNullOrWhiteSpace(treeQueryOutputPath))
                    {
                        GenerateAndSaveTreeQueryClass(treeQueryOutputPath, defaultTreeQueryNamespace, isLatestVersion);
                        GenerateAndSaveTreeQueryResultClass(treeQueryOutputPath, defaultTreeQueryResultNamespace, isLatestVersion);
                    }
                }
            }
        }

        private void GenerateAndSaveTreeClass(string treeOutputPath, DecisionTreeObject decisionTreeObject)
        {
            string generatedData = decisionTreeObject.TransformText();
            using (StreamWriter sw = new StreamWriter(treeOutputPath + "\\Trees\\" + decisionTreeObject.DecisionTreeClassName + ".cs"))
            {
                sw.Write(generatedData);
                sw.Flush();
            }
        }

        private void GenerateAndSaveTreeVariablesClass(string treeOutputPath, string defaultNamespace)
        {
            var decisionTreeSpecificVariables = new DecisionTreeSpecificVariables(treeVariablesJson, defaultNamespace, decisionTreeName, Convert.ToInt32(this.decisionTreeVersion));
            if (decisionTreeSpecificVariables != null)
            {
                string generatedTreeVariablesData = decisionTreeSpecificVariables.TransformText();
                using (StreamWriter sw = new StreamWriter(treeOutputPath + "\\Trees\\" + decisionTreeSpecificVariables.DecisionTreeVariablesClassName + ".cs"))
                {
                    sw.Write(generatedTreeVariablesData);
                    sw.Flush();
                }
            }
        }

        private void GenerateAndSaveTreeQueryClass(string treeQueryOutputPath, string defaultNamespace, bool isLatestVersion)
        {
            var decisionTreeQueryObject = new DecisionTreeQueryObject(treeVariablesJson, defaultNamespace, decisionTreeName, Convert.ToInt32(this.decisionTreeVersion), isLatestVersion);
            if (decisionTreeQueryObject != null)
            {
                string generatedQueryData = decisionTreeQueryObject.TransformText();
                using (StreamWriter sw = new StreamWriter(treeQueryOutputPath + "\\Queries\\" + decisionTreeQueryObject.DecisionTreeQueryClassName + ".cs"))
                {
                    sw.Write(generatedQueryData);
                    sw.Flush();
                }
            }
        }

        private void GenerateAndSaveTreeQueryResultClass(string treeQueryOutputPath, string defaultNamespace, bool isLatestVersion)
        {
            var decisionTreeQueryResultObject = new DecisionTreeQueryResultObject(treeVariablesJson, defaultNamespace, decisionTreeName, Convert.ToInt32(this.decisionTreeVersion), isLatestVersion);
            if (decisionTreeQueryResultObject != null)
            {
                string generatedQueryResultData = decisionTreeQueryResultObject.TransformText();
                using (StreamWriter sw = new StreamWriter(treeQueryOutputPath + "\\Models\\" + decisionTreeQueryResultObject.DecisionTreeQueryResultClassName + ".cs"))
                {
                    sw.Write(generatedQueryResultData);
                    sw.Flush();
                }
            }
        }
    }
}