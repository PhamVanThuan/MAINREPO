using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class WatchListConfigurationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WatchListConfigurationDataModel(string processName, string workFlowName, string statementName)
        {
            this.ProcessName = processName;
            this.WorkFlowName = workFlowName;
            this.StatementName = statementName;
		
        }
		[JsonConstructor]
        public WatchListConfigurationDataModel(int watchListConfigurationKey, string processName, string workFlowName, string statementName)
        {
            this.WatchListConfigurationKey = watchListConfigurationKey;
            this.ProcessName = processName;
            this.WorkFlowName = workFlowName;
            this.StatementName = statementName;
		
        }		

        public int WatchListConfigurationKey { get; set; }

        public string ProcessName { get; set; }

        public string WorkFlowName { get; set; }

        public string StatementName { get; set; }

        public void SetKey(int key)
        {
            this.WatchListConfigurationKey =  key;
        }
    }
}