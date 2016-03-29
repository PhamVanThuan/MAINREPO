using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using DomainService2.Workflow.Origination.Valuations;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.SetValuationStatusReturned
{
    public class SetValuationStatusReturnedCommandHandlerBase : WithFakes
    {
        protected static SetValuationStatusReturnedCommand command;
        protected static SetValuationStatusReturnedCommandHandler handler;
        protected static IDomainMessageCollection messages;
    }
}
