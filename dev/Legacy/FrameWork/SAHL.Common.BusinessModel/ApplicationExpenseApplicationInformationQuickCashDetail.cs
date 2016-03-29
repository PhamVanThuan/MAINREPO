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
	/// 
	/// </summary>
	public partial class ApplicationExpenseApplicationInformationQuickCashDetail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationExpenseApplicationInformationQuickCashDetail_DAO>, IApplicationExpenseApplicationInformationQuickCashDetail
	{
				public ApplicationExpenseApplicationInformationQuickCashDetail(SAHL.Common.BusinessModel.DAO.ApplicationExpenseApplicationInformationQuickCashDetail_DAO ApplicationExpenseApplicationInformationQuickCashDetail) : base(ApplicationExpenseApplicationInformationQuickCashDetail)
		{
			this._DAO = ApplicationExpenseApplicationInformationQuickCashDetail;
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public IApplicationExpense ApplicationExpense 
		{
			get
			{
				if (null == _DAO.ApplicationExpense) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplicationExpense, ApplicationExpense_DAO>(_DAO.ApplicationExpense);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ApplicationExpense = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ApplicationExpense = (ApplicationExpense_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public IApplicationInformationQuickCashDetail ApplicationInformationQuickCashDetail 
		{
			get
			{
				if (null == _DAO.ApplicationInformationQuickCashDetail) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplicationInformationQuickCashDetail, ApplicationInformationQuickCashDetail_DAO>(_DAO.ApplicationInformationQuickCashDetail);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ApplicationInformationQuickCashDetail = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ApplicationInformationQuickCashDetail = (ApplicationInformationQuickCashDetail_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


