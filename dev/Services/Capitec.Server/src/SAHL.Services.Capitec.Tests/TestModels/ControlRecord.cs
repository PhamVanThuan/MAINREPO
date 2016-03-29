using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Tests.TestModels
{
    public class ControlRecord
    {
        public int ControlNumber { get; set; }
        public string ControlDescription { get; set; }
        public int ControlNumeric { get; set; }
        public string ControlText { get; set; }
        public int ControlGroupKey { get; set; }
    }
}
