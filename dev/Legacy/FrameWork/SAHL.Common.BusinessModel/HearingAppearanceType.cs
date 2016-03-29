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
	/// SAHL.Common.BusinessModel.DAO.HearingAppearanceType_DAO
	/// </summary>
	public partial class HearingAppearanceType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.HearingAppearanceType_DAO>, IHearingAppearanceType
	{
				public HearingAppearanceType(SAHL.Common.BusinessModel.DAO.HearingAppearanceType_DAO HearingAppearanceType) : base(HearingAppearanceType)
		{
			this._DAO = HearingAppearanceType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HearingAppearanceType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HearingAppearanceType_DAO.HearingType
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
		/// SAHL.Common.BusinessModel.DAO.HearingAppearanceType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
	}
}


