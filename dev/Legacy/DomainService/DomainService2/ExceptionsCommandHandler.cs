namespace DomainService2
{
    using System;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.DomainMessages;
    using SAHL.Common.Exceptions;

    public class ExceptionsCommandHandler<T> : IHandlesDomainServiceCommandDecorator<T> where T : IDomainServiceCommand
    {
        private IHandlesDomainServiceCommand<T> innerHandler;

        public ExceptionsCommandHandler(IHandlesDomainServiceCommand<T> innerHandler)
        {
            this.innerHandler = innerHandler;
        }

        public IHandlesDomainServiceCommand<T> InnerHandler
        {
            get { return this.innerHandler; }
        }

        public void Handle(IDomainMessageCollection messages, T command)
        {
            try
            {
                this.innerHandler.Handle(messages, command);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}