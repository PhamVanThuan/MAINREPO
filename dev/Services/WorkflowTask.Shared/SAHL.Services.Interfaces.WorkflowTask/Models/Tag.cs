using System.Collections.Generic;
﻿using System;

namespace SAHL.Services.Interfaces.WorkflowTask
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Caption { get; set; }
        public Dictionary<string, string> Style { get; set; }
    }
}