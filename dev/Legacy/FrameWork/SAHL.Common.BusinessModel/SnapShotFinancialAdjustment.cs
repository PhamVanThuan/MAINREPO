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
	/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO
	/// </summary>
	public partial class SnapShotFinancialAdjustment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO>, ISnapShotFinancialAdjustment
	{
				public SnapShotFinancialAdjustment(SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO SnapShotFinancialAdjustment) : base(SnapShotFinancialAdjustment)
		{
			this._DAO = SnapShotFinancialAdjustment;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.Account
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
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FinancialAdjustment
		/// </summary>
		public IFinancialAdjustment FinancialAdjustment 
		{
			get
			{
				if (null == _DAO.FinancialAdjustment) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialAdjustment, FinancialAdjustment_DAO>(_DAO.FinancialAdjustment);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialAdjustment = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialAdjustment = (FinancialAdjustment_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FinancialService
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
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FinancialAdjustmentSource
		/// </summary>
		public IFinancialAdjustmentSource FinancialAdjustmentSource 
		{
			get
			{
				if (null == _DAO.FinancialAdjustmentSource) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialAdjustmentSource, FinancialAdjustmentSource_DAO>(_DAO.FinancialAdjustmentSource);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialAdjustmentSource = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialAdjustmentSource = (FinancialAdjustmentSource_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FinancialAdjustmentType
		/// </summary>
		public IFinancialAdjustmentType FinancialAdjustmentType 
		{
			get
			{
				if (null == _DAO.FinancialAdjustmentType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialAdjustmentType, FinancialAdjustmentType_DAO>(_DAO.FinancialAdjustmentType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialAdjustmentType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialAdjustmentType = (FinancialAdjustmentType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FinancialAdjustmentStatus
		/// </summary>
		public IFinancialAdjustmentStatus FinancialAdjustmentStatus 
		{
			get
			{
				if (null == _DAO.FinancialAdjustmentStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialAdjustmentStatus, FinancialAdjustmentStatus_DAO>(_DAO.FinancialAdjustmentStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialAdjustmentStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialAdjustmentStatus = (FinancialAdjustmentStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FromDate
		/// </summary>
		public DateTime? FromDate
		{
			get { return _DAO.FromDate; }
			set { _DAO.FromDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.EndDate
		/// </summary>
		public DateTime? EndDate
		{
			get { return _DAO.EndDate; }
			set { _DAO.EndDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.CancellationDate
		/// </summary>
		public DateTime? CancellationDate
		{
			get { return _DAO.CancellationDate; }
			set { _DAO.CancellationDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.CancellationReasonKey
		/// </summary>
		public Int32 CancellationReasonKey 
		{
			get { return _DAO.CancellationReasonKey; }
			set { _DAO.CancellationReasonKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.TransactionType
		/// </summary>
		public ITransactionType TransactionType 
		{
			get
			{
				if (null == _DAO.TransactionType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITransactionType, TransactionType_DAO>(_DAO.TransactionType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.TransactionType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.TransactionType = (TransactionType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FRARate
		/// </summary>
		public Double FRARate 
		{
			get { return _DAO.FRARate; }
			set { _DAO.FRARate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.IRAAdjustment
		/// </summary>
		public Double IRAAdjustment 
		{
			get { return _DAO.IRAAdjustment; }
			set { _DAO.IRAAdjustment = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.RPAReversalPercentage
		/// </summary>
		public Double RPAReversalPercentage 
		{
			get { return _DAO.RPAReversalPercentage; }
			set { _DAO.RPAReversalPercentage = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.DPADifferentialAdjustment
		/// </summary>
		public Double DPADifferentialAdjustment 
		{
			get { return _DAO.DPADifferentialAdjustment; }
			set { _DAO.DPADifferentialAdjustment = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.DPABalanceType
		/// </summary>
		public IBalanceType DPABalanceType 
		{
			get
			{
				if (null == _DAO.DPABalanceType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IBalanceType, BalanceType_DAO>(_DAO.DPABalanceType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DPABalanceType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DPABalanceType = (BalanceType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.Amount
		/// </summary>
		public Double Amount 
		{
			get { return _DAO.Amount; }
			set { _DAO.Amount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.SnapShotFinancialService
		/// </summary>
		public ISnapShotFinancialService SnapShotFinancialService 
		{
			get
			{
				if (null == _DAO.SnapShotFinancialService) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ISnapShotFinancialService, SnapShotFinancialService_DAO>(_DAO.SnapShotFinancialService);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.SnapShotFinancialService = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.SnapShotFinancialService = (SnapShotFinancialService_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


