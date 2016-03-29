using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Web.Public.Models
{
	public class PasswordViewModel
	{
		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }
	}
}