using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSSimulator
{
    public class IODevice
    {
        public List<Process> FCFSProcesses { get; set; }


        public IODevice()
        {
            FCFSProcesses = new List<Process>();

        }

        public Process FCFS()
        {
            if (FCFSProcesses.Count == 0)
                return null;

            var firstProcess = FCFSProcesses[0];
            FCFSProcesses.RemoveAt(0);

            var currentTimeLapse = firstProcess.Tasks[firstProcess.TaskIndex].Time;
            firstProcess.Tasks[firstProcess.TaskIndex].Time = 0;

            foreach (var p in FCFSProcesses)
            {
                p.WaitTime += (currentTimeLapse + 2);
                p.ResponseTime += (currentTimeLapse + 2);
                p.TurnaroundTime += (currentTimeLapse + 2);
            }

            return firstProcess;
        }
    }
}
