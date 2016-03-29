namespace SAHLDomainService
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.ServiceProcess;
    using DomainService2;
    using DomainService2.SharedServices;
    using Extensions;
    using Ninject;
    using SAHL.Common.BusinessModel.Config;
    using SAHL.Common.BusinessModel.Configuration;
    using SAHL.Common.BusinessModel.Interfaces.Configuration;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.BusinessModel.Interfaces.Service;
    using SAHL.Common.BusinessModel.Repositories;
    using SAHL.Common.BusinessModel.Service;
    using SAHL.Common.Service;
    using SAHL.Common.Service.Interfaces;
    using X2DomainService.Interface.Common;

    public partial class SAHLDomainServiceClass : ServiceBase
    {
        private DomainService2.IDomainService svc = null;
        private IDomainServiceIOC ioc = null;

        public SAHLDomainServiceClass()
        {
            InitializeLogging();
            CreateEventLog();
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            EventLog eLog = new EventLog();
            eLog.Source = "Domain Service";
            eLog.WriteEntry("Error:" + ex.Message + "|Stack Trace:" + ex.StackTrace, EventLogEntryType.Error);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                this.ioc = InitialiseIOC();
                svc = this.ioc.Get<IDomainService>();
                svc.OnStart();
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }

        protected override void OnStop()
        {
            try
            {
                svc.OnStop();
                svc = null;
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }

        private IDomainServiceIOC InitialiseIOC()
        {
            IKernel kernel = new StandardKernel();

            // Register an activerecord configurationprovider
            kernel.Bind<IActiveRecordConfigurationProvider>().To<ActiveRecordConfigFileConfigurationProvider>();

            // Register an activerecord initialiser
            kernel.Bind<IActiveRecordInitialiser>().To<ActiveRecordInitialiser>();

            // Register a remotingserviceshelper
            kernel.Bind<IRemotingServicesHelper>().To<RemotingServicesHelper>();

            // register a remotingserviceconfigurationprovider
            kernel.Bind<IRemotingServiceConfigurationProvider>().To<RemotingServiceConfigFileConfigurationProvider>();

            // register a remotingserviceconfigurator
            kernel.Bind<IRemotingServiceConfigurator>().To<RemotingServiceConfigurator>();

            // register all the IRemotingServices
            kernel.BindAllRemotingHosts();

            // register all the command handlers and decorators
            kernel.BindAllCommandHandlersAndDecorateWith(typeof(TransactionalCommandHandler<>), typeof(LoggingCommandHandler<>));

            // register a domain service
            kernel.Bind<IDomainService>().To<DomainService2.DomainService>();

            // register an IDomainServiceIOC and use a lamda to return a specific instance
            kernel.Bind<IDomainServiceIOC>().ToMethod(x => new NinjectDomainIOC(kernel));

            // register all the framework services
            //////////////////////////////////////

            // Rules Service
            kernel.Bind<IRuleService>().To<RuleService>();

            // ICastleTransactionsService
            kernel.Bind<ICastleTransactionsService>().To<CastleTransactionsService>();

            // ApplicationRepository
            kernel.Bind<IApplicationReadOnlyRepository>().To<ApplicationReadOnlyRepository>();
            kernel.Bind<IApplicationRepository>().To<ApplicationRepository>();

            // IPropertyRepository
            kernel.Bind<IPropertyRepository>().To<PropertyRepository>();

            // IOrganisationStructureRepository
            kernel.Bind<IOrganisationStructureRepository>().To<OrganisationStructureRepository>();

            // IX2Repository
            kernel.Bind<IX2Repository>().To<X2Repository>();

            // ILookupRepository
            kernel.Bind<ILookupRepository>().To<LookupRepository>();

            // ICommonRepository
            kernel.Bind<ICommonRepository>().To<CommonRepository>();

            // IReasonRepository
            kernel.Bind<IReasonRepository>().To<ReasonRepository>();

            // IStageDefinitionRepository
            kernel.Bind<IStageDefinitionRepository>().To<StageDefinitionRepository>();

            // ILifeRepository
            kernel.Bind<ILifeRepository>().To<LifeRepository>();

            // IAccountRepository
            kernel.Bind<IAccountReadOnlyRepository>().To<AccountReadOnlyRepository>();
            kernel.Bind<IAccountRepository>().To<AccountRepository>();

            // ICorrespondenceRepository
            kernel.Bind<ICorrespondenceRepository>().To<CorrespondenceRepository>();

            // IReportRepository
            kernel.Bind<IReportRepository>().To<ReportRepository>();

            // IRuleRepository
            kernel.Bind<IRuleRepository>().To<RuleRepository>();

            // IFinancialServiceRepository
            kernel.Bind<IFinancialServiceRepository>().To<FinancialServiceRepository>();

            // IDebtCounsellingRepository
            kernel.Bind<IDebtCounsellingRepository>().To<DebtCounsellingRepository>();

            // IControlRepository
            kernel.Bind<IControlRepository>().To<ControlRepository>();

            // IManualDebitOrderRepository
            kernel.Bind<IManualDebitOrderRepository>().To<ManualDebitOrderRepository>();

            // return the domain service ioc
            return kernel.Get<IDomainServiceIOC>();
        }

        private void CreateEventLog()
        {
            string sSource = "Domain Service";
            string sLog = "Application";

            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);
        }

        internal void InitializeLogging()
        {
            log4net.GlobalContext.Properties["LogName"] = MethodBase.GetCurrentMethod().DeclaringType.Name;
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}