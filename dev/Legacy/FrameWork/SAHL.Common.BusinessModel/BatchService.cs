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
	/// SAHL.Common.BusinessModel.DAO.BatchService_DAO
	/// </summary>
	public partial class BatchService : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BatchService_DAO>, IBatchService
	{
				public BatchService(SAHL.Common.BusinessModel.DAO.BatchService_DAO BatchService) : base(BatchService)
		{
			this._DAO = BatchService;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.BatchService_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.BatchService_DAO.RequestedBy
		/// </summary>
		public String RequestedBy 
		{
			get { return _DAO.RequestedBy; }
			set { _DAO.RequestedBy = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.BatchService_DAO.RequestedDate
		/// </summary>
		public DateTime RequestedDate 
		{
			get { return _DAO.RequestedDate; }
			set { _DAO.RequestedDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.BatchService_DAO.BatchCount
		/// </summary>
		public Int32 BatchCount 
		{
			get { return _DAO.BatchCount; }
			set { _DAO.BatchCount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.BatchService_DAO.BatchServiceTypeKey
		/// </summary>
		public Int32 BatchServiceTypeKey 
		{
			get { return _DAO.BatchServiceTypeKey; }
			set { _DAO.BatchServiceTypeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.BatchService_DAO.FileName
		/// </summary>
		public String FileName 
		{
			get { return _DAO.FileName; }
			set { _DAO.FileName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.BatchService_DAO.FileContent
		/// </summary>
		public Byte[] FileContent 
		{
			get { return _DAO.FileContent; }
			set { _DAO.FileContent = value;}
		}
	}
}


