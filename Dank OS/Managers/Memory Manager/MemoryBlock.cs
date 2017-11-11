using System.Collections.Generic;
using System.Windows.Media;

namespace Dank_OS
{
    public class MemAppData
    {
        public int ProcessId { get; set; }

        public string Name { get; set; }
        public double MemorySize { get; set; }
        public double BlockPercentageUseage { get; set; }

        public SolidColorBrush BlockColour { get; set; }
        public double TotalPercentageUseage { get; set; }

        public Process AppProcess { get; set; }

    }

    public class MemoryBlock
    {
        public string Name { get; set; }
        public double AvaliableMemory { get; set; }
        private double _totalmemory;
        public double TotalMemory
        {
            get
            {
                return _totalmemory;
            }
            set
            {
                AvaliableMemory = value;
                _totalmemory = value;
            }
        }
        public List<MemAppData> Apps { get; set; } = new List<MemAppData>();

        public void Alloc(MemAppData app)
        {
            app.BlockPercentageUseage = (app.MemorySize * 100) / _totalmemory;
            app.TotalPercentageUseage = (app.MemorySize * 100) / MemoryManager.Totalmemory;
            Apps.Add(app);
            AvaliableMemory -= app.MemorySize;
        }
        public void DeAlloc(int processId)
        {
            for (int i = 0; i < Apps.Count; i++)
                if (Apps[i].ProcessId == processId)
                {
                    AvaliableMemory += Apps[i].MemorySize;
                    Apps.RemoveAt(i);
                }
        }
    }
}
