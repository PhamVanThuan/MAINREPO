using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web;
using SAHL.Common;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.LegalEntity
{
	/// <summary>
	/// When one or more legal entities has a marital status of type COP, there must be another legal
	/// entity with a marital status of COP. The following objects may be passed.
	/// <list type="bullet">
	///     <item>
	///         <description>ILegalEntityNaturalPerson</description>
	///         <description>IMortgageLoanAccount</description>
	///     </item>
	/// </list>
	/// </summary>
	[RuleInfo]
	[RuleDBTag("LegalEntityNaturalPersonCOP",
	"When one or more Legal Entities has a marital status of type COP, there must be another Legal Entity with the same marital status.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonCOP")]
	public class LegalEntityNaturalPersonCOP : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The LegalEntityNaturalPersonCOP rule expects a Domain object to be passed.");

			if (!(Parameters[0] is ILegalEntityNaturalPerson
				|| Parameters[0] is IApplicationMortgageLoan))
				throw new ArgumentException("The LegalEntityNaturalPersonCOP rule expects the following objects to be passed: ILegalEntityNaturalPerson, IMortgageLoanAccount, IApplicationMortgageLoan.");

			#endregion Check for allowed object type(s)

			#region Run the RuleItem.

			ILegalEntityNaturalPerson legalEntityNaturalPerson = Parameters[0] as ILegalEntityNaturalPerson;

			if (legalEntityNaturalPerson != null && legalEntityNaturalPerson.MaritalStatus != null)
			{
				if (legalEntityNaturalPerson.MaritalStatus.Key == (int)MaritalStatuses.MarriedCommunityofProperty)
				{
					// There must be another LE with COP.
					// For each account teh LE plays a role in.
					foreach (IRole roles in legalEntityNaturalPerson.Roles)
					{
						if (roles.Account.AccountStatus.Key == (int)AccountStatuses.Open && roles.Account is IMortgageLoanAccount)
						{
							bool partnerExists = false;

							// on this account
							foreach (IRole accountRoles in roles.Account.Roles)
							{
								// find an LE other that current that has COP marital status
								if (accountRoles.LegalEntity.Key != legalEntityNaturalPerson.Key
									&& accountRoles.LegalEntity is ILegalEntityNaturalPerson
									&& ((ILegalEntityNaturalPerson)accountRoles.LegalEntity).MaritalStatus.Key == (int)MaritalStatuses.MarriedCommunityofProperty)
								{
									// JM Added - must check that if one LE with COP then both applicants must be lead applicants.
									if (accountRoles.RoleType.Key != (int)RoleTypes.MainApplicant)
									{
										string errorMsg = "Legal Entity must have a spouse that is also a Lead Applicant";
										AddMessage(errorMsg, errorMsg, Messages);
										return 0;
									}
									partnerExists = true;
								}
							}

							if (!partnerExists)
							{
								AddMessage("Legal Entity must have a spouse married in community of property", "", Messages);
								break;
							}
						}
					}
				}
			}

			// Added by SS - 29/08/2008
			IApplicationMortgageLoan appML = Parameters[0] as IApplicationMortgageLoan;

			if (appML != null)
			{
				int nbrOfApplicantsMarriedInCOP = 0;

				// JM removed the RoleTypes Suretor, LeadSuretor
				//int[] roles = { (int)SAHL.Common.Globals.OfferRoleTypes.MainApplicant, (int)SAHL.Common.Globals.OfferRoleTypes.Suretor, (int)OfferRoleTypes.LeadSuretor, (int)OfferRoleTypes.LeadMainApplicant };
				int[] roles = { (int)SAHL.Common.Globals.OfferRoleTypes.MainApplicant, (int)OfferRoleTypes.LeadMainApplicant };

				IReadOnlyEventList<ILegalEntityNaturalPerson> lstLegalEntities = appML.GetNaturalPersonLegalEntitiesByRoleType(Messages, roles, GeneralStatusKey.Active);

				for (int i = 0; i < lstLegalEntities.Count; i++)
				{
					if (lstLegalEntities[i].MaritalStatus != null && lstLegalEntities[i].MaritalStatus.Key == (int)MaritalStatuses.MarriedCommunityofProperty)
					{
						nbrOfApplicantsMarriedInCOP += 1;
					}
				}

				if ((nbrOfApplicantsMarriedInCOP % 2) > 0)
					AddMessage("Legal Entity must have a spouse married in community of property and must be a Main Applicant", "", Messages);
			}

			#endregion Run the RuleItem.

			return 0;
		}
	}

	[RuleDBTag("LegalEntityNaturalPersonMinAge",
	"The minimum age for a Legal Entity is 18 years.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMinAge")]
	[RuleParameterTag(new string[] { "@MinimumAge,18,9" })]
	[RuleInfo]
	public class LegalEntityNaturalPersonMinAge : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Setup - Check for allowed object type(s) and parameters

			if (Parameters.Length == 0)
				throw new ArgumentException("The LegalEntityNaturalPersonMinAge rule expects a Domain object to be passed.");

			if (!(Parameters[0] is ILegalEntityNaturalPerson
				|| Parameters[0] is IApplicationMortgageLoan))
				throw new ArgumentException("The LegalEntityNaturalPersonMinAge rule expects the following object(s) to be passed: ILegalEntityNaturalPerson.");

			// Expecting one parameter else fail.
			if (RuleItem.RuleParameters.Count < 1)
				throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

			int minimumAge = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

			#endregion Setup - Check for allowed object type(s) and parameters

			if (Parameters[0] is ILegalEntityNaturalPerson)
			{
				ILegalEntityNaturalPerson legalEntityNaturalPerson = (ILegalEntityNaturalPerson)Parameters[0];

				if (legalEntityNaturalPerson.AgeNextBirthday < minimumAge)
					AddMessage(String.Format("The minimum age for a legal entity is {0} years.", minimumAge), "", Messages);
			}

			IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

			if (applicationMortgageLoan != null)
			{
				foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
				{
					if (applicationRole.LegalEntity is ILegalEntityNaturalPerson
						&& applicationRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client)
					{
						ILegalEntityNaturalPerson legalEntityNaturalPerson = (ILegalEntityNaturalPerson)applicationRole.LegalEntity;

						if (legalEntityNaturalPerson.AgeNextBirthday < minimumAge)
							AddMessage(String.Format("{0}: The minimum age for a legal entity is {1} years.", legalEntityNaturalPerson.GetLegalName(LegalNameFormat.Full), minimumAge), "", Messages);
					}
				}

				// Commented out by SS on 13/05/2008 - this rule is a duplicate - already covered in LegalEntityNaturalPersonCOP
				//// COP partners must be 0 or even.
				//int partnersCount = 0;

				//foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
				//{
				//    if (applicationRole.LegalEntity is ILegalEntityNaturalPerson)
				//    {
				//        if (applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active
				//            && ((ILegalEntityNaturalPerson)applicationRole.LegalEntity).MaritalStatus != null
				//            && ((ILegalEntityNaturalPerson)applicationRole.LegalEntity).MaritalStatus.Key == (int)MaritalStatuses.MarriedCommunityofProperty
				//            && applicationRole.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client)
				//        {
				//            partnersCount++;
				//        }
				//    }
				//}

				//int evenNumber = 2,
				//    quotient;

				//if (Math.DivRem(partnersCount, evenNumber, out quotient) != 0)  // not even
				//{
				//    AddMessage("All Legal Entities with the Marital Status of COP must have a spouse with the same Marital Status.", "", Messages);
				//}
			}

			return 1;
		}
	}

	[RuleInfo]
	[RuleDBTag("LegalEntityNaturalPersonIDNumberDOB",
	"???",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonIDNumberDOB")]
	public class LegalEntityNaturalPersonIDNumberDOB : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Setup - Check for allowed object type(s) and parameters

			if (Parameters.Length == 0)
				throw new ArgumentException("The LegalEntityNaturalPersonIDNumberDOB rule expects a Domain object to be passed.");

			if (!(Parameters[0] is ILegalEntityNaturalPerson))
				throw new ArgumentException("The LegalEntityNaturalPersonIDNumberDOB rule expects the following object(s) to be passed: ILegalEntityNaturalPerson.");

			ILegalEntityNaturalPerson legalEntityNaturalPerson = (ILegalEntityNaturalPerson)Parameters[0];

			#endregion Setup - Check for allowed object type(s) and parameters

			#region Rule logic begins

			if ((legalEntityNaturalPerson.CitizenType.Key == (int)CitizenTypes.SACitizen || legalEntityNaturalPerson.CitizenType.Key == (int)CitizenTypes.SACitizenNonResident)
					&& !legalEntityNaturalPerson.IDNumber.Substring(0, 6).Equals(legalEntityNaturalPerson.DateOfBirth.Value.ToString("yyMMdd")))
			{
				AddMessage("The ID Number should correspond with the Date of Birth.", "", Messages);
			}

			#endregion Rule logic begins

			return 0;
		}
	}

	/// <summary>
	/// Determines if a legal entity has any open accounts. Certain fields may not be updated unless an exception status has been loaded.
	/// Takes the following parameters.
	/// <list type="bullet">
	///     <item>
	///         <description>ILegalEntity before the changes.</description>
	///         <description>ILegalEntity after the changes.</description>
	///     </item>
	/// </list>
	/// </summary>
	[RuleInfo]
	[RuleDBTag("DetermineLegalEntityUpdateOpenAccount",
	"May not update a legal entity that is connected to an open account unless an Invalid IDNumber Exception status is loaded on the legal entity.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.LegalEntity.DetermineLegalEntityUpdateOpenAccount")]
	public class DetermineLegalEntityUpdateOpenAccount : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			bool isLocked = false;

			#region Setup - Check for allowed object type(s) and parameters (Expects)

			if (Parameters.Length < 2)
				throw new ArgumentException("The DetermineLegalEntityUpdateOpenAccount rule expects 2 parameters to be passed.");

			if (!(Parameters[0] is ILegalEntity))
				throw new ArgumentException("The DetermineLegalEntityUpdateOpenAccount rule expects the following object(s) to be passed: ILegalEntity.");

			if (!(Parameters[1] is ILegalEntity))
				throw new ArgumentException("The DetermineLegalEntityUpdateOpenAccount rule expects the following object(s) to be passed: ILegalEntity.");

			ILegalEntity legalEntityBefore = (ILegalEntity)Parameters[0];
			ILegalEntity legalEntityAfter = (ILegalEntity)Parameters[1];

			// Gotta be the same legal entity
			if (legalEntityAfter.Key != legalEntityBefore.Key)
				throw new ArgumentException("The rule expects data for the same legal entity.");

			#endregion Setup - Check for allowed object type(s) and parameters (Expects)

			#region Rule logic begins

			if (legalEntityBefore.LegalEntityExceptionStatus == null
				|| legalEntityBefore.LegalEntityExceptionStatus.Key == (int)LegalEntityExceptionStatuses.InvalidIDNumber)
			{
				foreach (IRole role in legalEntityBefore.Roles)
				{
					if (role.Account.IsActive)
					{
						isLocked = true;
						break;
					}
				}
			}

			if (isLocked)
			{
				if (legalEntityAfter is ILegalEntityNaturalPerson && legalEntityBefore is ILegalEntityNaturalPerson)
				{
					ILegalEntityNaturalPerson legalEntityNaturalPersonAfter = (ILegalEntityNaturalPerson)legalEntityAfter;
					ILegalEntityNaturalPerson legalEntityNaturalPersonBefore = (ILegalEntityNaturalPerson)legalEntityBefore;

					HelperCompareProperties<int>(legalEntityNaturalPersonAfter.Salutation.Key, legalEntityNaturalPersonBefore.Salutation.Key, "Salutation", Messages);
					HelperCompareProperties<string>(legalEntityNaturalPersonAfter.Initials, legalEntityNaturalPersonBefore.Initials, "Initials", Messages);
					HelperCompareProperties<string>(legalEntityNaturalPersonAfter.FirstNames, legalEntityNaturalPersonBefore.FirstNames, "First Names", Messages);
					HelperCompareProperties<string>(legalEntityNaturalPersonAfter.Surname, legalEntityNaturalPersonBefore.Surname, "Surname", Messages);
					HelperCompareProperties<int>(legalEntityNaturalPersonAfter.Gender.Key, legalEntityNaturalPersonBefore.Gender.Key, "Gender", Messages);
					HelperCompareProperties<int>(legalEntityNaturalPersonAfter.MaritalStatus.Key, legalEntityNaturalPersonBefore.MaritalStatus.Key, "Marital Status", Messages);
					HelperCompareProperties<int>(legalEntityNaturalPersonAfter.CitizenType.Key, legalEntityNaturalPersonBefore.CitizenType.Key, "Citizenship Type", Messages);
					HelperCompareProperties<string>(legalEntityNaturalPersonAfter.IDNumber, legalEntityNaturalPersonBefore.IDNumber, "ID Number", Messages);

					if (legalEntityNaturalPersonAfter.CitizenType.Key != (int)CitizenTypes.SACitizen && legalEntityNaturalPersonAfter.CitizenType.Key != (int)CitizenTypes.SACitizenNonResident)
						HelperCompareProperties<string>(legalEntityNaturalPersonAfter.PassportNumber, legalEntityNaturalPersonBefore.PassportNumber, "Passport Number", Messages);
				}

				if (legalEntityAfter is ILegalEntityTrust && legalEntityBefore is ILegalEntityTrust)
				{
					ILegalEntityTrust legalEntityTrustAfter = (ILegalEntityTrust)legalEntityAfter;
					ILegalEntityTrust legalEntityTrustBefore = (ILegalEntityTrust)legalEntityBefore;

					HelperCompareProperties<string>(legalEntityTrustAfter.RegisteredName, legalEntityTrustBefore.RegisteredName, "Registered Name", Messages);
					HelperCompareProperties<string>(legalEntityTrustAfter.RegistrationNumber, legalEntityTrustBefore.RegistrationNumber, "Registration Number", Messages);
				}

				if (legalEntityAfter is ILegalEntityCompany && legalEntityBefore is ILegalEntityCompany)
				{
					ILegalEntityCompany legalEntityCompanyAfter = (ILegalEntityCompany)legalEntityAfter;
					ILegalEntityCompany legalEntityCompanyBefore = (ILegalEntityCompany)legalEntityBefore;

					HelperCompareProperties<string>(legalEntityCompanyAfter.RegisteredName, legalEntityCompanyBefore.RegisteredName, "Registered Name", Messages);
					HelperCompareProperties<string>(legalEntityCompanyAfter.RegistrationNumber, legalEntityCompanyBefore.RegistrationNumber, "Registration Number", Messages);
				}

				if (legalEntityAfter is ILegalEntityCloseCorporation && legalEntityBefore is ILegalEntityCloseCorporation)
				{
					ILegalEntityCloseCorporation legalEntityCloseCorporationAfter = (ILegalEntityCloseCorporation)legalEntityAfter;
					ILegalEntityCloseCorporation legalEntityCloseCorporationBefore = (ILegalEntityCloseCorporation)legalEntityBefore;

					HelperCompareProperties<string>(legalEntityCloseCorporationAfter.RegisteredName, legalEntityCloseCorporationBefore.RegisteredName, "Registered Name", Messages);
					HelperCompareProperties<string>(legalEntityCloseCorporationAfter.RegistrationNumber, legalEntityCloseCorporationBefore.RegistrationNumber, "Registration Number", Messages);
				}
			}

			#endregion Rule logic begins

			return 0;
		}

		private void HelperCompareProperties<T>(T propertyValueAfter, T propertyValueBefore, string propertyName, IDomainMessageCollection Messages)
		{
			if (!propertyValueAfter.Equals(propertyValueBefore))
			{
				AddMessage(String.Format("The legal entity details ({0}) may not be changed for legal entities with active accounts.", propertyName), "", Messages);
			}
		}
	}

	[RuleDBTag("MortgageLoanAccountLegalEntityRoleMainApplicant",
	"Must be at least 1 Legal Entity that plays a main applicant role in the account Applicable to Account and Application.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.LegalEntity.MortgageLoanAccountLegalEntityRoleMainApplicant")]
	[RuleInfo]
	public class MortgageLoanAccountLegalEntityRoleMainApplicant : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleMainApplicant rule expects a Domain object to be passed.");

			if (!(Parameters[0] is IMortgageLoanAccount || Parameters[0] is IApplicationMortgageLoan))
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleMainApplicant rule expects the following objects to be passed: IMortgageLoanAccount, IApplicationMortgageLoan.");

			#endregion Check for allowed object type(s)

			bool mainApplicantFound = false;
			bool leadMainApplicantFound = false;
			if (Parameters[0] is IMortgageLoanAccount)
			{
				IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;

				foreach (IRole role in mortgageLoanAccount.Roles)
				{
					if (role.RoleType.Key == (int)RoleTypes.MainApplicant
						&& role.GeneralStatus.Key == (int)GeneralStatuses.Active)
					{
						mainApplicantFound = true;
						break;
					}
				}
			}

			if (Parameters[0] is IApplicationMortgageLoan)
			{
				IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

				foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
				{
					if (applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
						|| applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor)
						leadMainApplicantFound = true;

					if (applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
						&& applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
					{
						mainApplicantFound = true;
					}
				}
			}
			if (leadMainApplicantFound)
			{
				string msg = "There cannot be any Lead Roles for this Application to be submitted to Credit.";
				AddMessage(msg, msg, Messages);
			}
			if (!mainApplicantFound)
			{
				string msg = "There must exist at least one Legal Entity that plays a Main Applicant Role in the Mortgage Loan.";
				AddMessage(msg, msg, Messages);
			}

			return 0;
		}
	}

	[RuleDBTag("MortgageLoanAccountLegalEntityRoleCompanyMainApplicant",
	"The Company can only play a role of Main Applicant in an account and never surety.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.LegalEntity.MortgageLoanAccountLegalEntityRoleCompanyMainApplicant")]
	[RuleInfo]
	public class MortgageLoanAccountLegalEntityRoleCompanyMainApplicant : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleCompanyMainApplicant rule expects a Domain object to be passed.");

			IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
			IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

			if (mortgageLoanAccount == null && applicationMortgageLoan == null)
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleCompanyMainApplicant rule expects the following objects to be passed: IMortgageLoanAccount OR IApplicationMortgageLoan.");

			#endregion Check for allowed object type(s)

			bool nonMainApplicantFound = false;

			if (mortgageLoanAccount != null)
			{
				foreach (IRole role in mortgageLoanAccount.Roles)
				{
					switch ((LegalEntityTypes)role.LegalEntity.LegalEntityType.Key)
					{
						case LegalEntityTypes.CloseCorporation:
						case LegalEntityTypes.Company:
						case LegalEntityTypes.Trust:

							// if it's a company, we need to check that it isn't an active suretor
							if (role.RoleType.Key == (int)RoleTypes.Suretor && role.GeneralStatus.Key == (int)GeneralStatuses.Active)
								nonMainApplicantFound = true;
							break;
						default:
							break;
					}

					// if we've found a suretor of type company, we might as well exit here
					if (nonMainApplicantFound)
						break;
				}
			}
			else if (applicationMortgageLoan != null)
			{
				foreach (IApplicationRole role in applicationMortgageLoan.ApplicationRoles)
				{
					switch ((LegalEntityTypes)role.LegalEntity.LegalEntityType.Key)
					{
						case LegalEntityTypes.CloseCorporation:
						case LegalEntityTypes.Company:
						case LegalEntityTypes.Trust:
							if ((role.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor || role.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor) && (role.GeneralStatus.Key == (int)GeneralStatuses.Active))
								nonMainApplicantFound = true;
							break;
						default:
							break;
					}

					if (nonMainApplicantFound)
						break;
				}
			}

			if (nonMainApplicantFound)
			{
				string msg = "A Company can only play a role of Main Applicant in an account and never Surety.";
				AddMessage(msg, msg, Messages);
			}

			return 0;
		}
	}

	[RuleDBTag("MortgageLoanAccountLegalEntityRoleCompanyMainApplicantQuantity",
	"There can only be one Main Applicant on the loan when there is a CC/Company/Trust Main Applicant.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.LegalEntity.MortgageLoanAccountLegalEntityRoleCompanyMainApplicantQuantity")]
	[RuleInfo]
	public class MortgageLoanAccountLegalEntityRoleCompanyMainApplicantQuantity : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleCompanyMainApplicantQuantity rule expects a Domain object to be passed.");

			if (!(Parameters[0] is IMortgageLoanAccount
				|| Parameters[0] is IApplicationMortgageLoan))
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleCompanyMainApplicantQuantity rule expects the following objects to be passed: IMortgageLoanAccount.");

			#endregion Check for allowed object type(s)

			bool hasCompany = false;
			int mainApplicantCount = 0;

			if (Parameters[0] is IMortgageLoanAccount)
			{
				IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
				foreach (IRole role in mortgageLoanAccount.Roles)
				{
					if ((role.RoleType.Key == (int)RoleTypes.MainApplicant
						|| role.RoleType.Key == (int)RoleTypes.Suretor)
						&& role.GeneralStatus.Key == (int)GeneralStatuses.Active)
					{
						switch ((LegalEntityTypes)role.LegalEntity.LegalEntityType.Key)
						{
							case LegalEntityTypes.CloseCorporation:
								hasCompany = true;
								break;
							case LegalEntityTypes.Company:
								hasCompany = true;
								break;
							case LegalEntityTypes.Trust:
								hasCompany = true;
								break;
							default:
								break;
						}
					}

					if (role.RoleType.Key == (int)RoleTypes.MainApplicant
						&& role.GeneralStatus.Key == (int)GeneralStatuses.Active)
						mainApplicantCount++;
				}
			}

			if (Parameters[0] is IApplicationMortgageLoan)
			{
				IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;
				foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
				{
					if ((applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
						|| applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
						|| applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
						|| applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor)
						&& applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
					{
						switch ((LegalEntityTypes)applicationRole.LegalEntity.LegalEntityType.Key)
						{
							case LegalEntityTypes.CloseCorporation:
								hasCompany = true;
								break;
							case LegalEntityTypes.Company:
								hasCompany = true;
								break;
							case LegalEntityTypes.Trust:
								hasCompany = true;
								break;
							default:
								break;
						}
					}

					if ((applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
						|| applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant)
						&& applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
						mainApplicantCount++;
				}
			}

			if (hasCompany
				&& mainApplicantCount > 1)
			{
				string msg = "There can only be one Main Applicant on the loan when there is a CC/Company/Trust Main Applicant.";
				AddMessage(msg, msg, Messages);
			}

			return 0;
		}
	}

	[RuleDBTag("MortgageLoanAccountLegalEntityRoleCompanyMainApplicantNaturalPersonSuretor",
	"When the Main Applicant is a Trust/CC/Company then there must be atleat 1 surety in the form of natural person.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.LegalEntity.MortgageLoanAccountLegalEntityRoleCompanyMainApplicantNaturalPersonSuretor")]
	[RuleInfo]
	public class MortgageLoanAccountLegalEntityRoleCompanyMainApplicantNaturalPersonSuretor : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleCompanyMainApplicantNaturalPersonSuretor rule expects a Domain object to be passed.");

			if (!(Parameters[0] is IApplicationMortgageLoan))
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleCompanyMainApplicantNaturalPersonSuretor rule expects the following objects to be passed: IApplicationMortgageLoan.");

			#endregion Check for allowed object type(s)

			bool isCompany = false;
			int suretorCount = 0;

			if (Parameters[0] is IApplicationMortgageLoan)
			{
				IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;
				foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
				{
					if ((applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
						|| applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant)
						&& applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
					{
						switch ((LegalEntityTypes)applicationRole.LegalEntity.LegalEntityType.Key)
						{
							case LegalEntityTypes.CloseCorporation:
								isCompany = true;
								break;
							case LegalEntityTypes.Company:
								isCompany = true;
								break;
							case LegalEntityTypes.Trust:
								isCompany = true;
								break;
							default:
								break;
						}
					}

					if ((applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
						|| applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor)
						&& applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active
						&& applicationRole.LegalEntity is ILegalEntityNaturalPerson)
						suretorCount++;
				}
			}

			if (isCompany
				&& suretorCount < 1)
			{
				string msg = "When the Main Applicant is a Trust/CC/Company then there must be atleat 1 surety in the form of natural person.";
				AddMessage(msg, msg, Messages);
			}
			return 0;
		}
	}

	[Obsolete(@"The rule would not yield the desired results as it throws an Exception when ILegalEntityNaturalPerson is not passed.
				The body that uses the ILegalEntityNaturalPerson is partly implemented.
				The body that uses another type (IApplicationMortgageLoan) looks correct though but because of the check will never reach that section")]
	[RuleDBTag("LegalEntityNaturalPersonMainOrSuretorApplicantMLDeclarationRehabilitatedDate",
	"Must be at least 1 Legal Entity that plays a main applicant role in the account Applicable to Account and Application.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityNaturalPersonMainOrSuretorApplicantMLDeclarationRehabilitatedDate")]
	[RuleInfo]
	public class LegalEntityNaturalPersonMainOrSuretorApplicantMLDeclarationRehabilitatedDate : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The LegalEntityNaturalPersonMainOrSuretorApplicantMLDeclarationRehabilitatedDate rule expects a Domain object to be passed.");

			if (!(Parameters[0] is ILegalEntityNaturalPerson))
				throw new ArgumentException("The LegalEntityNaturalPersonMainOrSuretorApplicantMLDeclarationRehabilitatedDate rule expects the following objects to be passed: ILegalEntityNaturalPerson.");

			#endregion Check for allowed object type(s)

			bool mainApplicantFound = false;
			if (Parameters[0] is ILegalEntityNaturalPerson)
			{
				ILegalEntityNaturalPerson legalEntityNaturalPerson = Parameters[0] as ILegalEntityNaturalPerson;

				foreach (IApplicationRole role in legalEntityNaturalPerson.ApplicationRoles)
				{
					//if (role.RoleType.Key == (int)RoleTypes.
					// role.
				}

				//foreach (IRole role in mortgageLoanAccount.Roles)
				//{
				//    if (role.RoleType.Key == (int)RoleTypes.MainApplicant
				//        && role.GeneralStatus.Key == (int)GeneralStatuses.Active)
				//    {
				//        mainApplicantFound = true;
				//        break;
				//    }
				//}
			}

			if (Parameters[0] is IApplicationMortgageLoan)
			{
				IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

				foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
				{
					if (applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
						&& applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
					{
						mainApplicantFound = true;
						break;
					}
				}
			}

			if (!mainApplicantFound)
			{
				string msg = "There must exist at least one Legal Entity that plays a Main Applicant Role in the Mortgage Loan.";
				AddMessage(msg, msg, Messages);
			}

			return 0;
		}
	}

	[RuleDBTag("LegalEntityMandatoryName",
	"Requires that a legal name is entered for saving a Lead Applicant",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityMandatoryName")]
	[RuleInfo]
	public class LegalEntityMandatoryName : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			ILegalEntity le = Parameters[0] as ILegalEntity;
			if (le is ILegalEntityCloseCorporation || le is ILegalEntityCompany || le is ILegalEntityTrust)
			{
				return 0;

				// This lenght will always be greater than zero
				// Rule "LegalEntityCompanyCCTrustMandatoryRegisteredName" will be checking this
				/*
				if (le.GetLegalName(LegalNameFormat.Full).Length == 0)
				{
					string errorMessage = "Company Name Required";
					AddMessage(errorMessage, errorMessage, Messages);
				}*/
			}
			else if (le is ILegalEntityNaturalPerson)
			{
				ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;

				if (Utils.StringUtils.IsNullOrEmptyTrimmed(leNP.FirstNames))
				{
					string errorMessage = "Legal Entity First Name Required";
					AddMessage(errorMessage, errorMessage, Messages);
				}
				if (Utils.StringUtils.IsNullOrEmptyTrimmed(leNP.Surname))
				{
					string errorMessage = "Legal Entity Surname Required";
					AddMessage(errorMessage, errorMessage, Messages);
				}
			}
			else
			{
				string errorMessage = "Unknown Legal Entity Type";
				AddMessage(errorMessage, errorMessage, Messages);
			}
			return 0;
		}
	}

	[RuleDBTag("LegalEntityContactDetailsMandatory",
	"Requires that at least one contact detail is entered for saving a Legal Entity",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityContactDetailsMandatory")]
	[RuleInfo]
	public class LegalEntityContactDetailsMandatory : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			ILegalEntity le = Parameters[0] as ILegalEntity;
			if (Utils.StringUtils.IsNullOrEmptyTrimmed(le.EmailAddress)
				&& Utils.StringUtils.IsNullOrEmptyTrimmed(le.CellPhoneNumber)

				//&& (string.IsNullOrEmpty(le.FaxCode) || string.IsNullOrEmpty(le.FaxNumber)) -- Fax Number should not be considered a mandatory contact number
				&& (Utils.StringUtils.IsNullOrEmptyTrimmed(le.HomePhoneCode) || Utils.StringUtils.IsNullOrEmptyTrimmed(le.HomePhoneNumber))
				&& (Utils.StringUtils.IsNullOrEmptyTrimmed(le.WorkPhoneCode) || Utils.StringUtils.IsNullOrEmptyTrimmed(le.WorkPhoneNumber)))
			{
				string errorMessage = "At least one contact detail (Email, Home, Work or Cell Number) is required";
				AddMessage(errorMessage, errorMessage, Messages);
			}
			return 0;
		}
	}

	[RuleDBTag("LegalEntityOriginationSource",
	"Warns if trying to add a LegalEntity that belongs to a different Origintion Source to the logged-in user",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityOriginationSource", false)]
	[RuleInfo]
	public class LegalEntityOriginationSource : BusinessRuleBase
	{
		public LegalEntityOriginationSource(ICastleTransactionsService castleTransactionService)
		{
			this.castleTransactionService = castleTransactionService;
		}

		private ICastleTransactionsService castleTransactionService;

		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The LegalEntityOriginationSource rule expects a Domain object to be passed.");

			if (!(Parameters[0] is ILegalEntity))
				throw new ArgumentException("The LegalEntityOriginationSource rule expects the following objects to be passed: ILegalEntity.");

			#endregion Check for allowed object type(s)

			ILegalEntity le = Parameters[0] as ILegalEntity;

			string loggedInUserName = WindowsIdentity.GetCurrent().Name;
			int primaryOriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans;
			if (loggedInUserName.ToLower().StartsWith("rcshl"))
				primaryOriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.RCS;

			string sqlQuery = UIStatementRepository.GetStatement("COMMON", "LegalEntityOriginationSource");
			ParameterCollection prms = new ParameterCollection();
			prms.Add(new SqlParameter("@leKey", le.Key.ToString()));

			DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);
			if (ds.Tables.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int osKey = Convert.ToInt32(dr[5]);
					if (osKey != primaryOriginationSourceKey)
					{
						string errorMessage = (String.IsNullOrEmpty(dr[0].ToString()) ? le.GetLegalName(LegalNameFormat.Full) : dr[0].ToString()) + " "
										+ (String.IsNullOrEmpty(dr[1].ToString()) ? GetIDRegistrationNumber(le) : dr[1].ToString()) + " is attached to a "
										+ dr[2].ToString() + " " + dr[3].ToString() + dr[4].ToString();

						AddMessage(errorMessage, errorMessage, Messages);
						return 0;
					}
				}
			}
			return 1;
		}

		private string GetIDRegistrationNumber(ILegalEntity le)
		{
			string idRegNumber = "";
			switch (le.LegalEntityType.Key)
			{
				case (int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson:
					ILegalEntityNaturalPerson np = le as ILegalEntityNaturalPerson;
					idRegNumber = !String.IsNullOrEmpty(np.IDNumber) ? np.IDNumber : "";
					break;
				case (int)SAHL.Common.Globals.LegalEntityTypes.Company:
					ILegalEntityCompany com = le as ILegalEntityCompany;
					idRegNumber = !String.IsNullOrEmpty(com.RegistrationNumber) ? com.RegistrationNumber : "";
					break;
				case (int)SAHL.Common.Globals.LegalEntityTypes.CloseCorporation:
					ILegalEntityCloseCorporation cc = le as ILegalEntityCloseCorporation;
					idRegNumber = !String.IsNullOrEmpty(cc.RegistrationNumber) ? cc.RegistrationNumber : "";
					break;
				case (int)SAHL.Common.Globals.LegalEntityTypes.Trust:
					ILegalEntityTrust trust = le as ILegalEntityTrust;
					idRegNumber = !String.IsNullOrEmpty(trust.RegistrationNumber) ? trust.RegistrationNumber : "";
					break;
				default:
					break;
			}
			return idRegNumber;
		}
	}

	[RuleDBTag("MortgageLoanAccountLegalEntityRoleLeadMainApplicant",
"Must be at least 1 Legal Entity that plays a lead main applicant role in the account Applicable to Account and Application.",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.LegalEntity.MortgageLoanAccountLegalEntityRoleLeadMainApplicant")]
	[RuleInfo]
	public class MortgageLoanAccountLegalEntityRoleLeadMainApplicant : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleLeadMainApplicant rule expects a Domain object to be passed.");

			if (!(Parameters[0] is IMortgageLoanAccount || Parameters[0] is IApplicationMortgageLoan))
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleLeadMainApplicant rule expects the following objects to be passed: IMortgageLoanAccount, IApplicationMortgageLoan.");

			#endregion Check for allowed object type(s)

			bool leadMainApplicantFound = false;
			if (Parameters[0] is IMortgageLoanAccount)
			{
				IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;

				foreach (IRole role in mortgageLoanAccount.Roles)
				{
					if (role.RoleType.Key == (int)RoleTypes.MainApplicant
						&& role.GeneralStatus.Key == (int)GeneralStatuses.Active)
					{
						leadMainApplicantFound = true;
						break;
					}
				}
			}

			if (Parameters[0] is IApplicationMortgageLoan)
			{
				IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

				foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
				{
					if (applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
						|| applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
						&& applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
					{
						leadMainApplicantFound = true;
						break;
					}
				}
			}

			if (!leadMainApplicantFound)
			{
				string msg = "There must exist at least one Legal Entity that plays a Lead - Main Applicant Role in the Mortgage Loan.";
				AddMessage(msg, msg, Messages);
			}

			return 0;
		}
	}

	[RuleDBTag("MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicant",
"The Company can only play a role of Lead Main Applicant in an account and never surety.",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.LegalEntity.MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicant")]
	[RuleInfo]
	public class MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicant : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicant rule expects a Domain object to be passed.");

			IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
			IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;

			if (mortgageLoanAccount == null && applicationMortgageLoan == null)
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicant rule expects the following objects to be passed: IMortgageLoanAccount OR IApplicationMortgageLoan.");

			#endregion Check for allowed object type(s)

			bool nonMainApplicantFound = false;

			if (mortgageLoanAccount != null)
			{
				foreach (IRole role in mortgageLoanAccount.Roles)
				{
					switch ((LegalEntityTypes)role.LegalEntity.LegalEntityType.Key)
					{
						case LegalEntityTypes.CloseCorporation:
						case LegalEntityTypes.Company:
						case LegalEntityTypes.Trust:

							// if it's a company, we need to check that it isn't an active suretor
							if (role.RoleType.Key == (int)RoleTypes.Suretor && role.GeneralStatus.Key == (int)GeneralStatuses.Active)
								nonMainApplicantFound = true;
							break;
						default:
							break;
					}

					// if we've found a suretor of type company, we might as well exit here
					if (nonMainApplicantFound)
						break;
				}
			}
			else if (applicationMortgageLoan != null)
			{
				foreach (IApplicationRole role in applicationMortgageLoan.ApplicationRoles)
				{
					switch ((LegalEntityTypes)role.LegalEntity.LegalEntityType.Key)
					{
						case LegalEntityTypes.CloseCorporation:
						case LegalEntityTypes.Company:
						case LegalEntityTypes.Trust:
							if ((role.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor || role.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor) && (role.GeneralStatus.Key == (int)GeneralStatuses.Active))
								nonMainApplicantFound = true;
							break;
						default:
							break;
					}

					if (nonMainApplicantFound)
						break;
				}
			}

			if (nonMainApplicantFound)
			{
				string msg = "A Company can only play a role of Lead - Main Applicant and never a Surety.";
				AddMessage(msg, msg, Messages);
			}

			return 0;
		}
	}

	[RuleDBTag("MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantQuantity",
"There can only be one Lead Main Applicant on the loan when there is a CC/Company/Trust Main Applicant.",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.LegalEntity.MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantQuantity")]
	[RuleInfo]
	public class MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantQuantity : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantQuantity rule expects a Domain object to be passed.");

			if (!(Parameters[0] is IMortgageLoanAccount
				|| Parameters[0] is IApplicationMortgageLoan))
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantQuantity rule expects the following objects to be passed: IMortgageLoanAccount.");

			#endregion Check for allowed object type(s)

			bool hasCompany = false;
			int mainApplicantCount = 0;

			if (Parameters[0] is IMortgageLoanAccount)
			{
				IMortgageLoanAccount mortgageLoanAccount = Parameters[0] as IMortgageLoanAccount;
				foreach (IRole role in mortgageLoanAccount.Roles)
				{
					if ((role.RoleType.Key == (int)RoleTypes.MainApplicant
						|| role.RoleType.Key == (int)RoleTypes.Suretor)
						&& role.GeneralStatus.Key == (int)GeneralStatuses.Active)
					{
						switch ((LegalEntityTypes)role.LegalEntity.LegalEntityType.Key)
						{
							case LegalEntityTypes.CloseCorporation:
								hasCompany = true;
								break;
							case LegalEntityTypes.Company:
								hasCompany = true;
								break;
							case LegalEntityTypes.Trust:
								hasCompany = true;
								break;
							default:
								break;
						}
					}

					if (role.RoleType.Key == (int)RoleTypes.MainApplicant
						&& role.GeneralStatus.Key == (int)GeneralStatuses.Active)
						mainApplicantCount++;
				}
			}

			if (Parameters[0] is IApplicationMortgageLoan)
			{
				IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;
				foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
				{
					if ((applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
						|| applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
						|| applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
						|| applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor)
						&& applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
					{
						switch ((LegalEntityTypes)applicationRole.LegalEntity.LegalEntityType.Key)
						{
							case LegalEntityTypes.CloseCorporation:
								hasCompany = true;
								break;
							case LegalEntityTypes.Company:
								hasCompany = true;
								break;
							case LegalEntityTypes.Trust:
								hasCompany = true;
								break;
							default:
								break;
						}
					}

					if ((applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
						|| applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant)
						&& applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
						mainApplicantCount++;
				}
			}

			if (hasCompany
				&& mainApplicantCount > 1)
			{
				string msg = "There can only be one Lead - Main Applicant on the loan when there is a CC/Company/Trust Lead - Main Applicant.";
				AddMessage(msg, msg, Messages);
			}

			return 0;
		}
	}

	[RuleDBTag("MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantNaturalPersonSuretor",
"When the Lead Main Applicant is a Trust/CC/Company then there must be atleat 1 surety in the form of natural person.",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.LegalEntity.MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantNaturalPersonSuretor")]
	[RuleInfo]
	public class MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantNaturalPersonSuretor : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantNaturalPersonSuretor rule expects a Domain object to be passed.");

			if (!(Parameters[0] is IApplicationMortgageLoan))
				throw new ArgumentException("The MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantNaturalPersonSuretor rule expects the following objects to be passed: IApplicationMortgageLoan.");

			#endregion Check for allowed object type(s)

			bool isCompany = false;
			int suretorCount = 0;

			if (Parameters[0] is IApplicationMortgageLoan)
			{
				IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;
				foreach (IApplicationRole applicationRole in applicationMortgageLoan.ApplicationRoles)
				{
					if ((applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
						|| applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant)
						&& applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
					{
						switch ((LegalEntityTypes)applicationRole.LegalEntity.LegalEntityType.Key)
						{
							case LegalEntityTypes.CloseCorporation:
								isCompany = true;
								break;
							case LegalEntityTypes.Company:
								isCompany = true;
								break;
							case LegalEntityTypes.Trust:
								isCompany = true;
								break;
							default:
								break;
						}
					}

					if ((applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
						|| applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor)
						&& applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active
						&& applicationRole.LegalEntity is ILegalEntityNaturalPerson)
						suretorCount++;
				}
			}

			if (isCompany
				&& suretorCount < 1)
			{
				string msg = "When the Lead - Main Applicant is a CC/Company/Trust then there must be at least one surety in the form of natural person.";
				AddMessage(msg, msg, Messages);
			}

			return 0;
		}
	}

	[RuleDBTag("ExternalRoleLitigationLegalEntityMandatoryDetail",
	"When a legal entity is added with the role of either Debt Counselling, Deceased Estates,Foreclosure,Sequestrations Check for Mandatory LE Details.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.LegalEntity.ExternalRoleLitigationLegalEntityMandatoryDetail")]
	[RuleInfo]
	public class ExternalRoleLitigationLegalEntityMandatoryDetail : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)

			if (Parameters.Length == 0)
				throw new ArgumentException("The ExternalRoleLitigationLegalEntityMandatoryDetail rule expects a Domain object to be passed.");

			if (!(Parameters[0] is IExternalRole))
				throw new ArgumentException("The ExternalRoleLitigationLegalEntityMandatoryDetail rule expects the following objects to be passed: IExternalRole.");

			#endregion Check for allowed object type(s)

			IExternalRole extRole = Parameters[0] as IExternalRole;

			bool LitigationRole = false;

			switch ((ExternalRoleTypes)extRole.ExternalRoleType.Key)
			{
				case ExternalRoleTypes.DebtCounselling:
					LitigationRole = true;
					break;
				case ExternalRoleTypes.DeceasedEstates:
					LitigationRole = true;
					break;
				case ExternalRoleTypes.Foreclosure:
					LitigationRole = true;
					break;
				case ExternalRoleTypes.Sequestrations:
					LitigationRole = true;
					break;
				default:
					break;
			}

			if (!LitigationRole)
			{
				return 0;
			}

			ILegalEntityNaturalPerson leNP = extRole.LegalEntity as ILegalEntityNaturalPerson;

			if (leNP == null)
			{
				string errorMessage = "Legal Entity must be a Natural Person";
				AddMessage(errorMessage, errorMessage, Messages);
				return 0;
			}

			if (Utils.StringUtils.IsNullOrEmptyTrimmed(leNP.FirstNames))
			{
				string errorMessage = "Legal Entity First Name Required";
				AddMessage(errorMessage, errorMessage, Messages);
			}
			if (Utils.StringUtils.IsNullOrEmptyTrimmed(leNP.Surname))
			{
				string errorMessage = "Legal Entity Surname Required";
				AddMessage(errorMessage, errorMessage, Messages);
			}
			if (Utils.StringUtils.IsNullOrEmptyTrimmed(leNP.EmailAddress))
			{
				string errorMessage = "Legal Entity Email Address Required";
				AddMessage(errorMessage, errorMessage, Messages);
			}

			if (Utils.StringUtils.IsNullOrEmptyTrimmed(leNP.CellPhoneNumber)
				&& Utils.StringUtils.IsNullOrEmptyTrimmed(leNP.HomePhoneNumber)
				&& Utils.StringUtils.IsNullOrEmptyTrimmed(leNP.WorkPhoneNumber))
			{
				string errorMessage = "At least one Telephone Number (Home, Work or Cell Number) is required";
				AddMessage(errorMessage, errorMessage, Messages);
			}

			return 0;
		}
	}

	[RuleInfo]
	[RuleDBTag("DetermineLegalEntityHasBeenDeclined",
	"Legal Entity has had a prevoius Decline.",
	"SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.LegalEntity.DetermineLegalEntityHasBeenDeclined")]
	public class DetermineLegalEntityHasBeenDeclined : BusinessRuleBase
	{
		public DetermineLegalEntityHasBeenDeclined(ICastleTransactionsService castleTransactionService)
		{
			this.castleTransactionService = castleTransactionService;
		}

		private ICastleTransactionsService castleTransactionService;

		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			string query = "";

			IApplicationMortgageLoan applicationMortgageLoan = Parameters[0] as IApplicationMortgageLoan;
			IProperty prop = Parameters[1] as IProperty;
			int propkey = prop.Key;
			foreach (IApplicationRole role in applicationMortgageLoan.ApplicationRoles)
			{
				if (role.ApplicationRoleType.ApplicationRoleTypeGroup.Key == 3)
				{
					query = UIStatementRepository.GetStatement("Common", "LegalEntityDeclineOnProperty");

					// Create a collection
					ParameterCollection parameters = new ParameterCollection();

					//Add the required parameters
					parameters.Add(new SqlParameter("@PropKey", propkey));
					parameters.Add(new SqlParameter("@LeKey", role.LegalEntity.Key));

					// execute

					DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
					if (ds.Tables.Count > 0)
					{
						foreach (DataRow dr in ds.Tables[0].Rows)
						{
							int osKey = Convert.ToInt32(dr[0]);
							string errorMessage = "An application, " + osKey + ", consisting of the same person and same property has previously been declined.";
							AddMessage(errorMessage, errorMessage, Messages);
							return 0;
						}
					}
				}
			}

			return 1;
		}
	}

	[RuleInfo]
	[RuleDBTag("LegalEntityPropertyPreviousDecline",
	"Legal Entity has had a prevoius Declined offer for this property.",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityPropertyPreviousDecline")]
	public class LegalEntityPropertyPreviousDecline : BusinessRuleBase
	{
		public LegalEntityPropertyPreviousDecline(ICastleTransactionsService castleTransactionService)
		{
			this.castleTransactionService = castleTransactionService;
		}

		private ICastleTransactionsService castleTransactionService;

		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			int leKey = Convert.ToInt32(Parameters[0]);
			int propKey = Convert.ToInt32(Parameters[1]);

			string query = UIStatementRepository.GetStatement("Common", "LegalEntityDeclineOnProperty");

			// Create a collection
			ParameterCollection parameters = new ParameterCollection();

			//Add the required parameters
			parameters.Add(new SqlParameter("@PropKey", propKey));
			parameters.Add(new SqlParameter("@LeKey", leKey));

			// execute
			object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

			// Get the Return Values
			int recs = o == null ? 0 : Convert.ToInt32(o);
			if (recs > 0)
			{
				string msg = String.Format(@"An application, {0}, consisting of the same person and same property has previously been declined.", recs.ToString());
				AddMessage(msg, msg, Messages);
				return 0;
			}

			return 1;
		}
	}

	[RuleDBTag("LegalEntityDuplicateApplication",
  "Checks whether the captured legal entity is a duplicate application for the same property but different application",
  "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityDuplicateApplication")]
	[RuleInfo]
	public class LegalEntityDuplicateApplication : BusinessRuleBase
	{
		public LegalEntityDuplicateApplication(ICastleTransactionsService castleTransactionService)
		{
			this.castleTransactionService = castleTransactionService;
		}

		private ICastleTransactionsService castleTransactionService;

		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			int iLegalEntityKey = (int)Parameters[0];
			int iOfferKey = (int)Parameters[1];
			int iPropertyKey = (int)Parameters[2];

			string sqlQuery = UIStatementRepository.GetStatement("Rules.LegalEntity", "LegalEntityDuplicateApplication");
			ParameterCollection prms = new ParameterCollection();

			prms.Add(new SqlParameter("@legalentitykey", iLegalEntityKey));
			prms.Add(new SqlParameter("@addtoOfferKey", iOfferKey));
			prms.Add(new SqlParameter("@propertyKey", iPropertyKey));
			prms.Add(new SqlParameter("@StageDefintionGroupNTUOffer", (int)StageDefinitionStageDefinitionGroups.NTUOffer));
			prms.Add(new SqlParameter("@StageDefintionGroupDeclineOffer", (int)StageDefinitionStageDefinitionGroups.DeclineOffer));

			DataSet dsProperty = this.castleTransactionService.ExecuteQueryOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

			if (dsProperty != null)
			{
				if (dsProperty.Tables.Count > 0)
				{
					if (dsProperty.Tables[0].Rows.Count > 0)
					{
						int iExistingApplication = Convert.ToInt32(dsProperty.Tables[0].Rows[0]["OfferKey"]);
						string errMsg = "Application " + iExistingApplication.ToString() + ", containing the clients ID number and the respective property, " +
										"already exists in the origination process";
						AddMessage(errMsg, errMsg, Messages);
						return 0;
					}
				}
			}

			return 1;
		}
	}

	[RuleDBTag("LegalEntityOpenFurtherLending", "Checking if Legal Entity Has Open Further Lending",
	"SAHL.Rules.DLL", "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityOpenFurtherLending")]
	[RuleInfo]
	public class LegalEntityOpenFurtherLending : BusinessRuleBase
	{
		private readonly ILegalEntityRepository legalEntityRepository;

		public LegalEntityOpenFurtherLending(ILegalEntityRepository legalEntityRepository)
		{
			this.legalEntityRepository = legalEntityRepository;
		}

		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] parameters)
		{
			if (parameters.Length == 0)
				throw new ArgumentException("The Legal Entity Open Further Lending rule expects a Domain object to be passed.");

			if (!(parameters[0] is ILegalEntity))
				throw new ArgumentException("The Legal Entity Open Further Lending rule expects the following objects to be passed: ILegalEntity.");

			ILegalEntity legalEntity = parameters[0] as ILegalEntity;
			IReadOnlyEventList<IApplication> furtherLendingCases = legalEntityRepository.GetOpenFurtherLendingApplicationsByLegalEntity(legalEntity);

			if (furtherLendingCases == null)
				return 1;

			string futherlendingApplist = "Further Lending Applications exist for ";
			string message = string.Empty;

			foreach (IApplication app in furtherLendingCases)
			{
				message = string.Format(@"The Account [{0}] has an open further lending application: {1}", app.Account.Key, app.Key);
				AddMessage(message, message, Messages);
				message = string.Empty;
			}

			return 0;
		}
	}

	[RuleDBTag("HasAccountInArrearsInLast6Months", "Check if the Legal Entity has had an Account in Arrears in the Last 6 Months",
	"SAHL.Rules.DLL", "SAHL.Common.BusinessModel.Rules.LegalEntity.HasAccountInArrearsInLast6Months")]
	[RuleInfo]
	public class HasAccountInArrearsInLast6Months : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
		{
			if (parameters.Length == 0)
				throw new ArgumentException("The Legal Entity Has Account In Arrears In Last 6 Months rule expects a Domain object to be passed.");

			if (!(parameters[0] is ILegalEntity))
				throw new ArgumentException("The Legal Entity Has Account In Arrears In Last 6 Months rule expects the following objects to be passed: ILegalEntity.");

			ILegalEntity legalEntity = parameters[0] as ILegalEntity;

			var accountsInArrears = legalEntity.Roles.Where(x => x.Account.AccountType == AccountTypes.MortgageLoan &&
																x.Account.HasBeenInArrears(6, 200))
															.Select(x => x.Account);

			var messageFormat = "The Mortgage Loan Account [{0}] is currently in arrears or has been in arrears in the last 6 months";
			foreach (var accountInArrears in accountsInArrears)
			{
				var accountInArrearsMessage = String.Format(messageFormat, accountInArrears.Key);
				AddMessage(accountInArrearsMessage, accountInArrearsMessage, messages);
			}

			return 0;
		}
	}

    [RuleDBTag("ActiveDomiciliumAddressIsUsedOnOpenApplications", "Checks if the Active Domicilium for a Legal Entity is used on Open Applications",
    "SAHL.Rules.DLL", "SAHL.Common.BusinessModel.Rules.LegalEntity.ActiveDomiciliumAddressIsUsedOnOpenApplications")]
    [RuleInfo]
    public class ActiveDomiciliumAddressIsUsedOnOpenApplications : BusinessRuleBase
    {
        private readonly IApplicationRepository applicationRepository;

        public ActiveDomiciliumAddressIsUsedOnOpenApplications(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            if (parameters.Length == 0)
                return 0;
            if (!(parameters[0] is ILegalEntity))
                return 0;

            ILegalEntity legalEntity = parameters[0] as ILegalEntity;

            if (legalEntity.ActiveDomiciliumAddress == null || legalEntity.ActiveDomiciliumAddress.GeneralStatus.Key != (int)GeneralStatuses.Active)
                return 0;

            var applicationsThatUseLegalEntityDomicilium = applicationRepository.GetApplicationsThatUseLegalEntityDomicilium(legalEntity.ActiveDomiciliumAddress);
            var openApplicationsThatUseLegalEntityDomicilium = applicationsThatUseLegalEntityDomicilium.Where(x => x.ApplicationRole.Application.IsOpen);
            if (openApplicationsThatUseLegalEntityDomicilium.Count() > 0)
            {
                var message = String.Format("The client has elected to use this domicilium address on open application(s) ({0}), updating will change the domicilium on these applications.", String.Join(",", openApplicationsThatUseLegalEntityDomicilium.Select(x => x.ApplicationRole.Application.Key)));
                AddMessage(message, message, messages);
            }
            return 0;
        }
    }

}