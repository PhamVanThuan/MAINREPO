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
	/// SAHL.Common.BusinessModel.DAO.PreContextFilter_DAO
	/// </summary>
	public partial class PreContextFilter : BusinessModelBase<SAHL.Common.BusinessModel.DAO.PreContextFilter_DAO>, IPreContextFilter
	{
				public PreContextFilter(SAHL.Common.BusinessModel.DAO.PreContextFilter_DAO PreContextFilter) : base(PreContextFilter)
		{
			this._DAO = PreContextFilter;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PreContextFilter_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PreContextFilter_DAO.ContextKey
		/// </summary>
		public Int32 ContextKey 
		{
			get { return _DAO.ContextKey; }
			set { _DAO.ContextKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PreContextFilter_DAO.OfferTypeKey
		/// </summary>
		public Int32 OfferTypeKey 
		{
			get { return _DAO.OfferTypeKey; }
			set { _DAO.OfferTypeKey = value;}
		}
	}
}


