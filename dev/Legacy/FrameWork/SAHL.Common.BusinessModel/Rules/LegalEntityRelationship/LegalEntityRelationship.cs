using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.LegalEntityRelationship
{
    [RuleDBTag("LegalEntityRelationshipCircular",
    "A legal entity cannot be added as a relationship to itself.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.LegalEntityRelationship.LegalEntityRelationshipCircular")]
    [RuleInfo]
    public class LegalEntityRelationshipCircular : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The LegalEntityRelationshipCircular rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ILegalEntityRelationship))
                throw new ArgumentException("The LegalEntityRelationshipCircular rule expects the following objects to be passed: ILegalEntityRelationship.");

            ILegalEntityRelationship legalEntityRelationship = Parameters[0] as ILegalEntityRelationship;

            if (legalEntityRelationship.LegalEntity.Key == legalEntityRelationship.RelatedLegalEntity.Key)
            {
                string errorMessage = "Cannot relate a Legal Entity to itself.";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("LegalEntityRelationshipDuplicate",
"Cannot add same LE more than once with same realtionship type",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.LegalEntityRelationship.LegalEntityRelationshipDuplicate")]
    [RuleInfo]
    public class LegalEntityRelationshipDuplicate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The LegalEntityRelationshipDuplicate rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ILegalEntityRelationship))
                throw new ArgumentException("The LegalEntityRelationshipDuplicate rule expects the following objects to be passed: ILegalEntityRelationship.");

            ILegalEntityRelationship legalEntityRelationship = Parameters[0] as ILegalEntityRelationship;

            // if there is no LegalEntityRelationshipType then exit this rule so this can get trapped in the mandatory field validation
            if (legalEntityRelationship.LegalEntityRelationshipType == null)
                return 1;

            bool relationshipExists = false;
            string relationshipType = "";
            foreach (ILegalEntityRelationship ler in legalEntityRelationship.LegalEntity.LegalEntityRelationships)
            {
                if (ler.RelatedLegalEntity.Key == legalEntityRelationship.RelatedLegalEntity.Key
                    && ler.LegalEntityRelationshipType.Key == legalEntityRelationship.LegalEntityRelationshipType.Key
                    && ler.Key != legalEntityRelationship.Key)
                {
                    relationshipExists = true;
                    relationshipType = ler.LegalEntityRelationshipType.Description;
                    break;
                }

            }

            if (relationshipExists == true)
            {
                string errorMessage = "Legal Entity already exists with a relationship of '" + relationshipType + "'.";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }
}
