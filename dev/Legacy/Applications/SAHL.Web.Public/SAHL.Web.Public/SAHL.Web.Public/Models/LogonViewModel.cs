using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SAHL.Web.Public.Models
{
    /// <summary>
    /// Logon View Model
    /// </summary>
	public class LoginViewModel
	{
		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

        [Display(Name="Remember Me")]
        public bool RememberMe { get; set; }
	}
}