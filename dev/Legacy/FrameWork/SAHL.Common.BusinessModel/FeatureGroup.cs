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
	/// SAHL.Common.BusinessModel.DAO.FeatureGroup_DAO
	/// </summary>
	public partial class FeatureGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FeatureGroup_DAO>, IFeatureGroup
	{
				public FeatureGroup(SAHL.Common.BusinessModel.DAO.FeatureGroup_DAO FeatureGroup) : base(FeatureGroup)
		{
			this._DAO = FeatureGroup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FeatureGroup_DAO.ADUserGroup
		/// </summary>
		public String ADUserGroup 
		{
			get { return _DAO.ADUserGroup; }
			set { _DAO.ADUserGroup = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FeatureGroup_DAO.Feature
		/// </summary>
		public IFeature Feature 
		{
			get
			{
				if (null == _DAO.Feature) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFeature, Feature_DAO>(_DAO.Feature);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Feature = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Feature = (Feature_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FeatureGroup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


