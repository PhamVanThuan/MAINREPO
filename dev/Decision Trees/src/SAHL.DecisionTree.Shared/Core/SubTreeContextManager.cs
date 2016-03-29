using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Reflection;
using SAHL.DecisionTree.Shared.Interfaces;
using SAHL.DecisionTree.Shared.Helpers;

namespace SAHL.DecisionTree.Shared.Core
{
    public class SubTreeContextManager : ISubTreeContextManager
    {

        private dynamic GlobalVersions { get; set; }
        private Dictionary<string, ISystemMessageCollection> SubtreeMessagesDictionary { get; set; }

        public SubTreeContextManager(dynamic GlobalVersions, Dictionary<string, ISystemMessageCollection> SubtreeMessagesDictionary)
        {
            this.GlobalVersions = GlobalVersions;
            this.SubtreeMessagesDictionary = SubtreeMessagesDictionary;
        }

        public TreeProcessingContext GetSubTreeDebugContext(string treeName, int treeVersion)
        {
            TreeProcessingContext subTreeContext = null;
            var query = @"SELECT dt.[Id] TreeID
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
                                dtv.[Version] = @treeVersion";

            string connectionString = ConfigurationManager.ConnectionStrings["DBCONNECTION_ServiceArchitect"].ConnectionString;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                IEnumerable<VersionedTreeMap> queryResult = null;
                queryResult = connection.Query<VersionedTreeMap>(query, new { treeName = treeName, treeVersion = treeVersion });
                if (queryResult.First() != null)
                {
                    ISystemMessageCollection subtreeMessageCollection = new SystemMessageCollection();
                    SubtreeMessagesDictionary.Add(string.Format("{0}_{1}", Utilities.StripInvalidChars(treeName), treeVersion), subtreeMessageCollection);
                    subTreeContext = new TreeProcessingContext(queryResult.First().Data.ToString(), this.GlobalVersions, subtreeMessageCollection);                    
                }
            }
            return subTreeContext;
        }

        public TreeProcessingContext GetSubtreeExecutionContext(string subtreeName, int subtreeVersion, string dbConnectionString)
        {
            ISystemMessageCollection systemMessageCollection = new SystemMessageCollection();

            var formattedSubtreeName = Utilities.StripInvalidChars(subtreeName);
            formattedSubtreeName = Utilities.CapitaliseFirstLetter(formattedSubtreeName);
            var subtreeExecutionManager = new TreeProcessingContext(formattedSubtreeName, subtreeVersion.ToString(), this.GlobalVersions, systemMessageCollection, true, dbConnectionString);

            return subtreeExecutionManager;
        }

        private class VersionedTreeMap
        {
            public Guid DecisionTreeId { get; set; }

            public string Name { get; set; }

            public int Version { get; set; }

            public string Data { get; set; }

            public string Description { get; set; }

            public bool IsActive { get; set; }
        };
    }
}
