using Microsoft.SqlServer.Server;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml;

public partial class StoredProcedures
{
    [SqlProcedure]
    public static void SendLegacyEventFromCompositeWebRequest()
    {
        string messageBody = string.Empty;
        try
        {
            const string LegacyEventFromCompositeTargetQueue = "LegacyEventFromCompositeTargetQueue";
            const string LegacyEventFromCompositeMessageType = "LegacyEventFromCompositeMessage";

            Uri uri = GetLegacyEventGeneratorWebURI();

            Guid? conversationHandle = null;
            messageBody = ServiceBrokerMessagingHelper.GetMessageFromQueue(LegacyEventFromCompositeTargetQueue, out conversationHandle);
            while (!string.IsNullOrEmpty(messageBody) && conversationHandle.HasValue)
            {
                try
                {
                    int stageTransitionCompositeKey = ServiceBrokerMessagingHelper.GetKeyByTagNameFromXmlMessage(messageBody, "StageTransitionCompositeKey");

                    if (stageTransitionCompositeKey == 0)
                    {
                        throw new Exception("Invalid StageTransitionCompositeKey");
                    }

                    string requestContents = @"{" +
                                            "\"_name\":" + "\"SAHL.Services.Interfaces.LegacyEventGenerator.Commands.CreateLegacyEventFromCompositeCommand," +
                                            "SAHL.Services.Interfaces.LegacyEventGenerator\"," +
                                            "\"StageTransitionCompositeKey\":" + stageTransitionCompositeKey + "}";

                    HttpWebRequest request = CreateEvent(uri, requestContents);

                    HttpStatusCode statusCode = GetResponseHttpStatus(conversationHandle, requestContents, request);
                    if (statusCode == HttpStatusCode.OK)
                    {
                        ServiceBrokerMessagingHelper.EndConversation(conversationHandle.Value, LegacyEventFromCompositeMessageType, messageBody);
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        LogError(messageBody, ex, "SendLegacyEventFromCompositeWebRequest");
                        SendToErrorQueue(messageBody, "LegacyEventFromCompositeServiceInitiator",
                                                      "LegacyEventFromCompositeErrorQueueTarget",
                                                      "ProcessLegacyEventFromCompositeMessage",
                                                      "LegacyEventFromCompositeMessage");

                        ServiceBrokerMessagingHelper.EndConversation(conversationHandle.Value, LegacyEventFromCompositeMessageType, messageBody);

                        //Clear the message as we have moved it to the Dead Letter Queue
                        messageBody = string.Empty;
                    }
                    catch (Exception ex2)
                    {
                        LogError(messageBody, ex2, "SendLegacyEventFromCompositeWebRequest");
                    }
                }
                messageBody = ServiceBrokerMessagingHelper.GetMessageFromQueue(LegacyEventFromCompositeTargetQueue, out conversationHandle);
            }

        }
        catch (Exception ex)
        {
            try
            {
                LogError(messageBody, ex, "SendLegacyEventFromCompositeWebRequest");
            }
            catch (Exception)
            {
                SqlContext.Pipe.Send("Failed logging Exception:" + ex.ToString());
            }
        }
    }

