using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Interfaces.DataTransferObjects
{
    public class BatchServiceResult
    {
        public int BatchServiceKey { get; set; }

        public string FileName { get; set; }

        public DateTime RequestedDate { get; set; }

        public string RequestedBy { get; set; }

        public int BatchCount { get; set; }

        public int Complete { get; set; }

        public int Pending { get; set; }

        public int Failed { get; set; }

        public int Unsuccessful { get; set; }
    }
}
