using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Web.ClientSecure.Models
{
	public class ReportParamsViewModel
	{
	    public string ReportParameterName { get; set; }

		[Required]
		public string ReportParameterValue { get; set; }

		public int ParameterTypeKey { get; set; }
	}
}