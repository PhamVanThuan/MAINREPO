using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Web.ClientSecure.Models
{

    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base(@"^([\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+\.)*[\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$") 
        {
        }
    }

    public class PasswordViewModel
    {
        [Required]
        [Display(Name = "E-mail address")]
        [EmailAttribute(ErrorMessage = "Please enter a valid Email Address")]
        public string UserName { get; set; }
    }
}