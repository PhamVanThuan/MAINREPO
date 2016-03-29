using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Web.Public.Models
{
    /// <summary>
    /// Search Result View Model
    /// </summary>
    public class SearchResultViewModel
    {
        [Display(Name="Account Number")]
        public int AccountKey { get; set; }

        [Display(Name = "Debt Counselling Case Number")]
        public int DebtCounsellingKey { get; set; }

        [Display(Name = "Case Description")]
        public string CaseDescription { get; set; }

        [Display(Name = "Legal Entities On Account")]
        public List<LegalEntityViewModel> LegalEntitiesOnAccount { get; set; }
    }
}