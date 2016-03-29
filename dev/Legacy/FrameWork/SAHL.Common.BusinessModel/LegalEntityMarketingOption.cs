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
	/// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOption_DAO
	/// </summary>
	public partial class LegalEntityMarketingOption : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOption_DAO>, ILegalEntityMarketingOption
	{
				public LegalEntityMarketingOption(SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOption_DAO LegalEntityMarketingOption) : base(LegalEntityMarketingOption)
		{
			this._DAO = LegalEntityMarketingOption;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOption_DAO.MarketingOption
		/// </summary>
		public IMarketingOption MarketingOption 
		{
			get
			{
				if (null == _DAO.MarketingOption) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMarketingOption, MarketingOption_DAO>(_DAO.MarketingOption);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.MarketingOption = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.MarketingOption = (MarketingOption_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOption_DAO.ChangeDate
		/// </summary>
		public DateTime? ChangeDate
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOption_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOption_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOption_DAO.LegalEntity
		/// </summary>
		public ILegalEntity LegalEntity 
		{
			get
			{
				if (null == _DAO.LegalEntity) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntity = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


