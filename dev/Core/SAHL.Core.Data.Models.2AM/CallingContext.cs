using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CallingContextDataModel :  IDataModel
    {
        public CallingContextDataModel(int callingContextKey, int callingContextTypeKey, string callingProcess, string callingMethod, string callingState)
        {
            this.CallingContextKey = callingContextKey;
            this.CallingContextTypeKey = callingContextTypeKey;
            this.CallingProcess = callingProcess;
            this.CallingMethod = callingMethod;
            this.CallingState = callingState;
		
        }		

        public int CallingContextKey { get; set; }

        public int CallingContextTypeKey { get; set; }

        public string CallingProcess { get; set; }

        public string CallingMethod { get; set; }

        public string CallingState { get; set; }
    }
}