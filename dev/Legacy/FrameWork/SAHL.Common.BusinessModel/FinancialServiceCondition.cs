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
	/// SAHL.Common.BusinessModel.DAO.FinancialServiceCondition_DAO
	/// </summary>
	public partial class FinancialServiceCondition : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialServiceCondition_DAO>, IFinancialServiceCondition
	{
				public FinancialServiceCondition(SAHL.Common.BusinessModel.DAO.FinancialServiceCondition_DAO FinancialServiceCondition) : base(FinancialServiceCondition)
		{
			this._DAO = FinancialServiceCondition;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceCondition_DAO.UserDefinedConditionText
		/// </summary>
		public String UserDefinedConditionText 
		{
			get { return _DAO.UserDefinedConditionText; }
			set { _DAO.UserDefinedConditionText = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceCondition_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceCondition_DAO.Condition
		/// </summary>
		public ICondition Condition 
		{
			get
			{
				if (null == _DAO.Condition) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICondition, Condition_DAO>(_DAO.Condition);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Condition = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Condition = (Condition_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceCondition_DAO.ConditionType
		/// </summary>
		public IConditionType ConditionType 
		{
			get
			{
				if (null == _DAO.ConditionType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IConditionType, ConditionType_DAO>(_DAO.ConditionType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ConditionType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ConditionType = (ConditionType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceCondition_DAO.FinancialService
		/// </summary>
		public IFinancialService FinancialService 
		{
			get
			{
				if (null == _DAO.FinancialService) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialService, FinancialService_DAO>(_DAO.FinancialService);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialService = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialService = (FinancialService_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


