using Database.Tests.Base;
using NUnit.Framework;

namespace Database.Tests
{
    public class DBTests : TestBase
    {
        [Ignore("Use this if u want to kill the db server || u r really really bored")]
        [Test, TestCaseSource(typeof(TestProvider), "GetAllTests")]
        public void __RunAllTests()
        {
            string message = RunAllTests("[2AM_Test]");
            message = message + RunAllTests("Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Ignore("Use this for local nUnit db test debugering")]
        [Test, TestCaseSource(typeof(TestProvider), "GetAllProcessTests")]
        public void _RunAllProcessTests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetAll2AMTests")]
        public void _RunAll2AMTests(string testName)
        {
            string message = RunTest(testName, "[2AM_Test]");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_backend")]
        public void Backend_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_batch")]
        public void Batch_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_dbo")]
        public void Dbo_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_deb")]
        public void Deb_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_fin")]
        public void Fin_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_fincore")]
        public void Fincore_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_frameworktest")]
        public void Framework_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_halo")]
        public void Halo_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_halonotran")]
        public void Halonotran_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_hoc")]
        public void Hoc_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_monthend")]
        public void Monthend_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_origination")]
        public void Origination_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_paymentallocation")]
        public void Paymentallocation_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_personalloan")]
        public void Personalloan_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_report")]
        public void Report_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_security")]
        public void Security_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_spv")]
        public void Spv_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_spvdet")]
        public void Spvdet_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_spvrh")]
        public void Spvrh_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_tranpost")]
        public void Tranpost_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetTests_trigger")]
        public void Trigger_Tests(string testName)
        {
            string message = RunTest(testName, "Process_Test");

            if (string.IsNullOrEmpty(message))
                Assert.Pass();
            else
                Assert.Fail(message);
        }

        [Test, TestCaseSource(typeof(TestProvider), "GetIgnoredTests")]
        public void __Ignored_Tests(string testName)
        {
            Assert.Ignore(testName + " was ignored");
        }
    }
}