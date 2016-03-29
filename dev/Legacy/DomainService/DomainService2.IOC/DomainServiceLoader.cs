using DomainService2.IOC.Extensions;
using DomainService2.SharedServices;
using DomainService2.V3.Client;
using EWorkConnector;
using Ninject;
using SAHL.Common.BusinessModel.Config;
using SAHL.Common.BusinessModel.Configuration;
using SAHL.Common.BusinessModel.Interfaces.Configuration;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.DataAccess;
using SAHL.Common.Logging;
using SAHL.Common.Security;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Communication;

namespace DomainService2.IOC
{
    public class DomainServiceLoader : IDomainServiceLoader
    {
        private IDomainServiceIOC ioc = null;

        public DomainServiceLoader()
            : this(false)
        {
        }

        public DomainServiceLoader(bool initialiseActiveRecord)
        {
            // setup the IOC
            this.ioc = InitialiseIOC();

            if (initialiseActiveRecord)
            {
                // Initialise ActiveRecord (do this once per process only)
                IActiveRecordInitialiser arInit = this.ioc.Get<IActiveRecordInitialiser>();
                arInit.InitialiseActiveRecord();
            }

            LogPlugin.Logger = this.ioc.Get<ILogger>();
            MetricsPlugin.Metrics = this.ioc.Get<IMetrics>();
        }

        # region Static Visibility stuff
        static IDomainServiceLoader instance;
        static readonly object instanceLock = new object();

        public static string ProcessName { get; set; }

