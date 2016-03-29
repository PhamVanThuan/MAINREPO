using Newtonsoft.Json;
using SAHL.Core.SystemMessages;

namespace SAHL.Core.Web.Services
{
    public class ServiceQueryResult : ServiceCommandResult
    {
        public ServiceQueryResult()
            : base()
        {
        }

        public ServiceQueryResult(ISystemMessageCollection systemMessages)
            : base(systemMessages)
        {
        }

        [JsonConstructor]
        public ServiceQueryResult(ISystemMessageCollection systemMessages, object returnData)
            : base(systemMessages)
        {
            this.ReturnData = returnData;
        }

        public object ReturnData { get; set; }

        public void SetReturnData(object value)
        {
            if (value != null)
            {
                this.ReturnData = value;
            }
        }
    }
}