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
	/// SAHL.Common.BusinessModel.DAO.Trade_DAO
	/// </summary>
	public partial class Trade : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Trade_DAO>, ITrade
	{
				public Trade(SAHL.Common.BusinessModel.DAO.Trade_DAO Trade) : base(Trade)
		{
			this._DAO = Trade;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Trade_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Trade_DAO.ResetConfiguration
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
		/// SAHL.Common.BusinessModel.DAO.Trade_DAO.TradeType
		/// </summary>
		public String TradeType 
		{
			get { return _DAO.TradeType; }
			set { _DAO.TradeType = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Trade_DAO.Company
		/// </summary>
		public String Company 
		{
			get { return _DAO.Company; }
			set { _DAO.Company = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Trade_DAO.TradeDate
		/// </summary>
		public DateTime TradeDate 
		{
			get { return _DAO.TradeDate; }
			set { _DAO.TradeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Trade_DAO.StartDate
		/// </summary>
		public DateTime StartDate 
		{
			get { return _DAO.StartDate; }
			set { _DAO.StartDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Trade_DAO.EndDate
		/// </summary>
		public DateTime EndDate 
		{
			get { return _DAO.EndDate; }
			set { _DAO.EndDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Trade_DAO.StrikeRate
		/// </summary>
		public Double StrikeRate 
		{
			get { return _DAO.StrikeRate; }
			set { _DAO.StrikeRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Trade_DAO.TradeBalance
		/// </summary>
		public Double TradeBalance 
		{
			get { return _DAO.TradeBalance; }
			set { _DAO.TradeBalance = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Trade_DAO.CapBalance
		/// </summary>
		public Double CapBalance 
		{
			get { return _DAO.CapBalance; }
			set { _DAO.CapBalance = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Trade_DAO.Premium
		/// </summary>
		public Double? Premium
		{
			get { return _DAO.Premium; }
			set { _DAO.Premium = value;}
		}
	}
}


