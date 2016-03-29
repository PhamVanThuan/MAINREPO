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
	/// SAHL.Common.BusinessModel.DAO.RuleParameter_DAO
	/// </summary>
	public partial class RuleParameter : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RuleParameter_DAO>, IRuleParameter
	{
				public RuleParameter(SAHL.Common.BusinessModel.DAO.RuleParameter_DAO RuleParameter) : base(RuleParameter)
		{
			this._DAO = RuleParameter;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RuleParameter_DAO.Name
		/// </summary>
		public String Name 
		{
			get { return _DAO.Name; }
			set { _DAO.Name = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RuleParameter_DAO.Value
		/// </summary>
		public String Value 
		{
			get { return _DAO.Value; }
			set { _DAO.Value = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RuleParameter_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RuleParameter_DAO.RuleItem
		/// </summary>
		public IRuleItem RuleItem 
		{
			get
			{
				if (null == _DAO.RuleItem) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRuleItem, RuleItem_DAO>(_DAO.RuleItem);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RuleItem = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RuleItem = (RuleItem_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RuleParameter_DAO.RuleParameterType
		/// </summary>
		public IParameterType RuleParameterType 
		{
			get
			{
				if (null == _DAO.RuleParameterType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IParameterType, ParameterType_DAO>(_DAO.RuleParameterType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RuleParameterType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RuleParameterType = (ParameterType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


