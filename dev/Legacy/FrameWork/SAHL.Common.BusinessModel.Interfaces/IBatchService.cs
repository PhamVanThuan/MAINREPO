using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.BatchService_DAO
	/// </summary>
	public partial interface IBatchService : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.BatchService_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.BatchService_DAO.RequestedBy
		/// </summary>
		System.String RequestedBy
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.BatchService_DAO.RequestedDate
		/// </summary>
		System.DateTime RequestedDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.BatchService_DAO.BatchCount
		/// </summary>
		System.Int32 BatchCount
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.BatchService_DAO.BatchServiceTypeKey
		/// </summary>
		System.Int32 BatchServiceTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.BatchService_DAO.FileName
		/// </summary>
		System.String FileName
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.BatchService_DAO.FileContent
		/// </summary>
		System.Byte[] FileContent
		{
			get;
			set;
		}
	}
}


