using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections;
using Castle.ActiveRecord;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    /// Repository for all methods used in conjunction with internet specific methods
    /// </summary>
    [FactoryType(typeof(IInternetRepository))]
    public class InternetRepository : AbstractRepositoryBase, IInternetRepository
    {

        /// <summary>
        /// Gets all of the internet lead users
        /// </summary>
        /// <returns></returns>
        public IEventList<IInternetLeadUsers> GetAllInternetLeadUsers()
        {
            InternetLeadUsers_DAO[] ilUsers = ActiveRecordBase<InternetLeadUsers_DAO>.FindAll();
            List<InternetLeadUsers_DAO> users = new List<InternetLeadUsers_DAO>();
            for (int i = 0; i < ilUsers.Length; i++)
            {
                users.Add(ilUsers[i]);
            }
            return new DAOEventList<InternetLeadUsers_DAO, IInternetLeadUsers, InternetLeadUsers>(users);
        }

        /// <summary>
        /// Gets all Active internet lead users
        /// </summary>
        /// <returns></returns>
        public IEventList<IInternetLeadUsers> GetAllActiveInternetLeadUsers()
        {
            InternetLeadUsers_DAO[] ilUsers = ActiveRecordBase<InternetLeadUsers_DAO>.FindAll();
            List<InternetLeadUsers_DAO> users = new List<InternetLeadUsers_DAO>();
            for (int i = 0; i < ilUsers.Length; i++)
            {
                if (ilUsers[i].GeneralStatus.Key == (int)GeneralStatuses.Active)
                    users.Add(ilUsers[i]);
            }
            return new DAOEventList<InternetLeadUsers_DAO, IInternetLeadUsers, InternetLeadUsers>(users);
        }

        /// <summary>
        /// returns an active general status
        /// </summary>
        /// <returns></returns>
        public IGeneralStatus GetActiveGeneralStatus()
        {
            return new GeneralStatus(ActiveRecordBase<GeneralStatus_DAO>.Find((int)GeneralStatuses.Active));
        }

        /// <summary>
        /// returns an inactive general status
        /// </summary>
        /// <returns></returns>
        public IGeneralStatus GetInActiveGeneralStatus()
        {
            return new GeneralStatus(ActiveRecordBase<GeneralStatus_DAO>.Find((int)GeneralStatuses.Inactive));
        }

        /// <summary>
        /// This updates the Internet Lead User's 
        /// </summary>
        /// <param name="internetLeadUserKey"></param>
        /// <param name="generalStatus"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool UpdateInternetLeadUser(int internetLeadUserKey, IGeneralStatus generalStatus, bool flag)
        {
            IInternetLeadUsers ilu = GetByKey<IInternetLeadUsers, InternetLeadUsers_DAO>(internetLeadUserKey);
            ilu.Flag = flag;
            ilu.GeneralStatus = generalStatus;
            Save<IInternetLeadUsers, InternetLeadUsers_DAO>(ilu);

            return true;
        }

        /// <summary>
        /// Sets the Flag, and returns the next ADUSer in the queue to get an intenret lead
        /// </summary>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public IADUser AssignInternetLeadUser(int offerKey)
        {
            IEventList<IInternetLeadUsers> internetleadusers = GetAllActiveInternetLeadUsers();
            IADUser iaduser = null;
            for (int i = 0; i < internetleadusers.Count; i++)
            {
                if (internetleadusers[i].Flag)
                {
                    if (i == internetleadusers.Count -1)
                    {
                        // Last record in the Table - need to move the flag to the top
                        internetleadusers[i].Flag = false;
                        SaveInternetLeadUser(internetleadusers[i]);
                        internetleadusers[0].Flag = true;
                        internetleadusers[0].LastCaseKey = offerKey;
                        internetleadusers[0].CaseCount = internetleadusers[0].CaseCount + 1;
                        SaveInternetLeadUser(internetleadusers[0]);
                        iaduser = internetleadusers[0].ADUser;
                        break;
                    }
                    
                    internetleadusers[i].Flag = false;
                    SaveInternetLeadUser(internetleadusers[i]);
                    internetleadusers[i + 1].Flag = true;
                    internetleadusers[i + 1].LastCaseKey = offerKey;
                    internetleadusers[i + 1].CaseCount = internetleadusers[i + 1].CaseCount + 1;
                    SaveInternetLeadUser(internetleadusers[i + 1]);
                    iaduser = internetleadusers[i + 1].ADUser;
                    break;
                }
            }

            return iaduser;
        }

        /// <summary>
        /// Refreshes the internet lead users
        /// </summary>
        /// <returns></returns>
        public bool RefreshInternetLeadUsers()
        {
            // Get the active ADUsers
            IEventList<IADUser> adUsers = GetInternetLeadADUsers();
            // Get the InternetLeadUSers
            IEventList<IInternetLeadUsers> internetLeadUsers = GetAllInternetLeadUsers();

            // Loop through AdUsers and InternetLeadUsers  and do the following
            for (int i = 0; i < adUsers.Count; i++)
            {
                bool exists = false;
                for (int j = 0; j < internetLeadUsers.Count; j++)
                {
                    if (adUsers[i].Key == internetLeadUsers[j].ADUser.Key)
                    {
                        exists = true;
                        if (adUsers[i].GeneralStatusKey.Key == (int)GeneralStatuses.Inactive)
                        {
                            // the ADUser is Inactive - so InternetLeads cannot be assigned to them - delete
                            DeleteInternetLeadUser(internetLeadUsers[j]);
                        }
                        //break;
                    }
                }
                if (!exists && (adUsers[i].GeneralStatusKey.Key == (int)GeneralStatuses.Active))
                {
                    // InternetLeadUser does not exist - add it
                    IInternetLeadUsers newUser = CreateNewIInternetLeadUsers();
                    newUser.ADUser = GetByKey<IADUser, ADUser_DAO>(adUsers[i].Key);
                    newUser.CaseCount = 0;
                    newUser.Flag = false;
                    newUser.GeneralStatus = GetActiveGeneralStatus();
                    newUser.LastCaseKey = 0;
                    SaveInternetLeadUser(newUser);
                }
            }
            return true;
        }


        private static IEventList<IADUser> GetInternetLeadADUsers()
        {
            string sql = string.Format("select * from [2am]..ADUser where ADUserName like '%SAHL\\%IN' COLLATE SQL_Latin1_General_CP1_CS_AS");
            DataTable dt = new DataTable();
            using (IDbConnection con = Helper.GetSQLDBConnection())
            {
                Helper.FillFromQuery(dt, sql, con, null);
                ADUser_DAO[] adusers = new ADUser_DAO[dt.Rows.Count];

                for (int i = 0; i < adusers.Length; i++)
                {
                    adusers[i] = ActiveRecordBase<ADUser_DAO>.Find(Convert.ToInt32(dt.Rows[i][0]));
                }

                return new DAOEventList<ADUser_DAO, IADUser, ADUser>(adusers);
            }
        }



        private IInternetLeadUsers CreateNewIInternetLeadUsers()
        {
            return CreateEmpty<IInternetLeadUsers, InternetLeadUsers_DAO>();
        }

        void SaveInternetLeadUser(IInternetLeadUsers internetleaduser)
        {
            Save<IInternetLeadUsers, InternetLeadUsers_DAO>(internetleaduser);
        }


        static void DeleteInternetLeadUser(IInternetLeadUsers internetleaduser)
        {
            InternetLeadUsers_DAO dao = (InternetLeadUsers_DAO)((IDAOObject)internetleaduser).GetDAOObject();
            dao.DeleteAndFlush();

        }


    }


}
