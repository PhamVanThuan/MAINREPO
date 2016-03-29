using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord;
using NHibernate;

namespace SAHL.Common.BusinessModel.Rules.LegalEntity
{
    /// <summary>
    /// 1: A trading Name must be provided
    /// 2: At least one contact number is required.
    /// Params:
    /// 0: ILegalEntityCloseCorporation
    /// </summary>
    [RuleInfo]
    [RuleDBTag("LegalEntityCompanyCCTrustMandatoryTradingName",
    "???",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityCompanyCCTrustMandatoryTradingName")]
    public class LegalEntityCompanyCCTrustMandatoryTradingName : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity LE = Parameters[0] as ILegalEntity;
            ILegalEntityGenericCompany LeGC = LE as ILegalEntityGenericCompany;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (LE != null && LeGC != null)
            {
                CheckTradingName(Messages, LeGC);
            }
            else if (role != null && role.LegalEntity is ILegalEntityGenericCompany)
            {
                LeGC = role.LegalEntity as ILegalEntityGenericCompany;
                CheckTradingName(Messages, LeGC);
            }
            return 1;
        }

        private void CheckTradingName(IDomainMessageCollection Messages, ILegalEntityGenericCompany LeGC)
        {
            if (Utils.StringUtils.IsNullOrEmptyTrimmed(LeGC.TradingName))
            {
                AddMessage("You must supply a Trading Name", "", Messages);
            }
        }
    }

    /// <summary>
    /// Params:
    /// 0: ILegalEntityCloseCorporation
    /// </summary>
    [RuleInfo]
    [RuleDBTag("LegalEntityCompanyCCTrustMandatoryContact",
    "???",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityCompanyCCTrustMandatoryContact")]
    public class LegalEntityCompanyCCTrustMandatoryContact : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity LE = Parameters[0] as ILegalEntity;
            ILegalEntityGenericCompany LeGC = LE as ILegalEntityGenericCompany;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (LE != null && LeGC != null)
            {
                CheckContact(Messages, LeGC);
            }
            else if (role != null && role.LegalEntity is ILegalEntityGenericCompany)
            {
                LeGC = role.LegalEntity as ILegalEntityGenericCompany;
                CheckContact(Messages, LeGC);
            }

            return 1;
        }

        private void CheckContact(IDomainMessageCollection Messages, ILegalEntityGenericCompany LE)
        {
            if (Utils.StringUtils.IsNullOrEmptyTrimmed(LE.CellPhoneNumber) && (Utils.StringUtils.IsNullOrEmptyTrimmed(LE.WorkPhoneCode) || Utils.StringUtils.IsNullOrEmptyTrimmed(LE.WorkPhoneNumber)) && (Utils.StringUtils.IsNullOrEmptyTrimmed(LE.HomePhoneCode) || Utils.StringUtils.IsNullOrEmptyTrimmed(LE.HomePhoneNumber)))
            {
                AddMessage("At least one contact number must be provided. Note that for non-cellphone numbers both a dialing code and number must be provided", "", Messages);
            }
        }
    }

    [RuleDBTag("ValidateUniqueRegistrationNumber",
    "A company/cc/trust must have a unique registration number.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.ValidateUniqueRegistrationNumber")]
    [RuleInfo]
    public class ValidateUniqueRegistrationNumber : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ValidateUniqueRegistrationNumber rule expects a Domain object to be passed.");

            //if (!(Parameters[0] is ILegalEntityCompany) && !(Parameters[0] is ILegalEntityCloseCorporation) && !(Parameters[0] is ILegalEntityTrust))
            //    throw new ArgumentException("The ValidateUniqueRegistrationNumber rule expects the following objects to be passed: ILegalEntityCompany or ILegalEntityCloseCorporation or ILegalEntityTrust.");

            ILegalEntity legalEntity = null;
            IApplicationRole role = null;
            if (Parameters[0] is IApplicationRole)
            {
                role = Parameters[0] as IApplicationRole;
                legalEntity = role.LegalEntity as ILegalEntity;
            }
            else if (Parameters[0] is ILegalEntity)
            {
                legalEntity = Parameters[0] as ILegalEntity;
            }

