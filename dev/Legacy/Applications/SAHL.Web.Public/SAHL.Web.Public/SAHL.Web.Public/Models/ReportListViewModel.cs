using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Public.Models
{
    public class ReportListViewModel
    {
        public ReportListViewModel()
        {
            Reports = new List<ReportViewModel>();
        }
        public List<ReportViewModel> Reports { get; set; }
    }
}