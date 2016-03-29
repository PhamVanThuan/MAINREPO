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
	/// SAHL.Common.BusinessModel.DAO.Detail_DAO
	/// </summary>
	public partial class Detail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Detail_DAO>, IDetail
	{
				public Detail(SAHL.Common.BusinessModel.DAO.Detail_DAO Detail) : base(Detail)
		{
			this._DAO = Detail;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Detail_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


