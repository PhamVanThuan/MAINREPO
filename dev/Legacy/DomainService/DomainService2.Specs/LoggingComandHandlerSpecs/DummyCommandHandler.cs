using DomainService2.Specs.TransactionalCommandHandlerSpecs;
using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Specs.LoggingCommandHandlerSpecs
{
    public class DummyCommandHandler : IHandlesDomainServiceCommand<DummyCommand>
    {
        public DummyCommandHandler()
        {

        }
        public void Handle(IDomainMessageCollection messages, DummyCommand command)
        {
            
        }
    }

    public class DummyCommandHandlerThatThrowsException : IHandlesDomainServiceCommand<DummyCommand>
    {
        public DummyCommandHandlerThatThrowsException()
        {

        }
        public void Handle(IDomainMessageCollection messages, DummyCommand command)
        {
            throw new Exception();
        }
    }

}
