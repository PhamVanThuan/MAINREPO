
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO
	/// </summary>
    public partial class UiStatement_WTF : BusinessModelBase<UiStatement_WTF_DAO>, IUiStatement_WTF
	{
        public UiStatement_WTF(UiStatement_WTF_DAO UiStatement_WTF) : base(UiStatement_WTF)
		{
            this._DAO = UiStatement_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.ApplicationName
		/// </summary>
		public String ApplicationName 
		{
			get { return _DAO.ApplicationName; }
			set { _DAO.ApplicationName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.StatementName
		/// </summary>
		public String StatementName 
		{
			get { return _DAO.StatementName; }
			set { _DAO.StatementName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.ModifyDate
		/// </summary>
		public DateTime? ModifyDate 
		{
			get { return _DAO.ModifyDate; }
			set { _DAO.ModifyDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.Version
		/// </summary>
		public Int32 Version 
		{
			get { return _DAO.Version; }
			set { _DAO.Version = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.ModifyUser
		/// </summary>
		public String ModifyUser 
		{
			get { return _DAO.ModifyUser; }
			set { _DAO.ModifyUser = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.Statement
		/// </summary>
		public String Statement 
		{
			get { return _DAO.Statement; }
			set { _DAO.Statement = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.Type
		/// </summary>
		public Int32 Type 
		{
			get { return _DAO.Type; }
			set { _DAO.Type = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.LastAccessedDate
		/// </summary>
		public DateTime? LastAccessedDate 
		{
			get { return _DAO.LastAccessedDate; }
			set { _DAO.LastAccessedDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


