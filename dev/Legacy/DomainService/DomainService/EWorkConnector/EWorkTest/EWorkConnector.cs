using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SAHL.eWorkProvider;
using SAHL.Datasets;
using SAHL.Authentication;

namespace EWorkTest
{
    public class EWorkConnector
    {
        eWork EW = new eWork();
        public string Login()
        {
            return EW.LogIn(Authenticator.GetSimpleWindowsUserName());
        }

        public string CreateCase(string p_SessionID, string p_MapName, int p_AccountKey)
        {
            return EW.CreateFolder(p_SessionID, p_MapName, p_AccountKey);
        }

        public string PerformAction(string SessionID, string ActionName, string FolderID)
        {
            return EW.InvokeAndSubmitAction(SessionID, FolderID, ActionName);
        }
    }

}
