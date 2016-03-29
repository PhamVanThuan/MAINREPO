using System;
namespace SAHL.Core.Testing.Fluent
{
    public sealed class FluentTestSetup
    {
        private FluentTestContext context;
        public FluentTestSetup(FluentTestContext context)
        {
            this.context = context;
        }
        public FluentTestExecute Setup<T>(Action<FluentTestParameters> action = null)
            where T : class
        {
            var fluentTestParameters = new FluentTestParameters();
            if (action != null)
            {
                action.Invoke(fluentTestParameters);
            }

            this.context.TestingIoc.Configure(x =>
            {
                var configuredInstance =   x.For<T>().Use<T>();
                foreach (var testParam in fluentTestParameters)
                {
                    configuredInstance.Ctor<dynamic>(testParam.Key).Is(testParam.Value);
                }
            });

            return new FluentTestExecute(this.context);
        }
    }
}