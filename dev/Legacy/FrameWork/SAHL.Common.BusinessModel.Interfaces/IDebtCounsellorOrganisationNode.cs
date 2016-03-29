namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Debt Counsellor Organisation Structure Node
    /// </summary>
    public interface IDebtCounsellorOrganisationNode : ILegalEntityOrganisationNode
    {        /// <summary>
        ///
        /// </summary>
        /// <param name="LegalEntity"></param>
        /// <param name="OrganisationType"></param>
        /// <param name="OrganisationStructureDescription"></param>
        /// <returns></returns>
        IDebtCounsellorOrganisationNode AddChildNode(ILegalEntity LegalEntity, IOrganisationType OrganisationType, string OrganisationStructureDescription);

        /// <summary>
        ///
        /// </summary>
        /// <param name="newParent"></param>
        /// <param name="legalEntity"></param>
        /// <returns></returns>
        IDebtCounsellorOrganisationNode MoveMe(IDebtCounsellorOrganisationNode newParent, ILegalEntity legalEntity);

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="OrganisationType"></param>
        /// <param name="OrganisationStructureDescription"></param>
        /// <returns></returns>
        IDebtCounsellorOrganisationNode Update(ILegalEntity legalEntity, IOrganisationType OrganisationType, string OrganisationStructureDescription);
    }
}