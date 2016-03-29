using System;
using System.Collections.Generic;

//using SAHL.Common.Datasets;
using System.Collections.Specialized;
using System.DirectoryServices;
using System.Security.Principal;
using SAHL.Common.Security;
using SAHL.Common.Logging;

//
//Note:
//Until the RCS Domain is created, to get authenticated,
//create a local group "RCSUser" and add your SAHL\LogonName to it.
//

namespace SAHL.Common.Authentication
{
    public class Authenticator
    {
        #region Events

        public event AuthenticatorUserAuthenticatedHandler AuthenticatorUserAuthenticated;

        #endregion Events

        #region Variables

        private System.Security.Principal.WindowsPrincipal m_windowsPrincipal;
        #endregion Variables

        #region Methods

        public Authenticator()
        {
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            m_windowsPrincipal = new WindowsPrincipal(windowsIdentity);
        }

        public void AuthenticateCurrentUser()
        {
            try
            {
                if (m_windowsPrincipal.Identity.IsAuthenticated != true)
                    throw new AuthenticationException("User is not authenticated.");
                if (!m_windowsPrincipal.IsInRole("RCSUser"))
                    throw new AuthenticationException("User is not an RCSUser.");

                OnCurrentUserAuthenticated();
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, ex);
                throw;
            }
        }

        private void OnCurrentUserAuthenticated()
        {
            if (AuthenticatorUserAuthenticated != null)
            {
                AuthenticatorUserAuthenticated();
            }
        }

        public IPrincipal GetCurrentPrincipal()
        {
            return m_windowsPrincipal;
        }

        public static string GetFullWindowsUserName()
        {
            return WindowsIdentity.GetCurrent().Name;
        }

        public static string GetSimpleWindowsUserName()
        {
            string user = WindowsIdentity.GetCurrent().Name;
            int DomainIndex = user.IndexOf("\\");
            if (DomainIndex == -1)
                return user;

            return user.Substring(DomainIndex + 1);
        }

