using System;

namespace SAHL.Core.Caching
{
    public class CacheKeyGenerator : ICacheKeyGenerator
    {
        private IIocContainer iocContainer;

        public CacheKeyGenerator(IIocContainer iocContainer)
        {
            this.iocContainer = iocContainer;
        }

        public string GetKey<CacheKeyType, CacheItemType>(CacheKeyType context)
        {
            Type cacheContextType = typeof(CacheKeyType);
            Type openGenericGeneratorFactoryType = typeof(ICacheKeyGeneratorFactory<>);
            Type genericGeneratorFactoryType = openGenericGeneratorFactoryType.MakeGenericType(cacheContextType);
            var generatorFactory = this.iocContainer.GetInstance(genericGeneratorFactoryType) as ICacheKeyGeneratorFactory<CacheKeyType>;
            return generatorFactory.GetKey<CacheItemType>(context);
        }
    }
}