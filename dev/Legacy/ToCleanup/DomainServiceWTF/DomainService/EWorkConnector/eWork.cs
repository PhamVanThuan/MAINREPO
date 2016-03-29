using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using SAHL.Common.DataAccess;
using System.Security.Principal;

namespace EWorkConnector
{
    public enum eWorkAlertType { ToDo, Watch, System };

    public enum eWorkFormType { Standard, Confirm, NonConfirm };

    /// <summary>
    /// Summary description for eWork
    /// </summary>
    public class eWork 
    {
        public static string GetSimpleWindowsUserName()
        {
            return "LighthouseUser";
            string user = WindowsIdentity.GetCurrent().Name;
            int DomainIndex = user.IndexOf("\\");
            if (DomainIndex == -1)
                return user;

            return user.Substring(DomainIndex + 1);
        }

        private eWorkEngine m_engine;

        public eWork()
        {
            m_engine = new eWorkEngine();
        }

        public eWork(string ServerName)
        {
            m_engine = new eWorkEngine(ServerName);
        }

        /// <summary>
        /// This does the main work in building and sending the request to the eWorks server
        /// </summary>
        /// <returns>eWorks server XML response</returns>
        private void SendRequest(eRequest Request)
        {
            Request.Response = new eResponse(m_engine.SendRequest(Request.RequestXml));
        }

        private string GetLoginInfo(string UserName)
        {
            TransactionContext context = null;
            try
            {
                context = TransactionController.GetContext("EWorkConnectionString");

                if (context.DataConnection.State != ConnectionState.Open)
                    context.DataConnection.Open();

                IDbCommand cmd = context.DataConnection.CreateCommand();
                cmd.CommandText = "select    ePassword   from    eUser (nolock)  where    eUserName = @UserName  ";
                SqlParameter param = new SqlParameter("@UserName", SqlDbType.VarChar);
                param.Direction = ParameterDirection.Input;
                param.Value = UserName;
                cmd.Parameters.Add(param);
                return Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (context != null)
                    context.DisposeContext();
            }
        }

        private string AlertTypeToString(eWorkAlertType AlertType)
        {
            string alertTypeStr = " ";
            if (AlertType == eWorkAlertType.Watch)
                alertTypeStr = "!";
            else if (AlertType == eWorkAlertType.System)
                alertTypeStr = "~";

            return alertTypeStr;
        }

        public string LogIn(string Username)
        {
            eRequest Request = new eRequest(TPRequestOp.eLoginRequest);
            SendRequest(Request);//first login request
            eResponse Response = Request.Response;

            if (Response.ResponseType == TPResponseOp.eLoginFormResponse)
            {
                Request.SetFieldInputListItem("username", Username);
                string loginInfo = GetLoginInfo(Username);

                if (loginInfo == null)
                    loginInfo = "";

                Request.SetFieldInputListItem("password", loginInfo);
                Request.ServerData = Response.ServerData;

                SendRequest(Request);//second login request
                Response = Request.Response;
            }

            if (Response.IsErrorResponse)
            {
                Exception ex = new eWorkException(Response.ResponseXml.DocumentElement.InnerXml);
                throw ex;
            }

            return Response.SessionID;
        }

        public string LogOut(string SessionID)
        {
            eRequest Request = new eRequest(TPRequestOp.eLogoutRequest);
            Request.SessionID = SessionID;
            SendRequest(Request);
            //if (Response.IsErrorResponse)
            //    return Response.ErrorString;
            //return "Session " + SessionID + " logged out successfully.";
            return Request.Response.ResponseXmlString;
        }

