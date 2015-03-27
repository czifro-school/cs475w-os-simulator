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

        public ProcessPool(List<RawProcess> rawProcesses)
        {
            _rawProcesses = rawProcesses;
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
                ProcessState = ProcessState.CREATED,
                Age = 0
            };

            //var process = new Process();
            //process.PID = Convert.ToInt32(processDetails[0]);
            //process.Priority = rand.Next(10);
            //process.TaskIndex = 0;
            //process.Tasks = new List<Task>();
            //for (var i = 2; i < 11; ++i)
            //{
            //    process.Tasks.Add(new Task { Type = (processDetails[i].Split(':').First() == "CPU"), Time = Convert.ToInt32(processDetails[i].Split(':').Last()) });
            //}
            //process.ProcessState = ProcessState.CREATED;
            //process.Age = 0;

            return process;
        }

        public void ReturnProcess(int PID)
        {
            var proc = _rawProcesses.SingleOrDefault(x => Convert.ToInt32(x.ProcessCSV.Split(',').First()) == PID);
            if (proc == null)
                return;

            var index = _rawProcesses.FindIndex(x => x.ProcessCSV.Equals(proc.ProcessCSV));

            _rawProcesses.ElementAt(index).IsBusy = false;
        }
    }
}
