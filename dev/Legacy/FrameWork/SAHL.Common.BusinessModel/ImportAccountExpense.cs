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
	public partial class ImportAccountExpense : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ImportAccountExpense_DAO>, IImportAccountExpense
	{
				public ImportAccountExpense(SAHL.Common.BusinessModel.DAO.ImportAccountExpense_DAO ImportAccountExpense) : base(ImportAccountExpense)
		{
			this._DAO = ImportAccountExpense;
		}
		/// <summary>
		/// 
		/// </summary>
		public String ExpenseTypeKey 
		{
			get { return _DAO.ExpenseTypeKey; }
			set { _DAO.ExpenseTypeKey = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String ExpenseAccountNumber 
		{
			get { return _DAO.ExpenseAccountNumber; }
			set { _DAO.ExpenseAccountNumber = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String ExpenseAccountName 
		{
			get { return _DAO.ExpenseAccountName; }
			set { _DAO.ExpenseAccountName = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String ExpenseReference 
		{
			get { return _DAO.ExpenseReference; }
			set { _DAO.ExpenseReference = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double TotalOutstandingAmount 
		{
			get { return _DAO.TotalOutstandingAmount; }
			set { _DAO.TotalOutstandingAmount = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double MonthlyPayment 
		{
			get { return _DAO.MonthlyPayment; }
			set { _DAO.MonthlyPayment = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Boolean ToBeSettled 
		{
			get { return _DAO.ToBeSettled; }
			set { _DAO.ToBeSettled = value;}
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
		public IImportApplication ImportApplication 
		{
			get
			{
				if (null == _DAO.ImportApplication) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IImportApplication, ImportApplication_DAO>(_DAO.ImportApplication);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ImportApplication = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ImportApplication = (ImportApplication_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


