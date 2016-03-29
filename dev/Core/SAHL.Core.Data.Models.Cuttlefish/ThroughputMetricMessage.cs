using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class ThroughputMetricMessageDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public ThroughputMetricMessageDataModel(string source, string userName, DateTime? messageDate, string machineName, string application, string metric)
        {
            this.Source = source;
            this.UserName = userName;
            this.MessageDate = messageDate;
            this.MachineName = machineName;
            this.Application = application;
            this.Metric = metric;
		
        }

        public ThroughputMetricMessageDataModel(int id, string source, string userName, DateTime? messageDate, string machineName, string application, string metric)
        {
            this.Id = id;
            this.Source = source;
            this.UserName = userName;
            this.MessageDate = messageDate;
            this.MachineName = machineName;
            this.Application = application;
            this.Metric = metric;
		
        }		

        public int Id { get; set; }

        public string Source { get; set; }

        public string UserName { get; set; }

        public DateTime? MessageDate { get; set; }

        public string MachineName { get; set; }

        public string Application { get; set; }

        public string Metric { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}