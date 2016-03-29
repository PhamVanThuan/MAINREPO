using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Configuration;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Configuration;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.Service
{
    internal class PerfTestObj
    {
        public string Name;
        public List<string> Messages = new List<string>();
        DateTime dtStart;
        string Message;
        long ticks = 0;

        internal PerfTestObj(string Name)
        {
            this.Name = Name;
        }

        public void Start(string Message)
        {
            dtStart = DateTime.Now;
            this.Message = Message;
        }

        public void Complete()
        {
            long t = DateTime.Now.Ticks - dtStart.Ticks;
            ticks += t;
            TimeSpan ts = new TimeSpan(t);
            Messages.Add(string.Format("{0}: [{1}:{2}]", Message, ts.Seconds, ts.Milliseconds));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            TimeSpan ts = new TimeSpan(ticks);
            sb.AppendFormat("{2}: [{0}:{1}]", ts.Seconds, ts.Milliseconds, Name);
            return sb.ToString();
        }
    }

    [FactoryType(typeof(IRuleService), LifeTime = FactoryTypeLifeTime.Singleton)]
    public class RuleService : IRuleService
    {
        private Dictionary<Type, object> dependencies;
        private bool _enabled = true;
        private List<int> _excludedRuleItems = new List<int>();
        private ILookupRepository _lookupRepo;
        private IRuleRepository ruleRepository;
        private ICastleTransactionsService castleTransactionsService;
        private ICreditCriteriaUnsecuredLendingRepository creditCriteriaUnsecuredLendingRepository;
        private IUIStatementService uiStatementService;
        private ICommonRepository commonRepository;
        private IApplicationRepository applicationRepository;
        private ILegalEntityRepository legalEntityRepository;
        private ICreditMatrixRepository creditMatrixRepository;
        private IReasonRepository reasonRepository;
        private IPropertyRepository propertyRepository;
        private IStageDefinitionRepository stageDefinitionRepository;
        private IAccountRepository accountRepository;
		private ICreditCriteriaRepository creditCriteriaRepository;
        private IDebtCounsellingRepository debtCounsellingRepository;
        private IX2Repository x2Repository;
        private Assembly _assembly;
        const string LogName = "Rules";

        static RuleService()
        {
            /*
            LogSection section = ConfigurationManager.GetSection("LogSection") as LogSection;
            LogSettingsClass lsl = new LogSettingsClass();
            lsl.AppName = "Rules";
            lsl.ConsoleLevel = Convert.ToInt32(section.Logging["Console"].level);
            lsl.ConsoleLevelLock = Convert.ToBoolean(section.Logging["Console"].Lock);
            lsl.FileLevel = Convert.ToInt32(section.Logging["File"].level);
            lsl.FileLevelLock = Convert.ToBoolean(section.Logging["File"].Lock);
            lsl.FilePath = section.Logging["File"].path;
            lsl.NumDaysToStore = 14;
            lsl.RollOverSizeInKB = 2048;
			LogPlugin.Logger.SeedLogSettings(lsl, LogName);
            */
        }

        /// <summary>
        ///
        /// </summary>
        public RuleService()
            : this(new RulesServiceConfigFileConfigurationProvider(),
                   RepositoryFactory.GetRepository<ILookupRepository>(),
                   RepositoryFactory.GetRepository<IRuleRepository>(),
                   new CastleTransactionsService(),
                   RepositoryFactory.GetRepository<ICreditCriteriaUnsecuredLendingRepository>(),
                   new UIStatementService(),
                    RepositoryFactory.GetRepository<ICommonRepository>(),
                    RepositoryFactory.GetRepository<IApplicationRepository>(),
                    RepositoryFactory.GetRepository<ILegalEntityRepository>(),
                    RepositoryFactory.GetRepository<ICreditMatrixRepository>(),
                    RepositoryFactory.GetRepository<IReasonRepository>(),
                    RepositoryFactory.GetRepository<IPropertyRepository>(),
                    RepositoryFactory.GetRepository<IStageDefinitionRepository>(),
                    RepositoryFactory.GetRepository<IAccountRepository>(),
					RepositoryFactory.GetRepository<ICreditCriteriaRepository>(),
            RepositoryFactory.GetRepository<IDebtCounsellingRepository>(),
            RepositoryFactory.GetRepository<IX2Repository>())
        {
        }

        public RuleService(IRulesServiceConfigurationProvider rulesConfiguration,
                           ILookupRepository lookupRepository,
                           IRuleRepository ruleRepository,
                           ICastleTransactionsService castleTransactionsService,
                           ICreditCriteriaUnsecuredLendingRepository creditCriteriaUnsecuredLendingRepository,
                           IUIStatementService uiStatementService,
                           ICommonRepository commonRepository,
                           IApplicationRepository applicationRepository,
                           ILegalEntityRepository legalEntityRepository,
                           ICreditMatrixRepository creditMatrixRepository,
                           IReasonRepository reasonRepository,
                           IPropertyRepository propertyRepository,
                           IStageDefinitionRepository stageDefinitionRepository,
                           IAccountRepository accountRepository,
						   ICreditCriteriaRepository creditCriteriaRepository,
                           IDebtCounsellingRepository debtCounsellingRepository,
                            IX2Repository x2Repository
            )
        {
            this._lookupRepo = lookupRepository;
            this.ruleRepository = ruleRepository;
            this.castleTransactionsService = castleTransactionsService;
            this.creditCriteriaUnsecuredLendingRepository = creditCriteriaUnsecuredLendingRepository;
            this.uiStatementService = uiStatementService;
            this.commonRepository = commonRepository;
            this.applicationRepository = applicationRepository;
            this.legalEntityRepository = legalEntityRepository;
            this.creditMatrixRepository = creditMatrixRepository;
            this.reasonRepository = reasonRepository;
            this.propertyRepository = propertyRepository;
            this.stageDefinitionRepository = stageDefinitionRepository;
            this.accountRepository = accountRepository;
			this.creditCriteriaRepository = creditCriteriaRepository;
            this.debtCounsellingRepository = debtCounsellingRepository;
            this.x2Repository = x2Repository;

            _assembly = Assembly.Load("SAHL.Common.BusinessModel");
            _enabled = rulesConfiguration.Enabled;

            //register dependencies
            dependencies = new Dictionary<Type, object>();
            dependencies.Add(typeof(ICastleTransactionsService), castleTransactionsService);
            dependencies.Add(typeof(IUIStatementService), uiStatementService);
            dependencies.Add(typeof(ICreditCriteriaUnsecuredLendingRepository), creditCriteriaUnsecuredLendingRepository);
            dependencies.Add(typeof(ICommonRepository), commonRepository);
            dependencies.Add(typeof(IApplicationRepository), applicationRepository);
            dependencies.Add(typeof(ILegalEntityRepository), legalEntityRepository);
            dependencies.Add(typeof(ICreditMatrixRepository), creditMatrixRepository);
            dependencies.Add(typeof(IReasonRepository), reasonRepository);
            dependencies.Add(typeof(IPropertyRepository), propertyRepository);
            dependencies.Add(typeof(IStageDefinitionRepository), stageDefinitionRepository);
            dependencies.Add(typeof(IAccountRepository), accountRepository);
			dependencies.Add(typeof(ICreditCriteriaRepository), creditCriteriaRepository);
            dependencies.Add(typeof(IDebtCounsellingRepository), debtCounsellingRepository);
            dependencies.Add(typeof(IX2Repository), x2Repository);
        }

        /// <summary>
        /// Determines whether the rule service is actually running rules.  If the SAHLRules application config
        /// section has an attribute "Enabled=false", this will return false and the rules will not run.  This
        /// should only be the case during testing.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        /// <summary>
        /// Internal method to run a rule.  This will collect the rule from the database, ensure that it is
        /// still active, and then load it from the rules assembly and run it.
        /// </summary>
        /// <param name="spc"></param>
        /// <param name="dmc"></param>
        /// <param name="ruleName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private int InternalExecuteRule(SAHLPrincipalCache spc, IDomainMessageCollection dmc, string ruleName, params object[] parameters)
        {
            // make sure the rule actually exists
            if (!_lookupRepo.RuleItemsByName.ContainsKey(ruleName))
                throw new Exception(string.Format("Rule: {0} does not exist in the database", ruleName));

            IRuleItem rule = _lookupRepo.RuleItemsByName[ruleName];

            // first check - ensure the rule hasn't been excluded or is inactive
            if (_excludedRuleItems.Contains(rule.Key) || rule.GeneralStatus.Key == (int)GeneralStatuses.Inactive)
                return 1;

            // if the rule is a warning and the principal is currently ignoring warnings,
            // just exit out of here
            if (!rule.EnforceRule && spc.IgnoreWarnings)
                return 1;

            // the rule needs to be run, so run it!
            int result = 1;
            string createErrorMessage = string.Format("Unable to create rule {0}", rule.TypeName);

            Type type = _assembly.GetType(rule.TypeName);
            if (type == null)
                throw new Exception(createErrorMessage);

            IBusinessRule busrule = null;
            try
            {
                busrule = CreateRule(type);

                //busrule = Activator.CreateInstance(type) as IBusinessRule;
                if (busrule == null)
                    throw new Exception(createErrorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(createErrorMessage, ex);
            }
#if DEBUG
            PerfTestObj p = new PerfTestObj(ruleName);
            p.Start("Starting Rule");
#endif
            bool RunRule = busrule.StartRule(dmc, rule);
            if (RunRule)
            {
                result = busrule.ExecuteRule(dmc, parameters);
            }
            busrule.CompleteRule();
#if DEBUG
            p.Complete();
            //LogPlugin.Logger.LogInfoForSource(p.ToString(), LogName);
#endif
            return result;
        }

        public IBusinessRule CreateRule(Type type)
        {
            var dependenciesForType = GetDependenciesForType(type, dependencies);
            if (dependenciesForType.Count == 0)
            {
                return Activator.CreateInstance(type) as IBusinessRule;
            }
            else
            {
                return Activator.CreateInstance(type, dependenciesForType.ToArray()) as IBusinessRule;
            }
        }

        private List<object> GetDependenciesForType(Type type, Dictionary<Type, object> dependencies)
        {
            List<object> dependenciesForType = new List<object>();
            foreach (var constructor in type.GetConstructors())
            {
                var constructorParameters = constructor.GetParameters();
                int numberOfParameters = constructorParameters.Count();
                foreach (var constructorParameter in constructorParameters)
                {
                    if (dependencies.Any(x => x.Key == constructorParameter.ParameterType))
                    {
                        dependenciesForType.Add(dependencies[constructorParameter.ParameterType]);
                    }
                }
                if (dependenciesForType.Count == numberOfParameters)
                {
                    return dependenciesForType;
                }
                else
                {
                    dependenciesForType.Clear();
                }
            }
            return null;
        }

        /// <summary>
        /// Executes a single rule.
        /// </summary>
        /// <param name="dmc">The domain messages collection to populate.</param>
        /// <param name="ruleName">The name of the rule.</param>
        /// <param name="parameters">Any additional parameters.</param>
        public int ExecuteRule(IDomainMessageCollection dmc, string ruleName, params object[] parameters)
        {
            if (!Enabled)
                return 1;

            ReloadExclusions();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            return InternalExecuteRule(spc, dmc, ruleName, parameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dmc">The domain messages collection to populate.</param>
        /// <param name="rulesToRun">A list of rules to run.</param>
        /// <param name="parameters"></param>
        public void ExecuteRuleSet(IDomainMessageCollection dmc, List<string> rulesToRun, params object[] parameters)
        {
            if (!Enabled)
                return;

            ReloadExclusions();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            // loop through and run all the rules
            for (int i = 0; i < rulesToRun.Count; i++)
            {
                InternalExecuteRule(spc, dmc, rulesToRun[i], parameters);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dmc"></param>
        /// <param name="ruleSetNameToRun">The name of the ruleset to run.</param>
        /// <param name="parameters"></param>
        public void ExecuteRuleSet(IDomainMessageCollection dmc, string ruleSetNameToRun, params object[] parameters)
        {
            List<string> rulesToRun = new List<string>();
            IWorkflowRuleSet wfs = this.ruleRepository.GetRuleSetByName(ruleSetNameToRun);
            rulesToRun = wfs.RulesToRun;
            this.ExecuteRuleSet(dmc, rulesToRun, parameters);
        }

        /// <summary>
        /// Tells the service to reload the exclusion set.
        /// </summary>
        public void ReloadExclusions()
        {
            _excludedRuleItems.Clear();

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IList<RuleExclusionSets> spcExclusionSets = spc.ExclusionSets;
            foreach (RuleExclusionSets res in spcExclusionSets)
            {
                RuleExclusionSet_DAO ruleExclusionSet = RuleExclusionSet_DAO.Find((int)res);
                foreach (RuleExclusion_DAO ruleExclusion in ruleExclusionSet.RuleExclusions)
                {
                    _excludedRuleItems.Add(ruleExclusion.RuleItemKey);
                }
            }
        }
    }

    /*

    /// <summary>
    /// Represents business rule contained within an assembly.
    /// </summary>
    public interface IRuleObj
    {
        /// <summary>
        ///
        /// </summary>
        Assembly asm { get; }

        /// <summary>
        ///
        /// </summary>
        string RuleName { get; }

        /// <summary>
        ///
        /// </summary>
        IBusinessRule Rule { get; }
    }

    internal class RuleObj : IRuleObj
    {
        Assembly _asm;
        string _RuleName;
        IBusinessRule _Rule;
        public RuleObj(Assembly asm, string RuleName, IBusinessRule Rule)
        {
            this._asm = asm;
            this._RuleName = RuleName;
            this._Rule = Rule;
        }

        #region IRuleObj Members

        public Assembly asm
        {
            get { return asm; }
        }

        public string RuleName
        {
            get { return _RuleName; }
        }

        public IBusinessRule Rule
        {
            get { return _Rule; }
        }

        #endregion IRuleObj Members
    }
     * */
}