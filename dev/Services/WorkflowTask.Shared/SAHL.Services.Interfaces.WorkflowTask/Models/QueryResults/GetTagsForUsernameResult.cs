using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.WorkflowTask
{
    public class GetTagsForUsernameResult
    {
        public Guid Id { get; set; }
        public string ForeColour { get; set; }
        public string BackColour { get; set; }
        public string Caption { get; set; }
    }
}