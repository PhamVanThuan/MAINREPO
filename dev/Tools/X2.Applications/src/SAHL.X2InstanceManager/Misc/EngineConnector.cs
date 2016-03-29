using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Security;
using SAHL.X2.Common;
using SAHL.X2.Framework.Interfaces;
using SAHL.X2.Framework.Common;
using SAHL.Core.X2.Messages;
using SAHL.Config.Services.X2.Client;
using SAHL.Core.Web.Services;

namespace SAHL.X2InstanceManager.Misc
{
    public class EngineConnector
    {
        IX2MessageCollection Messages = new X2MessageCollection();
        IX2Provider engine;
        string WorkflowName;
        string ProcessName;
        string engineURL;
        public EngineConnector(string WorkflowName, string processName, string engineURL)
        {
            engine = new X2EngineProviderFactory().GetX2EngineProvider();
            this.ProcessName = processName;
            this.WorkflowName = WorkflowName;
            this.engineURL = engineURL;

            WindowsIdentity p = System.Security.Principal.WindowsIdentity.GetCurrent();

            System.Threading.Thread.CurrentPrincipal = new WindowsPrincipal(p);
        }

        public void SetADUser(string ADUSer)
        {
            GenericIdentity gi = new GenericIdentity(ADUSer);
            GenericPrincipal gp = new GenericPrincipal(gi, null);
            System.Threading.Thread.CurrentPrincipal = gp;
        }

        X2ServiceClient GetConfiguredClient()
        {
            ServiceUrlConfiguration config = new ServiceUrlConfiguration(this.engineURL); 
            X2ServiceClient client = new X2ServiceClient(config);
            return client;
        }

        X2Response SendRequest<T>(T request) where T : X2Request
        {
            X2ServiceClient client = null;
            try
            {
                client = GetConfiguredClient();
                var response = client.PerformCommand(request,null);
                return request.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public SAHL.X2.Framework.Interfaces.X2ErrorResponse RecalcSecurity(Int64 InstanceID, string ActivityName)
        {
            List<ListRequestItem> items = new List<ListRequestItem>();
            ListRequestItem li = new ListRequestItem(InstanceID, ActivityName);
            items.Add(li);
            X2RequestForSecurityRecalc x2RequestForSecurityRecalc = new X2RequestForSecurityRecalc(Guid.NewGuid(), InstanceID, System.Threading.Thread.CurrentPrincipal.Identity.Name);

            var response = SendRequest(x2RequestForSecurityRecalc);
            if (response.IsErrorResponse)
            {
                return new X2.Framework.Interfaces.X2ErrorResponse(DateTime.Now, new X2ResponseException(response.Message), 1, "", "");
            }

            return null;
        }

        public Int64 CreateCase(string SessionID, string WorkflowName, int GenericKey, string KeyName, ref string ErrorString, string Create)
        {
            Int64 InstanceID = -1;
            X2ResponseBase resp = engine.CreateWorkFlowInstance(SessionID, ProcessName, (-1).ToString(), WorkflowName, Create, null, false, null);
            if (!resp.IsErrorResponse)
            {
                InstanceID = ((X2ActivityStartResponse)resp).InstanceId;
                Dictionary<string, string> args = new Dictionary<string, string>();
                args.Add(KeyName, GenericKey.ToString());
                resp = engine.CreateCompleteActivity(SessionID, InstanceID, Create, args, false);
                if (!resp.IsErrorResponse)
                {
                    ErrorString = string.Empty;
                }
                else
                {
                    ErrorString = ((SAHL.X2.Framework.Interfaces.X2ErrorResponse)resp).Exception.Value;
                }
            }
            else
            {
                ErrorString = ((SAHL.X2.Framework.Interfaces.X2ErrorResponse)resp).Exception.Value;
            }
            return InstanceID;
        }

        public bool PerformAction(string SessionID, Int64 InstanceID, string Activity, ref string ErrorMsg, Dictionary<string, string> Inputs)
        {
            X2ResponseBase resp = engine.StartActivity(SessionID, InstanceID, Activity, false, null);
            if (!resp.IsErrorResponse)
            {
                X2ActivityStartResponse r = resp as X2ActivityStartResponse;
                resp = engine.CompleteActivity(SessionID, r.InstanceId, Activity, Inputs, false, null);
                if (!resp.IsErrorResponse)
                {
                    ErrorMsg = string.Empty;
                    return true;
                }
                else
                {
                    ErrorMsg = ((SAHL.X2.Framework.Interfaces.X2ErrorResponse)resp).Exception.Value.ToString();
                    return false;
                }
            }
            else
            {
                ErrorMsg = ((SAHL.X2.Framework.Interfaces.X2ErrorResponse)resp).Exception.Value.ToString();
                return false;
            }
            return false;
        }
    }
}
