using System;
using System.Collections.Generic;
using System.Text;
using BaseTest;
using NUnit.Framework;

namespace Workers.UnitTest
{
    [TestFixture]
    public class DemoTest : BaseTest.BaseTest
    {
        public override void Start(int NumberToSimulate)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        [NUnit.Framework.Test()]
        public void RunThroughDemoMapTest()
        {
            DemoWorker worker = new DemoWorker();
            worker.SetNumberIterations(1);
            worker.SetSleepTimeRange(100, 100);
            worker.Setup(-1, 1, "GenericKey", "Demo1", "Demo", "sahl\\bcuser", null);
            bool b = worker.DoMapLifeCycle();
            if (!b)
            {
                Assert.Fail();
            }
        }
    }
}
