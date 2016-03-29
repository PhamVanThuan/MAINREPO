using System;
namespace SAHL.Core.Testing.Fluent
{
    public sealed class FluentTestAssert
    {
        private FluentTestContext context;
        public FluentTestAssert(FluentTestContext context)
        {
            this.context = context;
        }
        public void Assert<T>(Action<T> action)
        {
            var instance = this.context.TestingIoc.GetInstance<T>();
            action.Invoke(instance);
        }

        public void Assert<T, TResults>(Action<T, TResults> action)
        {
            var instance = this.context.TestingIoc.GetInstance<T>();
            object instance2 = new object();
            var key = String.Format("{0}{1}", typeof(T).Name, typeof(TResults).Name);
            this.context.Results.TryGetValue(key, out instance2);
            action.Invoke((T)instance, (TResults)instance2);
        }
    }
}