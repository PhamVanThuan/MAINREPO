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
	/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO
	/// </summary>
	public partial class CreditCriteria : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO>, ICreditCriteria
	{
				public CreditCriteria(SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO CreditCriteria) : base(CreditCriteria)
		{
			this._DAO = CreditCriteria;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MinLoanAmount
		/// </summary>
		public Double? MinLoanAmount
		{
			get { return _DAO.MinLoanAmount; }
			set { _DAO.MinLoanAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MaxLoanAmount
		/// </summary>
		public Double? MaxLoanAmount
		{
			get { return _DAO.MaxLoanAmount; }
			set { _DAO.MaxLoanAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MinPropertyValue
		/// </summary>
		public Double? MinPropertyValue
		{
			get { return _DAO.MinPropertyValue; }
			set { _DAO.MinPropertyValue = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MaxPropertyValue
		/// </summary>
		public Double? MaxPropertyValue
		{
			get { return _DAO.MaxPropertyValue; }
			set { _DAO.MaxPropertyValue = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.LTV
		/// </summary>
		public Double? LTV
		{
			get { return _DAO.LTV; }
			set { _DAO.LTV = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.PTI
		/// </summary>
		public Double? PTI
		{
			get { return _DAO.PTI; }
			set { _DAO.PTI = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MinIncomeAmount
		/// </summary>
		public Double? MinIncomeAmount
		{
			get { return _DAO.MinIncomeAmount; }
			set { _DAO.MinIncomeAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MaxIncomeAmount
		/// </summary>
		public Double? MaxIncomeAmount
		{
			get { return _DAO.MaxIncomeAmount; }
			set { _DAO.MaxIncomeAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.ExceptionCriteria
		/// </summary>
		public Boolean? ExceptionCriteria
		{
			get { return _DAO.ExceptionCriteria; }
			set { _DAO.ExceptionCriteria = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MinEmpiricaScore
		/// </summary>
		public Int32? MinEmpiricaScore
		{
			get { return _DAO.MinEmpiricaScore; }
			set { _DAO.MinEmpiricaScore = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.Category
		/// </summary>
		public ICategory Category 
		{
			get
			{
				if (null == _DAO.Category) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICategory, Category_DAO>(_DAO.Category);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Category = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Category = (Category_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.CreditMatrix
		/// </summary>
		public ICreditMatrix CreditMatrix 
		{
			get
			{
				if (null == _DAO.CreditMatrix) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICreditMatrix, CreditMatrix_DAO>(_DAO.CreditMatrix);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.CreditMatrix = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.CreditMatrix = (CreditMatrix_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.EmploymentType
		/// </summary>
		public IEmploymentType EmploymentType 
		{
			get
			{
				if (null == _DAO.EmploymentType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IEmploymentType, EmploymentType_DAO>(_DAO.EmploymentType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.EmploymentType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.EmploymentType = (EmploymentType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.Margin
		/// </summary>
		public IMargin Margin 
		{
			get
			{
				if (null == _DAO.Margin) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMargin, Margin_DAO>(_DAO.Margin);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Margin = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Margin = (Margin_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MortgageLoanPurpose
		/// </summary>
		public IMortgageLoanPurpose MortgageLoanPurpose 
		{
			get
			{
				if (null == _DAO.MortgageLoanPurpose) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMortgageLoanPurpose, MortgageLoanPurpose_DAO>(_DAO.MortgageLoanPurpose);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.MortgageLoanPurpose = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.MortgageLoanPurpose = (MortgageLoanPurpose_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


