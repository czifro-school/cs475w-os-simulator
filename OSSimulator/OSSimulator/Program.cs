using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OSSimulator.IO;

namespace OSSimulator
{
    class Program
    {
        private const int NumberOfProcesses = 9000;
        static void Main(string[] args)
        {
            var fileReader = new FileReader();
            Console.WriteLine("Checking for data file...");
            if (!fileReader.FileExistsAndHasContents)
            {
                Console.WriteLine("Could not find file. Generating new one ...");
                new FileGenerator().GenerateRandomProcessesFile(NumberOfProcesses);
            }

            Console.WriteLine("Loading processes from file ...");
            var processPool = new ProcessPool(fileReader.ReadContents());

            //////
            /// <summary>
            /// Uncomment one of the following to run a configuration: 
            ///     first number represents number of cores, 
            ///     second number represents which algorithm:
            ///         1 = FCFS, 2 = RR Long, 3 = RR Short
            /// </summary>
            //////

            var scheduler = new Scheduler(processPool, 1, 1);
            //var scheduler = new Scheduler(processPool, 1, 2);
            //var scheduler = new Scheduler(processPool, 1, 3);
            //var scheduler = new Scheduler(processPool, 2, 1);
            //var scheduler = new Scheduler(processPool, 2, 2);
            //var scheduler = new Scheduler(processPool, 2, 3);
            //var scheduler = new Scheduler(processPool, 4, 1);
            //var scheduler = new Scheduler(processPool, 4, 2);
            //var scheduler = new Scheduler(processPool, 4, 3);
            //var scheduler = new Scheduler(processPool, 8, 1);
            //var scheduler = new Scheduler(processPool, 8, 2);
            //var scheduler = new Scheduler(processPool, 8, 3);
            //var scheduler = new Scheduler(processPool, 16, 1);
            //var scheduler = new Scheduler(processPool, 16, 2);
            //var scheduler = new Scheduler(processPool, 16, 3);

            scheduler.Run();

            var analysis = scheduler.Analysis();

            Console.WriteLine("Analysis (DateTime, Avg Response, Avg Wait, Avg Turnarround): {0}", analysis);


        }
    }
}
