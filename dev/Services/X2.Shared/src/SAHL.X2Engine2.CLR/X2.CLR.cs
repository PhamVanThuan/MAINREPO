using Microsoft.SqlServer.Server;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml;

namespace SAHL.X2Engine2.CLR
{
    public partial class UserDefinedFunctions
    {
        //update [2am].dbo.control set controltext='http://sahls14/X2EngineService/api/commandhandlerwithreturneddata/performcommand' where ControlNumber=
        [SqlProcedure]
        public static int SendExternalActivityWebRequest(int activeExternalActivityID)
        {
            int success = -1;
            var requestContents = String.Empty;
            try
            {
                int workflowID = 0;
                int activatingInstanceID = 0;
                int externalActivityID = 0;
                string activityXMLData = string.Empty;

                // get the required data using the ID of the inserted ActiveExternalActivity record
                using (SqlConnection connection = new SqlConnection("context connection=true"))
                {
                    connection.Open();
                    var commandText = String.Format("Select ExternalActivityID, WorkflowID, ActivatingInstanceID, ActivityXMLData from [X2].[X2].ActiveExternalActivity where ID = {0}",
                                                                                                                                                                    activeExternalActivityID);
                    SqlCommand command = new SqlCommand(commandText, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        externalActivityID = reader[0] == null ? 0 : Convert.ToInt32(reader[0]);
                        workflowID = reader[1] == null ? 0 : Convert.ToInt32(reader[1]);
                        if (!DBNull.Equals(DBNull.Value, reader[2]))
                            activatingInstanceID = Convert.ToInt32(reader[2]);
                        if (!DBNull.Equals(DBNull.Value, reader[3]))
                            activityXMLData = Convert.ToString(reader[3]);
                    }
                    reader.Close();
                }
                // get the url of the web host from the control table
                string controllerUrl = String.Empty;
                using (SqlConnection connection = new SqlConnection("context connection=true"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select ControlText from [2am].[dbo].Control where ControlDescription = 'X2WebAPIUri' and ControlGroupKey = 5", connection);
                    controllerUrl = (string)command.ExecuteScalar();
                }

                if (String.IsNullOrEmpty(controllerUrl))
                    throw new Exception("Cannot find X2 Web API Uri in [2am].[dbo].Control table");

                // build up the uri to the SendRequest Controller
                Uri uri = null;
                Uri.TryCreate(controllerUrl, UriKind.Absolute, out uri);

                // build up a string of the map variables from the xmldata
                string mapVariables = string.Empty;

                if (!String.IsNullOrEmpty(activityXMLData))
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(activityXMLData);

                    XmlNodeList xmlFieldInputs = xdoc.SelectNodes("//FieldInputs/FieldInput");
                    int fieldCount = 0;
                    string delimiter = "";
                    foreach (XmlNode xmlField in xmlFieldInputs)
                    {
                        fieldCount++;

                        string name = xmlField.Attributes["Name"].InnerText;
                        string value = xmlField.InnerXml;
                        value = value.Replace("\\", "\\\\");
                        if (fieldCount > 1)
                            delimiter = ",";

                        mapVariables += delimiter + String.Format("\"{0}\":\"{1}\"", name, value.Replace("\"", "\\\""));
                    }
                }
                var serviceRequestMetadata = "\"$type\":" + "\"SAHL.Core.Services.ServiceRequestMetadata,SAHL.Core\"";
                requestContents = "{" +
                                      "\"$type\":" + "\"SAHL.Core.X2.Messages.X2ExternalActivityRequest, SAHL.Core.X2\"" + "," +
                                      "\"ActiveExternalActivityId\":" + activeExternalActivityID + "," +
                                      "\"ExternalActivityId\":" + externalActivityID + "," +
                                      "\"WorkFlowID\":" + workflowID + "," +
                                      "\"ActivatingInstanceId\":" + activatingInstanceID + "," +
                                      "\"CorrelationId\":\"" + Guid.NewGuid() + "\"," +
                                      "\"ServiceRequestMetadata\":{" + serviceRequestMetadata + "}," +
                                      "\"MapVariables\":{" + mapVariables + "}," +
                                      "\"RequestType\":" + 32 + "" +
                                     "}";

                // create a web request & call the 'SendRequest' method on the SAHL.X2Engine2.WebHost
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = requestContents.Length;
                request.ImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;

                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(requestContents);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                // get response from request
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();

                // lets examine the response and determine success
                using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    string responseText = streamReader.ReadToEnd();
                    //Now you have your response.
                    //or false depending on information in the response
                    success = 0;
                }
                webResponse.Close();
            }
            catch (WebException webException)
            {
                if (webException.Status == WebExceptionStatus.ProtocolError ||
                    webException.Status == WebExceptionStatus.ConnectFailure)
                {
                    using (SqlConnection connection = new SqlConnection("context connection=true"))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(@"insert into [x2].[x2].request (RequestID, Contents, RequestStatusID, RequestDate, RequestUpdatedDate, RequestTimeoutRetries) " +
                            "values(@RequestID, @Contents, @RequestStatusID, @RequestDate, @RequestUpdatedDate, @RequestTimeoutRetries)", connection);
                        command.Parameters.Add("RequestID", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                        command.Parameters.Add("Contents", SqlDbType.VarChar).Value = requestContents;
                        command.Parameters.Add("RequestStatusID", SqlDbType.Int).Value = 4;
                        command.Parameters.Add("RequestDate", SqlDbType.DateTime).Value = DateTime.Now;
                        command.Parameters.Add("RequestUpdatedDate", SqlDbType.DateTime).Value = DateTime.Now;
                        command.Parameters.Add("RequestTimeoutRetries", SqlDbType.Int).Value = 0;
                        command.ExecuteNonQuery();
                    }
                }

                LogError(webException, "SendExternalActivityWebRequest");
            }
            catch (Exception ex)
            {
                LogError(ex, "SendExternalActivityWebRequest");
                success = 0;
                SqlContext.Pipe.Send(ex.Message.ToString());
            }

            return success;
        }

        private static void LogError(Exception ex, string methodName)
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


                sqlConnection.Close();
            }
        }
    }
}