using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.DomainProcessManager.DomainProcesses.Managers.X2Workflow;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.FinanceDomain;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice
{
    public abstract class PayThirdPartyInvoiceProcessSpec : WithFakes
    {
        protected static PayAttorneyInvoiceProcess payAttorneyInvoiceDomainProcess;
        protected static IPayThirdPartyInvoiceStateMachine payThirdPartyInvoiceStateMachine;
        protected static IFinanceDomainServiceClient financeDomainService;
        protected static ICATSServiceClient catsService;
        protected static IX2WorkflowManager x2WorkflowManager;
        protected static ICommunicationManager communicationManager;
        protected static ILoggerSource loggerSource;
        protected static IRawLogger rawLogger;
        protected static ILoggerAppSource loggerAppSource;
        protected static ICombGuid combGuidGenerator;
        protected static IServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            financeDomainService = Substitute.For<IFinanceDomainServiceClient>();
            payThirdPartyInvoiceStateMachine = Substitute.For<IPayThirdPartyInvoiceStateMachine>();
            catsService = Substitute.For<ICATSServiceClient>();
            x2WorkflowManager = Substitute.For<IX2WorkflowManager>();
            communicationManager = Substitute.For<ICommunicationManager>();
            loggerSource = Substitute.For<ILoggerSource>();
            rawLogger = Substitute.For<IRawLogger>();
            loggerAppSource = Substitute.For<ILoggerAppSource>();
            combGuidGenerator = Substitute.For<ICombGuid>();
            metadata = Substitute.For<IServiceRequestMetadata>();

            int currentGuidIndex = 0;
            combGuidGenerator.WhenToldTo(y => y.Generate()).Return(() =>
            {
                var guid = GetNewGuid().ToList()[currentGuidIndex];
                currentGuidIndex++;
                return guid;
            });
        };

        private static IEnumerable<Guid> GetNewGuid()
        {
            for (int i = 0; i <= 100; i++)
            {
                yield return Guid.NewGuid();
            }
        }

        private Cleanup cleanupAfter = () =>
        {
            financeDomainService = null;
            payAttorneyInvoiceDomainProcess = null;
            catsService = null;
            x2WorkflowManager = null;
            communicationManager = null;
            loggerSource = null;
            rawLogger = null;
            loggerAppSource = null;
            combGuidGenerator = null;
            metadata = null;
        };
    }
}