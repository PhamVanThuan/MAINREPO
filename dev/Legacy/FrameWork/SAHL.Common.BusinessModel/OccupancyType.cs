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
	/// SAHL.Common.BusinessModel.DAO.OccupancyType_DAO
	/// </summary>
	public partial class OccupancyType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.OccupancyType_DAO>, IOccupancyType
	{
				public OccupancyType(SAHL.Common.BusinessModel.DAO.OccupancyType_DAO OccupancyType) : base(OccupancyType)
		{
			this._DAO = OccupancyType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OccupancyType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OccupancyType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


