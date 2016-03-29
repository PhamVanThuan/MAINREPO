using System.Collections.Generic;

namespace Automation.Framework
{
    public class Script
    {
        public string Name { get; set; }

        public string Setup { get; set; }

        public string Complete { get; set; }

        public List<Step> Steps { get; set; }
    }
}