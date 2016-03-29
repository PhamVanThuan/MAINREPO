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
	/// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO
	/// </summary>
	public partial class StatementDefinition : BusinessModelBase<SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO>, IStatementDefinition
	{
				public StatementDefinition(SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO StatementDefinition) : base(StatementDefinition)
		{
			this._DAO = StatementDefinition;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO.ApplicationName
		/// </summary>
		public String ApplicationName 
		{
			get { return _DAO.ApplicationName; }
			set { _DAO.ApplicationName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO.StatementName
		/// </summary>
		public String StatementName 
		{
			get { return _DAO.StatementName; }
			set { _DAO.StatementName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO.StatementParameters
		/// </summary>
		private DAOEventList<StatementParameter_DAO, IStatementParameter, StatementParameter> _StatementParameters;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO.StatementParameters
		/// </summary>
		public IEventList<IStatementParameter> StatementParameters
		{
			get
			{
				if (null == _StatementParameters) 
				{
					if(null == _DAO.StatementParameters)
						_DAO.StatementParameters = new List<StatementParameter_DAO>();
					_StatementParameters = new DAOEventList<StatementParameter_DAO, IStatementParameter, StatementParameter>(_DAO.StatementParameters);
					_StatementParameters.BeforeAdd += new EventListHandler(OnStatementParameters_BeforeAdd);					
					_StatementParameters.BeforeRemove += new EventListHandler(OnStatementParameters_BeforeRemove);					
					_StatementParameters.AfterAdd += new EventListHandler(OnStatementParameters_AfterAdd);					
					_StatementParameters.AfterRemove += new EventListHandler(OnStatementParameters_AfterRemove);					
				}
				return _StatementParameters;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO.Tokens
		/// </summary>
		private DAOEventList<Token_DAO, IToken, Token> _Tokens;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO.Tokens
		/// </summary>
		public IEventList<IToken> Tokens
		{
			get
			{
				if (null == _Tokens) 
				{
					if(null == _DAO.Tokens)
						_DAO.Tokens = new List<Token_DAO>();
					_Tokens = new DAOEventList<Token_DAO, IToken, Token>(_DAO.Tokens);
					_Tokens.BeforeAdd += new EventListHandler(OnTokens_BeforeAdd);					
					_Tokens.BeforeRemove += new EventListHandler(OnTokens_BeforeRemove);					
					_Tokens.AfterAdd += new EventListHandler(OnTokens_AfterAdd);					
					_Tokens.AfterRemove += new EventListHandler(OnTokens_AfterRemove);					
				}
				return _Tokens;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_StatementParameters = null;
			_Tokens = null;
			
		}
	}
}


