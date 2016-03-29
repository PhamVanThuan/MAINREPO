using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Web.Public.Models
{
    /// <summary>
    /// Search View Model
    /// </summary>
    public class SearchViewModel
    {
        [Display(Name="ID Number")]
        public string IDNumber { get; set; }

        [Display(Name = "Client Name")]
        public string LegalEntityName { get; set; }

        [Display(Name="Account Number")]
        [Range(0, int.MaxValue)]
        public int? CaseNumber { get; set; }

        public List<SearchResultViewModel> SearchResults { get; set; }
    }
}