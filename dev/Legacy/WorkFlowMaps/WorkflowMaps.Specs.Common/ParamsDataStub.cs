using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Core.Services;

namespace WorkflowMaps.Specs.Common
{
    public class ParamsDataStub : SAHL.Core.X2.IX2Params
    {
        public string ADUserName
        {
            get;
            set;
        }

        public string ActivityName
        {
            get;
            set;
        }

        public object Data
        {
            get;
            set;
        }

        public bool IgnoreWarning
        {
            get;
            set;
        }

        public string StateName
        {
            get;
            set;
        }


        public string WorkflowName
        {
            get;
            set;
        }

        public IServiceRequestMetadata ServiceRequestMetadata
        {
            get;
            set;
        }
    }
}