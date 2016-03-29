using System;
using System.Collections.Generic;
using SAHL.Test;
using Rhino.Mocks;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI;
using SAHL.Common.Service.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Caching;


namespace SAHL.Web.Services.Test
{
    public class TestServiceBase : TestBase
    {
        readonly Dictionary<Type, object> _mocks = new Dictionary<Type, object>();

        protected void SetupMockedView(IViewBase mockedView)
        {
            SetupResult.For(mockedView.CurrentPrincipal).Return(TestPrincipal);
            SetupResult.For(mockedView.ViewName).Return("Test");
        }

        private object Setup(SAHL.Common.Security.SAHLPrincipal SAHLPrincipal)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected void SetupNavigationMock(IView view, string navigationValue)
        {
            ISimpleNavigator navigator = _mockery.CreateMock<ISimpleNavigator>();
            SetupResult.For(view.Navigator).Return(navigator);
            navigator.Navigate(navigationValue);
        }

        protected void SetupBaseMocks()
        {
            // CBOService
            //ICBOService CBO = _mockery.CreateMock<ICBOService>();
            CacheManager CM = CacheFactory.GetCacheManager("MOCK");
            //Type MockType = typeof(ICBOService); 
            //CM.Add(MockType.ToString(), CBO);
            //_mocks.Add(MockType, CBO);
            // X2Service
            IX2Service X2 = _mockery.CreateMock<IX2Service>();
            Type MockType = typeof(IX2Service);
            CM.Add(MockType.ToString(), X2);
            _mocks.Add(MockType, X2);
        }

        protected T GetMock<T>()
        {
            return (T)_mocks[typeof(T)];
        }
    }
}
