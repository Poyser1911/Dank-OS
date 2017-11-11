using System;
using System.Diagnostics;

namespace Dank_OS
{
    public class Process
    {
        public int ProcessID { get; set; }
        private Stopwatch Timer { get; } = new Stopwatch();
        public DateTime StartTime { get; }
        public DateTime EndTime { get; private set; }
        public string CpuTime => $"{ Timer.Elapsed.TotalDays:0}:{ Timer.Elapsed.Hours}:{ Timer.Elapsed.Minutes}:{ Timer.Elapsed.Seconds}";

        public Process(int pid)
        {
            ProcessID = pid;
            StartTime = DateTime.Now;
            Timer.Start();
        }
        public void Stop()
        {
            EndTime = DateTime.Now;
            Timer.Stop();
        }
    }
}
