using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// The LegalEntityAddress_DAO class specifies the Addresses related to a Legal Entity.
	/// </summary>
	public partial interface ILegalEntityAddress : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// Foreign key reference to the Address table where the Address information is stored.
		/// </summary>
		IAddress Address
		{
			get;
			set;
		}
		/// <summary>
		/// Foreign key reference to the AddressType table where the AddressType information is stored. An address can belong
		/// to a single Address Type. e.g. (Residential or Postal)
		/// </summary>
		IAddressType AddressType
		{
			get;
			set;
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// The Foreign Key reference to the Legal Entity table. The relationship between a specific AddressKey and a LegalEntityKey
		/// can only exist once.
		/// </summary>
		ILegalEntity LegalEntity
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityAddress_DAO.LegalEntityDomiciliums
		/// </summary>
		IEventList<ILegalEntityDomicilium> LegalEntityDomiciliums
		{
			get;
		}
	}
}


