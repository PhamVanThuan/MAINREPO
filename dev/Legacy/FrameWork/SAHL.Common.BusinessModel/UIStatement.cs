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
	/// SAHL.Common.BusinessModel.DAO.UIStatement_DAO
	/// </summary>
	public partial class UIStatement : BusinessModelBase<SAHL.Common.BusinessModel.DAO.UIStatement_DAO>, IUIStatement
	{
				public UIStatement(SAHL.Common.BusinessModel.DAO.UIStatement_DAO UIStatement) : base(UIStatement)
		{
			this._DAO = UIStatement;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.ApplicationName
		/// </summary>
		public String ApplicationName 
		{
			get { return _DAO.ApplicationName; }
			set { _DAO.ApplicationName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.StatementName
		/// </summary>
		public String StatementName 
		{
			get { return _DAO.StatementName; }
			set { _DAO.StatementName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.ModifyDate
		/// </summary>
		public DateTime ModifyDate 
		{
			get { return _DAO.ModifyDate; }
			set { _DAO.ModifyDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.Version
		/// </summary>
		public Int32 Version 
		{
			get { return _DAO.Version; }
			set { _DAO.Version = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.ModifyUser
		/// </summary>
		public String ModifyUser 
		{
			get { return _DAO.ModifyUser; }
			set { _DAO.ModifyUser = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.Statement
		/// </summary>
		public String Statement 
		{
			get { return _DAO.Statement; }
			set { _DAO.Statement = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.uiStatementType
		/// </summary>
		public IUIStatementType uiStatementType 
		{
			get
			{
				if (null == _DAO.uiStatementType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IUIStatementType, UIStatementType_DAO>(_DAO.uiStatementType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.uiStatementType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.uiStatementType = (UIStatementType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.LastAccessedDate
		/// </summary>
		public DateTime LastAccessedDate 
		{
			get { return _DAO.LastAccessedDate; }
			set { _DAO.LastAccessedDate = value;}
		}
	}
}


