using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Interfaces.DataTransferObjects
{
	public class ApplicationAttributeToApply
	{
		public int ApplicationAttributeTypeKey { get; set; }
        public int ApplicationAttributeTypeGroupKey { get; set; }
		public bool ToBeRemoved { get; set; }
	}
}
