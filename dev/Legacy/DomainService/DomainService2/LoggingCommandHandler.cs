namespace DomainService2
{
    using System;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Logging;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;

    public class LoggingCommandHandler<T> : IHandlesDomainServiceCommandDecorator<T> where T : IDomainServiceCommand
    {
        private IHandlesDomainServiceCommand<T> innerHandler;
        private ILogger logger;

        public LoggingCommandHandler(IHandlesDomainServiceCommand<T> innerHandler, ILogger logger)
        {
            this.innerHandler = innerHandler;
            this.logger = logger;
        }

        public IHandlesDomainServiceCommand<T> InnerHandler
        {
            get { return this.innerHandler; }
        }



        public void Handle(IDomainMessageCollection messages, T command)
        {
            string methodName = string.Format("Handle: {0}", command.GetType().FullName);
            Dictionary<string, object> methodParams = new Dictionary<string, object>();
            methodParams.Add("command", command);
            try
            {
                // log onenter
                logger.LogOnEnterMethod(methodName, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParams } });

                // call the inner handler
                this.innerHandler.Handle(messages, command);

                // log onsuccess
                logger.LogOnMethodSuccess(methodName, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParams } });
            }
            catch (Exception ex)
            {
                // logging on exception
                logger.LogOnMethodException(methodName, ex, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParams } });
                throw;
            }
            finally
            {
                // log on exit
                logger.LogOnExitMethod(methodName, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParams } });
            }
        }
    }
}