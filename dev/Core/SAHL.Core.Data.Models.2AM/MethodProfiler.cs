using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MethodProfilerDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public MethodProfilerDataModel(DateTime? callingTime, string methodName, string className, double? totalSeconds, string innerMethods, int? threadID, string callingApp)
        {
            this.CallingTime = callingTime;
            this.MethodName = methodName;
            this.ClassName = className;
            this.TotalSeconds = totalSeconds;
            this.InnerMethods = innerMethods;
            this.ThreadID = threadID;
            this.CallingApp = callingApp;
		
        }
		[JsonConstructor]
        public MethodProfilerDataModel(long id, DateTime? callingTime, string methodName, string className, double? totalSeconds, string innerMethods, int? threadID, string callingApp)
        {
            this.id = id;
            this.CallingTime = callingTime;
            this.MethodName = methodName;
            this.ClassName = className;
            this.TotalSeconds = totalSeconds;
            this.InnerMethods = innerMethods;
            this.ThreadID = threadID;
            this.CallingApp = callingApp;
		
        }		

        public long id { get; set; }

        public DateTime? CallingTime { get; set; }

        public string MethodName { get; set; }

        public string ClassName { get; set; }

        public double? TotalSeconds { get; set; }

        public string InnerMethods { get; set; }

        public int? ThreadID { get; set; }

        public string CallingApp { get; set; }

        public void SetKey(long key)
        {
            this.id =  key;
        }
    }
}