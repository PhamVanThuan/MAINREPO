using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Common;
using SAHL.X2.Framework.Common;
using System.Security.Principal;
using SAHL.X2.Framework.Interfaces;

namespace BaseTest
{
    public class EngineConnector
    {
        X2EngineProvider engine;
        string WorkflowName;
        string ProcessName;
        public EngineConnector(string WorkflowName, string ProcessName, string EngineURL)
        {
            engine = new X2EngineProvider(EngineURL);
            this.ProcessName = ProcessName;
            this.WorkflowName = WorkflowName;
            WindowsIdentity p = System.Security.Principal.WindowsIdentity.GetCurrent();

            System.Threading.Thread.CurrentPrincipal = new WindowsPrincipal(p);
        }

        public void SetADUser(string ADUSer)
        {
            GenericIdentity gi = new GenericIdentity(ADUSer);
            GenericPrincipal gp = new GenericPrincipal(gi, null);
            System.Threading.Thread.CurrentPrincipal = gp;
        }

        public X2ErrorResponse RecalcSecurity(Int64 InstanceID, string ActivityName)
        {
            List<ListRequestItem> items = new List<ListRequestItem>();
            ListRequestItem li = new ListRequestItem(InstanceID, ActivityName);
            items.Add(li);
            X2RebuildWorklistRequest r = new X2RebuildWorklistRequest(items);
            X2ResponseBase resp = engine.ProcessListActivity(items);
            if (resp.IsErrorResponse)
            {
                return (X2ErrorResponse)resp;
            }
            string s = resp.XMLResponse.ToString();
            return null;
        }

        public string Login(ref string ErrorString)
        {
            X2ResponseBase resp = engine.LogIn();
            if (!resp.IsErrorResponse)
            {
                X2LoginResponse lrs = resp as X2LoginResponse;
                return (lrs.SessionId);
            }
            else
            {
                ErrorString = ((X2ErrorResponse)resp).Exception.ToString();
                return "";
            }
        }

        public Int64 CreateCase(string SessionID, string WorkflowName, int GenericKey, string KeyName, ref string ErrorString, string Create)
        {
            Int64 InstanceID = -1;
            //string Create = "Create Application";
            X2ResponseBase resp = engine.CreateWorkFlowInstance(SessionID, ProcessName, (-1).ToString(), WorkflowName, Create, null, false);
            if (!resp.IsErrorResponse)
            {
                InstanceID = ((X2ActivityStartResponse)resp).InstanceId;
                Dictionary<string, string> args = new Dictionary<string, string>();
                args.Add(KeyName, GenericKey.ToString());
                resp = engine.CompleteActivity(SessionID, InstanceID, Create, args, false);
                if (!resp.IsErrorResponse)
                {
                    ErrorString = string.Empty;
                }
                else
                {
                    ErrorString = ((X2ErrorResponse)resp).Exception.Value;
                }
            }
            else
            {
                ErrorString = ((X2ErrorResponse)resp).Exception.Value;
            }
            return InstanceID;
        }

        public bool PerformAction(string SessionID, Int64 InstanceID, string Activity, ref string ErrorMsg, Dictionary<string, string> Inputs)
        {
            X2ResponseBase resp = engine.StartActivity(SessionID, InstanceID, Activity, false);
            if (!resp.IsErrorResponse)
            {
                X2ActivityStartResponse r = resp as X2ActivityStartResponse;
                resp = engine.CompleteActivity(SessionID, r.InstanceId, Activity, Inputs, false);
                if (!resp.IsErrorResponse)
                {
                    ErrorMsg = string.Empty;
                    return true;
                }
                else
                {
                    int DMCCount = resp.Messages.Count;
                    ErrorMsg = string.Format("DMC:{0} - {1}", DMCCount, ((X2ErrorResponse)resp).Exception.Value.ToString());
                    return false;
                }
            }
            else
            {
                ErrorMsg = ((X2ErrorResponse)resp).Exception.Value.ToString();
                return false;
            }
        }
    }
}
