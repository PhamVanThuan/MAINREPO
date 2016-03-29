using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.DirectoryServices;
using System.Text;
using System.Collections;

/// <summary>
/// Summary description for ActiveDirectoryFunctions
/// </summary>
public class ActiveDirectoryFunctions
{
	public ActiveDirectoryFunctions()
	{
	}

    /// <summary>
    /// Checks to see whether a User exists in Active Directory
    /// </summary>
    #region FUNCTION : CheckUserExistsInAD
    public static bool CheckUserExistsInAD(string loginName)
    {
        string userName = ExtractUserName(loginName);
        DirectorySearcher search = new DirectorySearcher();
        search.Filter = String.Format("(SAMAccountName={0})", userName);
        search.PropertiesToLoad.Add("cn");
        SearchResult result = search.FindOne();

        return result != null;
    } 
    #endregion

    /// <summary>
    /// Returns various properties for an Active Directory User
    /// </summary>
    #region FUNCTION : GetADUserInfo
    public static Structs.structADUser GetADUserInfo(string sLoginName)
    {
        Structs.structADUser structUser = new Structs.structADUser();

        try
        {
            string userName = ExtractUserName(sLoginName);
            DirectorySearcher search = new DirectorySearcher();

            //Set a Client TimeOut
            search.ClientTimeout = new TimeSpan(0, 1, 0); //1 minute;

            //Specifiy the scope of your search
            search.SearchScope = SearchScope.Subtree;

            //Cache Results
            search.CacheResults = true;

            search.Filter = String.Format("(SAMAccountName={0})", userName);
            search.PropertiesToLoad.Add("samaccountname");
            search.PropertiesToLoad.Add("givenname");
            search.PropertiesToLoad.Add("sn");
            search.PropertiesToLoad.Add("cn");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("memberOf");

            SearchResult result = search.FindOne();

            if (result != null)
            {
                structUser.UserID = (string)result.Properties["samaccountname"][0];
                structUser.GivenName = (string)result.Properties["givenname"][0];
                structUser.Surname = (string)result.Properties["sn"][0];
                structUser.FullName = (string)result.Properties["cn"][0];
                structUser.Email = (string)result.Properties["mail"][0];
                structUser.MemberOf = new ArrayList(result.Properties["memberOf"].Count);
                int iGroupCount = 0;
                foreach (string sGroup in result.Properties["memberOf"])
                {
                    structUser.MemberOf.Add((string)result.Properties["memberOf"][iGroupCount]);
                    iGroupCount++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

        }

        return structUser;
    } 
    #endregion

    /// <summary>
    /// Returns an ArrayList of Groups that a User belongs too
    /// </summary>
    #region FUNCTION : GetADUserGroups
    public static ArrayList GetADUserGroups(string sLoginName)
    {
        ArrayList arrGroups = new ArrayList();
        try
        {
            // Get the User Information 
            Structs.structADUser user = new Structs.structADUser();
            user = GetADUserInfo(sLoginName);

            DirectorySearcher search = new DirectorySearcher();

            //Set a Client TimeOut
            search.ClientTimeout = new TimeSpan(0, 1, 0); //1 minute;

            //Specifiy the scope of your search
            search.SearchScope = SearchScope.Subtree;

            //Cache Results
            search.CacheResults = true;

            search.Filter = String.Format("(cn={0})", user.FullName);
            search.PropertiesToLoad.Add("memberOf");

            SearchResult result = search.FindOne();
            if (result != null)
            {
                int groupCount = result.Properties["memberOf"].Count;

                for (int counter = 0; counter < groupCount; counter++)
                {
                    arrGroups.Add((string)result.Properties["memberOf"][counter]);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return arrGroups;
    } 
    #endregion

    /// <summary>
    /// Returns an Arraylist of Users belonging to a particular AD Group
    /// </summary>
    #region FUNCTION : GetADGroupUsers
    public static ArrayList GetADGroupUsers(string groupName)
    {
        SearchResult result;
        DirectorySearcher search = new DirectorySearcher();

        //Set a Client TimeOut
        search.ClientTimeout = new TimeSpan(0, 1, 0); //1 minute;

        //Specifiy the scope of your search
        search.SearchScope = SearchScope.Subtree;

        //Cache Results
        search.CacheResults = true;

        search.Filter = String.Format("(cn={0})", groupName);
        search.PropertiesToLoad.Add("member");
        //search.PropertiesToLoad.Add("samaccountname");
        result = search.FindOne();

        ArrayList userNames = new ArrayList();
        if (result != null)
        {
            for (int counter = 0; counter < result.Properties["member"].Count; counter++)
            {
                string user = (string)result.Properties["member"][counter];
                //string ac = (string)result.Properties["samaccountname"][counter];
                userNames.Add(user);
            }
        }
        return userNames;
    } 
    #endregion

    /// <summary>
    /// Returns whether a User belongs to a specific AD Group
    /// </summary>
    #region FUNCTION : CheckUserInGroup
    public static bool CheckUserInGroup(string sGroupName, string sLoginName)
    {
        bool bBelongs = false;
        try
        {
            // Get the User Information 
            Structs.structADUser user = new Structs.structADUser();
            user = GetADUserInfo(sLoginName);

            DirectorySearcher search = new DirectorySearcher();

            //Set a Client TimeOut
            search.ClientTimeout = new TimeSpan(0, 1, 0); //1 minute;

            //Specifiy the scope of your search
            search.SearchScope = SearchScope.Subtree;

            //Cache Results
            search.CacheResults = true;

            search.Filter = String.Format("(cn={0})", user.FullName);
            search.PropertiesToLoad.Add("memberOf");

            SearchResult result = search.FindOne();
            if (result != null)
            {
                int groupCount = result.Properties["memberOf"].Count;

                for (int counter = 0; counter < groupCount; counter++)
                {
                    string sGrp = result.Properties["memberOf"][counter].ToString();
                    sGrp = sGrp.Substring(0,sGrp.IndexOf(","));
                    sGrp = sGrp.Replace("CN=","");
                    if (sGrp == sGroupName)
                    {
                        bBelongs = true;
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return bBelongs;
    }
    #endregion

    public static bool InGroup(string GroupName)
    {

        foreach (System.Security.Principal.IdentityReference ident in System.Security.Principal.WindowsIdentity.GetCurrent().Groups)
        {

            if (ident.Value.ToLower() == GroupName.ToLower())

                return true;

        }

        return false;

    }

    /// <summary>
    /// Extracts the UserName from the Login Name
    /// </summary>
    #region FUNCTION : ExtractUserName
    public static string ExtractUserName(string sPath)
    {
        string[] userPath = sPath.Split(new char[] { '\\' });

        return userPath[userPath.Length - 1];
    }
    #endregion


   

}
