using System;
using System.Collections.Generic;
using System.Text;
using Workers.UnitTest;
using BaseTest;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                List<IBaseTest> Tests = new List<IBaseTest>();
                IBaseTest test = new LifeTests();
                test.Start(15);
                Tests.Add(test);

                test = new LoadApplicationManagementTests();
                test.Start(2);
                Tests.Add(test);
                test = new LoadApplicationCaptureTest();
                test.Start(10);
                Tests.Add(test);
                

                Console.WriteLine("Started");

                Console.ReadLine();
                Console.WriteLine("Sending Signal to Stop ...");
                foreach (IBaseTest t in Tests)
                {
                    t.Stop();
                }
                Console.WriteLine("Signaled all workers to complete then exit.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
