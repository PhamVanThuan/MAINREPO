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
	/// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO
	/// </summary>
	public partial class HOCHistory : BusinessModelBase<SAHL.Common.BusinessModel.DAO.HOCHistory_DAO>, IHOCHistory
	{
				public HOCHistory(SAHL.Common.BusinessModel.DAO.HOCHistory_DAO HOCHistory) : base(HOCHistory)
		{
			this._DAO = HOCHistory;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.CommencementDate
		/// </summary>
		public DateTime? CommencementDate
		{
			get { return _DAO.CommencementDate; }
			set { _DAO.CommencementDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.CancellationDate
		/// </summary>
		public DateTime? CancellationDate
		{
			get { return _DAO.CancellationDate; }
			set { _DAO.CancellationDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.ChangeDate
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.HOCHistoryDetails
		/// </summary>
		private DAOEventList<HOCHistoryDetail_DAO, IHOCHistoryDetail, HOCHistoryDetail> _HOCHistoryDetails;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.HOCHistoryDetails
		/// </summary>
		public IEventList<IHOCHistoryDetail> HOCHistoryDetails
		{
			get
			{
				if (null == _HOCHistoryDetails) 
				{
					if(null == _DAO.HOCHistoryDetails)
						_DAO.HOCHistoryDetails = new List<HOCHistoryDetail_DAO>();
					_HOCHistoryDetails = new DAOEventList<HOCHistoryDetail_DAO, IHOCHistoryDetail, HOCHistoryDetail>(_DAO.HOCHistoryDetails);
					_HOCHistoryDetails.BeforeAdd += new EventListHandler(OnHOCHistoryDetails_BeforeAdd);					
					_HOCHistoryDetails.BeforeRemove += new EventListHandler(OnHOCHistoryDetails_BeforeRemove);					
					_HOCHistoryDetails.AfterAdd += new EventListHandler(OnHOCHistoryDetails_AfterAdd);					
					_HOCHistoryDetails.AfterRemove += new EventListHandler(OnHOCHistoryDetails_AfterRemove);					
				}
				return _HOCHistoryDetails;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.HOC
		/// </summary>
		public IHOC HOC 
		{
			get
			{
				if (null == _DAO.HOC) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHOC, HOC_DAO>(_DAO.HOC);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HOC = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HOC = (HOC_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.HOCInsurer
		/// </summary>
		public IHOCInsurer HOCInsurer 
		{
			get
			{
				if (null == _DAO.HOCInsurer) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHOCInsurer, HOCInsurer_DAO>(_DAO.HOCInsurer);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HOCInsurer = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HOCInsurer = (HOCInsurer_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_HOCHistoryDetails = null;
			
		}
	}
}


