using CommandLine;
using SAHL.Services.Cuttlefish.Worker.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Cuttlefish.Worker.LatencyMessage.v2
{
    class Program
    {
        static void Main(string[] args)
        {
            

            Console.WriteLine("LatencyMessage.v2");

            CommandLineArguments commands = new CommandLineArguments();

            Parser parser = new Parser();
            bool parserResult = parser.ParseArguments(args, commands);
            if (parserResult == true)
            {

                try
                {
                    Console.WriteLine("Parsed");
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Error");
                    Environment.Exit(-1);
                }
                //Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Error");
                Environment.Exit(-2);
            }

            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
