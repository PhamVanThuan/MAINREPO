using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using SAHL.Common.Attributes;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.Service
{
    /// <summary>
    ///
    /// </summary>
    [FactoryType(typeof(ISqlAgentService))]
    public class SqlAgentService : ISqlAgentService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public bool StartSQLServerAgentJob(string jobName)
        {
            IDbConnection con = SAHL.Common.DataAccess.Helper.GetSQLDBConnection("BatchConnectionString");
            ServerConnection ServerConnection = new ServerConnection((SqlConnection)con);
            bool retval = false;

            if (ServerConnection != null)
            {
                Server Server = new Server(ServerConnection);

                if (Server != null)
                {
                    Job Job = Server.JobServer.Jobs[jobName];

                    if (Job != null)
                    {
                        Job.Start();
                        retval = true;
                    }
                }
            }

            return retval;
        }
    }
}