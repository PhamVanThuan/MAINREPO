using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Web.Public.Models
{
    public class ProposalListViewModel
    {
        public ProposalListViewModel()
        {
            Proposals = new List<ProposalViewModel>();
        }

        [Display()]
        public List<ProposalViewModel> Proposals { get; set; }
    }
}