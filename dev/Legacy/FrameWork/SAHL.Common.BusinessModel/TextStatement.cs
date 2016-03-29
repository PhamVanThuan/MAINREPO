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
	/// SAHL.Common.BusinessModel.DAO.TextStatement_DAO
	/// </summary>
	public partial class TextStatement : BusinessModelBase<SAHL.Common.BusinessModel.DAO.TextStatement_DAO>, ITextStatement
	{
				public TextStatement(SAHL.Common.BusinessModel.DAO.TextStatement_DAO TextStatement) : base(TextStatement)
		{
			this._DAO = TextStatement;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TextStatement_DAO.StatementTitle
		/// </summary>
		public String StatementTitle 
		{
			get { return _DAO.StatementTitle; }
			set { _DAO.StatementTitle = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TextStatement_DAO.Statement
		/// </summary>
		public String Statement 
		{
			get { return _DAO.Statement; }
			set { _DAO.Statement = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TextStatement_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TextStatement_DAO.TextStatementType
		/// </summary>
		public ITextStatementType TextStatementType 
		{
			get
			{
				if (null == _DAO.TextStatementType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITextStatementType, TextStatementType_DAO>(_DAO.TextStatementType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.TextStatementType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.TextStatementType = (TextStatementType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


