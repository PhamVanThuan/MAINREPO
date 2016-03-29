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
	/// SAHL.Common.BusinessModel.DAO.ImportFile_DAO
	/// </summary>
	public partial class ImportFile : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ImportFile_DAO>, IImportFile
	{
				public ImportFile(SAHL.Common.BusinessModel.DAO.ImportFile_DAO ImportFile) : base(ImportFile)
		{
			this._DAO = ImportFile;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportFile_DAO.FileName
		/// </summary>
		public String FileName 
		{
			get { return _DAO.FileName; }
			set { _DAO.FileName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportFile_DAO.FileType
		/// </summary>
		public String FileType 
		{
			get { return _DAO.FileType; }
			set { _DAO.FileType = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportFile_DAO.DateImported
		/// </summary>
		public DateTime DateImported 
		{
			get { return _DAO.DateImported; }
			set { _DAO.DateImported = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportFile_DAO.Status
		/// </summary>
		public String Status 
		{
			get { return _DAO.Status; }
			set { _DAO.Status = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportFile_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportFile_DAO.XmlData
		/// </summary>
		public String XmlData 
		{
			get { return _DAO.XmlData; }
			set { _DAO.XmlData = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportFile_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