        /// <summary>
        /// Returns a subset of groups that the current principal has access to, taken from
        /// <c>GroupListIn</c>.
        /// </summary>
        /// <param name="GroupListIn"></param>
        /// <returns></returns>
        public static List<string> ValidateGroups(List<string> GroupListIn)
        {
            List<string> GroupListOut = new List<string>();
            try
            {
                SAHLPrincipal principal = SAHLPrincipal.GetCurrent();

                foreach (string s in GroupListIn)
                {
                    //Temporary fix to allow development for machines not registered on the SAHL domain
                    //Peet van der walt - Removed Temporary Fix
                    if (principal.IsInRole(s))
                        GroupListOut.Add(s);
                }
                return GroupListOut;
            }
            catch (Exception e)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, e);
                throw;
            }
        }

        public static UIUserGroup ReturnUIGroups()
        {
            // get settings from app config and add the groups to the result.
            StringCollection groups = new StringCollection();

            try
            {
                WindowsIdentity WinID = WindowsIdentity.GetCurrent();
                if (WinID == null)
                    throw new Exception("Invalid user name");
                WindowsPrincipal WinPrin = new WindowsPrincipal(WinID);

                //// try get the groups through AD first

                UIUserGroup retval = UIUserGroup.None;

                StringCollection UserRoles = (StringCollection)Properties.Settings.Default["Admin"];
                foreach (string UserRole in UserRoles)
                {
                    if (WinPrin.IsInRole(UserRole))
                    {
                        if (retval == UIUserGroup.None)
                            retval = UIUserGroup.Admin;
                        else
                            retval = (retval | UIUserGroup.Admin);
                        break;
                    }
                }

                UserRoles = (StringCollection)Properties.Settings.Default["Sales"];
                foreach (string UserRole in UserRoles)
                {
                    if (WinPrin.IsInRole(UserRole))
                    {
                        if (retval == UIUserGroup.None)
                            retval = UIUserGroup.Sales;
                        else
                            retval = (retval | UIUserGroup.Sales);
                        break;
                    }
                }

                UserRoles = (StringCollection)Properties.Settings.Default["Supervisor"];
                foreach (string UserRole in UserRoles)
                {
                    if (WinPrin.IsInRole(UserRole))
                    {
                        if (retval == UIUserGroup.None)
                            retval = UIUserGroup.Supervisor;
                        else
                            retval = (retval | UIUserGroup.Supervisor);
                        break;
                    }
                }

                UserRoles = (StringCollection)Properties.Settings.Default["Manager"];
                foreach (string UserRole in UserRoles)
                {
                    if (WinPrin.IsInRole(UserRole))
                    {
                        if (retval == UIUserGroup.None)
                            retval = UIUserGroup.Supervisor;
                        else
                            retval = (UIUserGroup.Manager | retval);
                        break;
                    }
                }

                return retval;

                #region LDAP implementation

                //string UserName = WinID.Name;
                //int DomainIndex = UserName.IndexOf("\\");
                //if (DomainIndex != -1)
                //    UserName = UserName.Substring(DomainIndex + 1);

                //// UserName = "RCSAdmin";

                //string ADUserName = ((string)Properties.Settings.Default["ADUser"]).Trim();
                //string ADPassWord = ((string)Properties.Settings.Default["ADPassWord"]).Trim();
                //string LDAPConnection = ((string)Properties.Settings.Default["LDAPConnection"]).Trim();

                ////DirectoryEntry obEntry = new DirectoryEntry("LDAP://DC=RCSHL,DC=COM", "RCSHL\\AdriaanAdmin", "Natal1");
                //DirectoryEntry obEntry = null;
                //if (ADPassWord.Length > 0 && ADUserName.Length > 0)
                //    obEntry = new DirectoryEntry(LDAPConnection, ADUserName, ADPassWord);
                //else
                //    obEntry = new DirectoryEntry(LDAPConnection);

                //DirectorySearcher srch = new DirectorySearcher(obEntry); //, "(sAMAccountName=" + strUser + ")");

                //srch.Filter = String.Format("(&(objectCategory=user)(samaccountname={0}))", UserName);

                //SearchResult res = srch.FindOne();

                //if (res != null )
                //{
                //    DirectoryEntry obUser = new DirectoryEntry(res.Path);
                //    // Invoke Groups method.
                //    DirectorySearcher DS = new DirectorySearcher(obUser);
                //    SearchResultCollection SRCUser = DS.FindAll();

                //    object obGroups = obUser.Invoke("Groups");
                //    foreach (object ob in (System.Collections.IEnumerable)obGroups)
                //    {
                //        // Create object for each group.
                //        DirectoryEntry obGpEntry = new DirectoryEntry(ob);
                //        groups.Add(obGpEntry.Name.Substring(3));
                //        obGpEntry.Dispose();
                //    }

                //    obUser.Dispose();
                //    DS.Dispose();
                //}

                //srch.Dispose();
                //obEntry.Dispose();

                //DirectorySearcher srch1 = new DirectorySearcher(obEntry); //, "(&objectClass=user)"); //(sn=" + strUser + ")");
                //srch1.Filter = "(objectCategory=user)"; // (samaccountname=AdriaanAdmin))";
                //SearchResultCollection Rc = srch1.FindAll();

                //for (int i = 0; i < Rc.Count; i++)
                //{
                //    SearchResult SR = Rc[i];
                //    ResultPropertyValueCollection SRC = SR.Properties["name"];
                //    for (int j = 0; j < SRC.Count; j++)
                //    {
                //        object gha = SRC[j];
                //    }

                //}

                //ResultPropertyCollection PropColl = R.Properties;
                // StringCollection PropNames = PropColl.PropertyNames;

                //UIUserGroup retval = UIUserGroup.None;

                //StringCollection UserRoles = (StringCollection)Properties.Settings.Default["Admin"];
                //foreach (string UserRole in UserRoles)
                //{
                //    if (groups.Contains(UserRole))
                //    {
                //        if (retval == UIUserGroup.None)
                //            retval = UIUserGroup.Admin;
                //        else
                //            retval = (retval | UIUserGroup.Admin);
                //        break;
                //    }
                //}

                //UserRoles = (StringCollection)Properties.Settings.Default["Sales"];
                //foreach (string UserRole in UserRoles)
                //{
                //    if (groups.Contains(UserRole))
                //    {
                //        if (retval == UIUserGroup.None)
                //            retval = UIUserGroup.Sales;
                //        else
                //            retval = (retval | UIUserGroup.Sales);
                //        break;
                //    }
                //}

                //UserRoles = (StringCollection)Properties.Settings.Default["Supervisor"];
                //foreach (string UserRole in UserRoles)
                //{
                //    if (groups.Contains(UserRole))
                //    {
                //        if (retval == UIUserGroup.None)
                //            retval = UIUserGroup.Supervisor;
                //        else
                //        retval = (retval | UIUserGroup.Supervisor);
                //        break;
                //    }
                //}

                //UserRoles = (StringCollection)Properties.Settings.Default["Manager"];
                //foreach (string UserRole in UserRoles)
                //{
                //    if (groups.Contains(UserRole))
                //    {
                //        if (retval == UIUserGroup.None)
                //            retval = UIUserGroup.Manager;
                //        else
                //            retval = (UIUserGroup.Manager | retval);
                //        break;
                //    }
                //}

                //return retval;

                #endregion LDAP implementation
            }
            catch (Exception e)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, e);
                return UIUserGroup.None;
            }
            finally
            {
            }
        }

        public static StringCollection ReturnUserGroups(string p_UserName, string p_Password, string p_UserToQuery)
        {
            // get settings from app config and add the groups to the result.
            StringCollection groups = new StringCollection();

            try
            {
                WindowsIdentity WinID = WindowsIdentity.GetCurrent();
                if (WinID == null)
                    throw new Exception("Invalid user name");
                WindowsPrincipal WinPrin = new WindowsPrincipal(WinID);

                //// try get the groups through AD first

                //return retval;

                string UserName = WinID.Name;
                groups.Add("WinUserName : " + UserName);
                int DomainIndex = p_UserToQuery.IndexOf("\\");
                if (DomainIndex != -1)
                    p_UserToQuery = p_UserToQuery.Substring(DomainIndex + 1);

                // UserName = "RCSAdmin";

                string ADUserName = p_UserName;//(string)Properties.Settings.Default["ADUser"];
                string ADPassWord = p_Password;//(string)Properties.Settings.Default["ADPassWord"];
                string LDAPConnection = (string)Properties.Settings.Default["LDAPConnection"];

                //DirectoryEntry obEntry = new DirectoryEntry("LDAP://DC=RCSHL,DC=COM", "RCSHL\\AdriaanAdmin", "Natal1");
                DirectoryEntry obEntry = null;
                if (ADPassWord.Length > 0 && ADUserName.Length > 0)
                    obEntry = new DirectoryEntry(LDAPConnection, ADUserName, ADPassWord);
                else
                    obEntry = new DirectoryEntry(LDAPConnection);

                DirectorySearcher srch = new DirectorySearcher(obEntry); //, "(sAMAccountName=" + strUser + ")");

                srch.Filter = String.Format("(&(objectCategory=user)(samaccountname={0}))", p_UserToQuery);

                SearchResult res = srch.FindOne();
                if (null != res)
                {
                    DirectoryEntry obUser = new DirectoryEntry(res.Path);
                    // Invoke Groups method.
                    DirectorySearcher DS = new DirectorySearcher(obUser);
                    SearchResultCollection SRCUser = DS.FindAll();

                    object obGroups = obUser.Invoke("Groups");
                    foreach (object ob in (System.Collections.IEnumerable)obGroups)
                    {
                        // Create object for each group.
                        DirectoryEntry obGpEntry = new DirectoryEntry(ob);
                        groups.Add(obGpEntry.Name.Substring(3));
                        obGpEntry.Dispose();
                    }

                    obUser.Dispose();
                    DS.Dispose();
                }

                srch.Dispose();
                obEntry.Dispose();
                return groups;
            }
            catch (Exception e)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, e);
                return null;
            }
            finally
            {
            }
        }

        public static StringCollection ReturnGroupSettings()
        {
            StringCollection UserRoles = (StringCollection)Properties.Settings.Default["Admin"];

            StringCollection retval = new StringCollection();
            retval.Add("ADMIN:");
            foreach (string group in UserRoles)
            {
                retval.Add(group);
            }
            UserRoles = (StringCollection)Properties.Settings.Default["Sales"];
            retval.Add("SALES:");
            foreach (string group in UserRoles)
            {
                retval.Add(group);
            }
            UserRoles = (StringCollection)Properties.Settings.Default["Supervisor"];
            retval.Add("SUPERVISOR:");
            foreach (string group in UserRoles)
            {
                retval.Add(group);
            }
            UserRoles = (StringCollection)Properties.Settings.Default["Manager"];
            retval.Add("MANAGER:");
            foreach (string group in UserRoles)
            {
                retval.Add(group);
            }

            return retval;
        }

        #region unused functions for trying to get cross domain users.

        //public static void play()
        //{
        //    StringCollection users = GetGroupMembers("RCSHL", "RCSAdmin");
        //  //  DirectoryEntry obEntry = new DirectoryEntry("LDAP://DC=RCSHL,DC=COM", "RCSHL\\AdriaanAdmin", "Natal1");

        //  //  DirectorySearcher srch = new DirectorySearcher(obEntry); //, "(sAMAccountName=" + strUser + ")");

        //  //srch.Filter = String.Format("(objectCategory=group)");

        //  //  SearchResultCollection res = srch.FindAll();

        //  //  StringCollection Users = new StringCollection();
        //  //  for (int i = 0; i < res.Count; i++)
        //  //  {
        //  //      DirectoryEntry obGroup = new DirectoryEntry(res[i].Path);

        //  //      object obUsers = obGroup.Invoke("Users");
        //  //      foreach (object ob in (System.Collections.IEnumerable)obUsers)
        //  //      {
        //  //          DirectoryEntry obUsEntry = new DirectoryEntry(ob);
        //  //          Users.Add(obUsEntry.Name);
        //  //      }
        //  //      Users.Clear();
        //  //  }
        //}

        /// <summary>
        ///
        /// </summary>
        /// <param name="strDomain"></param>
        /// <param name="strGroup"></param>
        /// <returns></returns>
        public static StringCollection GetGroupMembers(string strDomain, string strGroup)
        {
            StringCollection groupMemebers = new StringCollection();
            try
            {
                //DirectoryEntry ent = new DirectoryEntry("LDAP://DC=" + strDomain + ",DC=com");
                DirectoryEntry ent = new DirectoryEntry("LDAP://DC=RCSHL,DC=COM", "RCSHL\\AdriaanAdmin", "Natal1");
                DirectorySearcher srch = new DirectorySearcher(ent, "(CN=" + strGroup + ")");

                SearchResultCollection coll = srch.FindAll();
                foreach (SearchResult rs in coll)
                {
                    // DirectoryEntry grp = new DirectoryEntry(rs.Path);
                    // DirectorySearcher grpsrch = new DirectorySearcher(grp); //, "(objectcategory = user)");
                    //// grpsrch.SearchScope = SearchScope.Base;

                    // SearchResultCollection userSRC = grpsrch.FindAll();
                    // foreach (SearchResult usr in userSRC)
                    // {
                    //     DirectoryEntry user = new DirectoryEntry(usr.Path);
                    //     groupMemebers.Add(user.Name);
                    // }

                    ResultPropertyCollection resultPropColl = rs.Properties;

                    foreach (Object memberColl in resultPropColl["member"])
                    {
                        DirectoryEntry gpMemberEntry = new DirectoryEntry("LDAP://" + memberColl);
                        System.DirectoryServices.PropertyCollection userProps = gpMemberEntry.Properties;
                        object obVal = userProps["sAMAccountName"].Value;
                        object type = userProps["saAMAccountType"].Value;
                        object path = userProps["path"].Value;
                        if (null != obVal)
                        {
                            groupMemebers.Add(obVal.ToString());
                        }
                    }
                }
            }
            catch
            {
                //Trace.Write(ex.Message);
            }
            return groupMemebers;
        }

        #endregion unused functions for trying to get cross domain users.

        #endregion Methods
    }

    #region Delegates

    public delegate void AuthenticatorUserAuthenticatedHandler();

    #endregion Delegates
}