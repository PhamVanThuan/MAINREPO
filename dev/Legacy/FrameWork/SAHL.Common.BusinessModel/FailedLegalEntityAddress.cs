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
	/// SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO
	/// </summary>
	public partial class FailedLegalEntityAddress : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO>, IFailedLegalEntityAddress
	{
				public FailedLegalEntityAddress(SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO FailedLegalEntityAddress) : base(FailedLegalEntityAddress)
		{
			this._DAO = FailedLegalEntityAddress;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO.IsCleaned
		/// </summary>
		public Boolean IsCleaned 
		{
			get { return _DAO.IsCleaned; }
			set { _DAO.IsCleaned = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO.PostalIsCleaned
		/// </summary>
		public Boolean PostalIsCleaned 
		{
			get { return _DAO.PostalIsCleaned; }
			set { _DAO.PostalIsCleaned = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO.LegalEntity
		/// </summary>
		public ILegalEntity LegalEntity 
		{
			get
			{
				if (null == _DAO.LegalEntity) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntity = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO.FailedPostalMigration
		/// </summary>
		public IFailedPostalMigration FailedPostalMigration 
		{
			get
			{
				if (null == _DAO.FailedPostalMigration) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFailedPostalMigration, FailedPostalMigration_DAO>(_DAO.FailedPostalMigration);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FailedPostalMigration = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FailedPostalMigration = (FailedPostalMigration_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO.FailedStreetMigration
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


