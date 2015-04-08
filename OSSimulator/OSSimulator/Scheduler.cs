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
            PrepareProcesses();
        }

        /// <summary>
        /// Begins pulling in random processes and placing it on CPU, switches processes between CPU and IO
        /// </summary>
        public void Run()
        {
            while (true)
            {
                // f = a process that has finished its burst, z = a process that is now in zombie state
                List<Process> f = null, z = null;
                GetNextProcesses();
                if (_core.ProcessCount == 0)
                {
                    _masterClock += _core.MaxTimeQuantum;
                }
                else
                {


                    _core.ExecuteProcess();
                    _masterClock += _core.CurrentTimeQuantum;
                    GetNextProcesses();
                    _core.ContextSwitch();
                    _masterClock += _core.CSTime;
                    GetNextProcesses();

                    f = _core.GetFinishedProcess();
                    z = _core.GetZombieProcesses();

                    Console.WriteLine("{0} processes are now in Zombie state", z.Count);
                    Console.WriteLine("{0} processes finished CPU bursts", f.Count);
                    foreach (var process in z)
                    {
                        _processPool.ReturnProcess(process);
                    }

                    foreach (var process in f)
                    {
                        _ioDevice.AddProcess(process);
                    }
                }
                if (_ioDevice.ProcessCount == 0)
                {
                    _masterClock += _core.MaxTimeQuantum;
                }
                else
                {
                    _ioDevice.ExecuteProcess();
                    _masterClock += _ioDevice.CurrentTimeQuantum;
                    _ioDevice.ContextSwitch();
                    _masterClock += _ioDevice.CSTime;
                    f = _ioDevice.GetFinishedProcess();
                    z = _ioDevice.GetZombieProcesses();

                    Console.WriteLine("{0} processes are now in Zombie state", z.Count);
                    Console.WriteLine("{0} processes finished IO bursts", f.Count);
                    foreach (var process in z)
                    {
                        _processPool.ReturnProcess(process);
                    }

                    foreach (var process in f)
                    {
                        _ioDevice.AddProcess(process);
                    }
                }
            }
        }

        private void GetNextProcesses()
        {
            if (_readyProcesses.Count == 0)
                return;

            var processes = _readyProcesses.Where(x => x.ArrivalTime < _masterClock).ToList();
            foreach (var p in processes)
            {
                _readyProcesses.Remove(p);
                _core.AddProcess(p);

            }

        }

        private void PrepareProcesses()
        {
            Console.WriteLine("Loading random processes into memory...");
            while (_readyProcesses.Count < 50)
            {
                //if (_readyProcesses.Count > 5)
                //    return;

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
}
