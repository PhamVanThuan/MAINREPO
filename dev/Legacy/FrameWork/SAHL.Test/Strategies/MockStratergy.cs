using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Rhino.Mocks;
using SAHL.Common.Factories;

namespace SAHL.Test.Strategies
{
    public class MockStrategy : IFactoryStrategy
    {
        static MockRepository _mockery = new MockRepository();

        #region IFactoryStrategy Members

        public T CreateType<T>(params object[] Parameters)
        {
            CacheManager CM = CacheFactory.GetCacheManager("MOCK");
            Type TType = typeof(T);

            string TypeName = TType.ToString();
            if (CM.Contains(TypeName))
                return (T)CM[TypeName];

            if (TType.IsInterface)
                return _mockery.StrictMock<T>();
            else
                return _mockery.StrictMock<T>(Parameters);
        }

        #endregion IFactoryStrategy Members
    }
}