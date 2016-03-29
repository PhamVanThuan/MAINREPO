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
	/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO
	/// </summary>
	public partial class SnapShotRateOverride : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO>, ISnapShotRateOverride
	{
				public SnapShotRateOverride(SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO SnapShotRateOverride) : base(SnapShotRateOverride)
		{
			this._DAO = SnapShotRateOverride;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.AccountKey
		/// </summary>
		public Int32 AccountKey 
		{
			get { return _DAO.AccountKey; }
			set { _DAO.AccountKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.RateOverrideKey
		/// </summary>
		public Int32 RateOverrideKey 
		{
			get { return _DAO.RateOverrideKey; }
			set { _DAO.RateOverrideKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.FinancialServiceKey
		/// </summary>
		public Int32 FinancialServiceKey 
		{
			get { return _DAO.FinancialServiceKey; }
			set { _DAO.FinancialServiceKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.RateOverrideTypeKey
		/// </summary>
		public Int32 RateOverrideTypeKey 
		{
			get { return _DAO.RateOverrideTypeKey; }
			set { _DAO.RateOverrideTypeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.FromDate
		/// </summary>
		public DateTime FromDate 
		{
			get { return _DAO.FromDate; }
			set { _DAO.FromDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.Term
		/// </summary>
		public Int32 Term 
		{
			get { return _DAO.Term; }
			set { _DAO.Term = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.CapRate
		/// </summary>
		public Double CapRate 
		{
			get { return _DAO.CapRate; }
			set { _DAO.CapRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.FloorRate
		/// </summary>
		public Double FloorRate 
		{
			get { return _DAO.FloorRate; }
			set { _DAO.FloorRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.FixedRate
		/// </summary>
		public Double FixedRate 
		{
			get { return _DAO.FixedRate; }
			set { _DAO.FixedRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.Discount
		/// </summary>
		public Double Discount 
		{
			get { return _DAO.Discount; }
			set { _DAO.Discount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.GeneralStatusKey
		/// </summary>
		public Int32 GeneralStatusKey 
		{
			get { return _DAO.GeneralStatusKey; }
			set { _DAO.GeneralStatusKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.TradeKey
		/// </summary>
		public Int32 TradeKey 
		{
			get { return _DAO.TradeKey; }
			set { _DAO.TradeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.CancellationDate
		/// </summary>
		public DateTime CancellationDate 
		{
			get { return _DAO.CancellationDate; }
			set { _DAO.CancellationDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.CancellationReasonKey
		/// </summary>
		public Int32 CancellationReasonKey 
		{
			get { return _DAO.CancellationReasonKey; }
			set { _DAO.CancellationReasonKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.CapBalance
		/// </summary>
		public Double CapBalance 
		{
			get { return _DAO.CapBalance; }
			set { _DAO.CapBalance = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.Amount
		/// </summary>
		public Double Amount 
		{
			get { return _DAO.Amount; }
			set { _DAO.Amount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.CAPPaymentOptionKey
		/// </summary>
		public Int32 CAPPaymentOptionKey 
		{
			get { return _DAO.CAPPaymentOptionKey; }
			set { _DAO.CAPPaymentOptionKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.EndDate
		/// </summary>
		public DateTime EndDate 
		{
			get { return _DAO.EndDate; }
			set { _DAO.EndDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotRateOverride_DAO.SnapShotFinancialService
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


