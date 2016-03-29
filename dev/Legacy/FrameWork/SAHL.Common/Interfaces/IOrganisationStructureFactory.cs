using SAHL.Common.Globals;

namespace SAHL.Common.Interfaces
{
    public interface IOrganisationStructureFactory
    {
        OrganisationStructureNodeTypes OrganisationStructureNodeType { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="PersistanceObject"></param>
        /// <returns></returns>
        IBusinessModelObject GetBusinessModelInterface(object PersistanceObject);
    }
}