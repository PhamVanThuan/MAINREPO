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
	/// SAHL.Common.BusinessModel.DAO.HeaderIconDetails_DAO
	/// </summary>
	public partial class HeaderIconDetails : BusinessModelBase<SAHL.Common.BusinessModel.DAO.HeaderIconDetails_DAO>, IHeaderIconDetails
	{
				public HeaderIconDetails(SAHL.Common.BusinessModel.DAO.HeaderIconDetails_DAO HeaderIconDetails) : base(HeaderIconDetails)
		{
			this._DAO = HeaderIconDetails;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HeaderIconDetails_DAO.GenericKey
		/// </summary>
		public Int32 GenericKey 
		{
			get { return _DAO.GenericKey; }
			set { _DAO.GenericKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HeaderIconDetails_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HeaderIconDetails_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HeaderIconDetails_DAO.GenericKeyTypeKey
		/// </summary>
		public Int32 GenericKeyTypeKey 
		{
			get { return _DAO.GenericKeyTypeKey; }
			set { _DAO.GenericKeyTypeKey = value;}
		}
	}
}