            if (legalEntity != null && legalEntity is ILegalEntityGenericCompany)
            {
                string registrationNumber = "", registeredName = "", legalEntityTypeDesc = "";
                int registrationNumberCount = 0, matchingLegalEntityKey = 0;
                switch (legalEntity.LegalEntityType.Key)
                {
                    case (int)SAHL.Common.Globals.LegalEntityTypes.Company:
                        ILegalEntityCompany legalEntityCompany = legalEntity as ILegalEntityCompany;
                        registrationNumber = legalEntityCompany.RegistrationNumber;
                        break;
                    case (int)SAHL.Common.Globals.LegalEntityTypes.CloseCorporation:
                        ILegalEntityCloseCorporation legalEntityCloseCorporation = legalEntity as ILegalEntityCloseCorporation;
                        registrationNumber = legalEntityCloseCorporation.RegistrationNumber;
                        break;
                    case (int)SAHL.Common.Globals.LegalEntityTypes.Trust:
                        ILegalEntityTrust legalEntityTrust = legalEntity as ILegalEntityTrust;
                        registrationNumber = legalEntityTrust.RegistrationNumber;
                        break;
                    default:
                        break;
                }

                if (!String.IsNullOrEmpty(registrationNumber))
                {
                    // check company types for registration number
                    LegalEntityCompany_DAO[] matchesCompany = LegalEntityCompany_DAO.FindAllByProperty("RegistrationNumber", registrationNumber);
                    registrationNumberCount = matchesCompany.Length;
                    if (registrationNumberCount == 0)
                    {
                        LegalEntityCloseCorporation_DAO[] matchesCC = LegalEntityCloseCorporation_DAO.FindAllByProperty("RegistrationNumber", registrationNumber);
                        registrationNumberCount = matchesCC.Length;
                        if (registrationNumberCount == 0)
                        {
                            LegalEntityTrust_DAO[] matchesTrust = LegalEntityTrust_DAO.FindAllByProperty("RegistrationNumber", registrationNumber);
                            registrationNumberCount = matchesTrust.Length;
                            if (registrationNumberCount > 0)
                            {
                                legalEntityTypeDesc = "Trust";
                                matchingLegalEntityKey = matchesTrust[0].Key;
                                registeredName = matchesTrust[0].RegisteredName;
                            }
                        }
                        else
                        {
                            legalEntityTypeDesc = "Close Corporation";
                            matchingLegalEntityKey = matchesCC[0].Key;
                            registeredName = matchesCC[0].RegisteredName;
                        }
                    }
                    else
                    {
                        legalEntityTypeDesc = "Company";
                        matchingLegalEntityKey = matchesCompany[0].Key;
                        registeredName = matchesCompany[0].RegisteredName;
                    }

                    if (registrationNumberCount > 1 || (registrationNumberCount == 1 && matchingLegalEntityKey != legalEntity.Key))
                    {
                        string errorMessage = "A " + legalEntityTypeDesc + " (" + registeredName + ") already exists with this Registration Number."; ;
                        AddMessage(errorMessage, errorMessage, Messages);
                        return 0;
                    }
                }
            }
            else
            {
                throw new ArgumentException("The ValidateUniqueRegistrationNumber rule expects the following objects to be passed: ILegalEntityCompany or ILegalEntityCloseCorporation or ILegalEntityTrust.");
            }
            return 1;
        }
    }

    [RuleInfo]
    [RuleDBTag("LegalEntityCompanyCCTrustMandatoryRegisteredName",
    "A company/cc/trust must have a Registrated Name",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityCompanyCCTrustMandatoryRegisteredName")]
    public class LegalEntityCompanyCCTrustMandatoryRegisteredName : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity LE = Parameters[0] as ILegalEntity;
            ILegalEntityGenericCompany LeGC = LE as ILegalEntityGenericCompany;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (LE != null && LeGC != null)
            {
                CheckRegisteredName(Messages, LeGC);
            }
            else if (role != null && role.LegalEntity is ILegalEntityGenericCompany)
            {
                LeGC = role.LegalEntity as ILegalEntityGenericCompany;
                CheckRegisteredName(Messages, LeGC);
            }

            return 1;
        }

        private void CheckRegisteredName(IDomainMessageCollection Messages, ILegalEntityGenericCompany LeGC)
        {
            if (Utils.StringUtils.IsNullOrEmptyTrimmed(LeGC.RegisteredName))
            {
                AddMessage("Legal Entity Company Name Required", "", Messages);
            }
        }
    }

    [RuleInfo]
    [RuleDBTag("LegalEntityCompanyCCTrustMandatoryRegistrationNumber",
    "A company/cc/trust must have a Registration Number",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityCompanyCCTrustMandatoryRegistrationNumber")]
    public class LegalEntityCompanyCCTrustMandatoryRegistrationNumber : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity LE = Parameters[0] as ILegalEntity;
            ILegalEntityGenericCompany LeGC = LE as ILegalEntityGenericCompany;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (LE != null && LeGC != null)
            {
                CheckRegistrationNumber(Messages, LeGC);
            }
            else if (role != null && role.LegalEntity is ILegalEntityGenericCompany)
            {
                LeGC = role.LegalEntity as ILegalEntityGenericCompany;
                CheckRegistrationNumber(Messages, LeGC);
            }
            return 1;
        }

        private void CheckRegistrationNumber(IDomainMessageCollection Messages, ILegalEntityGenericCompany LeGC)
        {
            if (Utils.StringUtils.IsNullOrEmptyTrimmed(LeGC.RegistrationNumber))
            {
                AddMessage("Legal Entity Registration Number Required", "", Messages);
            }
        }
    }

    [RuleInfo]
    [RuleDBTag("LegalEntityCompanyCCTrustMandatoryTaxNumber",
    "A company/cc/trust must have a Tax Number",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityCompanyCCTrustMandatoryTaxNumber")]
    public class LegalEntityCompanyCCTrustMandatoryTaxNumber : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity LE = Parameters[0] as ILegalEntity;
            ILegalEntityGenericCompany LeGC = LE as ILegalEntityGenericCompany;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (LE != null && LeGC != null)
            {
                CheckTaxNumber(Messages, LeGC);
            }
            else if (role != null && role.LegalEntity is ILegalEntityGenericCompany)
            {
                LeGC = role.LegalEntity as ILegalEntityGenericCompany;
                CheckTaxNumber(Messages, LeGC);
            }
            return 1;
        }

        private void CheckTaxNumber(IDomainMessageCollection Messages, ILegalEntityGenericCompany LeGC)
        {
            if (Utils.StringUtils.IsNullOrEmptyTrimmed(LeGC.TaxNumber))
            {
                AddMessage("Legal Entity Tax Number Required", "", Messages);
            }
        }
    }

    [RuleInfo]
    [RuleDBTag("LegalEntityCompanyCCTrustMandatoryDocumentLanguage",
    "A company/cc/trust must have a Document Language",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityCompanyCCTrustMandatoryDocumentLanguage")]
    public class LegalEntityCompanyCCTrustMandatoryDocumentLanguage : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity LE = Parameters[0] as ILegalEntity;
            ILegalEntityGenericCompany LeGC = LE as ILegalEntityGenericCompany;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (LE != null && LeGC != null)
            {
                CheckDocumentLanguage(Messages, LeGC);
            }
            else if (role != null && role.LegalEntity is ILegalEntityGenericCompany)
            {
                LeGC = role.LegalEntity as ILegalEntityGenericCompany;
                CheckDocumentLanguage(Messages, LeGC);
            }
            return 1;
        }

        private void CheckDocumentLanguage(IDomainMessageCollection Messages, ILegalEntityGenericCompany LeGC)
        {
            if (LeGC.DocumentLanguage == null)
            {
                AddMessage("Legal Entity Document Language Required", "", Messages);
            }
        }
    }

    [RuleInfo]
    [RuleDBTag("LegalEntityCompanyCCTrustMandatoryLegalEntityStatus",
    "A company/cc/trust must have a Legal Entity Status",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntity.LegalEntityCompanyCCTrustMandatoryLegalEntityStatus")]
    public class LegalEntityCompanyCCTrustMandatoryLegalEntityStatus : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntity LE = Parameters[0] as ILegalEntity;
            ILegalEntityGenericCompany LeGC = LE as ILegalEntityGenericCompany;
            IApplicationRole role = Parameters[0] as IApplicationRole;

            if (LE != null && LeGC != null)
            {
                CheckLegalEntityStatus(Messages, LeGC);
            }
            else if (role != null && role.LegalEntity is ILegalEntityGenericCompany)
            {
                LeGC = role.LegalEntity as ILegalEntityGenericCompany;
                CheckLegalEntityStatus(Messages, LeGC);
            }
            return 1;
        }

        private void CheckLegalEntityStatus(IDomainMessageCollection Messages, ILegalEntityGenericCompany LeGC)
        {
            if (LeGC.LegalEntityStatus == null)
            {
                AddMessage("Legal Entity Status Required", "", Messages);

            }
        }
    }

}