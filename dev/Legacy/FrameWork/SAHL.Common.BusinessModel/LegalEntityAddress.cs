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
	/// The LegalEntityAddress_DAO class specifies the Addresses related to a Legal Entity.
	/// </summary>
	public partial class LegalEntityAddress : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityAddress_DAO>, ILegalEntityAddress
	{
				public LegalEntityAddress(SAHL.Common.BusinessModel.DAO.LegalEntityAddress_DAO LegalEntityAddress) : base(LegalEntityAddress)
		{
			this._DAO = LegalEntityAddress;
		}
		/// <summary>
		/// Foreign key reference to the Address table where the Address information is stored.
		/// </summary>
		public IAddress Address 
		{
			get
			{
				if (null == _DAO.Address) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAddress, Address_DAO>(_DAO.Address);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Address = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Address = (Address_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// Foreign key reference to the AddressType table where the AddressType information is stored. An address can belong
		/// to a single Address Type. e.g. (Residential or Postal)
		/// </summary>
		public IAddressType AddressType 
		{
			get
			{
				if (null == _DAO.AddressType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAddressType, AddressType_DAO>(_DAO.AddressType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.AddressType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.AddressType = (AddressType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// The Foreign Key reference to the Legal Entity table. The relationship between a specific AddressKey and a LegalEntityKey
		/// can only exist once.
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
		/// SAHL.Common.BusinessModel.DAO.LegalEntityAddress_DAO.LegalEntityDomiciliums
		/// </summary>
		private DAOEventList<LegalEntityDomicilium_DAO, ILegalEntityDomicilium, LegalEntityDomicilium> _LegalEntityDomiciliums;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityAddress_DAO.LegalEntityDomiciliums
		/// </summary>
		public IEventList<ILegalEntityDomicilium> LegalEntityDomiciliums
		{
			get
			{
				if (null == _LegalEntityDomiciliums) 
				{
					if(null == _DAO.LegalEntityDomiciliums)
						_DAO.LegalEntityDomiciliums = new List<LegalEntityDomicilium_DAO>();
					_LegalEntityDomiciliums = new DAOEventList<LegalEntityDomicilium_DAO, ILegalEntityDomicilium, LegalEntityDomicilium>(_DAO.LegalEntityDomiciliums);
				}
				return _LegalEntityDomiciliums;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_LegalEntityDomiciliums = null;
			
		}
	}
}


