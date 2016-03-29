using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Linq;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.DAO;

namespace SAHL.Common.BusinessModel.Rules.LegalEntityAddress
{
    /// <summary>
    /// Params:
    /// 0: ILegalEntityAddress
    /// </summary>
    [RuleInfo]
    [RuleDBTag("LegalEntityAddressResidentialConditionalAddressFormats",
    "???",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressResidentialConditionalAddressFormats")]
    public class LegalEntityAddressResidentialConditionalAddressFormats : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntityAddress leAddress = (ILegalEntityAddress)Parameters[0];
			if (leAddress != null && leAddress.AddressType != null && leAddress.AddressType.Key == Convert.ToInt32(AddressTypes.Residential))
            {
                IAddress address = leAddress.Address;
                if ((address != null) && !(address is IAddressStreet) && !(address is IAddressFreeText))
                {
                    AddMessage("Residential Addresses can only have Street or FreeText formats", "", Messages);
                    return 0;
                }
            }
            return 1;
        }
    }


    /// <summary>
    /// Params:
    /// 0: ILegalEntityAddress
    /// </summary>
    [RuleInfo]
    [RuleDBTag("LegalEntityAddressPostalConditionalAddressFormats",
    "???",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressPostalConditionalAddressFormats")]
    public class LegalEntityAddressPostalConditionalAddressFormats : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntityAddress leAddress = (ILegalEntityAddress)Parameters[0];
            if (leAddress != null && leAddress.AddressType != null && leAddress.AddressType.Key == Convert.ToInt32(AddressTypes.Postal))
            {
                IAddress address = leAddress.Address;
                if (address is IAddressFreeText)
                {
                    AddMessage("Postal Addresses can only have Street, Box, PrivateBag, PostNetSuite or ClusterBox formats", "", Messages);
                    return 0;
                }
            }
            return 1;
        }
    }

