using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Web.Public.Models
{
    /// <summary>
    /// Case Detail View Model
    /// </summary>
    public class CaseDetailViewModel
    {
        [Display(Name="Account Number")]
        public int AccountKey { get; set; }

        [Display(Name="Debt Counselling Case Number")]
        public int CaseNumber { get; set; }

        [Display(Name = "Case Description")]
        public string CaseDescription { get; set; }
    }
}