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
	/// SAHL.Common.BusinessModel.DAO.FailedPropertyAddress_DAO
	/// </summary>
	public partial class FailedPropertyAddress : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FailedPropertyAddress_DAO>, IFailedPropertyAddress
	{
				public FailedPropertyAddress(SAHL.Common.BusinessModel.DAO.FailedPropertyAddress_DAO FailedPropertyAddress) : base(FailedPropertyAddress)
		{
			this._DAO = FailedPropertyAddress;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPropertyAddress_DAO.IsCleaned
		/// </summary>
		public Boolean IsCleaned 
		{
			get { return _DAO.IsCleaned; }
			set { _DAO.IsCleaned = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPropertyAddress_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPropertyAddress_DAO.Property
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
		/// SAHL.Common.BusinessModel.DAO.FailedPropertyAddress_DAO.FailedStreetMigration
		/// </summary>
		public IFailedStreetMigration FailedStreetMigration 
		{
			get
			{
				if (null == _DAO.FailedStreetMigration) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFailedStreetMigration, FailedStreetMigration_DAO>(_DAO.FailedStreetMigration);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FailedStreetMigration = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FailedStreetMigration = (FailedStreetMigration_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


