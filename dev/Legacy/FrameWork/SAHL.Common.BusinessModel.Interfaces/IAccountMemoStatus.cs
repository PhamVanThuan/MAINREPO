using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.AccountMemoStatus_DAO
	/// </summary>
	public partial interface IAccountMemoStatus : IEntityValidation
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemoStatus_DAO.Description
		/// </summary>
		System.String Description
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemoStatus_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
	}
}


