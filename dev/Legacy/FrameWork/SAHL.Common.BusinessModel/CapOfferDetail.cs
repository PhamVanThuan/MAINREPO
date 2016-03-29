using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class CapApplicationDetail : IEntityValidation, ICapApplicationDetail, IDAOObject
	{
		private SAHL.Common.BusinessModel.DAO.CapApplicationDetail_DAO _CapApplicationDetail;
        public CapApplicationDetail(SAHL.Common.BusinessModel.DAO.CapApplicationDetail_DAO CapApplicationDetail)
		{
			this._CapApplicationDetail = CapApplicationDetail;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns><see cref="CapApplicationDetail_DAO"/></returns>		
		public object GetDAOObject()
		{
			return _CapApplicationDetail;
		}
		/// <summary>
		/// 
		/// </summary>
		public Double EffectiveRate 
		{
			get { return _CapApplicationDetail.EffectiveRate; }
			set { _CapApplicationDetail.EffectiveRate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double Payment 
		{
			get { return _CapApplicationDetail.Payment; }
			set { _CapApplicationDetail.Payment = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double Fee 
		{
			get { return _CapApplicationDetail.Fee; }
			set { _CapApplicationDetail.Fee = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AcceptanceDate
		{
			get { return _CapApplicationDetail.AcceptanceDate; }
			set { _CapApplicationDetail.AcceptanceDate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CapNTUReasonDate
		{
			get { return _CapApplicationDetail.CapNTUReasonDate; }
			set { _CapApplicationDetail.CapNTUReasonDate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _CapApplicationDetail.ChangeDate; }
			set { _CapApplicationDetail.ChangeDate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String UserID 
		{
			get { return _CapApplicationDetail.UserID; }
			set { _CapApplicationDetail.UserID = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32 Key 
		{
			get { return _CapApplicationDetail.Key; }
			set { _CapApplicationDetail.Key = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public ICapNTUReason CapNTUReason 
		{
			get
			{
				if (null == _CapApplicationDetail.CapNTUReason) return null;
				{
				IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
				return BMTM.GetMappedType<ICapNTUReason, CapNTUReason_DAO>(_CapApplicationDetail.CapNTUReason);
				}

			}

			set
			{
				if(value == null)
				{
					_CapApplicationDetail.CapNTUReason = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_CapApplicationDetail.CapNTUReason = (CapNTUReason_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public ICapApplication CapApplication 
		{
			get
			{
				if (null == _CapApplicationDetail.CapApplication) return null;
				{
				IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
				return BMTM.GetMappedType<ICapApplication, CapApplication_DAO>(_CapApplicationDetail.CapApplication);
				}

			}

			set
			{
				if(value == null)
				{
					_CapApplicationDetail.CapApplication = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_CapApplicationDetail.CapApplication = (CapApplication_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public ICapStatus CapStatus 
		{
			get
			{
				if (null == _CapApplicationDetail.CapStatus) return null;
				{
				IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
				return BMTM.GetMappedType<ICapStatus, CapStatus_DAO>(_CapApplicationDetail.CapStatus);
				}

			}

			set
			{
				if(value == null)
				{
					_CapApplicationDetail.CapStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_CapApplicationDetail.CapStatus = (CapStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public ICapTypeConfigurationDetail CapTypeConfigurationDetail 
		{
			get
			{
				if (null == _CapApplicationDetail.CapTypeConfigurationDetail) return null;
				{
				IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
				return BMTM.GetMappedType<ICapTypeConfigurationDetail, CapTypeConfigurationDetail_DAO>(_CapApplicationDetail.CapTypeConfigurationDetail);
				}

			}

			set
			{
				if(value == null)
				{
					_CapApplicationDetail.CapTypeConfigurationDetail = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_CapApplicationDetail.CapTypeConfigurationDetail = (CapTypeConfigurationDetail_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


