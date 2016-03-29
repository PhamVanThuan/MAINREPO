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
	public partial class ImportApplicationInformation : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ImportApplicationInformation_DAO>, IImportApplicationInformation
	{
				public ImportApplicationInformation(SAHL.Common.BusinessModel.DAO.ImportApplicationInformation_DAO ImportApplicationInformation) : base(ImportApplicationInformation)
		{
			this._DAO = ImportApplicationInformation;
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32 ApplicationTerm 
		{
			get { return _DAO.ApplicationTerm; }
			set { _DAO.ApplicationTerm = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double CashDeposit 
		{
			get { return _DAO.CashDeposit; }
			set { _DAO.CashDeposit = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double PropertyValuation 
		{
			get { return _DAO.PropertyValuation; }
			set { _DAO.PropertyValuation = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double FeesTotal 
		{
			get { return _DAO.FeesTotal; }
			set { _DAO.FeesTotal = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double InterimInterest 
		{
			get { return _DAO.InterimInterest; }
			set { _DAO.InterimInterest = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double MonthlyInstalment 
		{
			get { return _DAO.MonthlyInstalment; }
			set { _DAO.MonthlyInstalment = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double HOCPremium 
		{
			get { return _DAO.HOCPremium; }
			set { _DAO.HOCPremium = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double LifePremium 
		{
			get { return _DAO.LifePremium; }
			set { _DAO.LifePremium = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double PreApprovedAmount 
		{
			get { return _DAO.PreApprovedAmount; }
			set { _DAO.PreApprovedAmount = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double MaxCashAllowed 
		{
			get { return _DAO.MaxCashAllowed; }
			set { _DAO.MaxCashAllowed = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double MaxQuickCashAllowed 
		{
			get { return _DAO.MaxQuickCashAllowed; }
			set { _DAO.MaxQuickCashAllowed = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double RequestedQuickCashAmount 
		{
			get { return _DAO.RequestedQuickCashAmount; }
			set { _DAO.RequestedQuickCashAmount = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double BondToRegister 
		{
			get { return _DAO.BondToRegister; }
			set { _DAO.BondToRegister = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double LTV 
		{
			get { return _DAO.LTV; }
			set { _DAO.LTV = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double PTI 
		{
			get { return _DAO.PTI; }
			set { _DAO.PTI = value;}
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


