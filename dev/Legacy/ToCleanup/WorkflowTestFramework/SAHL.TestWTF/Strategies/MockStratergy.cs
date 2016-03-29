using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Factories;
using Rhino.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace SAHL.TestWTF.Strategies
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

            if(TType.IsInterface)
                return _mockery.CreateMock<T>();
            else
                return _mockery.CreateMock<T>(Parameters);
        }

        #endregion
    }
}
