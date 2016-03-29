using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SAHL.Web.Services.Internal.DataModel
{
	[DataContract]
	public class ReportParameter
	{
		[DataMember]
		public int ParameterTypeKey { get; set; }

		[DataMember]
		public string ReportParameterName { get; set; }

		[DataMember]
		public List<SAHL.Web.Services.Internal.DataModel.ReportParameter> ReportParams
		{
			get;
			set;
		}
		
	}
}