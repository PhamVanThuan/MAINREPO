using System;
using System.Collections.Generic;
using System.Reflection;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Logging;
using SAHL.Common.Service.Interfaces;

namespace DomainService2
{
    public class CommandHandler : ICommandHandler
    {
        private ISAHLPrincipalCacheProvider principalCacheProvider;
        private IDomainServiceIOC domainServiceIOC;
        private ILogger logger;
        private IRuleService ruleService;

        public CommandHandler(ISAHLPrincipalCacheProvider principalCacheProvider, IDomainServiceIOC domainServiceIOC, ILogger logger, IRuleService ruleService)
        {
            this.principalCacheProvider = principalCacheProvider;
            this.domainServiceIOC = domainServiceIOC;
            this.logger = logger;
            this.ruleService = ruleService;
        }

        public void HandleCommand<T>(IDomainMessageCollection messages, T command) where T : IDomainServiceCommand
        {
            ISAHLPrincipalCache SPC = this.principalCacheProvider.GetPrincipalCache();
            SPC.SetMessageCollection(messages);
            SPC.IgnoreWarnings = command.IgnoreWarnings;

            IHandlesDomainServiceCommand<T> handler = this.domainServiceIOC.GetCommandHandler<T>();
            handler.Handle(messages, command);
        }

        public bool CheckRules<T>(IDomainMessageCollection messages, T command) where T : RuleSetDomainServiceCommand
        {
            try
            {
                this.ruleService.ExecuteRuleSet(messages, command.WorkflowRuleSetName, command.RuleParameters);
                command.RuleParameters = null;
                return !HasMessages(messages, command.IgnoreWarnings);
            }
            catch (DomainValidationException)
            {
                command.RuleParameters = null;
                return !HasMessages(messages, command.IgnoreWarnings);
            }
            catch (Exception ex)
            {
                command.RuleParameters = null;
                Dictionary<string, object> methodParams = new Dictionary<string, object>();
                methodParams.Add("command", command);
                logger.LogFormattedErrorWithException("CommandHandler.CheckRules", "Unable to run ruleset: {0} parameters: {1}", ex, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParams } }, command.WorkflowRuleSetName, command.RuleParameters);
            }
            return false;
        }

        public bool CheckRule<T>(IDomainMessageCollection messages, T command) where T : RuleDomainServiceCommand
        {
            try
            {
                List<string> rulesToRun = new List<string>();
                rulesToRun.Add(command.WorkflowRuleName);
                this.ruleService.ExecuteRuleSet(messages, rulesToRun, command.RuleParameters);
                command.RuleParameters = null;
                return !HasMessages(messages, command.IgnoreWarnings);
            }
            catch (DomainValidationException)
            {
                command.RuleParameters = null;
                return !HasMessages(messages, command.IgnoreWarnings);
            }
            catch (Exception ex)
            {
                command.RuleParameters = null;
                Dictionary<string, object> methodParams = new Dictionary<string, object>();
                methodParams.Add("command", command);
                logger.LogFormattedErrorWithException("CommandHandler.CheckRule", "Unable to run rule: {0} parameters: {1}", ex, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParams } }, command.WorkflowRuleName, command.RuleParameters);
            }
            return false;
        }

        private bool HasMessages(IDomainMessageCollection messages, bool ignoreWarnings)
        {
            if (messages.HasErrorMessages)
            {
                return true;
            }
            if (messages.HasWarningMessages && !ignoreWarnings)
            {
                return true;
            }
            return false;
        }
    }
}