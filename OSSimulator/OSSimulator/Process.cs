using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSSimulator
{
    public class Process
    {
        public int PID { get; set; }
        public int ArrivalTime { get; set; }
        public List<Task> Tasks { get; set; } 
        public int TaskIndex { get; set; }
        public int Priority { get; set; }
        public ProcessState ProcessState { get; set; }
        public int Age { get; set; }
    }
}
