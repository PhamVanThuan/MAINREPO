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
	/// Certain products require the effective rate on the Account to be overriden. For example, a Super Lo loan will result in a 
		/// 0.60% discount to the effective rate or an invoked CAP will result in a discounted rate. RateOverride_DAO is instantiated 
		/// when the client elects to take up these types of products.
	/// </summary>
	public partial class RateOverride : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RateOverride_DAO>, IRateOverride
	{
				public RateOverride(SAHL.Common.BusinessModel.DAO.RateOverride_DAO RateOverride) : base(RateOverride)
		{
			this._DAO = RateOverride;
		}
		/// <summary>
		/// The date from which the Rate Override will be in effect.
		/// </summary>
		public DateTime? FromDate
		{
			get { return _DAO.FromDate; }
			set { _DAO.FromDate = value;}
		}
		/// <summary>
		/// The period, in months, for which the Rate Override will be applied.
		/// </summary>
		public Int32? Term
		{
			get { return _DAO.Term; }
			set { _DAO.Term = value;}
		}
		/// <summary>
		/// The rate at which the client has elected to CAP the rate applicable to their CAP Balance.
		/// </summary>
		public Double? CapRate
		{
			get { return _DAO.CapRate; }
			set { _DAO.CapRate = value;}
		}
		/// <summary>
		/// This is not currently being used.
		/// </summary>
		public Double? FloorRate
		{
			get { return _DAO.FloorRate; }
			set { _DAO.FloorRate = value;}
		}
		/// <summary>
		/// This is not currently being used.
		/// </summary>
		public Double? FixedRate
		{
			get { return _DAO.FixedRate; }
			set { _DAO.FixedRate = value;}
		}
		/// <summary>
		/// The percentage discount which applies for the rate override. For a Super Lo loan this would be 0.60%.
		/// </summary>
		public Double? Discount
		{
			get { return _DAO.Discount; }
			set { _DAO.Discount = value;}
		}
		/// <summary>
		/// The date on which the Rate Override was cancelled.
		/// </summary>
		public DateTime? CancellationDate
		{
			get { return _DAO.CancellationDate; }
			set { _DAO.CancellationDate = value;}
		}
		/// <summary>
		/// The balance which the client elected to CAP. If the client has taken a further loan, the CapRate is only applied
		/// to the CapBalance and not any subsequent increase.
		/// </summary>
		public Double? CapBalance
		{
			get { return _DAO.CapBalance; }
			set { _DAO.CapBalance = value;}
		}
		/// <summary>
		/// This property is used to capture a reduced debit order amount for the client, which will override the normal instalment due
		/// on the account. This could be used when a client is under debt review and could only afford a certain instalment or even a zero
		/// instalment.
		/// </summary>
		public Double? Amount
		{
			get { return _DAO.Amount; }
			set { _DAO.Amount = value;}
		}
		/// <summary>
		/// The date until which the Rate Override will be in effect.
		/// </summary>
		public DateTime? EndDate
		{
			get { return _DAO.EndDate; }
			set { _DAO.EndDate = value;}
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// This is the foreign key reference to the Reason table. Each Rateoverride that is cancelled by a client requires a cancellation reason.
		/// </summary>
		public ICancellationReason CancellationReason 
		{
			get
			{
				if (null == _DAO.CancellationReason) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICancellationReason, CancellationReason_DAO>(_DAO.CancellationReason);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.CancellationReason = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.CancellationReason = (CancellationReason_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// This is the foreign key reference to the FinancialService table. Each Rateoverride belongs to a Financial Service.
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
		/// This is the foreign key reference to the GeneralStatus table. Each RateOverride belongs to a specific status which determines
		/// whether it is active or not.
		/// </summary>
		public IGeneralStatus GeneralStatus 
		{
			get
			{
				if (null == _DAO.GeneralStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GeneralStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// This is the foreign key reference to the RateOverrideType table. Each RateOverride belongs to a specific type i.e.
		/// Super Lo, Interest Only, CAP.
		/// </summary>
		public IRateOverrideType RateOverrideType 
		{
			get
			{
				if (null == _DAO.RateOverrideType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRateOverrideType, RateOverrideType_DAO>(_DAO.RateOverrideType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RateOverrideType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RateOverrideType = (RateOverrideType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// This is the foreign key reference to the Trade table. A CAP RateOverride belongs to a specific trade. 
		/// The CapBalance on the RateOverride is allocated to this trade, which is bought in order to fund the CAP product.
		/// </summary>
		public ITrade Trade 
		{
			get
			{
				if (null == _DAO.Trade) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITrade, Trade_DAO>(_DAO.Trade);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Trade = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Trade = (Trade_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


