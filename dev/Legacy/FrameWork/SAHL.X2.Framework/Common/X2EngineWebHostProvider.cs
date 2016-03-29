using SAHL.Common.Logging;
using SAHL.Config.Services.X2.Client;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2.Common;
using SAHL.X2.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.X2.Framework.Common
{
    public class X2EngineWebHostProvider : IX2Engine
    {
        private string xmlResponse = "<X2Response></X2Response>";

        public X2EngineWebHostProvider()
        {
        }

        private X2ServiceClient GetConfiguredClient()
        {
            IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration(Properties.Settings.Default.X2WebHost_Url);
            IJsonActivator jsonActivator = new JsonActivator();
            X2ServiceClient client = new X2ServiceClient(serviceConfiguration);
            return client;
        }

        private X2Response SendRequest<T>(T request) where T : X2Request
        {
            X2ServiceClient client = null;
            try
            {
                client = GetConfiguredClient();
                var response = client.PerformCommand(request);
                return request.Result;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessage("SendRequest", string.Format("problem sending request {0}", ex.ToString()));
                throw;
            }
        }

        private X2Response SendMessage<T>(T message) where T : SAHL.Core.X2.Messages.IX2Message
        {
            X2ServiceClient client = null;
            try
            {
                client = GetConfiguredClient();
                var response = client.PerformCommand(message);
                return message.Result;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessage("SendRequest", string.Format("problem sending request {0}", ex.ToString()));
                throw;
            }
        }

        private IX2MessageCollection ConvertMessages(SystemMessageCollection messages)
        {
            X2MessageCollection x2Messages = new X2MessageCollection();
            if (messages == null)
                return x2Messages;
            foreach (var message in messages.AllMessages)
            {
                bool found = false;
                foreach (var x2msg in x2Messages)
                {
                    if (x2msg.Message == message.Message)
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                    continue;
                switch (message.Severity)
                {
                    case SystemMessageSeverityEnum.Error:
                        {
                            x2Messages.Add(new X2Message(message.Message, X2MessageType.Error));
                            break;
                        }
                    case SystemMessageSeverityEnum.Warning:
                        {
                            x2Messages.Add(new X2Message(message.Message, X2MessageType.Warning));
                            break;
                        }
                }
            }
            return x2Messages;
        }

        private Dictionary<string, string> ConvertFieldInputs(X2FieldInputList fieldInputList)
        {
            Dictionary<string, string> mapVars = new Dictionary<string, string>();
            if (null == fieldInputList) return mapVars;
            List<string> Keys = fieldInputList.Keys.ToList();
            foreach (string key in Keys)
            {
                mapVars.Add(key, fieldInputList[key]);
            }
            return mapVars;
        }

        public bool IsRunning
        {
            get;
            protected set;
        }

        public X2ResponseBase SendNodeManagmentMessage(IX2NodeManagementMessage message)
        {
            LogPlugin.Logger.LogDebugMessage("SendNodeManagementMessage", "Starting");

            X2Response requestResponse = SendMessage(message);
            if (requestResponse.IsErrorResponse)
            {
                LogPlugin.Logger.LogDebugMessage("SendNodeManagementMessage", string.Format("Complete - Fail {0}", requestResponse.Message));
                var response = new SAHL.X2.Framework.Interfaces.X2ErrorResponse(DateTime.Now, new X2ResponseException(requestResponse.Message), 12, "", xmlResponse);
                response.Messages = ConvertMessages(requestResponse.SystemMessages);
                string errorMessage = ((SAHL.Core.X2.Messages.X2ErrorResponse)requestResponse).Message;
                if (response.Messages.Count() == 0)
                {
                    response.Messages.Add(new X2Message(errorMessage, X2MessageType.Error));
                }
                return response;
            }
            else
            {
                LogPlugin.Logger.LogDebugMessage("SendNodeManagementMessage", "Complete");
                var response = new X2EngineResponse(DateTime.Now, xmlResponse);
                response.Messages = ConvertMessages(requestResponse.SystemMessages);
                return response;
            }
        }

        public X2ResponseBase SendActivityCancelRequest(X2ActivityCancelRequest request, string adUserName)
        {
            LogPlugin.Logger.LogDebugMessage("SendActivityCancelRequest", "Starting");
            X2RequestType requestType = X2RequestType.UserCancel;
            var serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata.Add(ServiceRequestMetadata.HEADER_USERNAME, adUserName);
            var complete = new X2RequestForExistingInstance(Guid.NewGuid(), request.InstanceId, requestType, serviceRequestMetadata, request.ActivityName, request.IgnoreWarnings);

            X2Response requestResponse = SendRequest(complete);
            if (requestResponse.IsErrorResponse)
            {
                LogPlugin.Logger.LogDebugMessage("SendActivityCancelRequest", string.Format("Complete - Fail {0}", requestResponse.Message));
                var response = new SAHL.X2.Framework.Interfaces.X2ErrorResponse(DateTime.Now, new X2ResponseException(requestResponse.Message), 12, "", xmlResponse);
                response.Messages = ConvertMessages(requestResponse.SystemMessages);
                string errorMessage = ((SAHL.Core.X2.Messages.X2ErrorResponse)requestResponse).Message;
                if (response.Messages.Count() == 0)
                {
                    response.Messages.Add(new X2Message(errorMessage, X2MessageType.Error));
                }
                return response;
            }
            else
            {
                LogPlugin.Logger.LogDebugMessage("SendActivityCancelRequest", "Complete");
                var response = new X2ActivityCancelResponse(DateTime.Now, request.InstanceId, request.ActivityName, xmlResponse);
                response.Messages = ConvertMessages(requestResponse.SystemMessages);
                return response;
            }
        }

        public X2ResponseBase SendActivityCompleteRequest(X2ActivityCompleteRequest request, string adUserName)
        {
            LogPlugin.Logger.LogDebugMessage("SendActivityCompleteRequest", "Starting");
            X2RequestType requestType = X2RequestType.UserComplete;
            var fieldInputs = ConvertFieldInputs(request.FieldInputList);
            var serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata.Add(ServiceRequestMetadata.HEADER_USERNAME, adUserName);
            var complete = new X2RequestForExistingInstance(Guid.NewGuid(), request.InstanceId, requestType, serviceRequestMetadata, request.ActivityName, request.IgnoreWarnings, fieldInputs, request.Data);

            // We need todo something here about the messages. They are now being returnd we just eating them
            X2Response requestResponse = SendRequest(complete);
            if (requestResponse.IsErrorResponse)
            {
                LogPlugin.Logger.LogDebugMessage("SendActivityCompleteRequest", string.Format("Complete - Fail {0}", requestResponse.Message));
                var response = new SAHL.X2.Framework.Interfaces.X2ErrorResponse(DateTime.Now, new X2ResponseException(requestResponse.Message), 13, "", xmlResponse);
                response.Messages = ConvertMessages(requestResponse.SystemMessages);
                string errorMessage = ((SAHL.Core.X2.Messages.X2ErrorResponse)requestResponse).Message;
                if (response.Messages.Count() == 0)
                {
                    response.Messages.Add(new X2Message(errorMessage, X2MessageType.Error));
                }
                return response;
            }
            else
            {
                LogPlugin.Logger.LogDebugMessage("SendActivityCompleteRequest", "Complete");
                var response = new X2ActivityCompleteResponse(DateTime.Now, request.InstanceId, "", request.ActivityName, "", xmlResponse);
                response.Messages = ConvertMessages(requestResponse.SystemMessages);
                return response;
            }
        }

        public X2ResponseBase SendActivityCreateCompleteRequest(X2ActivityCompleteRequest request, string adUserName)
        {
            LogPlugin.Logger.LogDebugMessage("SendActivityCreateCompleteRequest", "Starting");
            X2RequestType requestType = X2RequestType.CreateComplete;
            var inputFields = ConvertFieldInputs(request.FieldInputList);
            var serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata.Add(ServiceRequestMetadata.HEADER_USERNAME, adUserName);
            var complete = new X2RequestForExistingInstance(Guid.NewGuid(), request.InstanceId, requestType, serviceRequestMetadata, request.ActivityName,
                request.IgnoreWarnings, inputFields, request.Data);

            X2Response requestResponse = SendRequest(complete);
            if (requestResponse.IsErrorResponse)
            {
                LogPlugin.Logger.LogDebugMessage("SendActivityCreateCompleteRequest", string.Format("Complete - Fail {0}", requestResponse.Message));
                var response = new SAHL.X2.Framework.Interfaces.X2ErrorResponse(DateTime.Now, new X2ResponseException(requestResponse.Message), 13, "", xmlResponse);
                response.Messages = ConvertMessages(requestResponse.SystemMessages);
                string errorMessage = ((SAHL.Core.X2.Messages.X2ErrorResponse)requestResponse).Message;
                if (response.Messages.Count() == 0)
                {
                    response.Messages.Add(new X2Message(errorMessage, X2MessageType.Error));
                }
                return response;
            }
            else
            {
                LogPlugin.Logger.LogDebugMessage("SendActivityCreateCompleteRequest", "Complete");
                var response = new X2ActivityCompleteResponse(DateTime.Now, request.InstanceId, "", request.ActivityName, "", xmlResponse);
                response.Messages = ConvertMessages(requestResponse.SystemMessages);
                return response;
            }
        }

        public X2ResponseBase SendActivityStartRequest(X2ActivityStartRequest request, string adUserName)
        {
            LogPlugin.Logger.LogDebugMessage("SendActivityStartRequest", "Starting");
            var serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata.Add(ServiceRequestMetadata.HEADER_USERNAME, adUserName);
            var fieldInputs = ConvertFieldInputs(request.FieldInputList);
            var start = new X2RequestForExistingInstance(Guid.NewGuid(), request.InstanceId, X2RequestType.UserStart, serviceRequestMetadata, request.ActivityName, 
                request.IgnoreWarnings, fieldInputs, request.Data);
            X2Response requestResponse = SendRequest(start);
            if (requestResponse.IsErrorResponse)
            {
                LogPlugin.Logger.LogDebugMessage("SendActivityStartRequest", string.Format("Complete - Fail {0}", requestResponse.Message));
                var response = new SAHL.X2.Framework.Interfaces.X2ErrorResponse(DateTime.Now, new X2ResponseException(requestResponse.Message), 12, "", xmlResponse);
                response.Messages = ConvertMessages(requestResponse.SystemMessages);
                string errorMessage = ((SAHL.Core.X2.Messages.X2ErrorResponse)requestResponse).Message;
                if (response.Messages.Count() == 0)
                {
                    response.Messages.Add(new X2Message(errorMessage, X2MessageType.Error));
                }
                return response;
            }
            else
            {
                LogPlugin.Logger.LogDebugMessage("SendActivityStartRequest", "Complete");
                var errorResponse = new X2ActivityStartResponse(DateTime.Now, request.InstanceId, "", request.ActivityName, "", xmlResponse);
                errorResponse.Messages = ConvertMessages(requestResponse.SystemMessages);
                return errorResponse;
            }
        }

        public X2ResponseBase SendCreateWorkFlowInstanceWithCompleteRequest(X2CreateWorkFlowInstanceWithCompleteRequest request, string adUserName)
        {
            LogPlugin.Logger.LogDebugMessage("SendActivityCreateInstanceWithCompleteRequest", "Starting");
            var serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata.Add(ServiceRequestMetadata.HEADER_USERNAME, adUserName);
            var fieldInputs = ConvertFieldInputs(request.FieldInputList);
            var create = new SAHL.Core.X2.Messages.X2CreateInstanceWithCompleteRequest(Guid.NewGuid(), request.ActivityName, request.ProcessName, request.WorkFlowName, serviceRequestMetadata,
                request.IgnoreWarnings, request.ReturnActivityID, request.SourceInstanceID, fieldInputs, request.Data);

            X2Response requestResponse = SendRequest(create);
            if (requestResponse.IsErrorResponse)
            {
                LogPlugin.Logger.LogDebugMessage("SendActivityCreateInstanceWithCompleteRequest", string.Format("Complete - Fail {0}", requestResponse.Message));
                var response = new SAHL.X2.Framework.Interfaces.X2ErrorResponse(DateTime.Now, new X2ResponseException(requestResponse.Message), 12, "", xmlResponse);
                response.Messages = ConvertMessages(requestResponse.SystemMessages);
                string errorMessage = ((SAHL.Core.X2.Messages.X2ErrorResponse)requestResponse).Message;
                if (response.Messages.Count() == 0)
                {
                    response.Messages.Add(new X2Message(errorMessage, X2MessageType.Error));
                }
                return response;
            }
            else
            {
                LogPlugin.Logger.LogDebugMessage("SendActivityCreateInstanceWithCompleteRequest", "Complete");
                var response = new X2ActivityCompleteResponse(DateTime.Now, requestResponse.InstanceId, "", request.ActivityName, "", xmlResponse);
                response.Messages = ConvertMessages(requestResponse.SystemMessages);
                return response;
            }
        }

        public X2ResponseBase SendCreateWorkFlowInstanceRequest(X2CreateWorkFlowInstanceRequest request, string aDUserName)
        {
            LogPlugin.Logger.LogDebugMessage("SendActivityCreateRequest", "Starting");
            var inputFields = ConvertFieldInputs(request.FieldInputList);
            var serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata.Add(ServiceRequestMetadata.HEADER_USERNAME, aDUserName);
            var create = new X2CreateInstanceRequest(Guid.NewGuid(), request.ActivityName, request.ProcessName, request.WorkFlowName, serviceRequestMetadata, request.IgnoreWarnings,
                request.ReturnActivityID, request.SourceInstanceID, inputFields, request.Data);

            X2Response requestResponse = SendRequest(create);
            if (requestResponse.IsErrorResponse)
            {
                LogPlugin.Logger.LogDebugMessage("SendActivityCreateRequest", string.Format("Complete - Fail {0}", requestResponse.Message));
                var response = new SAHL.X2.Framework.Interfaces.X2ErrorResponse(DateTime.Now, new X2ResponseException(requestResponse.Message), 12, "", xmlResponse);
                response.Messages = ConvertMessages(requestResponse.SystemMessages);
                string errorMessage = ((SAHL.Core.X2.Messages.X2ErrorResponse)requestResponse).Message;
                if (response.Messages.Count() == 0)
                {
                    response.Messages.Add(new X2Message(errorMessage, X2MessageType.Error));
                }
                return response;
            }
            else
            {
                LogPlugin.Logger.LogDebugMessage("SendActivityCreateRequest", "Complete");
                var errorResponse = new X2ActivityStartResponse(DateTime.Now, requestResponse.InstanceId, "", request.ActivityName, "", xmlResponse);
                errorResponse.Messages = ConvertMessages(requestResponse.SystemMessages);
                return errorResponse;
            }
        }

        public X2ResponseBase SendLoginRequest(X2LoginRequest Request, string ADUserName)
        {
            return new X2LoginResponse(ADUserName, Guid.NewGuid().ToString(), DateTime.Now, DateTime.Now, xmlResponse);
        }

        public X2ResponseBase SendLogoutRequest(X2LogoutRequest Request, string ADUserName)
        {
            return new X2LogoutResponse(xmlResponse);
        }

        public X2ResponseBase SendProcessListRequest(string ADUser, X2RebuildWorklistRequest request)
        {
            if (request.ItemList == null || request.ItemList.Count == 0)
            {
                throw new ArgumentException("Itemlist can not be empty");
            }
            X2Response requestResponse = null;
            foreach (var item in request.ItemList)
            {
                var serviceRequestMetadata = new ServiceRequestMetadata();
                serviceRequestMetadata.Add(ServiceRequestMetadata.HEADER_USERNAME, ADUser);
                SAHL.Core.X2.Messages.X2RequestForSecurityRecalc recalc = new X2RequestForSecurityRecalc(Guid.NewGuid(), item.InstnaceID, serviceRequestMetadata);
                requestResponse = SendRequest(recalc);
                if (requestResponse.IsErrorResponse)
                {
                    var response = new SAHL.X2.Framework.Interfaces.X2ErrorResponse(DateTime.Now, new X2ResponseException(requestResponse.Message), 12, "", xmlResponse);
                    response.Messages = ConvertMessages(requestResponse.SystemMessages);
                    return response;
                }
            }
            var happyResponse = new X2ActivityStartResponse(DateTime.Now, requestResponse.InstanceId, "", "", "", xmlResponse);
            happyResponse.Messages = ConvertMessages(requestResponse.SystemMessages);
            return happyResponse;
        }

        public void StartEngine(bool publishOnlyMode)
        {
            IsRunning = true;
        }

        public void StopEngine()
        {
            throw new NotImplementedException();
        }

        #region old not required obsolete useless methods of not usedness :)

        public X2ResponseBase AquirePublisherMode(string ADUser, string ProcessName)
        {
            // dont need
            throw new NotImplementedException();
        }

        public X2ResponseBase ClearConnectionPool()
        {
            // dont need
            throw new NotImplementedException();
        }

        public X2ResponseBase ReleasePublisherMode(string ADUser)
        {
            // dont need
            throw new NotImplementedException();
        }

        public X2ResponseBase ResetScheduledEventTimer()
        {
            // dont need
            throw new NotImplementedException();
        }

        public X2ResponseBase SendCommandRequest(string ADUser, X2CommandRequest request)
        {
            // not needed
            throw new NotImplementedException();
        }

        public X2ResponseBase SendExternalActivityNotificationRequest()
        {
            // not needed
            throw new NotImplementedException();
        }

        #endregion old not required obsolete useless methods of not usedness :)
    }
}