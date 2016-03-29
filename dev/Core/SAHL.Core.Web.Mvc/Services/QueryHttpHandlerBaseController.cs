using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Services.Metrics;
using SAHL.Core.SystemMessages;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using System.Web.Http;

namespace SAHL.Core.Web.Services
{
    public class QueryHttpHandlerBaseController : ApiController
    {
        private readonly IServiceQueryRouter serviceQueryRouter;
        private readonly ICommandServiceRequestMetrics commandServiceMetrics;
        private readonly IHttpCommandAuthoriser authoriser;
        private readonly IJsonActivator jsonQueryActivator;
        private readonly IQueryParameterManager queryParameterManager;
        private readonly ILogger logger;
        private readonly ILoggerSource loggerSource;

        public QueryHttpHandlerBaseController(IServiceQueryRouter serviceQueryRouter, ICommandServiceRequestMetrics commandServiceMetrics, 
                                              IHttpCommandAuthoriser authoriser, IJsonActivator jsonQueryActivator, IQueryParameterManager queryParameterManager, 
                                              ILogger logger, ILoggerSource loggerSource)
        {
            this.serviceQueryRouter    = serviceQueryRouter;
            this.commandServiceMetrics = commandServiceMetrics;
            this.authoriser            = authoriser;
            this.jsonQueryActivator    = jsonQueryActivator;
            this.queryParameterManager = queryParameterManager;
            this.logger                = logger;
            this.loggerSource          = loggerSource;
        }

        [HttpPost]
        public async Task<dynamic> PerformHttpQuery(int? pageSize = null, int? currentPage = null, string filterOn = null, string filterValue = null, 
                                                    string orderBy = null, string sortDirection = null)
        {
            ServiceQueryResult result = new ServiceQueryResult();
            var returnCode = HttpStatusCode.OK;
            var userName = this.RequestContext.Principal != null ? this.RequestContext.Principal.Identity.Name : "Unknown";
            try
            {
                var json = Request.Content.ReadAsStringAsync().Result;
                var serviceCommand = jsonQueryActivator.DeserializeQuery(json);

                if (serviceCommand == null)
                {
                    this.logger.LogErrorWithException(this.loggerSource, userName, "PerformHttpQuery", "Could not get command through deserialisation", null);
                    this.logger.LogErrorWithException(this.loggerSource, userName, "PerformHttpQuery", "falied json string: " + json, null);
                    returnCode = HttpStatusCode.InternalServerError;
                }

                HttpCommandAuthoriser.AuthToken authToken = this.authoriser.AuthoriseCommand(serviceCommand);

                if (authToken.IsAuthorised())
                {
                    returnCode = PerformAuthorisedQuery(pageSize, currentPage, filterOn, filterValue, orderBy, sortDirection, serviceCommand, result);
                }
                else if (!authToken.Authenticated)
                {
                    result.SystemMessages.AddMessage(new SystemMessage("User authentication required.", SystemMessageSeverityEnum.Error));
                    returnCode = HttpStatusCode.Unauthorized;
                }
                else if (!authToken.Authorised)
                {
                    result.SystemMessages.AddMessage(new SystemMessage("User has insufficient privilege to perform requested operation.", SystemMessageSeverityEnum.Error));
                    returnCode = HttpStatusCode.MethodNotAllowed;
                }
            }
            catch (Exception ex)
            {
                result.SystemMessages.AddMessage(new SystemMessage(ex.Message, SystemMessageSeverityEnum.Exception));
                result.SystemMessages.AddMessage(new SystemMessage("An internal server error has occurred", SystemMessageSeverityEnum.Error));
                returnCode = HttpStatusCode.InternalServerError;
                this.logger.LogErrorWithException(this.loggerSource, userName, "PerformHttpQuery", "Exception while performing an http query", ex);
            }
            return CreateResponse(result, returnCode);
        }

        private HttpStatusCode PerformAuthorisedQuery(int? pageSize, int? currentPage, string filterOn, string filterValue, 
            string orderBy, string sortDirection, IServiceQuery serviceCommand, ServiceQueryResult result)
        {
            SetPaginationParameters(pageSize, currentPage);

            SetFilterParameters(filterOn, filterValue);

            SetSortParameters(orderBy, sortDirection);

            InternalQuery(serviceCommand as dynamic, result);

            return result.SystemMessages.HasErrors ? HttpStatusCode.InternalServerError : HttpStatusCode.OK;
        }

        private void SetSortParameters(string orderBy, string sortDirection)
        {
            if (!string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(sortDirection))
            {
                queryParameterManager.SetParameter<SortQueryParameter>(x =>
                {
                    x.OrderBy = orderBy;
                    x.SortDirection = ((SortQueryParameter.SortDirectionOptions) Enum.Parse(typeof (SortQueryParameter.SortDirectionOptions), sortDirection, true));
                });
            }
        }

        private void SetFilterParameters(string filterOn, string filterValue)
        {
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterValue))
            {
                queryParameterManager.SetParameter<FilterQueryParameter>(x =>
                {
                    x.FilterOn = filterOn;
                    x.FilterValue = filterValue;
                });
            }
        }

        private void SetPaginationParameters(int? pageSize, int? currentPage)
        {
            if (pageSize.HasValue && currentPage.HasValue &&
                pageSize.Value > 0 && currentPage.Value >= 0)
            {
                queryParameterManager.SetParameter<PaginationQueryParameter>(x =>
                {
                    x.PageSize = pageSize.Value;
                    x.CurrentPage = currentPage.Value;
                });
            }
        }

        private void InternalQuery<T>(T serviceQuery, ServiceQueryResult result) where T : IServiceQuery
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            commandServiceMetrics.IncrementRequestForCommand<T>("QueryHttpHandleCommand");

            var userName = this.RequestContext.Principal != null ? this.RequestContext.Principal.Identity.Name : "Unknown";
            this.logger.LogMethodMetric(this.loggerSource, userName, "PerformHttpQuery", () =>
            {
                result.SystemMessages.Aggregate(this.serviceQueryRouter.HandleQuery((object)serviceQuery));
            });

            dynamic serviceCommandDynamic = serviceQuery;
            if (serviceCommandDynamic.Result != null)
            {
                result.SetReturnData(serviceCommandDynamic.Result);
            }

            stopWatch.Stop();
            commandServiceMetrics.UpdateRequestLatencyForCommand<T>("QueryHttpHandleCommand", stopWatch.ElapsedMilliseconds);
        }

        private HttpResponseMessage CreateResponse(ServiceQueryResult result, System.Net.HttpStatusCode returnCode)
        {
            ITraceWriter traceWriter = new DiagnosticsTraceWriter { LevelFilter = TraceLevel.Verbose };

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
                TraceWriter = traceWriter,
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