    /// <summary>
    /// Params:
    /// 0: ILegalEntityAddress
    /// </summary>
    [RuleInfo]
    [RuleDBTag("LegalEntityAddressEffectiveDateMinimum",
    "The effective date for new legal entity address objects cannot be before today's date.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressEffectiveDateMinimum")]
    public class LegalEntityAddressEffectiveDateMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntityAddress leAddress = (ILegalEntityAddress)Parameters[0];
            if (leAddress.Key == 0 && leAddress.EffectiveDate < DateTime.Today)
            {
                AddMessage("The Effective Date cannot be before today's date", "", Messages);
                return 0;
            }
            return 1;
        }
    }

    /// <summary>
    /// Params:
    /// 0: ILegalEntityAddress
    /// </summary>
    [RuleInfo]
    [RuleDBTag("LegalEntityAddressEffectiveDateMaximum",
    "The effective date for legal entity address objects cannot be more than a certain number of months from today's date.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressEffectiveDateMaximum")]
    [RuleParameterTag(new string[] { "@MaximumMonths,6,9" })]
    public class LegalEntityAddressEffectiveDateMaximum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntityAddress leAddress = (ILegalEntityAddress)Parameters[0];
            int months = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

            if (leAddress.EffectiveDate > DateTime.Now.AddMonths(months))
            {
                string message = String.Format("The Effective Date cannot be more than {0} months after today's date", months);
                AddMessage(message, message, Messages);
                return 0;
            }
            return 1;
        }
    }


    /// <summary>
    /// Rule that checks to ensure the LegalEntityAddress is not attached to any open applications before it 
    /// is deactivated.
    /// </summary>
    [RuleInfo]
    [RuleDBTag("LegalEntityAddressApplicationMailingAddressCheck",
    "Legal Entity Address objects cannot be deleted/deactivated if they're the mailing address on an open application.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressApplicationMailingAddressCheck")]
    public class LegalEntityAddressApplicationMailingAddressCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntityAddress leAddress = (ILegalEntityAddress)Parameters[0];
            int addressKey = leAddress.Address.Key;

            // if the address is active, then we do not need to perform this check
            if (leAddress.GeneralStatus.Key == (int)GeneralStatuses.Active)
                return 1;

            int result = 1;
            foreach (IApplicationRole role in leAddress.LegalEntity.GetApplicationRolesByRoleTypeGroups(OfferRoleTypeGroups.Client))
            {
                IApplication application = role.Application;
                if (application.ApplicationStatus.Key == (int)OfferStatuses.Open)
                {
                    foreach (IApplicationMailingAddress mailingAddress in application.ApplicationMailingAddresses)
                    {
                        if (mailingAddress.Address.Key == addressKey)
                        {
                            string msg = String.Format("The address cannot be removed from the legal entity as it is the mailing address for an open application ({0})", application.Key);
                            AddMessage(msg, msg, Messages);
                            result = 0;
                        }
                    }
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Rule that checks to ensure the LegalEntityAddress is not attached to any open applications before it 
    /// is deactivated.
    /// </summary>
    [RuleInfo]
    [RuleDBTag("LegalEntityAddressAccountMailingAddressCheck",
    "Legal Entity Address objects cannot be deleted/deactivated if they're the mailing address on an open account.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressAccountMailingAddressCheck")]
    public class LegalEntityAddressAccountMailingAddressCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntityAddress leAddress = (ILegalEntityAddress)Parameters[0];
            int addressKey = leAddress.Address.Key;

            // if the address is active, then we do not need to perform this check
            if (leAddress.GeneralStatus.Key == (int)GeneralStatuses.Active)
                return 1;

            int result = 1;
            foreach (IRole role in leAddress.LegalEntity.Roles)
            {
                IAccount account = role.Account;
                if (account.AccountStatus.Key == (int)AccountStatuses.Open)
                {
                    foreach (IMailingAddress mailingAddress in account.MailingAddresses)
                    {
                        if (mailingAddress.Address.Key == addressKey)
                        {
                            string msg = String.Format("The address cannot be removed from the legal entity as it is the mailing address for an open account ({0})", account.Key);
                            AddMessage(msg, msg, Messages);
                            result = 0;
                        }
                    }
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Rule that checks to ensure the LegalEntityAddress is not deleted when the Role is Suretor or Main Applicant
    /// and there is only one active address
    /// </summary>
    [RuleInfo]
    [RuleDBTag("LegalEntityAddressDoNotDeleteOnRole",
    "Legal Entity Address objects cannot be deleted/deactivated if they're the only address and the Role is Suretor or Main Applicant",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressDoNotDeleteOnRole")]
    public class LegalEntityAddressDoNotDeleteOnRole : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The LegalEntityAddressDoNotDeleteOnRole rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ILegalEntityAddress))
                throw new ArgumentException("The LegalEntityAddressDoNotDeleteOnRole rule expects the following objects to be passed: ILegalEntityAddress.");

            #endregion


            ILegalEntityAddress leAddress = (ILegalEntityAddress)Parameters[0];
            int addressKey = leAddress.Address.Key;
            // if the address is active, then we do not need to perform this check - only need to check that the address is inactive (deleted)
            if (leAddress.GeneralStatus.Key == (int)GeneralStatuses.Active)
               return 1;

            foreach (ILegalEntityAddress lea in leAddress.LegalEntity.LegalEntityAddresses)
            {
               if (lea.GeneralStatus.Key == (int)GeneralStatuses.Active && lea.Key != leAddress.Key)
               {
                   // more than one active Address so can return and delete it safely.
                   return 1;
               }
            }
            // can only get to this point if there is only one address (all others are inactive).
            // need to check if role is mainapplicant or suretor. if so then we cannot delete the address.
            
            foreach (IRole role in leAddress.LegalEntity.Roles)
            {
                IAccount account = role.Account;
                if (account.AccountStatus.Key == (int)AccountStatuses.Open)
                {
                    if (role.RoleType.Key == (int)RoleTypes.MainApplicant || role.RoleType.Key == (int)RoleTypes.Suretor)
                    {
                        // Role is either MainApplicant or Suretor and there is only 1 LegalEntityAddress
                        string msg = String.Format("The address cannot be removed on a MainApplicant or Suretor when it is the only Legal Entity Address.");
                        AddMessage(msg, msg, Messages);
                        return 0;
                    }
                }
            }
            
            foreach (IApplicationRole role in leAddress.LegalEntity.ApplicationRoles)
            {
                IApplication application = role.Application;
                
                if (role.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant || role.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor || role.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant)
                {
                    // Role is either MainApplicant or Suretor and there is only 1 LegalEntityAddress
                    string msg = String.Format("The address cannot be removed on a MainApplicant or Suretor when it is the only Legal Entity Address.");
                    AddMessage(msg, msg, Messages);
                    return 0;
                }
                
            }
            
            return 1;
        }
    }

    /// <summary>
    /// Rule stops a legalentity addres being removed if it is the LegalEntityDomicilium for any legalentity
    /// </summary>
    [RuleInfo]
    [RuleDBTag("LegalEntityAddressCantBeMadeInactiveIfLinkedToALegalEntityDomicilium",
    "Legal Entity Address objects cannot be deleted/deactivated if they're a legalentity domicilium.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressCantBeMadeInactiveIfLinkedToALegalEntityDomicilium")]
    public class LegalEntityAddressCantBeMadeInactiveIfLinkedToALegalEntityDomicilium : BusinessRuleBase
    {
        private readonly ILegalEntityRepository legalEntityRepository;
        public LegalEntityAddressCantBeMadeInactiveIfLinkedToALegalEntityDomicilium(ILegalEntityRepository legalEntityRepository)
        {
            this.legalEntityRepository = legalEntityRepository;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntityAddress leAddress = (ILegalEntityAddress)Parameters[0];
            int addressKey = leAddress.Address.Key;
            int result = 0;

            IEventList<ILegalEntityDomicilium> legalEntityDomiciliums = legalEntityRepository.GetLegalEntityDomiciliumsForAddressKey(leAddress.Address.Key);
            foreach (ILegalEntityDomicilium legalEntityDomicilium in legalEntityDomiciliums)
            {
                if (legalEntityDomicilium.GeneralStatus.Key == ((int)SAHL.Common.Globals.GeneralStatuses.Active)
                    || legalEntityDomicilium.GeneralStatus.Key == ((int)SAHL.Common.Globals.GeneralStatuses.Pending))
                {
                    string msg = String.Format("The address cannot be removed from the legal entity as it is the domicilium address for legal entity ({0})", legalEntityDomicilium.LegalEntityAddress.LegalEntity.Key);
                    AddMessage(msg, msg, Messages);
                    result = 1;
                }
            }

            return result;
        }
    }

    [RuleInfo]
    [RuleDBTag("LegalEntityAddressChangesWillUpdateActiveDomicilium",
    "LegalEntityAddressChangesWillUpdateActiveDomicilium",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressChangesWillUpdateActiveDomicilium")]
    public class LegalEntityAddressChangesWillUpdateActiveDomicilium : BusinessRuleBase
    {
        private readonly ILegalEntityRepository legalEntityRepository;
        public LegalEntityAddressChangesWillUpdateActiveDomicilium(ILegalEntityRepository legalEntityRepository)
        {
            this.legalEntityRepository = legalEntityRepository;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntityAddress leAddress = (ILegalEntityAddress)Parameters[0];
            int addressKey = leAddress.Address.Key;
            int result = 0;

            IEventList<ILegalEntityDomicilium> legalEntityDomiciliums = legalEntityRepository.GetLegalEntityDomiciliumsForAddressKey(leAddress.Address.Key);
            foreach (ILegalEntityDomicilium legalEntityDomicilium in legalEntityDomiciliums)
            {
                if (legalEntityDomicilium.GeneralStatus.Key == ((int)SAHL.Common.Globals.GeneralStatuses.Active) 
                    && legalEntityDomicilium.LegalEntityAddress.Key == leAddress.Key
                    )
                {
                    string msg = String.Format("Changing this address will update the active domicilium address for {0}.", 
                            legalEntityDomicilium.LegalEntityAddress.LegalEntity.DisplayName);
                    AddMessage(msg, msg, Messages);
                    result = 1;
                }
            }

            return result;
        }
    }

    [RuleInfo]
    [RuleDBTag("LegalEntityAddressCanNotBeDeletedIfItIsTheActiveLegalEntityDomicilium",
    "LegalEntityAddressCanNotBeDeletedIfItIsTheActiveLegalEntityDomicilium",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressCanNotBeDeletedIfItIsTheActiveLegalEntityDomicilium")]
    public class LegalEntityAddressCanNotBeDeletedIfItIsTheActiveLegalEntityDomicilium : BusinessRuleBase
    {
        private readonly ILegalEntityRepository legalEntityRepository;
        public LegalEntityAddressCanNotBeDeletedIfItIsTheActiveLegalEntityDomicilium(ILegalEntityRepository legalEntityRepository)
        {
            this.legalEntityRepository = legalEntityRepository;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntityAddress leAddress = (ILegalEntityAddress)Parameters[0];
            int addressKey = leAddress.Address.Key;
            int result = 0;

            IEventList<ILegalEntityDomicilium> legalEntityDomiciliums = legalEntityRepository.GetLegalEntityDomiciliumsForAddressKey(leAddress.Address.Key);
            foreach (ILegalEntityDomicilium legalEntityDomicilium in legalEntityDomiciliums)
            {
                if ((legalEntityDomicilium.GeneralStatus.Key == ((int)SAHL.Common.Globals.GeneralStatuses.Active)
                    || legalEntityDomicilium.GeneralStatus.Key == ((int)SAHL.Common.Globals.GeneralStatuses.Pending))
                    && legalEntityDomicilium.LegalEntityAddress.Key == leAddress.Key)
                {
                    string msg = String.Format("This address is linked to the {0} domicilium address for {1} and cannot be deleted.", 
                        legalEntityDomicilium.GeneralStatus.Description, legalEntityDomicilium.LegalEntityAddress.LegalEntity.DisplayName);
                    AddMessage(msg, msg, Messages);
                    result = 1;
                }
            }

            return result;
        }
    }

    [RuleInfo]
    [RuleDBTag("LegalEntityAddressChangesWillUpdatePendingDomicilium",
    "LegalEntityAddressChangesWillUpdatePendingDomicilium",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressChangesWillUpdatePendingDomicilium")]
    public class LegalEntityAddressChangesWillUpdatePendingDomicilium : BusinessRuleBase
    {
        // run on the change of LE address
        private readonly ILegalEntityRepository legalEntityRepository;
        private readonly IApplicationRepository applicationRepository;
        public LegalEntityAddressChangesWillUpdatePendingDomicilium(ILegalEntityRepository legalEntityRepository, IApplicationRepository applicationRepository)
        {
            this.legalEntityRepository = legalEntityRepository;
            this.applicationRepository = applicationRepository;
        }

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntityAddress leAddress = (ILegalEntityAddress)Parameters[0];
            int addressKey = leAddress.Address.Key;
            int result = 0;

            IEventList<ILegalEntityDomicilium> legalEntityDomiciliums = legalEntityRepository.GetLegalEntityDomiciliumsForAddressKey(leAddress.Address.Key);
            foreach (ILegalEntityDomicilium legalEntityDomicilium in legalEntityDomiciliums)
            {
                if (legalEntityDomicilium.GeneralStatus.Key == ((int)SAHL.Common.Globals.GeneralStatuses.Pending) 
                    && legalEntityDomicilium.LegalEntityAddress.Key == leAddress.Key)
                {
                    var applicationRolesThatUseLegalEntityDomicilium = applicationRepository.GetApplicationsThatUseLegalEntityDomicilium(legalEntityDomicilium.LegalEntityAddress);
                    var openApplicationRolesLinkedToOpenApplication = applicationRolesThatUseLegalEntityDomicilium.Where(x => x.ApplicationRole.Application.IsOpen);

                    foreach (var role in openApplicationRolesLinkedToOpenApplication)
                    {
                        var message = String.Format("{0} has elected to use this address as the proposed domicilium on application {1}, updating will change the domicilium on this application.",
                            role.ApplicationRole.LegalEntity.DisplayName, role.ApplicationRole.ApplicationKey);
                        AddMessage(message, message, Messages);
                        result = 1;
                    }

                }
            }
            return result;
        }
    }
}
