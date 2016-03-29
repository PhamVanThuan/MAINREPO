using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Security;
using System.Data;
using SAHL.Common.DataAccess;
using System.Security.Principal;
using SAHL.Common.CacheData;
using SAHL.Common;
using SAHL.Common.UI;

namespace WorkflowTestFramework
{
    public class UserPool
    {
        // Constructor
        public UserPool()
        {
            //Get all ADUsers into the list
            PopulateADUsers();
        }

        private void PopulateADUsers()
        {
            IDbConnection con = Helper.GetSQLDBConnection("X2");
            IDataReader dr = null;

            if (userList == null)
                userList = new List<string>();
            else
                userList.Clear();

            try
            {
                //Get a list of ADUsers
                string strSQL = "select ADUserName from TestDB_WTF..ADUser";
                con.Open();
                dr = Helper.ExecuteReader(con, strSQL);

                //loop and add
                while (dr.Read())
                {
                    string username = dr.GetString(0);

                    if (!userList.Contains(username))
                        userList.Add(username);
                }
            }
            finally
            {
                if (dr != null)
                    dr.Dispose();
                con.Dispose();
            }
        }

        #region UserLists

        private List<string> activeUserList = new List<string>();

        private List<string> userList = new List<string>();

        #endregion

        public SAHLPrincipal GetSAHLPrincipalForProcessing()
        {
            return GetSAHLPrincipal(activeUserList, userList);
        }

        public void ReleaseSAHLPrincipal(SAHLPrincipal p)
        {
            ReleaseSAHLPrincipal(activeUserList, p);
        }

        static SAHLPrincipal GetSAHLPrincipal(List<string> ActiveUserList, List<string> UserList)
        {
            //make it ThreadSafe
            lock (ActiveUserList)
            {
                //get an unused user
                foreach (string adu in UserList)
                {
                    if (!ActiveUserList.Contains(adu))
                    {
                        //mark the user as being locked
                        ActiveUserList.Add(adu);

                        //Get the SAHLPricipal 
                        SAHLPrincipal p = new SAHLPrincipal(new GenericIdentity(adu));

                        // Set the X2 nodeset, X2 sets this to null, 
                        // and will complain bitterly if there is no reference to the nodeset
                        lock (p)
                        {
                            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(p);
                            if (!spc.NodeSets.ContainsKey(CBONodeSetType.X2))
                                spc.NodeSets.Add(CBONodeSetType.X2, new CBONodeSet(CBONodeSetType.X2)); 
                        }

                        //return the user
                        return p;
                    }
                }

                return null;
            }
        }

        static void ReleaseSAHLPrincipal(List<string> ActiveUserList, SAHLPrincipal p)
        {
            //dont need to be ThreadSafe when releasing
            string user = p.Identity.Name;
            if (ActiveUserList.Contains(user))
            {
                //mark the user as not being locked
                ActiveUserList.Remove(user);
            }
        }
    }
}
