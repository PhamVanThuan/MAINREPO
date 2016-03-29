using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Web.Views.Life.Interfaces;
using Rhino.Mocks;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI;
using SAHL.Common.Service.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.Test
{
    public class TestViewBase : TestBase
    {
        Dictionary<Type, object> _mocks = new Dictionary<Type, object>();

        protected void SetupMockedView(IViewBase MockedView)
        {
            SetupResult.For(MockedView.CurrentPrincipal).Return(TestPrincipal);
            SetupResult.For(MockedView.ViewName).Return("Test");
        }

        private object Setup(SAHL.Common.Security.SAHLPrincipal sAHLPrincipal)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected void SetupNavigationMock(IView view, string navigationValue)
        {
            ISimpleNavigator navigator = _mockery.StrictMock<ISimpleNavigator>();
            SetupResult.For(view.Navigator).Return(navigator);
            navigator.Navigate(navigationValue);
        }

        protected void SetupBaseMocks()
        {
            // CBOService
            //ICBOService CBO = _mockery.StrictMock<ICBOService>();
            CacheManager CM = CacheFactory.GetCacheManager("MOCK");
            //Type MockType = typeof(ICBOService); 
            //CM.Add(MockType.ToString(), CBO);
            //_mocks.Add(MockType, CBO);
            // X2Service
            IX2Service X2 = _mockery.StrictMock<IX2Service>();
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
