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
	/// RoleType_DAO contains the different types of Roles that a Legal Entity can play on an Account at SAHL. This would include:
		/// Main ApplicantSuretorPrevious InsurerAssured Life
	/// </summary>
	public partial class RoleType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RoleType_DAO>, IRoleType
	{
				public RoleType(SAHL.Common.BusinessModel.DAO.RoleType_DAO RoleType) : base(RoleType)
		{
			this._DAO = RoleType;
		}
		/// <summary>
		/// The description of the Role Type
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


