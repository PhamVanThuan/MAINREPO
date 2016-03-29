using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SAHL.X2Engine2.CommandHandlers.Decorators
{
    [DecorationOrder(2)]
    public class X2MetricCommandHandlerDecorator<TCommand> : IServiceCommandHandlerDecorator<TCommand> where TCommand : IServiceCommand
    {
        private IServiceCommandHandler<TCommand> innerHandler;
        private IRawMetricsLogger metrics;
        private IRawLogger logger;
        private ILoggerSource loggerSource;
        private ILoggerAppSource loggerAppSource;

        public IServiceCommandHandler<TCommand> InnerCommandHandler
        {
            get { return this.innerHandler; }
        }

        public X2MetricCommandHandlerDecorator(IServiceCommandHandler<TCommand> innerHandler, IRawMetricsLogger metrics, IRawLogger logger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
        {
            this.innerHandler = innerHandler;
            this.metrics = metrics;
            this.logger = logger;
            this.loggerSource = loggerSource;
            this.loggerAppSource = loggerAppSource;
        }

        public ISystemMessageCollection HandleCommand(TCommand command, IServiceRequestMetadata metadata)
        {
            string metricName = command.GetType().FullName;
            var startTime = DateTime.Now;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var messages = new SystemMessageCollection();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(SAHL.Core.Logging.Logger.CorrelationId, command.Id);
            parameters.Add(SAHL.Core.Logging.Logger.ThreadId, Thread.CurrentThread.ManagedThreadId);
            try
            {
                messages.Aggregate(innerHandler.HandleCommand(command, metadata));
            }
            catch (MapReturnedFalseException)
            {
                // we dont want to log these exceptions so do nothing
                throw;
            }
            catch (Exception ex)
            {
                logger.LogErrorWithException(this.loggerSource.LogLevel, this.loggerSource.Name, this.loggerAppSource.ApplicationName, "X2User", metricName, ex.Message, ex, parameters);
                throw;
            }
            stopWatch.Stop();
            metrics.GetThreadLocalStore()[SAHL.Core.Logging.Logger.CorrelationId] = command.Id;
            metrics.GetThreadLocalStore()[SAHL.Core.Logging.Logger.ThreadId] = Thread.CurrentThread.ManagedThreadId;
            metrics.LogLatencyMetric("", "X2Node", "X2User", metricName, startTime, new TimeSpan(stopWatch.ElapsedTicks), parameters);
            return messages;
        }
    }
}