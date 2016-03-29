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
	/// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO
	/// </summary>
	public partial class HearingDetail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.HearingDetail_DAO>, IHearingDetail
	{
				public HearingDetail(SAHL.Common.BusinessModel.DAO.HearingDetail_DAO HearingDetail) : base(HearingDetail)
		{
			this._DAO = HearingDetail;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.CaseNumber
		/// </summary>
		public String CaseNumber 
		{
			get { return _DAO.CaseNumber; }
			set { _DAO.CaseNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.HearingDate
		/// </summary>
		public DateTime HearingDate 
		{
			get { return _DAO.HearingDate; }
			set { _DAO.HearingDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.DebtCounselling
		/// </summary>
		public IDebtCounselling DebtCounselling 
		{
			get
			{
				if (null == _DAO.DebtCounselling) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDebtCounselling, DebtCounselling_DAO>(_DAO.DebtCounselling);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DebtCounselling = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DebtCounselling = (DebtCounselling_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.HearingType
		/// </summary>
		public IHearingType HearingType 
		{
			get
			{
				if (null == _DAO.HearingType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHearingType, HearingType_DAO>(_DAO.HearingType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HearingType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HearingType = (HearingType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.Court
		/// </summary>
		public ICourt Court 
		{
			get
			{
				if (null == _DAO.Court) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICourt, Court_DAO>(_DAO.Court);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Court = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Court = (Court_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.HearingAppearanceType
		/// </summary>
		public IHearingAppearanceType HearingAppearanceType 
		{
			get
			{
				if (null == _DAO.HearingAppearanceType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHearingAppearanceType, HearingAppearanceType_DAO>(_DAO.HearingAppearanceType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HearingAppearanceType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HearingAppearanceType = (HearingAppearanceType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.Comment
		/// </summary>
		public String Comment 
		{
			get { return _DAO.Comment; }
			set { _DAO.Comment = value;}
		}
	}
}


