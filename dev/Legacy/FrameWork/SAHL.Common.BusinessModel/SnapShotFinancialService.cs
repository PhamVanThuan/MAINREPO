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
	/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO
	/// </summary>
	public partial class SnapShotFinancialService : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO>, ISnapShotFinancialService
	{
				public SnapShotFinancialService(SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO SnapShotFinancialService) : base(SnapShotFinancialService)
		{
			this._DAO = SnapShotFinancialService;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.Account
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
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.FinancialService
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.FinancialServiceType
		/// </summary>
		public IFinancialServiceType FinancialServiceType 
		{
			get
			{
				if (null == _DAO.FinancialServiceType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialServiceType, FinancialServiceType_DAO>(_DAO.FinancialServiceType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialServiceType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialServiceType = (FinancialServiceType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.ResetConfiguration
		/// </summary>
		public IResetConfiguration ResetConfiguration 
		{
			get
			{
				if (null == _DAO.ResetConfiguration) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IResetConfiguration, ResetConfiguration_DAO>(_DAO.ResetConfiguration);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ResetConfiguration = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ResetConfiguration = (ResetConfiguration_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.ActiveMarketRate
		/// </summary>
		public Double ActiveMarketRate 
		{
			get { return _DAO.ActiveMarketRate; }
			set { _DAO.ActiveMarketRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.Margin
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
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.Installment
		/// </summary>
		public Double Installment 
		{
			get { return _DAO.Installment; }
			set { _DAO.Installment = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.SnapShotFinancialAdjustments
		/// </summary>
		private DAOEventList<SnapShotFinancialAdjustment_DAO, ISnapShotFinancialAdjustment, SnapShotFinancialAdjustment> _SnapShotFinancialAdjustments;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.SnapShotFinancialAdjustments
		/// </summary>
		public IEventList<ISnapShotFinancialAdjustment> SnapShotFinancialAdjustments
		{
			get
			{
				if (null == _SnapShotFinancialAdjustments) 
				{
					if(null == _DAO.SnapShotFinancialAdjustments)
						_DAO.SnapShotFinancialAdjustments = new List<SnapShotFinancialAdjustment_DAO>();
					_SnapShotFinancialAdjustments = new DAOEventList<SnapShotFinancialAdjustment_DAO, ISnapShotFinancialAdjustment, SnapShotFinancialAdjustment>(_DAO.SnapShotFinancialAdjustments);
                    _SnapShotFinancialAdjustments.BeforeAdd += new EventListHandler(OnSnapShotFinancialAdjustments_BeforeAdd);				
					_SnapShotFinancialAdjustments.BeforeRemove += new EventListHandler(OnSnapShotFinancialAdjustments_BeforeRemove);					
					_SnapShotFinancialAdjustments.AfterAdd += new EventListHandler(OnSnapShotFinancialAdjustments_AfterAdd);					
					_SnapShotFinancialAdjustments.AfterRemove += new EventListHandler(OnSnapShotFinancialAdjustments_AfterRemove);					
				}
				return _SnapShotFinancialAdjustments;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialService_DAO.SnapShotAccount
		/// </summary>
		public ISnapShotAccount SnapShotAccount 
		{
			get
			{
				if (null == _DAO.SnapShotAccount) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ISnapShotAccount, SnapShotAccount_DAO>(_DAO.SnapShotAccount);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.SnapShotAccount = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.SnapShotAccount = (SnapShotAccount_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_SnapShotFinancialAdjustments = null;
			
		}

        

    }
}


