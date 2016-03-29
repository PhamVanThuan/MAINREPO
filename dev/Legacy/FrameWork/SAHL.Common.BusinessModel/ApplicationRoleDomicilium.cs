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
	/// SAHL.Common.BusinessModel.DAO.OfferRoleDomicilium_DAO
	/// </summary>
	public partial class ApplicationRoleDomicilium : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationRoleDomicilium_DAO>, IApplicationRoleDomicilium
	{
		public ApplicationRoleDomicilium(SAHL.Common.BusinessModel.DAO.ApplicationRoleDomicilium_DAO ApplicationRoleDomicilium)
			: base(ApplicationRoleDomicilium)
		{
			this._DAO = ApplicationRoleDomicilium;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OfferRoleDomicilium_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OfferRoleDomicilium_DAO.LegalEntityDomicilium
		/// </summary>
		public ILegalEntityDomicilium LegalEntityDomicilium 
		{
			get
			{
				if (null == _DAO.LegalEntityDomicilium) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntityDomicilium, LegalEntityDomicilium_DAO>(_DAO.LegalEntityDomicilium);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntityDomicilium = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntityDomicilium = (LegalEntityDomicilium_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OfferRoleDomicilium_DAO.ApplicationRole
		/// </summary>
		public IApplicationRole ApplicationRole 
		{
			get
			{
				if (null == _DAO.ApplicationRole) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplicationRole, ApplicationRole_DAO>(_DAO.ApplicationRole);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ApplicationRole = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ApplicationRole = (ApplicationRole_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OfferRoleDomicilium_DAO.ADUser
		/// </summary>
		public IADUser ADUser 
		{
			get
			{
				if (null == _DAO.ADUser) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IADUser, ADUser_DAO>(_DAO.ADUser);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ADUser = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ADUser = (ADUser_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OfferRoleDomicilium_DAO.ChangeDate
		/// </summary>
		public DateTime? ChangeDate
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
	}
}


