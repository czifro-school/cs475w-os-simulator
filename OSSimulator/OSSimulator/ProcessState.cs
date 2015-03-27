using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSSimulator
{
    public enum ProcessState
    {
        CREATED,
        READY_TO_RUN_SWAPPED,
        READY_TO_RUN_IN_MEMORY,
        USER_RUNNING,
        PREEMPTED,
        ZOMBIE
    }
}
