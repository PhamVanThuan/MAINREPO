using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SAHL.Services.Query.Server.Specs.Helpers
{
    public static class TestHelpers
    {
        public static MethodInfo GetMethodInfo<T, U>(Expression<Func<T, U>> expression)
        {
            return ((MethodCallExpression) expression.Body).Method;
        }
    }
}