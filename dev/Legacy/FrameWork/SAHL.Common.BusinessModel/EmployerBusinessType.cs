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
	/// EmployerBusinessType_DAO is used to hold the different business types which can be applied to an Employer.
	/// </summary>
	public partial class EmployerBusinessType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.EmployerBusinessType_DAO>, IEmployerBusinessType
	{
				public EmployerBusinessType(SAHL.Common.BusinessModel.DAO.EmployerBusinessType_DAO EmployerBusinessType) : base(EmployerBusinessType)
		{
			this._DAO = EmployerBusinessType;
		}
		/// <summary>
		/// The description of the Employer Business type. e.g. Company/Sole Proprietor
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


