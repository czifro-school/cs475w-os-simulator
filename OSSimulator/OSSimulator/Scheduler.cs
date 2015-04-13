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
        private readonly List<Process> _completedProcesses = new List<Process>(); 
        private readonly List<Core> _cores = new List<Core>();
        private readonly Core _core = new Core();
        private readonly IODevice _ioDevice = new IODevice();
        private readonly ProcessPool _processPool;
        private readonly int _numProcesses = 50;
        private long _masterClock = 0;

        public Scheduler(ProcessPool processPool, int numCores = 1)
        {
            _processPool = processPool;
            PrepareProcesses();
        }

        /// <summary>
        /// Begins pulling in random processes and placing it on CPU, switches processes between CPU and IO
        /// </summary>
        public void Run()
        {
            _masterClock = _readyProcesses[0].ArrivalTime;
            while (true)
            {
                GetNextProcesses();
                var fcfsProccess = _core.FCFS();
                var rrLong = _core.RRLong();
                var rrShort = _core.RRShort();

                if (fcfsProccess.Tasks[fcfsProccess.TaskIndex++].Type)
                {
                    _core.FCFSProcesses.Insert(0, fcfsProccess);
                }
                else
                {
                    _ioDevice.FCFSProcesses.Add(fcfsProccess);
                }

                if (rrLong != null)
                {
                    if (rrLong.TaskIndex != rrLong.Tasks.Count || rrLong.Tasks[rrLong.TaskIndex].Time != 0)
                    {
                        _ioDevice.FCFSProcesses.Add(rrLong);
                    }
                }

                if (rrShort != null)
                {
                    if (rrShort.TaskIndex != rrShort.Tasks.Count || rrShort.Tasks[rrShort.TaskIndex].Time != 0)
                    {
                        _ioDevice.FCFSProcesses.Add(rrShort);
                    }
                }
            }
        }

        public void RunConfig1()
        {
            while (_completedProcesses.Count < _numProcesses)
            {
                GetNextProcesses();

                foreach (var core in _cores)
                {
                    GetNextProcesses();
                    var fcfsProcess = core.FCFS();

                    if (fcfsProcess.TaskIndex == fcfsProcess.Tasks.Count &&
                    fcfsProcess.Tasks[fcfsProcess.TaskIndex].Time == 0)
                        _completedProcesses.Add(fcfsProcess);
                    else if (fcfsProcess.Tasks[fcfsProcess.TaskIndex++].Type)
                    {
                        core.FCFSProcesses.Insert(0, fcfsProcess);
                    }
                    else
                    {
                        _ioDevice.FCFSProcesses.Add(fcfsProcess);
                    }

                    fcfsProcess = _ioDevice.FCFS();
                    if (fcfsProcess != null)
                    {
                        if (fcfsProcess.TaskIndex == fcfsProcess.Tasks.Count &&
                    fcfsProcess.Tasks[fcfsProcess.TaskIndex].Time == 0)
                            _completedProcesses.Add(fcfsProcess);
                        else if (!fcfsProcess.Tasks[fcfsProcess.TaskIndex++].Type)
                        {
                            _ioDevice.FCFSProcesses.Insert(0, fcfsProcess);
                        }
                        else
                        {
                            core.FCFSProcesses.Add(fcfsProcess);
                        }
                    }
                }
            }
        }

        public void RunConfig2()
        {
            while (_completedProcesses.Count < _numProcesses)
            {
                GetNextProcesses();

                foreach (var core in _cores)
                {
                    var rrLong = core.RRLong();

                    if (rrLong != null)
                    {
                        if (rrLong.TaskIndex != rrLong.Tasks.Count || rrLong.Tasks[rrLong.TaskIndex].Time != 0)
                        {
                            _ioDevice.FCFSProcesses.Add(rrLong);
                        }
                    }

                    rrLong = _ioDevice.FCFS();
                    if (rrLong != null)
                    {
                        if (rrLong.TaskIndex == rrLong.Tasks.Count &&
                        rrLong.Tasks[rrLong.TaskIndex].Time == 0)
                            _completedProcesses.Add(rrLong);
                        else if (!rrLong.Tasks[rrLong.TaskIndex++].Type)
                        {
                            _ioDevice.FCFSProcesses.Insert(0, rrLong);
                        }
                        else
                        {
                            core.FCFSProcesses.Add(rrLong);
                        }
                    }
                }
            }
        }

        public void RunConfig3()
        {
            while (_completedProcesses.Count < _numProcesses)
            {
                GetNextProcesses();

                foreach (var core in _cores)
                {
                    var rrShort = core.RRShort();

                    if (rrShort != null)
                    {
                        if (rrShort.TaskIndex != rrShort.Tasks.Count || rrShort.Tasks[rrShort.TaskIndex].Time != 0)
                        {
                            _ioDevice.FCFSProcesses.Add(rrShort);
                        }
                    }
                    _masterClock += core.EventTime;
                    GetNextProcesses();

                    rrShort = _ioDevice.FCFS();
                    if (rrShort != null)
                    {
                        if (rrShort.TaskIndex == rrShort.Tasks.Count &&
                        rrShort.Tasks[rrShort.TaskIndex].Time == 0)
                            _completedProcesses.Add(rrShort);
                        else if (!rrShort.Tasks[rrShort.TaskIndex++].Type)
                        {
                            _ioDevice.FCFSProcesses.Insert(0, rrShort);
                        }
                        else
                        {
                            core.FCFSProcesses.Add(rrShort);
                        }
                    }
                } 
            }
        }

        private void GetNextProcesses()
        {
            if (_readyProcesses.Count == 0)
                return;

            var arrivedProcesses =
                _readyProcesses.Where(x => x.ArrivalTime <= _masterClock).OrderBy(p => p.ArrivalTime).ToList();

            foreach (var core in _cores)
            {
                switch (core.Config)
                {
                    case 1:

                        break;
                    case 2:

                        break;

                    case 3:

                        break;
                }
            }
        }

        private void PrepareProcesses()
        {
            Console.WriteLine("Loading random processes into memory...");
            while (_readyProcesses.Count < _numProcesses)
            {

                var createdProcess = _processPool.GetRandomProcess();

                if (_readyProcesses.Count == 0)
                {
                    _readyProcesses.Add(createdProcess);
                    //return;
                }

                //var readyProcess = _readyProcesses.SingleOrDefault(r => r.ArrivalTime > createdProcess.ArrivalTime);

                Process readyProcess = null;
                foreach (var r in _readyProcesses)
                {
                    if (r.ArrivalTime > createdProcess.ArrivalTime)
                    {
                        readyProcess = r;
                        break;
                    }
                }

                if (readyProcess != null)
                    _readyProcesses.Insert(_readyProcesses.FindIndex(x => x.PID == readyProcess.PID), createdProcess);

                else
                    _readyProcesses.Add(createdProcess);

                
            }
        }
    }
}
