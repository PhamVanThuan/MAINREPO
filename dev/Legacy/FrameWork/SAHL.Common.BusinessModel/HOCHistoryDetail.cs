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
	/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO
	/// </summary>
	public partial class HOCHistoryDetail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO>, IHOCHistoryDetail
	{
				public HOCHistoryDetail(SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO HOCHistoryDetail) : base(HOCHistoryDetail)
		{
			this._DAO = HOCHistoryDetail;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCHistory
		/// </summary>
		public IHOCHistory HOCHistory 
		{
			get
			{
				if (null == _DAO.HOCHistory) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHOCHistory, HOCHistory_DAO>(_DAO.HOCHistory);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HOCHistory = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HOCHistory = (HOCHistory_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.EffectiveDate
		/// </summary>
		public DateTime EffectiveDate 
		{
			get { return _DAO.EffectiveDate; }
			set { _DAO.EffectiveDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.UpdateType
		/// </summary>
		public Char UpdateType 
		{
			get { return _DAO.UpdateType; }
			set { _DAO.UpdateType = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCThatchAmount
		/// </summary>
		public Double? HOCThatchAmount
		{
			get { return _DAO.HOCThatchAmount; }
			set { _DAO.HOCThatchAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCConventionalAmount
		/// </summary>
		public Double? HOCConventionalAmount
		{
			get { return _DAO.HOCConventionalAmount; }
			set { _DAO.HOCConventionalAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCShingleAmount
		/// </summary>
		public Double? HOCShingleAmount
		{
			get { return _DAO.HOCShingleAmount; }
			set { _DAO.HOCShingleAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCProrataPremium
		/// </summary>
		public Double? HOCProrataPremium
		{
			get { return _DAO.HOCProrataPremium; }
			set { _DAO.HOCProrataPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCMonthlyPremium
		/// </summary>
		public Double? HOCMonthlyPremium
		{
			get { return _DAO.HOCMonthlyPremium; }
			set { _DAO.HOCMonthlyPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.PrintDate
		/// </summary>
		public DateTime? PrintDate
		{
			get { return _DAO.PrintDate; }
			set { _DAO.PrintDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.SendFileDate
		/// </summary>
		public DateTime? SendFileDate
		{
			get { return _DAO.SendFileDate; }
			set { _DAO.SendFileDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.ChangeDate
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCAdministrationFee
		/// </summary>
		public Double HOCAdministrationFee 
		{
			get { return _DAO.HOCAdministrationFee; }
			set { _DAO.HOCAdministrationFee = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCBasePremium
		/// </summary>
		public Double HOCBasePremium 
		{
			get { return _DAO.HOCBasePremium; }
			set { _DAO.HOCBasePremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.SASRIAAmount
		/// </summary>
		public Double SASRIAAmount 
		{
			get { return _DAO.SASRIAAmount; }
			set { _DAO.SASRIAAmount = value;}
		}
	}
}


