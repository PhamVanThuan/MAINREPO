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

namespace SAHL.Common.BusinessModel
{
    public class EstateAgentOrganisationNode : LegalEntityOrganisationNode, IEstateAgentOrganisationNode
    {
        public EstateAgentOrganisationNode(SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO OrganisationStructure)
            : base(OrganisationStructure)
		{		}

        #region IEstateAgentOrganisationNode Members

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("LegelEntityEstateAgencyOnlyOnePrinciple");
            //Rules.Add("OneLegalEntityInstanceInOrgStructure");
            //Rules.Add("EstateAgentMultipleAgencies"); //changed to run on add only, rule runs from the presenter
            Rules.Add("LegalEntityEstateAgencyCheckForPrinciple");
        }

        public IEstateAgentOrganisationNode AddChildNode(ILegalEntity LegalEntity, IOrganisationType OrganisationType, string OrganisationStructureDescription)
        {
            //can we find a child with the same OT with the same desc
            ILegalEntityOrganisationNode leon = FindChild(OrganisationType, OrganisationStructureDescription, OrganisationStructureNodeTypes.EstateAgent);

            if (this.Parent == null && OrganisationType.Key == (int)OrganisationTypes.Designation)
            {
                string msg = "Can not add a designation (principal/consultant) to the ROOT NODE";
                spc.DomainMessages.Add(new Error(msg, msg));
                return null;
            }


            if (leon == null)
            {
                leon = base.AddChildNode(OrganisationType, OrganisationStructureDescription, OrganisationStructureNodeTypes.EstateAgent);
            }
            else if (OrganisationType.Key != (int)OrganisationTypes.Designation)
            {
                spc.DomainMessages.Add(new Error("This item already exists and can not be added.", "This item already exists and can not be added."));
                return null;
            }

            if (leon.GeneralStatus.Key != (int)GeneralStatuses.Active)
                leon.GeneralStatus = LKRepo.GeneralStatuses[GeneralStatuses.Active];


            leon.AddLegalEntity(LegalEntity);
            
            return (IEstateAgentOrganisationNode)leon;
        }

        public override void AddLegalEntity(ILegalEntity LegalEntity)
        {
            //string msg = "A Legal Entity can not be added to an Estate Agent Organisation Structure directly, please use AddChildNode.";
            //spc.DomainMessages.Add(new Error(msg, msg));
            base.AddLegalEntity(LegalEntity);
        }

        public override void RemoveMe(ILegalEntity LegalEntity)
        {
            string msg = string.Empty;

            // 1. Check if safe to remove
            if (this.HasActiveChildren)
            {
                    msg = "Cannot remove this node as it is linked to active child nodes.";
                    spc.DomainMessages.Add(new Error(msg,msg));
                    return;
            }

            // 2. All fine remove the LE
            this.RemoveLegalEntity(LegalEntity);

            // 3. Check there are no other LEs linked to OS
            if (this.LegalEntities.Count > 0)
                return;
            else
            {
                this.GeneralStatus = this.LKRepo.GeneralStatuses[GeneralStatuses.Inactive];
                //this.EARepo.SaveEstateAgentOrganisationStructure(this);
            }
        }

        public IEstateAgentOrganisationNode MoveMe(IEstateAgentOrganisationNode newParent, ILegalEntity legalEntity)
        {
            string msg;
            bool valid = true;
            switch (this.OrganisationType.Key)
            {
                case (int)SAHL.Common.Globals.OrganisationTypes.Branch_Originator:
                    if (newParent.OrganisationType.Key != (int)OrganisationTypes.Company)
                    {
                        msg = "Estate Agency Branch node can only be moved to a Company node.";
                        spc.DomainMessages.Add(new Error(msg, msg));
                        valid = false;
                    }
                    break;

                case (int)SAHL.Common.Globals.OrganisationTypes.Company:
                    if (newParent.OrganisationType.Key != (int)OrganisationTypes.Region_Channel
                     && newParent.OrganisationType.Key != (int)OrganisationTypes.Company)
                    {
                        msg = "Estate Agency Company node can only be moved to a Channel or Company node.";
                        spc.DomainMessages.Add(new Error(msg, msg));
                        valid = false;
                    }
                    break;

                case (int)SAHL.Common.Globals.OrganisationTypes.Region_Channel:
                    msg = "Estate Agency root node cannot be moved.";
                    spc.DomainMessages.Add(new Error(msg, msg));
                    valid = false;
                    break;

                default:
                    break;
            }

            if (!valid)
                return null;

            return (IEstateAgentOrganisationNode)base.MoveMe((ILegalEntityOrganisationNode)newParent);
        }

        public IEstateAgentOrganisationNode Update(ILegalEntity legalEntity, IOrganisationType OrganisationType, string OrganisationStructureDescription)
        {
            //the only update for a non-Designation item is the OT 
            //and the Description where the Desc = LE's DisplayName
            
            //remove any LE's that are not the one we are updating
            //foreach (ILegalEntity le in this.LegalEntities)
            //{
            //    if (le.Key != legalEntity.Key)
            //        base.RemoveLegalEntity(le);
            //}

            //if (this.LegalEntities.Count == 0)
            //    base.AddLegalEntity(legalEntity);

            this.OrganisationType = OrganisationType;
            this.Description = legalEntity.DisplayName;

            return this;
        }

        public ILegalEntityNaturalPerson GetEstateAgentPrincipal()
        {
            //check if we have the child we are after
            foreach (IEstateAgentOrganisationNode eaon in this.ChildOrganisationStructures)
            {
                if (eaon.OrganisationType.Key == (int)OrganisationTypes.Designation 
                    && eaon.GeneralStatus.Key == (int)GeneralStatuses.Active
                    && eaon.LegalEntities.Count > 0)
                {
                    return (ILegalEntityNaturalPerson)eaon.LegalEntities[0];
                }
            }

            //didnt find it so recurse up the orgstructure tree until we find a Principal.
            ILegalEntityOrganisationNode leon = base.GetOrgstructureParentItem(OrganisationTypes.Designation, Constants.EstateAgent.Principal);

            if (leon != null && leon.LegalEntities.Count > 0)
                return (ILegalEntityNaturalPerson)leon.LegalEntities[0];

            return null;
        }

        public ILegalEntity GetBranch()
        {
            ILegalEntityOrganisationNode leon = base.GetOrgstructureParentItem(OrganisationTypes.Branch_Originator);
            if (leon != null && leon.LegalEntities.Count == 1)
                return leon.LegalEntities[0];

            return null;
        }

        public ILegalEntity GetAgency()
        {
            ILegalEntityOrganisationNode leon = base.GetOrgstructureParentItem(OrganisationTypes.Company);
            if (leon != null && leon.LegalEntities.Count == 1)
                return leon.LegalEntities[0];

            return null;
        }

        #endregion

    }
}
