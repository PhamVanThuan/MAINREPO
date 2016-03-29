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
	/// SAHL.Common.BusinessModel.DAO.OriginationSourceIcon_DAO
	/// </summary>
	public partial class OriginationSourceIcon : BusinessModelBase<SAHL.Common.BusinessModel.DAO.OriginationSourceIcon_DAO>, IOriginationSourceIcon
	{
				public OriginationSourceIcon(SAHL.Common.BusinessModel.DAO.OriginationSourceIcon_DAO OriginationSourceIcon) : base(OriginationSourceIcon)
		{
			this._DAO = OriginationSourceIcon;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceIcon_DAO.OriginationSourceKey
		/// </summary>
		public Int32 OriginationSourceKey 
		{
			get { return _DAO.OriginationSourceKey; }
			set { _DAO.OriginationSourceKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceIcon_DAO.Icon
		/// </summary>
		public String Icon 
		{
			get { return _DAO.Icon; }
			set { _DAO.Icon = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceIcon_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


