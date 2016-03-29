using System;

namespace SAHL.Common.Factories
{
    public static class ServiceFactory
    {
        public static T GetService<T>(params object[] Parameters)
        {
            try
            {
                TypeFactory.EnsureLoaded();
                return TypeFactory.CreateType<T>(Parameters);
            }
            catch (Exception ex)
            {
                Type TT = typeof(T);
                throw new Exception(string.Format("Unable to create type:{0}", TT.FullName), ex);
            }
        }
    }
}