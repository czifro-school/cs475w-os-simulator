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
        private const string FCFSSingle = "FCFSSingle.csv";
        private const string FCFSDual = "FCFSDual.csv";
        private const string FCFSQuad = "FCFSQuad.csv";
        private const string FCFSOcto = "FCFSOcto.csv";
        private const string FCFS16 = "FCFS16.csv";
        private const string RRLongSingle = "RRLongSingle.csv";
        private const string RRLongDual = "RRLongDual.csv";
        private const string RRLongQuad = "RRLongQuad.csv";
        private const string RRLongOcto = "RRLongOcto.csv";
        private const string RRLong16 = "RRLong16.csv";


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
                processes[i] = (i+1).ToString() + "," + (rand.Next(300) + 30) + ",";
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

        public void AddToFCFSSingle(string results)
        {
            string[] lines;

            if (new FileInfo("..\\..\\" + FCFSSingle).Length == 0)
            {
                lines = new[] {"datetime,avg response,avg wait,avg turnaround", results};
                File.WriteAllLines("..\\..\\" + FCFSSingle, lines);
                return;
            }

            lines = new[] { results };
            File.WriteAllLines("..\\..\\" + FCFSSingle, lines);
        }

        public void AddToFCFSDual(string results)
        {
            string[] lines;

            if (new FileInfo("..\\..\\" + FCFSSingle).Length == 0)
            {
                lines = new[] { "datetime,avg response,avg wait,avg turnaround", results };
                File.WriteAllLines("..\\..\\" + FCFSSingle, lines);
                return;
            }

            lines = new[] { results };
            File.WriteAllLines("..\\..\\" + FCFSSingle, lines);
        }
    }
}
