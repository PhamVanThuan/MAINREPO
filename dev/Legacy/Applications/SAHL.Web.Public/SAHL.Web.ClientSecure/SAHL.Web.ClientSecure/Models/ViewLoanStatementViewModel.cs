using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Web.ClientSecure.Models
{
    public class ViewLoanStatementViewModel
    {
        public int ReportKey { get; set; }
        public string ReportName { get; set; }
        [Required, Display(Name="From Date")]
        public DateTime FromDate { get; set; }
        [Required, Display(Name = "To Date")]
        public DateTime ToDate { get; set; }
        [Required, Display(Name = "Account")]
        public Int32 AccountKey { get; set; }

        public IDictionary<int, string> ReportFormats { get; set; }
        [Required, Display(Name = "ReportFormatKey")]
        public int ReportFormatKey { get; set; }

    }
}