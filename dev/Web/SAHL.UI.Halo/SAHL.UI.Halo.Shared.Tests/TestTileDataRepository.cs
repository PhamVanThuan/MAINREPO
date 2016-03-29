using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using StructureMap;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.UI.Halo.Models.Client;
using SAHL.UI.Halo.DataProvider.Client;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Client;
using SAHL.UI.Halo.ContentProvider.Client;
using SAHL.UI.Halo.MyHalo.Configuration.Home;
using SAHL.UI.Halo.Pages.Common.Transactions;
using SAHL.UI.Halo.Models.Common.LoanTransaction;
using SAHL.UI.Halo.Configuration.Client.MortgageLoan;
using SAHL.UI.Halo.ContentProvider.Client.MortgageLoan;
using SAHL.UI.Halo.DataProvider.Common.LoanTransactions;
using SAHL.UI.Halo.Configuration.Account.LoanTransaction;
using SAHL.UI.Halo.Configuration.Application;
using SAHL.UI.Halo.DataProvider.Application;
using SAHL.UI.Halo.MyHalo.Configuration.Home.LossControlManager;

namespace SAHL.UI.Halo.Shared.Tests
{
    [TestFixture]
    public class TestTileDataRepository
    {
        private IIocContainer iocContainer;

        [TestFixtureSetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(expression =>
                {
                    expression.Scan(scanner =>
                    {
                        scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));
                        scanner.LookForRegistries();
                    });

                    expression.For<IDbFactory>().Use<FakeDbFactory>();
                });

