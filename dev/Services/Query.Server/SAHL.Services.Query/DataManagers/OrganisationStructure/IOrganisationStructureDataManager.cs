using System.Collections.Generic;
using SAHL.Core.Data.Models._2AM;
using OrganisationStructureDataModel = SAHL.Services.Query.Models.OrganisationStructure.OrganisationStructureDataModel;

namespace SAHL.Services.Query.DataManagers.OrganisationStructure
{
    public interface IOrganisationStructureDataManager
    {
        List<OrganisationStructureDataModel> GetOrganisationStructureByParentKey(int? parentOrganisationStructureId);
        OrganisationStructureDataModel GetOrganisationStructure(int id);
        List<OrganisationTypeDataModel> GetOrganisationTypes();
        OrganisationTypeDataModel GetOrganisationType(int id);
    }
}