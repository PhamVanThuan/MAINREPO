using System;

using StructureMap.Pipeline;
using StructureMap.Interceptors;

namespace SAHL.Config.Core.Decorators
{
    //sourced from https://gist.github.com/MattHoneycutt/3687346
    public static class StructureMapDecorator
    {
        public static DecoratorHelper<TTarget> Decorate<TTarget>(SmartInstance<TTarget> instance)
        {
            return new DecoratorHelper<TTarget>(instance);
        }
    }

    public class DecoratorHelper<TTarget>
    {
        private readonly SmartInstance<TTarget> instance;

        public DecoratorHelper(SmartInstance<TTarget> instance)
        {
            this.instance = instance;
        }

        public DecoratedInstance<TTarget> With<TDecorator>()
        {
            ContextEnrichmentHandler<TTarget> decorator = (context, commandHandler) =>
            {
                Type pluginType = context.BuildStack.Current.RequestedType;

                context.RegisterDefault(pluginType, commandHandler);

                return context.GetInstance<TDecorator>();
            };

            instance.EnrichWith(decorator);

            return new DecoratedInstance<TTarget>(instance, decorator);
        }
    }

    public class DecoratedInstance<TTarget>
    {
        private readonly SmartInstance<TTarget> instance;
        private ContextEnrichmentHandler<TTarget> decorator;

        public DecoratedInstance(SmartInstance<TTarget> instance, ContextEnrichmentHandler<TTarget> decorator)
        {
            this.instance = instance;
            this.decorator = decorator;
        }

        public DecoratedInstance<TTarget> AndThen<TDecorator>()
        {
            var previousDecorator = decorator;

            ContextEnrichmentHandler<TTarget> newDecorator = (context, commandHandler) =>
            {
                var pluginType = context.BuildStack.Current.RequestedType;

                var innerInstance = previousDecorator(context, commandHandler);

                context.RegisterDefault(pluginType, innerInstance);

                return context.GetInstance<TDecorator>();
            };

            instance.EnrichWith(newDecorator);

            decorator = newDecorator;

            return this;
        }
    }
}