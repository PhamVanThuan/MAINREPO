using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.UserProfile.Managers
{
    public class UserOrgStructureDataManager : IUserOrgStructureDataManager
    {
        private DbFactory dbFactory;

        public UserOrgStructureDataManager(DbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void AddLinkOfAdUserToOrgStructure(int adUserKey, int orgStructureKey)
        {
            using (var dataContext = dbFactory.NewDb().InAppContext())
            {
                dataContext.Insert<UserOrganisationStructureDataModel>(new InsertNewUserIntoStructureStatement(adUserKey, orgStructureKey));
            }
        }

        public int FindLinkOfAdUserToOrgStructure(int adUserKey, int orgStructureKey)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveLinkOfAdUserToOrgStructure(int userOrgStructureKey)
        {
            throw new System.NotImplementedException();
        }

        public void CreateOrgStructureNode()
        {
            throw new System.NotImplementedException();
        }

        public void CreateAdUser()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteOrgStructeNode()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAdUser()
        {
            throw new System.NotImplementedException();
        }

        public OrgStructreHierachy PullHierachyOfOrgStructure()
        {
            throw new System.NotImplementedException();
        }

        public OrgStructreHierachy PullHierachyOfOrgStructureFromNode()
        {
            throw new System.NotImplementedException();
        }
    }

    public class InsertNewUserIntoStructureStatement : ISqlStatement<UserOrganisationStructureDataModel>
    {
        public int AdUserKey { get; private set; }
        public int OrgStructureKey { get; private set; }

        public InsertNewUserIntoStructureStatement(int adUserKey, int orgStructureKey)
        {
            AdUserKey = adUserKey;
            OrgStructureKey = orgStructureKey;
        }

        public string GetStatement()
        {
            return "select 1";
        }
    }
}