        public static IDomainServiceLoader Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new DomainServiceLoader(true);
                    }
                    return instance;
                }
            }
            set
            {
                lock (instanceLock)
                {
                    instance = value;
                }
            }
        }

        #endregion

        T IDomainServiceLoader.Get<T>()
        {
            return this.ioc.Get<T>();
        }

        public IDomainServiceIOC DomainServiceIOC
        {
            get
            {
                return this.ioc;
            }
        }

        private IDomainServiceIOC InitialiseIOC()
        {
            IKernel kernel = new StandardKernel();
            kernel.Load(new[] { "SAHL.Config.Legacy.dll" });
            // Register an activerecord configurationprovider
            kernel.Bind<IActiveRecordConfigurationProvider>().To<ActiveRecordConfigFileConfigurationProvider>();

            // Register an activerecord initialiser
            kernel.Bind<IActiveRecordInitialiser>().To<ActiveRecordInitialiser>();

            // register all the IDomainHosts
            kernel.BindAllDomainHosts();

            // register all the command handlers and decorators
            kernel.BindAllCommandHandlersAndDecorateWith(typeof(ExceptionsCommandHandler<>), typeof(LoggingCommandHandler<>), typeof(TransactionalCommandHandler<>));

            // register an IDomainServiceIOC and use a lamda to return a specific instance
            kernel.Bind<IDomainServiceIOC>().ToMethod(x => new NinjectDomainIOC(kernel));

            // register all the framework services
            //////////////////////////////////////

            
            kernel.Bind<MessageBusLoggerConfiguration>().ToSelf();
            kernel.Bind<ILogger>().To<SafeLoggingWrapper>();
            kernel.Bind<ILogger>().To<MessageBusLogger>().WhenInjectedInto<SafeLoggingWrapper>();
			kernel.Bind<IMessageBusConfigurationProvider>().To<X2EasyNetQMessageBusConfigurationProvider>()
                .OnActivation(x =>
                    {
                        ((X2EasyNetQMessageBusConfigurationProvider)x).X2ProcessName = ProcessName;
                    });
            kernel.Bind<IMessageBus>().To<EasyNetQMessageBus>().InSingletonScope();
            kernel.Bind<MessageBusMetricsConfiguration>().ToSelf();
            kernel.Bind<IMetrics>().To<SafeMetricsWrapper>();
            kernel.Bind<IMetrics>().To<MessageBusMetrics>().WhenInjectedInto<SafeMetricsWrapper>();



            // ITransactionManager
            kernel.Bind<ITransactionManager>().To<ActiveRecordTransactionManager>();

            // Rules Service
            kernel.Bind<IRuleService>().To<RuleService>();

            // ICastleTransactionsService
            kernel.Bind<ICastleTransactionsService>().To<CastleTransactionsService>();

            // IUIStatementService
            kernel.Bind<IUIStatementService>().To<UIStatementService>();

            // ApplicationRepository
            kernel.Bind<IApplicationReadOnlyRepository>().To<ApplicationReadOnlyRepository>();
            kernel.Bind<IApplicationRepository>().To<ApplicationRepository>();

            // IPropertyRepository
            kernel.Bind<IPropertyRepository>().To<PropertyRepository>();

            // IOrganisationStructureRepository
            kernel.Bind<IOrganisationStructureRepository>().To<OrganisationStructureRepository>();

            // IX2Repository
            kernel.Bind<IX2Repository>().To<X2Repository>();

            // IX2Service
            kernel.Bind<IX2Service>().To<X2Service>();

            // ILookupRepository
            kernel.Bind<ILookupRepository>().To<LookupRepository>().InSingletonScope();

            // IRuleRepository
            kernel.Bind<IRuleRepository>().To<RuleRepository>();

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

            // IRulesServiceConfigurationProvider
            kernel.Bind<IRulesServiceConfigurationProvider>().To<RulesServiceConfigFileConfigurationProvider>();

            // IFinancialServiceRepository
            kernel.Bind<IFinancialServiceRepository>().To<FinancialServiceRepository>();

            // IDebtCounsellingRepository
            kernel.Bind<IDebtCounsellingRepository>().To<DebtCounsellingRepository>();

            // IControlRepository
            kernel.Bind<IControlRepository>().To<ControlRepository>();

            // IManualDebitOrderRepository
            kernel.Bind<IManualDebitOrderRepository>().To<ManualDebitOrderRepository>();

            // ICapRepository
            kernel.Bind<ICapRepository>().To<CapRepository>();

            // IMortgageLoanRepository
            kernel.Bind<IMortgageLoanRepository>().To<MortgageLoanRepository>();

            // ICreditScoringRepository
            kernel.Bind<ICreditScoringRepository>().To<CreditScoringRepository>();

            // IITCRepository
            kernel.Bind<IITCRepository>().To<ITCRepository>();

            // IWorkflowAssignmentRepository
            kernel.Bind<IWorkflowAssignmentRepository>().To<WorkflowAssignmentRepository>();

            //IWorkflowSecurityRepository
            kernel.Bind<IWorkflowSecurityRepository>().To<WorkflowSecurityRepository>().InSingletonScope();

            //IeWork
            kernel.Bind<IeWork>().To<eWork>();

            // ISAHLPrincipalCache
            kernel.Bind<ISAHLPrincipalCache>().To<SAHLPrincipalCache>();

            // ISAHLPrincipalCacheProvider
            kernel.Bind<ISAHLPrincipalCacheProvider>().To<SAHLPrincipalCacheProvider>();

            // ISAHLPrincipalProvider
            kernel.Bind<ISAHLPrincipalProvider>().To<SAHLPrincipalProvider>();

            // IX2Workflow Service
            kernel.Bind<IX2WorkflowService>().To<X2WorkflowService>();

            // IMemoRepository
            kernel.Bind<IMemoRepository>().To<MemoRepository>();

            // IMemoReadOnlyRepository
            kernel.Bind<IMemoReadOnlyRepository>().To<MemoReadOnlyRepository>();

            // ILegalEntityRepository
            kernel.Bind<ILegalEntityRepository>().To<LegalEntityRepository>();

            // IHelpDeskRepository
            kernel.Bind<IHelpDeskRepository>().To<HelpDeskRepository>();

            // IMessageService
            kernel.Bind<IMessageService>().To<MessagingService>();

            // ICommandHandler
            kernel.Bind<ICommandHandler>().To<CommandHandler>();

            // ISurvey Repository
            kernel.Bind<ISurveyRepository>().To<SurveyRepository>();

            // IDocumentCheckListService
            kernel.Bind<IDocumentCheckListService>().To<DocumentCheckListService>();

            // IRegistrationRepository
            kernel.Bind<IRegistrationRepository>().To<RegistrationRepository>();

            // IDisbursementRepository
            kernel.Bind<IDisbursementRepository>().To<DisbursementRepository>();

            // IConditionsRepository
            kernel.Bind<IConditionsRepository>().To<ConditionsRepository>();

			// ICreditMatrixRepository
			kernel.Bind<ICreditMatrixRepository>().To<CreditMatrixRepository>();

			// ICreditCriteriaRepository
			kernel.Bind<ICreditCriteriaRepository>().To<CreditCriteriaRepository>();

            // ILoanTransactionRepository
            kernel.Bind<ILoanTransactionRepository>().To<LoanTransactionRepository>();

            kernel.Bind<IWorkflowRoleAssignmentRepository>().To<WorkflowRoleAssignmentRepository>();
            
            kernel.Bind<IApplicationUnsecuredLendingRepository>().To<ApplicationUnsecuredLendingRepository>();

            kernel.Bind<ICreditCriteriaUnsecuredLendingRepository>().To<CreditCriteriaUnsecuredLendingRepository>();

			kernel.Bind<IDocumentCheckListRepository>().To<DocumentCheckListRepository>();

            //IApplicationDomainService
            kernel.Bind<IApplicationDomainService>().To<ApplicationDomainService>();

            kernel.Bind<IAffordabilityAssessmentRepository>().To<AffordabilityAssessmentRepository>();

            // return the domain service ioc
            return kernel.Get<IDomainServiceIOC>();
        }
    }
}