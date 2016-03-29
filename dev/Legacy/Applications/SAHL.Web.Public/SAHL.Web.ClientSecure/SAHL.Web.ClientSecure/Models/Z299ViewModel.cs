using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAHL.Web.ClientSecure.Models
{
    public class Z299ViewModel
    {
        public int ReportKey { get; set; }
        public string ReportName { get; set; }
        [Required, Display(Name = "Account")]
        public Int32 AccountKey { get; set; }
        public IDictionary<int, string> ReportFormats { get; set; }
        [Required, Display(Name = "ReportFormatKey")]
        public int ReportFormatKey { get; set; }
    }
}