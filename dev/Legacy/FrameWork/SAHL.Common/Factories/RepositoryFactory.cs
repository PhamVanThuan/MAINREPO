using System;
using System.Text;

namespace SAHL.Common.Factories
{
    public static class RepositoryFactory
    {
        public static T GetRepository<T>(params object[] Parameters)
        {
            try
            {
                TypeFactory.EnsureLoaded();
                return TypeFactory.CreateType<T>(Parameters);
            }
            catch (System.Reflection.ReflectionTypeLoadException ex)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var loaderException in ex.LoaderExceptions)
                {
                    builder.AppendFormat("Loader Exception : {0}{1}", loaderException.Message, Environment.NewLine);
                }
                throw new Exception(string.Format("Unable to create type:{0}", builder.ToString()), ex);
            }
            catch (Exception ex)
            {
                Type TT = typeof(T);
                throw new Exception(string.Format("Unable to create type:{0}", TT.FullName), ex);
            }
        }
    }
}