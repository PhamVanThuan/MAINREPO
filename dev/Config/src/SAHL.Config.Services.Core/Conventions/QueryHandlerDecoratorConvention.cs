using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Config.Core.Decorators;
using SAHL.Core.Data;
using SAHL.Core.Extensions;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace SAHL.Config.Services.Core.Conventions
{
    public class QueryHandlerDecoratorConvention : IRegistrationConvention
    {
        private readonly IEnumerable<Type> decorators;

        public QueryHandlerDecoratorConvention()
        {
            decorators = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("SAHL") || x.FullName.StartsWith("Capitec"))
                                      .SelectMany(t => t.GetTypes())
                                      .Where(t => t.IsClass && t.IsAssignableToGenericType(typeof(IServiceQueryHandlerDecorator<,>)))
                                      .OrderBy(t =>
                                          {
                                              var attribute = t.GetCustomAttributes(typeof(DecorationOrderAttribute), true).FirstOrDefault() as DecorationOrderAttribute;
                                              return attribute == null ? 0 : attribute.Index;
                                          });
        }

        public void Process(Type queryHandlerType, Registry registry)
        {
            if ((!queryHandlerType.IsAssignableToGenericType(typeof(IServiceQueryHandler<>)) && !queryHandlerType.IsAssignableToGenericType(typeof(ISqlServiceQuery<>))) ||
                queryHandlerType.IsAssignableToGenericType(typeof(IServiceQueryHandlerDecorator<,>)) ||
                queryHandlerType.IsAbstract ||
                queryHandlerType == typeof(SqlServiceQueryHandler<,>) ||
                queryHandlerType.FullName.StartsWith("SAHL.Services.DecisionTree.QueryHandlers.DecisionTreeServiceQueryHandler"))
            { return; }

            var type = queryHandlerType.GetInterface("IDontDecorateServiceCommand");

            if (!queryHandlerType.IsAssignableToGenericType(typeof(ISqlServiceQuery<>)))
            {
                this.ProcessNonSqlServiceQueries(queryHandlerType, registry, type);
            }
            else
            {
                this.ProcessSqlServiceQuery(queryHandlerType, registry, type);
            }
        }

        private void ProcessNonSqlServiceQueries(Type queryHandlerType, Registry registry, Type type)
        {
            var queryHandlerInterfaces = queryHandlerType.GetInterfaces().Where(x => x.IsAssignableToGenericType(typeof(IServiceQueryHandler<>)));
            foreach (var queryHandlerInterface in queryHandlerInterfaces)
            {
                var queryType = queryHandlerInterface.GetGenericArguments().First();

                // get the returntype of the query
                var serviceQueryType = queryType.GetInterfaces().SingleOrDefault(x => x.IsAssignableToGenericType(typeof(IServiceQuery<>)) && x.IsInterface);
                if (serviceQueryType == null) { continue; }

                var serviceGenericQueryReturnType = serviceQueryType.GetGenericArguments().FirstOrDefault();
                if (serviceGenericQueryReturnType == null) { continue; }

                var serviceQueryReturnType = serviceGenericQueryReturnType.GetGenericArguments().FirstOrDefault();
                var enrichWithMethod = typeof(QueryHandlerDecoratorConvention).GetMethod("EnrichWith");
                var genericEnrichWithMethod = enrichWithMethod.MakeGenericMethod(new Type[] { queryType, serviceQueryReturnType, queryHandlerType });

                if (type == null)
                {
                    genericEnrichWithMethod.Invoke(null, new object[] { registry, decorators });
                }
            }
        }

        private void ProcessSqlServiceQuery(Type queryHandlerType, Registry registry, Type type)
        {
            var queryType = queryHandlerType;
            var sqlQueryInterfaces = queryType.GetInterfaces().Where(x => x.IsAssignableToGenericType(typeof(ISqlServiceQuery<>)));

            foreach (var sqlQueryInterface in sqlQueryInterfaces)
            {
                var sqlQueryReturnType = sqlQueryInterface.GetGenericArguments()[0];
                var openGenericSqlServiceQueryHandler = typeof(SqlServiceQueryHandler<,>);
                var genericSqlServiceQueryHandler = openGenericSqlServiceQueryHandler.MakeGenericType(new Type[] { queryType, sqlQueryReturnType });
                var enrichWithMethod = typeof(QueryHandlerDecoratorConvention).GetMethod("EnrichWith");
                var genericEnrichWithMethod = enrichWithMethod.MakeGenericMethod(new Type[] { queryType, sqlQueryReturnType, genericSqlServiceQueryHandler });

                if (type == null)
                {
                    genericEnrichWithMethod.Invoke(null, new object[] { registry, decorators });
                }
            }
        }

        public static void EnrichWith<TQueryType, TQueryResultType, TQueryHandler>(Registry registry, IEnumerable<Type> decorators)
            where TQueryType : IServiceQuery<IServiceQueryResult<TQueryResultType>>
            where TQueryHandler : IServiceQueryHandler<TQueryType>
        {
            var registeredInstance = registry.For<IServiceQueryHandler<TQueryType>>().Use<TQueryHandler>();
            object previousDecorator = null;

            foreach (var decorator in decorators)
            {
                var genericDecoratorType = decorator.MakeGenericType(new Type[] { typeof(TQueryType), typeof(TQueryResultType) });
                if (previousDecorator == null)
                {
                    var method = typeof(StructureMapDecorator).GetMethod("Decorate");
                    var genericMethod = method.MakeGenericMethod(typeof(TQueryHandler));
                    var decoratorHelper = genericMethod.Invoke(null, new object[] { registeredInstance });

                    var withMethod = decoratorHelper.GetType().GetMethod("With");
                    var genericWithMethod = withMethod.MakeGenericMethod(genericDecoratorType);
                    previousDecorator = genericWithMethod.Invoke(decoratorHelper, null);
                }
                else
                {
                    var andThenMethod = previousDecorator.GetType().GetMethod("AndThen");
                    var genericAndThenMethod = andThenMethod.MakeGenericMethod(genericDecoratorType);
                    previousDecorator = genericAndThenMethod.Invoke(previousDecorator, null);
                }
            }
        }
    }
}