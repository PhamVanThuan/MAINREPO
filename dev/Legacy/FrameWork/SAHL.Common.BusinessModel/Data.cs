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
	/// SAHL.Common.BusinessModel.DAO.Data_DAO
	/// </summary>
	public partial class Data : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Data_DAO>, IData
	{
				public Data(SAHL.Common.BusinessModel.DAO.Data_DAO Data) : base(Data)
		{
			this._DAO = Data;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.SecurityGroup
		/// </summary>
		public String SecurityGroup 
		{
			get { return _DAO.SecurityGroup; }
			set { _DAO.SecurityGroup = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.ArchiveDate
		/// </summary>
		public String ArchiveDate 
		{
			get { return _DAO.ArchiveDate; }
			set { _DAO.ArchiveDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.DataContainer
		/// </summary>
		public Decimal DataContainer 
		{
			get { return _DAO.DataContainer; }
			set { _DAO.DataContainer = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.BackupVolume
		/// </summary>
		public Decimal BackupVolume 
		{
			get { return _DAO.BackupVolume; }
			set { _DAO.BackupVolume = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Overlay
		/// </summary>
		public String Overlay 
		{
			get { return _DAO.Overlay; }
			set { _DAO.Overlay = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.STOR
		/// </summary>
		public Decimal STOR 
		{
			get { return _DAO.STOR; }
			set { _DAO.STOR = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.GUID
		/// </summary>
		public String GUID 
		{
			get { return _DAO.GUID; }
			set { _DAO.GUID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Extension
		/// </summary>
		public String Extension 
		{
			get { return _DAO.Extension; }
			set { _DAO.Extension = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key1
		/// </summary>
		public String Key1 
		{
			get { return _DAO.Key1; }
			set { _DAO.Key1 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key2
		/// </summary>
		public String Key2 
		{
			get { return _DAO.Key2; }
			set { _DAO.Key2 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key3
		/// </summary>
		public String Key3 
		{
			get { return _DAO.Key3; }
			set { _DAO.Key3 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key4
		/// </summary>
		public String Key4 
		{
			get { return _DAO.Key4; }
			set { _DAO.Key4 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key5
		/// </summary>
		public String Key5 
		{
			get { return _DAO.Key5; }
			set { _DAO.Key5 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key6
		/// </summary>
		public String Key6 
		{
			get { return _DAO.Key6; }
			set { _DAO.Key6 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key7
		/// </summary>
		public String Key7 
		{
			get { return _DAO.Key7; }
			set { _DAO.Key7 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key8
		/// </summary>
		public String Key8 
		{
			get { return _DAO.Key8; }
			set { _DAO.Key8 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.MsgTo
		/// </summary>
		public String MsgTo 
		{
			get { return _DAO.MsgTo; }
			set { _DAO.MsgTo = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.MsgFrom
		/// </summary>
		public String MsgFrom 
		{
			get { return _DAO.MsgFrom; }
			set { _DAO.MsgFrom = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.MsgSubject
		/// </summary>
		public String MsgSubject 
		{
			get { return _DAO.MsgSubject; }
			set { _DAO.MsgSubject = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.MsgReceived
		/// </summary>
		public DateTime? MsgReceived
		{
			get { return _DAO.MsgReceived; }
			set { _DAO.MsgReceived = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.MsgSent
		/// </summary>
		public DateTime? MsgSent
		{
			get { return _DAO.MsgSent; }
			set { _DAO.MsgSent = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key9
		/// </summary>
		public String Key9 
		{
			get { return _DAO.Key9; }
			set { _DAO.Key9 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key10
		/// </summary>
		public String Key10 
		{
			get { return _DAO.Key10; }
			set { _DAO.Key10 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key11
		/// </summary>
		public String Key11 
		{
			get { return _DAO.Key11; }
			set { _DAO.Key11 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key12
		/// </summary>
		public String Key12 
		{
			get { return _DAO.Key12; }
			set { _DAO.Key12 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key13
		/// </summary>
		public String Key13 
		{
			get { return _DAO.Key13; }
			set { _DAO.Key13 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key14
		/// </summary>
		public String Key14 
		{
			get { return _DAO.Key14; }
			set { _DAO.Key14 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key15
		/// </summary>
		public String Key15 
		{
			get { return _DAO.Key15; }
			set { _DAO.Key15 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key16
		/// </summary>
		public String Key16 
		{
			get { return _DAO.Key16; }
			set { _DAO.Key16 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Title
		/// </summary>
		public String Title 
		{
			get { return _DAO.Title; }
			set { _DAO.Title = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.OriginalFilename
		/// </summary>
		public String OriginalFilename 
		{
			get { return _DAO.OriginalFilename; }
			set { _DAO.OriginalFilename = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Data_DAO.Key
		/// </summary>
		public Decimal Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


