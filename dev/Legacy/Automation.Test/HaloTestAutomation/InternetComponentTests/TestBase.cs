using BuildingBlocks;
using NUnit.Framework;

namespace InternetComponentTests
{
    [TestFixture]
    public abstract class TestBase<TestView> where TestView : ObjectMaps.Pages.BasePageControls, new()
    {
        [TestFixtureSetUp]
        protected virtual void OnTestFixtureSetup()
        {
        }

        [TestFixtureTearDown]
        protected virtual void OnTestFixtureTearDown()
        {
        }

        [SetUp]
        protected virtual void OnTestStart()
        {
        }

        [TearDown]
        protected virtual void OnTestTearDown()
        {
        }

        protected TestView View
        {
            get
            {
                return (TestView)this.Browser.Page<TestView>(false);
            }
        }

        protected TestBrowser Browser { get; set; }

        public T Service<T>()
        {
            return ServiceLocator.Instance.GetService<T>();
        }
    }
}