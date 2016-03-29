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
	/// SAHL.Common.BusinessModel.DAO.ErrorRepository_DAO
	/// </summary>
	public partial class ErrorRepository : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ErrorRepository_DAO>, IErrorRepository
	{
				public ErrorRepository(SAHL.Common.BusinessModel.DAO.ErrorRepository_DAO ErrorRepository) : base(ErrorRepository)
		{
			this._DAO = ErrorRepository;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ErrorRepository_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ErrorRepository_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ErrorRepository_DAO.Active
		/// </summary>
		public Boolean Active 
		{
			get { return _DAO.Active; }
			set { _DAO.Active = value;}
		}
	}
}


