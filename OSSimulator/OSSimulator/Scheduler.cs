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
        private readonly IODevice _ioDevice = new IODevice();
        private readonly ProcessPool _processPool;
        private readonly int _numProcesses = 5000;
        private readonly int _config;
        private long _masterClock = 0;

        /// <summary>
        /// Initializes the number of requested cores, 1 by default
        /// Uses config value to set the algorithm the core will use, 1 = FCFS, 2 = RR Long, 3 = RR Short
        /// </summary>
        /// <param name="processPool"></param>
        /// <param name="numCores"></param>
        /// <param name="config"></param>
        public Scheduler(ProcessPool processPool, int numCores = 1, int config = 1)
        {
            _processPool = processPool;
            PrepareProcesses();

            _config = config;

            for (var i = 0; i < numCores; ++i)
            {
                _cores.Add(new Core(config));
            }
        }

        /// <summary>
        /// Begins simulation, based on config value in constructor, it will run the appropriate algorithm
        /// </summary>
        public void Run()
        {
            _masterClock = _readyProcesses[0].ArrivalTime + 25;
            switch (_config)
            {
                case 1:
                    Console.WriteLine("Starting FCFS on {0} CPUs", _cores.Count);
                    RunConfig1();
                    break;
                case 2:
                    Console.WriteLine("Starting RR Long on {0} CPUs", _cores.Count);
                    RunConfig2();
                    break;
                case 3:
                    Console.WriteLine("Starting RR Short on {0} CPUs", _cores.Count);
                    RunConfig3();
                    break;
            }
        }

        public string Analysis()
        {
            var analysisString = "";

            analysisString += DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + ",";
            analysisString += _completedProcesses.Average(x => x.ResponseTime) + ",";
            analysisString += _completedProcesses.Average(x => x.WaitTime) + ",";
            analysisString += _completedProcesses.Average(x => x.TurnaroundTime) + ",";

            return analysisString;
        }

        /// <summary>
        /// Simulates First Come First Server Algorithm
        /// </summary>
        private void RunConfig1()
        {
            while (_completedProcesses.Count < _numProcesses)
            {
                GetNextProcesses();

                foreach (var core in _cores)
                {
                    if (core.FCFSProcesses.Count == 0)
                        continue;
                    GetNextProcesses();
                    var fcfsProcess = core.FCFS();

                    if (fcfsProcess.TaskIndex == (fcfsProcess.Tasks.Count - 1) &&
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
                    _masterClock += core.EventTime;

                    fcfsProcess = _ioDevice.FCFS();
                    if (fcfsProcess != null)
                    {
                        if (fcfsProcess.TaskIndex == (fcfsProcess.Tasks.Count - 1) &&
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

        /// <summary>
        /// Simulates Round-Robin with Long Time Quantum
        /// </summary>
        private void RunConfig2()
        {
            while (_completedProcesses.Count < _numProcesses)
            {

                GetNextProcesses();

                foreach (var core in _cores)
                {
                    if (core.RRLongProcesses.Count == 0)
                        continue;
                    var rrLong = core.RRLong();

                    if (rrLong != null)
                    {
                        if (rrLong.TaskIndex < rrLong.Tasks.Count && rrLong.Tasks[rrLong.TaskIndex].Time != 0)
                        {
                            _ioDevice.FCFSProcesses.Add(rrLong);
                        }
                        else
                        {
                            _completedProcesses.Add(rrLong);
                        }
                    }
                    _masterClock += core.EventTime;

                    rrLong = _ioDevice.FCFS();
                    if (rrLong != null)
                    {
                        if (rrLong.TaskIndex == (rrLong.Tasks.Count - 1) &&
                        rrLong.Tasks[rrLong.TaskIndex].Time == 0)
                            _completedProcesses.Add(rrLong);
                        else if (!rrLong.Tasks[++rrLong.TaskIndex].Type)
                        {
                            _ioDevice.FCFSProcesses.Insert(0, rrLong);
                        }
                        else
                        {
                            core.RRLongProcesses.Add(rrLong);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Simulates Round-Robin with Short Time Quantum
        /// </summary>
        private void RunConfig3()
        {
            while (_completedProcesses.Count < _numProcesses)
            {
                GetNextProcesses();

                foreach (var core in _cores)
                {
                    if (core.RRShortProcesses.Count == 0)
                        continue;
                    var rrShort = core.RRShort();

                    if (rrShort != null)
                    {
                        if (rrShort.TaskIndex < rrShort.Tasks.Count && rrShort.Tasks[rrShort.TaskIndex].Time != 0)
                        {
                            _ioDevice.FCFSProcesses.Add(rrShort);
                        }
                        else
                        {
                            _completedProcesses.Add(rrShort);
                        }
                    }
                    _masterClock += core.EventTime;
                    GetNextProcesses();

                    rrShort = _ioDevice.FCFS();
                    if (rrShort != null)
                    {
                        if (rrShort.TaskIndex == (rrShort.Tasks.Count - 1) &&
                        rrShort.Tasks[rrShort.TaskIndex].Time == 0)
                            _completedProcesses.Add(rrShort);
                        else if (!rrShort.Tasks[++rrShort.TaskIndex].Type)
                        {
                            _ioDevice.FCFSProcesses.Insert(0, rrShort);
                        }
                        else
                        {
                            core.RRShortProcesses.Add(rrShort);
                        }
                    }
                } 
            }
        }

        /// <summary>
        /// Gets the processes that arrived during event and adds it to list appropriate for the algorithm
        /// </summary>
        private void GetNextProcesses()
        {
            if (_readyProcesses.Count == 0)
                return;
            
            var arrivedProcesses =
                _readyProcesses.Where(x => x.ArrivalTime <= _masterClock).OrderBy(p => p.ArrivalTime).ToList();

            for (int j = 0, i = 0; i < arrivedProcesses.Count; j = (j + 1) % _cores.Count)
            {
                switch (_cores[j].Config)
                {
                    case 1:
                        _cores[j].FCFSProcesses.Add(arrivedProcesses[i++]);
                        break;
                    case 2:
                        _cores[j].RRLongProcesses.Add(arrivedProcesses[i++]);
                        break;
                    case 3:
                        _cores[j].RRShortProcesses.Add(arrivedProcesses[i++]);
                        break;
                }
            }

            foreach (var process in arrivedProcesses)
            {
                _readyProcesses.RemoveAt(_readyProcesses.FindIndex(x => x.PID == process.PID));
            }
        }

        /// <summary>
        /// Loads Processes into memory (simulate) and sorts them by arrival time
        /// </summary>
        private void PrepareProcesses()
        {
            Console.WriteLine("Loading random processes into memory...");

            // For debugging
            //_readyProcesses.AddRange(_processPool.TestRun().OrderBy(x => x.ArrivalTime));

            while (_readyProcesses.Count < _numProcesses)
            {

                var createdProcess = _processPool.GetRandomProcess();

                if (_readyProcesses.Count == 0)
                {
                    _readyProcesses.Add(createdProcess);
                }

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
