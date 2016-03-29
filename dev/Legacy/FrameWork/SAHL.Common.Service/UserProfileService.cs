using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using SAHL.Common.UserProfiles;
using SAHL.Common.BusinessModel;
using System.Data;
using Castle.ActiveRecord;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Common.Service
{
    // **********************************************************************************
    // MattS - 12/02/2009
    // I don't think this is being used anywhere.  ADUSer was taken off the SPC which means 
    // this needs looking at as it's use will be VERY slow.  Doing a FindAll didn't reveal 
    // it being used anywhere, but leavingi t in for now.
    // **********************************************************************************

    // TODO: Review this class
    [FactoryType(typeof(IUserProfileService))] 
    public class UserProfileService : IUserProfileService
    {
        public SAHL.Common.UserProfiles.UserProfile GetUserProfile(SAHLPrincipal principal)
        {
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(principal);
            
            if (SPC.Profile.Settings.Count == 0)
            {
                ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                IADUser adUser = secRepo.GetADUserByPrincipal(principal.Identity.Name);
                IEventList<SAHL.Common.BusinessModel.Interfaces.IUserProfileSetting> settings = adUser.UserProfileSettings;

                for (int i = 0; i < settings.Count; i++)
                {
                    string SettingType = settings[i].SettingType;
                    SAHL.Common.UserProfiles.IUserProfileSetting IUPS = Activator.CreateInstance(Type.GetType(SettingType)) as SAHL.Common.UserProfiles.IUserProfileSetting;
                    IUPS.Load(settings[i].SettingValue);
                    SPC.Profile.Settings.Add(IUPS.Name, IUPS);
                }
            }

            return SPC.Profile;
        }

        public void PersistUserProfile(SAHLPrincipal principal, SAHL.Common.UserProfiles.UserProfile UserProfile)
        {
            SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(principal);
            // rewrite all the user settings regardless
            // first remove the old ones
            using (new TransactionScope())
            {
                ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                IADUser adUser = secRepo.GetADUserByPrincipal(principal.Identity.Name);
                IEventList<SAHL.Common.BusinessModel.Interfaces.IUserProfileSetting> settings = adUser.UserProfileSettings;

                while (settings.Count > 0)
                {
                    DomainMessageCollection DMC = new DomainMessageCollection();
                    settings.RemoveAt(DMC, 0);
                }

                // add all the existing ones
                foreach (KeyValuePair<string, SAHL.Common.UserProfiles.IUserProfileSetting> UPSKVP in SPC.Profile.Settings)
                {
                    string Result = UPSKVP.Value.Persist();
                    UserProfileSetting_DAO USP = new UserProfileSetting_DAO();
                    USP.SettingName = UPSKVP.Value.Name;
                    USP.SettingValue = Result;
                    USP.SettingType = UPSKVP.Value.SettingType.ToString();
                    IDAOObject ADDAOobj = adUser as IDAOObject;
                    ADUser_DAO ADUser = ADDAOobj.GetDAOObject() as ADUser_DAO;
                    USP.ADUser = ADUser;
                    USP.SaveAndFlush();
                }
            }
        }
    }
}
