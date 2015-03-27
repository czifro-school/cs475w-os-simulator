using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSSimulator.IO
{
    public class FileReader
    {
        private readonly string _path = "";
        private const string Filename = "processes.csv";

        public FileReader()
        {
            _path = Path.GetFullPath("..\\..\\" + Filename);
        }

        public bool FileExistsAndHasContents
        {
            get { return File.Exists(_path) && new FileInfo(_path).Length > 0; }
        }

        public List<RawProcess> ReadContents()
        {
            var rawProcesses = new List<RawProcess>();
            rawProcesses =
                File.ReadAllLines(_path).Select(x => new RawProcess {IsBusy = false, ProcessCSV = x}).ToList();

            return rawProcesses;
        }
    }
}
