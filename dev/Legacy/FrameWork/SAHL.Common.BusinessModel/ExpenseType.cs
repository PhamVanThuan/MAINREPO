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
	/// SAHL.Common.BusinessModel.DAO.ExpenseType_DAO
	/// </summary>
	public partial class ExpenseType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ExpenseType_DAO>, IExpenseType
	{
				public ExpenseType(SAHL.Common.BusinessModel.DAO.ExpenseType_DAO ExpenseType) : base(ExpenseType)
		{
			this._DAO = ExpenseType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExpenseType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExpenseType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExpenseType_DAO.ExpenseTypeGroup
		/// </summary>
		public IExpenseTypeGroup ExpenseTypeGroup 
		{
			get
			{
				if (null == _DAO.ExpenseTypeGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IExpenseTypeGroup, ExpenseTypeGroup_DAO>(_DAO.ExpenseTypeGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ExpenseTypeGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ExpenseTypeGroup = (ExpenseTypeGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExpenseType_DAO.PaymentType
		/// </summary>
		public IPaymentType PaymentType 
		{
			get
			{
				if (null == _DAO.PaymentType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IPaymentType, PaymentType_DAO>(_DAO.PaymentType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.PaymentType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.PaymentType = (PaymentType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


