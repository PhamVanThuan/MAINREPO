
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO
	/// </summary>
	public partial interface ILegalEntity_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.LegalEntityTypeKey
		/// </summary>
		System.Int32 LegalEntityTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.MaritalStatusKey
		/// </summary>
		System.Int32 MaritalStatusKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.GenderKey
		/// </summary>
		System.Int32 GenderKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.PopulationGroupKey
		/// </summary>
		System.Int32 PopulationGroupKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.IntroductionDate
		/// </summary>
		System.DateTime? IntroductionDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.Salutationkey
		/// </summary>
		System.Int32 Salutationkey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.FirstNames
		/// </summary>
		System.String FirstNames
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.Initials
		/// </summary>
		System.String Initials
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.Surname
		/// </summary>
		System.String Surname
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.PreferredName
		/// </summary>
		System.String PreferredName
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.IDNumber
		/// </summary>
		System.String IDNumber
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.PassportNumber
		/// </summary>
		System.String PassportNumber
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.TaxNumber
		/// </summary>
		System.String TaxNumber
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.RegistrationNumber
		/// </summary>
		System.String RegistrationNumber
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.RegisteredName
		/// </summary>
		System.String RegisteredName
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.TradingName
		/// </summary>
		System.String TradingName
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.DateOfBirth
		/// </summary>
		System.DateTime? DateOfBirth
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.HomePhoneCode
		/// </summary>
		System.String HomePhoneCode
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.HomePhoneNumber
		/// </summary>
		System.String HomePhoneNumber
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.WorkPhoneCode
		/// </summary>
		System.String WorkPhoneCode
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.WorkPhoneNumber
		/// </summary>
		System.String WorkPhoneNumber
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.CellPhoneNumber
		/// </summary>
		System.String CellPhoneNumber
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.EmailAddress
		/// </summary>
		System.String EmailAddress
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.FaxCode
		/// </summary>
		System.String FaxCode
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.FaxNumber
		/// </summary>
		System.String FaxNumber
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.Password
		/// </summary>
		System.String Password
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.CitizenTypeKey
		/// </summary>
		System.Int32 CitizenTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.LegalEntityStatusKey
		/// </summary>
		System.Int32 LegalEntityStatusKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.Comments
		/// </summary>
		System.String Comments
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.LegalEntityExceptionStatusKey
		/// </summary>
		System.Int32 LegalEntityExceptionStatusKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.UserID
		/// </summary>
		System.String UserID
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.ChangeDate
		/// </summary>
		System.DateTime? ChangeDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.EducationKey
		/// </summary>
		System.Int32 EducationKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.HomeLanguageKey
		/// </summary>
		System.Int32 HomeLanguageKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.DocumentLanguageKey
		/// </summary>
		System.Int32 DocumentLanguageKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.ResidenceStatusKey
		/// </summary>
		System.Int32 ResidenceStatusKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.ADUsers
		/// </summary>
		IEventList<IADUser_WTF> ADUsers
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.ApplicationRoles
		/// </summary>
		IEventList<IApplicationRole_WTF> ApplicationRoles
		{
			get;
		}
	}
}



