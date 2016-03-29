using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Web.Public.Models
{
    /// <summary>
    /// Business Event History View Model
    /// </summary>
    public class ViewBusinessEventHistoryViewModel
    {
        public int ReportKey { get; set; }
        public string ReportName { get; set; }
        [Required, Display(Name = "Please select a From Date")]
        public DateTime FromDate { get; set; }
        [Required, Display(Name = "Please select a To Date")]
        public DateTime ToDate { get; set; }
    }
}