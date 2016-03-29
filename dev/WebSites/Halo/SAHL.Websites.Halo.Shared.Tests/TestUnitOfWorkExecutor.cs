using System;
using System.Security.Principal;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;
using NSubstitute;

namespace SAHL.Websites.Halo.Shared.Tests
{
    [TestFixture]
    public class TestUnitOfWorkExecutor
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var currentUser = Substitute.For<IPrincipal>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var executor = new UnitOfWorkExecutor(currentUser);
            //---------------Test Result -----------------------
            Assert.IsNotNull(executor);
        }

        [Test]
        public void Execute_GivenNoActions_ShouldDoNothing()
        {
            //---------------Set up test pack-------------------
            var executor = this.CreateUnitOfWorkExecutor();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var executeResult = executor.Execute<IInvalidTestAction>();
            //---------------Test Result -----------------------
            Assert.IsFalse(executeResult);
        }

        [Test]
        public void Execute_GivenActions_ShouldExecuteActions()
        {
            //---------------Set up test pack-------------------
            var executor = this.CreateUnitOfWorkExecutor();
            //---------------Assert Precondition----------------
            TestUnitOfWorkAction.HasExecuted = false;
            //---------------Execute Test ----------------------
            var executeResult = executor.Execute<IValidTestAction>();
            //---------------Test Result -----------------------
            Assert.IsTrue(executeResult);
            Assert.IsTrue(TestUnitOfWorkAction.HasExecuted);
        }

        private UnitOfWorkExecutor CreateUnitOfWorkExecutor(IPrincipal currentUser = null)
        {
            var principal = currentUser ?? Substitute.For<IPrincipal>();
            var executor = new UnitOfWorkExecutor(principal);
            return executor;
        }

        public interface IInvalidTestAction : IUnitOfWorkAction
        {
        }

        public interface IValidTestAction : IUnitOfWorkAction
        {
        }

        public class TestUnitOfWorkAction : UnitOfWorkActionBase, IValidTestAction
        {
            static TestUnitOfWorkAction()
            {
                HasExecuted = false;
            }

            public static bool HasExecuted { get; set; }
            public override bool Execute()
            {
                HasExecuted = true;
                return true;
            }
        }
    }
}
