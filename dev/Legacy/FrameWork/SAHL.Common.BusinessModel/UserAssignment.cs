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
	/// SAHL.Common.BusinessModel.DAO.UserAssignment_DAO
	/// </summary>
	public partial class UserAssignment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.UserAssignment_DAO>, IUserAssignment
	{
				public UserAssignment(SAHL.Common.BusinessModel.DAO.UserAssignment_DAO UserAssignment) : base(UserAssignment)
		{
			this._DAO = UserAssignment;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserAssignment_DAO.FinancialServiceKey
		/// </summary>
		public Int32 FinancialServiceKey 
		{
			get { return _DAO.FinancialServiceKey; }
			set { _DAO.FinancialServiceKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserAssignment_DAO.OriginationSourceProductKey
		/// </summary>
		public Int32 OriginationSourceProductKey 
		{
			get { return _DAO.OriginationSourceProductKey; }
			set { _DAO.OriginationSourceProductKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserAssignment_DAO.AssignmentDate
		/// </summary>
		public DateTime AssignmentDate 
		{
			get { return _DAO.AssignmentDate; }
			set { _DAO.AssignmentDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserAssignment_DAO.AssigningUser
		/// </summary>
		public String AssigningUser 
		{
			get { return _DAO.AssigningUser; }
			set { _DAO.AssigningUser = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserAssignment_DAO.AssignedUser
		/// </summary>
		public String AssignedUser 
		{
			get { return _DAO.AssignedUser; }
			set { _DAO.AssignedUser = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserAssignment_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


