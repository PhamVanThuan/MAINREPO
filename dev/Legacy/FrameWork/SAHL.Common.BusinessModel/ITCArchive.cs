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
	/// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO
	/// </summary>
	public partial class ITCArchive : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ITCArchive_DAO>, IITCArchive
	{
				public ITCArchive(SAHL.Common.BusinessModel.DAO.ITCArchive_DAO ITCArchive) : base(ITCArchive)
		{
			this._DAO = ITCArchive;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.LegalEntityKey
		/// </summary>
		public Int32 LegalEntityKey 
		{
			get { return _DAO.LegalEntityKey; }
			set { _DAO.LegalEntityKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.AccountKey
		/// </summary>
		public Int32? AccountKey 
		{
			get { return _DAO.AccountKey; }
			set { _DAO.AccountKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.ChangeDate
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.ResponseXML
		/// </summary>
		public String ResponseXML 
		{
			get { return _DAO.ResponseXML; }
			set { _DAO.ResponseXML = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.ResponseStatus
		/// </summary>
		public String ResponseStatus 
		{
			get { return _DAO.ResponseStatus; }
			set { _DAO.ResponseStatus = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.ArchiveUser
		/// </summary>
		public String ArchiveUser 
		{
			get { return _DAO.ArchiveUser; }
			set { _DAO.ArchiveUser = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.ArchiveDate
		/// </summary>
		public DateTime ArchiveDate 
		{
			get { return _DAO.ArchiveDate; }
			set { _DAO.ArchiveDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.RequestXML
		/// </summary>
		public String RequestXML 
		{
			get { return _DAO.RequestXML; }
			set { _DAO.RequestXML = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