        public string CreateFolder(string SessionID, string Map, Dictionary<string, string> Vars, string ChangedField)
        {
            eRequest Request = new eRequest(TPRequestOp.eActionRequest);
            Request.SessionID = SessionID;
            Request.SetAttribute("Map", Map);
            //if (null != Vars)
            //{
            //    Request = HandleVars(Vars, Request);
            //}

            //send the request
            SendRequest(Request);
            eResponse Response = Request.Response;

            if (Response.ResponseType == TPResponseOp.eActionResponse)
            {
                //ok, we should get an eActionResponse with a single field in the 
                //fieldoutputlist. We need to insert our account key in it and send
                //back a fieldinputlist in a submit request.
                Request = new eRequest(TPRequestOp.eSubmitRequest);
                Request.SessionID = SessionID;
                Request.FolderID = Response.FolderID;
                if (null != Vars)
                {
                    Request = HandleVars(Vars, Request, ChangedField);
                }
                //Request.Attributes.Add("UpdatedFieldID", "AttorneyNumber");
                //Request.SetFieldInputListItem("AttorneyNumber", "16");
                //Request.SetFieldInputListItem("ProspectNumber", "273672");
                //Request.SetFieldInputListItem("UserToDo", "841");
                Request.ServerData = Response.ServerData;
                Request.Action = Response.GetAttribute("Action");
                SendRequest(Request);
                Response = Request.Response;
            }

            //if there was an error, return the errorstring
            if (Response.IsErrorResponse)
            {
                eWorkException ex = null;
                if (Response != null)
                {
                    if (Response.ResponseXml != null && Response.ResponseXml.DocumentElement != null)
                        ex = new eWorkException(Response.ResponseXml.DocumentElement.InnerXml);
                    else if (Response.ResponseXml != null)
                        ex = new eWorkException(Response.ResponseXml.InnerXml);
                    else
                        ex = new eWorkException("Create Case Failed");
                }
                else
                    ex = new eWorkException("Create Case Failed");
                    throw ex;
            }

            return Response.FolderID;
        }

        private eRequest HandleVars(Dictionary<string, string> Vars, eRequest Request, string ChangedField)
        {
            //Request.Attributes.Add("UpdatedFieldID", "ApplicationKey");
            //Request.SetFieldInputListItem("ApplicationKey", "9998");
            //Request.SetFieldInputListItem("AssignedUser", "bob");

            //Request.Attributes.Add("UpdatedFieldID", "ProspectNumber");
            //Request.SetFieldInputListItem("ProspectNumber", "9998");
            //Request.SetFieldInputListItem("UserToDo", "bob");
            //Request.SetFieldInputListItem("AttorneyNo", "555");
            
            string[] Keys = new string[Vars.Count];
            Vars.Keys.CopyTo(Keys, 0);
            Request.Attributes.Add("UpdatedFieldID", ChangedField);
            foreach (string s in Keys)
            {
                Request.SetFieldInputListItem(s, Vars[s]);
            }
            return Request;
        }

        public string InvokeAndSubmitAction(string SessionID, string FolderID, string ActionName, Dictionary<string, string> Vars, string ChangedField)
        {
            eRequest Request = new eRequest(TPRequestOp.eActionRequest);
            Request.SessionID = SessionID;
            Request.FolderID = FolderID;
            Request.Action = ActionName;
            //Request.Attributes.Add("UpdatedFieldID", "AttorneyKey");
            //Request.SetFieldInputListItem("ApplicationKey", "9998");
            //Request.SetFieldInputListItem("AssignedUser", "bob");
            //Request.SetFieldInputListItem("AttorneyKey", "1");
            if (null != Vars)
            {
                Request = HandleVars(Vars, Request, ChangedField);
            }
            

            int EventID = -1;// eWorkDataWorker.GetLastEventID(FolderID, ActionName);
            Request.ClientData = EventID.ToString();

            SendRequest(Request);
            if (Request.Response.IsErrorResponse)
            {
                Exception ex = new eWorkException(Request.Response.ResponseXml.DocumentElement.InnerXml);
                    throw ex;
            }
            if (Request.Response.ResponseType == TPResponseOp.eActionResponse)
            {
                eResponse Response = Request.Response;
                
                Request = new eRequest(TPRequestOp.eSubmitRequest);
                Request.SessionID = SessionID;
                Request.FolderID = Response.FolderID;
                Request.Action = ActionName;
                //Request.Attributes.Add("UpdatedFieldID", "AttorneyKey");
                //Request.SetFieldInputListItem("ApplicationKey", "9998");
                //Request.SetFieldInputListItem("AssignedUser", "bob");
                //Request.SetFieldInputListItem("AttorneyKey", "1");
                if (null != Vars)
                {
                    Request = HandleVars(Vars, Request, ChangedField);
                }
                Request.ServerData = Response.ServerData;
                //Request.Action = Response.GetAttribute("Action");

                SendRequest(Request);
                Response = Request.Response;
                if (Request.Response.IsErrorResponse)
                {
                    Exception ex = new eWorkException(Request.Response.ResponseXml.DocumentElement.InnerXml);
                        throw ex;
                }
            }



            return Request.Response.ResponseXmlString;
        }

        public string CancelAction(string SessionID, string FolderID, string ActionName)
        {
            return "";
        }

    }
}
