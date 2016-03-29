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
	/// SAHL.Common.BusinessModel.DAO.STOR_DAO
	/// </summary>
	public partial class STOR : BusinessModelBase<SAHL.Common.BusinessModel.DAO.STOR_DAO>, ISTOR
	{
				public STOR(SAHL.Common.BusinessModel.DAO.STOR_DAO STOR) : base(STOR)
		{
			this._DAO = STOR;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Name
		/// </summary>
		public String Name 
		{
			get { return _DAO.Name; }
			set { _DAO.Name = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Folder
		/// </summary>
		public String Folder 
		{
			get { return _DAO.Folder; }
			set { _DAO.Folder = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.BFulltext
		/// </summary>
		public Int32 BFulltext 
		{
			get { return _DAO.BFulltext; }
			set { _DAO.BFulltext = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.NonIndexableChars
		/// </summary>
		public String NonIndexableChars 
		{
			get { return _DAO.NonIndexableChars; }
			set { _DAO.NonIndexableChars = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key1
		/// </summary>
		public String Key1 
		{
			get { return _DAO.Key1; }
			set { _DAO.Key1 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key2
		/// </summary>
		public String Key2 
		{
			get { return _DAO.Key2; }
			set { _DAO.Key2 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key3
		/// </summary>
		public String Key3 
		{
			get { return _DAO.Key3; }
			set { _DAO.Key3 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key4
		/// </summary>
		public String Key4 
		{
			get { return _DAO.Key4; }
			set { _DAO.Key4 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key5
		/// </summary>
		public String Key5 
		{
			get { return _DAO.Key5; }
			set { _DAO.Key5 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key6
		/// </summary>
		public String Key6 
		{
			get { return _DAO.Key6; }
			set { _DAO.Key6 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key7
		/// </summary>
		public String Key7 
		{
			get { return _DAO.Key7; }
			set { _DAO.Key7 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key8
		/// </summary>
		public String Key8 
		{
			get { return _DAO.Key8; }
			set { _DAO.Key8 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key1Options
		/// </summary>
		public String Key1Options 
		{
			get { return _DAO.Key1Options; }
			set { _DAO.Key1Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key2Options
		/// </summary>
		public String Key2Options 
		{
			get { return _DAO.Key2Options; }
			set { _DAO.Key2Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key3Options
		/// </summary>
		public String Key3Options 
		{
			get { return _DAO.Key3Options; }
			set { _DAO.Key3Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key4Options
		/// </summary>
		public String Key4Options 
		{
			get { return _DAO.Key4Options; }
			set { _DAO.Key4Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key5Options
		/// </summary>
		public String Key5Options 
		{
			get { return _DAO.Key5Options; }
			set { _DAO.Key5Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key6Options
		/// </summary>
		public String Key6Options 
		{
			get { return _DAO.Key6Options; }
			set { _DAO.Key6Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key7Options
		/// </summary>
		public String Key7Options 
		{
			get { return _DAO.Key7Options; }
			set { _DAO.Key7Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key8Options
		/// </summary>
		public String Key8Options 
		{
			get { return _DAO.Key8Options; }
			set { _DAO.Key8Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key1MinMax
		/// </summary>
		public String Key1MinMax 
		{
			get { return _DAO.Key1MinMax; }
			set { _DAO.Key1MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key2MinMax
		/// </summary>
		public String Key2MinMax 
		{
			get { return _DAO.Key2MinMax; }
			set { _DAO.Key2MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key3MinMax
		/// </summary>
		public String Key3MinMax 
		{
			get { return _DAO.Key3MinMax; }
			set { _DAO.Key3MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key4MinMax
		/// </summary>
		public String Key4MinMax 
		{
			get { return _DAO.Key4MinMax; }
			set { _DAO.Key4MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key5MinMax
		/// </summary>
		public String Key5MinMax 
		{
			get { return _DAO.Key5MinMax; }
			set { _DAO.Key5MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key6MinMax
		/// </summary>
		public String Key6MinMax 
		{
			get { return _DAO.Key6MinMax; }
			set { _DAO.Key6MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key7MinMax
		/// </summary>
		public String Key7MinMax 
		{
			get { return _DAO.Key7MinMax; }
			set { _DAO.Key7MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key8MinMax
		/// </summary>
		public String Key8MinMax 
		{
			get { return _DAO.Key8MinMax; }
			set { _DAO.Key8MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.BAudit
		/// </summary>
		public Int32 BAudit 
		{
			get { return _DAO.BAudit; }
			set { _DAO.BAudit = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.LogFolder
		/// </summary>
		public String LogFolder 
		{
			get { return _DAO.LogFolder; }
			set { _DAO.LogFolder = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.DefaultDocTitle
		/// </summary>
		public String DefaultDocTitle 
		{
			get { return _DAO.DefaultDocTitle; }
			set { _DAO.DefaultDocTitle = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key9
		/// </summary>
		public String Key9 
		{
			get { return _DAO.Key9; }
			set { _DAO.Key9 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key10
		/// </summary>
		public String Key10 
		{
			get { return _DAO.Key10; }
			set { _DAO.Key10 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key11
		/// </summary>
		public String Key11 
		{
			get { return _DAO.Key11; }
			set { _DAO.Key11 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key12
		/// </summary>
		public String Key12 
		{
			get { return _DAO.Key12; }
			set { _DAO.Key12 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key13
		/// </summary>
		public String Key13 
		{
			get { return _DAO.Key13; }
			set { _DAO.Key13 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key14
		/// </summary>
		public String Key14 
		{
			get { return _DAO.Key14; }
			set { _DAO.Key14 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key15
		/// </summary>
		public String Key15 
		{
			get { return _DAO.Key15; }
			set { _DAO.Key15 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key16
		/// </summary>
		public String Key16 
		{
			get { return _DAO.Key16; }
			set { _DAO.Key16 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key9Options
		/// </summary>
		public String Key9Options 
		{
			get { return _DAO.Key9Options; }
			set { _DAO.Key9Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key10Options
		/// </summary>
		public String Key10Options 
		{
			get { return _DAO.Key10Options; }
			set { _DAO.Key10Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key11Options
		/// </summary>
		public String Key11Options 
		{
			get { return _DAO.Key11Options; }
			set { _DAO.Key11Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key12Options
		/// </summary>
		public String Key12Options 
		{
			get { return _DAO.Key12Options; }
			set { _DAO.Key12Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key13Options
		/// </summary>
		public String Key13Options 
		{
			get { return _DAO.Key13Options; }
			set { _DAO.Key13Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key14Options
		/// </summary>
		public String Key14Options 
		{
			get { return _DAO.Key14Options; }
			set { _DAO.Key14Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key15Options
		/// </summary>
		public String Key15Options 
		{
			get { return _DAO.Key15Options; }
			set { _DAO.Key15Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key16Options
		/// </summary>
		public String Key16Options 
		{
			get { return _DAO.Key16Options; }
			set { _DAO.Key16Options = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key9MinMax
		/// </summary>
		public String Key9MinMax 
		{
			get { return _DAO.Key9MinMax; }
			set { _DAO.Key9MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key10MinMax
		/// </summary>
		public String Key10MinMax 
		{
			get { return _DAO.Key10MinMax; }
			set { _DAO.Key10MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key11MinMax
		/// </summary>
		public String Key11MinMax 
		{
			get { return _DAO.Key11MinMax; }
			set { _DAO.Key11MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key12MinMax
		/// </summary>
		public String Key12MinMax 
		{
			get { return _DAO.Key12MinMax; }
			set { _DAO.Key12MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key13MinMax
		/// </summary>
		public String Key13MinMax 
		{
			get { return _DAO.Key13MinMax; }
			set { _DAO.Key13MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key14MinMax
		/// </summary>
		public String Key14MinMax 
		{
			get { return _DAO.Key14MinMax; }
			set { _DAO.Key14MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key15MinMax
		/// </summary>
		public String Key15MinMax 
		{
			get { return _DAO.Key15MinMax; }
			set { _DAO.Key15MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key16MinMax
		/// </summary>
		public String Key16MinMax 
		{
			get { return _DAO.Key16MinMax; }
			set { _DAO.Key16MinMax = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Exclusions
		/// </summary>
		public String Exclusions 
		{
			get { return _DAO.Exclusions; }
			set { _DAO.Exclusions = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.STOR_DAO.Key
		/// </summary>
		public Decimal Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


