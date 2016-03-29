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
	/// SAHL.Common.BusinessModel.DAO.ClientAnswerRank_DAO
	/// </summary>
	public partial interface IClientAnswerRank : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ClientAnswerRank_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ClientAnswerRank_DAO.ClientAnswer
		/// </summary>
		IClientAnswer ClientAnswer
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ClientAnswerRank_DAO.Sequence
		/// </summary>
		System.Int32 Sequence
		{
			get;
			set;
		}
	}
}


