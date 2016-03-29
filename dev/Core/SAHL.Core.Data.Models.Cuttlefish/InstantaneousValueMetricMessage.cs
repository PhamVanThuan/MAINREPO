using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class InstantaneousValueMetricMessageDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public InstantaneousValueMetricMessageDataModel(int? instantaneousValue, string source, string userName, DateTime? messageDate, string machineName, string application)
        {
            this.InstantaneousValue = instantaneousValue;
            this.Source = source;
            this.UserName = userName;
            this.MessageDate = messageDate;
            this.MachineName = machineName;
            this.Application = application;
		
        }

        public InstantaneousValueMetricMessageDataModel(int id, int? instantaneousValue, string source, string userName, DateTime? messageDate, string machineName, string application)
        {
            this.Id = id;
            this.InstantaneousValue = instantaneousValue;
            this.Source = source;
            this.UserName = userName;
            this.MessageDate = messageDate;
            this.MachineName = machineName;
            this.Application = application;
		
        }		

        public int Id { get; set; }

        public int? InstantaneousValue { get; set; }

        public string Source { get; set; }

        public string UserName { get; set; }

        public DateTime? MessageDate { get; set; }

        public string MachineName { get; set; }

        public string Application { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}