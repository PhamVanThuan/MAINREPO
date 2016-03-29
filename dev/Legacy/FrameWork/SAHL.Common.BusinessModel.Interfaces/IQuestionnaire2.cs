using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
	public partial interface IQuestionnaire
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IReadOnlyEventList<IQuestion> Questions
		{
			get;
		}
	}
}
