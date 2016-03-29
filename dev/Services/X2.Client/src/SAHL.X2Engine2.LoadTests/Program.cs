using SAHL.X2Engine2.Tests;
using SAHL.X2Engine2.Tests.X2.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.LoadTests
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = SpecificationIoCBootstrapper.Initialize();
            
            //var testCase = new TestCase();
            var testCase = new ApplicationCaptureTestCase(container.GetInstance<IX2TestDataManager>());
            TestService testService = new TestService(testCase);
            testService.CreateWorkers();
            Console.ReadLine();
        }
    }
}
