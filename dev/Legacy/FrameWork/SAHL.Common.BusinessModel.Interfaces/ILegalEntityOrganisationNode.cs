using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO
    /// </summary>
    public partial interface ILegalEntityOrganisationNode : IGenericOrganisationNode
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ChildOrganisationStructures
        /// </summary>
        IEventList<ILegalEntityOrganisationNode> ChildOrganisationStructures
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="OrganisationType"></param>
        /// <param name="OrganisationStructureDescription"></param>
        /// <returns></returns>
        ILegalEntityOrganisationNode FindChild(IOrganisationType OrganisationType, string OrganisationStructureDescription);

        /// <summary>
        /// Find a Child and return the type expected by OrganisationStructureNodeType
        /// </summary>
        /// <param name="OrganisationType"></param>
        /// <param name="OrganisationStructureDescription"></param>
        /// <param name="organisationStructureNodeType"></param>
        /// <returns></returns>
        ILegalEntityOrganisationNode FindChild(IOrganisationType OrganisationType, string OrganisationStructureDescription, OrganisationStructureNodeTypes organisationStructureNodeType);

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ADUsers
        /// </summary>
        IReadOnlyEventList<ILegalEntity> LegalEntities
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.Parent
        /// </summary>
        ILegalEntityOrganisationNode Parent
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="LegalEntity"></param>
        void RemoveLegalEntity(ILegalEntity LegalEntity);

        /// <summary>
        ///
        /// </summary>
        /// <param name="LegalEntity"></param>
        void AddLegalEntity(ILegalEntity LegalEntity);

        /// <summary>
        ///
        /// </summary>
        /// <param name="OrganisationType"></param>
        /// <param name="OrganisationStructureDescription"></param>
        /// <param name="organisationStructureNodeType"></param>
        /// <returns></returns>
        ILegalEntityOrganisationNode AddChildNode(IOrganisationType OrganisationType, string OrganisationStructureDescription, OrganisationStructureNodeTypes organisationStructureNodeType);

        ILegalEntityOrganisationNode GetOrgstructureParentItem(OrganisationTypes orgType, string description);

        ILegalEntityOrganisationNode GetOrgstructureParentItem(OrganisationTypes orgType);

        ILegalEntityOrganisationNode GetOrgstructureTopParentItem(OrganisationTypes orgType, int rootNodeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="newParent"></param>
        /// <returns></returns>
        ILegalEntityOrganisationNode MoveMe(ILegalEntityOrganisationNode newParent);

        /// <summary>
        ///
        /// </summary>
        /// <param name="LegalEntity"></param>
        void RemoveMe(ILegalEntity LegalEntity);

        /// <summary>
        ///
        /// </summary>
        bool HasActiveChildren { get; }
    }
}