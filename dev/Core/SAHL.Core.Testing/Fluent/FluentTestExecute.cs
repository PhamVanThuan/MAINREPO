using System;
namespace SAHL.Core.Testing.Fluent
{
    public sealed class FluentTestExecute
    {
        private FluentTestContext context;
        public FluentTestExecute(FluentTestContext context)
        {
            this.context = context;
        }

        public FluentTestAssert Execute<T>(Action<T> action)
        {
            var instance = this.context.TestingIoc.GetInstance<T>();
            action.Invoke(instance);
            return new FluentTestAssert(this.context);
        }

        public FluentTestAssert Execute<T, TResults>(Func<T, TResults> func)
        {
            var instance = this.context.TestingIoc.GetInstance<T>();
            var returnVal = func.Invoke(instance);
            var key = String.Format("{0}{1}", typeof(T).Name, typeof(TResults).Name);
            this.context.Results.Add(key, returnVal);
            return new FluentTestAssert(this.context);
        }
    }
}