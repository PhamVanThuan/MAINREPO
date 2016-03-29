using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Common.DomainMessages;

namespace SAHL.Common.BusinessModel
{
    public class EstateAgentDesignationOrganisationNode : LegalEnitityDesignationOrganisationNode, IEstateAgentOrganisationNode
    {
        public EstateAgentDesignationOrganisationNode(SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO OrganisationStructure)
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
            string msg = "Items can not be added to Estate Agent Consultants or Principals.";
            spc.DomainMessages.Add(new SAHL.Common.DomainMessages.Error(msg, msg));
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ILegalEntityNaturalPerson GetEstateAgentPrincipal()
        {
            //Staring from the Consultant we are climbing up the orgstructure tree until we find a Principal.
            ILegalEntityOrganisationNode leon = base.GetOrgstructureParentItem(OrganisationTypes.Designation, Constants.EstateAgent.Principal);
            if (leon != null && leon.LegalEntities.Count > 0)
                return (ILegalEntityNaturalPerson)leon.LegalEntities[0];

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ILegalEntity GetBranch()
        {
            //Staring from the Consultant we are climbing up the orgstructure tree until we find a OrganisationTypes.Branch_Originator.
            ILegalEntityOrganisationNode leon = base.GetOrgstructureParentItem(OrganisationTypes.Branch_Originator);
            if (leon != null && leon.LegalEntities.Count == 1)
                return leon.LegalEntities[0];

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ILegalEntity GetAgency()
        {
            ILegalEntityOrganisationNode leon = base.GetOrgstructureParentItem(OrganisationTypes.Company);
            if (leon != null && leon.LegalEntities.Count == 1)
                return leon.LegalEntities[0];

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newParent"></param>
        /// <param name="legalEntity"></param>
        /// <returns></returns>
        public IEstateAgentOrganisationNode MoveMe(IEstateAgentOrganisationNode newParent, ILegalEntity legalEntity)
        {
            //Checks first
            if (newParent.OrganisationType.Key != (int)OrganisationTypes.Branch_Originator
                     && newParent.OrganisationType.Key != (int)SAHL.Common.Globals.OrganisationTypes.Company)
            {
                string msg = "Estate Agency Designation node can only be moved to a Branch or Company node.";
                spc.DomainMessages.Add(new Error(msg, msg));
                return null;
            }
            
            IOrganisationType ot = this.OrganisationType;
            string desc = this.Description;

            //if I am a Principal, 
            //and I am trying to add myself to a parent that has an active Pricipal Designation
            //dont move me
            if (this.Description == Constants.EstateAgent.Principal)
            {
                IEstateAgentOrganisationNode eaon = (IEstateAgentOrganisationNode)newParent.FindChild(ot, desc, OrganisationStructureNodeTypes.EstateAgent);
                if (eaon != null && eaon.GeneralStatus.Key == (int)GeneralStatuses.Active)
                {
                    string msg = String.Format(@"The target {0} already has a {1}, this item can not be moved.", newParent.Description, Constants.EstateAgent.Principal);
                    spc.DomainMessages.Add(new Error(msg, msg));
                    return null;
                }
            }

            //remove the provided le and set the designation node to inactive if required
            this.RemoveMe(legalEntity);
            //set this le up as a designation on the new Parent
            newParent.AddChildNode(legalEntity, ot, desc);

            return (IEstateAgentOrganisationNode)newParent.FindChild(ot, desc, OrganisationStructureNodeTypes.EstateAgent);
        }

        public IEstateAgentOrganisationNode Update(ILegalEntity legalEntity, IOrganisationType OrganisationType, string OrganisationStructureDescription)
        {
            //check if the designation is being swapped between Consultant and Principal
            if (this.Description != OrganisationStructureDescription)
            {
                IEstateAgentOrganisationNode parent = (IEstateAgentOrganisationNode)this.Parent;
                
                //Checks first
                if (parent.OrganisationType.Key != (int)OrganisationTypes.Branch_Originator
                         && parent.OrganisationType.Key != (int)SAHL.Common.Globals.OrganisationTypes.Company)
                {
                    string msg = "Estate Agency Designation node can only be saved to a Branch or Company node.";
                    spc.DomainMessages.Add(new Error(msg, msg));
                    return null;
                }

                //remove the provided le and set the designation node to inactive if required
                this.RemoveMe(legalEntity);
                //set this le up as a designation on the Parent
                parent.AddChildNode(legalEntity, OrganisationType, OrganisationStructureDescription);

                return (IEstateAgentOrganisationNode)parent.FindChild(OrganisationType, OrganisationStructureDescription, OrganisationStructureNodeTypes.EstateAgent);
            }

            return this;
        }

        #endregion
    }
}
