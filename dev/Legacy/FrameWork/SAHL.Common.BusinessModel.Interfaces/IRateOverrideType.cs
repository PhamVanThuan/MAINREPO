using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// RateOverrideType_DAO is used in order to hold the descriptions of the types of Rate Overrides.
	/// </summary>
	public partial interface IRateOverrideType : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// The description of the Rate Override. e.g. Interest Only/Super Lo/
		/// </summary>
		System.String Description
		{
			get;
			set;
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateOverrideType_DAO.RateOverrideTypeGroup
		/// </summary>
		IRateOverrideTypeGroup RateOverrideTypeGroup
		{
			get;
			set;
		}
	}
}


