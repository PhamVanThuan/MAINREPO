using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SAHL.Web.ClientSecure.Models
{
    /// <summary>
    /// Logon View Model
    /// </summary>
	public class LoginViewModel
	{
        [Required]
        [Display(Name = "E-mail address")]
        [EmailAttribute(ErrorMessage = "Please enter a valid Email Address")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

        [Display(Name="Remember Me")]
        public bool RememberMe { get; set; }
	}
}