using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.DomainProcess;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.DomainProcessManager.DomainProcesses.Managers.X2Workflow;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.DomainProcessManager.DomainProcesses.Managers.Client;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.BankAccountDomain;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.FinancialDomain;
using SAHL.Services.Interfaces.PropertyDomain;

namespace SAHL.DomainProcessManager.DomainProcesses.Tests
{
    [TestFixture]
    public class TestSwitchApplicationCreationDomainProcess
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var domainProcess = this.CreateDomainProcess();
            //---------------Test Result -----------------------
            Assert.IsNotNull(domainProcess);
        }

        [Test]
        public void Start_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var creationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.SwitchLoan) as SwitchApplicationCreationModel;
            var domainMapper = new DomainModelMapper();
            domainMapper.CreateMap<SwitchApplicationCreationModel, SwitchApplicationModel>();
            SwitchApplicationModel switchApplicationModel = domainMapper.Map(creationModel);

            var applicationCreatedEvent = new SwitchApplicationAddedEvent(new DateTime(2014, 01, 01),
                1234,
                switchApplicationModel.ApplicationType,
                switchApplicationModel.ApplicationStatus,
                switchApplicationModel.ApplicationSourceKey,
                switchApplicationModel.OriginationSource,
                switchApplicationModel.ExistingLoan,
                switchApplicationModel.EstimatedPropertyValue,
                switchApplicationModel.Term,
                switchApplicationModel.CashOut,
                switchApplicationModel.Product);

            var domainProcess = this.CreateDomainProcess();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var startTask = domainProcess.Start(creationModel, typeof(SwitchApplicationAddedEvent).Name);
            var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid());
            serviceRequestMetadata["AddSwitchApplicationCommandCorrelationId"] = Guid.NewGuid().ToString();

            domainProcess.HandleEvent(applicationCreatedEvent, serviceRequestMetadata);
            IDomainProcessStartResult startResult = null;
            Assert.DoesNotThrow(() => startResult = startTask.Result);
            //---------------Test Result -----------------------
            Assert.IsTrue(startResult.Result);
        }

        [Test]
        public void Handle_GivenBasicApplicationCreatedEvent_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            const int applicationNumber = 294893;
            var creationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.SwitchLoan) as SwitchApplicationCreationModel;
            var domainMapper = new DomainModelMapper();
            domainMapper.CreateMap<SwitchApplicationCreationModel, SwitchApplicationModel>();
            var switchApplicationModel = domainMapper.Map(creationModel);

            var domainProcess = this.CreateDomainProcess();
            domainProcess.Start(creationModel, typeof(SwitchApplicationAddedEvent).Name);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid());
            serviceRequestMetadata["AddSwitchApplicationCommandCorrelationId"] = Guid.NewGuid().ToString();
            Assert.DoesNotThrow(() => domainProcess.HandleEvent(new SwitchApplicationAddedEvent(new DateTime(2014, 01, 01),
                applicationNumber,
                switchApplicationModel.ApplicationType,
                switchApplicationModel.ApplicationStatus,
                switchApplicationModel.ApplicationSourceKey,
                switchApplicationModel.OriginationSource,
                switchApplicationModel.ExistingLoan,
                switchApplicationModel.EstimatedPropertyValue,
                switchApplicationModel.Term,
                switchApplicationModel.CashOut,
                switchApplicationModel.Product),
                serviceRequestMetadata));
            //---------------Test Result -----------------------
        }

        private SwitchApplicationDomainProcess CreateDomainProcess()
        {
            var applicationDomainService = MockRepository.GenerateMock<IApplicationDomainServiceClient>();
            var clientDomainService = MockRepository.GenerateMock<IClientDomainServiceClient>();
            var addressDomainService = MockRepository.GenerateMock<IAddressDomainServiceClient>();
            var financialDomainService = MockRepository.GenerateMock<IFinancialDomainServiceClient>();
            var bankAccountDomainService = MockRepository.GenerateMock<IBankAccountDomainServiceClient>();
            var combGuid = MockRepository.GenerateMock<ICombGuid>();
            var applicationStateMachine = MockRepository.GenerateMock<IApplicationStateMachine>();
            var clientDataManager = MockRepository.GenerateMock<IClientDataManager>();
            var x2WorkflowManager = MockRepository.GenerateMock<IX2WorkflowManager>();
            var linkedKeyManager = MockRepository.GenerateMock<ILinkedKeyManager>();
            var propertyDomainService = MockRepository.GenerateMock<IPropertyDomainServiceClient>();
            var communicationManager = MockRepository.GenerateMock<ICommunicationManager>();
            var applicationDataManager = MockRepository.GenerateMock<IApplicationDataManager>();
            var domainRuleManager = MockRepository.GenerateMock<IDomainRuleManager<ApplicationCreationModel>>();
            var rawLogger = NSubstitute.Substitute.For<IRawLogger>();
            var loggerSource = NSubstitute.Substitute.For<ILoggerSource>();
            var loggerAppSource = NSubstitute.Substitute.For<ILoggerAppSource>();

            var domainProcess = new SwitchApplicationDomainProcess(
                applicationStateMachine,
                applicationDomainService,
                clientDomainService,
                addressDomainService,
                financialDomainService,
                bankAccountDomainService,
                combGuid,
                clientDataManager,
                x2WorkflowManager,
                linkedKeyManager,
                propertyDomainService,
                communicationManager,
                applicationDataManager,
                domainRuleManager,
                rawLogger,
                loggerSource,
                loggerAppSource);

            applicationDataManager.Expect(adm => adm.DoesSuppliedVendorExist(Arg<string>.Is.Anything)).Return(true);

            applicationStateMachine.Expect(a => a.SystemMessages).Return(SystemMessageCollection.Empty());
            applicationDomainService.Expect(
                a => a.PerformCommand(Arg<AddSwitchApplicationCommand>.Is.Anything, Arg<IServiceRequestMetadata>.Is.Anything))
                .Return(SystemMessageCollection.Empty());
            clientDomainService.Expect(a => a.PerformCommand(Arg<AddNaturalPersonClientCommand>.Is.Anything, Arg<IServiceRequestMetadata>.Is.Anything))
                .Return(SystemMessageCollection.Empty());
            clientDomainService.Expect(
                a => a.PerformCommand(Arg<AddMarketingOptionsToClientCommand>.Is.Anything, Arg<IServiceRequestMetadata>.Is.Anything))
                .Return(SystemMessageCollection.Empty());
            clientDomainService.Expect(a => a.PerformCommand(Arg<AddEmployerCommand>.Is.Anything, Arg<IServiceRequestMetadata>.Is.Anything))
                .Return(SystemMessageCollection.Empty());
            clientDomainService.Expect(a => a.PerformQuery(Arg<FindClientByIdNumberQuery>.Is.Anything)).WhenCalled(call =>
            {
                FindClientByIdNumberQuery query = call.Arguments[0] as FindClientByIdNumberQuery;
                query.Result = new ServiceQueryResult<ClientDetailsQueryResult>(new List<ClientDetailsQueryResult>());
            }).Return(SystemMessageCollection.Empty());

            clientDomainService.Expect(a => a.PerformQuery(Arg<FindClientByPassportNumberQuery>.Is.Anything)).WhenCalled(call =>
            {
                FindClientByPassportNumberQuery query = call.Arguments[0] as FindClientByPassportNumberQuery;
                query.Result = new ServiceQueryResult<ClientDetailsQueryResult>(new List<ClientDetailsQueryResult>());
            }).Return(SystemMessageCollection.Empty());

            return domainProcess;
        }
    }
}
