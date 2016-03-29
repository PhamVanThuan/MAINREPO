using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.RelationshipRole
{
    [RuleDBTag("AddRelationshipRoleBetween2LegalEntity",
        "AddRelationshipRoleBetween2LegalEntity",
        "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.RelationshipRole.AddRelationshipRoleBetween2LegalEntity")]
    [RuleInfo]
    public class AddRelationshipRoleBetween2LegalEntity : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {

            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The AddRelationshipRoleBetween2LegalEntity rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ILegalEntity))
                throw new ArgumentException("The AddRelationshipRoleBetween2LegalEntity rule expects the following objects to be passed: ILegalEntity.");

            #endregion

            ILegalEntity legalEntity = Parameters[0] as ILegalEntity;
            bool isRelatedLegalEntityNull = false;

            foreach (ILegalEntityRelationship legalEntityRelationship in legalEntity.LegalEntityRelationships)
            {
                if (legalEntityRelationship.RelatedLegalEntity == null)
                {
                    isRelatedLegalEntityNull = true;
                    break;
                }
            }

            if (isRelatedLegalEntityNull)
                AddMessage("At least 2 Legal Entities are required to setup the relationship.", "", Messages);

            return 0;
        }
    }

    [RuleDBTag("AddRelationshipRoleMortgageLoanAccount",
        "AddRelationshipRoleMortgageLoanAccount",
        "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.RelationshipRole.AddRelationshipRoleMortgageLoanAccount")]
    [RuleInfo]
    public class AddRelationshipRoleMortgageLoanAccount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {

            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The AddRelationshipRoleMortgageLoanAccount rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IMortgageLoanAccount))
                throw new ArgumentException("The AddRelationshipRoleMortgageLoanAccount rule expects the following objects to be passed: IMortgageLoanAccount.");

            #endregion

            IMortgageLoanAccount account = Parameters[0] as IMortgageLoanAccount;

            bool isRelatedLegalEntityOK = false;

            // For each role ine the account
            foreach (IRole role in account.Roles)
            {
                // For each relationship in the role
                foreach (ILegalEntityRelationship legalEntityRelationship in role.LegalEntity.LegalEntityRelationships)
                {
                    isRelatedLegalEntityOK = false;

                    // Do these 2 exist in the account roles?
                    foreach (IRole accountRole in account.Roles)
                    {
                        if (role.LegalEntity.Key == legalEntityRelationship.RelatedLegalEntity.Key
                            && (accountRole.RoleType.Key == (int)RoleTypes.MainApplicant
                            || accountRole.RoleType.Key == (int)RoleTypes.Suretor))
                        {
                            isRelatedLegalEntityOK = true;
                            break;
                        }
                    }
                    if (!isRelatedLegalEntityOK)
                        break;
                }
            }

            if (!isRelatedLegalEntityOK)
                AddMessage("Relationships may only be set up between legal entities that play roles of Surety or Main Applicant in the same account.", "", Messages);

            return 0;
        }
    }

    [RuleDBTag("AddRelationshipRoleExistenceApplication",
        "AddRelationshipRoleExistenceApplication",
        "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.RelationshipRole.AddRelationshipRoleExistenceApplication")]
    [RuleInfo]
    public class AddRelationshipRoleExistenceApplication : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {

            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The AddRelationshipRoleExistenceApplication rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The AddRelationshipRoleExistenceApplication rule expects the following objects to be passed: IApplication.");

            #endregion

            IApplication application = Parameters[0] as IApplication;

            int countActiveApplicationRoles = 0;
            bool isRelated = true;

            // Count the active roles in the account
            foreach (IApplicationRole applicationRole in application.ApplicationRoles)
            {
                if (applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
                    countActiveApplicationRoles++;
            }

            if (countActiveApplicationRoles > 1)
            {
                isRelated = false;
                // All of them need to be related
                foreach (IApplicationRole applicationRole in application.ApplicationRoles)
                {
                    if (applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active
                        && applicationRole.LegalEntity.LegalEntityRelationships.Count > 0)
                        isRelated = true;
                }
            }

            if (!isRelated)
                AddMessage("A relationship must be set where there's more than one legal entity on the application.", "", Messages);

            return 0;
        }
    }

    [RuleDBTag("AddRelationshipRoleSurety",
        "AddRelationshipRoleSurety",
        "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.RelationshipRole.AddRelationshipRoleSurety")]
    [RuleInfo]
    public class AddRelationshipRoleSurety : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {

            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The AddRelationshipRoleSurety rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The AddRelationshipRoleSurety rule expects the following objects to be passed: IApplication.");

            #endregion

            IApplication application = Parameters[0] as IApplication;

            bool hasRelatedSuretor = true;

            // Count the active roles in the account
            foreach (IApplicationRole applicationRole in application.ApplicationRoles)
            {
                if (applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active
                    && applicationRole.ApplicationRoleType.Key == (int)RoleTypes.Suretor)
                {
                    hasRelatedSuretor = RelatedMainApplicantExists(application, applicationRole.LegalEntity);
                    // Look for a related main applicant

                    if (!hasRelatedSuretor)
                        break;
                }

                if (!hasRelatedSuretor)
                    break;
            }

            if (!hasRelatedSuretor)
                AddMessage("All Suretor Legal Entities must be related to at least one Main Applicant.", "", Messages);

            return 0;
        }

        private bool RelatedMainApplicantExists(IApplication application, ILegalEntity suretor)
        {
            bool relationshipExists = false;

            foreach (ILegalEntityRelationship legalEntityRelationship in suretor.LegalEntityRelationships)
            {

                // Must play main applicant role in the relationship ...
                // legalEntityRelationship.RelatedLegalEntity.Key
                foreach (IApplicationRole applicationRole in application.ApplicationRoles)
                {
                    if (applicationRole.ApplicationRoleType.Key == (int)RoleTypes.MainApplicant
                        && applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active
                        && applicationRole.LegalEntity.Key == legalEntityRelationship.RelatedLegalEntity.Key)
                    {
                        relationshipExists = true;
                        break;
                    }
                }
                if (relationshipExists)
                    break;
            }

            return relationshipExists;
        }
    }
}
