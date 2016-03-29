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
	/// OfferRole_DAO is instantiated to represent the different Roles that Legal Entities are playing on the Application.
	/// </summary>
	public partial interface IApplicationRole : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// The date on which the status of the Role was last changed.
		/// </summary>
		System.DateTime StatusChangeDate
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
		/// Each Role belongs to a specific Application Role Type. The Role Types are defined in the OfferRoleType table and include
		/// Insurer, Valuator, Branch Consultant etc.
		/// </summary>
		IApplicationRoleType ApplicationRoleType
		{
			get;
			set;
		}
		/// <summary>
		/// The status of the ApplicationRole either Active or Inactive.
		/// </summary>
		IGeneralStatus GeneralStatus
		{
			get;
			set;
		}
		/// <summary>
		/// The details regarding the Legal Entity playing the Role in the Application is stored in the LegalEntity table. This is
		/// the LegalEntityKey for that Legal Entity.
		/// </summary>
		System.Int32 LegalEntityKey
		{
			get;
			set;
		}
		/// <summary>
		/// The key of the application to which the ApplicationRole belongs.
		/// </summary>
		System.Int32 ApplicationKey
		{
			get;
			set;
		}
		/// <summary>
		/// A collection of role attributes that are defined for this Role.
		/// </summary>
		IEventList<IApplicationRoleAttribute> ApplicationRoleAttributes
		{
			get;
		}
		/// <summary>
		/// A collection of application declarations that are defined for this Role.
		/// </summary>
		IEventList<IApplicationDeclaration> ApplicationDeclarations
		{
			get;
		}
	}
}


