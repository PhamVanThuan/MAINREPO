using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel
{
    public class LegalEnitityDesignationOrganisationNode : LegalEntityOrganisationNode
    {
        public LegalEnitityDesignationOrganisationNode(SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO OrganisationStructure)
            : base(OrganisationStructure)
		{
		}

        #region ILegalEntityOrganisationNode Members

        public override void RemoveLegalEntity(ILegalEntity LegalEntity)
        {
            base.RemoveLegalEntity(LegalEntity);
        }

        public override ILegalEntityOrganisationNode AddChildNode(IOrganisationType OrganisationType, string OrganisationStructureDescription, OrganisationStructureNodeTypes organisationStructureNodeType)
        {
            throw new Exception("The method or operation AddLegalEntity (LE D ON) is not implemented.");
        }

        #endregion
    }
}
