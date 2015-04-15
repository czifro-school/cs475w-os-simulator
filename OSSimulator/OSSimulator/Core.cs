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
        public List<Process> RRLongProcesses { get; set; }
        public int Config { get; private set; }
        public int EventTime { get; private set; }

        public Core(int config)
        {
            FCFSProcesses = new List<Process>();
            RRShortProcesses = new List<Process>();
            RRLongProcesses = new List<Process>();
            Config = config;
        }

        public Process FCFS()
        {
            var firstProcess = FCFSProcesses[0];
            FCFSProcesses.RemoveAt(0);

            var currentTimeLapse = firstProcess.Tasks[firstProcess.TaskIndex].Time;
            firstProcess.Tasks[firstProcess.TaskIndex].Time = 0;
            firstProcess.HasExecutedOnce = true;

            foreach (var p in FCFSProcesses)
            {
                if (!p.HasExecutedOnce)
                    p.ResponseTime += (currentTimeLapse + 2);
                p.WaitTime += (currentTimeLapse + 2);
                p.TurnaroundTime += (currentTimeLapse + 2);
            }
            firstProcess.TurnaroundTime += (currentTimeLapse + 2);
            firstProcess.WaitTime += 2;

            EventTime = (currentTimeLapse + 2);
            return firstProcess;
        }

        public Process RRLong()
        {
            var maxLongTimeQuantum = 20;

            var firstProcess = RRLongProcesses[0];
                RRLongProcesses.RemoveAt(0);
            var currentTimeQuantum = (firstProcess.Tasks[firstProcess.TaskIndex].Time < maxLongTimeQuantum ? 
                firstProcess.Tasks[firstProcess.TaskIndex].Time :
                maxLongTimeQuantum);
            EventTime = (currentTimeQuantum + 2);

            firstProcess.Tasks[firstProcess.TaskIndex].Time -= currentTimeQuantum;

            foreach (var p in RRLongProcesses)
            {
                if (!p.HasExecutedOnce)
                    p.ResponseTime = (currentTimeQuantum + 2);
                p.TurnaroundTime += (currentTimeQuantum + 2);
                p.WaitTime += (currentTimeQuantum + 2);
            }
            firstProcess.TurnaroundTime += (currentTimeQuantum + 2);
            firstProcess.WaitTime += 2;

            if ((firstProcess.TaskIndex == (firstProcess.Tasks.Count - 1) &&
                firstProcess.Tasks[firstProcess.TaskIndex].Time == 0) ||
                (firstProcess.Tasks[firstProcess.TaskIndex].Time == 0 &&
                !firstProcess.Tasks[firstProcess.TaskIndex++].Type))
            {
                return firstProcess;
            }
            RRLongProcesses.Add(firstProcess);
            return null;
        }

        public Process RRShort()
        {
            var maxLongTimeQuantum = 10;

            var firstProcess = RRShortProcesses[0];
            RRShortProcesses.RemoveAt(0);
            var currentTimeQuantum = (firstProcess.Tasks[firstProcess.TaskIndex].Time < maxLongTimeQuantum ?
                firstProcess.Tasks[firstProcess.TaskIndex].Time :
                maxLongTimeQuantum);
            EventTime = (currentTimeQuantum + 2);

            firstProcess.Tasks[firstProcess.TaskIndex].Time -= currentTimeQuantum;

            foreach (var p in RRShortProcesses)
            {
                if (!p.HasExecutedOnce)
                    p.ResponseTime = (currentTimeQuantum + 2);
                p.TurnaroundTime += (currentTimeQuantum + 2);
                p.WaitTime += (currentTimeQuantum + 2);
            }
            firstProcess.TurnaroundTime += (currentTimeQuantum + 2);
            firstProcess.WaitTime += 2;

            if ((firstProcess.TaskIndex == (firstProcess.Tasks.Count - 1) &&
                firstProcess.Tasks[firstProcess.TaskIndex].Time == 0) ||
                (firstProcess.Tasks[firstProcess.TaskIndex].Time == 0 &&
                !firstProcess.Tasks[firstProcess.TaskIndex++].Type))
            {
                return firstProcess;
            }
            RRShortProcesses.Add(firstProcess);
            return null;
        }
    } 
}
