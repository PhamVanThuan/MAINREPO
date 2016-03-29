using System.Security.Cryptography;
using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.UserProfile.Managers.Statements;

namespace SAHL.Services.UserProfile.Managers
{
    public class OrganisationStructureDataMangager : IOrganisationStructreDataMangager
    {

        private readonly IDbFactory dbFactory;

        public OrganisationStructureDataMangager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }


        public void AddNewLevelAtPoint(int parentKey, int organisationTypeKey, string description)
        {
            using (var db = NewInAppContext())
            {
                db.Insert(new OrganisationStructureDataModel(parentKey, description, organisationTypeKey, 1));
                db.Complete();
            }
        }

        public void RemoveLevelFromStructure(int organisationTypeKey)
        {
            using (var db = NewInAppContext())
            {
                db.DeleteByKey<OrganisationStructureDataModel,int>(organisationTypeKey);
                db.Complete();
            }
        }

        public void RenameLevelInStructure(int organisationTypeKey, string description)
        {
            using (var db = NewInAppContext())
            {
                ISqlStatement<OrganisationStructureDataModel> statement = new RenameOrganisationStructureLevelStatement(organisationTypeKey, description);
                db.Update<OrganisationStructureDataModel>(statement);
                db.Complete();
            }
        }

        public void MoveOrganisationStructurelevel(int organisationTypeKey, int newParentOrganisationTypeKey)
        {
            using (var db = NewInAppContext())
            {
                ISqlStatement<OrganisationStructureDataModel> statement = new MoveOrganisationLevelToDifferentParentStatement(organisationTypeKey, newParentOrganisationTypeKey);
                db.Update<OrganisationStructureDataModel>(statement);
                db.Complete();
            }
        }

        private IDbContext NewInAppContext()
        {
            return dbFactory.NewDb().InAppContext();
        }
    }
}