using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Property
{
    [RuleDBTag("PropertyInspectionContactDetails",
"If the inspection contact details are entered, both the name and number must be entered",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Property.PropertyInspectionContactDetails")]
    [RuleInfo]
    public class PropertyInspectionContactDetails : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The PropertyInspectionContactDetails rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IPropertyAccessDetails))
                throw new ArgumentException("The PropertyInspectionContactDetails rule expects the following objects to be passed: IPropertyAccessDetails.");

            IPropertyAccessDetails propertyAccessDetails = Parameters[0] as IPropertyAccessDetails;

            bool error = false;
            if (!String.IsNullOrEmpty(propertyAccessDetails.Contact1) && String.IsNullOrEmpty(propertyAccessDetails.Contact1Phone))
            {
                error = true;
                string message = "Inspection Telephone must be entered.";
                AddMessage(message, message, Messages);
            }
            if (!String.IsNullOrEmpty(propertyAccessDetails.Contact1Phone) && String.IsNullOrEmpty(propertyAccessDetails.Contact1))
            {
                error = true;
                string message = "Inspection Contact must be entered.";
                AddMessage(message, message, Messages);
            }
            if (!String.IsNullOrEmpty(propertyAccessDetails.Contact2) && String.IsNullOrEmpty(propertyAccessDetails.Contact2Phone))
            {
                error = true;
                string message = "Inspection Telephone 2 must be entered.";
                AddMessage(message, message, Messages);
            }
            if (!String.IsNullOrEmpty(propertyAccessDetails.Contact2Phone) && String.IsNullOrEmpty(propertyAccessDetails.Contact2))
            {
                error = true;
                string message = "Inspection Contact 2 must be entered.";
                AddMessage(message, message, Messages);
            }

            if (error)
                return 0;

            return 1;
        }
    }
}