    [SqlProcedure]
    public static void SendLegacyEventFromTransitionWebRequest()
    {
        string messageBody = string.Empty;
        try
        {
            const string LegacyEventFromTransitionQueueName = "LegacyEventFromTransitionQueue";
            const string LegacyEventFromTransitionMessageType = "LegacyEventFromTransitionMessage";

            Uri uri = GetLegacyEventGeneratorWebURI();

            Guid? conversationHandle = null;
            messageBody = ServiceBrokerMessagingHelper.GetMessageFromQueue(LegacyEventFromTransitionQueueName, out conversationHandle);
            while (!string.IsNullOrEmpty(messageBody) && conversationHandle.HasValue)
            {
                try
                {
                    int stageTransitionKey = ServiceBrokerMessagingHelper.GetKeyByTagNameFromXmlMessage(messageBody, "StageTransitionKey");

                    if (stageTransitionKey == 0)
                    {
                        throw new Exception("Invalid StageTransitionKey");
                    }

                    string requestContents = @"{" +
                                            "\"_name\":" + "\"SAHL.Services.Interfaces.LegacyEventGenerator.Commands.CreateLegacyEventFromTransitionCommand," +
                                            "SAHL.Services.Interfaces.LegacyEventGenerator\"," +
                                            "\"StageTransitionKey\":" + stageTransitionKey + "}";

                    HttpWebRequest request = CreateEvent(uri, requestContents);

                    HttpStatusCode statusCode = GetResponseHttpStatus(conversationHandle, requestContents, request);
                    if (statusCode == HttpStatusCode.OK)
                    {
                        ServiceBrokerMessagingHelper.EndConversation(conversationHandle.Value, LegacyEventFromTransitionMessageType, messageBody);
                    }

                }
                catch (Exception ex)
                {
                    try
                    {
                        LogError(messageBody, ex, "SendLegacyEventFromTransitionWebRequest");
                        SendToErrorQueue(messageBody, "LegacyEventFromTransitionServiceInitiator",
                                                      "LegacyEventFromTransitionErrorQueueTarget",
                                                      "ProcessLegacyEventFromTransitionMessage",
                                                      "LegacyEventFromTransitionMessage");

                        ServiceBrokerMessagingHelper.EndConversation(conversationHandle.Value, LegacyEventFromTransitionMessageType, messageBody);

                        //Clear the message as we have moved it to the Dead Letter Queue
                        messageBody = string.Empty;
                    }
                    catch (Exception ex2)
                    {
                        LogError(messageBody, ex2, "SendLegacyEventFromTransitionWebRequest");
                    }
                }

                messageBody = ServiceBrokerMessagingHelper.GetMessageFromQueue(LegacyEventFromTransitionQueueName, out conversationHandle);
            }
        }
        catch (Exception ex)
        {
            try
            {
                LogError(messageBody, ex, "SendLegacyEventFromTransitionWebRequest");
            }
            catch (Exception ex2)
            {
                SqlContext.Pipe.Send("Failed logging Exception:" + ex2.ToString());
            }
        }
    }

