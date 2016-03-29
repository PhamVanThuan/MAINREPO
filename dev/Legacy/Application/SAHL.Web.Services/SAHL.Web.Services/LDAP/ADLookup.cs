using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Security.Principal;
using System.ComponentModel;
using Microsoft.Win32;
using System.Text;
using System.Diagnostics;
using System.DirectoryServices;
using System.Collections;
using System.Data;

namespace SAHL.Web.Services.LDAP
{
    class ADLookup
    {
        #region > Private Data <
        /// <summary>
        /// Days of the week for hours of availability breakout.
        /// </summary>
        private string[] _DayOfWeek = { " Sun: ", " Mon: ", " Tue: ", " Wed: ", " Thu: ", " Fri: ", " Sat: " };

        /// <summary>
        /// Operational domain.
        /// </summary>
        private string _Domain = string.Empty;
        #endregion

        #region > Constructors <
        /// <summary>
        /// Instantiate object.
        /// </summary>
        public ADLookup()
        {
            // Nothing to do
        }

        /// <summary>
        /// Instantiate object, set domain context.
        /// </summary>
        public ADLookup(string DomainName)
        {
            _Domain = DomainName;
        }
        #endregion

        #region > Public Methods <
        /// <summary>
        /// Retrieve the list of domains available.
        /// </summary>
        public List<string> DomainList()
        {
            List<string> ret = new List<string>();
            DirectoryEntry dir = new DirectoryEntry("WinNT:");
            try
            {
                DirectoryEntries dirs = dir.Children;
                foreach (DirectoryEntry dom in dirs)
                    ret.Add(dom.Name);
            }
            catch (Exception ex)
            {
                // TODO: Put exception handler here.
                string a = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// Retrieve a list of users in the domain.
        /// </summary>
        public DataTable UsersInDomain()
        {
            if (_Domain.Length < 1)
                return null;
            return UsersInDomain(_Domain);
        }

        /// <summary>
        /// Retrieve a list of users in the domain.
        /// </summary>
        /// <param name="DomainName">Domain to search.</param>
        public DataTable UsersInDomain(string DomainName)
        {
            DataTable ret = new DataTable("UserData");
            ret.Columns.Add(new DataColumn("LogonId", typeof(string)));
            ret.Columns.Add(new DataColumn("FullName", typeof(string)));
            DirectoryEntry dir = new DirectoryEntry(string.Format("WinNT://{0}", DomainName));
            try
            {
                DirectoryEntries dirs = dir.Children;
                foreach (DirectoryEntry dom in dirs)
                {
                    if (dom.SchemaClassName == "User")
                    {
                        DataRow nr = ret.NewRow();
                        nr["LogonId"] = dom.Name;
                        nr["FullName"] = dom.Properties["FullName"].Value.ToString();
                        ret.Rows.Add(nr);
                    }
                }
                ret.AcceptChanges();
            }
            catch (Exception ex)
            {
                // TODO: Put exception handler here.
                string a = ex.Message;
            }

            if (ret.Rows.Count > 0)
                return ret.Copy();
            return null;
        }

        /// <summary>
        /// Retrieve a list of groups in the domain.
        /// </summary>
        public List<string> GroupsInDomain()
        {
            if (_Domain.Length < 1)
                return null;
            return GroupsInDomain(_Domain);
        }

        /// <summary>
        /// Retrieve a list of groups in the domain.
        /// </summary>
        /// <param name="DomainName">Domain to search.</param>
        public List<string> GroupsInDomain(string DomainName)
        {
            List<string> ret = new List<string>();
            DirectoryEntry dir = new DirectoryEntry(string.Format("WinNT://{0}", DomainName));
            try
            {
                DirectoryEntries dirs = dir.Children;
                foreach (DirectoryEntry dom in dirs)
                    if (dom.SchemaClassName == "Group")
                        ret.Add(dom.Name);
            }
            catch (Exception ex)
            {
                // TODO: Put exception handler here.
                string a = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// Retrieve a list of groups for a user.
        /// </summary>
        /// <param name="UserName">User name to search for.</param>
        public List<string> ListGroupsForUser(string UserName)
        {
            if (_Domain.Length < 1)
                return null;
            return ListGroupsForUser(_Domain, UserName);
        }

        /// <summary>
        /// Retrieve a list of groups for a user.
        /// </summary>
        /// <param name="DomainName">Domain name containing user.</param>
        /// <param name="UserName">User name to search.</param>
        public List<string> ListGroupsForUser(string DomainName, string UserName)
        {
            List<string> ret = new List<string>();
            DirectoryEntry dir = new DirectoryEntry(string.Format("WinNT://{0}/{1}", DomainName, UserName));
            try
            {
                object groups = dir.Invoke("groups", null);
                foreach (object group in (IEnumerable)groups)
                {
                    DirectoryEntry groupEntry = new DirectoryEntry(group);
                    ret.Add(groupEntry.Name);
                }
            }
            catch (Exception ex)
            {
                // TODO: Put exception handler here.
                string a = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// Retrieve a list of users in a group.
        /// </summary>
        /// <param name="GroupName">Group name to search.</param>
        public DataTable ListUsersInGroup(string GroupName)
        {
            if (_Domain.Length < 1)
                return null;
            return ListUsersInGroup(_Domain, GroupName);
        }

        /// <summary>
        /// Retrieve a list of users in a group.
        /// </summary>
        /// <param name="DomainName">Domain name containing group.</param>
        /// <param name="GroupName">Group name to search.</param>
        public DataTable ListUsersInGroup(string DomainName, string GroupName)
        {
            DataTable ret = new DataTable("UserData");
            ret.Columns.Add(new DataColumn("LogonId", typeof(string)));
            ret.Columns.Add(new DataColumn("FullName", typeof(string)));
            DirectoryEntry dir = new DirectoryEntry(string.Format("WinNT://{0}/{1}", DomainName, GroupName));
            try
            {
                object users = dir.Invoke("members", null);
                foreach (object user in (IEnumerable)users)
                {
                    DirectoryEntry userEntry = new DirectoryEntry(user);
                    DataRow nr = ret.NewRow();
                    nr["LogonId"] = userEntry.Name;
                    nr["FullName"] = userEntry.Properties["FullName"].Value.ToString();
                    ret.Rows.Add(nr);
                }
                ret.AcceptChanges();
            }
            catch
            {
                // Ignore it
            }

            if (ret.Rows.Count > 0)
                return ret.Copy();
            return null;
        }

        /// <summary>
        /// Retrieve a list of properties for the user in the domain.
        /// </summary>
        /// <param name="UserName">User name to search.</param>
        public List<string> ListUsersProperties(string UserName)
        {
            if (_Domain.Length < 1)
                return null;
            return ListUserProperties(_Domain, UserName);
        }

        /// <summary>
        /// Retrieve a list of properties for the user in the domain.
        /// </summary>
        /// <param name="DomainName">Domain name containing user.</param>
        /// <param name="UserName">User name to search.</param>
        public List<string> ListUserProperties(string DomainName, string UserName)
        {
            string pval = string.Empty;
            List<string> ret = new List<string>();
            DirectoryEntry dir = new DirectoryEntry(string.Format("WinNT://{0}/{1}", DomainName, UserName));
            try
            {
                foreach (string propName in dir.Properties.PropertyNames)
                {
                    switch (propName)
                    {
                        case "LoginHours":
                            if (dir.Properties[propName].Value == null)
                                pval = "All Hours";
                            else
                            {
                                string sched = DecodeLoginHours((byte[])dir.Properties[propName].Value);
                                if (sched.Length < 1)
                                    pval = "All Hours";
                                else
                                    pval = string.Format(" > Click to view < |{0}", sched);
                            }
                            break;
                        case "objectSid":
                            pval = SIDToString((byte[])dir.Properties[propName].Value);
                            break;
                        default:
                            pval = dir.Properties[propName].Value.ToString();
                            break;
                    }
                    ret.Add(string.Format("{0}: {1}", propName, pval));
                }
            }
            catch (Exception ex)
            {
                // TODO: Put exception handler here.
                string a = ex.Message;
            }
            return ret;
        }
        #endregion

        #region > Private Methods <
        /// <summary>
        /// Translate the hours into something readable.
        /// </summary>
        /// <param name="HoursValue">Hours to convert.</param>
        /// <returns>A string indicating the hours of availability.</returns>
        private string DecodeLoginHours(byte[] HoursValue)
        {
            // See if we have anything
            if (HoursValue.Length < 1)
                return string.Empty;

            // Pick up the time zone bias
            int Bias = GetActiveBias();

            // Convert the HoursValue array into a character array of 1's and 0's.
            // That's a really simple statement for a bit of a convoluted process:
            //  The HoursValue byte array consists of 21 elements (21 bytes) where
            //  each bit represents a specified login hour in Universal Time
            //  Coordinated (UTC).  These bits must be reconstructed into an array
            //  that we can display (using 1's and 0's) and associated correctly to
            //  each of the hour increments by using the machines current timezone
            //  information.

            // Load the HoursValue byte array into a BitArray
            //   This little trick also allows us to read through the array from 
            //   left to right, rather than from right to left for each of the 21
            //   elements of the Byte array.
            BitArray ba = new BitArray(HoursValue);

            // This is the adjusted bit array (accounting for the ActiveTimeBias)
            BitArray bt = new BitArray(168);

            // Actual index in target array
            int ai = 0;

            // Copy the source bit array to the target bit array with offset
            for (int i = 0; i < ba.Length; i++)
            {
                // Adjust for the ActiveTimeBias
                ai = i - Bias;
                if (ai < 0)
                    ai += 168;

                // Adjust the index for out of range offsets
                ai %= 168;

                // Place the value
                bt[ai] = ba[i];
            }

            // Time to construct the output
            int colbump = 0;
            int rowbump = 0;
            int rowcnt = 0;
            StringBuilder resb = new StringBuilder();
            resb.Append("      ------- Hour of the Day -------");
            resb.Append(Environment.NewLine);
            resb.Append("      M-3 3-6 6-9 9-N N-3 3-6 6-9 9-M");
            resb.Append(Environment.NewLine);
            resb.Append(_DayOfWeek[rowcnt]);
            for (int i = 0; i < bt.Length; i++)
            {
                // Put in a 0 or a 1
                resb.Append((bt[i]) ? "1" : "0");
                colbump++;
                rowbump++;

                // After 24 elements are written, start the next line
                if (rowbump == 24)
                {
                    // Make sure we're not on the last element
                    if (i < (bt.Length - 1))
                    {
                        rowbump = 0;
                        colbump = 0;
                        resb.Append(Environment.NewLine);
                        rowcnt++;
                        resb.Append(_DayOfWeek[rowcnt]);
                    }
                }
                else
                {
                    // Insert a space after every 3 characters unless we've gone to a new line
                    if (colbump == 3)
                    {
                        resb.Append(" ");
                        colbump = 0;
                    }
                }
            }

            // Return the result
            return resb.ToString();
        }

        /// <summary>
        /// Retrieve the current machine ActiveTimeBias.
        /// </summary>
        /// <returns>an integer representing the ActiveTimeBias in hours.</returns>
        private int GetActiveBias()
        {
            // Open the TimeZone key
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\TimeZoneInformation");
            if (key == null)
                return 0;

            // Pick up the time bias
            int Bias = (int)key.GetValue("ActiveTimeBias");

            // Close the parent key
            key.Close();

            // return the result adjusted for hours (instead of minutes)
            return (Bias / 60);
        }

        /// <summary>
        /// Convert a binary SID to a string.
        /// </summary>
        /// <param name="sidBinary">SID to convert.</param>
        /// <returns>String representation of a SID.</returns>
        private string SIDToString(byte[] sidBinary)
        {
            SecurityIdentifier sid = new SecurityIdentifier(sidBinary, 0);
            return sid.ToString();
        }
        #endregion
    }

}