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
	/// SAHL.Common.BusinessModel.DAO.Setup_DAO
	/// </summary>
	public partial class Setup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Setup_DAO>, ISetup
	{
				public Setup(SAHL.Common.BusinessModel.DAO.Setup_DAO Setup) : base(Setup)
		{
			this._DAO = Setup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Setup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Setup_DAO.Name
		/// </summary>
		public String Name 
		{
			get { return _DAO.Name; }
			set { _DAO.Name = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Setup_DAO.Query
		/// </summary>
		public String Query 
		{
			get { return _DAO.Query; }
			set { _DAO.Query = value;}
		}
	}
}


