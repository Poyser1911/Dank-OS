using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dank_OS
{
    public class Process
    {
        public int ProcessID { get; set; }
        private Stopwatch _timer { get; set; } = new Stopwatch();
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public string CPUTime
        {
            get
            {
                return $"{ _timer.Elapsed.Hours}:{ _timer.Elapsed.Minutes}:{ _timer.Elapsed.Seconds}:{ _timer.Elapsed.Milliseconds}";
            }
        }

        public Process(int pid)
        {
            ProcessID = pid;
            StartTime = DateTime.Now;
            _timer.Start();
        }
        public void Stop()
        {
            EndTime = DateTime.Now;
            _timer.Stop();
        }
    }
}
