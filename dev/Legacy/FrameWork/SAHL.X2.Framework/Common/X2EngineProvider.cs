using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.X2.Framework.Common
{
    public class X2WebEngineProvider : IX2Provider
    {
        static object syncObj = new object();
        IX2Engine m_Engine;
        string m_ConstructorURL = null;
        bool publishOnlyMode = false;

        public X2WebEngineProvider()
        {
            m_Engine = new X2EngineWebHostProvider();
        }

        public X2WebEngineProvider(string X2URL) :base()
        {
            m_ConstructorURL = X2URL;
        }

        #region IX2Provider Members

        public X2ResponseBase LogIn()
        {
            try
            {
                // get an X2 instance
                

                X2LoginRequest X2LRq = new X2LoginRequest(null);

                if (m_Engine != null)
                {
                    try
                    {
                        return m_Engine.SendLoginRequest(X2LRq, Thread.CurrentPrincipal.Identity.Name);
                    }
                    catch (Exception Ex)
                    {
                        X2ErrorResponse err = new X2ErrorResponse(DateTime.Now, new X2ResponseException(Ex.Message), ErrorCodes.UnExpectedEngineError, "X2EngineProvider.Login", string.Empty);
                        return err;
                    }
                }
                else
                {
                    X2ErrorResponse err = new X2ErrorResponse(DateTime.Now, new X2ResponseException("Engine could not be retrieved by provider."), ErrorCodes.UnExpectedEngineError, "X2EngineProvider.Login", string.Empty);
                    return err;
                }
            }
            catch (Exception e)
            {
                X2ErrorResponse err = new X2ErrorResponse(DateTime.Now, new X2ResponseException(e.Message), ErrorCodes.UnExpectedEngineError, "X2EngineProvider.Login", string.Empty);
                return err;
            }
        }

        public X2ResponseBase LogOut(string SessionID)
        {
            // get an X2 instance
            

            X2LogoutRequest eLOq = new X2LogoutRequest(SessionID);
            return m_Engine.SendLogoutRequest(eLOq, Thread.CurrentPrincipal.Identity.Name);
        }

        public X2ResponseBase CreateWorkFlowInstance(string SessionID, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> FieldInputs, bool IgnoreWarnings)
        {
            return CreateWorkFlowInstance(SessionID, ProcessName, ProcessVersion, WorkFlowName, ActivityName, FieldInputs, IgnoreWarnings, null);
        }

        public X2ResponseBase CreateWorkFlowInstance(string SessionID, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> FieldInputs, bool IgnoreWarnings, object Data)
        {
            // get an X2 instance
            X2CreateWorkFlowInstanceRequest CWRq = new X2CreateWorkFlowInstanceRequest(SessionID, ProcessName, ProcessVersion, WorkFlowName, ActivityName, FieldInputs, IgnoreWarnings, Data);
            return m_Engine.SendCreateWorkFlowInstanceRequest(CWRq, Thread.CurrentPrincipal.Identity.Name);
        }

        public X2ResponseBase StartActivity(string SessionID, Int64 InstanceID, string ActivityName, bool IgnoreWarnings)
        {
            return StartActivity(SessionID, InstanceID, ActivityName, IgnoreWarnings, null);
        }

        public X2ResponseBase StartActivity(string SessionID, Int64 InstanceID, string ActivityName, bool IgnoreWarnings, object Data)
        {
            // get an X2 instance
            

            X2ActivityStartRequest X2ASRq = new X2ActivityStartRequest(SessionID, InstanceID, ActivityName, IgnoreWarnings, Data);
            return m_Engine.SendActivityStartRequest(X2ASRq, Thread.CurrentPrincipal.Identity.Name);
        }

        public X2ResponseBase CompleteActivity(string SessionID, Int64 InstanceID, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings)
        {
            return CompleteActivity(SessionID, InstanceID, ActivityName, InputFields, IgnoreWarnings, null);
        }

        public X2ResponseBase CompleteActivity(string SessionID, Int64 InstanceID, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data)
        {
            // get an X2 instance
            

            X2ActivityCompleteRequest X2ACRq = new X2ActivityCompleteRequest(SessionID, InstanceID, ActivityName, IgnoreWarnings, Data);
            CopyInputFieldList(InputFields, X2ACRq.FieldInputList);
            return m_Engine.SendActivityCompleteRequest(X2ACRq, Thread.CurrentPrincipal.Identity.Name);
        }

        public X2ResponseBase CreateCompleteActivity(string SessionID, Int64 InstanceID, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data)
        {
            // get an X2 instance
            

            X2ActivityCompleteRequest X2ACRq = new X2ActivityCompleteRequest(SessionID, InstanceID, ActivityName, IgnoreWarnings, Data);
            CopyInputFieldList(InputFields, X2ACRq.FieldInputList);
            return m_Engine.SendActivityCreateCompleteRequest(X2ACRq, Thread.CurrentPrincipal.Identity.Name);
        }


        public X2ResponseBase CancelActivity(string SessionID, Int64 InstanceID, string ActivityName)
        {
            // get an X2 instance
            

            X2ActivityCancelRequest X2ACnRq = new X2ActivityCancelRequest(SessionID, InstanceID, ActivityName);
            return m_Engine.SendActivityCancelRequest(X2ACnRq, Thread.CurrentPrincipal.Identity.Name);
        }

        public X2ResponseBase CommandActivity(string Command, string CommandArgs, string SessionID)
        {
            
            X2CommandRequest req = new X2CommandRequest(Command, CommandArgs, SessionID);
            return m_Engine.SendCommandRequest(Thread.CurrentPrincipal.Identity.Name, req);
        }

        public X2ResponseBase ProcessListActivity(List<ListRequestItem> ItemList)
        {
            
            X2RebuildWorklistRequest req = new X2RebuildWorklistRequest(ItemList);
            return m_Engine.SendProcessListRequest(Thread.CurrentPrincipal.Identity.Name, req);
        }

        public X2ResponseBase AquirePublisherMode(string ADUser, string ProcessName)
        {
            
            return m_Engine.AquirePublisherMode(ADUser, ProcessName);
        }

        public X2ResponseBase ReleasePublisherMode(string ADUser)
        {
            
            return m_Engine.ReleasePublisherMode(ADUser);
        }

        public X2ResponseBase ClearConnectionPool()
        {
            
            return m_Engine.ClearConnectionPool();
        }

        public X2ResponseBase CreateWorkFlowInstanceWithComplete(string SessionID, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> FieldInputs, bool IgnoreWarnings, object Data)
        {
            X2CreateWorkFlowInstanceWithCompleteRequest CWRq = new X2CreateWorkFlowInstanceWithCompleteRequest(SessionID, ProcessName, ProcessVersion, WorkFlowName, ActivityName, FieldInputs, IgnoreWarnings, Data);
            return m_Engine.SendCreateWorkFlowInstanceWithCompleteRequest(CWRq, Thread.CurrentPrincipal.Identity.Name);
        }

        public X2ResponseBase RefreshCacheInX2NodeProcess(Object data)
        {
            SAHL.Core.X2.Messages.IX2NodeManagementMessage message = new SAHL.Core.X2.Messages.Management.X2NodeManagementMessage(SAHL.Core.X2.Messages.X2ManagementType.RefreshCache, data);
            return m_Engine.SendNodeManagmentMessage(message);
        }

        #endregion IX2Provider Members

        #region Private Members

        private void CopyInputFieldList(Dictionary<string, string> Source, Dictionary<string, string> Destination)
        {
            if (Source != null && Destination != null)
            {
                string[] SourceKeys = new string[Source.Keys.Count];
                Source.Keys.CopyTo(SourceKeys, 0);
                string[] SourceVals = new string[Source.Values.Count];
                Source.Values.CopyTo(SourceVals, 0);
                for (int i = 0; i < Source.Count; i++)
                {
                    if (Destination.ContainsKey(SourceKeys[i]))
                    {
                        Destination[SourceKeys[i]] = SourceVals[i];
                    }
                    else
                        Destination.Add(SourceKeys[i], SourceVals[i]);
                }
            }
        }

        #endregion Private Members
    }

    public class X2RemotingEngineProvider : IX2Provider
    {
        static object syncObj = new object();
        IX2Engine m_Engine;
        string m_ConstructorURL = null;
        bool publishOnlyMode = false;

        public X2RemotingEngineProvider()
        {
        }

        public X2RemotingEngineProvider(string X2URL)
        {
            m_ConstructorURL = X2URL;
        }

        #region IX2Provider Members

        public X2ResponseBase LogIn()
        {
            try
            {
                // get an X2 instance
                EnsureEngineInstance();

                X2LoginRequest X2LRq = new X2LoginRequest(null);

                if (m_Engine != null)
                {
                    try
                    {
                        return m_Engine.SendLoginRequest(X2LRq, Thread.CurrentPrincipal.Identity.Name);
                    }
                    catch (Exception Ex)
                    {
                        X2ErrorResponse err = new X2ErrorResponse(DateTime.Now, new X2ResponseException(Ex.Message), ErrorCodes.UnExpectedEngineError, "X2EngineProvider.Login", string.Empty);
                        return err;
                    }
                }
                else
                {
                    X2ErrorResponse err = new X2ErrorResponse(DateTime.Now, new X2ResponseException("Engine could not be retrieved by provider."), ErrorCodes.UnExpectedEngineError, "X2EngineProvider.Login", string.Empty);
                    return err;
                }
            }
            catch (Exception e)
            {
                X2ErrorResponse err = new X2ErrorResponse(DateTime.Now, new X2ResponseException(e.Message), ErrorCodes.UnExpectedEngineError, "X2EngineProvider.Login", string.Empty);
                return err;
            }
        }

        public X2ResponseBase LogOut(string SessionID)
        {
            // get an X2 instance
            EnsureEngineInstance();

            X2LogoutRequest eLOq = new X2LogoutRequest(SessionID);
            return m_Engine.SendLogoutRequest(eLOq, Thread.CurrentPrincipal.Identity.Name);
        }

        public X2ResponseBase CreateWorkFlowInstance(string SessionID, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> FieldInputs, bool IgnoreWarnings)
        {
            return CreateWorkFlowInstance(SessionID, ProcessName, ProcessVersion, WorkFlowName, ActivityName, FieldInputs, IgnoreWarnings, null);
        }

        public X2ResponseBase CreateWorkFlowInstance(string SessionID, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> FieldInputs, bool IgnoreWarnings, object Data)
        {
            // get an X2 instance
            EnsureEngineInstance();

            X2CreateWorkFlowInstanceRequest CWRq = new X2CreateWorkFlowInstanceRequest(SessionID, ProcessName, ProcessVersion, WorkFlowName, ActivityName, FieldInputs, IgnoreWarnings, Data);
            return m_Engine.SendCreateWorkFlowInstanceRequest(CWRq, Thread.CurrentPrincipal.Identity.Name);
        }

        public X2ResponseBase StartActivity(string SessionID, Int64 InstanceID, string ActivityName, bool IgnoreWarnings)
        {
            return StartActivity(SessionID, InstanceID, ActivityName, IgnoreWarnings, null);
        }

        public X2ResponseBase StartActivity(string SessionID, Int64 InstanceID, string ActivityName, bool IgnoreWarnings, object Data)
        {
            // get an X2 instance
            EnsureEngineInstance();

            X2ActivityStartRequest X2ASRq = new X2ActivityStartRequest(SessionID, InstanceID, ActivityName, IgnoreWarnings, Data);
            return m_Engine.SendActivityStartRequest(X2ASRq, Thread.CurrentPrincipal.Identity.Name);
        }

        public X2ResponseBase CompleteActivity(string SessionID, Int64 InstanceID, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings)
        {
            return CompleteActivity(SessionID, InstanceID, ActivityName, InputFields, IgnoreWarnings, null);
        }

        public X2ResponseBase CompleteActivity(string SessionID, Int64 InstanceID, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data)
        {
            // get an X2 instance
            EnsureEngineInstance();

            X2ActivityCompleteRequest X2ACRq = new X2ActivityCompleteRequest(SessionID, InstanceID, ActivityName, IgnoreWarnings, Data);
            CopyInputFieldList(InputFields, X2ACRq.FieldInputList);
            return m_Engine.SendActivityCompleteRequest(X2ACRq, Thread.CurrentPrincipal.Identity.Name);
        }

        public X2ResponseBase CreateCompleteActivity(string SessionID, Int64 InstanceID, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data)
        {
            // get an X2 instance
            EnsureEngineInstance();

            X2ActivityCompleteRequest X2ACRq = new X2ActivityCompleteRequest(SessionID, InstanceID, ActivityName, IgnoreWarnings, Data);
            CopyInputFieldList(InputFields, X2ACRq.FieldInputList);
            return m_Engine.SendActivityCreateCompleteRequest(X2ACRq, Thread.CurrentPrincipal.Identity.Name);
        }

        public X2ResponseBase CancelActivity(string SessionID, Int64 InstanceID, string ActivityName)
        {
            // get an X2 instance
            EnsureEngineInstance();

            X2ActivityCancelRequest X2ACnRq = new X2ActivityCancelRequest(SessionID, InstanceID, ActivityName);
            return m_Engine.SendActivityCancelRequest(X2ACnRq, Thread.CurrentPrincipal.Identity.Name);
        }

        public X2ResponseBase CommandActivity(string Command, string CommandArgs, string SessionID)
        {
            EnsureEngineInstance();
            X2CommandRequest req = new X2CommandRequest(Command, CommandArgs, SessionID);
            return m_Engine.SendCommandRequest(Thread.CurrentPrincipal.Identity.Name, req);
        }

        public X2ResponseBase ProcessListActivity(List<ListRequestItem> ItemList)
        {
            EnsureEngineInstance();
            X2RebuildWorklistRequest req = new X2RebuildWorklistRequest(ItemList);
            return m_Engine.SendProcessListRequest(Thread.CurrentPrincipal.Identity.Name, req);
        }

        public X2ResponseBase AquirePublisherMode(string ADUser, string ProcessName)
        {
            EnsureEngineInstance();
            return m_Engine.AquirePublisherMode(ADUser, ProcessName);
        }

        public X2ResponseBase ReleasePublisherMode(string ADUser)
        {
            EnsureEngineInstance();
            return m_Engine.ReleasePublisherMode(ADUser);
        }

        public X2ResponseBase ClearConnectionPool()
        {
            EnsureEngineInstance();
            return m_Engine.ClearConnectionPool();
        }

        public X2ResponseBase CreateWorkFlowInstanceWithComplete(string SessionID, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> FieldInputs, bool IgnoreWarnings, object Data)
        {
            X2CreateWorkFlowInstanceWithCompleteRequest CWRq = new X2CreateWorkFlowInstanceWithCompleteRequest(SessionID, ProcessName, ProcessVersion, WorkFlowName, ActivityName, FieldInputs, IgnoreWarnings, Data);
            return m_Engine.SendCreateWorkFlowInstanceWithCompleteRequest(CWRq, Thread.CurrentPrincipal.Identity.Name);
        }

        public X2ResponseBase RefreshCacheInX2NodeProcess(Object data)
        {
            SAHL.Core.X2.Messages.IX2NodeManagementMessage message = new SAHL.Core.X2.Messages.Management.X2NodeManagementMessage(SAHL.Core.X2.Messages.X2ManagementType.RefreshCache, data);
            return m_Engine.SendNodeManagmentMessage(message);
        }

        #endregion IX2Provider Members

        #region Private Members

        private void EnsureEngineInstance()
        {
            if (m_Engine != null) return;
            lock (syncObj)
            {
                if (m_Engine == null)
                {
                    IChannel Ch = ChannelServices.GetChannel("X2");
                    if (Ch == null)
                    {
                        IDictionary dict = new Hashtable();
                        dict["port"] = "8089";
                        dict["name"] = "X2";
                        //// Set up a client channel.
                        TcpClientChannel chan = new TcpClientChannel(dict, null);
                        ChannelServices.RegisterChannel(chan, false);
                    }
                    string URL = null;
                    if (m_ConstructorURL == null)
                        URL = Properties.Settings.Default.X2URL;
                    else
                        URL = m_ConstructorURL;
                    m_Engine = (IX2Engine)Activator.GetObject(typeof(IX2Engine), URL);
                    if (m_Engine == null)
                        throw new Exception("Could not retrieve an instance of the engine from: " + URL);

                    try
                    {
                        if (!m_Engine.IsRunning)
                        {
                            m_Engine.StartEngine(publishOnlyMode);
                        }
                        //cm = CacheFactory.GetCacheManager("X2");
                        //if (!cm.Contains("ENGINE"))
                        //  cm.Add("ENGINE", m_Engine);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("IsRunningFailed: " + e.Message);
                    }
                }
            }
        }

        private void CopyInputFieldList(Dictionary<string, string> Source, Dictionary<string, string> Destination)
        {
            if (Source != null && Destination != null)
            {
                string[] SourceKeys = new string[Source.Keys.Count];
                Source.Keys.CopyTo(SourceKeys, 0);
                string[] SourceVals = new string[Source.Values.Count];
                Source.Values.CopyTo(SourceVals, 0);
                for (int i = 0; i < Source.Count; i++)
                {
                    if (Destination.ContainsKey(SourceKeys[i]))
                    {
                        Destination[SourceKeys[i]] = SourceVals[i];
                    }
                    else
                        Destination.Add(SourceKeys[i], SourceVals[i]);
                }
            }
        }

        #endregion Private Members


       
    }
}