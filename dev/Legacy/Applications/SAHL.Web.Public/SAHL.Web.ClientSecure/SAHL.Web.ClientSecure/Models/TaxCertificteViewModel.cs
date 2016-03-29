using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace SAHL.Web.ClientSecure.Models
{
    public class TaxCertificteViewModel
    {
        public int ReportKey { get; set; }
        public string ReportName { get; set; }

        [Required, Display(Name = "Year"), Range(1990, 2030)]
        public int Year { get; set; }

        [Required, Display(Name = "Account")]
        public Int32 AccountKey { get; set; }

        public IDictionary<int, string> ReportFormats { get; set; }
        [Required, Display(Name = "ReportFormatKey")]
        public int ReportFormatKey { get; set; }
    }
}