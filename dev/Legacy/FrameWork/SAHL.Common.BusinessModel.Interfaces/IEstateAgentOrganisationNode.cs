namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface IEstateAgentOrganisationNode : ILegalEntityOrganisationNode
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="LegalEntity"></param>
        /// <param name="OrganisationType"></param>
        /// <param name="OrganisationStructureDescription"></param>
        /// <returns></returns>
        IEstateAgentOrganisationNode AddChildNode(ILegalEntity LegalEntity, IOrganisationType OrganisationType, string OrganisationStructureDescription);

        ILegalEntityNaturalPerson GetEstateAgentPrincipal();

        ILegalEntity GetBranch();

        ILegalEntity GetAgency();

        /// <summary>
        ///
        /// </summary>
        /// <param name="newParent"></param>
        /// <param name="legalEntity"></param>
        /// <returns></returns>
        IEstateAgentOrganisationNode MoveMe(IEstateAgentOrganisationNode newParent, ILegalEntity legalEntity);

        IEstateAgentOrganisationNode Update(ILegalEntity legalEntity, IOrganisationType OrganisationType, string OrganisationStructureDescription);
    }
}