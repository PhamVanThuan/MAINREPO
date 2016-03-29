using System;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Attributes;

namespace SAHL.Common.Factories.Strategies
{
    public class DefaultStrategy : IFactoryStrategy
    {
        const string FACTORY = "FACTORY";

        #region IFactoryStrategy Members

        public T CreateType<T>(params object[] Parameters)
        {
            CacheManager CM = CacheFactory.GetCacheManager(FACTORY);
            Type TT = typeof(T);
            string TN = TT.ToString();

            if (CM.Contains(TN))
            {
                FactoryCacheItem FCi = CM[TN] as FactoryCacheItem;
                if (FCi != null)
                {
                    if (FCi.LifeTime == FactoryTypeLifeTime.Singleton && FCi.SingletonInstance != null)
                    {
                        return (T)FCi.SingletonInstance;
                    }
                    else
                    {
                        object obj = Activator.CreateInstance(FCi.FactoryItemInstanceType, Parameters);
                        if (FCi.LifeTime == FactoryTypeLifeTime.Singleton)
                            FCi.SingletonInstance = obj;

                        return (T)obj;
                    }
                }
            }

            return default(T);
        }

        #endregion IFactoryStrategy Members
    }
}