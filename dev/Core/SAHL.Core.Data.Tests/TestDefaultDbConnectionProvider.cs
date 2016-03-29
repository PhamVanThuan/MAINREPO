using NUnit.Framework;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using System;

namespace SAHL.Core.Data.Tests
{
    [TestFixture]
    public class TestDefaultDbConnectionProvider
    {
        [Test]
        public void RegisterUnitOfWorkContext_GivenEmptyContextName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateDbConnectionProvider();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => provider.RegisterUnitOfWorkContext(string.Empty));
            //---------------Test Result -----------------------
            StringAssert.Contains("uowContextName", exception.Message);
        }

        [Test]
        public void RegisterUnitOfWorkContext_GivenContextName_ShouldAddItemToQueue()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateDbConnectionProvider();
            //---------------Assert Precondition----------------
            Assert.IsFalse(provider.HasUnitOfWorkContexts);
            //---------------Execute Test ----------------------
            provider.RegisterUnitOfWorkContext("Test");
            //---------------Test Result -----------------------
            Assert.IsTrue(provider.HasUnitOfWorkContexts);
        }

        [Test]
        public void RegisterUnitOfWorkContext_GivenContextName_ShouldNotAddItemToQueue()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateFakeDbConnectionProvider();
            provider.RegisterUnitOfWorkContext("Test_1");
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, provider.NoUnitOfWorkContexts);
            //---------------Execute Test ----------------------
            provider.RegisterUnitOfWorkContext("Test_1");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, provider.NoUnitOfWorkContexts);
        }

        [Test]
        public void RegisterUnitOfWorkContext_GivenContextNamedoesNotExistInQueue_ShouldAddItemToQueue()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateFakeDbConnectionProvider();
            provider.RegisterUnitOfWorkContext("Test_1");
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, provider.NoUnitOfWorkContexts);
            //---------------Execute Test ----------------------
            provider.RegisterUnitOfWorkContext("Test_2");
            //---------------Test Result -----------------------
            Assert.AreEqual(2, provider.NoUnitOfWorkContexts);
        }

        [Test]
        public void UnregisterUnitOfWorkContext_GivenNoItemInQueue_ShouldDoNothing()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateDbConnectionProvider();
            //---------------Assert Precondition----------------
            Assert.IsFalse(provider.HasUnitOfWorkContexts);
            //---------------Execute Test ----------------------
            provider.UnregisterUnitOfWorkContext(string.Empty);
            //---------------Test Result -----------------------
            Assert.IsFalse(provider.HasUnitOfWorkContexts);
        }

        [Test]
        public void UnregisterUnitOfWorkContext_GivenOneItemInQueue_ShouldRemoveItemFromQueue()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateFakeDbConnectionProvider();
            provider.RegisterUnitOfWorkContext("Test_1");
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, provider.NoUnitOfWorkContexts);
            //---------------Execute Test ----------------------
            provider.UnregisterUnitOfWorkContext("Test_1");
            //---------------Test Result -----------------------
            Assert.AreEqual(0, provider.NoUnitOfWorkContexts);
        }

        [Test]
        public void UnregisterUnitOfWorkContext_GivenTwoItemsInQueue_ShouldRemoveLastItemAddedFromQueue()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateFakeDbConnectionProvider();
            provider.RegisterUnitOfWorkContext("Test_1");
            provider.RegisterUnitOfWorkContext("Test_2");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, provider.NoUnitOfWorkContexts);
            //---------------Execute Test ----------------------
            provider.UnregisterUnitOfWorkContext("Test_2");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, provider.NoUnitOfWorkContexts);
            Assert.IsTrue(provider.IsUnitOfWorkContextInQueue("Test_1"));
            Assert.IsFalse(provider.IsUnitOfWorkContextInQueue("Test_2"));
        }

        [Test]
        public void RegisterContext_GivenEmptyContextName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateDbConnectionProvider();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => provider.RegisterContext(string.Empty));
            //---------------Test Result -----------------------
            StringAssert.Contains("connectionContextName", exception.Message);
        }

        [Test]
        public void RegisterContext_GivenContextName_ShouldAddContextToContextList()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateDbConnectionProvider();
            //---------------Assert Precondition----------------
            Assert.IsFalse(provider.HasRegisteredContexts);
            //---------------Execute Test ----------------------
            provider.RegisterContext(Strings.DBCONTEXT_APP);
            //---------------Test Result -----------------------
            Assert.IsTrue(provider.HasRegisteredContexts);
        }

        [Test]
        public void RegisterContext_GivenContextName_ShouldBeGivenConnection()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateDbConnectionProvider();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var dbConnection = provider.RegisterContext(Strings.DBCONTEXT_APP);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dbConnection);
        }

        [Test]
        public void RegisterContext_GivenNewContextName_ShouldBeUsingNewConnection()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateDbConnectionProvider();
            var dbConnection1 = provider.RegisterContext(Strings.DBCONTEXT_APP);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var dbConnection2 = provider.RegisterContext(Strings.DBCONTEXT_WORKFLOW);
            //---------------Test Result -----------------------
            Assert.AreNotSame(dbConnection1, dbConnection2);
        }

        [Test]
        public void RegisterContext_GivenExistingContextName_ShouldBeUsingSameConnection()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateDbConnectionProvider();
            var dbConnection1 = provider.RegisterContext(Strings.DBCONTEXT_APP);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var dbConnection2 = provider.RegisterContext(Strings.DBCONTEXT_APP);
            //---------------Test Result -----------------------
            Assert.AreSame(dbConnection1, dbConnection2);
        }

        [Test]
        public void UnregisterContext_GivenEmptyContextName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateDbConnectionProvider();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => provider.UnRegisterContext(string.Empty));
            //---------------Test Result -----------------------
            StringAssert.Contains("connectionContextName", exception.Message);
        }

        [Test]
        public void UnregisterContext_GivenNonExistingContextName_ShouldDoNothing()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateDbConnectionProvider();
            //---------------Assert Precondition----------------
            Assert.IsFalse(provider.HasRegisteredContexts);
            //---------------Execute Test ----------------------
            provider.UnRegisterContext(Strings.DBCONTEXT_APP);
            //---------------Test Result -----------------------
            Assert.IsFalse(provider.HasRegisteredContexts);
        }

        [Test]
        public void RegisterContext_GivenExistingUnitOfWorkContext_ShouldAssociateContextWithUnitOfWork()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateFakeDbConnectionProvider();
            provider.RegisterUnitOfWorkContext(Strings.DBCONTEXT_APP);
            //---------------Assert Precondition----------------
            Assert.IsTrue(provider.IsUnitOfWorkContextInQueue(Strings.DBCONTEXT_APP));
            //---------------Execute Test ----------------------
            provider.RegisterContext(Strings.DBCONTEXT_APP);
            //---------------Test Result -----------------------
            Assert.IsTrue(provider.HasRegisteredContexts);
        }

        [Test]
        public void RegisterContext_GivenExistingUnitOfWorkContext_ShouldUseSameConnectionForAllDatabaseContextsWithSameUnitOfWork()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateDbConnectionProvider();
            provider.RegisterUnitOfWorkContext(Strings.DBCONTEXT_APP);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var connection1 = provider.RegisterContext(Strings.DBCONTEXT_APP);
            var connection2 = provider.RegisterContext(Strings.DBCONTEXT_APP);
            var connection3 = provider.RegisterContext(Strings.DBCONTEXT_WORKFLOW);
            //---------------Test Result -----------------------
            Assert.AreSame(connection1, connection2);
            Assert.AreNotSame(connection1, connection3);
        }

        [Test]
        public void RegisterContext_GivenMultipleUnitOfWorkContexts_ShouldUseSameConnectionForAllDatabaseContextsWithUnitOfWork()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateDbConnectionProvider();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            provider.RegisterUnitOfWorkContext(Guid.NewGuid().ToString());
            var connection1 = provider.RegisterContext(Strings.DBCONTEXT_APP);
            var connection2 = provider.RegisterContext(Strings.DBCONTEXT_APP);
            var connection3 = provider.RegisterContext(Strings.DBCONTEXT_WORKFLOW);

            provider.RegisterUnitOfWorkContext(Guid.NewGuid().ToString());
            var connection4 = provider.RegisterContext(Strings.DBCONTEXT_APP);
            var connection5 = provider.RegisterContext(Strings.DBCONTEXT_APP);
            //---------------Test Result -----------------------
            Assert.AreSame(connection1, connection2);
            Assert.AreSame(connection4, connection5);
            Assert.AreNotSame(connection1, connection3);
            Assert.AreSame(connection1, connection4);
        }

        [Test]
        public void RegisterContext_GivenMultipleUnitOfWorkContexts_AndUnitOfWorkEmbedded_ShouldUseSameConnectionForAllDatabaseContextsWithSameUnitOfWork()
        {
            //---------------Set up test pack-------------------
            var provider = this.CreateDbConnectionProvider();
            var uowContext1 = Guid.NewGuid().ToString();
            var uowContext2 = Guid.NewGuid().ToString();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            provider.RegisterUnitOfWorkContext(uowContext1);
            var connection1 = provider.RegisterContext(Strings.DBCONTEXT_APP);
            var connection2 = provider.RegisterContext(Strings.DBCONTEXT_APP);

            provider.RegisterUnitOfWorkContext(uowContext2);
            var connection3 = provider.RegisterContext(Strings.DBCONTEXT_APP);
            var connection4 = provider.RegisterContext(Strings.DBCONTEXT_APP);
            provider.UnregisterUnitOfWorkContext(Strings.DBCONTEXT_APP);

            var connection5 = provider.RegisterContext(Strings.DBCONTEXT_APP);
            //---------------Test Result -----------------------
            Assert.AreSame(connection1, connection2);
            Assert.AreSame(connection3, connection4);
            Assert.AreSame(connection1, connection3);
            Assert.AreSame(connection1, connection4);
            Assert.AreSame(connection1, connection5);
        }

        private IDbConnectionProvider CreateDbConnectionProvider(IDbConfigurationProvider dbConfigurationProvider = null)
        {
            var configurationProvider = dbConfigurationProvider ?? new DefaultDbConfigurationProvider();
            var provider = new DefaultDbConnectionProvider(configurationProvider);
            return provider;
        }

        private FakeDbConnectionProvider CreateFakeDbConnectionProvider(IDbConfigurationProvider dbConfigurationProvider = null)
        {
            var configurationProvider = dbConfigurationProvider ?? new DefaultDbConfigurationProvider();
            var provider = new FakeDbConnectionProvider(configurationProvider);
            return provider;
        }

        private class FakeDbConnectionProvider : DefaultDbConnectionProvider
        {
            public FakeDbConnectionProvider(IDbConfigurationProvider dbConfigurationProvider)
                : base(dbConfigurationProvider)
            {
            }

            public new int NoUnitOfWorkContexts
            {
                get { return base.NoUnitOfWorkContexts; }
            }

            public new bool IsUnitOfWorkContextInQueue(string uowContextName)
            {
                return base.IsUnitOfWorkContextInQueue(uowContextName);
            }
        }
    }
}