            this.iocContainer = ObjectFactory.GetInstance<IIocContainer>();
            Assert.IsNotNull(this.iocContainer);
        }

        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var container = Substitute.For<IIocContainer>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var repository = new TileDataRepository(container);
            //---------------Test Result -----------------------
            Assert.IsNotNull(repository);
        }

        [TestCase("iocContainer")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<TileDataRepository>(parameterName);
            //---------------Test Result -----------------------
        }

        //[Test]
        //public void FindTileDataModel_GivenNoDataModelExists_ShouldReturnNull()
        //{
        //    //---------------Set up test pack-------------------
        //    var repository = this.CreateTileDataRepository();
        //    //---------------Assert Precondition----------------
        //    //---------------Execute Test ----------------------
        //    var dataModel = repository.FindTileDataModel(new InvoicePaymentOverViewRootTileConfiguration());
        //    //---------------Test Result -----------------------
        //    Assert.IsNull(dataModel);
        //}

        [Test]
        public void FindTileDataModel_GivenDataModelExists_ShouldReturnDataModelInstance()
        {
            //---------------Set up test pack-------------------
            var repository = this.CreateTileDataRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var dataModel = repository.FindTileDataModel(new ClientRootTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataModel);
            Assert.IsInstanceOf<ClientRootModel>(dataModel);
        }

        //[Test]
        //public void FindTilePageState_GivenNoPageStateExists_ShouldReturnNull()
        //{
        //    //---------------Set up test pack-------------------
        //    var repository = this.CreateTileDataRepository();
        //    //---------------Assert Precondition----------------
        //    //---------------Execute Test ----------------------
        //    var tileState = repository.FindTilePageState(new InvoicePaymentOverViewRootTileConfiguration());
        //    //---------------Test Result -----------------------
        //    Assert.IsNull(tileState);
        //}

        [Test]
        public void FindTilePageState_GivenPageStateExists_ShouldReturnPageStateInstance()
        {
            //---------------Set up test pack-------------------
            var repository = this.CreateTileDataRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileState = repository.FindTilePageState(new LoanTransactionsRootTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNotNull(tileState);
            Assert.IsInstanceOf<LoanTransactionsPageState>(tileState);
        }

        [Test]
        public void FindTileChildDataProvider_ShouldRetrieveFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new MortgageLoanChildTileConfiguration();
            var localContainer    = Substitute.For<IIocContainer>();
            var repository        = this.CreateTileDataRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindTileChildDataProvider(tileConfiguration);
            //---------------Test Result -----------------------
            localContainer.Received(1).GetInstance(typeof(IHaloTileChildDataProvider<MortgageLoanChildTileConfiguration>));
        }

        [Test]
        public void FindTileChildDataProvider_GivenNoTileChildDataProvider_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var localContainer = Substitute.For<IIocContainer>();
            localContainer.GetInstance(typeof(IHaloTileChildDataProvider<ClientRootTileConfiguration>)).Returns(null);

            var repository = this.CreateTileDataRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProvider = repository.FindTileChildDataProvider(new ClientRootTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNull(contentProvider);
        }

        [Test]
        public void FindTileChildDataProvider_GivenTileChildDataProviderExistForModel_ShouldReturnTileChildDataProvider()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new MortgageLoanChildTileConfiguration();
            var repository        = this.CreateTileDataRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProvider = repository.FindTileChildDataProvider(tileConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(contentProvider);
            Assert.IsInstanceOf<MortgageLoanChildDataProvider>(contentProvider);
        }

        [Test]
        public void FindTileDynamicDataProvider_ShouldRetrieveFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new ApplicationRootTileConfiguration();
            var localContainer    = Substitute.For<IIocContainer>();
            var repository        = this.CreateTileDataRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindTileDynamicDataProvider(tileConfiguration);
            //---------------Test Result -----------------------
            localContainer.Received(1).GetInstance(typeof(IHaloTileDynamicDataProvider<ApplicationRootTileConfiguration>));
        }

        [Test]
        public void FindTileDynamicDataProvider_GivenNoTileDynamicDataProvider_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var localContainer = Substitute.For<IIocContainer>();
            localContainer.GetInstance(typeof(IHaloTileDynamicDataProvider<ApplicationRootTileConfiguration>)).Returns(null);

            var repository = this.CreateTileDataRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProvider = repository.FindTileDynamicDataProvider(new ApplicationRootTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNull(contentProvider);
        }

        [Test]
        public void FindTileDynamicDataProvider_GivenTileDynamicDataProviderExist_ShouldReturnTileDynamicDataProvider()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new ApplicationRootTileConfiguration();
            var repository        = this.CreateTileDataRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProvider = repository.FindTileDynamicDataProvider(tileConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(contentProvider);
            Assert.IsInstanceOf<ApplicationDynamicDataProvider>(contentProvider);
        }

        [Test]
        public void FindTileContentDataProvider_ShouldRetrieveFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new ClientRootTileConfiguration();
            var localContainer    = Substitute.For<IIocContainer>();
            var repository        = this.CreateTileDataRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindTileContentDataProvider(tileConfiguration);
            //---------------Test Result -----------------------
            localContainer.Received(1).GetInstance(typeof(IHaloTileContentDataProvider<ClientRootModel>));
        }

        [Test]
        public void FindTileContentDataProvider_GivenNoTileDataProvider_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var localContainer = Substitute.For<IIocContainer>();
            localContainer.GetInstance(typeof(IHaloTileContentDataProvider<ClientRootModel>)).Returns(null);

            var repository = this.CreateTileDataRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProvider = repository.FindTileContentDataProvider(new ClientRootTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNull(contentProvider);
        }

        [Test]
        public void FindTileContentDataProvider_GivenTileDataProviderExistForModel_ShouldReturnTileDataProvider()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new ClientRootTileConfiguration();
            var repository        = this.CreateTileDataRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProvider = repository.FindTileContentDataProvider(tileConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(contentProvider);
            Assert.IsInstanceOf<ClientRootTileContentDataProvider>(contentProvider);
        }

        [Test]
        public void FindTileContentMultipleDataProvider_ShouldRetrieveFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new LoanTransactionsRootTileConfiguration();
            var localContainer    = Substitute.For<IIocContainer>();
            var repository        = this.CreateTileDataRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindTileContentMultipleDataProvider(tileConfiguration);
            //---------------Test Result -----------------------
            localContainer.Received(1).GetInstance(typeof(IHaloTileContentMultipleDataProvider<LoanTransactionTileModel>));
        }

        [Test]
        public void FindTileContentMultipleDataProvider_GivenNoTileDataProvider_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var localContainer = Substitute.For<IIocContainer>();
            localContainer.GetInstance(typeof(IHaloTileContentMultipleDataProvider<LoanTransactionTileModel>)).Returns(null);

            var repository = this.CreateTileDataRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProvider = repository.FindTileContentMultipleDataProvider(new LoanTransactionsRootTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNull(contentProvider);
        }

        [Test]
        public void FindTileContentMultipleDataProvider_GivenTileDataProviderExistForModel_ShouldReturnTileDataProvider()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new LoanTransactionsRootTileConfiguration();
            var repository        = this.CreateTileDataRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProvider = repository.FindTileContentMultipleDataProvider(tileConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(contentProvider);
            Assert.IsInstanceOf<LoanTransactionsChildTileContentDataProvider>(contentProvider);
        }

        [Test]
        public void FindTileEditorDataProvider_ShouldRetrieveFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new ClientDetailEditorTileConfiguration();
            var localContainer    = Substitute.For<IIocContainer>();
            var repository        = this.CreateTileDataRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindTileEditorDataProvider(tileConfiguration);
            //---------------Test Result -----------------------
            localContainer.Received(1).GetInstance(typeof(IHaloTileEditorDataProvider<ClientRootModel>));
        }

        [Test]
        public void FindTileEditorDataProvider_GivenNoTileEditorDataProvider_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var localContainer = Substitute.For<IIocContainer>();
            localContainer.GetInstance(typeof(IHaloTileEditorDataProvider<ClientRootModel>)).Returns(null);

            var repository = this.CreateTileDataRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProvider = repository.FindTileEditorDataProvider(new ClientDetailEditorTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNull(contentProvider);
        }

        [Test]
        public void FindTileEditorDataProvider_GivenTileEditorDataProviderExistForModel_ShouldReturnTileEditorDataProvider()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new ClientDetailEditorTileConfiguration();
            var repository        = this.CreateTileDataRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var editorDataProvider = repository.FindTileEditorDataProvider(tileConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(editorDataProvider);
            Assert.IsInstanceOf<ClientRootEditorTileDataProvider>(editorDataProvider);
        }

        private ITileDataRepository CreateTileDataRepository(IIocContainer iocContainer = null)
        {
            iocContainer   = iocContainer ?? this.iocContainer;
            var repository = new TileDataRepository(iocContainer);
            return repository;
        }
    }
}
