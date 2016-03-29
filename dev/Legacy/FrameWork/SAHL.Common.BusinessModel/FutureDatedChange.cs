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
	/// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO
	/// </summary>
	public partial class FutureDatedChange : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO>, IFutureDatedChange
	{
				public FutureDatedChange(SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO FutureDatedChange) : base(FutureDatedChange)
		{
			this._DAO = FutureDatedChange;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.IdentifierReferenceKey
		/// </summary>
		public Int32 IdentifierReferenceKey 
		{
			get { return _DAO.IdentifierReferenceKey; }
			set { _DAO.IdentifierReferenceKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.EffectiveDate
		/// </summary>
		public DateTime EffectiveDate 
		{
			get { return _DAO.EffectiveDate; }
			set { _DAO.EffectiveDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.NotificationRequired
		/// </summary>
		public Boolean NotificationRequired 
		{
			get { return _DAO.NotificationRequired; }
			set { _DAO.NotificationRequired = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.InsertDate
		/// </summary>
		public DateTime InsertDate 
		{
			get { return _DAO.InsertDate; }
			set { _DAO.InsertDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.ChangeDate
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.FutureDatedChangeDetails
		/// </summary>
		private DAOEventList<FutureDatedChangeDetail_DAO, IFutureDatedChangeDetail, FutureDatedChangeDetail> _FutureDatedChangeDetails;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.FutureDatedChangeDetails
		/// </summary>
		public IEventList<IFutureDatedChangeDetail> FutureDatedChangeDetails
		{
			get
			{
				if (null == _FutureDatedChangeDetails) 
				{
					if(null == _DAO.FutureDatedChangeDetails)
						_DAO.FutureDatedChangeDetails = new List<FutureDatedChangeDetail_DAO>();
					_FutureDatedChangeDetails = new DAOEventList<FutureDatedChangeDetail_DAO, IFutureDatedChangeDetail, FutureDatedChangeDetail>(_DAO.FutureDatedChangeDetails);
					_FutureDatedChangeDetails.BeforeAdd += new EventListHandler(OnFutureDatedChangeDetails_BeforeAdd);					
					_FutureDatedChangeDetails.BeforeRemove += new EventListHandler(OnFutureDatedChangeDetails_BeforeRemove);					
					_FutureDatedChangeDetails.AfterAdd += new EventListHandler(OnFutureDatedChangeDetails_AfterAdd);					
					_FutureDatedChangeDetails.AfterRemove += new EventListHandler(OnFutureDatedChangeDetails_AfterRemove);					
				}
				return _FutureDatedChangeDetails;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.FutureDatedChangeType
		/// </summary>
		public IFutureDatedChangeType FutureDatedChangeType 
		{
			get
			{
				if (null == _DAO.FutureDatedChangeType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFutureDatedChangeType, FutureDatedChangeType_DAO>(_DAO.FutureDatedChangeType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FutureDatedChangeType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FutureDatedChangeType = (FutureDatedChangeType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_FutureDatedChangeDetails = null;
			
		}
	}
}


