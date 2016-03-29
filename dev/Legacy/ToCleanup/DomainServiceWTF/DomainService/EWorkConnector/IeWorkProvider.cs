using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace EWorkConnector
{
    interface IeWorkProvider
    {
        string LogIn(string Username);

        string LogOut(string SessionID);

        string CreateFolder(string SessionID, string Map, int AccountKey, string UserName);

        string InvokeAction(string SessionID, string FolderID, string ActionName);

        string SubmitAction(string SessionID, string FolderID, string ActionName, string ServerData);

        string CancelAction(string SessionID, string FolderID, string ActionName);
    }

    [Serializable]
    public class eWorkException : Exception
    {
        string m_ResponseError = "";
        public eWorkException(string p_ResponseError)
        {
            m_ResponseError = HttpUtility.HtmlEncode(p_ResponseError);
        }

        public override string Message
        {
            get
            {
                return m_ResponseError;
            }
        }
    }
}
