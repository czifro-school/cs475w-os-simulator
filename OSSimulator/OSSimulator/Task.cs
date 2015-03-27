using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSSimulator
{
    public class Task
    {
        public int Time { get; set; }
        /// <summary>
        /// true = CPU, false = IO
        /// </summary>
        public bool Type { get; set; }
    }
}
