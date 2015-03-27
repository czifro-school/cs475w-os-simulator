using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSSimulator
{
    public class Scheduler
    {
        private readonly List<Process> _readyProcesses = new List<Process>(); 
        private readonly Core _core = new Core();
        private readonly IODevice _ioDevice = new IODevice();
        private readonly ProcessPool _processPool;
        private int _masterClock = 0;

        public Scheduler(ProcessPool processPool)
        {
            _processPool = processPool;
        }

        /// <summary>
        /// Begins pulling in random processes and placing it on CPU, switches processes between CPU and IO
        /// </summary>
        public void Run()
        {
            while (true)
            {
                // f = a process that has finished its burst, z = a process that is now in zombie state
                Process f = null, z = null;
                if (_core.ProcessCount > 0)
                {
                    if (!_core.CPUBusy)
                    {
                        if (!_core.CPUContextSwitching)
                        {
                            if (_core.CanExecute)
                                _core.ExecuteProcess();
                            else if (_core.CanCS)
                                _core.ContextSwitch();

                            f = _core.GetFinishedProcess();
                            if (f != null)
                            {
                                f.Age = 0;
                                _ioDevice.AddProcess(f);
                            }
                            z = _core.GetZombieProcess();
                            if (z != null)
                            {
                                _processPool.ReturnProcess(z.PID);
                            }
                        }
                        else
                            _core.CSTime--;
                    }
                    else
                        _core.CurrentTimeQuantum--;
                }
                if (_ioDevice.ProcessCount > 0)
                {
                    if (!_ioDevice.CPUBusy)
                    {
                        if (!_ioDevice.CPUContextSwitching)
                        {
                            if (_ioDevice.CanExecute)
                                _ioDevice.ExecuteProcess();
                            else if (_ioDevice.CanCS)
                                _ioDevice.ContextSwitch();

                            f = _ioDevice.GetFinishedProcess();
                            if (f != null)
                            {
                                f.Age = 0;
                                _core.AddProcess(f);
                            }
                            z = _ioDevice.GetZombieProcess();
                            if (z != null)
                            {
                                _processPool.ReturnProcess(z.PID);
                            }
                        }
                        else
                            _ioDevice.CSTime--;
                    }
                    else
                        _ioDevice.CurrentTimeQuantum--;
                }

                var p = GetNextProcess();
                if (p != null)
                {
                    _core.AddProcess(p);
                }
                PrepareProcess();
                ++_masterClock;
            }
        }

        private Process GetNextProcess()
        {
            if (_readyProcesses.Count == 0)
                return null;

            var p = _readyProcesses[0];
            _readyProcesses.RemoveAt(0);

            p.ProcessState = ProcessState.READY_TO_RUN_IN_MEMORY;

            return p;
        }

        private void PrepareProcess()
        {
            if (_readyProcesses.Count > 5)
                return;

            var createdProcess = _processPool.GetRandomProcess();

            createdProcess.ProcessState = ProcessState.READY_TO_RUN_SWAPPED;

            if (_readyProcesses.Count == 0)
            {
                _readyProcesses.Add(createdProcess);
                return;
            }

            foreach (var readyProcess in _readyProcesses.Where(readyProcess => readyProcess.ArrivalTime > createdProcess.ArrivalTime))
            {
                _readyProcesses.Insert(_readyProcesses.FindIndex(x => x.PID == readyProcess.PID), createdProcess);
                break;
            }
        }
    }
}
