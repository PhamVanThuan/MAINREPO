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
	/// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO
	/// </summary>
	public partial class StatementParameter : BusinessModelBase<SAHL.Common.BusinessModel.DAO.StatementParameter_DAO>, IStatementParameter
	{
				public StatementParameter(SAHL.Common.BusinessModel.DAO.StatementParameter_DAO StatementParameter) : base(StatementParameter)
		{
			this._DAO = StatementParameter;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.ParameterName
		/// </summary>
		public String ParameterName 
		{
			get { return _DAO.ParameterName; }
			set { _DAO.ParameterName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.ParameterLength
		/// </summary>
		public Int32? ParameterLength
		{
			get { return _DAO.ParameterLength; }
			set { _DAO.ParameterLength = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.DisplayName
		/// </summary>
		public String DisplayName 
		{
			get { return _DAO.DisplayName; }
			set { _DAO.DisplayName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.Required
		/// </summary>
		public Boolean Required 
		{
			get { return _DAO.Required; }
			set { _DAO.Required = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.ParameterType
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
		/// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.PopulationStatementDefinition
		/// </summary>
		public IStatementDefinition PopulationStatementDefinition 
		{
			get
			{
				if (null == _DAO.PopulationStatementDefinition) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IStatementDefinition, StatementDefinition_DAO>(_DAO.PopulationStatementDefinition);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.PopulationStatementDefinition = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.PopulationStatementDefinition = (StatementDefinition_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.StatementDefinition
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
	}
}


