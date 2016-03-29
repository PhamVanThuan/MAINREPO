using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.DAO;

namespace SAHL.Common.BusinessModel
{
    public class DebtCounsellorDesignationOrganisationNode : LegalEntityOrganisationNode, IDebtCounsellorOrganisationNode
    {
        public DebtCounsellorDesignationOrganisationNode(OrganisationStructure_DAO OrganisationStructure)
            : base(OrganisationStructure)
		{		}

        #region IDebtCounsellorOrganisationNode Members

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
        }

        public IDebtCounsellorOrganisationNode AddChildNode(ILegalEntity LegalEntity, IOrganisationType OrganisationType, string OrganisationStructureDescription)
        {
            string msg = "Items can not be added to Contacts.";
            spc.DomainMessages.Add(new SAHL.Common.DomainMessages.Error(msg, msg));
            return null;
        }

        public override void RemoveMe(ILegalEntity LegalEntity)
        {
            // 1. Remove LE
            base.RemoveLegalEntity(LegalEntity);

            // 2. Check if Parent OS has children attached
            if (this.LegalEntities.Count > 0)
                return;
            else
            {
                this.GeneralStatus = this.LKRepo.GeneralStatuses[GeneralStatuses.Inactive];
            }
        }

        public IDebtCounsellorOrganisationNode MoveMe(IDebtCounsellorOrganisationNode newParent, ILegalEntity legalEntity)
        {
            //Checks first
            if (newParent.OrganisationType.Key != (int)OrganisationTypes.Company
                && newParent.OrganisationType.Key != (int)SAHL.Common.Globals.OrganisationTypes.Branch_Originator
                && newParent.OrganisationType.Key != (int)SAHL.Common.Globals.OrganisationTypes.Department)
            {
                string msg = "Debt Counsellor Contacts can only be moved to a Company, Branch or Department node.";
                spc.DomainMessages.Add(new Error(msg, msg));
                return null;
            }


            IOrganisationType ot = this.OrganisationType;
            string desc = this.Description;

            //remove the provided le and set the designation node to inactive if required
            this.RemoveMe(legalEntity);

            //set this le up as a designation on the new Parent
            newParent.AddChildNode(legalEntity, ot, desc);

            return (IDebtCounsellorOrganisationNode)newParent.FindChild(ot, desc, OrganisationStructureNodeTypes.DebtCounsellor);
        }

        public IDebtCounsellorOrganisationNode Update(ILegalEntity legalEntity, IOrganisationType OrganisationType, string OrganisationStructureDescription)
        {
            this.OrganisationType = OrganisationType;
            this.Description = OrganisationStructureDescription;

            return this;
        }

        #endregion
    }
}
