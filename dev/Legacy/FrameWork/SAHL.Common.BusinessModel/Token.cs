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
	/// SAHL.Common.BusinessModel.DAO.Token_DAO
	/// </summary>
	public partial class Token : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Token_DAO>, IToken
	{
				public Token(SAHL.Common.BusinessModel.DAO.Token_DAO Token) : base(Token)
		{
			this._DAO = Token;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Token_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Token_DAO.MustTranslate
		/// </summary>
		public Boolean MustTranslate 
		{
			get { return _DAO.MustTranslate; }
			set { _DAO.MustTranslate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Token_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Token_DAO.ConditionTokens
		/// </summary>
		private DAOEventList<ConditionToken_DAO, IConditionToken, ConditionToken> _ConditionTokens;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Token_DAO.ConditionTokens
		/// </summary>
		public IEventList<IConditionToken> ConditionTokens
		{
			get
			{
				if (null == _ConditionTokens) 
				{
					if(null == _DAO.ConditionTokens)
						_DAO.ConditionTokens = new List<ConditionToken_DAO>();
					_ConditionTokens = new DAOEventList<ConditionToken_DAO, IConditionToken, ConditionToken>(_DAO.ConditionTokens);
					_ConditionTokens.BeforeAdd += new EventListHandler(OnConditionTokens_BeforeAdd);					
					_ConditionTokens.BeforeRemove += new EventListHandler(OnConditionTokens_BeforeRemove);					
					_ConditionTokens.AfterAdd += new EventListHandler(OnConditionTokens_AfterAdd);					
					_ConditionTokens.AfterRemove += new EventListHandler(OnConditionTokens_AfterRemove);					
				}
				return _ConditionTokens;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Token_DAO.ParameterType
		/// </summary>
		public IParameterType ParameterType 
		{
			get
			{
				if (null == _DAO.ParameterType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IParameterType, ParameterType_DAO>(_DAO.ParameterType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ParameterType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ParameterType = (ParameterType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Token_DAO.StatementDefinition
		/// </summary>
		public IStatementDefinition StatementDefinition 
		{
			get
			{
				if (null == _DAO.StatementDefinition) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IStatementDefinition, StatementDefinition_DAO>(_DAO.StatementDefinition);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.StatementDefinition = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.StatementDefinition = (StatementDefinition_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Token_DAO.TokenType
		/// </summary>
		public ITokenType TokenType 
		{
			get
			{
				if (null == _DAO.TokenType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITokenType, TokenType_DAO>(_DAO.TokenType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.TokenType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.TokenType = (TokenType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ConditionTokens = null;
			
		}
	}
}


