using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAHL.Web.ClientSecure.Models
{
    public class RequestFundsViewModel
    {
        [Required(ErrorMessage = "Account is required")]
        public int AccountKey { get; set; }

        [Required(ErrorMessage = "Please enter an amount you would like to request")]
        public decimal Amount { get; set; }
    }
}