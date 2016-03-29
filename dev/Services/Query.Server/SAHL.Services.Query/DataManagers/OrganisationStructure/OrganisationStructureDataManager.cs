using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Query.DataManagers.OrganisationStructure.Statements;
using OrganisationStructureDataModel = SAHL.Services.Query.Models.OrganisationStructure.OrganisationStructureDataModel;

namespace SAHL.Services.Query.DataManagers.OrganisationStructure
{
    public class OrganisationStructureDataManager : IOrganisationStructureDataManager
    {
        private readonly IDbFactory dbFactory;

        public OrganisationStructureDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public List<OrganisationStructureDataModel> GetOrganisationStructureByParentKey(int? parentOrganisationStructureId)
        {
            var query = new GetOrganisationStructureByParentKeyStatement(parentOrganisationStructureId);
            using (var context = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return context.Select(query).ToList();
            }
        }

        public OrganisationStructureDataModel GetOrganisationStructure(int id)
        {
            var query = new GetOrganisationStructureStatement(id);
            using (var context = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return context.SelectOne(query);
            }
        }

        public List<OrganisationTypeDataModel> GetOrganisationTypes()
        {
            var query = new GetOrganisationTypesStatement();
            using (var context = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return context.Select(query).ToList();
            }
        }

        public OrganisationTypeDataModel GetOrganisationType(int id)
        {
            var query = new GetOrganisationTypeStatement(id);
            using (var context = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return context.SelectOne(query);
            }
        }
    }
}
