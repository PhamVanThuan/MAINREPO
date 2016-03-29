using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using DomainService2.Workflow.Origination.Valuations;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.CleanupChildCases
{
    public class CleanupChildCasesCommandHandlerBase : WithFakes
    {
        protected static CleanupChildCasesCommand command;
        protected static CleanupChildCasesCommandHandler handler;
        protected static IDomainMessageCollection messages;
    }
}
