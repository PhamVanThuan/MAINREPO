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
	/// Rateconfiguration is used to expose available MarketRate / Margin combinations. Each mortgageLoan links to a RateConfiguration Entry, 
		/// but a list of MortgageLoans is not exposed here as this is a lookup.
	/// </summary>
	public partial class RateConfiguration : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RateConfiguration_DAO>, IRateConfiguration
	{
				public RateConfiguration(SAHL.Common.BusinessModel.DAO.RateConfiguration_DAO RateConfiguration) : base(RateConfiguration)
		{
			this._DAO = RateConfiguration;
		}
		/// <summary>
		/// This is the primary key, used to identify an instance of RateConfiguration.
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// The margin.
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
		/// The MarketRate.
		/// </summary>
		public IMarketRate MarketRate 
		{
			get
			{
				if (null == _DAO.MarketRate) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMarketRate, MarketRate_DAO>(_DAO.MarketRate);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.MarketRate = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.MarketRate = (MarketRate_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


