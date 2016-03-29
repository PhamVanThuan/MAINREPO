using SAHL.Config.Core.Conventions;
using SAHL.Config.Core.Decorators;
using SAHL.Core.Extensions;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.DecisionTree.QueryHandlers;
using SAHL.Services.Interfaces.DecisionTree;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Config.Services.DecisionTree.Server
{
    public class DecisionTreeHandlerDecoratorConvention : IRegistrationConvention
    {
        private IEnumerable<Type> decorators;

        public DecisionTreeHandlerDecoratorConvention()
        {
            decorators = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("SAHL") || x.FullName.StartsWith("Capitec"))
                                      .SelectMany(t => t.GetTypes())
                                      .Where(t => t.IsClass &&
                                            t.IsAssignableToGenericType(typeof(IServiceQueryHandlerDecorator<,>))).OrderBy(t =>
                                      {
                                          var attribute = t.GetCustomAttributes(typeof(DecorationOrderAttribute), true).FirstOrDefault() as DecorationOrderAttribute;
                                          if (attribute == null)
                                              return 0;
                                          return attribute.Index;
                                      });
        }

        public void Process(Type queryType, Registry registry)
        {
            Type type = queryType.GetInterface("IDontDecorateServiceCommand");
            if ((queryType.IsAssignableToGenericType(typeof(IServiceQuery<>)) && (queryType.IsAssignableToGenericType(typeof(IDecisionTreeServiceQuery<>)))
                 && queryType.IsAbstract == false))
            {
                var decisionTreeQueryInterfaces = queryType.GetInterfaces().Where(x => x.IsAssignableToGenericType(typeof(IDecisionTreeServiceQuery<>)));
                foreach (var decisionTreeQueryInterface in decisionTreeQueryInterfaces)
                {
                    var decisionTreeQueryReturnType = decisionTreeQueryInterface.GetGenericArguments()[0];
                    var openGenericDecisionTreeServiceQueryHandler = typeof(DecisionTreeServiceQueryHandler<,>);
                    var genericDecisionTreeServiceQueryHandler = openGenericDecisionTreeServiceQueryHandler.MakeGenericType(new Type[] { queryType, decisionTreeQueryReturnType });
                    var enrichWithMethod = typeof(DecisionTreeHandlerDecoratorConvention).GetMethod("EnrichWith");
                    var genericEnrichWithMethod = enrichWithMethod.MakeGenericMethod(new Type[] { queryType, decisionTreeQueryReturnType, genericDecisionTreeServiceQueryHandler });
                    if (type == null)
                    {
                        genericEnrichWithMethod.Invoke(null, new object[] { registry, decorators });
                    }
                }
            }
        }

        public static void EnrichWith<QueryType, QueryResultType, QueryHandler>(Registry registry, IEnumerable<Type> decorators)
            where QueryType : IServiceQuery<IServiceQueryResult<QueryResultType>>
            where QueryHandler : IServiceQueryHandler<QueryType>
        {
            var registeredInstance = registry.For<IServiceQueryHandler<QueryType>>().Use<QueryHandler>();
            object previousDecorator = null;
            foreach (var decorator in decorators)
            {
                var genericDecoratorType = decorator.MakeGenericType(new Type[] { typeof(QueryType), typeof(QueryResultType) });
                if (previousDecorator == null)
                {
                    //.Decorate<MyQueryHandler<MyQuery>>();
                    var method = typeof(StructureMapDecorator).GetMethod("Decorate");
                    var genericMethod = method.MakeGenericMethod(typeof(QueryHandler));
                    object decoratorHelper = genericMethod.Invoke(null, new object[] { registeredInstance });

                    //.With<LoggingQueryHandlerDecorator<MyQuery>>()
                    var withMethod = decoratorHelper.GetType().GetMethod("With");
                    var genericWithMethod = withMethod.MakeGenericMethod(genericDecoratorType);
                    previousDecorator = genericWithMethod.Invoke(decoratorHelper, null);
                }
                else
                {
                    //.AndThen<TransactionQueryHandlerDecorator<MyQuery>>()
                    var andThenMethod = previousDecorator.GetType().GetMethod("AndThen");
                    var genericAndThenMethod = andThenMethod.MakeGenericMethod(genericDecoratorType);
                    previousDecorator = genericAndThenMethod.Invoke(previousDecorator, null);
                }
            }
        }
    }
}