using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public interface IOrganisationStructureRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="AppRole"></param>
        /// <param name="InstanceID"></param>
        void DeactivateWorkflowAssignment(IApplicationRole AppRole, int InstanceID);

        /// <summary>
        ///
        /// </summary>
        /// <param name="UserOrganisationStructureKey"></param>
        /// <returns></returns>
        IAllocationMandateSetGroup GetAllocationMandateSetGroupByUserOrganisationStructureKey(int UserOrganisationStructureKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="InstanceID"></param>
        /// <param name="AppRole"></param>
        /// <returns></returns>
        bool HasActiveWorkflowAssignment(int InstanceID, IApplicationRole AppRole);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        DataSet GetOrganisationStructureAllDSForKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="InstanceID"></param>
        /// <param name="OfferRoleTypeOrganisationStructureMappingKey"></param>
        /// <param name="ADUserKey"></param>
        /// <param name="GeneralStatusKey"></param>
        /// <returns></returns>
        DataTable GetWorkflowAssignment(int InstanceID, int OfferRoleTypeOrganisationStructureMappingKey, int ADUserKey, int GeneralStatusKey);

        /// <summary>
        /// Returns a list of ADUsers for a OrganisationStructure
        /// </summary>
        /// <param name="OrganisationStructureKey"></param>
        /// <returns></returns>
        IEventList<IADUser> GetUsersForOrganisationStructureKey(int OrganisationStructureKey);

        /// <summary>
        /// Returns a list of ADUsers for a OrganisationStructure.
        /// </summary>
        /// <param name="OrganisationStructureKey"></param>
        /// <param name="recursive">If true, this will return ALL users below the node.</param>
        /// <returns></returns>
        IEventList<IADUser> GetUsersForOrganisationStructureKey(int OrganisationStructureKey, bool recursive);

        /// <summary>
        /// Gets an ADUser for a given key
        /// </summary>
        /// <param name="ADUserKey"></param>
        /// <returns></returns>
        IADUser GetADUserByKey(int ADUserKey);

        /// <summary>
        /// Gets a complete list of all AdUsers in the DB.
        /// </summary>
        /// <returns></returns>
        IEventList<IADUser> GetCompleteAdUserList();

        /// <summary>
        /// Gets an AdUser object for a legalentitykey. Null if not found. LEKey should always be unique
        /// </summary>
        /// <param name="LEKey"></param>
        /// <returns></returns>
        IADUser GetAdUserByLegalEntityKey(int LEKey);

        /// <summary>
        /// Returns a list of AdUsers for a partial Name search
        /// </summary>
        /// <param name="PartialName"></param>
        /// <param name="maxRecords">The maximum number of records to return.</param>
        /// <returns></returns>
        IEventList<IADUser> GetAdUsersByPartialName(string PartialName, int maxRecords);

        /// <summary>
        /// Creates a new Empty IAdUser
        /// </summary>
        /// <returns></returns>
        IADUser CreateEmptyAdUser();

        /// <summary>
        /// Saves a newly created / updates AdUser object to the DB.
        /// </summary>
        /// <param name="AdUser"></param>
        void SaveAdUser(IADUser AdUser);

        /// <summary>
        /// Gets an IAduser for the logon name IE SAHL\\PaulC
        /// </summary>
        /// <param name="SAHLName"></param>
        /// <returns></returns>
        IADUser GetAdUserForAdUserName(string SAHLName);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IFeature CreateEmptyFeature();

        /// <summary>
        ///
        /// </summary>
        /// <param name="feature"></param>
        void SaveFeature(IFeature feature);

        /// <summary>
        /// Gets a list of top level features that do not have a parent. This is generally used
        /// </summary>
        /// <returns></returns>
        IEventList<IFeature> GetTopLevelFeatureList();

        /// <summary>
        /// Gets the top level organisation structure for an originationsource. 1(SAHL) will return
        /// </summary>
        /// <param name="OSPKey"></param>
        /// <returns></returns>
        IEventList<IOrganisationStructure> GetTopLevelOrganisationStructureForOriginationSource(int OSPKey);

        /// <summary>
        /// Returns the list of companysSouth African Home Loans (Pty) Ltd SAHL Life Assurance Company Limited Imperial RCS Blakes
        /// </summary>
        /// <returns></returns>
        IEventList<IOrganisationStructure> GetCompanyList();

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IOrganisationStructure GetOrganisationStructureForKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        IOrganisationStructure GetOrganisationStructureForDescription(string Description);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        IOrganisationStructure GetRootOrganisationStructureForDescription(string Description);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IOrganisationStructure CreateEmptyOrganisationStructure();

        /// <summary>
        ///
        /// </summary>
        /// <param name="leos"></param>
        void SaveLegalEntityOrganisationNode(ILegalEntityOrganisationNode leos);

        /// <summary>
        ///
        /// </summary>
        /// <param name="os"></param>
        void SaveOrganisationStructure(IOrganisationStructure os);

        /// <summary>
        ///
        /// </summary>
        IEventList<IFeature> GetCompleteFeatureList();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEventList<IFeatureGroup> GetDistinctFeatureGroupList();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEventList<IFeatureGroup> GetCompleteFeatureGroupList();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEventList<IFeatureGroup> GetFeatureGroupsForUserRoles(string userRoles);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEventList<IFeature> GetFeaturesForUserRoles(string userRoles);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEventList<ICBOMenu> GetTopLevelCBONodes();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEventList<IContextMenu> GetTopLevelContextMenuNodes();

        /// <summary>
        ///
        /// </summary>
        /// <param name="cm"></param>
        void SaveContextMenu(IContextMenu cm);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IContextMenu CreateEmptyContextMenu();

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IContextMenu GetContextMenuByKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ICBOMenu CreateEmptyCBO();

        /// <summary>
        ///
        /// </summary>
        /// <param name="cbo"></param>
        void SaveCBO(ICBOMenu cbo);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IFeature GetFeatureByKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        ICBOMenu GetCBOByKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        IApplicationRoleType GetApplicationRoleTypeByName(string Name);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IApplicationRoleType GetApplicationRoleTypeByKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IApplicationRole CreateNewApplicationRole();

        /// <summary>
        ///
        /// </summary>
        /// <param name="approle"></param>
        void SaveApplicationRole(IApplicationRole approle);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="LegalEntityKey"></param>
        /// <returns></returns>
        IEventList<IApplicationRole> FindApplicationRolesForApplicationKeyAndLEKey(int ApplicationKey, int LegalEntityKey);

        /// <summary>
        /// Finds all active application roles of a role type against an application
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="AppRoleTypeKey"></param>
        /// <returns></returns>
        IEventList<IApplicationRole> GetActiveAssignedApplicationRoles(int ApplicationKey, int AppRoleTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="MaxResults"></param>
        /// <returns></returns>
        IEventList<IApplicationRole> GetTopXApplicationRolesForApplicationKey(int ApplicationKey, int MaxResults);

        /// <summary>
        ///
        /// </summary>
        /// <param name="LEKeys"></param>
        /// <returns></returns>
        IEventList<IApplicationRole> GetTopXApplicationRolesForLEKeys(List<int> LEKeys);

        /// <summary>
        /// Gets the latest entry from the OfferRole table for an offerkey
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        IApplicationRole GetTopApplicationRoleForApplicationKey(int ApplicationKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="adUserName"></param>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        IApplicationRole GetApplicationRoleForADUserAndApplication(string adUserName, int applicationKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="adUserName"></param>
        /// <param name="appRoleTypeKey"></param>
        /// <returns></returns>
        IApplicationRole GetApplicationRoleByADUserRoleType(int applicationKey, string adUserName, int appRoleTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationRole"></param>
        /// <returns></returns>
        IOrganisationStructure GetOrganisationStructForADUser(IApplicationRole applicationRole);

        /// <summary>
        /// Returns a list of Application Roles the application can possibly be assigned to based on Current State
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="orgStruct"></param>
        /// <param name="ignoreX2Filter"></param>
        /// <returns></returns>
        IEventList<IApplicationRoleType> GetAppRoleTypesBasedOnAppAndState(long instanceID, IOrganisationStructure orgStruct, bool ignoreX2Filter);

        /// <summary>
        /// Returns a list of Application Roles the application can possibly be assigned to based on Next State
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="ActivityName"></param>
        /// <param name="orgStruct"></param>
        /// <param name="ignoreX2Filter"></param>
        /// <returns></returns>
        IEventList<IApplicationRoleType> GetAppRoleTypesBasedOnAppAndNextState(long instanceID, string ActivityName, IOrganisationStructure orgStruct, bool ignoreX2Filter);

        /// <summary>
        /// Gets a list of ADUsers for a specific role in a specific point in the organisation structure
        /// </summary>
        /// <param name="art"></param>
        /// <param name="orgStruct"></param>
        /// <returns></returns>
        IEventList<IADUser> GetUsersForRoleTypeAndOrgStruct(IApplicationRoleType art, IOrganisationStructure orgStruct);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="ApplicationRoleTypeKey"></param>
        /// <param name="GeneralStatusKey"></param>
        /// <returns></returns>
        IApplicationRole GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(int ApplicationKey, int ApplicationRoleTypeKey, int GeneralStatusKey);

        /// <summary>
        /// Marks an Application Role as inactive. This is called when a workflow case is assigned to a new user and the previous
        /// user is no longer required to have the case on their worklist
        /// </summary>
        /// <param name="role"></param>
        void MarkRoleAsInactive(IApplicationRole role);

        #region Helpdesk Hacks due to orgstructure wierdness

        /// <summary>
        /// Gets a list of adusers who are part of HELPDESK PROCESSOR ORg structure
        /// </summary>
        /// <param name="OrgStructureName"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        IList<IADUser> GetAllHelpdeskConsultants(string OrgStructureName, IADUser currentUser);

        ///// <summary>
        ///// Updates the X2Data.Help_Desk table setting the newly assigned consultant. Helpdesk doesnt
        ///// fit into the offerrole setup and helpdesk is not based on applicationkey.
        ///// </summary>
        ///// <param name="UserToAssignTo"></param>
        ///// /// <param name="InstanceID"></param>
        //void AssignNewHelpDeskUser(IADUser UserToAssignTo, Int64 InstanceID);

        #endregion Helpdesk Hacks due to orgstructure wierdness

        /// <summary>
        ///
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orgStructure"></param>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        IEventList<IADUser> GetBranchUsersForUserInThisBranch(IADUser user, OrganisationStructureGroup orgStructure, string Prefix);

        /// <summary>
        /// Gets the users in a dynamic roel via the org structure
        /// NB THIS ONLY WORKS WHERE THE DYNAMIC ROLE IS MAPPED TO ONE AND ONLY ONE ORGSTRUCTURE BRANCH (x) WILL NOT WORK
        /// </summary>
        /// <param name="art"></param>
        /// <returns></returns>
        IEventList<IADUser> GetUsersForDynamicRole(IApplicationRoleType art);

        /// <summary>
        ///
        /// </summary>
        /// <param name="art"></param>
        /// <param name="LookAtParent"></param>
        /// <returns></returns>
        IEventList<IADUser> GetUsersForDynamicRole(IApplicationRoleType art, bool LookAtParent);

        /// <summary>
        ///
        /// </summary>
        /// <param name="art"></param>
        /// <param name="LookAtParent"></param>
        /// <returns></returns>
        IEventList<IADUser> GetAllUsersForDynamicRole(IApplicationRoleType art, bool LookAtParent);

        /// <summary>
        /// Gets the users in a dynamic role via the org structure
        /// </summary>
        /// <param name="art"></param>
        /// <param name="orgStructure"></param>
        /// <returns></returns>
        IEventList<IADUser> GetUsersForDynamicRole(IApplicationRoleType art, IOrganisationStructure orgStructure);

        /// <summary>
        /// Gets the dynamic role types
        /// </summary>
        /// <param name="userOrgansationStructures"></param>
        /// <returns></returns>
        IList<IApplicationRoleType> GetDynamicRoleTypes(IEventList<IUserOrganisationStructure> userOrgansationStructures);

        /// <summary>
        ///
        /// </summary>
        /// <param name="appRole"></param>
        /// <returns></returns>
        IOrganisationStructure GetBranchForConsultant(IApplicationRole appRole);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IAttorney GetAttorneyByLegalEntityKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="aduser"></param>
        /// <returns></returns>
        IList<IOrganisationStructure> GetOrgStructsPerADUser(IADUser aduser);

        /// <summary>
        ///
        /// </summary>
        /// <param name="orgList"></param>
        /// <returns></returns>
        IList<IApplicationRoleType> GetAppRoleTypesForOrgStructList(IList<IOrganisationStructure> orgList);

        /// <summary>
        ///
        /// </summary>
        /// <param name="orgList"></param>
        /// <returns></returns>
        IList<IWorkflowRoleType> GetWorkflowRoleTypesForOrgStructList(IList<IOrganisationStructure> orgList);

        /// <summary>
        ///
        /// </summary>
        /// <param name="appRoleType"></param>
        /// <param name="orgList"></param>
        /// <param name="filtered"></param>
        /// <returns></returns>
        IEventList<IADUser> GetADUsersPerRoleTypeAndOrgStructList(IApplicationRoleType appRoleType, IList<IOrganisationStructure> orgList, bool filtered);

        /// <summary>
        /// Gets a list of IOrganisationStructureOriginationSource for an ADUser.
        /// </summary>
        /// <param name="adUserName"></param>
        /// <returns></returns>
        IList<IOrganisationStructureOriginationSource> GetOrgStructureOriginationSourcesPerADUser(string adUserName);

        /// <summary>
        /// Gets a list of OriginationSourceKeys for an ADUser
        /// </summary>
        /// <param name="adUserName"></param>
        /// <returns></returns>
        List<int> GetOriginationSourceKeysPerADUser(string adUserName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="adUserName"></param>
        /// <returns></returns>
        IApplicationRole GetApplicationRoleForADUser(int applicationKey, string adUserName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="adUserName"></param>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        IEventList<IApplicationRole> GetApplicationRolesByAppKey(int applicationKey, string adUserName, long instanceID);

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationRoleKey"></param>
        /// <returns></returns>
        IApplicationRole GetApplicationRoleByKey(int applicationRoleKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="orgStruct"></param>
        /// <param name="appRole"></param>
        /// <returns></returns>
        IEventList<IADUser> GetADUserPerOrgStructAndAppRole(IOrganisationStructure orgStruct, IApplicationRole appRole);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="ApplicationRoleTypeKey"></param>
        /// <returns></returns>
        DataTable GetReassignUserApplicationRoleList(int ApplicationKey, int ApplicationRoleTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectedNodesLst"></param>
        /// <returns></returns>
        DataTable GetOrganisationStructureConfirmationList(List<int> selectedNodesLst);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ADUserKey"></param>
        /// <returns></returns>
        DataTable GetUserOrganisationStructureHistory(int ADUserKey);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IUserOrganisationStructure GetEmptyUserOrganisationStructure();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IUserOrganisationStructureHistory GetEmptyUserOrganisationStructureHistory();

        /// <summary>
        ///
        /// </summary>
        /// <param name="uos"></param>
        void SaveUserOrganisationStructure(IUserOrganisationStructure uos);

        /// <summary>
        ///
        /// </summary>
        /// <param name="uosh"></param>
        void SaveUserOrganisationStructureHistory(IUserOrganisationStructureHistory uosh);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationRoleTypeKey"></param>
        /// <param name="ApplicationKey"></param>
        /// <param name="LegalEntityKey"></param>
        /// <param name="ReUse"></param>
        /// <returns></returns>
        IApplicationRole GenerateApplicationRole(int ApplicationRoleTypeKey, int ApplicationKey, int LegalEntityKey, bool ReUse);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationRoleKey"></param>
        void DeactivateApplicationRole(int ApplicationRoleKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="ApplicationRoleTypeKey"></param>
        void DeactivateExistingApplicationRoles(int ApplicationKey, int ApplicationRoleTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="ApplicationRoleTypeKey"></param>
        /// <param name="LegalEntityKey"></param>
        /// <param name="GeneralStatusKey"></param>
        /// <returns></returns>
        IApplicationRole GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyAndLegalEntityKey(int ApplicationKey, int ApplicationRoleTypeKey, int LegalEntityKey, int GeneralStatusKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="LegalEntityKey"></param>
        /// <param name="ApplicationRoleTypeKey"></param>
        /// <param name="GeneralStatusKey"></param>
        /// <returns></returns>
        IApplicationRole GetLatestApplicationRoleByLegalEntityAndApplicationRoleTypeKey(int LegalEntityKey, int ApplicationRoleTypeKey, int GeneralStatusKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="appRoleType"></param>
        /// <param name="orgList"></param>
        /// <returns></returns>
        Dictionary<IADUser, int> GetADUsersPerRoleTypeAndOrgStructDictionary(IApplicationRoleType appRoleType, IList<IOrganisationStructure> orgList);

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflowRoleType"></param>
        /// <param name="orgList"></param>
        /// <returns></returns>
        Dictionary<IADUser, int> GetADUsersPerWorkflowRoleTypeAndOrgStructDictionary(IWorkflowRoleType workflowRoleType, IList<IOrganisationStructure> orgList);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="ApplicationRoleTypeKey"></param>
        /// <param name="ApplicationRoleKey"></param>
        void DeactivateExistingApplicationRoles(int ApplicationKey, int ApplicationRoleTypeKey, int ApplicationRoleKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ADUserKey"></param>
        /// <param name="OrganisationStructureKey"></param>
        /// <returns></returns>
        IList<IOrganisationStructure> GetOrgStructsPerADUserAndCompany(int ADUserKey, int OrganisationStructureKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationRoleKey"></param>
        /// <returns></returns>
        int GetOfferRoleTypeOrganisationStructureMappingKey(int ApplicationRoleKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="organisationStructureKey"></param>
        /// <param name="offerRoleTypeKey"></param>
        /// <returns></returns>
        int GetOfferRoleTypeOrganisationStructureMappingKey(int organisationStructureKey, int offerRoleTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AppRole"></param>
        /// <param name="InstanceID"></param>
        /// <param name="GeneralStatus"></param>
        void CreateWorkflowAssignment(IApplicationRole AppRole, int InstanceID, GeneralStatuses GeneralStatus);

        /// <summary>
        ///
        /// </summary>
        /// <param name="OrganisationStructureKey"></param>
        IEventList<IApplicationRoleType> GetCreditRoleTypes(int OrganisationStructureKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="OrganisationStructureKey"></param>
        IEventList<IADUser> GetUsersPerOrgStruct(int OrganisationStructureKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="instanceID"></param>
        /// <param name="aduserKey"></param>
        /// <param name="applicationRole"></param>
        /// <param name="message"></param>
        void ReassignUser(int applicationKey, long instanceID, int aduserKey, IApplicationRole applicationRole, out string message);

        /// <summary>
        ///
        /// </summary>
        /// <param name="appRoleTypeKey"></param>
        /// <param name="orgList"></param>
        /// <returns></returns>
        DataTable GetADUsersPerRoleTypeAndOrgStructListDT(int appRoleTypeKey, IList<IOrganisationStructure> orgList);

        /// <summary>
        ///
        /// </summary>
        /// <param name="uosrrsKey"></param>
        /// <returns></returns>
        IUserOrganisationStructureRoundRobinStatus GetUserOrganisationStructureRoundRobinStatus(int uosrrsKey);

        IOrganisationStructure GetOrgstructureParentLevel(IOrganisationStructure orgItem, int orgType);

        IOrganisationStructure GetOrgstructureTopParentLevel(IOrganisationStructure orgItem, int orgType, int orgStructureRootKey);

        IOrganisationStructure GetOrganisationStructureForLE(int leKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="userOrganisationStructureRoundRobinStatus"></param>
        void SaveUserOrganisationStructureRoundRobinStatus(IUserOrganisationStructureRoundRobinStatus userOrganisationStructureRoundRobinStatus);

        /// <summary>
        ///
        /// </summary>
        /// <param name="uos"></param>
        /// <param name="startDate"></param>
        /// <param name="messages"></param>
        void SaveUserOrganisationStructure(IUserOrganisationStructure uos, DateTime startDate, IDomainMessageCollection messages);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IUserOrganisationStructureRoundRobinStatus GetEmptyUserOrganisationStructureRoundRobinStatus();

        ILegalEntityOrganisationNode GetLegalEntityOrganisationNodeForKey(int Key);

        IList<ILegalEntityOrganisationStructure> GetLegalEntityOrganisationStructuresForLegalEntityKey(int legalEntityKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="roleTypeKey"></param>
        /// <returns></returns>
        int GetLegalEntityOrganisationKeyByApplicationAndRoleType(int appKey, int roleTypeKey);

        /// <summary>
        /// Returns a list of LegalEntities for a OrganisationStructure
        /// </summary>
        /// <param name="OrganisationStructureKey"></param>
        /// <returns></returns>
        IEventList<ILegalEntity> GetLegalEntitiesForOrganisationStructureKey(int OrganisationStructureKey);

        /// <summary>
        /// Returns a list of LegalEntities for a OrganisationStructure.
        /// </summary>
        /// <param name="OrganisationStructureKey"></param>
        /// <param name="recursive">If true, this will return ALL users below the node.</param>
        /// <returns></returns>
        IEventList<ILegalEntity> GetLegalEntitiesForOrganisationStructureKey(int OrganisationStructureKey, bool recursive);

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflowRoleTypes"></param>
        /// <returns></returns>
        DataTable GetUsersForWorkflowRoleType(IList<SAHL.Common.Globals.WorkflowRoleTypes> workflowRoleTypes);

        /// <summary>
        ///
        /// </summary>
        /// <param name="mortgageLoanApplication"></param>
        /// <returns></returns>
        string GetLifeConsultantADUserNameFromMandate(IApplication mortgageLoanApplication);

        /// <summary>
        /// This will return a Dictionary of OfferRoleType/s and WorkflowRoleType/s linked to an ADUser.
        /// The Key has a descriptor prefixed at the end to identify if you working in the context of a WorkflowRoleType or OfferRoleType.
        /// The reason for this "HACK" was OfferRoleType and WorkflowRoleType don't share a common interface so we have to code against implementation.
        /// </summary>
        /// <param name="ADUSerName"></param>
        /// <returns></returns>
        Dictionary<string, string> GetRoleTypesForADUser(string ADUSerName);

        /// <summary>
        /// This will return a Dictionary of OfferRoleType/s and WorkflowRoleType/s linked to an Organisation Structure.
        /// The Key has a descriptor prefixed at the end to identify if you working in the context of a WorkflowRoleType or OfferRoleType.
        /// The reason for this "HACK" was OfferRoleType and WorkflowRoleType don't share a common interface so we have to code against implementation.
        /// </summary>
        /// <param name="OrganisationStructureKeys"></param>
        /// <returns></returns>
        Dictionary<string, string> GetRoleTypesByOrganisationStructureKey(List<int> OrganisationStructureKeys);

        /// <summary>
        /// Get ADUsers linked via the Workflow RoleType Organisation Structure Mapping
        /// </summary>
        /// <param name="workflowRoleTypeKey"></param>
        /// <param name="orgList"></param>
        /// <returns></returns>
        DataTable GetADUsersByWorkflowRoleTypeAndOrgStructList(int workflowRoleTypeKey, IList<IOrganisationStructure> orgList);

        /// <summary>
        /// This will return a Dictionary of OfferRoleType/s and WorkflowRoleType/s linked for an ADUser through their UserOrganisationStructure.
        /// The Key has a descriptor prefixed at the end to identify if you working in the context of a WorkflowRoleType or OfferRoleType.
        /// The reason for this "HACK" was OfferRoleType and WorkflowRoleType don't share a common interface so we have to code against implementation.
        /// </summary>
        /// <param name="OrganisationStructureKeys"></param>
        /// <param name="ADUserKey"></param>
        /// <returns></returns>
        Dictionary<string, string> GetRoleTypesByOrganisationStructureKeyAndADUserKey(List<int> OrganisationStructureKeys, int ADUserKey);

        IApplicationRole GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(int applicationKey, int applicationRoleTypeKey);
    }

    public enum OrganisationStructureGroup
    {
        Consultant,
        Admin,
        Manager
    };
}