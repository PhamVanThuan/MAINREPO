﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.ClientSecure.Models
{
    public class DetailReportViewModel
    {
        public string ReportName { get; set; }
        public Dictionary<string, object> ReportParameters { get; set; }
    }
}