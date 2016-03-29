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
	/// SAHL.Common.BusinessModel.DAO.TitleDeedCheck_DAO
	/// </summary>
	public partial class TitleDeedCheck : BusinessModelBase<SAHL.Common.BusinessModel.DAO.TitleDeedCheck_DAO>, ITitleDeedCheck
	{
				public TitleDeedCheck(SAHL.Common.BusinessModel.DAO.TitleDeedCheck_DAO TitleDeedCheck) : base(TitleDeedCheck)
		{
			this._DAO = TitleDeedCheck;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TitleDeedCheck_DAO.Key
		/// </summary>
		public String Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TitleDeedCheck_DAO.TitleDeedIndicator
		/// </summary>
		public String TitleDeedIndicator 
		{
			get { return _DAO.TitleDeedIndicator; }
			set { _DAO.TitleDeedIndicator = value;}
		}
	}
}


