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
	/// SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO
	/// </summary>
	public partial class CreditCriteriaUnsecuredLending : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO>, ICreditCriteriaUnsecuredLending
	{
				public CreditCriteriaUnsecuredLending(SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO CreditCriteriaUnsecuredLending) : base(CreditCriteriaUnsecuredLending)
		{
			this._DAO = CreditCriteriaUnsecuredLending;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO.MinLoanAmount
		/// </summary>
		public Double MinLoanAmount 
		{
			get { return _DAO.MinLoanAmount; }
			set { _DAO.MinLoanAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO.MaxLoanAmount
		/// </summary>
		public Double MaxLoanAmount 
		{
			get { return _DAO.MaxLoanAmount; }
			set { _DAO.MaxLoanAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO.Term
		/// </summary>
		public Int32 Term 
		{
			get { return _DAO.Term; }
			set { _DAO.Term = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO.CreditMatrixUnsecuredLending
		/// </summary>
		public ICreditMatrixUnsecuredLending CreditMatrixUnsecuredLending 
		{
			get
			{
				if (null == _DAO.CreditMatrixUnsecuredLending) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICreditMatrixUnsecuredLending, CreditMatrixUnsecuredLending_DAO>(_DAO.CreditMatrixUnsecuredLending);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.CreditMatrixUnsecuredLending = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.CreditMatrixUnsecuredLending = (CreditMatrixUnsecuredLending_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO.Margin
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


