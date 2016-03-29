namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface IPaymentDistributionAgentOrganisationNode : ILegalEntityOrganisationNode
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="LegalEntity"></param>
        /// <param name="OrganisationType"></param>
        /// <param name="OrganisationStructureDescription"></param>
        /// <returns></returns>
        IPaymentDistributionAgentOrganisationNode AddChildNode(ILegalEntity LegalEntity, IOrganisationType OrganisationType, string OrganisationStructureDescription);

        /// <summary>
        ///
        /// </summary>
        /// <param name="newParent"></param>
        /// <param name="legalEntity"></param>
        /// <returns></returns>
        IPaymentDistributionAgentOrganisationNode MoveMe(IPaymentDistributionAgentOrganisationNode newParent, ILegalEntity legalEntity);

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="OrganisationType"></param>
        /// <param name="OrganisationStructureDescription"></param>
        /// <returns></returns>
        IPaymentDistributionAgentOrganisationNode Update(ILegalEntity legalEntity, IOrganisationType OrganisationType, string OrganisationStructureDescription);
    }
}