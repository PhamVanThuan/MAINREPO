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
	/// SAHL.Common.BusinessModel.DAO.SubsidyProvider_DAO
	/// </summary>
	public partial class SubsidyProvider : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SubsidyProvider_DAO>, ISubsidyProvider
	{
				public SubsidyProvider(SAHL.Common.BusinessModel.DAO.SubsidyProvider_DAO SubsidyProvider) : base(SubsidyProvider)
		{
			this._DAO = SubsidyProvider;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SubsidyProvider_DAO.PersalOrganisationCode
		/// </summary>
		public String PersalOrganisationCode 
		{
			get { return _DAO.PersalOrganisationCode; }
			set { _DAO.PersalOrganisationCode = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SubsidyProvider_DAO.ContactPerson
		/// </summary>
		public String ContactPerson 
		{
			get { return _DAO.ContactPerson; }
			set { _DAO.ContactPerson = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SubsidyProvider_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SubsidyProvider_DAO.ChangeDate
		/// </summary>
		public DateTime? ChangeDate
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SubsidyProvider_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SubsidyProvider_DAO.LegalEntity
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
		/// SAHL.Common.BusinessModel.DAO.SubsidyProvider_DAO.SubsidyProviderType
		/// </summary>
		public ISubsidyProviderType SubsidyProviderType 
		{
			get
			{
				if (null == _DAO.SubsidyProviderType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ISubsidyProviderType, SubsidyProviderType_DAO>(_DAO.SubsidyProviderType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.SubsidyProviderType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.SubsidyProviderType = (SubsidyProviderType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SubsidyProvider_DAO.GEPFAffiliate
        /// </summary>
        public bool GEPFAffiliate
        {
            get { return _DAO.GEPFAffiliate; }
            set { _DAO.GEPFAffiliate = value; }
        }
    }
}