    private static void SendToErrorQueue(string MessageBody, string fromService, string toService, string contract, string messageType)
    {
        string queryText = string.Format(@"DECLARE @Dialog_Handle UNIQUEIDENTIFIER
                                BEGIN DIALOG CONVERSATION @Dialog_Handle
                                FROM SERVICE {0}
                                TO SERVICE '{1}'
                                ON CONTRACT {2}
                                WITH ENCRYPTION = OFF;

                                SEND ON CONVERSATION @Dialog_Handle
                                MESSAGE TYPE {3} (@MessageBody);", fromService, toService, contract, messageType);
        using (SqlConnection sqlConnection = new SqlConnection("context connection=true"))
        using (SqlCommand cmd = new SqlCommand(queryText, sqlConnection))
        {
            sqlConnection.Open();
            cmd.CommandText = queryText;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("MessageBody", MessageBody));
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }

    private static void LogError(string MessageBody, Exception ex, string methodName)
    {
        using (SqlConnection sqlConnection = new SqlConnection("context connection=true"))
        using (SqlCommand cmd = new SqlCommand())
        {
            sqlConnection.Open();
            cmd.Connection = sqlConnection;

            var query = @"INSERT INTO [Cuttlefish].[dbo].[LogMessage] ( [MessageDate], [LogMessageType], [MethodName], [Message], [Source], [UserName], [MachineName], [Application] )
                                VALUES (@messageDate, @logMessageType, @methodName, @message, @source, @userName, @machineName, @application);
                                SELECT SCOPE_IDENTITY();";
            cmd.CommandText = query;
            cmd.Parameters.Add(new SqlParameter("messageDate", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("logMessageType", "Error"));
            cmd.Parameters.Add(new SqlParameter("methodName", methodName));
            cmd.Parameters.Add(new SqlParameter("message", ex.Message));
            var method = MethodBase.GetCurrentMethod();
            var type = method.DeclaringType;
            var fullName = string.Format("{0}.{1}", type.AssemblyQualifiedName.Replace(", Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", ""), methodName);
            cmd.Parameters.Add(new SqlParameter("source", fullName));
            cmd.Parameters.Add(new SqlParameter("userName", "System"));

            cmd.Parameters.Add(new SqlParameter("machineName", "Database"));
            cmd.Parameters.Add(new SqlParameter("application", type.Assembly.FullName.Replace(", Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "")));
            var id = cmd.ExecuteScalar();

            query = @"INSERT INTO [Cuttlefish].[dbo].[MessageParameters] ( [LogMessage_id], [ParameterValue], [ParameterKey] )
                                      VALUES(@logMessage_id, @parameterValue, @parameterKey)";
            cmd.CommandText = query;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("logMessage_id", Convert.ToInt64(id)));
            cmd.Parameters.Add(new SqlParameter("parameterValue", ex.ToString()));
            cmd.Parameters.Add(new SqlParameter("parameterKey", "Exception"));
            cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("logMessage_id", Convert.ToInt64(id)));
            cmd.Parameters.Add(new SqlParameter("parameterValue", MessageBody));
            cmd.Parameters.Add(new SqlParameter("parameterKey", "Message"));
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }

    private static HttpStatusCode GetResponseHttpStatus(Guid? conversationHandle, string requestContents, HttpWebRequest request)
    {
        using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            WebResponse response = null;
            try
            {
                streamWriter.Write(requestContents);
                streamWriter.Flush();
                streamWriter.Close();

                response = request.GetResponse();
                return ((HttpWebResponse)response).StatusCode;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }
    }

    private static HttpWebRequest CreateEvent(Uri uri, string requestContents)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = requestContents.Length;
        request.ImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
        request.UserAgent = "SQL Server CLR";
        return request;
    }

    private static Uri GetLegacyEventGeneratorWebURI()
    {
        string legacyEventGeneratorWebURI;
        using (SqlConnection sqlConnection = new SqlConnection("context connection=true"))
        {
            sqlConnection.Open();
            SqlCommand command = new SqlCommand(@"Select ControlText from [2am].[dbo].Control (nolock) where ControlDescription = 'LegacyEventGeneratorWebURI'", sqlConnection);
            legacyEventGeneratorWebURI = (string)command.ExecuteScalar();
        }

        if (string.IsNullOrEmpty(legacyEventGeneratorWebURI))
        {
            throw new Exception("Cannot find LegacyEventGenerator Web URI in the Control table");
        }

        Uri uri = null;
        if (!Uri.TryCreate(legacyEventGeneratorWebURI, UriKind.Absolute, out uri))
        {
            throw new Exception("Invalid LegacyEventGenerator Web URI in the Control table");
        }
        return uri;
    }
}

public static class ServiceBrokerMessagingHelper
{
    public static string GetMessageFromQueue(string queueName, out Guid? conversationHandle, int top = 1)
    {
        conversationHandle = null;
        string messageBody = string.Empty;

        using (SqlConnection sqlConnection = new SqlConnection("context connection=true"))
        {
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand(string.Format(@"WAITFOR (RECEIVE TOP ({0}) conversation_handle, CONVERT(XML, message_body) as message_body
                                                                    FROM [{1}]),TIMEOUT 10000", top, queueName), sqlConnection);

            sqlCommand.CommandTimeout = 0;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.Read())
            {
                conversationHandle = new Guid(sqlDataReader[0].ToString());
                messageBody = sqlDataReader[1].ToString();
            }
            sqlDataReader.Close();
        }
        return messageBody;
    }

    public static void EndConversation(Guid conversationHandle, string messageType, string responseMessage)
    {
        using (SqlConnection sqlConnection = new SqlConnection("context connection=true"))
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(string.Format(@"END CONVERSATION '{0}';", conversationHandle, messageType, responseMessage), sqlConnection);
            cmd.ExecuteNonQuery();
        }
    }

    public static void ResetQueueActivation(string queueName)
    {
        using (SqlConnection sqlConnection = new SqlConnection("context connection=true"))
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(string.Format(@"RECEIVE TOP(0) * FROM {0};", queueName), sqlConnection);
            cmd.ExecuteNonQuery();
        }
    }

    public static int GetKeyByTagNameFromXmlMessage(string MessageBody, string KeyTagName)
    {
        try
        {
            int stageTransitionCompositeKey = 0;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<root>" + MessageBody + "</root>");

            XmlNodeList nodeList = doc.GetElementsByTagName(KeyTagName);
            if (nodeList.Count > 0)
            {
                int.TryParse(nodeList[nodeList.Count - 1].InnerXml, out stageTransitionCompositeKey);
            }

            return stageTransitionCompositeKey;
        }
        catch
        {
            return 0;
        }
    }
}