using SAHL.Core.Services;
using SAHL.Core.X2;

namespace SAHL.WorkflowMaps.Specs.Common
{
    public class ParamsDataStub : IX2Params
    {
        public string ADUserName { get; set; }

        public string ActivityName { get; set; }

        public object Data { get; set; }

        public bool IgnoreWarning { get; set; }

        public string StateName { get; set; }

        public string WorkflowName { get; set; }

        public IServiceRequestMetadata ServiceRequestMetadata { get; set; }
    }
}