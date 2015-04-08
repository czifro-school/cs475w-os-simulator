using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


            var scheduler = new Scheduler(processPool);

            Console.WriteLine("Starting up uniprocessor ...");
            scheduler.Run();
        }
    }
}
