using System;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IEstateAgentRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="eaos"></param>
        void SaveEstateAgentOrganisationStructure(IEstateAgentOrganisationNode eaos);

        void GetEstateAgentInfoWithHistory(int legalEntityKey, DateTime maxDate, out ILegalEntity Company, out ILegalEntity Branch, out ILegalEntity Principal);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IEstateAgentOrganisationNode GetEstateAgentOrganisationNodeForKey(int Key);

        /// <summary>
        /// Get the IEstateAgentOrganisationNode for the LegalEntityKey provided
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IEstateAgentOrganisationNode GetEstateAgentOrganisationNodeForLegalEntity(int key);
    }
}