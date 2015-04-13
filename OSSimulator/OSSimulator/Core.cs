using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSSimulator
{
    public class Core
    {
        public List<Process> FCFSProcesses { get; set; }
        public List<Process> RRShortProcesses { get; set; }
        public List<Process> RRLongProesses { get; set; }
        public int Config { get; set; }
        public int EventTime { get; set; }

        public Core()
        {
            FCFSProcesses = new List<Process>();
            RRShortProcesses = new List<Process>();
            RRLongProesses = new List<Process>();
        }

        public Process FCFS()
        {
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
            EventTime = (currentTimeLapse + 2);
            return firstProcess;
        }

        public Process RRLong()
        {
            var maxLongTimeQuantum = 20;

            var firstProcess = RRLongProesses[0];
                RRLongProesses.RemoveAt(0);
            var currentTimeQuantum = (firstProcess.Tasks[firstProcess.TaskIndex].Time < maxLongTimeQuantum ? 
                firstProcess.Tasks[firstProcess.TaskIndex].Time :
                maxLongTimeQuantum);
            EventTime = (currentTimeQuantum + 2);

            firstProcess.Tasks[firstProcess.TaskIndex].Time -= currentTimeQuantum;

            foreach (var p in RRLongProesses)
            {
                if (p.ResponseTime == 0)
                    p.ResponseTime = (currentTimeQuantum + 2);
                p.TurnaroundTime += (currentTimeQuantum + 2);
                p.WaitTime += (currentTimeQuantum + 2);
            }
            firstProcess.TurnaroundTime += currentTimeQuantum;
            firstProcess.WaitTime += currentTimeQuantum;

            if ((firstProcess.TaskIndex == firstProcess.Tasks.Count &&
                firstProcess.Tasks[firstProcess.TaskIndex].Time == 0) ||
                (firstProcess.Tasks[firstProcess.TaskIndex].Time == 0 &&
                !firstProcess.Tasks[firstProcess.TaskIndex++].Type))
            {
                return firstProcess;
            }
            RRLongProesses.Add(firstProcess);
            return null;
        }

        public Process RRShort()
        {
            var maxLongTimeQuantum = 10;

            var firstProcess = RRLongProesses[0];
            RRLongProesses.RemoveAt(0);
            var currentTimeQuantum = (firstProcess.Tasks[firstProcess.TaskIndex].Time < maxLongTimeQuantum ?
                firstProcess.Tasks[firstProcess.TaskIndex].Time :
                maxLongTimeQuantum);
            EventTime = (currentTimeQuantum + 2);

            firstProcess.Tasks[firstProcess.TaskIndex].Time -= currentTimeQuantum;

            foreach (var p in RRLongProesses)
            {
                if (p.ResponseTime == 0)
                    p.ResponseTime = (currentTimeQuantum + 2);
                p.TurnaroundTime += (currentTimeQuantum + 2);
                p.WaitTime += (currentTimeQuantum + 2);
            }
            firstProcess.TurnaroundTime += currentTimeQuantum;
            firstProcess.WaitTime += currentTimeQuantum;

            if (firstProcess.TaskIndex == firstProcess.Tasks.Count &&
                firstProcess.Tasks[firstProcess.TaskIndex].Time == 0)
            {
                return firstProcess;
            }
            RRLongProesses.Add(firstProcess);
            return null;
        }
    } 
}
