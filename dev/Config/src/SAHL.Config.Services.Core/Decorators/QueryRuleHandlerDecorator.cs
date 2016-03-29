using System;
using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Core.SystemMessages;

namespace SAHL.Config.Services.Core.Decorators
{
    [DecorationOrder(2)]
    public class QueryRuleHandlerDecorator<TQuery, TQueryResult> :
        IServiceQueryHandlerDecorator<TQuery, TQueryResult> where TQuery : IServiceQuery<IServiceQueryResult<TQueryResult>>
    {
        private readonly IServiceQueryHandler<TQuery> innerHandler;
        private readonly IIocContainer iocContainer;

        public QueryRuleHandlerDecorator(IServiceQueryHandler<TQuery> innerHandler, IIocContainer iocContainer)
        {
            this.innerHandler = innerHandler;
            this.iocContainer = iocContainer;
        }

        public IServiceQueryHandler<TQuery> InnerQueryHandler
        {
            get { return this.innerHandler; }
        }

        public ISystemMessageCollection HandleQuery(TQuery query)
        {
            var messages = new SystemMessageCollection();

            // get any rules for the query
            var rules = this.iocContainer.GetAllInstances<IServiceQueryRule<TQuery>>();
            foreach (var rule in rules)
            {
                try
                {
                    messages.Aggregate(rule.ExecuteRule(query));
                }
                catch (Exception)
                {
                    messages.AddMessage(new SystemMessage(GetRuleViolationMessage(rule), SystemMessageSeverityEnum.Error));
                }
            }

            if (!messages.HasErrors)
            {
                messages.Aggregate(innerHandler.HandleQuery(query));
            }

            return messages;
        }

        private static string GetRuleViolationMessage(IServiceQueryRule<TQuery> rule)
        {
            return string.Format("There was a system error running the a rule [{0}], processing has been halted", rule.GetType().FullName);
        }
    }
}