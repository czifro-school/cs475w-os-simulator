using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OSSimulator.IO
{
    public class FileGenerator
    {
        private readonly string _path = "";
        private const string Filename = "processes.csv";

        public FileGenerator()
        {
            _path = Path.GetFullPath("..\\..\\" + Filename);
        }

        /// <summary>
        /// Generates a CSV file with random processes in it
        /// </summary>
        /// <param name="numProcesses"></param>
        public void GenerateRandomProcessesFile(int numProcesses)
        {
            var rand = new Random();
            const int maxBurstTime = 25; // milliseconds
            var processes = new string[numProcesses];
            string CPU = "CPU:", IO = "IO:";

            for (var i = 0; i < numProcesses; ++i)
            {
                processes[i] = (i+1).ToString() + "," + (rand.Next(maxBurstTime) + maxBurstTime).ToString() + ",";
                processes[i] += CPU + (rand.Next(maxBurstTime) + maxBurstTime) + ",";

                // Generate random amounts of either IO or CPU bursts
                processes[i] += (rand.Next(2) == 0 ? CPU : IO) + (rand.Next(maxBurstTime) + maxBurstTime) + ",";
                processes[i] += (rand.Next(2) == 0 ? CPU : IO) + (rand.Next(maxBurstTime) + maxBurstTime) + ",";
                processes[i] += (rand.Next(2) == 0 ? CPU : IO) + (rand.Next(maxBurstTime) + maxBurstTime) + ",";
                processes[i] += (rand.Next(2) == 0 ? CPU : IO) + (rand.Next(maxBurstTime) + maxBurstTime) + ",";
                processes[i] += (rand.Next(2) == 0 ? CPU : IO) + (rand.Next(maxBurstTime) + maxBurstTime) + ",";
                processes[i] += (rand.Next(2) == 0 ? CPU : IO) + (rand.Next(maxBurstTime) + maxBurstTime) + ",";
                processes[i] += (rand.Next(2) == 0 ? CPU : IO) + (rand.Next(maxBurstTime) + maxBurstTime) + ",";
                processes[i] += (rand.Next(2) == 0 ? CPU : IO) + (rand.Next(maxBurstTime) + maxBurstTime);
            }
            System.IO.File.WriteAllLines(_path, processes);
        }
    }
}
