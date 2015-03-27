using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSSimulator
{
    public class Core
    {
        private readonly List<Process> _processes = new List<Process>();
        private readonly List<Process> _zombieProcesses = new List<Process>();
        private readonly List<Process> _finishedProcesses = new List<Process>();
        private const int MaxTimeQuantum = 20;
        private const int MaxContextSwitchTime = 2;

        public Core()
        {
            CanExecute = true;
        }

        private Process ProcessInUse { get; set; }

        public int ProcessCount { get { return _processes.Count; } }

        public int CSTime { get; set; }
        public int CurrentTimeQuantum { get; set; }

        public bool CPUBusy { get { return 0 < CurrentTimeQuantum; } }

        public bool CPUContextSwitching { get { return 0 < CSTime; } }

        public bool CanCS { get; private set; }
        public bool CanExecute { get; private set; }

        /// <summary>
        /// Puts process in running state, discrete event
        /// </summary>
        public void ExecuteProcess()
        {
            if (_processes.Count == 0)
                return;
            ProcessInUse = _processes[0];
            ProcessInUse.ProcessState = ProcessState.USER_RUNNING;
            var processTaskTime = ProcessInUse.Tasks[ProcessInUse.TaskIndex].Time;
            CurrentTimeQuantum = (MaxTimeQuantum < processTaskTime ? MaxTimeQuantum : processTaskTime);
            ProcessInUse.Tasks[ProcessInUse.TaskIndex].Time -= CurrentTimeQuantum;
            CanCS = true;
            CanExecute = false;
            _processes.RemoveAt(0);
        }

        /// <summary>
        /// Pulls process from running state, 
        /// sets context switch time, 
        /// ages processes to prevent starvation, 
        /// if process is not finished, priority is reduced and process is add back to queue
        /// if process is finished with CPU burst and needs to switch to IO, process is placed in outgoing queue
        /// if process is completely finished, processes is put in Zombie state and placed on queue for terminated processes.
        /// </summary>
        public void ContextSwitch()
        {
            CanCS = false;
            CanExecute = true;
            if (ProcessInUse.TaskIndex == 7 && ProcessInUse.Tasks[ProcessInUse.TaskIndex].Time == 0)
            {
                ProcessInUse.ProcessState = ProcessState.ZOMBIE;
                _zombieProcesses.Add(ProcessInUse);
                ProcessInUse = null;
                AgeProcesses();
                CSTime = MaxContextSwitchTime;
                return;
            }
            else if (ProcessInUse.Tasks[ProcessInUse.TaskIndex].Time == 0)
            {
                ProcessInUse.TaskIndex++;
                if (!ProcessInUse.Tasks[ProcessInUse.TaskIndex].Type)
                {
                    ProcessInUse.ProcessState = ProcessState.READY_TO_RUN_SWAPPED;
                    _finishedProcesses.Add(ProcessInUse);
                    AgeProcesses();
                    ProcessInUse = null;
                    CSTime = MaxContextSwitchTime;
                }
                else
                {
                    AgeProcesses();
                    ProcessInUse.ProcessState = ProcessState.READY_TO_RUN_IN_MEMORY;
                    ProcessInUse.Priority = (0 >= ProcessInUse.Priority ? 0 : ProcessInUse.Priority - 1);
                    AddProcess(ProcessInUse);
                    ProcessInUse = null;
                    CSTime = MaxContextSwitchTime;
                }
                return;
            }

            ProcessInUse.Priority = (0 >= ProcessInUse.Priority ? 0 : ProcessInUse.Priority - 1);
            AgeProcesses();
            ProcessInUse.ProcessState = ProcessState.READY_TO_RUN_IN_MEMORY;
            AddProcess(ProcessInUse);
            ProcessInUse = null;
            CSTime = MaxContextSwitchTime;
        }

        /// <summary>
        /// Inserts process based on priority level
        /// </summary>
        /// <param name="process"></param>
        public void AddProcess(Process process)
        {
            if (ProcessCount == 0)
            {
                process.ProcessState = ProcessState.READY_TO_RUN_IN_MEMORY;
                _processes.Add(process);
                return;
            }

            foreach (var process1 in _processes.Where(process1 => process.Priority > process1.Priority))
            {
                _processes.Insert(_processes.FindIndex(x => x.PID == process1.PID), process);
                break;
            }
        }

        /// <summary>
        /// Ages all processes, those above a certain threshold will be temporarily removed fro list, bumped in priority, and re-added to list so new priority can take affect
        /// </summary>
        private void AgeProcesses()
        {
            foreach (var process in _processes)
            {
                process.Age++;
            }

            var agedProcesses = _processes.Where(x => x.Age > 10).ToList();

            _processes.RemoveAll(x => x.Age > 10);

            foreach (var agedProcess in agedProcesses)
            {
                agedProcess.Priority = (9 <= agedProcess.Priority ? 9 : agedProcess.Priority + 1);
                AddProcess(agedProcess);
            }
        }

        public Process GetZombieProcess()
        {
            if (_zombieProcesses.Count == 0)
                return null;

            var z = _zombieProcesses.ElementAt(0);
            _zombieProcesses.RemoveAt(0);
            return z;
        }

        public Process GetFinishedProcess()
        {
            if (_finishedProcesses.Count == 0)
                return null;

            var f = _finishedProcesses[0];
            _finishedProcesses.RemoveAt(0);
            return f;
        }
    }
}
