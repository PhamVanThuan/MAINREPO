using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services.CommandPersistence;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;

namespace SAHL.Core.Web.Services
{
    public class CommandHttpHandlerBaseController : ApiController //, IStartable
    {
        private readonly ILogger logger;
        private readonly ILoggerSource loggerSource;
        private readonly ICommandSessionFactory commandSessionFactory;
        private readonly IHttpCommandRun httpCommandRun;
        private readonly IJsonActivator jsonActivator;
        private readonly IHostContext hostContext;

        public CommandHttpHandlerBaseController(ILogger logger, ILoggerSource loggerSource, ICommandSessionFactory commandSessionFactory, 
                                                IHttpCommandRun httpCommandRun, IJsonActivator jsonActivator, IHostContext hostContext)
        {
            this.logger                = logger;
            this.loggerSource          = loggerSource;
            this.commandSessionFactory = commandSessionFactory;
            this.httpCommandRun        = httpCommandRun;
            this.jsonActivator         = jsonActivator;
            this.hostContext           = hostContext;
        }

        /// <summary>
        /// get meta data from headers/ if not exist then get assume actual user
        /// pass to svccommandrouter
        /// router responsable for storing before and removing after handling
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<dynamic> PerformHttpCommand()
        {
            ServiceCommandResult result = new ServiceCommandResult();
            HttpStatusCode returnCode;
            IPrincipal userPrincipal = hostContext.GetUser();
            string runAsUsername = userPrincipal != null ? userPrincipal.Identity.Name : "Unknown";
            try
            {
                var json = Request.Content.ReadAsStringAsync().Result;
                var headerDetails = GetHeaderDetailsFromContext(hostContext);
                var contextDetails = new ContextDetails
                {
                    Username = runAsUsername,
                    ContextValues = headerDetails
                };

                ICommandSession commandSession = commandSessionFactory.CreateNewCommandManager(json);

                string serialisedContextDetails = jsonActivator.SerializeObject(contextDetails);

                commandSession.PersistCommand(serialisedContextDetails);
                returnCode = httpCommandRun.RunCommand(result, commandSession);
            }
            catch (Exception ex)
            {
                result.SystemMessages.AddMessage(new SystemMessage(ex.Message, SystemMessageSeverityEnum.Exception));
                result.SystemMessages.AddMessage(new SystemMessage("An internal server error has occurred", SystemMessageSeverityEnum.Error));
                returnCode = HttpStatusCode.InternalServerError;
                this.logger.LogErrorWithException(this.loggerSource, runAsUsername, "PerformHttpCommand", "Exception while performing an http command", ex);
            }

            return CreateResponse(result, returnCode);
        }

        private List<KeyValuePair<string, string>> GetHeaderDetailsFromContext(IHostContext hostContext)
        {
            string[] keys = hostContext.GetKeys();
            return keys.Select(key =>
            {
                string contextValue = hostContext.GetContextValue(key, "");
                return new KeyValuePair<string, string>(key, contextValue);
            }).ToList();
        }

        protected virtual string GetUsername(IHostContext context)
        {
            var username = "Unknown";
            if (context == null)
            {
                return username;
            }
            var principal = context.GetUser();
            if (principal != null && principal.Identity != null)
            {
                username = principal.Identity.Name;
            }
            return username;
        }

        private HttpResponseMessage CreateResponse(ServiceCommandResult result, HttpStatusCode returnCode)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
                PreserveReferencesHandling = PreserveReferencesHandling.Arrays,
                ContractResolver = new DefaultContractResolver
                {
                    DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.Instance
                },
                Error = (error, eventargs) =>
                {
                    var userName = this.RequestContext.Principal != null ? this.RequestContext.Principal.Identity.Name : "Unknown";
                    Exception ex = eventargs.ErrorContext.Error;
                    this.logger.LogErrorWithException(this.loggerSource, userName, "CreateResponse", "Serialisation Error", ex);
                }
            };

            var response = Request.CreateResponse(returnCode, result, new JsonMediaTypeFormatter { SerializerSettings = settings });
            return response;
        }
    }
}