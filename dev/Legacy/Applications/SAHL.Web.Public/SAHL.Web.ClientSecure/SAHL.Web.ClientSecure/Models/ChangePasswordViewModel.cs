using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SAHL.Web.ClientSecure.Models
{
    public class ChangePasswordViewModel
    {
        [Required, Display(Name = "Password")]
        [StringLength(50, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        public string NewPassword { get; set; }

        [Required, Display(Name = "Confirm Password")]
        [StringLength(50, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        [System.Web.Mvc.Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}