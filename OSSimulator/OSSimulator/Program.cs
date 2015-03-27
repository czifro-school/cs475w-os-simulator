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

            if (!fileReader.FileExistsAndHasContents)
                new FileGenerator().GenerateRandomProcessesFile(NumberOfProcesses);

            var processPool = new ProcessPool(fileReader.ReadContents());

            var scheduler = new Scheduler(processPool);

            scheduler.Run();
        }
    }
}
