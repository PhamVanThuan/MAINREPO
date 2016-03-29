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
	/// SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO
	/// </summary>
	public partial class CreditCriteriaShortTerm : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO>, ICreditCriteriaShortTerm
	{
				public CreditCriteriaShortTerm(SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO CreditCriteriaShortTerm) : base(CreditCriteriaShortTerm)
		{
			this._DAO = CreditCriteriaShortTerm;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO.MinLoanAmount
		/// </summary>
		public Double MinLoanAmount 
		{
			get { return _DAO.MinLoanAmount; }
			set { _DAO.MinLoanAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO.MaxLoanAmount
		/// </summary>
		public Double MaxLoanAmount 
		{
			get { return _DAO.MaxLoanAmount; }
			set { _DAO.MaxLoanAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO.Term
		/// </summary>
		public Int32 Term 
		{
			get { return _DAO.Term; }
			set { _DAO.Term = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO.CreditMatrixShortTerm
		/// </summary>
		public ICreditMatrixShortTerm CreditMatrixShortTerm 
		{
			get
			{
				if (null == _DAO.CreditMatrixShortTerm) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICreditMatrixShortTerm, CreditMatrixShortTerm_DAO>(_DAO.CreditMatrixShortTerm);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.CreditMatrixShortTerm = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.CreditMatrixShortTerm = (CreditMatrixShortTerm_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO.Margin
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
	}
}


