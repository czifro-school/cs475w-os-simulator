using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSSimulator
{
    public class ProcessPool
    {
        private readonly List<RawProcess> _rawProcesses;
        public List<Process> FinishedProcesses { get; set; }

        public ProcessPool(List<RawProcess> rawProcesses)
        {
            _rawProcesses = rawProcesses;
            FinishedProcesses = new List<Process>();
        }

        public Process GetRandomProcess()
        {
            var rand = new Random();
            var filteredRawProcess = _rawProcesses.Where(x => !x.IsBusy).ToList();
            var randomRawProcess = filteredRawProcess.ElementAt(rand.Next(filteredRawProcess.Count()));

            var processDetails = randomRawProcess.ProcessCSV.Split(',');

            var process = new Process
            {
                PID = Convert.ToInt32(processDetails[0]),
                ArrivalTime = Convert.ToInt32(processDetails[1]),
                Priority = rand.Next(10),
                TaskIndex = 0,
                Tasks = new List<Task>
                {
                    new Task { Type = (processDetails[2].Split(':').First() == "CPU"), Time = Convert.ToInt32(processDetails[2].Split(':').Last()) },
                    new Task { Type = (processDetails[3].Split(':').First() == "CPU"), Time = Convert.ToInt32(processDetails[3].Split(':').Last()) },
                    new Task { Type = (processDetails[4].Split(':').First() == "CPU"), Time = Convert.ToInt32(processDetails[4].Split(':').Last()) },
                    new Task { Type = (processDetails[5].Split(':').First() == "CPU"), Time = Convert.ToInt32(processDetails[5].Split(':').Last()) },
                    new Task { Type = (processDetails[6].Split(':').First() == "CPU"), Time = Convert.ToInt32(processDetails[6].Split(':').Last()) },
                    new Task { Type = (processDetails[7].Split(':').First() == "CPU"), Time = Convert.ToInt32(processDetails[7].Split(':').Last()) },
                    new Task { Type = (processDetails[8].Split(':').First() == "CPU"), Time = Convert.ToInt32(processDetails[8].Split(':').Last()) },
                    new Task { Type = (processDetails[9].Split(':').First() == "CPU"), Time = Convert.ToInt32(processDetails[9].Split(':').Last()) },
                    new Task { Type = (processDetails[10].Split(':').First() == "CPU"), Time = Convert.ToInt32(processDetails[10].Split(':').Last()) },
                },
                Age = 0
            };

            return process;
        }

        public void ReturnProcess(Process p)
        {
            FinishedProcesses.Add(p);
        }
    }
}
