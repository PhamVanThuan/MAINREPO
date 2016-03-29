using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Xml;
using SAHL.X2.Common;
using SAHL.X2.Framework.Common;
using SAHL.X2.Framework.Interfaces;

namespace Automation.Framework
{
    public class EngineConnector
    {
        public static string _engineURL { get; set; }

        public static string EngineURL
        {
            get
            {
                if (string.IsNullOrEmpty(_engineURL))
                {
                    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    var group = config.GetSectionGroup("applicationSettings");
                    var section = group.Sections["SAHL.X2.Framework.Properties.Settings"];
                    var propCol = section.ElementInformation.Properties;
                    var propertiesInfo = new PropertyInformation[propCol.Count];
                    propCol.CopyTo(propertiesInfo, 0);
                    var elemCol = propertiesInfo[0].Value as ConfigurationElementCollection;
                    var _settings = elemCol as SettingElementCollection;
                    var element = _settings.Get("X2WebHost_Url");
                    if (element == null)
                        throw new ConfigurationErrorsException("Could not locate the SettingElement:  X2URL in the configuration file");
                    SettingValueElement settingVal = element.Value;
                    XmlNode valueNode = settingVal.ValueXml;
                    return valueNode.InnerText;
                }
                return _engineURL;
            }
        }

        private X2MessageCollection Messages = new X2MessageCollection();
        private X2EngineProviderFactory engineFactory;
        private IX2Provider engine;
        private string WorkflowName;
        private string ProcessName;

        public EngineConnector(string WorkflowName, string ProcessName)
        {
            engineFactory = new X2EngineProviderFactory();
            engine = engineFactory.GetX2EngineProvider();
            this.ProcessName = ProcessName;
            this.WorkflowName = WorkflowName;
            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            Thread.CurrentPrincipal = new WindowsPrincipal(windowsIdentity);
        }

        public void SetADUser(string ADUSer)
        {
            var genericIdentity = new GenericIdentity(ADUSer);
            var genericPrincipal = new GenericPrincipal(genericIdentity, null);
            Thread.CurrentPrincipal = genericPrincipal;
        }

        public string Login(ref string ErrorString)
        {
            var resp = engine.LogIn();
            if (!resp.IsErrorResponse)
            {
                var x2LoginResponse = resp as X2LoginResponse;
                return (x2LoginResponse.SessionId);
            }
            else
            {
                ErrorString = ((X2ErrorResponse)resp).Exception.ToString();
                return "";
            }
        }

        public Int64 CreateCase(string SessionID, string WorkflowName, Dictionary<string, string> inputFields, ref string ErrorString, string Create, object data = null)
        {
            Int64 InstanceID = -1;
            X2ResponseBase resp = engine.CreateWorkFlowInstance(SessionID, ProcessName, (-1).ToString(), WorkflowName, Create, inputFields, false, data);
            if (!resp.IsErrorResponse)
            {
                InstanceID = ((X2ActivityStartResponse)resp).InstanceId;
                resp = engine.CreateCompleteActivity(SessionID, InstanceID, Create, inputFields, false, data);
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

        public bool PerformAction(string SessionID, Int64 InstanceID, string Activity, ref string ErrorMsg, Dictionary<string, string> Inputs, bool ignoreWarnings, object data)
        {
            var response = engine.StartActivity(SessionID, InstanceID, Activity, ignoreWarnings, data);
            if (!response.IsErrorResponse)
            {
                X2ActivityStartResponse r = response as X2ActivityStartResponse;
                if (data != null)
                    response = engine.CompleteActivity(SessionID, r.InstanceId, Activity, Inputs, ignoreWarnings, data);
                else
                    response = engine.CompleteActivity(SessionID, r.InstanceId, Activity, Inputs, ignoreWarnings, null);
                if (!response.IsErrorResponse)
                {
                    ErrorMsg = string.Empty;
                    return true;
                }
                else
                {
                    var x2message = ((X2ErrorResponse)response).Messages.HasErrorMessages ? string.Join(",", response.Messages.ErrorMessages.Select(x => x.Message)) : string.Empty;
                    ErrorMsg = string.Format("{0}- X2 Message: {1}", ((X2ErrorResponse)response).Exception.Value.ToString(), x2message);
                    return false;
                }
            }
            else
            {
                var x2message = ((X2ErrorResponse)response).Messages.HasErrorMessages ? string.Join(",", response.Messages.ErrorMessages.Select(x => x.Message)) : string.Empty;
                ErrorMsg = string.Format("{0}- X2 Message: {1}", ((X2ErrorResponse)response).Exception.Value.ToString(), x2message);
                return false;
            }
        }
    }
}