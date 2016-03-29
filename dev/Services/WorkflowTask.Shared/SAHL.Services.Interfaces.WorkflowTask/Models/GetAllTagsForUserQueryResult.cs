using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.WorkflowTask
{
    public class GetAllTagsForUserQueryResult
    {
        public Dictionary<Guid, Tag> Tags { get; set; }
    }
}