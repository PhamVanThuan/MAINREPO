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
	/// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO
	/// </summary>
	public partial class SnapShotAccount : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO>, ISnapShotAccount
	{
				public SnapShotAccount(SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO SnapShotAccount) : base(SnapShotAccount)
		{
			this._DAO = SnapShotAccount;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.Account
		/// </summary>
		public IAccount Account 
		{
			get
			{
				if (null == _DAO.Account) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.Account);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Account = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Account = (Account_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.RemainingInstallments
		/// </summary>
		public Int32 RemainingInstallments 
		{
			get { return _DAO.RemainingInstallments; }
			set { _DAO.RemainingInstallments = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.Product
		/// </summary>
		public IProduct Product 
		{
			get
			{
				if (null == _DAO.Product) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IProduct, Product_DAO>(_DAO.Product);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Product = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Product = (Product_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.Valuation
		/// </summary>
		public IValuation Valuation 
		{
			get
			{
				if (null == _DAO.Valuation) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IValuation, Valuation_DAO>(_DAO.Valuation);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Valuation = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Valuation = (Valuation_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.InsertDate
		/// </summary>
		public DateTime InsertDate 
		{
			get { return _DAO.InsertDate; }
			set { _DAO.InsertDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.DebtCounselling
		/// </summary>
		public IDebtCounselling DebtCounselling 
		{
			get
			{
				if (null == _DAO.DebtCounselling) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDebtCounselling, DebtCounselling_DAO>(_DAO.DebtCounselling);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DebtCounselling = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DebtCounselling = (DebtCounselling_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.HOCPremium
		/// </summary>
		public Double HOCPremium 
		{
			get { return _DAO.HOCPremium; }
			set { _DAO.HOCPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.LifePremium
		/// </summary>
		public Double LifePremium 
		{
			get { return _DAO.LifePremium; }
			set { _DAO.LifePremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.MonthlyServiceFee
		/// </summary>
		public Double MonthlyServiceFee 
		{
			get { return _DAO.MonthlyServiceFee; }
			set { _DAO.MonthlyServiceFee = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.SnapShotFinancialServices
		/// </summary>
		private DAOEventList<SnapShotFinancialService_DAO, ISnapShotFinancialService, SnapShotFinancialService> _SnapShotFinancialServices;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotAccount_DAO.SnapShotFinancialServices
		/// </summary>
		public IEventList<ISnapShotFinancialService> SnapShotFinancialServices
		{
			get
			{
				if (null == _SnapShotFinancialServices) 
				{
					if(null == _DAO.SnapShotFinancialServices)
						_DAO.SnapShotFinancialServices = new List<SnapShotFinancialService_DAO>();
					_SnapShotFinancialServices = new DAOEventList<SnapShotFinancialService_DAO, ISnapShotFinancialService, SnapShotFinancialService>(_DAO.SnapShotFinancialServices);
					_SnapShotFinancialServices.BeforeAdd += new EventListHandler(OnSnapShotFinancialServices_BeforeAdd);					
					_SnapShotFinancialServices.BeforeRemove += new EventListHandler(OnSnapShotFinancialServices_BeforeRemove);					
					_SnapShotFinancialServices.AfterAdd += new EventListHandler(OnSnapShotFinancialServices_AfterAdd);					
					_SnapShotFinancialServices.AfterRemove += new EventListHandler(OnSnapShotFinancialServices_AfterRemove);					
				}
				return _SnapShotFinancialServices;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_SnapShotFinancialServices = null;
			
		}
	}
}


