using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.ClientSecure.Models
{
	public class ReportViewModel
	{
        private string p;


        public ReportViewModel()
        {
        }

        public ReportViewModel(string p)
        {
            this.ReportName = p;
        }
		public string ReportName { get; set; }

		public List<ReportParamsViewModel> ReportParameters { get; set; }

	}
}