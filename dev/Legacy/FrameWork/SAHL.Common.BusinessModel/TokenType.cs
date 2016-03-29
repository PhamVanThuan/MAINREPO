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
	/// SAHL.Common.BusinessModel.DAO.TokenType_DAO
	/// </summary>
	public partial class TokenType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.TokenType_DAO>, ITokenType
	{
				public TokenType(SAHL.Common.BusinessModel.DAO.TokenType_DAO TokenType) : base(TokenType)
		{
			this._DAO = TokenType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TokenType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TokenType_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TokenType_DAO.RunStatement
		/// </summary>
		public Boolean RunStatement 
		{
			get { return _DAO.RunStatement; }
			set { _DAO.RunStatement = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TokenType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


