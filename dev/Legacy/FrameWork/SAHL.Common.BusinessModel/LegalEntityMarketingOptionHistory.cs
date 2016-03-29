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
	/// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO
	/// </summary>
	public partial class LegalEntityMarketingOptionHistory : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO>, ILegalEntityMarketingOptionHistory
	{
				public LegalEntityMarketingOptionHistory(SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO LegalEntityMarketingOptionHistory) : base(LegalEntityMarketingOptionHistory)
		{
			this._DAO = LegalEntityMarketingOptionHistory;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO.LegalEntityMarketingOptionKey
		/// </summary>
		public ILegalEntityMarketingOption LegalEntityMarketingOptionKey 
		{
			get
			{
				if (null == _DAO.LegalEntityMarketingOptionKey) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntityMarketingOption, LegalEntityMarketingOption_DAO>(_DAO.LegalEntityMarketingOptionKey);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntityMarketingOptionKey = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntityMarketingOptionKey = (LegalEntityMarketingOption_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO.LegalEntity
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO.MarketingOptionKey
		/// </summary>
		public Int32 MarketingOptionKey 
		{
			get { return _DAO.MarketingOptionKey; }
			set { _DAO.MarketingOptionKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO.ChangeAction
		/// </summary>
		public Char ChangeAction 
		{
			get { return _DAO.ChangeAction; }
			set { _DAO.ChangeAction = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO.ChangeDate
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


