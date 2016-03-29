
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;

using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ApplicationRoleTypeGroup_DAO
	/// </summary>
    public partial class ApplicationRoleTypeGroup_WTF : BusinessModelBase<ApplicationRoleTypeGroup_WTF_DAO>, IApplicationRoleTypeGroup
	{
        public ApplicationRoleTypeGroup_WTF(ApplicationRoleTypeGroup_WTF_DAO ApplicationRoleTypeGroup_WTF) : base(ApplicationRoleTypeGroup_WTF)
		{
            this._DAO = ApplicationRoleTypeGroup_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRoleTypeGroup_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRoleTypeGroup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}



