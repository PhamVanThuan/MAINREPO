using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Globals;
using SAHL.Common.DomainMessages;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Debt Counsellor Organisation Node
    /// </summary>
    public class DebtCounsellorOrganisationNode : LegalEntityOrganisationNode, IDebtCounsellorOrganisationNode
    {
        public DebtCounsellorOrganisationNode(OrganisationStructure_DAO OrganisationStructure)
            : base(OrganisationStructure)
        { }

        #region IDebtCounsellorOrganisationNode Members

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
        }

        public IDebtCounsellorOrganisationNode AddChildNode(ILegalEntity LegalEntity, IOrganisationType OrganisationType, string OrganisationStructureDescription)
        {
            //can we find a child with the same OT with the same desc
            ILegalEntityOrganisationNode leon = FindChild(OrganisationType, OrganisationStructureDescription, OrganisationStructureNodeTypes.DebtCounsellor);

            if (this.Parent == null && OrganisationType.Key == (int)OrganisationTypes.Department)
            {
                string msg = "Can not add a department to the ROOT NODE";
                spc.DomainMessages.Add(new Error(msg, msg));
                return null;
            }

            if (leon == null)
            {
                leon = base.AddChildNode(OrganisationType, OrganisationStructureDescription, OrganisationStructureNodeTypes.DebtCounsellor);
            }

            if (leon.GeneralStatus.Key != (int)GeneralStatuses.Active)
                leon.GeneralStatus = LKRepo.GeneralStatuses[GeneralStatuses.Active];


            leon.AddLegalEntity(LegalEntity);

            return (IDebtCounsellorOrganisationNode)leon;
        }

        public override void AddLegalEntity(ILegalEntity LegalEntity)
        {
            base.AddLegalEntity(LegalEntity);
        }

        public override void RemoveMe(ILegalEntity LegalEntity)
        {
            string msg = string.Empty;

            // 1. Check if safe to remove
            if (this.HasActiveChildren)
            {
                msg = "Cannot remove this node as it is linked to active child nodes.";
                spc.DomainMessages.Add(new Error(msg, msg));
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
            }

            
        }

        public IDebtCounsellorOrganisationNode MoveMe(IDebtCounsellorOrganisationNode newParent, ILegalEntity legalEntity)
        {
            string msg;
            bool valid = true;
            switch (this.OrganisationType.Key)
            {
                case (int)SAHL.Common.Globals.OrganisationTypes.Company:
                    if (newParent.OrganisationType.Key != (int)OrganisationTypes.Company && newParent.OrganisationType.Key != (int)OrganisationTypes.Region_Channel)
                    {
                        msg = "Company node can only be moved to a Company node.";
                        spc.DomainMessages.Add(new Error(msg, msg));
                        valid = false;
                    }
                    break;

                //case (int)SAHL.Common.Globals.OrganisationTypes.Department:
                //    if (newParent.OrganisationType.Key != (int)OrganisationTypes.Company)
                //    {
                //        msg = "Department node can only be moved to a Company node.";
                //        spc.DomainMessages.Add(new Error(msg, msg));
                //        valid = false;
                //    }
                //    break;
                default:
                    break;
            }

            if (!valid)
                return null;

            return (IDebtCounsellorOrganisationNode)base.MoveMe((ILegalEntityOrganisationNode)newParent);
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
