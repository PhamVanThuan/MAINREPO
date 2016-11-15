using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ResidenceStatus_DAO
	/// </summary>
	public partial class ResidenceStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ResidenceStatus_DAO>, IResidenceStatus
	{
				public ResidenceStatus(SAHL.Common.BusinessModel.DAO.ResidenceStatus_DAO ResidenceStatus) : base(ResidenceStatus)
		{
			this._DAO = ResidenceStatus;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ResidenceStatus_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ResidenceStatus_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}

