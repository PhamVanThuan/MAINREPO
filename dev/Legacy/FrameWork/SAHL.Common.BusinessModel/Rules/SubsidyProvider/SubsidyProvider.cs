using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using Castle.ActiveRecord;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using System.Text.RegularExpressions;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.SubsidyProvider
{

    [RuleDBTag("SubsidyProviderAddUnique",
    "SubsidyProvider Name must be unique",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.SubsidyProvider.SubsidyProviderAddUnique")]
    [RuleInfo]
    public class SubsidyProviderAddUnique : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ISubsidyProvider sp = (ISubsidyProvider)Parameters[0];
            int spList = FindProviderByRegisteredName(sp.LegalEntity.DisplayName.Trim());
            if (sp.Key == 0 && spList > 0)
            {
                AddMessage("This Subsidy Provider already exists", "", Messages);
            }
            return 1;

        }
      
        private int FindProviderByRegisteredName(string desc)
        {
            int RetVal = 0;
            string HQL = "Select sp from SubsidyProvider_DAO sp where sp.LegalEntity.Key in "
                  + "  ( select let.Key from LegalEntityCloseCorporation_DAO let where let.RegisteredName = ?) "
               + " or sp.LegalEntity.Key in "
               + " ( select lec.Key from LegalEntityCompany_DAO lec where lec.RegisteredName = ?) "
               + " or sp.LegalEntity.Key IN "
               + "( select lecc.Key from LegalEntityCloseCorporation_DAO lecc where lecc.RegisteredName = ?) ";

            SimpleQuery<SubsidyProvider_DAO> q = new SimpleQuery<SubsidyProvider_DAO>(HQL, desc, desc, desc);
            SubsidyProvider_DAO[] res = q.Execute();

            IEventList<ISubsidyProvider> list = new DAOEventList<SubsidyProvider_DAO, ISubsidyProvider, SAHL.Common.BusinessModel.SubsidyProvider>(res);
            RetVal = list.Count;
            return RetVal;

        }
    }
    [RuleDBTag("SubsidyProviderLegalEntityType",
     "SubsidyProviderLegalEntityType can not be Natural Person",
     "SAHL.Rules.DLL",
     "SAHL.Common.BusinessModel.Rules.SubsidyProvider.SubsidyProviderLegalEntityType")]
    [RuleInfo]
    public class SubsidyProviderLegalEntityType : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ISubsidyProvider sp = (ISubsidyProvider)Parameters[0];
            ILegalEntity le = sp.LegalEntity;

            if ((le.LegalEntityType.Key == (int)LegalEntityTypes.NaturalPerson) || (le.LegalEntityType.Key == (int)LegalEntityTypes.Unknown))
            {
                AddMessage("Subsidy Provider can not be of type Natural Person", "", Messages);
            }
            return 1;
        }
    }
    [RuleDBTag("SubsidyProviderLegalEntityEmailAddress",
   "SubsidyProvider LegalEntity EmailAddress must be in a valid email format",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.SubsidyProvider.SubsidyProviderLegalEntityEmailAddress")]
    [RuleInfo]
    public class SubsidyProviderLegalEntityEmailAddress : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ISubsidyProvider sp = (ISubsidyProvider)Parameters[0];
            ILegalEntity le = sp.LegalEntity;

            bool IsValid;

            if (le.EmailAddress.Length > 0)
            {
                Regex regxEmail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                IsValid = CommonValidation.IsEmail(le.EmailAddress);
            }
            else
                IsValid = true;

            if (!IsValid)
            {
                AddMessage("Please enter email address in correct format", "", Messages);
            }
            return 1;
        }
    }

    /// <summary>
    /// Rule that ensures that an address is captured with a SubsidyProvider.
    /// </summary>
    [RuleDBTag("SubsidyProviderAddressMandatory",
    "Ensures that an address is captured with a SubsidyProvider",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.SubsidyProvider.SubsidyProviderAddressMandatory")]
    [RuleInfo]
    public class SubsidyProviderAddressMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ISubsidyProvider sp = (ISubsidyProvider)Parameters[0];
            ILegalEntity le = sp.LegalEntity;

            // if the legal entity has not been set, we can ignore this rule
            if (le == null)
                return 1;

            if (le.LegalEntityAddresses.Count == 0)
            {
                AddMessage("An address must be provided with a Subsidy Provider", "", Messages);
                return 0;
            }
            return 1;
        }
    }
}
