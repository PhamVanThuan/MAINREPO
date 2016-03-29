namespace SAHL.Common.Service.Interfaces
{
    public interface ISqlAgentService
    {
        /// <summary>
        /// Starts a sql agent job given the job name
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        bool StartSQLServerAgentJob(string jobName);
    }
}