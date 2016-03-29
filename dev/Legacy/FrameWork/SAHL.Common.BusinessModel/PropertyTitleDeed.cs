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
	/// SAHL.Common.BusinessModel.DAO.PropertyTitleDeed_DAO
	/// </summary>
	public partial class PropertyTitleDeed : BusinessModelBase<SAHL.Common.BusinessModel.DAO.PropertyTitleDeed_DAO>, IPropertyTitleDeed
	{
				public PropertyTitleDeed(SAHL.Common.BusinessModel.DAO.PropertyTitleDeed_DAO PropertyTitleDeed) : base(PropertyTitleDeed)
		{
			this._DAO = PropertyTitleDeed;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyTitleDeed_DAO.TitleDeedNumber
		/// </summary>
		public String TitleDeedNumber 
		{
			get { return _DAO.TitleDeedNumber; }
			set { _DAO.TitleDeedNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyTitleDeed_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyTitleDeed_DAO.Property
		/// </summary>
		public IProperty Property 
		{
			get
			{
				if (null == _DAO.Property) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IProperty, Property_DAO>(_DAO.Property);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Property = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Property = (Property_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// Gets/sets the deeds office to which the title deed belongs.
		/// </summary>
		public IDeedsOffice DeedsOffice 
		{
			get
			{
				if (null == _DAO.DeedsOffice) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDeedsOffice, DeedsOffice_DAO>(_DAO.DeedsOffice);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DeedsOffice = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DeedsOffice = (DeedsOffice_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


