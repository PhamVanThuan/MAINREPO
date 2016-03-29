namespace SAHL.Services.UserProfile.Managers
{
    public interface IUserOrgStructureDataManager
    {
        void AddLinkOfAdUserToOrgStructure(int adUserKey, int orgStructureKey);
        int FindLinkOfAdUserToOrgStructure(int adUserKey, int orgStructureKey);
        void RemoveLinkOfAdUserToOrgStructure(int userOrgStructureKey);


        //note fleshed out yet for method calls, or for parameters
        void CreateOrgStructureNode();
        void CreateAdUser();
        void DeleteOrgStructeNode();
        void DeleteAdUser();

        OrgStructreHierachy PullHierachyOfOrgStructure();
        OrgStructreHierachy PullHierachyOfOrgStructureFromNode();



    }
}