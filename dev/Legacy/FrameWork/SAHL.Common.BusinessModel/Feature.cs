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
	/// SAHL.Common.BusinessModel.DAO.Feature_DAO
	/// </summary>
	public partial class Feature : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Feature_DAO>, IFeature
	{
				public Feature(SAHL.Common.BusinessModel.DAO.Feature_DAO Feature) : base(Feature)
		{
			this._DAO = Feature;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Feature_DAO.ShortName
		/// </summary>
		public String ShortName 
		{
			get { return _DAO.ShortName; }
			set { _DAO.ShortName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Feature_DAO.LongName
		/// </summary>
		public String LongName 
		{
			get { return _DAO.LongName; }
			set { _DAO.LongName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Feature_DAO.HasAccess
		/// </summary>
		public Boolean HasAccess 
		{
			get { return _DAO.HasAccess; }
			set { _DAO.HasAccess = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Feature_DAO.Sequence
		/// </summary>
		public Int32 Sequence 
		{
			get { return _DAO.Sequence; }
			set { _DAO.Sequence = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Feature_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Feature_DAO.ChildFeatures
		/// </summary>
		private DAOEventList<Feature_DAO, IFeature, Feature> _ChildFeatures;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Feature_DAO.ChildFeatures
		/// </summary>
		public IEventList<IFeature> ChildFeatures
		{
			get
			{
				if (null == _ChildFeatures) 
				{
					if(null == _DAO.ChildFeatures)
						_DAO.ChildFeatures = new List<Feature_DAO>();
					_ChildFeatures = new DAOEventList<Feature_DAO, IFeature, Feature>(_DAO.ChildFeatures);
					_ChildFeatures.BeforeAdd += new EventListHandler(OnChildFeatures_BeforeAdd);					
					_ChildFeatures.BeforeRemove += new EventListHandler(OnChildFeatures_BeforeRemove);					
					_ChildFeatures.AfterAdd += new EventListHandler(OnChildFeatures_AfterAdd);					
					_ChildFeatures.AfterRemove += new EventListHandler(OnChildFeatures_AfterRemove);					
				}
				return _ChildFeatures;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Feature_DAO.ParentFeature
		/// </summary>
		public IFeature ParentFeature 
		{
			get
			{
				if (null == _DAO.ParentFeature) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFeature, Feature_DAO>(_DAO.ParentFeature);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ParentFeature = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ParentFeature = (Feature_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Feature_DAO.FeatureGroups
		/// </summary>
		private DAOEventList<FeatureGroup_DAO, IFeatureGroup, FeatureGroup> _FeatureGroups;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Feature_DAO.FeatureGroups
		/// </summary>
		public IEventList<IFeatureGroup> FeatureGroups
		{
			get
			{
				if (null == _FeatureGroups) 
				{
					if(null == _DAO.FeatureGroups)
						_DAO.FeatureGroups = new List<FeatureGroup_DAO>();
					_FeatureGroups = new DAOEventList<FeatureGroup_DAO, IFeatureGroup, FeatureGroup>(_DAO.FeatureGroups);
					_FeatureGroups.BeforeAdd += new EventListHandler(OnFeatureGroups_BeforeAdd);					
					_FeatureGroups.BeforeRemove += new EventListHandler(OnFeatureGroups_BeforeRemove);					
					_FeatureGroups.AfterAdd += new EventListHandler(OnFeatureGroups_AfterAdd);					
					_FeatureGroups.AfterRemove += new EventListHandler(OnFeatureGroups_AfterRemove);					
				}
				return _FeatureGroups;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ChildFeatures = null;
			_FeatureGroups = null;
			
		}
	}
}


